Imports System
Imports System.Drawing
Imports Pretty_Printer

Module Program
    Sub Main(args As String())
        Dim PrettyPrinter = New PrettyPrinter With {
            .BackgroundColour = Color.Blue
        }
        PrettyPrinter.PrintLine("Blue background")
        PrettyPrinter.PrintLine($"{Console.BackgroundColor} = {ConsoleColor.Blue}")

        PrettyPrinter.Reset()
        PrettyPrinter.PrintLine(Console.BackgroundColor.ToString & " or is it " & PrettyPrinter.BackgroundColour.ToString)

        PrettyPrinter.BackgroundColour = Color.FromArgb(Color.Peru.ToArgb)
        PrettyPrinter.PrintLine("Wow you're smart")
        PrettyPrinter.PrintLine("Ok i can't see stop")

        PrettyPrinter.BackgroundColour = Nothing
        PrettyPrinter.PrintLine("ok better")


        PrettyPrinter.TextColour = Color.Aquamarine
        PrettyPrinter.PrintLine("Isn' aquaMARriineeeeee")

        PrettyPrinter.TextColour = Color.FromArgb(25, 90, 87)
        PrettyPrinter.PrintLine("Well that wasssssss a colour"
                             )
        PrettyPrinter.TextColour = Nothing
        PrettyPrinter.PrintLine("Ah back to the way things should")
    End Sub
End Module
