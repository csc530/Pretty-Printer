Imports System
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Pretty_Printer

Module Program
	Sub Main(args As String())
		
	dim p =new 	PrettyPrinter()
		p.BackgroundColour = New Color().Blue
		p.PrintLine(Environment.CurrentDirectory)

		Dim getStdHandle As Integer = WindowsConsoleAPI.GetStdHandle(WindowsConsoleAPI.StdHandles.STD_INPUT_HANDLE)
		Console.WriteLine(getStdHandle)

		Dim out

		dim consoleMode = windowsConsoleAPI.getConsoleMode(getStdHandle, out)
		Console.WriteLine($"{consoleMode} - {out}")

		dim setMode = windowsConsoleAPI.setConsoleMode(getStdHandle,
		                                      WindowsConsoleAPI.ConsoleModes.ENABLE_VIRTUAL_TERMINAL_PROCESSING or   WindowsConsoleAPI.ConsoleModes.ENABLE_PROCESSED_OUTPUT or      WindowsConsoleAPI.ConsoleModes.ENABLE_VIRTUAL_TERMINAL_PROCESSING)
		console.WriteLine($"{setMode} - ")


		console.WriteLine($"{PrettyPrinter.SequenceStart}32m")
		
		
		Console.WriteLine(WindowsConsoleAPI.GetLastError)


		Dim enums = New List(Of String)
		For Each value As WindowsConsoleAPI.ConsoleModes In [Enum].GetValues(out.[GetType]())
			If out.HasFlag(value) Then
				enums.Add([Enum].GetName(value))
			End If
		Next

		enums.ForEach(Sub(e) Console.WriteLine(e))
	End Sub
End Module
