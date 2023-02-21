Imports System.Drawing
Imports System.Text
Imports System.Threading

''' <summary>
''' <see cref="Console"/> wrapper class extending its features with virtual terminal sequences
''' </summary>
Public Class PrettyConsole



	''' <summary>
	''' Underline printed console text
	''' </summary>
	''' <returns><see cref="Boolean"/> if the current output will underlined</returns>
	Public Property Underline As Boolean


	''' <summary>
	''' The output's background colour; does NOT SUPPORT TRANSPARENCY in colours
	''' </summary>
	''' <value>
	''' The background colour;
	''' </value>
	''' <returns></returns>
	Public Property BackgroundColour As ConsoleColour

	''' <summary>
	''' The text colour;
	''' does NOT SUPPORT TRANSPARENCY in colours
	''' </summary>
	Public Property TextColour As ConsoleColour


	''' <summary>Creates a pretty console object that writes to <see cref="System.Console"/> output</summary>
	''' <exception cref="NotSupportedException">If the environment console does not support Virtual Terminal Sequences<seealso cref="https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences"/></exception>
	Sub New()
		' other enum values they said weren't in use for windows to activate vts 🤷🏿‍♂️
		If Environment.OSVersion.Platform = PlatformID.Win32NT Then

			'? Enable virtual terminal sequences
			Dim stdHandle = WindowsConsoleApi.GetStdHandle(WindowsConsoleApi.StdHandles.StdInputHandle)
			Dim consoleMode As WindowsConsoleApi.ConsoleModes
			WindowsConsoleApi.GetConsoleMode(stdHandle, consoleMode)
			Const compatibleConsoleModes = WindowsConsoleApi.ConsoleModes.EnableVirtualTerminalProcessing Or WindowsConsoleApi.ConsoleModes.EnableProcessedOutput
			If _
				Not consoleMode.HasFlag(compatibleConsoleModes) AndAlso
				Not WindowsConsoleApi.SetConsoleMode(stdHandle, compatibleConsoleModes) _
				Then
				Throw New NotSupportedException("Unable to initialize virtual console sequences")
			End If
		End If
	End Sub

	Function GetVirtualSequences() As String
		Dim sequence As New StringBuilder
		sequence.Append(VirtualTerminalSequences.GetColourSequence(TextColour, VirtualTerminalSequences.ConsoleLayer.Foreground))
		sequence.Append(VirtualTerminalSequences.GetColourSequence(BackgroundColour, VirtualTerminalSequences.ConsoleLayer.Background))
		sequence.Append(VirtualTerminalSequences.GetUnderlinedSequence(Underline))
		Return sequence.ToString
	End Function


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
