Imports System.Drawing
Imports csc530.Pretty
Imports DL.PrettyText

Module Program
	Private ReadOnly Pretty As csc530.Pretty.PrettyConsole = New PrettyConsole()

	Function Main(args As String()) As Integer
		Pretty.PrintLine($"I'm set blue", , ConsoleColour.BrightBlue)
				TestUnderline()
				TestPrintOverloads()
		TestTextColour()
				TestAlternatePrint()
				TestSlowPrint()
		Return 0
	End Function

	Private Sub TestAlternatePrint()
		TestSingleColourAlternatePrint()
		MultiColourAlternate()
	End Sub

	Dim rainbow as ConsoleColour()= {Color.Red, Color.Orange, Color.Green, Color.Blue, Color.Indigo, Color.Violet}

	Sub MultiColourAlternate()
		dim colours as consolecolour() = {Color.Tomato, Color.Thistle, Color.Turquoise}
		Pretty.AlternatePrint("Now we can get real radical with our coolours...", rainbow, colours)
		Pretty.AlternatePrintLine("Fine I'll make it  readable...?", rainbow)
		Pretty.AlternatePrintLine("better, happy, I am look how pretty I am... :D",, rainbow)
	End Sub

	Private Sub TestSingleColourAlternatePrint()
		Pretty.AlternateColourPrint("This is red then blue....", Color.Red, Color.Blue)
		Pretty.AlternateColourPrintLine("Now it's green back to normal.. :)", Color.Green)
		Pretty.AlternateBackgroundPrint("Ok now we're switching up the background....", Color.FromArgb(42, 18, 223), Color.Aquamarine)
		Pretty.AlternateBackgroundPrintLine("Ok impressive", Color.MediumSpringGreen)
	End Sub

	Private Sub TestSlowPrint()
		Pretty.SlowPrint("Let's slow things down..", 1)
		Pretty.SlowPrintLine("Ok that was too slow XD")
		Pretty.SlowPrintLine("Let ramp things up then", 10, unitOfSpeed:=PrettyConsole.PrintUnit.Word)
		Pretty.SlowPrintLine("Let ramp things up then", 1, unitOfSpeed:=PrettyConsole.PrintUnit.Line)
	End Sub

	Private Sub TestPrintOverloads()
		Pretty.Print("Well Let's do something for the one line", Color.Beige, Color.FromArgb(123))
		Pretty.PrintLine(" ___ Impressive but let's try state maintenance?")
		Pretty.PrintLine("Well colour this on for size🌠🌠🌠🌠🌃", Color.Black, Color.Cornsilk)
		Pretty.PrintLine("Ok finew razzle the ****dazzle**** off of me", underline := true)
		Pretty.PrintLine($"{vbCrLf}LEVELED UP AND UNDERLINED", textColour:=Color.Azure, underline:=True)
	End Sub

	Private Sub TestUnderline()
		Pretty.Underline = True
		Dim t = False
		Pretty.Print(t.ToString())
		t = Nothing
		Pretty.PrintLine(t.ToString())
		Pretty.Underline = False
	End Sub

	Private Sub TestTextColour()
		Pretty.TextColour = Color.Aquamarine
		Pretty.PrintLine("Isn' aquaMARriineeeeee")

		Pretty.TextColour = Color.FromArgb(25, 90, 87)
		Pretty.PrintLine("Well that wasssssss a colour")
		Pretty.TextColour = Nothing
		Pretty.PrintLine("Ah back to the way things should")
	End Sub

	Private Sub CalcAvgPrintCharTime()
		Dim test(100) As Decimal
		Dim timer1 = New Stopwatch()
		For i = 0 To test.Length - 1
			timer1.Start()
			Pretty.Print($"{i}")
			timer1.Stop()
			Pretty.PrintLine($": {timer1.Elapsed}")
			test(i) = timer1.Elapsed.TotalMilliseconds
			timer1.Reset()
		Next

		Pretty.PrintLine(test.Average)
	End Sub

	Private Sub TeestBackground()
		Pretty.BackgroundColour = Color.Blue
		Pretty.PrintLine("Blue background")
		Pretty.PrintLine($"TO PERUU!!")


		Pretty.BackgroundColour = Color.FromArgb(Color.Peru.ToArgb)
		Pretty.PrintLine("Wow you're smart")
		Pretty.PrintLine("Ok i can't see stop")


		Pretty.BackgroundColour = Nothing
		Pretty.PrintLine("ok better")
	End Sub
End Module
