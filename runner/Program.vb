Imports System
Imports System.Drawing
Imports Pretty_Printer

Module Program
	Sub Main(args As String())
		Dim prettyPrinter = New PrettyPrinter With {
				.BackgroundColour = Color.Blue
				}
		prettyPrinter.PrintLine("Blue background")
		prettyPrinter.PrintLine($"{Console.BackgroundColor} = {ConsoleColor.Blue}")

		prettyPrinter.Reset()
		prettyPrinter.PrintLine(Console.BackgroundColor.ToString & " or is it " & prettyPrinter.BackgroundColour.ToString)

		prettyPrinter.BackgroundColour = Color.FromArgb(Color.Peru.ToArgb)
		prettyPrinter.PrintLine("Wow you're smart")
		prettyPrinter.PrintLine("Ok i can't see stop")

		prettyPrinter.BackgroundColour = Nothing
		prettyPrinter.PrintLine("ok better")


		prettyPrinter.TextColour = Color.Aquamarine
		prettyPrinter.PrintLine("Isn' aquaMARriineeeeee")

		prettyPrinter.Underline = True
		Dim t = False
		prettyPrinter.Print(t.ToString())
		t = Nothing
		prettyPrinter.PrintLine(t.ToString())
		prettyPrinter.Underline = False

		prettyPrinter.TextColour = Color.FromArgb(25, 90, 87)
		prettyPrinter.PrintLine("Well that wasssssss a colour")
		prettyPrinter.TextColour = Nothing
		prettyPrinter.PrintLine("Ah back to the way things should")
	End Sub
End Module
