Imports System


Module Program
    Sub Main(args As String())
#If DEBUG Then
        args = {"D:\ourPic.png"}
#End If
        If args.Length < 1 Then
            Console.WriteLine("Picture needs to be a argument")
        End If
        Dim path As String = String.Join(" ", args)
        Dim map = New Map(path)
        map.SaveSoultion(String.Join("\", path.Split("\").Take(path.Split("\").Length - 1)) + "\" + "solution.png")

        For x As Integer = 0 To map.Size.Width - 1
            For y As Integer = 0 To map.Size.Height - 1
                Dim dot As Dot = map.GetDot(x, y)
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


End Module
