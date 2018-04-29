Module I2C


    Public Function GetEEPROMString(eeprom_address As Integer, base_address As Integer, maxlen As Integer) As String
        Dim tmp As String
        For i = 0 To maxlen - 1
            Dim valore As Integer = 0
            I2CEEpromRead(eeprom_address, base_address + i, valore)
            If valore = 0 Then
                Exit For
            End If
            tmp = tmp & Chr(valore)
        Next
        Return tmp
    End Function


    Public Function ReadAFEBoardParametersI2C(ByRef SN_AFE As String, ByRef fitBias_min As Double, ByRef fitBias_max As Double) As Boolean
        Dim tmp
        Dim bias As Double = 0
        Dim tmp32 As Integer
        I2CEEpromRead(&H50, &H10, tmp)
        tmp32 = (tmp And &HFF) << 8
        I2CEEpromRead(&H50, &H11, tmp)
        fitBias_min = (tmp32 + (tmp And &HFF)) / 100

        I2CEEpromRead(&H50, &H12, tmp)
        tmp32 = (tmp And &HFF) << 8
        I2CEEpromRead(&H50, &H13, tmp)
        fitBias_max = (tmp32 + (tmp And &HFF)) / 100


        SN_AFE = GetEEPROMString(&H50, 0, 8)

        If SN_AFE.StartsWith("NI07") Then
            Return True
        Else
            SN_AFE = "none"
            fitBias_min = 51
            fitBias_max = 60
            Return False
        End If
    End Function


End Module
