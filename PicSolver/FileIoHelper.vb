Imports System.IO

Public Module FileIoHelper

    Private ReadOnly Property PathCharacter() As Char
        Get
            Return Path.Combine("4", "4").Replace("4", "").First()
        End Get
    End Property

    Public Function ExtractDirectory(ByVal path As String) As String
        Return String.Join(PathCharacter, path.Split(PathCharacter).Take(path.Split(PathCharacter).Length - 1))
    End Function
    Public Function ExtractFileName(ByVal path As String) As String
        Return path.Split(PathCharacter).Last()
    End Function
End Module