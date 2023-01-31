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
	Public Property VirtualSequence() As List(Of String)
	Private Function GetVirtualSequences() As String
		Dim sequences = New StringBuilder()
		For Each sequence In VirtualSequence
			sequences.Append(sequence)
		Next
		VirtualSequence.Clear()
		Return sequences.ToString()
	End Function
	Private _BackgroundColour As Color

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

		REM Background colour
		' https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences#text-formatting
		If BackgroundColour = Nothing Then
			VirtualSequence.Add($"{SequenceStart}0m")
		ElseIf IsConsoleColour(BackgroundColour) Then
			Console.BackgroundColor = [Enum].Parse(Of ConsoleColor)(BackgroundColour.Name)
		Else
			VirtualSequence.Add($"{SequenceStart}48;2;{BackgroundColour.R};{BackgroundColour.G};{BackgroundColour.B}m")
		End If

	End Sub

	Sub New()
		VirtualSequence = New List(Of String)
	End Sub



	''' <summary>Resets this instance.</summary>
	Sub Reset()
		VirtualSequence.Clear()
		BackgroundColour = Nothing


		Console.ResetColor()
		Console.Write(GetVirtualSequences)
	End Sub


#Region "Print"
	Sub PrintLine(value As String)
		Console.WriteLine(GetVirtualSequences() & value)
	End Sub


	Sub Print(value As String)
		Console.Write(GetVirtualSequences() & value)
	End Sub
#End Region

#Region "Colour"


	Private Shared Function IsConsoleColour(colour As Color) As Boolean
		If colour.Name = "0" Then
			Return False
		Else Return [Enum].TryParse(Of ConsoleColor)(colour.Name, True, Nothing)
		End If

	End Function
#Region "Text"

#End Region
#End Region


End Class
