Imports System
Imports System.Drawing


Module Program
    Sub Main(args As String())
#If DEBUG Then
        args = {"D:\ourPic.png"}
#End If
        If args.Length < 1 Then
            Console.WriteLine("Picture needs to be a argument")
        End If
        Dim map = GetMap(String.Join(" ", args))
        Dim positions = GetStartAndEndPoint(map)

        Dim currentLoc = positions.startPos
        Dim endReached = False
        While (endReached = False)
            Dim dot = map(currentLoc.X)(currentLoc.Y)
            dot.PathUsed = True
            Dim allOptions = GetAroundDot(map, currentLoc)
            Dim forward = allOptions.Where(Function(e) map(e.X)(e.Y).Wall = False And map(e.X)(e.Y).PathUsed = False And map(e.X)(e.Y).DeadEnd = False).ToArray()
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

            Dim nextDot = map(forward.First().X)(forward.First().Y)
            nextDot.PreviousLocation = dot.Location
            If nextDot.EndPoint Then
                endReached = True
            Else

                currentLoc = nextDot.Location
            End If
        End While

        For x As Integer = 0 To map.Length - 1
            For y As Integer = 0 To map.First().Length - 1
                Dim dot As Dot = map(x)(y)
                Console.SetCursorPosition(x + 1, y + 1)

                If dot.Wall Then
                    Console.BackgroundColor = ConsoleColor.White
                ElseIf dot.PathUsed Or dot.EndPoint Then
                    Console.BackgroundColor = ConsoleColor.Red
                Else
                    Console.BackgroundColor = ConsoleColor.Black
                End If
                Console.Write(" ")
            Next
        Next


    End Sub




    Public Function GetAroundDot(ByRef map As Dot()(), ByRef pos As Point) As List(Of Point)
        Dim maxX = map.Length
        Dim maxY = map.First().Length
        Dim moves = New List(Of Point)() From {New Point(pos.X - 1, pos.Y), New Point(pos.X + 1, pos.Y), New Point(pos.X, pos.Y - 1), New Point(pos.X, pos.Y + 1)}

        Return moves _
        .Where(Function(e) e.X < maxX And e.X > -1) _
            .Where(Function(e) e.Y > -1 And e.Y < maxY).ToList()
    End Function

    Public Function GetStartAndEndPoint(ByRef map As Dot()()) As (startPos As Point, endPos As Point)
        Dim start As Point = Nothing
        Dim ends As Point = Nothing
        Dim func As Action(Of Integer, Integer, Dot()()) = Function(x, y, miniMap)
                                                               If miniMap(x)(y).StartPoint Then
                                                                   If start <> Nothing Then
                                                                       Throw New Exception("Can't have multiple starts")
                                                                   End If
                                                                   start = New Point(x, y)
                                                               End If
                                                               If miniMap(x)(y).EndPoint Then
                                                                   If ends <> Nothing Then
                                                                       Throw New Exception("Can't have multiple ends")
                                                                   End If
                                                                   ends = New Point(x, y)
                                                               End If
                                                           End Function

        IterateThroughMap(map, func)

        If start = Nothing Or ends = Nothing Then
            Throw New Exception("no start or end found")
        End If

        Return (start, ends)
    End Function

    Public Function GetMap(ByVal path As String) As Dot()()
        Using image = New Bitmap(path)
            Dim minimap2 = Enumerable.Range(0, image.Size.Width).Select(Function(t) New Dot(image.Size.Height - 1) {}).ToArray()
            Dim func As Action(Of Integer, Integer, Dot()()) = Function(x, y, minimap)
                                                                   If image.GetPixel(x, y).R = 0 And image.GetPixel(x, y).G = 0 And image.GetPixel(x, y).B = 0 Then
                                                                       minimap(x)(y) = New Dot(True, New Point(x, y))
                                                                   ElseIf image.GetPixel(x, y).G > 0 And image.GetPixel(x, y).R < 255 And image.GetPixel(x, y).G <= 255 Then
                                                                       minimap(x)(y) = New Dot(False, New Point(x, y), True)
                                                                   ElseIf image.GetPixel(x, y).G < 255 And image.GetPixel(x, y).R > 0 And image.GetPixel(x, y).R <= 255 Then
                                                                       minimap(x)(y) = New Dot(False, New Point(x, y), False, True)
                                                                   Else
                                                                       minimap(x)(y) = New Dot(False, New Point(x, y))
                                                                   End If
                                                               End Function
            IterateThroughMap(minimap2, func)

            Return minimap2
        End Using
    End Function



    Public Sub IterateThroughMap(ByRef map As Dot()(), ByRef func As Action(Of Integer, Integer, Dot()()))
        For x As Integer = 0 To map.Length - 1
            For y As Integer = 0 To map.First().Length - 1
                func(x, y, map)
            Next
        Next
    End Sub
End Module
