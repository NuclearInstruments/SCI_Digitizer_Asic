Module Translation
    Dim sipm_mapping() As Integer = {13, 4, 55, 46, 51, 59, 60, 15, 31, 62, 7, 36, 44, 5, 52, 37, 40, 32, 11, 35, 2, 48, 10, 18, 45, 39, 47, 26, 19, 34, 3, 29, 9, 1, 16, 41, 30, 42, 56, 50, 27, 22, 24, 0, 57, 8, 58, 25, 61, 63, 6, 54, 38, 20, 21, 28, 49, 43, 33, 17, 53, 14, 12, 23}
    Public Function TranslatePosition(channel As Integer) As Integer
        Return sipm_mapping(channel)
    End Function

    Public Function TranslatePositionInv(channel As Integer) As Integer
        For i = 0 To 63
            If channel = sipm_mapping(i) Then
                Return i
            End If
        Next
        ' Return sipm_mapping(channel)
    End Function
    Public MappingCH = {"A", "B", "C", "D", "E", "F", "G", "H"}
    Public Sub ChannelToLabel(channel As Integer, ByRef col As String, ByRef row As Integer)

        Dim ccol = Math.Floor(channel / 8)
        row = channel - (ccol) * 8 + 1
        col = MappingCH(ccol)
    End Sub
    Public Function TranslateADCSiPM(channel As Integer, board As Integer)
        If board = 0 Then
            Select Case channel
                Case 0
                    Return 40
                Case 1
                    Return 32
                Case 2
                    Return 11
                Case 3
                    Return 35
                Case 4
                    Return 2
                Case 5
                    Return 48
                Case 6
                    Return 10
                Case 7
                    Return 18
                Case 8
                    Return 45
                Case 9
                    Return 39
                Case 10
                    Return 47
                Case 11
                    Return 26
                Case 12
                    Return 19
                Case 13
                    Return 34
                Case 14
                    Return 3
                Case 15
                    Return 29
                Case 16
                    Return 13
                Case 17
                    Return 4
                Case 18
                    Return 55
                Case 19
                    Return 46
                Case 20
                    Return 51
                Case 21
                    Return 59
                Case 22
                    Return 60
                Case 23
                    Return 15
                Case 24
                    Return 31
                Case 25
                    Return 62
                Case 26
                    Return 7
                Case 27
                    Return 36
                Case 28
                    Return 44
                Case 29
                    Return 5
                Case 30
                    Return 52
                Case 31
                    Return 37
            End Select
        End If

        If board = 1 Then
            Select Case channel
                Case 0
                    Return 40
                Case 1
                    Return 32
                Case 2
                    Return 11
                Case 3
                    Return 35
                Case 4
                    Return 2
                Case 5
                    Return 48
                Case 6
                    Return 20
                Case 7
                    Return 18
                Case 8
                    Return 45
                Case 9
                    Return 39
                Case 10
                    Return 47
                Case 11
                    Return 26
                Case 12
                    Return 19
                Case 13
                    Return 34
                Case 14
                    Return 3
                Case 15
                    Return 29
                Case 16
                    Return 13
                Case 17
                    Return 4
                Case 18
                    Return 55
                Case 19
                    Return 46
                Case 20
                    Return 51
                Case 21
                    Return 59
                Case 22
                    Return 60
                Case 23
                    Return 15
                Case 24
                    Return 31
                Case 25
                    Return 62
                Case 26
                    Return 7
                Case 27
                    Return 36
                Case 28
                    Return 44
                Case 29
                    Return 5
                Case 30
                    Return 52
                Case 31
                    Return 53
            End Select
        End If
    End Function


    Public Sub TranslateSiPMDAC(channel As Integer, ByRef DAC_IC As Integer, ByRef DAC_CH As Integer)

        Select Case channel
            Case 0
                DAC_IC = 0
                DAC_CH = 15
            Case 1
                DAC_IC = 0
                DAC_CH = 6
            Case 2
                DAC_IC = 3
                DAC_CH = 0
            Case 3
                DAC_IC = 3
                DAC_CH = 5
            Case 4
                DAC_IC = 1
                DAC_CH = 10
            Case 5
                DAC_IC = 1
                DAC_CH = 4
            Case 6
                DAC_IC = 2
                DAC_CH = 7
            Case 7
                DAC_IC = 1
                DAC_CH = 14
            Case 8
                DAC_IC = 0
                DAC_CH = 8
            Case 9
                DAC_IC = 0
                DAC_CH = 2
            Case 10
                DAC_IC = 3
                DAC_CH = 7
            Case 11
                DAC_IC = 3
                DAC_CH = 11
            Case 12
                DAC_IC = 2
                DAC_CH = 9
            Case 13
                DAC_IC = 1
                DAC_CH = 2
            Case 14
                DAC_IC = 2
                DAC_CH = 6
            Case 15
                DAC_IC = 1
                DAC_CH = 6
            Case 16
                DAC_IC = 0
                DAC_CH = 9
            Case 17
                DAC_IC = 2
                DAC_CH = 8
            Case 18
                DAC_IC = 3
                DAC_CH = 2
            Case 19
                DAC_IC = 3
                DAC_CH = 14
            Case 20
                DAC_IC = 2
                DAC_CH = 1
            Case 21
                DAC_IC = 2
                DAC_CH = 2
            Case 22
                DAC_IC = 0
                DAC_CH = 13
            Case 23
                DAC_IC = 2
                DAC_CH = 13
            Case 24
                DAC_IC = 0
                DAC_CH = 14
            Case 25
                DAC_IC = 0
                DAC_CH = 4
            Case 26
                DAC_IC = 3
                DAC_CH = 8
            Case 27
                DAC_IC = 0
                DAC_CH = 12
            Case 28
                DAC_IC = 2
                DAC_CH = 3
            Case 29
                DAC_IC = 3
                DAC_CH = 13
            Case 30
                DAC_IC = 0
                DAC_CH = 7
            Case 31
                DAC_IC = 1
                DAC_CH = 13
            Case 32
                DAC_IC = 3
                DAC_CH = 15
            Case 33
                DAC_IC = 2
                DAC_CH = 12
            Case 34
                DAC_IC = 3
                DAC_CH = 10
            Case 35
                DAC_IC = 3
                DAC_CH = 3
            Case 36
                DAC_IC = 1
                DAC_CH = 15
            Case 37
                DAC_IC = 1
                DAC_CH = 9
            Case 38
                DAC_IC = 2
                DAC_CH = 0
            Case 39
                DAC_IC = 3
                DAC_CH = 4
            Case 40
                DAC_IC = 3
                DAC_CH = 6
            Case 41
                DAC_IC = 0
                DAC_CH = 1
            Case 42
                DAC_IC = 0
                DAC_CH = 3
            Case 43
                DAC_IC = 2
                DAC_CH = 4
            Case 44
                DAC_IC = 1
                DAC_CH = 12
            Case 45
                DAC_IC = 3
                DAC_CH = 9
            Case 46
                DAC_IC = 1
                DAC_CH = 1
            Case 47
                DAC_IC = 3
                DAC_CH = 12
            Case 48
                DAC_IC = 3
                DAC_CH = 1
            Case 49
                DAC_IC = 2
                DAC_CH = 5
            Case 50
                DAC_IC = 0
                DAC_CH = 10
            Case 51
                DAC_IC = 1
                DAC_CH = 7
            Case 52
                DAC_IC = 1
                DAC_CH = 0
            Case 53
                DAC_IC = 2
                DAC_CH = 14
            Case 54
                DAC_IC = 2
                DAC_CH = 10
            Case 55
                DAC_IC = 1
                DAC_CH = 5
            Case 56
                DAC_IC = 0
                DAC_CH = 11
            Case 57
                DAC_IC = 0
                DAC_CH = 5
            Case 58
                DAC_IC = 0
                DAC_CH = 0
            Case 59
                DAC_IC = 1
                DAC_CH = 3
            Case 60
                DAC_IC = 1
                DAC_CH = 11
            Case 61
                DAC_IC = 2
                DAC_CH = 11
            Case 62
                DAC_IC = 1
                DAC_CH = 8
            Case 63
                DAC_IC = 2
                DAC_CH = 15
        End Select


    End Sub
End Module
