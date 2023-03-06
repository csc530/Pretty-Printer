Imports System.Drawing


Public Class TestColourData

	Public Shared ReadOnly ROYGBIV As Color() = {Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet}
	Public Shared ReadOnly UniqueNamed As Color() = {Color.OrangeRed, Color.DarkOrange, Color.LightGoldenrodYellow, Color.LawnGreen, Color.DodgerBlue, Color.Khaki, Color.PaleVioletRed}
	Public Shared ReadOnly customColours As Color() = {Color.FromArgb(0, 12, 212), Color.FromArgb(122, 88, 91), Color.FromArgb(100, 100, 100)}
	Public Shared ReadOnly Iterator Property Rainbow As IEnumerable(Of Object())
		Get
			For Each colour In ROYGBIV
				Yield {colour}
			Next
		End Get
	End Property

	Public Shared ReadOnly Iterator Property CustomColour As IEnumerable(Of Object())
		Get
			For Each colour In customColours
				Yield {colour}
			Next
		End Get
	End Property
	Public Shared ReadOnly Iterator Property UniqueNamedColours As IEnumerable(Of Object())
		Get
			For Each colour In UniqueNamed
				Yield {colour}
			Next
		End Get
	End Property

End Class
