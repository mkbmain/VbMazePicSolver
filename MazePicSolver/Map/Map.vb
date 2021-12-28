Imports System.Drawing
Imports MazePicSolver.Extensions

Namespace Map

    Class Map
        Private Property MapDots As MapDot()()
        Private Property Solved As Boolean
        Sub New(ByVal imagePath As String)
            MapDots = Helpers.LoadMapDotsFromImage(imagePath)
        End Sub

        Public ReadOnly Property Size As Size
            Get
                Return New Size(MapDots.Length, MapDots.First().Length)
            End Get
        End Property

        Public Function GetDot(ByVal point As Point) As MapDot
            Return GetDot(point.X, point.Y)
        End Function
        Public Function GetDot(ByVal x As Integer, ByVal y As Integer) As MapDot
            Return MapDots(x)(y)
        End Function

        Public Sub SaveSoultion(ByVal savePath As String)
            If Solved = False Then
                Solve()
            End If
            Helpers.SaveImage(MapDots, Me.Size, savePath)
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
                Dim allOptions = MapDots.GetAroundArrayOfArrays(currentLoc)
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


        Public Function GetStartAndEndPoint() As (startPos As Point, endPos As Point)
            Dim start As Point = Nothing
            Dim ends As Point = Nothing
            Dim func As Action(Of Integer, Integer, MapDot()()) = Sub(x, y, map)
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

            MapDots.IterateThroughMap(func)

            If start = Nothing Or ends = Nothing Then
                Throw New Exception("no start or end found")
            End If

            Return (start, ends)
        End Function

    End Class
End Namespace