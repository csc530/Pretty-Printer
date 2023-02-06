Partial Public Class PrettyConsole
	Public Enum PrintUnit
		Character
		Word
		Line
	End Enum
	Private Enum Colour
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
	Private Enum ColourModifier
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
End Class
