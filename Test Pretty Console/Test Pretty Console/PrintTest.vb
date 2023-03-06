Imports System
Imports csc530.Pretty
Imports Xunit
Imports Xunit.Abstractions

Public Class PrintTest

        Private ReadOnly output As ITestOutputHelper
        Private ReadOnly prettyConsole As PrettyConsole

        Sub New(output As ITestOutputHelper)
            prettyConsole = New PrettyConsole()
            Me.output = output
        End Sub


        <Fact>
        Sub Print_TextColour_ReturnsToDefault()
            Dim originalColour = prettyConsole.TextColour
            prettyConsole.PrintLine("I'm testing the single use", textColour:=ConsoleColour.Black)
            Assert.Equal(originalColour, prettyConsole.TextColour)

        End Sub
    End Class

