Imports System.Drawing
Imports Pretty_Printer

Module Program
	Private ReadOnly Pretty As Pretty_Printer.Console = New Console()

	Function Main(args As String()) As Integer
		TestAlternatePrint()
		Return 0
	End Function

	Private Sub TestAlternatePrint()
		Pretty.AlternatePrint("Hey so this is cool", {Color.RebeccaPurple, Color.PowderBlue}.ToList, {Color.AliceBlue}.ToList)
		Pretty.AlternatePrint("But does it realllly work", {Color.RebeccaPurple, Color.PowderBlue}.ToList,)
		Pretty.AlternatePrint("Hey so this is cool", backgroundColours:={Color.RebeccaPurple, Color.PowderBlue}.ToList)

	End Sub

	Private Sub TestSlowPrint()
		Pretty.SlowPrint("Let's slow things down..", 1)
		Pretty.SlowPrint("Ok that was too slow XD")
		Pretty.SlowPrint("Let ramp thungs up then", 10, unitOfSpeed:=Console.PrintUnit.Word)
		Pretty.SlowPrint("Let ramp thungs up then", 1, unitOfSpeed:=Console.PrintUnit.Line)
	End Sub

	Private Sub TestPrintOverloads()
		Pretty.Print("Well Let's do something for the one line", Color.Beige, Color.FromArgb(123))
		Pretty.PrintLine(" ___ Impressive but let's try state maintenance?")
		Pretty.PrintLine("Well colour this on for size🌠🌠🌠🌠🌃", Color.Black, Color.Cornsilk)
		Pretty.PrintLine("Ok finew razzle the ****dazzle**** off of me")
		Pretty.PrintLine($"{vbCrLf}LEVELED UP AND UNDERLINED", textColour := Color.Azure, underline := True)
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
