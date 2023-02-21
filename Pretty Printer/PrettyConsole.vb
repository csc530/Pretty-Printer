Imports System.Drawing
Imports System.Text
Imports System.Threading

''' <summary>
''' <see cref="Console"/> wrapper class extending its features with virtual terminal sequences
''' </summary>
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
	''' <summary>
	''' Starting escape sequence to enter virtual terminal sequences
	''' </summary>
public	Shared ReadOnly SequenceStart As String = $"{Convert.ToChar(ConsoleKey.Escape)}["
	''' <summary>
	''' List of modifications to console output
	''' </summary>
	''' <returns>A set of changes to the console output</returns>
	Private Property ConsoleModifications As HashSet(Of ConsoleVirtualTerminalSequences)
	''' <summary>
	''' The virtual terminal sequences to modify the console output
	''' </summary>
	''' <returns>A string of the vts to modify subsequent outputs</returns>
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
					If BackgroundColour Is Nothing Then
						sequences.Append(49)
						ConsoleModifications.Remove(ConsoleVirtualTerminalSequences.TextBackground)
					Else
						sequences.Append($"48;2;{BackgroundColour.value.R};{BackgroundColour.value.G};{BackgroundColour.value.B}")
					End If
					sequences.Append("m"c)

				Case ConsoleVirtualTerminalSequences.TextColour
					If TextColour Is Nothing Then
						sequences.Append(39)
						ConsoleModifications.Remove(ConsoleVirtualTerminalSequences.TextColour)
					Else
						sequences.Append($"38;2;{TextColour.Value.R};{TextColour.value.G};{TextColour.value.B}")
					End If
					sequences.Append("m"c)

				Case Else
					sequences.Remove(sequences.Length - SequenceStart.Length, SequenceStart.Length)
					REM My.Application.Log can`t find log on `my`

			End Select
		Next
		Return sequences.ToString()
	End Function

	Private _underline As Boolean
	''' <summary>
	''' Underline printed console text
	''' </summary>
	''' <returns><see cref="Boolean"/> if the current output will underlined</returns>
	Public Property Underline As Boolean
		Get
			Return _underline
		End Get
		Set
			_underline = Value
			ConsoleModifications.Add(ConsoleVirtualTerminalSequences.TextUnderline)
		End Set
	End Property

	Private _backgroundColour As ConsoleColour

	''' <summary>
	''' The output's background colour; does NOT SUPPORT TRANSPARENCY in colours
	''' </summary>
	''' <value>
	''' The background colour;
	''' </value>
	''' <returns></returns>
	Public Property BackgroundColour As ConsoleColour
		Get
			Return _backgroundColour
		End Get
		Set(value As ConsoleColour)
			_backgroundColour = value
			ConsoleModifications.Add(ConsoleVirtualTerminalSequences.TextBackground)
		End Set
	End Property

	''' <summary>
	''' The text colour;
	''' does NOT SUPPORT TRANSPARENCY in colours
	''' </summary>
	Private _textColour As ConsoleColour

	Public Property TextColour As ConsoleColour
		Get
			Return _textColour
		End Get
		Set(value As ConsoleColour)
			_textColour = value
			ConsoleModifications.Add(ConsoleVirtualTerminalSequences.TextColour)
		End Set

	End Property


	''' <summary>Creates a pretty console object that writes to <see cref="System.Console"/> output</summary>
	''' <exception cref="NotSupportedException">If the environment console does not support Virtual Terminal Sequences<seealso cref="https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences"/></exception>
	Sub New()
		ConsoleModifications = New HashSet(Of ConsoleVirtualTerminalSequences)
		' other enum values they said weren't in use for windows to activate vts 🤷🏿‍♂️
		If Environment.OSVersion.Platform = PlatformID.Win32NT Then

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


	''' <summary>Resets all customization made to this pretty console</summary>
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
End Class
