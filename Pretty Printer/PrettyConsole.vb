Option Strict On

Imports System.Drawing
Imports System.Text
Imports System.Threading

Public Class PrettyConsole
	Private Enum ConsoleVirtualTerminalSequences
		'https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences#simple-cursor-positioning
		SimpleCursorPositioning
		'https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences#simple-cursor-positioning
		CursorPositioning
		'https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences#cursor-visibility
		CursorVisibility
		'https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences#cursor-shape
		CursorShape
		'https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences#viewport-positioning
		ViewportPositioning
		'https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences#text-modification
		TextUnderline
		'https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences#text-formatting
		TextBackground
		TextColour
	End Enum

	Shared ReadOnly SequenceStart As String = $"{Convert.ToChar(ConsoleKey.Escape)}["
	Private Property ConsoleModifications As HashSet(Of ConsoleVirtualTerminalSequences)


	Private Function GetVirtualSequences() As String
		Dim sequences = New StringBuilder()
		For Each sequence In ConsoleModifications
			sequences.Append(SequenceStart)
			Select Case sequence
				Case ConsoleVirtualTerminalSequences.SimpleCursorPositioning

				Case ConsoleVirtualTerminalSequences.CursorPositioning

				Case ConsoleVirtualTerminalSequences.CursorVisibility

				Case ConsoleVirtualTerminalSequences.CursorShape

				Case ConsoleVirtualTerminalSequences.ViewportPositioning

				Case ConsoleVirtualTerminalSequences.TextUnderline
					If Not Underline Then
						sequences.Append(24)
						ConsoleModifications.Remove(ConsoleVirtualTerminalSequences.TextUnderline)
					Else
						sequences.Append(4)
					End If
					sequences.Append("m"c)

				Case ConsoleVirtualTerminalSequences.TextBackground
					If BackgroundColour = Nothing Then
						sequences.Append(49)
						ConsoleModifications.Remove(ConsoleVirtualTerminalSequences.TextBackground)
					Else
						sequences.Append($"48;2;{BackgroundColour.R};{BackgroundColour.G};{BackgroundColour.B}")
					End If
					sequences.Append("m"c)

				Case ConsoleVirtualTerminalSequences.TextColour
					If TextColour = Nothing Then
						sequences.Append(39)
						ConsoleModifications.Remove(ConsoleVirtualTerminalSequences.TextColour)
					Else
						sequences.Append($"38;2;{TextColour.R};{TextColour.G};{TextColour.B}")
					End If
					sequences.Append("m"c)

				Case Else
					sequences.Remove(sequences.Length - SequenceStart.Length, SequenceStart.Length)
					'                    My.Application.Log can`t find log on `my`


			End Select
		Next
		Return sequences.ToString()
	End Function

	Private _underline As Boolean

	Public Property Underline As Boolean
		Get
			Return _underline
		End Get
		Set
			_underline = Value
			ConsoleModifications.Add(ConsoleVirtualTerminalSequences.TextUnderline)
		End Set
	End Property

	Private _backgroundColour As Color

	''' <value>
	''' The background colour;
	''' does NOT SUPPORT TRANSPARENCY in colours
	''' </value>
	Public Property BackgroundColour As Color
		Get
			Return _backgroundColour
		End Get
		Set(value As Color)
			_backgroundColour = value
			ConsoleModifications.Add(ConsoleVirtualTerminalSequences.TextBackground)
		End Set
	End Property

	''' <summary>
	''' The text colour;
	''' does NOT SUPPORT TRANSPARENCY in colours
	''' </summary>
	Private _textColour As Color

	Public Property TextColour As Color
		Get
			Return _textColour
		End Get
		Set(value As Color)
			_textColour = value
			ConsoleModifications.Add(ConsoleVirtualTerminalSequences.TextColour)
		End Set
	End Property


	'''
	''' <summary>Creates a pretty console object that writes to <see cref="System.Console"/> output</summary>
	''' <exception cref="NotSupportedException">If the environment console does not support Virtual Terminal Sequences<seealso cref="https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences"/></exception>
	''' 
	Sub New()
		ConsoleModifications = New HashSet(Of ConsoleVirtualTerminalSequences)
		' other enum values said they weren't in use 🤷🏿‍♂️
		If PlatformID.Win32NT = Environment.OSVersion.Platform Then

			'? Enable virtual terminal sequences
			Dim stdHandle = WindowsConsoleApi.GetStdHandle(WindowsConsoleApi.StdHandles.StdInputHandle)
			Dim consoleMode As WindowsConsoleApi.ConsoleModes
			WindowsConsoleApi.GetConsoleMode(stdHandle, consoleMode)
			Const compatibleConsoleModes = WindowsConsoleApi.ConsoleModes.EnableVirtualTerminalProcessing Or
			                               WindowsConsoleApi.ConsoleModes.EnableProcessedOutput
			If _
				Not consoleMode.HasFlag(compatibleConsoleModes) AndAlso
				Not WindowsConsoleApi.SetConsoleMode(stdHandle, compatibleConsoleModes) _
				Then
				Throw New NotSupportedException("Unable to initialize virtual console sequences")
			End If
		End If
	End Sub


	''' <summary>Resets this instance.</summary>
	Sub Reset()
		BackgroundColour = Nothing
		TextColour = Nothing
		Underline = False

		System.Console.Write(GetVirtualSequences)
	End Sub

	''' <summary>
	''' Clears console window of outputs
	''' </summary>
	Sub Clear()
		System.Console.Clear()
	End Sub


#Region "Print"

	Public Sub PrintLine(value As String, Optional backgroundColour As Color = Nothing,
		Optional textColour As Color = Nothing, Optional underline As Boolean = False)
		Print(value & Environment.NewLine, backgroundColour, textColour, underline)
	End Sub

	Sub PrintLine(value As String)
		Print(value & Environment.NewLine)
	End Sub

	Sub Print(value As String, Optional backgroundColour As Color = Nothing, Optional textColour As Color = Nothing,
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

	Sub Print(value As String)
		System.Console.Write(GetVirtualSequences() & value)
	End Sub


	Sub SlowPrintLine(value As String, Optional speed As Integer = 15,
		Optional backgroundColour As Color = Nothing, Optional textColour As Color = Nothing,
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
	Sub SlowPrint(value As String, Optional speed As Integer = 15,
		Optional backgroundColour As Color = Nothing, Optional textColour As Color = Nothing,
		Optional underline As Boolean = False, Optional unitOfSpeed As PrintUnit = PrintUnit.Character)
		' * made it blocking as if they try to print or modify the classes values not errors are thrown, sry🤷🏿‍♂️
		Select Case unitOfSpeed
			Case PrintUnit.Character
				' i.e. the x^-1 inverse because were' sleeping it and not setting the speed
				' ex. speed 2chars/sec => sleep = 500 = half a second not 2000 = 4chars/sec
				' by 1000 to convert to milliseconds
				speed = Convert.ToInt32(Math.Round(1000/speed, 0))
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

	Private Sub _printSlow(value As String, speed As Integer, backgroundColour As Color, textColour As Color,
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
					Thread.Sleep(Convert.ToInt32((285*speed/67)))
				End If
			Else
				Thread.Sleep(speed)
			End If
		Next
	End Sub

	'todo add select units

	Public Sub AlternateColourPrintLine(value As String, Optional primaryColour as Color = nothing,
		Optional secondaryColour as Color = Nothing)
		AlternateColourPrint(value & Environment.NewLine, primaryColour, secondaryColour)
	End Sub


	Public Sub AlternateColourPrint(value As String, Optional primaryColour as Color = nothing,
		Optional secondaryColour as Color = Nothing)
		if primaryColour = secondaryColour
			Print(value, textColour := primaryColour)
			Exit Sub
		End If

		'unset colours are set to the current textcolour
		If primaryColour = Nothing
			primaryColour = TextColour
		End If
		If secondaryColour = Nothing
			secondaryColour = TextColour
		end if

		Dim colours = {primaryColour, secondaryColour}
		For i = 0 to value.Length - 1
			Print(value(i), textColour := colours(i mod 2))
		Next
	End Sub

	Public Sub AlternateBackgroundPrintLine(value As String, Optional primaryColour as Color = nothing,
		Optional secondaryColour as Color = Nothing)
		AlternateBackgroundPrint(value & Environment.NewLine, primaryColour, secondaryColour)
	End Sub


	Public Sub AlternateBackgroundPrint(value As String, Optional primaryColour as Color = nothing,
		Optional secondaryColour as Color = Nothing)
		if primaryColour = secondaryColour
			Print(value, backgroundColour := primaryColour)
			Exit Sub
		End If

		'unset colours are set to the current textcolour
		If primaryColour = Nothing
			primaryColour = TextColour
		End If
		If secondaryColour = Nothing
			secondaryColour = TextColour
		end if

		Dim colours = {primaryColour, secondaryColour}
		For i = 0 to value.Length - 1
			Print(value(i), backgroundColour := colours(i mod 2))
		Next
	End Sub

	Public Sub AlternatePrintLine(value As String, Optional textColours as Color() = nothing,
		Optional backgroundColours as Color() = Nothing)
		AlternatePrint(value & Environment.NewLine, textColours, backgroundColours)
	End Sub

	Public Sub AlternatePrint(value As String, Optional textColours as Color() = nothing,
		Optional backgroundColours as Color() = Nothing)
		if IsNothing(textColours) And IsNothing(backgroundColours)
			Print(value)
			Exit Sub
		End If

		REM the checks for not isnothing(*) are superfluous from the above conditional but are added for clarity as well as the last for isnothing(y) for the same reason
		If not IsNothing(textColours) AndAlso textColours.Length = 2 AndAlso IsNothing(backgroundColours)
			AlternateColourPrint(value, textColours(0), textColours(1))
		elseif not IsNothing(backgroundColours) AndAlso backgroundColours.Length = 2 AndAlso IsNothing(textColours)
			AlternateBackgroundPrint(value, backgroundColours(0), backgroundColours(1))
		end if
		
		'todo
	End Sub


#End Region


	Private Shared Function IsConsoleColour(colour As Color) As Boolean
		If colour.Name = "0" Then
			Return False
		Else
			Return [Enum].TryParse (Of ConsoleColor)(colour.Name, True, Nothing)
		End If
	End Function
End Class
