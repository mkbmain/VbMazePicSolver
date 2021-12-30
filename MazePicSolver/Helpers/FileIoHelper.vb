Imports System.IO

Namespace Helpers
    Public Module FileIoHelper
        Private _pathSeparator As Char = Nothing
        Private ReadOnly Property PathSeparatorCharacter() As Char
            Get
                If _pathSeparator <> Nothing Then
                    Return _pathSeparator
                End If
                _pathSeparator = Path.Combine(" ", " ").Trim().First()
                Return _pathSeparator
            End Get
        End Property

        Public Function ExtractDirectory(path As String) As String
            Return String.Join(PathSeparatorCharacter, path.Split(PathSeparatorCharacter).Take(path.Split(PathSeparatorCharacter).Length - 1))
        End Function
        Public Function ExtractFileName(path As String) As String
            Return path.Split(PathSeparatorCharacter).Last()
        End Function
    End Module
End Namespace