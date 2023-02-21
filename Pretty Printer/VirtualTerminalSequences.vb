Imports csc530.Pretty.VirtualTerminalSequences
Imports System.Text
Imports Microsoft.VisualBasic
Imports System.Drawing
Imports csc530.Pretty.ConsoleColour

Public MustInherit Class VirtualTerminalSequences
    ''' <summary>
    ''' Starting escape sequence to enter virtual terminal sequences
    ''' </summary>
    Public Shared ReadOnly Property Start As String = $"{Convert.ToChar(ConsoleKey.Escape)}["
    Enum ConsoleVirtualTerminalSequence
        'https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences#simple-cursor-positioning
        SimpleCursorPositioning
        'https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences#simple-cursor-positioning
        CursorPositioning
        'https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences#cursor-visibility
        CursorVisibility
        'https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences#cursor-shape
        CursorShape
        'https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences#viewport-positioning
        ViewportPositioning
        'https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences#text-modification
        TextUnderline
        'https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences#text-formatting
        TextBackground
        TextColour
    End Enum

    ''' <summary>
    ''' The virtual terminal sequences to modify the console output
    ''' </summary>
    ''' <returns>A string of the vts to modify subsequent outputs</returns>
    Private Function GetVirtualSequences(virtualSequences As Dictionary(Of ConsoleVirtualTerminalSequence, Object)) As String
        Dim sequences = New StringBuilder()
        For Each sequence In virtualSequences.Keys
            sequences.Append(Start)
            Select Case sequence
                Case ConsoleVirtualTerminalSequence.SimpleCursorPositioning

                Case ConsoleVirtualTerminalSequence.CursorPositioning

                Case ConsoleVirtualTerminalSequence.CursorVisibility

                Case ConsoleVirtualTerminalSequence.CursorShape

                Case ConsoleVirtualTerminalSequence.ViewportPositioning

                Case ConsoleVirtualTerminalSequence.TextUnderline
                Case ConsoleVirtualTerminalSequence.TextBackground
                Case ConsoleVirtualTerminalSequence.TextColour
                Case Else
            End Select
        Next
        Return sequences.ToString()
    End Function
    Public Shared ReadOnly ResetTextFormattingSequence As String = $"{Start}{0}m"
    Public Shared ReadOnly SwapForegroundBackgroundColours As String = $"{Start}{7}m"
    Public Shared ReadOnly ResetForegroundBackgroundColours As String = $"{Start}{27}m"



    Shared Function GetColourSequence(colour As ConsoleColour, layer As ConsoleLayer) As String
        Dim sequence As New StringBuilder(Start)
        Dim value = 30
        If layer = ConsoleLayer.Background Then
            value += 10
        End If

        Dim knownColour As KnownConsoleColour
        If colour Is Nothing Then
            sequence.Clear()
        Else
            If colour.IsKnownColour(knownColour) Then
                If colour.Bright Then
                    value += 60
                End If
                value += knownColour
                sequence.Append(value)
            ElseIf colour.Value = Nothing OrElse colour.Value.A = 0 Then
                value += 9
                sequence.Append(value)
            Else
                If colour.Bright Then
                    sequence.Append($"{1}m")
                Else
                    sequence.Append($"{22}m")
                End If

                value += 8
                sequence.Append($"{value};2;")
                Dim color = colour.Value
                sequence.Append($"{color.R};{color.G};{color.B}")
            End If
            sequence.Append("m"c)
        End If
        Return sequence.ToString
    End Function

    Public Shared Function GetUnderlinedSequence(underlined As Boolean) As String
        If underlined Then
            Return $"{Start}{4}m"
        Else
            Return $"{Start}{24}m"
        End If
    End Function

    Enum ConsoleLayer
        Background = 1
        Foreground = 2
    End Enum

End Class
