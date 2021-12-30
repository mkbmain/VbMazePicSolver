Imports System.IO


Module Program
    Sub Main(args As String())
        If args.Length < 1 Then
            Console.WriteLine("a picture or directory containing png's needs to be a argument")
            Return
        End If
        Dim showWorking As Boolean = False
        If args.First().ToLower() = "true" Or args.First().ToLower() = "false" Then
            showWorking = Boolean.Parse(args.First())
            args = args.Skip(1).ToArray()
        End If

        Dim paths As String() = Array.Empty(Of String)()
        Dim path As String = String.Join(" ", args)
        If Directory.Exists(path) Then
            paths = Directory.GetFiles(path).Where(Function(t) t.ToLower().EndsWith(".png") Or t.ToLower().EndsWith(".jpg") Or t.ToLower().EndsWith(".bmp")) _
                .Where(Function(t) Helpers.ExtractFileName(t).ToLower().Contains("-solution") = False).ToArray()
        ElseIf File.Exists(path) Then
            paths = {path}
        End If

        If paths.Any() = False Then
            Console.WriteLine("a picture or directory containing png's needs to be a argument")
            Return
        End If
        For Each loc As String In paths
            Run(loc, showWorking)
        Next
    End Sub

    Public Sub Run(imagePath As String, showWorking As Boolean)
        Dim newFileName As String = Helpers.ExtractFileName(imagePath).Split(".").First() + "-solution.png"
        Dim outputFile As String = Path.Combine(Helpers.ExtractDirectory(imagePath), newFileName)
        Dim map = New Map.Map(imagePath)
        map.SaveSolution(outputFile, showWorking)
    End Sub
End Module