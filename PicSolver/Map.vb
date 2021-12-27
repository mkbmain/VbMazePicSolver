Imports System.Drawing

Class Map
    Private Property Dots As Dot()()
    Private Property Solved As Boolean
    Sub New(ByVal imagePath As String)
        LoadFromIamge(imagePath)
    End Sub

    Public ReadOnly Property Size As Size
        Get
            Return New Size(Dots.Length, Dots.First().Length)
        End Get
    End Property

    Public Function GetDot(ByVal point As Point) As Dot
        Return GetDot(point.X, point.Y)
    End Function
    Public Function GetDot(ByVal x As Integer, ByVal y As Integer) As Dot
        Return Dots(x)(y)
    End Function

    Private Sub LoadFromIamge(ByVal imagePath As String)
        Using image = New Bitmap(imagePath)
            Dots = Enumerable.Range(0, image.Size.Width).Select(Function(t) New Dot(image.Size.Height - 1) {}).ToArray()
            Dim func As Action(Of Integer, Integer, Dot()()) = Sub(x, y, map)
                                                                   If image.GetPixel(x, y).R = 0 And image.GetPixel(x, y).G = 0 And image.GetPixel(x, y).B = 0 Then
                                                                       map(x)(y) = New Dot(True, New Point(x, y))
                                                                   ElseIf image.GetPixel(x, y).G > 0 And image.GetPixel(x, y).R < 255 And image.GetPixel(x, y).G <= 255 Then
                                                                       map(x)(y) = New Dot(False, New Point(x, y), True)
                                                                   ElseIf image.GetPixel(x, y).G < 255 And image.GetPixel(x, y).R > 0 And image.GetPixel(x, y).R <= 255 Then
                                                                       map(x)(y) = New Dot(False, New Point(x, y), False, True)
                                                                   Else
                                                                       map(x)(y) = New Dot(False, New Point(x, y))
                                                                   End If
                                                               End Sub
            IterateThroughMap(func)
        End Using
    End Sub

    Public Sub SaveSoultion(ByVal savePath As String)
        If Solved = False Then
            Solve()
        End If

        Using image As New Bitmap(Size.Width, Size.Height)
            Dim graphics As Graphics = System.Drawing.Graphics.FromImage(image)
            Dim blackPen As Pen = New Pen(Brushes.Black)
            Dim whitePen As Pen = New Pen(Brushes.White)
            Dim redPen As Pen = New Pen(Brushes.Red)

            For x As Integer = 0 To Me.Size.Width - 1
                For y As Integer = 0 To Me.Size.Height - 1
                    Dim dot As Dot = Me.GetDot(x, y)
                    Dim point As Point = New Point(x, y)
                    Dim rectangle = New Rectangle(point, New Size(1, 1))

                    If dot.Wall Then
                        graphics.FillRectangle(Brushes.Black, rectangle)
                        graphics.DrawRectangle(blackPen, rectangle)
                    ElseIf dot.PathUsed Or dot.EndPoint Then
                        graphics.FillRectangle(Brushes.Red, rectangle)
                        graphics.DrawRectangle(redPen, rectangle)
                    Else
                        graphics.FillRectangle(Brushes.White, rectangle)
                        graphics.DrawRectangle(whitePen, rectangle)
                    End If
                Next
            Next
            image.Save(savePath, System.Drawing.Imaging.ImageFormat.Png)
        End Using
    End Sub

    Public Sub Solve()
        If Solved Then
            Return
        End If
        Dim positions = Me.GetStartAndEndPoint()
        Dim currentLoc = positions.startPos
        Dim endReached = False
        While (endReached = False)
            Dim dot = Me.GetDot(currentLoc)
            dot.PathUsed = True
            Dim allOptions = Me.GetAroundDot(currentLoc)
            Dim forward = allOptions.Where(Function(e) Me.GetDot(e).Wall = False And Me.GetDot(e).PathUsed = False And Me.GetDot(e).DeadEnd = False).ToArray()
            If (forward.Any() = False) Then
                ' backTrackTime
                If dot.StartPoint Then
                    ' we are screwed
                    Throw New Exception("Ok we are F%!^&d")
                End If
                currentLoc = dot.PreviousLocation
                dot.PathUsed = False
                dot.DeadEnd = True
                Continue While
            End If

            Dim nextDot = Me.GetDot(forward.First())
            nextDot.PreviousLocation = dot.Location
            If nextDot.EndPoint Then
                endReached = True
                Solved = True
            Else

                currentLoc = nextDot.Location
            End If
        End While
    End Sub

    Public Function GetAroundDot(ByRef pos As Point) As List(Of Point)

        Return New List(Of Point)() From {New Point(pos.X - 1, pos.Y), New Point(pos.X + 1, pos.Y), New Point(pos.X, pos.Y - 1), New Point(pos.X, pos.Y + 1)} _
          .Where(Function(e) e.X < Dots.Length And e.X > -1) _
            .Where(Function(e) e.Y > -1 And e.Y < Dots.First().Length).ToList()
    End Function

    Public Function GetStartAndEndPoint() As (startPos As Point, endPos As Point)
        Dim start As Point = Nothing
        Dim ends As Point = Nothing
        Dim func As Action(Of Integer, Integer, Dot()()) = Sub(x, y, map)
                                                               If map(x)(y).StartPoint Then
                                                                   If start <> Nothing Then
                                                                       Throw New Exception("Can't have multiple starts")
                                                                   End If
                                                                   start = New Point(x, y)
                                                               End If
                                                               If map(x)(y).EndPoint Then
                                                                   If ends <> Nothing Then
                                                                       Throw New Exception("Can't have multiple ends")
                                                                   End If
                                                                   ends = New Point(x, y)
                                                               End If
                                                           End Sub

        IterateThroughMap(func)

        If start = Nothing Or ends = Nothing Then
            Throw New Exception("no start or end found")
        End If

        Return (start, ends)
    End Function

    Public Sub IterateThroughMap(ByRef func As Action(Of Integer, Integer, Dot()()))
        For x As Integer = 0 To Dots.Length - 1
            For y As Integer = 0 To Dots.First().Length - 1
                func(x, y, Dots)
            Next
        Next
    End Sub
End Class