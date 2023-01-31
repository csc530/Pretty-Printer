Imports System
Imports System.Drawing
Imports Pretty_Printer

Module Program
    Sub Main(args As String())
        Dim PrettyPrinter = New PrettyPrinter()

        PrettyPrinter.BackgroundColour = Color.Blue
        PrettyPrinter.PrintLine("Blue background")
        Console.WriteLine($"{Console.BackgroundColor} = {ConsoleColor.Blue}")


        PrettyPrinter.Reset()

        PrettyPrinter.BackgroundColour = Color.FromArgb(Color.Peru.ToArgb)
        PrettyPrinter.PrintLine(Color.Peru.ToArgb)
        Console.WriteLine($"{PrettyPrinter.SequenceStart}48;2;153;144;254mPoppySeed")
        Console.WriteLine($"{PrettyPrinter.BackgroundColour} = {Color.Peru}")
    End Sub
End Module
