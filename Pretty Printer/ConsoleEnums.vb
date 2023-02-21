Imports System.ComponentModel

Partial Public Class PrettyConsole
	Public Enum PrintUnit
		Character
		Word
		Line
	End Enum
	Private Function IsPrintUnitBreakpoint(printunit As PrintUnit, value As Char) As Boolean
		Select Case printunit
			Case PrintUnit.Character
				Return value <> Nothing
			Case PrintUnit.Word
				Return value <> Nothing AndAlso String.IsNullOrWhiteSpace(value)
			Case PrintUnit.Line
				Return value = Environment.NewLine
			Case Else
				Throw New InvalidEnumArgumentException("Invalid enum value", 1, printunit.GetType)
		End Select
	End Function
	

	
End Class
