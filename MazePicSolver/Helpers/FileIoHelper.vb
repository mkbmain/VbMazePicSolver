Imports System.IO

Namespace Helpers
    Public Module FileIoHelper
        Private ReadOnly Property PathSeparatorCharacter() As Char
            Get
                Return Path.Combine("4", "4").Replace("4", "").First()
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