Imports System
Imports System.Drawing
Imports Pretty_Printer

Module Program
    Sub Main(args As String())
        Dim PrettyPrinter = New PrettyPrinter With {
            .BackgroundColour = Color.Blue
        }
        PrettyPrinter.PrintLine("Blue background")
        Console.WriteLine($"{Console.BackgroundColor} = {ConsoleColor.Blue}")

        PrettyPrinter.Reset()
        PrettyPrinter.PrintLine(Console.BackgroundColor.ToString & " or is it " & PrettyPrinter.BackgroundColour.ToString)
        PrettyPrinter.BackgroundColour = Color.FromArgb(Color.Peru.ToArgb)
        PrettyPrinter.PrintLine("Wow you're smart")
        PrettyPrinter.PrintLine("Ok i can't see stop")
        PrettyPrinter.BackgroundColour = Nothing
        PrettyPrinter.PrintLine("ok better")

    End Sub
End Module
