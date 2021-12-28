Imports System.Drawing
Imports System.Runtime.CompilerServices

Namespace Extensions
    ' overkill I know but wanted to play with extensions and generics in VB
    Public Module IterateMethods
        <Extension()>
        Public Sub IterateThroughMap(Of T)(ByRef arrayOfArrays As IEnumerable(Of IEnumerable(Of T)), ByRef func As Action(Of Integer, Integer, T()()))
            For x As Integer = 0 To arrayOfArrays.Count() - 1
                For y As Integer = 0 To arrayOfArrays.First().Count() - 1
                    func(x, y, arrayOfArrays)
                Next
            Next
        End Sub

        <Extension()>
        Public Function GetAroundArrayOfArrays(Of T)(arrayOfArrays As IEnumerable(Of IEnumerable(Of T)), pos As Point) As List(Of Point)
            Return New List(Of Point)() From {New Point(pos.X - 1, pos.Y), New Point(pos.X + 1, pos.Y), New Point(pos.X, pos.Y - 1), New Point(pos.X, pos.Y + 1)} _
                .Where(Function(e) e.X < arrayOfArrays.Count() And e.X > -1) _
                .Where(Function(e) e.Y > -1 And e.Y < arrayOfArrays.First().Count()).ToList()
        End Function
    End Module
End Namespace