Imports System.Drawing

Partial Class PrettyConsole
	Public Sub AlternateColourPrintLine(value As String, Optional primaryColour As Color = Nothing, Optional secondaryColour As Color = Nothing)
		AlternateColourPrint(value & Environment.NewLine, primaryColour, secondaryColour)
	End Sub


	Public Sub AlternateColourPrint(value As String, Optional primaryColour As Color = Nothing,
									Optional secondaryColour As Color = Nothing)
		If primaryColour = secondaryColour Then
			Print(value, textColour:=primaryColour)
			Exit Sub
		End If

		'unset colours are set to the current textcolour
		If primaryColour = Nothing Then
			primaryColour = TextColour
		End If
		If secondaryColour = Nothing Then
			secondaryColour = TextColour
		End If

		Dim colours = {primaryColour, secondaryColour}
		For i = 0 To value.Length - 1
			Print(value(i), textColour:=colours(i Mod 2))
		Next
	End Sub

	Sub AlternateBackgroundPrintLine(value As String, Optional primaryColour As Color = Nothing,
									 Optional secondaryColour As Color = Nothing)
		AlternateBackgroundPrint(value & Environment.NewLine, primaryColour, secondaryColour)
	End Sub


	Public Sub AlternateBackgroundPrint(value As String, Optional primaryColour As Color = Nothing,
										Optional secondaryColour As Color = Nothing)
		If primaryColour = secondaryColour Then
			Print(value, backgroundColour:=primaryColour)
			Exit Sub
		End If

		'unset colours are set to the current textcolour
		If primaryColour = Nothing Then
			primaryColour = TextColour
		End If
		If secondaryColour = Nothing Then
			secondaryColour = TextColour
		End If

		Dim colours = {primaryColour, secondaryColour}
		For i = 0 To value.Length - 1
			Print(value(i), backgroundColour:=colours(i Mod 2))
		Next
	End Sub


	Private Sub _SimpleAlternatePrint(value As String,
									  Optional backgroundColours As Color() = Nothing,
									  Optional textColours As Color() = Nothing)
		If IsNothing(textColours) And IsNothing(backgroundColours) Then
			Print(value)
			Exit Sub
		End If

		REM the checks for not isnothing(*) are superfluous from the above conditional but are added for clarity as well as the last for isnothing(y) for the same reason
		If textColours IsNot Nothing AndAlso textColours.Length = 2 AndAlso IsNothing(backgroundColours) Then
			AlternateColourPrint(value, textColours(0), textColours(1))
		ElseIf Not IsNothing(backgroundColours) AndAlso backgroundColours.Length = 2 AndAlso IsNothing(textColours) Then
			AlternateBackgroundPrint(value, backgroundColours(0), backgroundColours(1))
		Else
			If textColours Is Nothing Then
				textColours = {TextColour}
			End If
			If backgroundColours Is Nothing Then
				backgroundColours = {BackgroundColour}
			End If
			_AlternateBackgroundPrint(value, textColours, backgroundColours)

		End If
	End Sub

	Private Sub _AlternateBackgroundPrint(ByVal value As String, ByVal textColours As Color(), backgroundColours As Color())
		Dim bgColour, txtColour As Color
		For i = 0 To value.Length - 1
			bgColour = backgroundColours(i Mod backgroundColours.Length)
			txtColour = textColours(i Mod textColours.Length)
			Print(value(i).ToString, bgColour, txtColour)
		Next
	End Sub

	Public Sub AlternatePrint(value As String,
							  Optional backgroundColours As Color() = Nothing,
							  Optional textColours As Color() = Nothing,
							  Optional backgroundInterval As Integer = 1,
							  Optional backgroundIntervalUnit As PrintUnit = PrintUnit.Character,
							  Optional textInterval As Integer = 1,
							  Optional textIntervalUnit As PrintUnit = PrintUnit.Character)
		REM check if the complex options are left as their defaults
		If _
			backgroundInterval = 1 AndAlso backgroundIntervalUnit = PrintUnit.Character AndAlso
			textIntervalUnit = PrintUnit.Character AndAlso textInterval = 1 Then
			_SimpleAlternatePrint(value, backgroundColours, textColours)
		Else
			_ComplexAlternatePrint(value, backgroundColours, backgroundInterval, backgroundIntervalUnit, textColours,
								   textInterval,
								   textIntervalUnit)
		End If
	End Sub

	Public Sub AlternatePrintLine(value As String,
								  Optional backgroundColours As Color() = Nothing,
								  Optional textColours As Color() = Nothing,
								  Optional backgroundInterval As Integer = 1,
								  Optional backgroundIntervalUnit As PrintUnit = PrintUnit.Character,
								  Optional textInterval As Integer = 1,
								  Optional textIntervalUnit As PrintUnit = PrintUnit.Character)
		value &= Environment.NewLine
		AlternatePrint(value, backgroundColours, textColours, backgroundInterval, backgroundIntervalUnit, textInterval, textIntervalUnit)
	End Sub

	Private Sub _ComplexAlternatePrint(value As String, Optional backgroundColours As Color() = Nothing,
									   Optional backgroundInterval As Integer = 1,
									   Optional backgroundIntervalUnit As PrintUnit = PrintUnit.Character,
									   Optional textColours As Color() = Nothing, Optional textInterval As Integer = 1,
									   Optional textIntervalUnit As PrintUnit = PrintUnit.Character)
		If backgroundColours Is Nothing AndAlso textColours Is Nothing Then
			Print(value)
		End If

		If textColours Is Nothing Then
			textColours = {TextColour}
		End If
		REM explicit assignment to backgroundColours; could have been an else for intellisense/IDE noise
		' if textColours is nothing then backgroundColours must be nothing
		If backgroundColours Is Nothing Then
			backgroundColours = {BackgroundColour}
		End If

		Dim bgCounter = backgroundInterval
		Dim txtCounter = textInterval
		Dim bgColour, txtColour As Color
		Dim bgIndex = -1
		Dim txtIndex = -1
		For i = 0 To value.Length - 1
			Dim currentChar As Char = value(i)

			If IsPrintUnitBreakpoint(textIntervalUnit, currentChar) Then
				txtCounter -= 1
			End If
			If IsPrintUnitBreakpoint(backgroundIntervalUnit, currentChar) Then
				bgCounter -= 1
			End If

			If bgCounter = 0 Then
				bgIndex += 1
				bgColour = backgroundColours(bgIndex Mod backgroundColours.Length)
			End If
			If txtCounter = 0 Then
				txtIndex += 1
				txtColour = textColours(txtIndex Mod textColours.Length)
			End If

			Print(currentChar, bgColour, txtColour)
		Next
	End Sub
End Class