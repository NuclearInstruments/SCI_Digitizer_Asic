'Public Class pProperties

'    Dim lblBiasVar(63) As Label
'    Public nudBiasVar(63) As NumericUpDown

'    Dim lblGain(63) As Label
'    Public nudGain(63) As NumericUpDown

'    Dim lblOffset(63) As Label
'    Public nudOffset(63) As NumericUpDown


'    Dim lblThresholdTrigger(63) As Label
'    Public nThresholdTrigger(63) As NumericUpDown

'    Private Sub pProperties_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        Dim col, row
'        Polarity.Items.Add("POSITIVE")
'        Polarity.Items.Add("NEGATIVE")
'        Polarity.SelectedIndex = 0

'        trMode.Items.Add("DERIVATIVE")
'        trMode.Items.Add("EXTERNAL")
'        trMode.Items.Add("FRAME")
'        trMode.Items.Add("FRAME - MULTIBOARD")
'        '   trMode.Items.Add("THRESHOLD")

'        trMode.SelectedIndex = 0

'        blCor.Items.Add("NO")
'        blCor.Items.Add("8 SAMPLES")
'        blCor.Items.Add("16 SAMPLES")
'        blCor.Items.Add("32 SAMPLES")
'        blCor.Items.Add("64 SAMPLES")
'        blCor.Items.Add("SOA")
'        blCor.Items.Add("MANUAL")
'        blCor.SelectedIndex = 4

'        psyncmode.Items.Add("Event Counter")
'        psyncmode.Items.Add("Timecode")
'        psyncmode.SelectedIndex = 0

'        TabPage1.AutoScroll = True
'        TabPage2.AutoScroll = True
'        TabPage3.AutoScroll = True
'        TabPage6.AutoScroll = True
'        Dim t = 45
'        For i = 0 To 63 Step 2
'            lblBiasVar(i) = New Label
'            lblBiasVar(i + 1) = New Label
'            nudBiasVar(i) = New NumericUpDown
'            nudBiasVar(i + 1) = New NumericUpDown
'            ChannelToLabel(i, col, row)
'            lblBiasVar(i).Text = col & " - " & row
'            lblBiasVar(i).Top = t
'            lblBiasVar(i).Left = 19
'            lblBiasVar(i).Width = 40

'            nudBiasVar(i).Maximum = 3.2
'            nudBiasVar(i).DecimalPlaces = 4
'            nudBiasVar(i).Increment = 0.001
'            nudBiasVar(i).Top = t - 3
'            nudBiasVar(i).Left = lblBiasVar(i).Left + lblBiasVar(i).Width + 15
'            nudBiasVar(i).Width = 60
'            nudBiasVar(i).Value = 0
'            ChannelToLabel(i + 1, col, row)
'            lblBiasVar(i + 1).Text = col & " - " & row
'            lblBiasVar(i + 1).Top = t
'            lblBiasVar(i + 1).Left = 19 + 130
'            lblBiasVar(i + 1).Width = 40


'            nudBiasVar(i + 1).Maximum = 3.3
'            nudBiasVar(i + 1).DecimalPlaces = 4
'            nudBiasVar(i + 1).Increment = 0.001
'            nudBiasVar(i + 1).Top = t - 3
'            nudBiasVar(i + 1).Left = lblBiasVar(i + 1).Left + lblBiasVar(i + 1).Width + 15
'            nudBiasVar(i + 1).Width = 60
'            nudBiasVar(i + 1).Value = 0


'            TabPage1.Controls.Add(lblBiasVar(i))
'            TabPage1.Controls.Add(lblBiasVar(i + 1))

'            TabPage1.Controls.Add(nudBiasVar(i))
'            TabPage1.Controls.Add(nudBiasVar(i + 1))

'            t += lblBiasVar(i).Height + 5
'        Next


'        t = 10
'        For i = 0 To 63
'            lblGain(i) = New Label
'            lblOffset(i) = New Label
'            nudGain(i) = New NumericUpDown
'            nudOffset(i) = New NumericUpDown

'            lblGain(i).Text = "CH " & i + 1 & " Gain"
'            lblGain(i).Top = t
'            lblGain(i).Left = 19
'            lblGain(i).Width = 70

'            nudGain(i).Maximum = 10
'            nudGain(i).Value = 1
'            nudGain(i).DecimalPlaces = 4
'            nudGain(i).Increment = 0.001
'            nudGain(i).Top = t - 3
'            nudGain(i).Left = lblGain(i).Left + lblGain(i).Width + 10
'            nudGain(i).Width = 60

'            lblOffset(i).Text = "Offset"
'            lblOffset(i).Top = t
'            lblOffset(i).Left = 19 + 150
'            lblOffset(i).Width = 40


'            nudOffset(i).Maximum = 16383
'            nudOffset(i).DecimalPlaces = 0
'            nudOffset(i).Increment = 1
'            nudOffset(i).Top = t - 3
'            nudOffset(i).Left = lblOffset(i).Left + lblOffset(i).Width + 10
'            nudOffset(i).Width = 60


'            TabPage2.Controls.Add(lblGain(i))
'            TabPage2.Controls.Add(lblOffset(i))

'            TabPage2.Controls.Add(nudGain(i))
'            TabPage2.Controls.Add(nudOffset(i))

'            t += lblBiasVar(i).Height + 5
'        Next

'        t = 50
'        For i = 0 To 63 Step 2
'            lblThresholdTrigger(i) = New Label
'            lblThresholdTrigger(i + 1) = New Label
'            nThresholdTrigger(i) = New NumericUpDown
'            nThresholdTrigger(i + 1) = New NumericUpDown
'            ChannelToLabel(i, col, row)
'            lblThresholdTrigger(i).Text = col & " - " & row
'            lblThresholdTrigger(i).Top = t
'            lblThresholdTrigger(i).Left = 19
'            lblThresholdTrigger(i).Width = 40

'            nThresholdTrigger(i).Maximum = 16000
'            nThresholdTrigger(i).DecimalPlaces = 0
'            nThresholdTrigger(i).Increment = 10
'            nThresholdTrigger(i).Top = t - 3
'            nThresholdTrigger(i).Left = lblThresholdTrigger(i).Left + lblThresholdTrigger(i).Width + 15
'            nThresholdTrigger(i).Width = 60
'            nThresholdTrigger(i).Value = 0
'            ChannelToLabel(i + 1, col, row)
'            lblThresholdTrigger(i + 1).Text = col & " - " & row
'            lblThresholdTrigger(i + 1).Top = t
'            lblThresholdTrigger(i + 1).Left = 19 + 130
'            lblThresholdTrigger(i + 1).Width = 40


'            nThresholdTrigger(i + 1).Maximum = 16000
'            nThresholdTrigger(i + 1).DecimalPlaces = 0
'            nThresholdTrigger(i + 1).Increment = 10
'            nThresholdTrigger(i + 1).Top = t - 3
'            nThresholdTrigger(i + 1).Left = lblThresholdTrigger(i + 1).Left + lblThresholdTrigger(i + 1).Width + 15
'            nThresholdTrigger(i + 1).Width = 60
'            nThresholdTrigger(i + 1).Value = 0


'            TabPage6.Controls.Add(lblThresholdTrigger(i))
'            TabPage6.Controls.Add(lblThresholdTrigger(i + 1))

'            TabPage6.Controls.Add(nThresholdTrigger(i))
'            TabPage6.Controls.Add(nThresholdTrigger(i + 1))

'            t += lblThresholdTrigger(i).Height + 5
'        Next


'        rm.Columns.Clear()
'        rm.Rows.Clear()
'        rm.Columns.Add("CH", "CH")
'        rm.Columns.Add("IN", "IN")
'        rm.Columns.Add("OUT", "OUT")
'        rm.Columns.Add("DEAD", "DEAD")
'        For i = 0 To 63
'            rm.Rows.Add(i, 0, 0, 0)
'        Next

'        TabPage6.Refresh()

'    End Sub

'    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

'    End Sub

'    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click

'    End Sub



'    'Public Sub I2CDACWrite(chip_address As Integer, internal_address As Integer, value As Integer)
'    '    MainForm.board1.FPGAWriteReg((chip_address << 1) + 0 + (1 << 8) + (1 << 11), &H704)
'    '    System.Threading.Thread.Sleep(5)
'    '    MainForm.board1.FPGAWriteReg(internal_address + (1 << 11), &H704)
'    '    System.Threading.Thread.Sleep(5)
'    '    MainForm.board1.FPGAWriteReg(((value >> 8) And &HFF) + (3 << 6) + (1 << 11), &H704)
'    '    System.Threading.Thread.Sleep(5)
'    '    MainForm.board1.FPGAWriteReg(((value >> 0) And &HFF) + (1 << 9) + (1 << 11), &H704)
'    '    System.Threading.Thread.Sleep(5)
'    'End Sub

'    'Public Sub I2CDACCFG(chip_address As Integer)
'    '    MainForm.board1.FPGAWriteReg((chip_address << 1) + 0 + (1 << 8) + (1 << 11), &H704)
'    '    System.Threading.Thread.Sleep(5)
'    '    MainForm.board1.FPGAWriteReg(&HC + (1 << 11), &H704)
'    '    System.Threading.Thread.Sleep(5)
'    '    MainForm.board1.FPGAWriteReg(&H2F + (0 << 6) + (1 << 11), &H704)
'    '    System.Threading.Thread.Sleep(5)
'    '    MainForm.board1.FPGAWriteReg(0 + (1 << 9) + (1 << 11), &H704)
'    '    System.Threading.Thread.Sleep(5)
'    'End Sub

'    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
'        If bSeparateTrigger.Checked = False Then
'            'trigger level
'            For i = 0 To 31
'                MainForm.board1.FPGAWriteReg(trLevel.Value, &H450 + i)
'            Next

'            If MainForm.nboard > 1 Then
'                For i = 0 To 31
'                    MainForm.board2.FPGAWriteReg(trLevel.Value, &H450 + i)
'                Next
'            End If
'        Else
'            For j = 0 To 63
'                Dim a = TranslatePositionInv(j)
'                If a < 32 Then
'                    MainForm.board1.FPGAWriteReg(nThresholdTrigger(j).Value, &H450 + a)
'                Else
'                    If MainForm.nboard > 1 Then
'                        MainForm.board2.FPGAWriteReg(nThresholdTrigger(j).Value, &H450 + a - 32)
'                    End If
'                End If
'            Next

'            'separate trigger level

'        End If



'        'noise filter
'        If nf_b.Checked = True Then
'            MainForm.board1.FPGAWriteReg(1, &H706)
'        Else
'            MainForm.board1.FPGAWriteReg(0, &H706)
'        End If

'        If MainForm.nboard > 1 Then
'            If nf_b.Checked = True Then
'                MainForm.board2.FPGAWriteReg(1, &H706)
'            Else
'                MainForm.board2.FPGAWriteReg(0, &H706)
'            End If
'        End If


'        'integration time
'        MainForm.board1.FPGAWriteReg(Int(iTime.Value / MainForm.Ts), &H402)
'        If MainForm.nboard > 1 Then
'            MainForm.board2.FPGAWriteReg(Int(iTime.Value / MainForm.Ts), &H402)
'        End If

'        'pileup rejector enable
'        If prEn.Checked = True Then
'            MainForm.board1.FPGAWriteReg(1, &H404)
'        Else
'            MainForm.board1.FPGAWriteReg(0, &H404)
'        End If

'        If MainForm.nboard > 1 Then
'            If prEn.Checked = True Then
'                MainForm.board2.FPGAWriteReg(1, &H404)
'            Else
'                MainForm.board2.FPGAWriteReg(0, &H404)
'            End If
'        End If

'        'pileup rejector time
'        MainForm.board1.FPGAWriteReg(Int((iTime.Value + prTime.Value) / MainForm.Ts), &H403)
'        If MainForm.nboard > 1 Then
'            MainForm.board2.FPGAWriteReg(Int((iTime.Value + prTime.Value) / MainForm.Ts), &H403)
'        End If

'        'trigger holdoff
'        MainForm.board1.FPGAWriteReg(Int((trHold.Value) / MainForm.Ts), &H401)
'        If MainForm.nboard > 1 Then
'            MainForm.board2.FPGAWriteReg(Int((trHold.Value) / MainForm.Ts), &H401)
'        End If

'        MainForm.rdc.ScaleFactor = Int(iTime.Value / MainForm.Ts) / (dGain.Value)

'        'baseline correction
'        If blCor.SelectedIndex = 0 Or blCor.SelectedIndex = 6 Then
'            MainForm.board1.FPGAWriteReg(6, &H40C)
'            If blCor.SelectedIndex = 6 Then
'                MainForm.board1.FPGAWriteReg(blOffset.Value, &H409)
'            Else
'                MainForm.board1.FPGAWriteReg(0, &H409)
'            End If

'        Else
'            If blCor.SelectedIndex = 5 Then
'                MainForm.board1.FPGAWriteReg(7, &H40C)
'                MainForm.board1.FPGAWriteReg(blOffset.Value, &H409)
'                MainForm.board1.FPGAWriteReg(1, &H40C)
'                MainForm.board1.FPGAWriteReg(0, &H40C)
'            Else
'                MainForm.board1.FPGAWriteReg(blCor.SelectedIndex - 1, &H40C)
'                MainForm.board1.FPGAWriteReg(blOffset.Value, &H409)
'            End If
'        End If

'        If MainForm.nboard > 1 Then
'            If blCor.SelectedIndex = 0 Or blCor.SelectedIndex = 6 Then
'                MainForm.board2.FPGAWriteReg(6, &H40C)
'                If blCor.SelectedIndex = 6 Then
'                    MainForm.board2.FPGAWriteReg(blOffset.Value, &H409)
'                Else
'                    MainForm.board2.FPGAWriteReg(0, &H409)
'                End If

'            Else
'                If blCor.SelectedIndex = 5 Then
'                    MainForm.board2.FPGAWriteReg(7, &H40C)
'                    MainForm.board2.FPGAWriteReg(blOffset.Value, &H409)
'                    MainForm.board2.FPGAWriteReg(1, &H40C)
'                    MainForm.board2.FPGAWriteReg(0, &H40C)
'                Else
'                    MainForm.board2.FPGAWriteReg(blCor.SelectedIndex - 1, &H40C)
'                    MainForm.board2.FPGAWriteReg(blOffset.Value, &H409)
'                End If
'            End If
'        End If

'        'data delay
'        MainForm.board1.FPGAWriteReg(Int((dDelay.Value) / MainForm.Ts) + 5, &H700)
'        If MainForm.nboard > 1 Then
'            MainForm.board2.FPGAWriteReg(Int((dDelay2.Value) / MainForm.Ts) + 5, &H700)
'        End If

'        'trigger advance
'        MainForm.board1.FPGAWriteReg(Int((trDelay.Value) / 10) + 5, &H707)
'        If MainForm.nboard > 1 Then
'            MainForm.board2.FPGAWriteReg(Int((trDelay2.Value) / 10) + 5, &H707)
'        End If

'        'trigger mode
'        If trMode.SelectedIndex = 0 Then
'            MainForm.board1.FPGAWriteReg(0, &H701)
'            MainForm.board1.FPGAWriteReg(0, &H709)
'        Else
'            If trMode.SelectedIndex = 1 Then
'                MainForm.board1.FPGAWriteReg(1, &H701)
'                MainForm.board1.FPGAWriteReg(0, &H709)
'            Else
'                If trMode.SelectedIndex = 2 Then
'                    MainForm.board1.FPGAWriteReg(1, &H701)
'                    MainForm.board1.FPGAWriteReg(0, &H708)
'                    MainForm.board1.FPGAWriteReg(1, &H709)
'                Else
'                    MainForm.board1.FPGAWriteReg(1, &H701)
'                    MainForm.board1.FPGAWriteReg(1, &H708)
'                    MainForm.board1.FPGAWriteReg(1, &H709)
'                End If
'            End If
'        End If
'        If MainForm.nboard > 1 Then
'            If trMode.SelectedIndex = 0 Then
'                MainForm.board2.FPGAWriteReg(0, &H701)
'                MainForm.board2.FPGAWriteReg(0, &H709)
'            Else
'                If trMode.SelectedIndex = 1 Then
'                    MainForm.board2.FPGAWriteReg(1, &H701)
'                    MainForm.board2.FPGAWriteReg(0, &H709)
'                Else
'                    If trMode.SelectedIndex = 2 Then
'                        MainForm.board2.FPGAWriteReg(1, &H701)
'                        MainForm.board2.FPGAWriteReg(0, &H708)
'                        MainForm.board2.FPGAWriteReg(1, &H709)
'                    Else
'                        MainForm.board2.FPGAWriteReg(1, &H701)
'                        MainForm.board2.FPGAWriteReg(1, &H708)
'                        MainForm.board2.FPGAWriteReg(1, &H709)
'                    End If
'                End If
'            End If
'        End If

'        'board correlation
'        If MainForm.nboard > 1 Then
'            If cBoard.Checked = False Then
'                MainForm.board1.FPGAWriteReg(0, &H600)
'                MainForm.board2.FPGAWriteReg(0, &H600)
'                MainForm.rdc.noncorrelate = True
'            Else
'                MainForm.board1.FPGAWriteReg(0, &H600)
'                MainForm.board2.FPGAWriteReg(1, &H600)
'                MainForm.rdc.noncorrelate = False
'            End If
'        Else
'            MainForm.board1.FPGAWriteReg(0, &H600)
'        End If
'        BiasUpdate()
'        If bBias.Checked = False Then
'            MainForm.hvon.Enabled = True
'            MainForm.hvoff.Enabled = False
'        Else
'            MainForm.hvon.Enabled = False
'            MainForm.hvoff.Enabled = True
'        End If



'        I2CDACCFG(&H54 + 3)
'        I2CDACCFG(&H54 + 2)
'        I2CDACCFG(&H54 + 1)
'        I2CDACCFG(&H54 + 0)

'        Dim DAC_IC, DAC_CH
'        For i = 0 To 63
'            TranslateSiPMDAC(i, DAC_IC, DAC_CH)
'            Dim val = Int(nudBiasVar(i).Value / 5.0 * 16383)
'            I2CDACWrite(&H54 + DAC_IC, DAC_CH, val)
'        Next

'        MainForm.rdc.smallpack = bLowRate.Checked


'        My.Settings.bBIAS = bBias.Checked
'        My.Settings.BIAS = vBias.Value
'        My.Settings.trMode = trMode.SelectedIndex
'        My.Settings.trLevel = trLevel.Value
'        My.Settings.dDelay = dDelay.Value
'        My.Settings.trDelay = trDelay.Value
'        My.Settings.dDelay2 = dDelay2.Value
'        My.Settings.trDelay2 = trDelay2.Value
'        My.Settings.trHold = trHold.Value
'        My.Settings.prEn = prEn.Checked
'        My.Settings.prTime = prTime.Value
'        My.Settings.iTime = iTime.Value
'        My.Settings.blCor = blCor.SelectedIndex
'        My.Settings.blOffset = blOffset.Value
'        My.Settings.nf_b = nf_b.Checked
'        My.Settings.dGain = dGain.Value
'        My.Settings.cBoard = cBoard.Checked
'        My.Settings.bLowRate = bLowRate.Checked
'        My.Settings.bseparateThreshold = bSeparateTrigger.Checked
'        Try
'        Dim strout = ""

'            For i = 0 To nudBiasVar.Length - 2
'                strout = strout & nudBiasVar(i).Value & ";"
'            Next
'            strout = strout & nudBiasVar(nudBiasVar.Length - 1).Value

'            My.Settings.biasGainSettings = strout


'            strout = ""

'            For i = 0 To nThresholdTrigger.Length - 2
'                strout = strout & nThresholdTrigger(i).Value & ";"
'            Next
'            strout = strout & nudBiasVar(nThresholdTrigger.Length - 1).Value

'            My.Settings.separateTresholdValues = strout


'        Catch ex As Exception

'        End Try

'        My.Settings.Save()
'        MsgBox("Done", vbInformation + vbOKOnly)
'    End Sub
'    Public Sub BiasUpdate()
'        If bBias.Checked = False Then
'            MainForm.board1.FPGAWriteReg(0, &H703)
'            MainForm.board1.FPGAWriteReg(0 + (1 << 8), &H703)
'        Else
'            MainForm.board1.FPGAWriteReg(0, 703)
'            Dim vmin As Double = MainForm.AFE_MINBIAS
'            Dim vmax As Double = MainForm.AFE_MAXBIAS
'            Dim vbi = (vBias.Value - vmin) / (vmax - vmin) * 255
'            MainForm.board1.FPGAWriteReg(0, &H703)
'            Dim data = 255 - Int(vbi)
'            MainForm.board1.FPGAWriteReg(data + (1 << 8), &H703)
'        End If
'    End Sub
'    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

'        'I2CEEpromWrite(&H50, 0, Asc("N"))
'        'System.Threading.Thread.Sleep(50)
'        'I2CEEpromWrite(&H50, 1, Asc("I"))
'        'System.Threading.Thread.Sleep(50)
'        'I2CEEpromWrite(&H50, 2, Asc("0"))
'        'System.Threading.Thread.Sleep(50)
'        'I2CEEpromWrite(&H50, 3, Asc("7"))
'        'System.Threading.Thread.Sleep(50)
'        'I2CEEpromWrite(&H50, 4, Asc("0"))
'        'System.Threading.Thread.Sleep(50)
'        'I2CEEpromWrite(&H50, 5, Asc("0"))
'        'System.Threading.Thread.Sleep(50)
'        'I2CEEpromWrite(&H50, 6, Asc("0"))
'        'System.Threading.Thread.Sleep(50)
'        'I2CEEpromWrite(&H50, 7, Asc("3"))
'        'System.Threading.Thread.Sleep(50)


'        'Dim vmin = 4980
'        'Dim vmax = 6210

'        'I2CEEpromWrite(&H50, &H10, (vmin >> 8) And &HFF)
'        'System.Threading.Thread.Sleep(50)
'        'I2CEEpromWrite(&H50, &H11, (vmin >> 0) And &HFF)
'        'System.Threading.Thread.Sleep(50)

'        'I2CEEpromWrite(&H50, &H12, (vmax >> 8) And &HFF)
'        'System.Threading.Thread.Sleep(50)
'        'I2CEEpromWrite(&H50, &H13, (vmax >> 0) And &HFF)
'        'System.Threading.Thread.Sleep(50)

'        'Dim res = 3300
'        'I2CEEpromWrite(&H50, &H14, (res >> 8) And &HFF)
'        'System.Threading.Thread.Sleep(50)
'        'I2CEEpromWrite(&H50, &H15, (res >> 0) And &HFF)
'        'System.Threading.Thread.Sleep(50)

'        'Dim cap = 100
'        'I2CEEpromWrite(&H50, &H16, (cap >> 8) And &HFF)
'        'System.Threading.Thread.Sleep(50)
'        'I2CEEpromWrite(&H50, &H17, (cap >> 0) And &HFF)
'        'System.Threading.Thread.Sleep(50)


'        'Dim valore
'        'For i = 0 To 10
'        '    I2CEEpromRead(&H50, i, valore)
'        '    System.Threading.Thread.Sleep(50)
'        'Next

'        Dim MODELLO_SENSORE As String
'        MODELLO_SENSORE = "S13615-1025-N"

'        Dim SN_NISENSORE As String
'        SN_NISENSORE = "NI080003"


'        Dim SN_SENSORE As String
'        SN_SENSORE = "CUSTOM"

'        Dim SN_MANUF As String
'        SN_MANUF = "HAMA"

'        Dim vpower = 576


'        For i = 0 To SN_NISENSORE.Length - 1
'            I2CEEpromWrite(&H52, 0 + i, Asc(SN_NISENSORE.Substring(i, 1)))
'            System.Threading.Thread.Sleep(50)
'        Next
'        I2CEEpromWrite(&H52, 0 + SN_NISENSORE.Length, 0)


'        For i = 0 To MODELLO_SENSORE.Length - 1
'            I2CEEpromWrite(&H52, 8 + i, Asc(MODELLO_SENSORE.Substring(i, 1)))
'            System.Threading.Thread.Sleep(50)
'        Next
'        I2CEEpromWrite(&H52, 8 + MODELLO_SENSORE.Length, 0)

'        For i = 0 To SN_SENSORE.Length - 1
'            I2CEEpromWrite(&H52, 32 + i, Asc(SN_SENSORE.Substring(i, 1)))
'            System.Threading.Thread.Sleep(50)
'        Next
'        I2CEEpromWrite(&H52, 32 + SN_SENSORE.Length, 0)

'        For i = 0 To SN_MANUF.Length - 1
'            I2CEEpromWrite(&H52, 64 + i, Asc(SN_MANUF.Substring(i, 1)))
'            System.Threading.Thread.Sleep(50)
'        Next
'        I2CEEpromWrite(&H52, 64 + SN_MANUF.Length, 0)

'        I2CEEpromWrite(&H52, 128, (vpower >> 8))
'        System.Threading.Thread.Sleep(50)
'        I2CEEpromWrite(&H52, 129, (vpower And &HFF))
'        System.Threading.Thread.Sleep(50)



'    End Sub

'    Private Sub Button3_Click(sender As Object, e As EventArgs)
'        Dim temp As Integer
'        I2CTempRead(&H48, temp)
'    End Sub

'    Private Sub TabPage3_Click(sender As Object, e As EventArgs) Handles TabPage3.Click

'    End Sub

'    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
'        MainForm.board1.FPGAWriteReg(0, &H703)
'        Dim data = 1
'        MainForm.board1.FPGAWriteReg(data + (1 << 8), &H703)
'    End Sub

'    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
'        MainForm.board1.FPGAWriteReg(0, &H703)
'        Dim data = 255
'        MainForm.board1.FPGAWriteReg(data + (1 << 8), &H703)
'    End Sub

'    Private Sub vBias_ValueChanged(sender As Object, e As EventArgs) Handles vBias.ValueChanged

'    End Sub

'    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
'        Timer1.Enabled = False
'        Try
'            Dim runid As Integer
'            Dim runtime As String
'            Dim triggercount As ULong
'            Dim eventcounter As ULong
'            Dim lastspilltime As String
'            Dim ssMADA1 As ULong
'            Dim ssMADA2 As ULong
'            Dim filler1 As Long = 0
'            Dim filler2 As Long = 0
'            Dim isRunning As Boolean = False
'            MainForm.rdc.GetRunInfo(runid, runtime, triggercount, eventcounter, lastspilltime, ssMADA1, ssMADA2, filler1, filler2, isRunning)
'            If MainForm.rdc.cTargetMode = 1 And isRunning Then

'                If MainForm.rdc.cTargetValue < triggercount Then
'                    MainForm.StopSpectraAcquisition()
'                End If
'            End If
'            If MainForm.rdc.cTargetMode = 2 And isRunning Then
'                If MainForm.rdc.cTargetValue < runtime Then
'                    MainForm.StopSpectraAcquisition()
'                End If
'            End If
'            pDummy.Text = filler1 & " - " & filler2
'            ps1.Text = runid
'            ps2.Text = runtime & "s"
'            ps3.Text = triggercount
'            ps4.Text = eventcounter
'            ps5.Text = lastspilltime & "s"
'            pcntMada1.Text = ssMADA1
'            pcntMADA2.Text = ssMADA2
'            If eventcounter > 1500000 Then
'                MainForm.STopAcquisitionOnFile("Acquisition has been stopped. Maximum number of storable events was reached")
'            End If
'        Catch ex As Exception

'        End Try
'        Timer1.Enabled = True

'    End Sub

'    Private Sub TabPage6_Click(sender As Object, e As EventArgs) Handles TabPage6.Click

'    End Sub

'    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
'        For i = 0 To 63
'            nThresholdTrigger(i).Value = vThAll.Value
'        Next
'    End Sub
'End Class
