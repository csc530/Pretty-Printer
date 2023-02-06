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

	Public Shared ReadOnly SequenceStart As String = $"{Convert.ToChar(ConsoleKey.Escape)}["
	Private Property ConsoleModifications() As HashSet(Of ConsoleVirtualTerminalSequences)

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
			Dim out As WindowsConsoleApi.ConsoleModes
			WindowsConsoleApi.GetConsoleMode(stdHandle, out)
			Const virtualConsoleModes = WindowsConsoleApi.ConsoleModes.EnableVirtualTerminalProcessing Or
										WindowsConsoleApi.ConsoleModes.EnableProcessedOutput
			If Not out.HasFlag(virtualConsoleModes) AndAlso Not WindowsConsoleApi.SetConsoleMode(stdHandle, virtualConsoleModes) _
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
	''' <param name="newLine">if set to <c>true</c> appends a newline character to the end of the text.</param>
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

	Private Sub _printSlow(value As String, speed As Integer, backgroundColour As Color, textColour As Color, underline As Boolean)
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

	Public Sub AlternatePrintLine(value As String, Optional textcolours As List(Of Color) = Nothing,
		Optional backgroundColours As List(Of Color) = Nothing)
		alternatePrint(value & Environment.NewLine, textcolours, backgroundColours)
	End Sub


	Public Sub AlternatePrint(value As String, Optional textcolours As List(Of Color) = Nothing,
		Optional backgroundColours As List(Of Color) = Nothing, Optional textFrequency As Integer = 1, Optional textUnit As PrintUnit = PrintUnit.Character, Optional backgroundFrequency As Integer = 1, Optional backgruondUnit As PrintUnit = PrintUnit.Character)
		REM option
		If (textcolours Is Nothing OrElse textcolours.Count = 0) AndAlso (backgroundColours Is Nothing OrElse backgroundColours.Count = 0) Then
			Print(value)
		Else
			If textcolours Is Nothing Then
				textcolours = New List(Of Color) From {(TextColour)}
			End If
			If backgroundColours Is Nothing Then
				backgroundColours = New List(Of Color) From {(BackgroundColour)}
			End If
			'todo remove space/invisble characters from contributing ti akternate
			If textFrequency = 1 AndAlso backgroundFrequency = 1 Then
				alternatePrint(value, textcolours, backgroundColours)
			Else
				customalternateprint(value, textcolours, textFrequency, textUnit, backgroundColours, backgroundFrequency, backgruondUnit)
			End If

		End If
	End Sub

	Private Sub customalternateprint(value As String, textcolours As List(Of Color), textFrequency As Integer, textUnit As PrintUnit, backgroundColours As List(Of Color), backgroundFrequency As Integer, backgruondUnit As PrintUnit)
		'todo add select units
		Dim bgColour = (0)
		Dim txtColour = 0
		For index = 0 To value.Length - 1
			If index Mod textFrequency = 0 Then
				txtColour += 1
			End If
			If index Mod backgroundFrequency = 0 Then
				bgColour += 1
			End If
			Print(value(index), backgroundColours(bgColour Mod backgroundColours.Count), textcolours(txtColour Mod textcolours.Count))
		Next
	End Sub

	Private Sub alternatePrint(value As String, textcolours As List(Of Color), backgroundColours As List(Of Color))
		For index = 0 To value.Length - 1
			Print(value(index), backgroundColours(index Mod backgroundColours.Count), textcolours(index Mod textcolours.Count))
		Next
	End Sub

#End Region


	Private Shared Function IsConsoleColour(colour As Color) As Boolean
		If colour.Name = "0" Then
			Return False
		Else
			Return [Enum].TryParse(Of ConsoleColor)(colour.Name, True, Nothing)
		End If
	End Function
End Class
