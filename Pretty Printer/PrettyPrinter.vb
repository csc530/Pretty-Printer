Option Strict On

Imports System.Drawing
Imports System.Text

Public Class PrettyPrinter
	Enum Colour
		None
		Black
		Red
		Green
		Yellow
		Blue
		Magenta
		Cyan
		White
		Bright = 16
	End Enum

	<Flags>
	Enum ColourModifier
		'Returns all attributes To the Default state prior To modification
		None = 0
		'Applies brightness/intensity flag To foreground color
		Bright = 1
		'Removes brightness/intensity flag from foreground color
		NoBright = 22
		'Adds underline
		Underline = 4
		'Removes underline
		NoUnderline = 24
		'Swaps foreground And background colors
		Negative = 7
		'(No negative)	Returns foreground/background To normal
		Positive = 27
	End Enum

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

	Public shared ReadOnly SequenceStart As String = $"{Convert.ToChar(ConsoleKey.Escape)}["
	Private Property ConsoleModifications() As HashSet(Of ConsoleVirtualTerminalSequences)

	Private Function GetVirtualSequences() As String
		Dim sequences = New StringBuilder()
		For Each sequence In ConsoleModifications
			sequences.Append(SequenceStart)
			Select Case sequence
				Case ConsoleVirtualTerminalSequences.SimpleCursorPositioning
					Exit Select
				Case ConsoleVirtualTerminalSequences.CursorPositioning
					Exit Select
				Case ConsoleVirtualTerminalSequences.CursorVisibility
					Exit Select
				Case ConsoleVirtualTerminalSequences.CursorShape
					Exit Select
				Case ConsoleVirtualTerminalSequences.ViewportPositioning
					Exit Select
				Case ConsoleVirtualTerminalSequences.TextUnderline
					If	Underline = false then
						sequences.append(24)
						ConsoleModifications.remove(ConsoleVirtualTerminalSequences.TextUnderline)
						else
							sequences.append(4)
					End If
					sequences.Append("m"c)
					Exit Select
				Case ConsoleVirtualTerminalSequences.TextBackground
					If BackgroundColour = Nothing Then
						sequences.Append(49)
						ConsoleModifications.Remove(ConsoleVirtualTerminalSequences.TextBackground)
					Else
						sequences.Append($"48;2;{BackgroundColour.R};{BackgroundColour.G};{BackgroundColour.B}")
					End If
					sequences.Append("m"c)
					Exit Select
				Case ConsoleVirtualTerminalSequences.TextColour
					If TextColour = Nothing Then
						sequences.Append("39")
						ConsoleModifications.Remove(ConsoleVirtualTerminalSequences.TextBackground)
					Else
						sequences.Append($"38;2;{TextColour.R};{TextColour.G};{TextColour.B}")
					End If
					sequences.Append("m"c)
					Exit Select
				Case Else
					sequences.Remove(sequences.Length - SequenceStart.Length, SequenceStart.Length)
					'                    My.Application.Log can`t find log on `my`
					Exit Select

			End Select
		Next
		Return sequences.ToString()
	End Function

	Private _underline as boolean
	public Property	Underline as boolean
		get 
			return _underline
		End get
		Set
			_underline = value
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


	Sub New()
		ConsoleModifications = New HashSet(Of ConsoleVirtualTerminalSequences)

		' other enum values said they weren't in use 🤷🏿‍♂️
		if not PlatformId.Win32NT = environment.OSVersion.Platform then
			exit sub
		End If

		'? Enable virtual terminal sequences
		Dim stdHandle = WindowsConsoleAPI.GetStdHandle(WindowsConsoleAPI.StdHandles.STD_INPUT_HANDLE)
		Dim out as WindowsConsoleAPI.ConsoleModes
		windowsConsoleAPI.getConsoleMode(stdHandle, out)
		Const virtualConsoleModes = WindowsConsoleAPI.ConsoleModes.ENABLE_VIRTUAL_TERMINAL_PROCESSING or
		                            WindowsConsoleAPI.ConsoleModes.ENABLE_PROCESSED_OUTPUT
		If Not out.HasFlag(virtualConsoleModes) AndAlso Not windowsConsoleAPI.setConsoleMode(stdHandle, virtualConsoleModes) _
			Then
			Throw new SystemException("Unable to initialize virtual console sequences")
		End If
	End Sub


	''' <summary>Resets this instance.</summary>
	Sub Reset()
		BackgroundColour = Nothing
		TextColour = Nothing
		Underline = false

		Console.Write(GetVirtualSequences)
	End Sub


#Region "Print"

	Sub PrintLine(value As String)
		Print(value & Environment.NewLine)
	End Sub


	Sub Print(value As String)
		Console.Write(GetVirtualSequences() & value)
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
