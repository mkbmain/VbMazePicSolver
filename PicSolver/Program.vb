Imports System
Imports System.IO


Module Program
    Sub Main(args As String())
        args = {"d:\"}
        If args.Length < 1 Then
            Console.WriteLine("a picture or directory containing png's needs to be a argument")
            Return
        End If
        Dim paths As String() = {}
        Dim path As String = String.Join(" ", args)
        If (Directory.Exists(path)) Then
            paths = Directory.GetFiles(path).Where(Function(t) t.ToLower().EndsWith(".png") Or t.ToLower().EndsWith(".jpg") Or t.ToLower().EndsWith(".bmp")).ToArray()
        ElseIf File.Exists(path) Then
            paths = {path}
        End If
        If paths.Any() = False Then
            Console.WriteLine("a picture or directory containing png's needs to be a argument")
            Return
        End If
        For Each loc As String In paths
            Run(loc)
        Next
    End Sub

    Public Sub Run(ByVal imagePath As String)
        Dim newFileName As String = ExtractFileName(imagePath).Split(".").First() + "-solution.png"
        Dim outputFile As String = System.IO.Path.Combine(ExtractDirectory(imagePath), newFileName)
        Dim map = New Map(imagePath)
        map.SaveSoultion(outputFile)
    End Sub
End Module