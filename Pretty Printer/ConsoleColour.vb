Imports System.Drawing
Imports System.Text

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
		Me.New
		Me.Bright = bright
	End Sub

	Sub New(value As Color, Optional bright As Boolean = False)
		Me.New(bright)
		Me.Value = value
	End Sub

	Public Sub New()
		Bright = Nothing
		Value = Color.Empty
	End Sub

	Public Overrides Function Equals(obj As Object) As Boolean
		If obj.GetType = GetType(ConsoleColour) Then
			Return CType(obj, ConsoleColour) = Me
		ElseIf obj.GetType = GetType(ConsoleColor) Then
			Return Me = CType(obj, ConsoleColor)
		ElseIf obj.GetType = GetType(Color) Then
			Return Me = CType(obj, Color)
		Else
			Return False
		End If
	End Function

	Public Overrides Function ToString() As String
		If Value = Nothing OrElse Value = Color.Empty Then
			Return "Default (Colourless)"
		End If
		Dim sb As New StringBuilder
		If Bright Then
			sb.Append("Bright")
			sb.Append(" "c)
		End If
		If Value.IsNamedColor Then
			sb.Append(Value.Name)
		Else
			sb.Append($"{{{Value.R}, {Value.G}, {Value.B}}}")
		End If
		Return sb.ToString
	End Function


	Shared Operator =(left As ConsoleColour, right As Color) As Boolean
		If left Is Nothing Then
			Return right = Nothing
		ElseIf right = Nothing Then
			Return left Is Nothing
		Else
			Return Not left.Bright AndAlso left.Value = right
		End If
	End Operator

	Shared Operator <>(left As ConsoleColour, right As Color) As Boolean
		If left Is Nothing Then
			Return right <> Nothing
		ElseIf right = Nothing Then
			Return left IsNot Nothing
		Else
			Return left.Bright OrElse left.Value <> right
		End If
	End Operator

	Public Shared Operator =(left As ConsoleColour, right As ConsoleColour) As Boolean
		If left Is Nothing Then
			Return right Is Nothing
		ElseIf right Is Nothing Then
			Return left Is Nothing
		Else
			Return left.Value = right.Value AndAlso left.Bright = right.Bright
		End If
	End Operator

	Shared Operator <>(left As ConsoleColour, right As ConsoleColour) As Boolean
		If left Is Nothing Then
			Return right IsNot Nothing
		ElseIf right Is Nothing Then
			Return left IsNot Nothing
		Else
			Return left.Value <> right.Value OrElse left.Bright <> right.Bright
		End If
	End Operator

	Public Shared Widening Operator CType(value As Color) As ConsoleColour
		Return New ConsoleColour(value)
	End Operator

	Public Shared Widening Operator CType(value As KnownColor) As ConsoleColour
		Return New ConsoleColour(Color.FromKnownColor(value))
	End Operator

	Public Shared Widening Operator CType(value As ConsoleColor) As ConsoleColour
		Dim result As ConsoleColor
		If [Enum].TryParse(value.ToString, result) Then
			If result = ConsoleColor.DarkYellow Then
				REM DArk yellow isn't a named colour for some reason so it's equivalent according to uncle google is ochre as defined below
				Dim ochre = Color.FromArgb(205, 105, 0)
				Return New ConsoleColour(ochre)
			End If
			Return New ConsoleColour(Color.FromName(result.ToString))
		Else
			'todo: decide best return type basically erroneous and impossible input
			'nothing, empty cc, or err
			Throw New ArgumentException("Invalid ConsoleColor", NameOf(value))
		End If
	End Operator

	'Decided on narrowing since th representation of bright can't be transferred to color
	Overloads Shared Narrowing Operator CType(value As ConsoleColour) As Color
		If value = Nothing Then
			Return Nothing
		Else
			Return value.Value
		End If
	End Operator

	Function IsKnownColour() As Boolean
		Return [Enum].TryParse(Of KnownConsoleColour)(Value.Name, Nothing)
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