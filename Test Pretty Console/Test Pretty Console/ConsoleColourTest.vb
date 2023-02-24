Imports System
Imports System.Drawing
Imports System.Text.RegularExpressions
Imports csc530.Pretty
Imports Xunit
Imports Xunit.Abstractions
Imports Xunit.Sdk

Namespace Test_Pretty_Console

    Public Class ConsoleColourTest

#Region "Constructors"
        <Theory>
        <Trait(Trait.Category, Trait.Categories.Constructor)>
        <InlineData({Nothing})>
        <MemberData(NameOf(TestColourData.SingleRainbow), MemberType:=GetType(TestColourData))>
        Public Sub Constructor_Value_Colour(colour As Color)
            Dim consoleColour = New ConsoleColour(colour)
            Assert.Equal(consoleColour.Value, colour)
        End Sub

        <Fact>
        <Trait(Trait.Category, Trait.Categories.Constructor)>
        Sub Constructor_Empty_Colour()
            Dim consoleColour = New ConsoleColour
            Assert.Equal(consoleColour.Value, Color.Empty)
            Assert.Equal(consoleColour.Bright, False)
            Assert.NotNull(consoleColour)
        End Sub
#End Region

        <Theory>
        <InlineData({Nothing})>
        <MemberData(NameOf(TestColourData.SingleRainbow), MemberType:=GetType(TestColourData))>
        Public Sub Assign_Value_Colour(colour As Color)
            Dim consoleColour = New ConsoleColour
            consoleColour.Value = colour
            Assert.Equal(consoleColour.Value, colour)
        End Sub


    End Class
End Namespace


