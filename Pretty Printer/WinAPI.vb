﻿''' <summary>
''' 
''' 
''' <seealso cref="https://learn.microsoft.com/en-us/windows/console/classic-vs-vt#definitions"/>
''' </summary>
Public NotInheritable Class WindowsConsoleApi
	Declare Auto Function GetLastError Lib "Kernel32.dll"() As Integer

	' From https://learn.microsoft.com/en-us/windows/console/getconsolemode
	<Flags>
	Public Enum ConsoleHandle
		''' <summary>
		''' Characters read by the ReadFile Or ReadConsole Function are written To the active screen buffer As they are typed into the console. This mode can be used only If the ENABLE_LINE_INPUT mode Is also enabled.
		''' </summary>
		EnableEchoInput = &H4
		''' <summary>
		''' When enabled, text entered In a console window will be inserted at the current cursor location And all text following that location will Not be overwritten. When disabled, all following text will be overwritten.
		''' </summary>
		EnableInsertMode = &H20
		''' <summary>
		''' The ReadFile Or ReadConsole Function returns only When a carriage Return character Is read. If this mode Is disabled, the functions Return When one Or more characters are available.
		''' </summary>
		EnableLineInput = &H2
		''' <summary>
		''' If the mouse pointer Is within the borders Of the console window And the window has the keyboard focus, mouse events generated by mouse movement And button presses are placed In the input buffer. These events are discarded by ReadFile Or ReadConsole, even When this mode Is enabled. The ReadConsoleInput Function can be used To read MOUSE_EVENT input records from the input buffer.
		''' </summary>
		EnableMouseInput = &H10
		''' <summary>
		''' CTRL+C Is processed by the system And Is Not placed In the input buffer. If the input buffer Is being read by ReadFile Or ReadConsole, other control keys are processed by the system And are Not returned In the ReadFile Or ReadConsole buffer. If the ENABLE_LINE_INPUT mode Is also enabled, backspace, carriage Return, And line feed characters are handled by the system.
		''' </summary>
		EnableProcessedInput = &H1
		''' <summary>
		''' This flag enables the user To use the mouse To Select And edit text. To enable this mode, use ENABLE_QUICK_EDIT_MODE | ENABLE_EXTENDED_FLAGS. To disable this mode, use ENABLE_EXTENDED_FLAGS without this flag.
		''' </summary>
		EnableQuickEditMode = &H40
		''' <summary>
		''' User interactions that change the size Of the console screen buffer are reported In the console's input buffer. Information about these events can be read from the input buffer by applications using the ReadConsoleInput function, but not by those using ReadFile or ReadConsole.
		''' </summary>
		EnableWindowInput = &H8
		''' <summary>
		''' Setting this flag directs the Virtual Terminal processing engine To convert user input received by the console window into Console Virtual Terminal Sequences that can be retrieved by a supporting application through ReadFile Or ReadConsole functions.
		''' 
		''' The typical usage Of this flag Is intended In conjunction With ENABLE_VIRTUAL_TERMINAL_PROCESSING On the output handle To connect To an application that communicates exclusively via virtual terminal sequences.
		''' </summary>
		EnableVirtualTerminalInput = &H200
	End Enum

	<Flags>
	Enum ConsoleModes
		''' <summary>
		''' Characters written by the WriteFile Or WriteConsole Function Or echoed by the ReadFile Or ReadConsole Function are parsed For ASCII control sequences, And the correct action Is performed. Backspace, tab, bell, carriage Return, And line feed characters are processed. It should be enabled When Using control sequences Or When ENABLE_VIRTUAL_TERMINAL_PROCESSING Is Set.
		''' </summary>
		EnableProcessedOutput = &H1
		''' <summary>
		''' When writing With WriteFile Or WriteConsole Or echoing With ReadFile Or ReadConsole, the cursor moves To the beginning Of the Next row When it reaches the End Of the current row. This causes the rows displayed In the console window To scroll up automatically When the cursor advances beyond the last row In the window. It also causes the contents Of the console screen buffer To scroll up (../discarding the top row Of the console screen buffer) When the cursor advances beyond the last row In the console screen buffer. If this mode Is disabled, the last character In the row Is overwritten With any subsequent characters.
		''' </summary>
		EnableWrapAtEolOutput = &H2
		''' <summary>
		''' When writing With WriteFile Or WriteConsole, characters are parsed For VT100 And similar control character sequences that control cursor movement, color/font mode, And other operations that can also be performed via the existing Console APIs. For more information, see Console Virtual Terminal Sequences.
		''' 
		''' **Ensure ENABLE_PROCESSED_OUTPUT Is Set When Using this flag.**
		''' </summary>
		EnableVirtualTerminalProcessing = &H4

		''' <summary>
		''' When writing With WriteFile Or WriteConsole, this adds an additional state To End-Of-line wrapping that can delay the cursor move And buffer scroll operations.
		''' 
		''' Normally when ENABLE_WRAP_AT_EOL_OUTPUT Is set And text reaches the end of the line, the cursor will immediately move to the next line And the contents of the buffer will scroll up by one line. In contrast with this flag set, the cursor does Not move to the next line, And the scroll operation Is Not performed. The written character will be printed in the final position on the line And the cursor will remain above this character as if ENABLE_WRAP_AT_EOL_OUTPUT was off, but the next printable character will be printed as if ENABLE_WRAP_AT_EOL_OUTPUT Is on. No overwrite will occur. Specifically, the cursor quickly advances down to the following line, a scroll Is performed if necessary, the character Is printed, And the cursor advances one more position.
		''' 
		''' The typical usage Of this flag Is intended In conjunction With setting ENABLE_VIRTUAL_TERMINAL_PROCESSING To better emulate a terminal emulator where writing the final character On the screen (../In the bottom right corner) without triggering an immediate scroll Is the desired behavior.
		''' </summary>
		DisableNewlineAutoReturn = &H8
		''' <summary>
		''' The APIs For writing character attributes including WriteConsoleOutput And WriteConsoleOutputAttribute allow the usage Of flags from character attributes To adjust the color Of the foreground And background Of text. Additionally, a range Of DBCS flags was specified With the COMMON_LVB prefix. Historically, these flags only functioned In DBCS code pages For Chinese, Japanese, And Korean languages.
		''' 
		'''   With exception Of the leading Byte And trailing Byte flags, the remaining flags describing line drawing And reverse video (../swap foreground And background colors) can be useful For other languages To emphasize portions Of output.
		'''   
		''' Setting this console mode flag will allow these attributes To be used In every code page On every language.
		''' 
		''' 
		''' It Is off by default to maintain compatibility with known applications that have historically taken advantage of the console ignoring these flags on non-CJK machines to store bits in these fields for their own purposes Or by accident.
		''' 
		''' Note that Using the ENABLE_VIRTUAL_TERMINAL_PROCESSING mode can result In LVB grid And reverse video flags being Set While this flag Is still off If the attached application requests underlining Or inverse video via Console Virtual Terminal Sequences.
		''' </summary>
		EnableLvbGridWorldwide = &H10
	End Enum

enum StdHandles as long
		''' <summary>
		''' The standard input device. Initially, this Is the console input buffer
		''' </summary>
		 StdInputHandle = UInteger.MaxValue - 10

		''' <summary>
		''' 	The standard output device. Initially, this Is the active console screen buffer
		''' </summary>
		 StdOutputHandle = UInteger.MaxValue - 11

		''' <summary>
		''' 	The standard Error device. Initially, this Is the active console screen buffer
		''' </summary>
		 StdErrorHandle = UInteger.MaxValue - 12
	
	InvalidHandleValue = -1
	End enum
	


	Declare Auto Function GetStdHandle Lib "kernel32.dll"(stdHandle As StdHandles) As ConsoleHandle

	Declare Auto Function GetConsoleMode Lib "kernel32.dll"(
		ByVal hConsoleHandle As ConsoleHandle,
		ByRef lpMode As ConsoleModes
		) As Boolean
	Declare Auto Function SetConsoleMode Lib "kernel32.dll"(
		ByVal hConsoleHandle As ConsoleHandle,
		ByVal lpMode As ConsoleModes
		) As Boolean
End Class