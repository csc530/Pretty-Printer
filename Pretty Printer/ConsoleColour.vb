Imports System.Drawing

Public Class ConsoleColour
    Shared ReadOnly Black As New ConsoleColour(Color.Black)
    Shared ReadOnly Red As New ConsoleColour(Color.Red)
    Shared ReadOnly Green As New ConsoleColour(Color.Green)
    Shared ReadOnly Yellow As New ConsoleColour(Color.Yellow)
    Shared ReadOnly Blue As New ConsoleColour(Color.Blue)
    Shared ReadOnly magenta As New ConsoleColour(Color.Magenta)
    Shared ReadOnly Cyan As New ConsoleColour(Color.Cyan)
    Shared ReadOnly White As New ConsoleColour(Color.White)
    Shared ReadOnly BrightBlack As New ConsoleColour(Color.Black, True)
    Shared ReadOnly BrightRed As New ConsoleColour(Color.Red, True)
    Shared ReadOnly BrightGreen As New ConsoleColour(Color.Green, True)
    Shared ReadOnly BrightYellow As New ConsoleColour(Color.Yellow, True)
    Shared ReadOnly BrightBlue As New ConsoleColour(Color.Blue, True)
    Shared ReadOnly Brightmagenta As New ConsoleColour(Color.Magenta, True)
    Shared ReadOnly BrightCyan As New ConsoleColour(Color.Cyan, True)
    Shared ReadOnly BrightWhite As New ConsoleColour(Color.White, True)


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
        Return value.Value
    End Operator

    Private Enum KnownConsoleColour
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
End Class