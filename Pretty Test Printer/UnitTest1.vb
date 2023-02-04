Imports NUnit.Framework
Imports Pretty_Printer
Imports System.Drawing

Namespace Pretty_Test_Printer

    Public Class Tests
        Dim PrettyPrinter As Console

        <SetUp>
        Public Sub Setup()
            PrettyPrinter = New Console()
        End Sub

        <Test>
        Public Sub SetBackgroundConsoleColour()
            PrettyPrinter.BackgroundColour = Color.Blue
            PrettyPrinter.PrintLine("Blue background")
            System.Console.WriteLine($"{System.Console.BackgroundColor} = {ConsoleColor.Blue}")
            Assert.True(Global.System.Console.BackgroundColor = Global.System.ConsoleColor.Blue)
        End Sub

        <Test>
        Public Sub SetBackgroundColour()
            REM todo find out a better way to check console output settings
            PrettyPrinter.BackgroundColour = Color.Peru
            PrettyPrinter.PrintLine(Color.Peru.toString)
            System.Console.WriteLine($"{System.Console.BackgroundColor} = {ConsoleColor.Blue}")
            Assert.True(PrettyPrinter.BackgroundColour = Color.Peru)
        End Sub



    End Class

End Namespace