Option Strict On

Imports System.Drawing

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
		NONE = 0
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

	Public ReadOnly SequenceStart As String = Convert.ToChar(ConsoleKey.Escape) & "["
	Public Property VirtualSequence() As String

	Public Property BackgroundColour As Color
		Get
			Return _BackgroundColour
		End Get
		Set(value As Color)
			_BackgroundColour = value
			UpdateVirtualSequence()
		End Set
	End Property

	Private Sub UpdateVirtualSequence()
		Dim sequences = New List(Of String)

		REM Background colour
		' https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences#text-formatting
		If IsConsoleColour(BackgroundColour) Then
			Console.BackgroundColor = [Enum].Parse(Of ConsoleColor)(BackgroundColour.Name)
		Else
			sequences.Add($"{SequenceStart}48;2;{BackgroundColour.R};{BackgroundColour.G};{BackgroundColour.B}m")
		End If

		sequences.ForEach(Sub(sequence) Console.Write(sequence))
	End Sub

	REM Settings
	Private _BackgroundColour As Color

	''' <summary>Resets this instance.</summary>
	Sub Reset()
		VirtualSequence = Nothing
		ResetBackground()

		' print reset
		Dim sequences = New List(Of String)
		REM colour
		sequences.Add(SequenceStart & "0m")

		sequences.ForEach(Sub(sequence) Console.Write(sequence))
	End Sub


#Region "Print"
#Region "New Line"
	Sub PrintLine(value As String)
		Console.WriteLine(VirtualSequence & value)
	End Sub
#End Region
#Region "no line"
	Sub Print(value As String)
		Console.Write(VirtualSequence & value)
	End Sub
#End Region
#End Region

#Region "Colour"


	Private Function IsConsoleColour(colour As Color) As Boolean
		If colour.Name = "0" Then
			Return False
		Else Return [Enum].TryParse(Of ConsoleColor)(colour.Name, True, Nothing)
		End If

	End Function


	Public Sub ResetBackground()
		BackgroundColour = Nothing
	End Sub
#Region "Text"

#End Region
#End Region


End Class
