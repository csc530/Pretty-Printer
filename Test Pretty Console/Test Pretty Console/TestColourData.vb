Imports System.Drawing


Public Class TestColourData
	Public Shared ReadOnly Property SingleRainbow As IEnumerable(Of Object())
		Get
			Dim arr As List(Of Object()) = New List(Of Object())
			arr.Add({Color.Red})
			arr.Add({Color.Orange})
			arr.Add({Color.Green})
			arr.Add({Color.Blue})
			arr.Add({Color.Indigo})
			arr.Add({Color.Violet})
			REM custom colour
			arr.Add({Color.FromArgb(77, 72, 134)})
			arr.Add({Color.FromArgb(68, 212, 54)})
			arr.Add({Color.FromArgb(158, 62, 229)})

			Return arr

		End Get
	End Property

	Public Shared ReadOnly Iterator Property Rainbow As IEnumerable(Of Object())
		Get
			Yield Array.ConvertAll({Color.Red, Color.Orange, Color.Green, Color.Blue, Color.Indigo, Color.Violet}, AddressOf Converter)
		End Get
	End Property

	Private Shared Function Converter(color As Color) As Object

		Return CType(color, Object)
	End Function
End Class