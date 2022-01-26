Imports System.Drawing
Imports MazePicSolver.Extensions

Namespace Map

    Class Map
        Private Property MapDots As MapDot()()
        Private Property Solved As Boolean

        Sub New(imagePath As String)
            MapDots = Helpers.LoadMapDotsFromImage(imagePath)
        End Sub

        Public ReadOnly Property Size As Size
            Get
                Return New Size(MapDots.Length, MapDots.First().Length)
            End Get
        End Property

        Public Function GetDot(point As Point) As MapDot
            Return GetDot(point.X, point.Y)
        End Function

        Public Function GetDot(x As Integer, y As Integer) As MapDot
            Return MapDots(x)(y)
        End Function

        Public Sub SaveSolution(savePath As String, showWorking As Boolean)
            Solve()
            Helpers.SaveImage(MapDots, Size, savePath, showWorking)
        End Sub

        Public Sub Solve()
            If Solved Then
                Return
            End If
            Dim positions = GetStartAndEndPoint()
            Dim currentLoc = positions.startPos

            While (Solved = False)
                Dim dot = GetDot(currentLoc)
                dot.PathUsed = True
                Dim allOptions = MapDots.GetAroundArrayOfArrays(currentLoc)
                Dim forward = allOptions.Where(Function(e) GetDot(e).Wall = False And GetDot(e).PathUsed = False And GetDot(e).DeadEnd = False).ToArray()
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

                Dim nextDot = GetDot(forward.First())
                nextDot.PreviousLocation = dot.Location
                If nextDot.EndPoint Then
                    Solved = True
                Else
                    currentLoc = nextDot.Location
                End If
            End While
        End Sub

        Public Function GetStartAndEndPoint() As (startPos As Point, endPos As Point)
            Dim start As Point = Nothing
            Dim ends As Point = Nothing

            MapDots.IterateThroughMap(Sub(x, y, map)
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
                                      End Sub)

            If start = Nothing Or ends = Nothing Then
                Throw New Exception("no start or end found")
            End If

            Return (start, ends)
        End Function

    End Class
End Namespace