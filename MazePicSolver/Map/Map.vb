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

        Public Sub BruteForceSaveSolution(savePath As String, showWorking As Boolean)
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
                Dim forward = allOptions.Where(Function(e) GetDot(e).Wall = False AndAlso GetDot(e).PathUsed = False AndAlso GetDot(e).DeadEnd = False).ToArray()
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

        Public Sub MapDirectRouteSolveSaveSolution(savePath As String)
            MapDots.IterateThroughMap(Sub(x, y, mapDots) mapDots(x)(y).Reset())
            Dim positions = GetStartAndEndPoint()
            Dim endDot As MapDot = MapDots(positions.endPos.X)(positions.endPos.Y)
            Dim currentOptions = New List(Of Point) From {positions.startPos}
            Dim steps As UInteger
            While endDot.ShortestFromStart = 0
                steps = steps + 1
                Dim options = currentOptions.SelectMany(Function(e) MapDots.GetAroundArrayOfArrays(e).
                                                            Where(Function(w) MapDots.GetPoint(w).Wall = False AndAlso
                                                                       MapDots.GetPoint(w).ShortestFromStart = 0 AndAlso
                                                                       MapDots.GetPoint(w).StartPoint = False
                                                                  )).Distinct().ToList()

                If options.Count = 0 Then
                    Exit While
                End If

                For Each item In options
                    MapDots.GetPoint(item).ShortestFromStart = steps
                Next

                currentOptions = options
            End While

            If endDot.ShortestFromStart <> 0 Then
                Dim current = positions.endPos
                Dim lookfor = MapDots.GetPoint(current).ShortestFromStart - 1
                While lookfor > 0
                    current = MapDots.GetAroundArrayOfArrays(current).First(Function(q) MapDots.GetPoint(q).ShortestFromStart = lookfor)
                    lookfor = lookfor - 1
                    MapDots.GetPoint(current).PathUsed = True
                End While

                Helpers.SaveImage(MapDots, Size, savePath)
            End If
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