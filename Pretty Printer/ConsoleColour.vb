Imports System.Drawing

Public Class ConsoleColour
	Public Shared ReadOnly Black As New ConsoleColour(Color.Black)
	Public Shared ReadOnly Red As New ConsoleColour(Color.Red)
	Public Shared ReadOnly Green As New ConsoleColour(Color.Green)
	Public Shared ReadOnly Yellow As New ConsoleColour(Color.Yellow)
	Public Shared ReadOnly Blue As New ConsoleColour(Color.Blue)
	Public Shared ReadOnly Magenta As New ConsoleColour(Color.Magenta)
	Public Shared ReadOnly Cyan As New ConsoleColour(Color.Cyan)
	Public Shared ReadOnly White As New ConsoleColour(Color.White)
	Public Shared ReadOnly BrightBlack As New ConsoleColour(Color.Black, True)
	Public Shared ReadOnly BrightRed As New ConsoleColour(Color.Red, True)
	Public Shared ReadOnly BrightGreen As New ConsoleColour(Color.Green, True)
	Public Shared ReadOnly BrightYellow As New ConsoleColour(Color.Yellow, True)
	Public Shared ReadOnly BrightBlue As New ConsoleColour(Color.Blue, True)
	Public Shared ReadOnly BrightMagenta As New ConsoleColour(Color.Magenta, True)
	Public Shared ReadOnly BrightCyan As New ConsoleColour(Color.Cyan, True)
	Public Shared ReadOnly BrightWhite As New ConsoleColour(Color.White, True)


	Public Property Bright As Boolean
	Property Value As Color

	Sub New(Optional bright As Boolean = False)
		Me.Bright = bright
		Value = Color.Empty
	End Sub

	Sub New(value As Color, Optional bright As Boolean = False)
		Me.New(bright)
		Me.Value = value
	End Sub


	Shared Operator =(left As ConsoleColour, right As Color) As Boolean
		if left is nothing
			return right = nothing
		else if right = nothing
			return left is nothing
		else
			Return Not left.Bright AndAlso left.Value = right
		end if
	End Operator

	Shared Operator <>(left As ConsoleColour, right As Color) As Boolean
		if left is nothing
			return right <> nothing
		else if right = nothing
			return left isnot nothing
		else
			Return left.Bright OrElse left.Value <> right
		end if
	End Operator

	Public Shared Operator =(left As ConsoleColour, right As ConsoleColour) As Boolean
		if left is nothing
			return right is nothing
		else       if right is nothing
			return left is nothing
		else
			Return left.Value = right.Value AndAlso left.Bright = right.Bright
		End If
	End Operator

	Shared Operator <>(left As ConsoleColour, right As ConsoleColour) As Boolean
		if left is nothing
			return right isnot nothing
		else       if right is nothing
			return left isnot nothing
		else
			Return left.Value <> right.Value OrElse left.Bright <> right.Bright
		End If
	End Operator

	Public Shared Narrowing Operator CType(value As Color) As ConsoleColour
		Return New ConsoleColour(value)
	End Operator

	Public Shared Widening Operator CType(value As ConsoleColor) As ConsoleColour
		Dim result As KnownColor
		If [Enum].TryParse(value.ToString, result) Then
			Return New ConsoleColour(Color.FromKnownColor(result))
		Else
			'todo: decide best return type basically erroneous and impossible input
			'nothing, empty cc, or err
			Throw New ArgumentException("Invalid ConsoleColor", NameOf(value))
		End If
	End Operator

	Overloads Shared Widening Operator CType(value As ConsoleColour) As Color
		if value is nothing
			return nothing
		Else
			Return value.Value
		End If
	End Operator

	Function IsKnownColour() As Boolean
		Return IsKnownColour(Nothing)
	End Function

	Function IsKnownColour(ByRef knownColour As KnownConsoleColour) As Boolean
		Return Value.IsNamedColor AndAlso [Enum].TryParse (Of KnownConsoleColour)(Value.Name, knownColour)
	End Function

	Enum KnownConsoleColour
		Black
		Red
		Green
		Yellow
		Blue
		Magenta
		Cyan
		White
	End Enum
End Class