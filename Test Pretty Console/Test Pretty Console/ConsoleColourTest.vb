Imports System
Imports System.Drawing
Imports System.Text.RegularExpressions
Imports csc530.Pretty
Imports Test_Pretty_Console.Trait
Imports Xunit
Imports Xunit.Abstractions
Imports Xunit.Sdk


Public Class ConsoleColourTest

	Shared ReadOnly Iterator Property ConstructorOptions As IEnumerable(Of Object())
		Get
			Dim brightOptions = {Nothing, False, True}
			Dim i = 0
			For Each colour As Object In TestColourData.customColours.Concat(TestColourData.ROYGBIV)
				Yield {colour, brightOptions(i Mod brightOptions.Length)}
				i += 1
			Next
		End Get
	End Property

	Shared ReadOnly Iterator Property nonequalColours As IEnumerable(Of Object())
		Get
			For i = 0 To TestColourData.ROYGBIV.Length - 1
				Yield {TestColourData.ROYGBIV(i), TestColourData.UniqueNamed(i)}
			Next
		End Get
	End Property

	Shared ReadOnly Iterator Property consolecolors As IEnumerable(Of Object())
		Get
			For Each colour In [Enum].GetValues(Of ConsoleColor)
				If colour <> ConsoleColor.DarkYellow Then
					'Because in Color does not have predefined 'dark yellow'
					Yield {colour}
				End If
			Next
		End Get
	End Property
	Shared ReadOnly Iterator Property knowncolors As IEnumerable(Of Object())
		Get
			For Each colour In [Enum].GetValues(Of KnownColor)
				Yield {colour}
			Next
		End Get
	End Property


#Region "Constructors"
	<Theory>
	<Trait(Category, Categories.Constructor)>
	<MemberData(NameOf(ConstructorOptions))>
	Public Sub Constructor_colour_bright(colour As Color, bright As Boolean)
		Dim consolecolour As New ConsoleColour(colour, bright)
		Assert.Equal(consolecolour.Value, colour)
	End Sub


	<Theory>
	<Trait(Trait.Category, Trait.Categories.Constructor)>
	<InlineData({Nothing})>
	<InlineData(True)>
	<InlineData(False)>
	Public Sub Constructor_Bright(value As Boolean)
		Dim consoleColour = New ConsoleColour(bright:=value)
		Assert.Equal(consoleColour.Bright, value)
	End Sub

	<Theory>
	<Trait(Trait.Category, Trait.Categories.Constructor)>
	<InlineData({Nothing})>
	<MemberData(NameOf(TestColourData.Rainbow), MemberType:=GetType(TestColourData))>
	Public Sub Constructor_Colour(colour As Color)
		Dim consoleColour = New ConsoleColour(colour)
		Assert.Equal(consoleColour.Value, colour)
	End Sub

	<Fact>
	<Trait(Trait.Category, Trait.Categories.Constructor)>
	Sub Constructor_Empty()
		Dim consoleColour = New ConsoleColour
		Assert.Equal(consoleColour.Value, Color.Empty)
		Assert.Equal(consoleColour.Bright, False)
		Assert.NotNull(consoleColour)
	End Sub
#End Region
#Region "InEquality"


	<Theory>
	<MemberData(NameOf(ConstructorOptions))>
	<Trait(Category, Categories.equality)>
	Sub Null_Comparison(colour As Color, bright As Boolean)
		Dim consolecolour As New ConsoleColour(colour, bright)
		Assert.NotEqual(Nothing, consolecolour)
	End Sub
	<Theory>
	<Trait(Category, Categories.equality)>
	<Trait([Return], Returns.unequal)>
	<MemberData(NameOf(nonequalColours))>
	Sub Inequal_Colour_Equal_Bright(colour As Color, differentColour As Color)
		Dim consolecolour As New ConsoleColour(colour)
		Dim differentConsoleColour As New ConsoleColour(differentColour)
		Assert.NotEqual(differentConsoleColour, consolecolour)
	End Sub
	<Theory>
	<Trait(Category, Categories.equality)>
	<Trait([Return], Returns.unequal)>
	<InlineData({True, False})>
	<InlineData({Nothing, True})>
	Sub Equal_Colour_Unequal_Bright(bright As Boolean, differentBright As Boolean)
		Dim consolecolour As New ConsoleColour(bright)
		Dim differentConsoleColour As New ConsoleColour(differentBright)
		Assert.NotEqual(differentConsoleColour, consolecolour)
	End Sub

#End Region

#Region "Operator inequality"
	<Theory>
	<MemberData(NameOf(ConstructorOptions))>
	<Trait(Category, Categories.equality)>
	<Trait(Category, Categories.operator)>
	<Trait([Return], Returns.unequal)>
	Sub Operator_Null_Comparison(colour As Color, bright As Boolean)
		Dim consolecolour As New ConsoleColour(colour, bright)
		Dim result = consolecolour Is Nothing
		Assert.False(result)
		result = consolecolour = Nothing
		Assert.False(result)
	End Sub
	<Theory>
	<Trait(Category, Categories.equality)>
	<Trait(Category, Categories.operator)>
	<Trait([Return], Returns.unequal)>
	<MemberData(NameOf(nonequalColours))>
	Sub Operator_Inequal_Colour_Equal_Bright(colour As Color, differentColour As Color)
		Dim consolecolour As New ConsoleColour(colour)
		Dim differentConsoleColour As New ConsoleColour(differentColour)
		Dim result = consolecolour <> differentConsoleColour
		Assert.True(result)
		result = consolecolour = differentConsoleColour
		Assert.False(result)
	End Sub
	<Theory>
	<Trait(Category, Categories.equality)>
	<Trait([Return], Returns.unequal)>
	<InlineData({True, False})>
	<InlineData({Nothing, True})>
	Sub Operator_Equal_Colour_Unequal_Bright(bright As Boolean, differentBright As Boolean)
		Dim consolecolour As New ConsoleColour(bright)
		Dim differentConsoleColour As New ConsoleColour(differentBright)
		Dim result = consolecolour = differentConsoleColour
		Assert.False(result)
		result = consolecolour <> differentConsoleColour
		Assert.True(result)
	End Sub
#End Region

#Region "Equality"
	<Fact>
	<Trait(Category, Categories.equality)>
	<Trait([Return], Returns.equal)>
	Sub Equality_EmptyConsoleColours()
		Dim consolecolour As New ConsoleColour()
		Dim differentConsoleColour As New ConsoleColour()
		Assert.Equal(differentConsoleColour, consolecolour)
	End Sub

	<Theory>
	<Trait(Category, Categories.equality)>
	<Trait([Return], Returns.equal)>
	<MemberData(NameOf(ConstructorOptions))>
	Sub Equality_ConsoleColours(colour As Color, bright As Boolean)
		Dim consolecolour As New ConsoleColour(colour, bright)
		Dim differentConsoleColour As New ConsoleColour(colour, bright)
		Assert.Equal(differentConsoleColour, consolecolour)
	End Sub



#End Region
#Region "Operator equality"
	<Fact>
	<Trait(Category, Categories.equality)>
	<Trait([Return], Returns.equal)>
	Sub Operator_Equality_EmptyConsoleColours()
		Dim consolecolour As New ConsoleColour()
		Dim differentConsoleColour As New ConsoleColour()
		Dim result = consolecolour = differentConsoleColour
		Assert.True(result)
		result = consolecolour <> differentConsoleColour
		Assert.False(result)
	End Sub

	<Theory>
	<Trait(Category, Categories.equality)>
	<Trait([Return], Returns.equal)>
	<MemberData(NameOf(ConstructorOptions))>
	Sub Operator_Equality_ConsoleColours(colour As Color, bright As Boolean)
		Dim consolecolour As New ConsoleColour(colour, bright)
		Dim differentConsoleColour As New ConsoleColour(colour, bright)
		Dim result = consolecolour = differentConsoleColour
		Assert.True(result)
		result = consolecolour <> differentConsoleColour
		Assert.False(result)
	End Sub

#End Region


#Region "Convert to Color equality"
	<Theory>
	<Trait(Category, Categories.equality)>
	<Trait([Return], Returns.equal)>
	<MemberData(NameOf(TestColourData.Rainbow), MemberType:=GetType(TestColourData))>
	<MemberData(NameOf(TestColourData.CustomColour), MemberType:=GetType(TestColourData))>
	<MemberData(NameOf(TestColourData.UniqueNamedColours), MemberType:=GetType(TestColourData))>
	Sub ConvertTo_Color_Equality(colour As Color)
		Dim consolecolour As New ConsoleColour(colour)
		Dim asColor As Color = consolecolour
		Assert.Equal(colour, asColor)
	End Sub


	<Fact>
	<Trait(Category, Categories.equality)>
	<Trait([Return], Returns.equal)>
	Sub ConvertTo_EmptyColor_Equality()
		Dim consolecolour As New ConsoleColour()
		Dim asColor As Color = consolecolour
		Assert.Equal(Color.Empty, asColor)
		Assert.Equal(Nothing, asColor)
	End Sub

	<Theory>
	<Trait(Category, Categories.equality)>
	<Trait([Return], Returns.equal)>
	<MemberData(NameOf(nonequalColours))>
	Sub ConvertTo_Color_InEquality(colour As Color, differntColour As Color)
		Dim consolecolour As New ConsoleColour(differntColour)
		Dim asColor As Color = consolecolour
		Assert.Equal(differntColour, asColor)
		Assert.NotEqual(colour, asColor)
		Assert.NotEqual(colour, differntColour)
	End Sub


	<Theory>
	<Trait(Category, Categories.equality)>
	<Trait([Return], Returns.equal)>
	<MemberData(NameOf(TestColourData.Rainbow), MemberType:=GetType(TestColourData))>
	Sub ConvertTo_EmptyColor_InEquality(colour As Color)
		Dim consolecolour As New ConsoleColour()
		Dim asColor As Color = consolecolour

		Assert.Equal(Color.Empty, asColor)
		Assert.Equal(Nothing, asColor)

		Assert.NotEqual(colour, asColor)
	End Sub
#End Region
#Region "Convert from Color Equality"
	<Theory>
	<MemberData(NameOf(TestColourData.Rainbow), MemberType:=GetType(TestColourData))>
	Sub ConvertFrom_Color_Equality(colour As Color)
		Dim consolecolour As ConsoleColour = colour
		Assert.Equal(colour, consolecolour.Value)
		Assert.Equal(False, consolecolour.Bright)
		REM convert the expected (colour) to ConsoleColour
		Assert.Equal(colour, consolecolour)
	End Sub


	<Fact>
	Sub ConvertFrom_EmptyColor_Equality()
		Dim consolecolour As ConsoleColour = Color.Empty
		Assert.Equal(Color.Empty, consolecolour.Value)
		Assert.Equal(False, consolecolour.Bright)
		Assert.Equal(Color.Empty, consolecolour)
	End Sub

	<Theory>
	<MemberData(NameOf(nonequalColours))>
	Sub ConvertFrom_Color_InEquality(colour As Color, differentColour As Color)
		Dim consolecolour As ConsoleColour = differentColour
		Assert.NotEqual(colour, differentColour)
		Assert.NotEqual(colour, consolecolour.Value)
		REM convert the expected (colour) to ConsoleColour
		Assert.NotEqual(colour, consolecolour)
	End Sub


	<Theory>
	<MemberData(NameOf(TestColourData.Rainbow), MemberType:=GetType(TestColourData))>
	Sub ConvertFrom_EmptyColor_InEquality(colour As Color)
		Dim consolecolour As ConsoleColour = Color.Empty
		Assert.NotEqual(colour, Color.Empty)
		Assert.NotEqual(colour, consolecolour.Value)
		Assert.NotEqual(colour, consolecolour)
	End Sub
#End Region

#Region "Conversion "

#End Region

#Region "Conversion from KnownColor equality"
	<Theory>
	<MemberData(NameOf(knowncolors))>
	<Trait(Category, Categories.conversion)>
	<Trait([Return], Returns.equal)>
	Sub ConvertFrom_KnownColor(knownColour As KnownColor)
		Dim consoleColour As ConsoleColour = knownColour
		Dim correctColour = Color.FromKnownColor(knownColour)
		Assert.Equal(correctColour, consoleColour.Value)
		Assert.Equal(knownColour.ToString, consoleColour.Value.Name)
		Assert.Equal(False, consoleColour.Bright)
	End Sub
#End Region

#Region "Defaults"
	<Trait(Category, Categories.default)>
	<Fact>
	Sub Default_Properies_value()
		Dim consoleColour As New ConsoleColour
		Assert.Equal(consoleColour.Value, Color.Empty)
		Assert.Equal(consoleColour.Value, Nothing)
	End Sub

	<Trait(Category, Categories.default)>
	<Fact>
	Sub Default_Properies_bright()
		Dim consoleColour As New ConsoleColour
		Assert.Equal(consoleColour.Bright, False)
		Assert.Equal(consoleColour.Bright, Nothing)
	End Sub
#End Region

End Class
