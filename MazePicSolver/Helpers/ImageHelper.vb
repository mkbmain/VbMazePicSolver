Imports System.Diagnostics.CodeAnalysis
Imports System.Drawing
Imports System.Drawing.Imaging
Imports MazePicSolver.Extensions
Imports MazePicSolver.Map

Namespace Helpers

    Module ImageHelper

        <SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification:="<Pending>")>
        Public Function LoadMapDotsFromImage(imagePath As String) As MapDot()()
            Dim dots As MapDot()()
            Using image = New Bitmap(imagePath)
                dots = Enumerable.Range(0, image.Size.Width).Select(Function(t) New MapDot(image.Size.Height - 1) {}).ToArray()
                ' this style looks a lot nicer in c# and I know the image will box of the heap but minor slow down and small price to pay for dryness
                Dim func As Action(Of Integer, Integer, MapDot()()) = Sub(x, y, map)
                                                                          If image.GetPixel(x, y).R = 0 And image.GetPixel(x, y).G = 0 And image.GetPixel(x, y).B = 0 Then
                                                                              map(x)(y) = New MapDot(True, New Point(x, y))
                                                                          ElseIf image.GetPixel(x, y).G > 0 And image.GetPixel(x, y).R < 255 And image.GetPixel(x, y).G <= 255 Then
                                                                              map(x)(y) = New MapDot(False, New Point(x, y), True)
                                                                          ElseIf image.GetPixel(x, y).G < 255 And image.GetPixel(x, y).R > 0 And image.GetPixel(x, y).R <= 255 Then
                                                                              map(x)(y) = New MapDot(False, New Point(x, y), False, True)
                                                                          Else
                                                                              map(x)(y) = New MapDot(False, New Point(x, y))
                                                                          End If
                                                                      End Sub
                dots.IterateThroughMap(func)
            End Using
            Return dots
        End Function


        <SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification:="<Pending>")>
        Public Sub SaveImage(map As MapDot()(), size As Size, savePath As String)
            Using image As New Bitmap(size.Width, size.Height)
                Dim graphics As Graphics = Graphics.FromImage(image)
                Dim blackPen = New Pen(Brushes.Black)
                Dim whitePen = New Pen(Brushes.White)
                Dim redPen = New Pen(Brushes.Red)

                For x As Integer = 0 To size.Width - 1
                    For y As Integer = 0 To size.Height - 1
                        Dim mapDot As MapDot = map(x)(y)
                        Dim point = New Point(x, y)
                        Dim rectangle = New Rectangle(point, New Size(1, 1))

                        If mapDot.Wall Then
                            graphics.FillRectangle(Brushes.Black, rectangle)
                            graphics.DrawRectangle(blackPen, rectangle)
                        ElseIf mapDot.PathUsed Or mapDot.EndPoint Then
                            graphics.FillRectangle(Brushes.Red, rectangle)
                            graphics.DrawRectangle(redPen, rectangle)
                        Else
                            graphics.FillRectangle(Brushes.White, rectangle)
                            graphics.DrawRectangle(whitePen, rectangle)
                        End If
                    Next
                Next
                image.Save(savePath, ImageFormat.Png)
            End Using
        End Sub

    End Module
End Namespace