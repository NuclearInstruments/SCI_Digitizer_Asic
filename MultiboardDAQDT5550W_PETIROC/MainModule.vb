Imports System.IO
Imports System.Threading
Imports DT5550W_P_lib
Imports Newtonsoft.Json

Module MainModule
    Public DTList As New List(Of DT5550W_HAL)
    Dim running As Boolean = False
    Dim ATT As New List(Of AcqThreads)
    Dim cCfg As ConfigClass
    Private Structure NameCommandLineStuct
        Dim Name As String
        Dim Value As String
    End Structure
    Private CommandLineArgs As New List(Of NameCommandLineStuct)

    Function ParseCommandLine() As Boolean
        'step one, Do we have a command line?
        If String.IsNullOrEmpty(Command) Then
            'give up if we don't
            Return False
        End If

        'does the command line have at least one named parameter?
        If Not Command.Contains("/") Then
            'give up if we don't
            Return False
        End If
        'Split the command line on our slashes.  
        Dim Params As String() = Split(Command, "/")

        'Iterate through the parameters passed
        For Each arg As String In Params
            'only process if the argument is not empty
            If Not String.IsNullOrEmpty(arg) Then
                'and contains an equal 
                If arg.Contains("=") Then

                    Dim tmp As NameCommandLineStuct
                    'find the equal sign
                    Dim idx As Integer = arg.IndexOf("=")
                    'if the equal isn't at the end of the string
                    If idx < arg.Length - 1 Then
                        'parse the name value pair
                        tmp.Name = arg.Substring(0, idx).Trim()
                        tmp.Value = arg.Substring(idx + 1).Trim()
                        'add it to the list.
                        CommandLineArgs.Add(tmp)
                    End If
                Else
                    Dim tmp As NameCommandLineStuct
                    tmp.Name = arg.Trim()
                    tmp.Value = 0
                    'add it to the list.
                    CommandLineArgs.Add(tmp)
                End If
            End If

        Next
        Return True
    End Function

    Sub Main()

        Dim configurationFile = "config.json"
        Dim skipAsic As Boolean = False

        If ParseCommandLine() Then
            For Each commandItem As NameCommandLineStuct In CommandLineArgs
                Select Case commandItem.Name.ToLower
                    Case "config"
                        Console.WriteLine(String.Format("Config file is is {0}", commandItem.Value))
                        configurationFile = commandItem.Value
                    Case "skipconfig"
                        Console.WriteLine(String.Format("Skip asic configuration"))
                        skipAsic = True
                End Select
            Next
        End If


        'Create a simple configuration
        Console.ForegroundColor = ConsoleColor.White

        Dim CfgTXT As String = ""
        Try
            Console.WriteLine("Loading configuration file: " & configurationFile)
            CfgTXT = File.ReadAllText(configurationFile)
            Console.ForegroundColor = ConsoleColor.Green
            Console.WriteLine("Successfully loaded")
            Console.ForegroundColor = ConsoleColor.White

        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Unable to load configuration: " & ex.Message)
            Console.ForegroundColor = ConsoleColor.White
            End
        End Try

        cCfg = JsonConvert.DeserializeObject(Of ConfigClass)(CfgTXT)

        If cCfg.ListOfDevices.Count = 0 Then
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("No devices in configuration file")
            Console.ForegroundColor = ConsoleColor.White
            End
        End If


        Console.ForegroundColor = ConsoleColor.White
        Console.WriteLine("")
        Console.WriteLine("--------------------------------------------------------------------------------------")
        Console.WriteLine("|   ID    |       SN       |      MODEL     |  CHANNELS  |     BUILD      |  MASTER  |")
        Console.WriteLine("|---------|----------------|----------------|------------|----------------|----------|")

        Dim masterFound As Integer = -1
        Dim boardFound As Integer = 0
        For i = 0 To cCfg.ListOfDevices.Count - 1
            Dim newDT As New DT5550W_HAL
            If newDT.Connect(cCfg.ListOfDevices(i)) Then
                Dim model As String = "AAAAAAAAAAAAAAAAAAAAAAAAA"
                Dim asic_count As Integer
                Dim channel_count As Integer
                Dim SN As Integer
                newDT.GetDGCardModel(model, asic_count, SN)
                channel_count = asic_count * 32
                Dim build As UInt32
                newDT.GetBuild(build)
                Dim buildstring As String = Hex(build)
                Dim ismasterString As String = ""
                If cCfg.ListOfDevices(i).ToString.ToUpper = cCfg.MasterSN.ToString.ToUpper Then
                    ismasterString = "*"
                    newDT.isMaster = True
                    masterFound = i
                Else
                    newDT.isMaster = False
                End If
                Console.WriteLine($"| {i.ToString.PadRight(7)} | {cCfg.ListOfDevices(i).PadRight(14)} | {model.PadRight(14)} | {channel_count.ToString.PadRight(10)} | {buildstring.PadRight(14)} | {ismasterString.PadRight(8)} |")
                DTList.Add(newDT)
                boardFound = boardFound + 1
            Else
                Console.WriteLine($"| {i.ToString.PadRight(7)} | {cCfg.ListOfDevices(i).PadRight(14)} | UNABLE TO FIND BOARD                                    |")

            End If
        Next
        Console.WriteLine("--------------------------------------------------------------------------------------")

        Console.WriteLine("")

        If boardFound <> cCfg.ListOfDevices.Count Then
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Unable to match connected board with SN in the configuration file")
            Console.ForegroundColor = ConsoleColor.White
            End
        End If

        If masterFound < 0 Then
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Unable to find the master board")
            Console.ForegroundColor = ConsoleColor.White
            End
        End If


        Console.ForegroundColor = ConsoleColor.Green
        Console.WriteLine(boardFound & "/" & cCfg.ListOfDevices.Count & " connected. Master board is: " & masterFound)
        Console.ForegroundColor = ConsoleColor.White


        If skipAsic = False Then
            Console.WriteLine("")
            Console.WriteLine("")
            Console.WriteLine("Configuring ASIC...")

            For i = 0 To cCfg.BoardsSettings.Count - 1
                Dim found = False
                For j = 0 To DTList.Count - 1
                    If DTList(j).SerialNumber = cCfg.BoardsSettings(i).SN Then
                        Console.WriteLine($"Configuring BOARD {i}...")


                        If cCfg.BoardsSettings(j).configuration_mode.ToUpper = "ASIC_CONFIGURATION" Then
                            Dim model As String = "AAAAAAAAAAAAAAAAAAAAAAAAA"
                            Dim asic_count As Integer
                            Dim SN As Integer
                            DTList(j).GetDGCardModel(model, asic_count, SN)

                            For asic = 0 To asic_count - 1
                                Console.WriteLine($"    Generating bitstream ASIC {asic} OK")
                                Dim ProgramWord() As UInt32 = New UInt32((20) - 1) {}
                                Dim pC As New DT5550W_PETIROC.PetirocConfig

                                Dim aset As New AsicSetting
                                If cCfg.BoardsSettings(j).asic_configuration.asic_settings.Count <= asic Then
                                    aset = cCfg.BoardsSettings(j).asic_configuration.asic_settings.Last
                                Else
                                    aset = cCfg.BoardsSettings(j).asic_configuration.asic_settings(asic)
                                End If
                                For z = 0 To 31
                                    Dim cset As New ChannelSpecific
                                    If aset.channel_specific.Count <= z Then
                                        cset = aset.channel_specific.Last
                                    Else
                                        cset = aset.channel_specific(z)
                                    End If


                                    pC.inputDAC(z).enable = cset.BIAS
                                    pC.inputDAC(z).value = cset.BIAS_OFFSET
                                    pC.InputDiscriminator(z).maskDiscriminatorQ = cset.MASK_CHARGE
                                    pC.InputDiscriminator(z).maskDiscriminatorT = cset.MASK_TIME
                                    pC.InputDiscriminator(z).DACValue = cset.THRESHOLD_ADJ
                                Next

                                pC.InputPolarity = IIf(cCfg.BoardsSettings(j).Polarity.ToUpper = "POSITIVE", tPOLARITY.POSITIVE, tPOLARITY.NEGATIVE)
                                pC.DAC_Q_threshold = IIf(cCfg.BoardsSettings(j).Polarity.ToUpper = "POSITIVE", 1024 - cCfg.BoardsSettings(j).asic_configuration.global_settings.CHARGE_TRIGGER, cCfg.BoardsSettings(j).asic_configuration.global_settings.CHARGE_TRIGGER)
                                pC.DAC_T_threshold = IIf(cCfg.BoardsSettings(j).Polarity.ToUpper = "POSITIVE", 1024 - cCfg.BoardsSettings(j).asic_configuration.global_settings.TIME_TRIGGER, cCfg.BoardsSettings(j).asic_configuration.global_settings.TIME_TRIGGER)
                                pC.DelayTrigger = cCfg.BoardsSettings(j).asic_configuration.global_settings.INTERNAL_TRIGGER_DELAY
                                If cCfg.BoardsSettings(j).asic_configuration.global_settings.SHAPER_C1.ToUpper = "1.25PF" Then
                                    pC.SlowInputC = 0
                                ElseIf cCfg.BoardsSettings(j).asic_configuration.global_settings.SHAPER_C1.ToUpper = "2.5PF" Then
                                    pC.SlowInputC = 1
                                ElseIf cCfg.BoardsSettings(j).asic_configuration.global_settings.SHAPER_C1.ToUpper = "3.75PF" Then
                                    pC.SlowInputC = 2
                                ElseIf cCfg.BoardsSettings(j).asic_configuration.global_settings.SHAPER_C1.ToUpper = "5PF" Then
                                    pC.SlowInputC = 3
                                End If

                                If cCfg.BoardsSettings(j).asic_configuration.global_settings.SHAPER_C1.ToUpper = "100PF" Then
                                    pC.SlowFeedbackC = 0
                                ElseIf cCfg.BoardsSettings(j).asic_configuration.global_settings.SHAPER_C1.ToUpper = "200PF" Then
                                    pC.SlowFeedbackC = 1
                                ElseIf cCfg.BoardsSettings(j).asic_configuration.global_settings.SHAPER_C1.ToUpper = "300PF" Then
                                    pC.SlowFeedbackC = 2
                                ElseIf cCfg.BoardsSettings(j).asic_configuration.global_settings.SHAPER_C1.ToUpper = "400PF" Then
                                    pC.SlowFeedbackC = 3
                                End If

                                If cCfg.BoardsSettings(j).UseChangeTrigger = True Then
                                    pC.TriggerOut = False
                                Else
                                    pC.TriggerOut = True
                                End If


                                'disable T disciminator if external trigger is selected
                                If cCfg.BoardsSettings(i).TriggerSource = "EXTERNAL" Then
                                    pC.PowerControl.DiscrT = False
                                    pC.PowerControl.Delay_Discr = False
                                    pC.PowerControl.Delay_Ramp = False
                                    pC.PowerControl.TDC_ramp = False
                                    pC.PulseMode.DiscrT = False
                                    pC.PulseMode.Delay_Discr = False
                                    pC.PulseMode.Delay_Ramp = False
                                    pC.PulseMode.TDC_ramp = False
                                    For z = 0 To 31
                                        pC.InputDiscriminator(z).maskDiscriminatorQ = True
                                        pC.InputDiscriminator(z).maskDiscriminatorT = True
                                    Next
                                Else
                                    pC.PowerControl.DiscrT = True
                                    pC.PowerControl.Delay_Discr = True
                                    pC.PowerControl.Delay_Ramp = True
                                    pC.PowerControl.TDC_ramp = True
                                    pC.PulseMode.DiscrT = True
                                    pC.PulseMode.Delay_Discr = True
                                    pC.PulseMode.Delay_Ramp = True
                                    pC.PulseMode.TDC_ramp = True
                                End If


                                pC.GenerateUint32Config(ProgramWord)
                                Select Case asic
                                    Case 0
                                        DTList(j).ConfigureAsic(True, False, False, False, ProgramWord)
                                    Case 1
                                        DTList(j).ConfigureAsic(False, True, False, False, ProgramWord)
                                    Case 2
                                        DTList(j).ConfigureAsic(False, False, True, False, ProgramWord)
                                    Case 3
                                        DTList(j).ConfigureAsic(False, False, False, True, ProgramWord)
                                End Select
                                Console.WriteLine($"    Configuring ASIC {asic} OK")
                            Next

                        Else
                            For a = 0 To cCfg.BoardsSettings(i).bitstreams.Count - 1
                                Dim ProgramWord() As UInt32 = New UInt32((20) - 1) {}
                                DTList(j).PetirocClass.PetirocCfg.ConvertStringToUint32Config(cCfg.BoardsSettings(i).bitstreams(a), ProgramWord)
                                Select Case a
                                    Case 0
                                        DTList(j).ConfigureAsic(True, False, False, False, ProgramWord)
                                    Case 1
                                        DTList(j).ConfigureAsic(False, True, False, False, ProgramWord)
                                    Case 2
                                        DTList(j).ConfigureAsic(False, False, True, False, ProgramWord)
                                    Case 3
                                        DTList(j).ConfigureAsic(False, False, False, True, ProgramWord)
                                End Select
                                Console.WriteLine($"    Configuring ASIC {a} OK")
                            Next
                        End If





                        DTList(j).SetHV(IIf(cCfg.BoardsSettings(i).HV_VOLT > 5, 1, 0), cCfg.BoardsSettings(i).HV_VOLT, cCfg.BoardsSettings(i).HV_MAX)

                        DTList(j).ConfigureSignalGenerator(cCfg.BoardsSettings(i).SelfTrigger.enable,
                                                cCfg.BoardsSettings(i).SelfTrigger.enable,
                                                cCfg.BoardsSettings(i).SelfTrigger.enable,
                                                cCfg.BoardsSettings(i).SelfTrigger.enable,
                                                 cCfg.BoardsSettings(i).SelfTrigger.rate)
                        Dim TriggerMode As Integer = 0
                        If cCfg.BoardsSettings(i).TriggerSource = "INTERNAL" Then
                            TriggerMode = 0
                        ElseIf cCfg.BoardsSettings(i).TriggerSource = "INT/EXT" Then
                            TriggerMode = 1
                        ElseIf cCfg.BoardsSettings(i).TriggerSource = "EXTERNAL" Then
                            TriggerMode = 2
                        Else
                            Console.ForegroundColor = ConsoleColor.Red
                            Console.WriteLine("Invalid parameter in TriggerSource")
                            Console.ForegroundColor = ConsoleColor.White
                            End
                        End If

                        DTList(j).SelectTriggerMode(cCfg.BoardsSettings(i).UseChangeTrigger)

                        DTList(j).EnableTriggerFrame(cCfg.BoardsSettings(i).EnableFrameTrigger)

                        If TriggerMode = 0 Then
                            DTList(j).EnableExternalTrigger(False)
                            DTList(j).ConfigureExtHold(1, False)
                        End If
                        If TriggerMode = 1 Then
                            DTList(j).EnableExternalTrigger(True)
                            DTList(j).ConfigureExtHold(1, False)
                        End If
                        If TriggerMode = 2 Then
                            DTList(j).EnableExternalTrigger(False)
                            DTList(j).ConfigureExtHold(cCfg.BoardsSettings(i).ExtHoldDelay, True)
                        End If


                        DTList(j).EnableExternalVeto(cCfg.BoardsSettings(i).EnableExternalVeto)
                        DTList(j).EnableResetTDCOnT0(True)


                        If DTList(j).isMaster Then
                            DTList(j).ExtRunEnable(False)
                            DTList(j).RUNControl(False)
                        Else
                            DTList(j).ExtRunEnable(True)
                            DTList(j).RUNControl(False)
                        End If


                        found = True
                    End If
                Next

                If found = False Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine($"Error in the configuration file. Settings for SN: {cCfg.BoardsSettings(i).SN} does not exists.")
                    Console.ForegroundColor = ConsoleColor.White
                    End
                End If
            Next

            Console.ForegroundColor = ConsoleColor.Green
            Console.WriteLine("All board are successfully configured")
            Console.ForegroundColor = ConsoleColor.White
        Else
            Console.ForegroundColor = ConsoleColor.Green
            Console.WriteLine("Board configuration skipped")
            Console.ForegroundColor = ConsoleColor.White

        End If
        Console.Clear()
        Console.ForegroundColor = ConsoleColor.Yellow
        Console.WriteLine("-------------------------------------------------------------------------------")
        Console.WriteLine("|   ID    |       SN       |    SET POINT   |        V       | WAIT |    mA    |")
        Console.WriteLine("|---------|----------------|----------------|----------------|------|----------|")
        For i = 0 To DTList.Count - 1
            Console.WriteLine("|         |                |                |                |      |          |")
        Next
        Console.WriteLine("-------------------------------------------------------------------------------")

        Console.SetCursorPosition(1, 17)
        Console.WriteLine("Waiting HV on selected board...")
        Console.SetCursorPosition(1, 18)
        Console.WriteLine($"Press q to exit or press s to skip waiting HV")

        Dim good = True

        Do
            good = True
            For i = 0 To cCfg.BoardsSettings.Count - 1
                For j = 0 To DTList.Count - 1
                    If DTList(j).SerialNumber = cCfg.BoardsSettings(i).SN Then
                        Dim hv_enable As Boolean = 0
                        Dim hv_volt As Single = 0
                        Dim hv_current As Single = 0
                        DTList(j).GetHV(hv_enable, hv_volt, hv_current)
                        Console.SetCursorPosition(1, 3 + i)
                        Console.Write(i.ToString.PadRight(4))
                        Console.SetCursorPosition(11, 3 + i)
                        Console.Write(DTList(j).SerialNumber.PadRight(16))
                        Console.SetCursorPosition(28, 3 + i)
                        Console.Write(cCfg.BoardsSettings(i).HV_VOLT.ToString.PadLeft(16))
                        Console.SetCursorPosition(45, 3 + i)
                        Console.Write(Math.Round(hv_volt, 3).ToString.PadLeft(16))
                        Console.SetCursorPosition(62, 3 + i)
                        Console.Write(IIf(cCfg.BoardsSettings(i).wait_hv, "  **  ", "      "))
                        Console.SetCursorPosition(69, 3 + i)
                        Console.Write(Math.Round(hv_current, 3).ToString.PadLeft(10))
                        If cCfg.BoardsSettings(i).wait_hv Then
                            If hv_volt < cCfg.BoardsSettings(i).HV_VOLT - 0.2 Then
                                good = False
                            End If
                        End If
                    End If
                Next
            Next


            If Console.KeyAvailable Then
                Dim k As ConsoleKeyInfo = Console.ReadKey()
                If k.KeyChar = "q" Or k.KeyChar = "Q" Then
                    Console.ForegroundColor = ConsoleColor.White
                    Console.WriteLine("")
                    End
                End If
                If k.KeyChar = "s" Or k.KeyChar = "S" Then
                    Console.ForegroundColor = ConsoleColor.White
                    Console.WriteLine("")
                    good = True
                End If
            End If
            System.Threading.Thread.Sleep(100)
        Loop While good = False



        Console.ForegroundColor = ConsoleColor.White



        Console.ForegroundColor = ConsoleColor.Green
        Console.WriteLine("")
        Console.WriteLine("")
        Console.WriteLine("STARTING ACQUISITION PROCESS")
        Console.WriteLine("----------------------------")
        Console.ForegroundColor = ConsoleColor.White

        running = True

        If cCfg.GeneralSettings.RunTarget.ToUpper = "FREE" Then
            RUN_TARGET_MODE = TargetMode.FreeRunning
            RUN_TARGET_VALUE = 10
        ElseIf cCfg.GeneralSettings.RunTarget.ToUpper = "TIME" Then
            RUN_TARGET_MODE = TargetMode.Time_ns
            RUN_TARGET_VALUE = cCfg.GeneralSettings.TargetValue * 1000000000
        ElseIf cCfg.GeneralSettings.RunTarget.ToUpper = "COUNTS" Then
            RUN_TARGET_MODE = TargetMode.Events
            RUN_TARGET_VALUE = cCfg.GeneralSettings.TargetValue

        End If


        Dim RunId As Integer
        RunId = (DateTime.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds


        'Starting slaves
        For j = 0 To DTList.Count - 1
            If DTList(j).isMaster = False Then
                'DTList(j).RUNControl(True)
                LaunchThread(j, RunId)
            End If
        Next

        'Starting master as last
        For j = 0 To DTList.Count - 1
            If DTList(j).isMaster = True Then
                LaunchThread(j, RunId)
            End If
        Next

        Console.Clear()
        Console.CursorVisible = False
        Console.WriteLine("-------------------------------------------------------------------------------------------------")
        Console.WriteLine("| ID |     SN     |       TIME      |      PACKETS    |    TIME CODE    | RUNNING | TEMP |  HV  |")
        Console.WriteLine("|----|------------|-----------------|-----------------|-----------------|---------|------|------|")
        For i = 0 To ATT.Count - 1
            Console.WriteLine("|    |            |                 |                 |                 |         |      |      |")
        Next
        Console.WriteLine("-------------------------------------------------------------------------------------------------")
        Console.SetCursorPosition(1, 16)
        Console.ForegroundColor = ConsoleColor.Cyan
        If RUN_TARGET_MODE = TargetMode.FreeRunning Then
            Console.WriteLine($"RUN MODE: FREE RUN")
        ElseIf RUN_TARGET_MODE = TargetMode.Time_ns Then
            Console.WriteLine($"RUN MODE: TIME TARGET   RUN TARGET: {RUN_TARGET_VALUE}ns")
        ElseIf RUN_TARGET_MODE = TargetMode.Events Then
            Console.WriteLine($"RUN MODE: ENEVTS        RUN TARGET: {RUN_TARGET_VALUE} events.")
        End If
        Console.SetCursorPosition(1, 17)
        Console.ForegroundColor = ConsoleColor.Magenta
        Console.WriteLine($"RUNNING... Storing data on: {cCfg.DataStorageFolder & "\" & cCfg.FileName}   RUN ID:{RunId}")
        Console.ForegroundColor = ConsoleColor.White
        Console.SetCursorPosition(1, 18)
        Console.WriteLine($"Press q to stop acquisition")

        While True
            Dim i = 0
            Dim aliveThread = 0
            For Each atp In ATT
                If atp.th.IsAlive Then
                    Console.SetCursorPosition(1, 3 + i)
                    Console.Write(atp.id.ToString.PadRight(4))
                    Console.SetCursorPosition(7, 3 + i)
                    Console.Write(atp.SN.PadRight(11))
                    Console.SetCursorPosition(19, 3 + i)
                    Console.Write(atp.ATQ.sTime.ToString.PadLeft(17))
                    Console.SetCursorPosition(37, 3 + i)
                    Console.Write(atp.ATQ.sEventCounter.ToString.PadLeft(17))
                    Console.SetCursorPosition(55, 3 + i)
                    Console.Write(atp.ATQ.CurrentTimecode.ToString.PadLeft(17))
                    Console.SetCursorPosition(83, 3 + i)
                    Console.Write(Math.Round(atp.ATQ.Temp, 1).ToString.PadLeft(6))
                    Console.SetCursorPosition(90, 3 + i)
                    Console.Write(Math.Round(atp.ATQ.HV, 2).ToString.PadLeft(6))

                    Console.SetCursorPosition(10, 5)

                    If atp.master Then
                        Console.SetCursorPosition(73, 3 + i)
                        Console.Write(Math.Round(atp.ATQ.sTargetPercent, 1).ToString.PadLeft(9))
                    Else
                        Console.SetCursorPosition(73, 3 + i)
                        Console.Write("    *    ")
                    End If
                    aliveThread += 1
                Else
                    Console.SetCursorPosition(73, 3 + i)
                    Console.Write("         ")

                    If atp.master Then
                        Console.SetCursorPosition(1, 17)
                        Console.ForegroundColor = ConsoleColor.Green
                        Console.WriteLine($"Acquisition completed. RUN ID:{RunId}                                                              ")
                        Console.SetCursorPosition(1, 18)
                        Console.WriteLine($"Waiting for all other thread exits                                                                 ")
                        Console.ForegroundColor = ConsoleColor.White
                        running = False
                    End If
                End If

                i = i + 1
            Next

            If Console.KeyAvailable Then
                Dim k As ConsoleKeyInfo = Console.ReadKey()
                If k.KeyChar = "q" Or k.KeyChar = "Q" Then
                    running = False
                End If
            End If

            If aliveThread = 0 Then
                Console.ForegroundColor = ConsoleColor.Yellow
                Console.WriteLine("")



                For i = 0 To cCfg.BoardsSettings.Count - 1
                    For j = 0 To DTList.Count - 1
                        If DTList(j).SerialNumber = cCfg.BoardsSettings(i).SN Then
                            If cCfg.BoardsSettings(i).switch_off_hv_on_end Then
                                Console.WriteLine("Switching of HV on board " & DTList(j).SerialNumber)
                                DTList(j).SetHV(False, 20, 25)
                            End If
                        End If
                    Next
                Next

                Console.ForegroundColor = ConsoleColor.White

                End
            End If
            System.Threading.Thread.Sleep(100)
        End While



    End Sub




    Public Enum TargetMode
        FreeRunning = 0
        Time_ns = 1
        Events = 2
        Clusters = 3
    End Enum
    Public RUN_TARGET_MODE As TargetMode
    Public RUN_TARGET_VALUE As Double

    Public Class AcqThreads
        Public id As Integer
        Public master As Boolean
        Public SN As String
        Public th As Thread
        Public ATS As AcqThreadSettings
        Public ATQ As AcqThreadStatus

        Public RunId As String

    End Class
    Public Class AcqThreadStatus

        Public sEventCounter As Long
        Public sByteCounter As Long
        Public sTargetPercent As Double
        Public sTime As String = ""
        Public sProcTime As Double
        Public sAcqTime As Double
        Public RunCompleted As Boolean
        Public Temp As Double
        Public CurrentTimecode As Double
        Public HV As Double
    End Class


    Public Class AcqThreadSettings
        Public TempSensorSource As Integer = 0
        Public DisableTempReadingAcq As Boolean = False
        Public EnableTempComp As Boolean = False
        Public TempCompCoef As Double = 0
        Public CurrentHVSet As Double = 25
        Public CurrentHVON As Boolean = False
        Public CurrentHVMax As Double = 25
        Public InputPolarity As Integer = 0
        Public File As String = ""
    End Class


    Public Function LaunchThread(j As Integer, RunId As String)
        DTList(j).RUNControl(True)
        Dim ATS As New AcqThreadSettings
        Dim ATQ As New AcqThreadStatus

        Dim ATN As New AcqThreads
        ATN.id = j
        ATN.SN = DTList(j).SerialNumber
        ATN.master = True
        ATN.ATS = ATS
        ATN.ATQ = ATQ
        ATN.RunId = RunId


        For i = 0 To cCfg.BoardsSettings.Count - 1
            If DTList(j).SerialNumber = cCfg.BoardsSettings(i).SN Then
                ATS.CurrentHVSet = cCfg.BoardsSettings(i).HV_VOLT
                ATS.CurrentHVON = IIf(cCfg.BoardsSettings(i).HV_VOLT > 5, True, False)
                ATS.CurrentHVMax = cCfg.BoardsSettings(i).HV_MAX
                ATS.EnableTempComp = cCfg.BoardsSettings(i).EnableHVCompensation
                ATS.DisableTempReadingAcq = Not cCfg.BoardsSettings(i).ReadTemperatureAndHV
                ATS.TempCompCoef = cCfg.BoardsSettings(i).HVCompensationCoefficent
                ATS.InputPolarity =  IIf(cCfg.BoardsSettings(j).Polarity.ToUpper = "POSITIVE", tPOLARITY.POSITIVE, tPOLARITY.NEGATIVE)
            End If
        Next


        ATN.ATS.File = cCfg.DataStorageFolder & "\" & cCfg.FileName & "_" & RunId & "_" & ATN.SN & ".data"
        ATT.Add(ATN)
        ATT.Last.th = New Thread(Sub()
                                     AcquisitionThread(ATN.id)
                                 End Sub)
        ATT.Last.th.Start()
    End Function
    Public Sub AcquisitionThread(thid As Integer)

        Dim iid = -1
        For j = 0 To ATT.Count - 1
            If ATT(j).id = thid Then
                iid = j
                Exit For
            End If
        Next

        If iid = -1 Then
            Console.WriteLine("Unable to start acquistion thread")
            Exit Sub
        End If

        Dim StartTime As Date

        Dim TransferSize As Integer = 1000

        Dim BI As t_BoardInfo = DTList(ATT(iid).id).GetBoardInfo
        Dim Buffer(BI.DigitalDataPacketSize * TransferSize * 2) As UInt32
        Dim ValidWord As UInt32 = 0
        DTList(ATT(iid).id).FlushFIFO()

        DTList(ATT(iid).id).RUNControl(False)
        Dim Events As New Queue(Of DT5550W_PETIROC.t_DataPETIROC)
        Dim TotalEvents As Int64 = 0
        Dim DecodedEvents As Int64 = 0
        Dim EventsBefore As Int64 = 0
        Dim DURATION As TimeSpan
        Dim DwnTime As Date
        Dim tDwnTime As TimeSpan
        Dim ProcTime As Date
        Dim tProcTime As TimeSpan

        Dim CurrentTimecode As Double = 0
        Dim EventCounter As Int64 = 0

        Dim tx As StreamWriter = Nothing

        Try
            tx = New StreamWriter(ATT(iid).ATS.File)
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine($"Unable to open file: {ATT(iid).ATS.File }. Error: " & ex.Message)
            Console.ForegroundColor = ConsoleColor.White
        End Try


        Dim strline = ""


        strline = "ID;ASIC;EventCounter;RUN_EventTimeCodeLSB;T0_to_Event_Timecode;"
        For i = 0 To BI.channelsPerAsic - 1
            strline &= $"HIT_{i};"
        Next
        For i = 0 To BI.channelsPerAsic - 1
            strline &= $"CHARGE_{i};"
        Next
        For i = 0 To BI.channelsPerAsic - 1
            strline &= $"COARSE_{i};"
        Next
        For i = 0 To BI.channelsPerAsic - 1
            strline &= $"FINE_{i};"
        Next
        strline &= "HV;mA;TEMPERATURE"
        'strline = strline.Remove(strline.Length - 1)

        tx.WriteLine(strline)

        Dim TempTime = Now
        ATT(iid).ATQ.sByteCounter = 0
        StartTime = Now

        Dim tA = -1000, tb = -1000

        DTList(ATT(iid).id).GetSensTemperature(2, tA)
        Dim enable, voltage, current
        If DTList(ATT(iid).id).GetHV(enable, voltage, current) Then

        End If

        If ATT(iid).ATS.TempSensorSource = 0 Then
            DTList(ATT(iid).id).GetSensTemperature(0, tA)
        Else
            DTList(ATT(iid).id).GetSensTemperature(2, tA)
        End If

        Dim CurTemp As Double = 0, CurHv As Double = 0, CurI As Double = 0
        CurTemp = tA
        CurHv = voltage
        CurI = current

        DTList(ATT(iid).id).RUNControl(True)
        While running
            DwnTime = Now

            'HV Temperature feedback
            If ATT(iid).ATS.DisableTempReadingAcq = False Then
                If (Now - TempTime).TotalMilliseconds > 1000 Then
                    tA = -1000
                    tb = -1000
                    If ATT(iid).ATS.TempSensorSource = 0 Then
                        DTList(ATT(iid).id).GetSensTemperature(0, tA)
                        If DTList(ATT(iid).id).GetBoardInfo.totalAsics > 2 Then
                            DTList(ATT(iid).id).GetSensTemperature(1, tb)
                        End If
                    Else
                        DTList(ATT(iid).id).GetSensTemperature(2, tA)
                    End If
                    Dim tAv = 0
                    If tb > -150 Then
                        tAv = (tA + tb) / 2
                    Else
                        tAv = tA
                    End If
                    If ATT(iid).ATS.EnableTempComp Then
                        DTList(ATT(iid).id).SetHVTempFB(ATT(iid).ATS.CurrentHVON, ATT(iid).ATS.CurrentHVSet, ATT(iid).ATS.CurrentHVMax, ATT(iid).ATS.TempCompCoef, tA)
                    End If




                    If DTList(ATT(iid).id).GetHV(enable, voltage, current) Then

                    End If

                    CurTemp = tA
                    CurHv = voltage
                    CurI = current
                    ATT(iid).ATQ.HV = voltage
                    ATT(iid).ATQ.Temp = tAv

                    TempTime = Now
                End If
            End If


            tDwnTime = Now - DwnTime
            ATT(iid).ATQ.sAcqTime = tDwnTime.TotalMilliseconds


            DURATION = DateTime.Now - StartTime
            ATT(iid).ATQ.sTime = DURATION.Hours.ToString.PadLeft(2, "0"c) & ":" &
                        DURATION.Minutes.ToString.PadLeft(2, "0"c) & ":" &
                        DURATION.Seconds.ToString.PadLeft(2, "0"c) & "." &
                        DURATION.Milliseconds.ToString.PadLeft(3, "0"c)

            ProcTime = Now



            'Events acquisition and decode
            DTList(ATT(iid).id).GetRawBuffer(Buffer, TransferSize, 2000, BI.DigitalDataPacketSize, ValidWord)
            EventsBefore = TotalEvents
            DecodedEvents = DTList(ATT(iid).id).DecodePetirocRowEvents(Buffer, ValidWord, Events, 0, ATT(iid).ATS.InputPolarity)



            ''Write on file
            While Events.Count > 0
                strline = ""
                Dim e = Events.Dequeue

                For j = 0 To BI.channelsPerAsic - 1
                    Dim vtemp As Double = (CType(e.charge(j), Double))
                    vtemp = IIf(vtemp < 0, 0, vtemp)
                    vtemp = IIf(vtemp > 1023, 1023, vtemp)
                    e.charge(j) = vtemp
                Next


                Dim hitNumber(e.hit.Count - 1) As UInt16
                For i = 0 To e.hit.Count - 1
                    hitNumber(i) = IIf(e.hit(i), 1, 0)
                Next
                strline &= TotalEvents & ";" & e.AsicID & ";" & e.EventCounter & ";" & e.RunEventTimecode & ";" & e.EventTimecode & ";" & String.Join(";", hitNumber) & ";" & String.Join(";", e.charge) & ";" & String.Join(";", e.CoarseTime) & ";" & String.Join(";", e.FineTime)
                strline &= ";" & CurHv & ";" & CurI & ";" & CurTemp
                tx.WriteLine(strline)
                ATT(iid).ATQ.sByteCounter += strline.Length

                TotalEvents += 1
                CurrentTimecode = e.RunEventTimecode_ns
            End While


            tProcTime = Now - ProcTime
            ATT(iid).ATQ.sProcTime = tProcTime.TotalMilliseconds
            ATT(iid).ATQ.sEventCounter = TotalEvents

            ATT(iid).ATQ.CurrentTimecode = CurrentTimecode

            'Check for end conditions

            If RUN_TARGET_MODE = TargetMode.Time_ns Then
                ATT(iid).ATQ.sTargetPercent = CurrentTimecode / RUN_TARGET_VALUE * 100
                If RUN_TARGET_VALUE <= CurrentTimecode Then
                    running = False
                    ATT(iid).ATQ.RunCompleted = True
                End If

            End If
            If RUN_TARGET_MODE = TargetMode.Events Then
                ATT(iid).ATQ.sTargetPercent = TotalEvents / RUN_TARGET_VALUE * 100
                If RUN_TARGET_VALUE <= TotalEvents Then
                    running = False
                    ATT(iid).ATQ.RunCompleted = True

                End If
            End If

        End While

        If Not IsNothing(tx) Then
            tx.Close()
        End If

        DTList(ATT(iid).id).RUNControl(False)
    End Sub


End Module
