Imports System.Drawing
Imports System.Threading

Partial Class PrettyConsole

	''' <inheritdoc cref="PrintLine(String)"/>
	''' <param name="backgroundColour">The background colour of the text</param>
	''' <param name="textColour">The colour of the text</param>
	''' <param name="underline">underline the text</param>
	Public Sub PrintLine(value As String, Optional backgroundColour As ConsoleColour = Nothing,
						 Optional textColour As ConsoleColour = Nothing, Optional underline As Boolean = False)
		Print(value & Environment.NewLine, backgroundColour, textColour, underline)
	End Sub

	''' <summary>
	''' Print given text with an appended new line character to the console
	''' </summary>
	''' <param name="value">the text to print</param>
	Sub PrintLine(value As String)
		Print(value & Environment.NewLine)
	End Sub

	'''<inheritdoc cref="Print(String)"/>
	Sub Print(value As String, Optional backgroundColour As ConsoleColour = Nothing, Optional textColour As ConsoleColour = Nothing,
			  Optional underline As Boolean = False)

		Dim previous = (Me.BackgroundColour, Me.TextColour, underline)
		Me.BackgroundColour = backgroundColour
		Me.TextColour = textColour
		Me.Underline = underline
		Print(value)
		' ? revert back to original values
		Me.BackgroundColour = previous.BackgroundColour
		Me.TextColour = previous.TextColour
		Me.Underline = previous.underline
	End Sub

	''' <summary>Print the givent text to the console output</summary>
	''' <inheritdoc cref="PrintLine(String, ConsoleColour, ConsoleColour, Boolean)"/>
	Sub Print(value As String)
		System.Console.Write(GetVirtualSequences() & value)
	End Sub

	'''<summary>Print to the console ending with a new line character at a set speed; imitates typing text</summary>
	'''<inheritdoc cref="SlowPrint(String, Integer, ConsoleColour, ConsoleColour, Boolean, PrintUnit)"/>
	Sub SlowPrintLine(value As String, Optional speed As Integer = 15,
					  Optional backgroundColour As ConsoleColour = Nothing, Optional textColour As ConsoleColour = Nothing,
					  Optional underline As Boolean = False, Optional unitOfSpeed As PrintUnit = PrintUnit.Character)
		SlowPrint(value & Environment.NewLine, speed, backgroundColour, textColour, underline, unitOfSpeed)
	End Sub

	''' <summary>
	''' Print to the console at a set speed; imitates typing text
	''' </summary>
	''' <param name="value">The value to print</param>
	''' <param name="speed">The speed as <paramref name="unitOfSpeed">units</paramref> per second</param>
	''' <param name="backgroundColour">The background colour.</param>
	''' <param name="textColour">The text colour.</param>
	''' <param name="underline">if set to <c>true</c> underlines the text.</param>
	''' <param name="unitOfSpeed">The unit to print per second.</param>
	Sub SlowPrint(value As String, Optional speed As Integer = 15, Optional backgroundColour As ConsoleColour = Nothing, Optional textColour As ConsoleColour = Nothing, Optional underline As Boolean = False, Optional unitOfSpeed As PrintUnit = PrintUnit.Character)
		' * made it blocking as if they try to print or modify the classes values not errors are thrown, sry🤷🏿‍♂️
		Select Case unitOfSpeed
			Case PrintUnit.Character
				' i.e. the x^-1 inverse because were' sleeping it and not setting the speed
				' ex. speed 2chars/sec => sleep = 500 = half a second not 2000 = 4chars/sec
				' by 1000 to convert to milliseconds
				speed = Convert.ToInt32(Math.Round(1000 / speed, 0))
				_printSlow(value, speed, backgroundColour, textColour, underline)
			Case PrintUnit.Line
				For Each line In value.Split(Environment.NewLine)
					' simply put; we want to print out chars per second with the unit chars duh🙄
					SlowPrint(line, line.Length, backgroundColour, textColour, underline, PrintUnit.Character)
				Next
			Case PrintUnit.Word
				For Each word In value.Split(" ")
					SlowPrint(word & " ", word.Length, backgroundColour, textColour, underline, PrintUnit.Character)
				Next
		End Select
	End Sub

	Private Sub _printSlow(value As String, speed As Integer, backgroundColour As ConsoleColour, textColour As ConsoleColour,
						   underline As Boolean)
		Dim character As Char
		For index = 0 To value.Length - 1
			character = value(index)
			Print(character, backgroundColour, textColour, underline)
			If _
				index + 1 < value.Length AndAlso
				(Environment.NewLine = value(index + 1) OrElse value(index + 1) = vbCrLf OrElse value(index + 1) = vbLf) Then
				Print(value(index + 1), backgroundColour, textColour, underline)
				index += 1
				'todo change cursor to underline just not block makes newline jump jarring
				If index + 1 <> value.Length - 1 Then
					Thread.Sleep(Convert.ToInt32((285 * speed / 67)))
				End If
			Else
				Thread.Sleep(speed)
			End If
		Next
	End Sub

End Class
