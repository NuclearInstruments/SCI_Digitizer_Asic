Imports System.IO
Imports System.Threading
Imports DT5550W_P_lib
Imports Newtonsoft.Json

Module MainModule
    Public DTList As New List(Of DT5550W_HAL)
    Dim running As Boolean = False

    Sub Main()

        Dim configurationFile = "config.json"
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

        Dim cCfg As ConfigClass = JsonConvert.DeserializeObject(Of ConfigClass)(CfgTXT)

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


        Console.WriteLine("")
        Console.WriteLine("")
        Console.WriteLine("Configuring ASIC...")

        For i = 0 To cCfg.BoardsSettings.Count - 1
            Dim found = False
            For j = 0 To DTList.Count - 1
                If DTList(j).SerialNumber = cCfg.BoardsSettings(i).SN Then
                    Console.WriteLine($"Configuring BOARD {i}...")
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

        Console.ForegroundColor = ConsoleColor.Green
        Console.WriteLine("")
        Console.WriteLine("")
        Console.WriteLine("STARTING ACQUISITION PROCESS")
        Console.WriteLine("----------------------------")
        Console.ForegroundColor = ConsoleColor.White

        running = True
        RUN_TARGET_MODE = TargetMode.Time_ns
        RUN_TARGET_VALUE = 10000000000

        Dim ATT As New List(Of AcqThreads)



        'Starting slaves
        For j = 0 To DTList.Count - 1
            If DTList(j).isMaster = False Then
                DTList(j).RUNControl(True)
            End If
        Next

        'Starting master as last
        For j = 0 To DTList.Count - 1
            If DTList(j).isMaster = True Then
                DTList(j).RUNControl(True)
                Dim ATS As New AcqThreadSettings
                Dim ATQ As New AcqThreadStatus

                Dim ATN As New AcqThreads
                ATN.id = j
                ATN.SN = DTList(j).SerialNumber
                ATN.master = True
                ATN.ATS = ATS
                ATN.ATQ = ATQ

                ATN.th = New Thread(Sub()
                                        AcquisitionThread(DTList(ATN.id), ATS, ATN.ATQ)
                                    End Sub)
                ATN.th.Start()
                ATT.Add(ATN)
            End If
        Next

        While True

            For Each atp In ATT
                If atp.th.IsAlive Then
                    Console.WriteLine(atp.ATQ.sEventCounter)
                Else
                    If atp.master Then

                        Console.ForegroundColor = ConsoleColor.Green
                        Console.WriteLine("Acquisition completed")
                        Console.ForegroundColor = ConsoleColor.White
                        End
                    End If
                End If
            Next

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

    End Class
    Public Structure AcqThreadStatus

        Public sEventCounter As Long
        Public sByteCounter As Long
        Public sTargetPercent As Double
        Public sTime As String
        Public sProcTime As Double
        Public sAcqTime As Double
        Public RunCompleted As Boolean
    End Structure


    Public Class AcqThreadSettings
        Public TempSensorSource As Integer = 0
        Public DisableTempReadingAcq As Boolean = True
        Public EnableTempComp As Boolean = False
        Public TempCompCoef As Double = 0
        Public CurrentHVSet As Double = 25
        Public CurrentHVON As Boolean = False
        Public CurrentHVMax As Double = 25
        Public InputPolarity As Integer = 0
    End Class

    Public Sub AcquisitionThread(board As DT5550W_HAL, ATS As AcqThreadSettings, ByRef ATQ As AcqThreadStatus)



        Dim StartTime As Date



        Dim file As String = "c:\\temp\\file_test.jdata"
        Dim TransferSize As Integer = 1000

        Dim BI As t_BoardInfo = board.GetBoardInfo
        Dim Buffer(BI.DigitalDataPacketSize * TransferSize * 2) As UInt32
        Dim ValidWord As UInt32 = 0
        board.FlushFIFO()

        board.RUNControl(False)
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
            tx = New StreamWriter(file)
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine($"Unable to open file: {file}. Error: " & ex.Message)
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

        strline = strline.Remove(strline.Length - 1)




        tx.WriteLine(strline)




        Dim TempTime = Now
        ATQ.sByteCounter = 0
        StartTime = Now

        board.RUNControl(True)
        While running
            DwnTime = Now

            'HV Temperature feedback
            If ATS.DisableTempReadingAcq = False Then
                If (Now - TempTime).TotalMilliseconds > 10000 Then
                    Dim tA = -1000, tb = -1000
                    If ATS.TempSensorSource = 0 Then
                        board.GetSensTemperature(0, tA)
                        If board.GetBoardInfo.totalAsics > 2 Then
                            board.GetSensTemperature(1, tb)
                        End If
                    Else
                        board.GetSensTemperature(2, tA)
                    End If

                    If ATS.EnableTempComp Then
                        Dim tAv = 0
                        If tb > -150 Then
                            tAv = (tA + tb) / 2
                        Else
                            tAv = tA
                        End If
                        board.SetHVTempFB(ATS.CurrentHVON, ATS.CurrentHVSet, ATS.CurrentHVMax, ATS.TempCompCoef, tAv)
                    End If



                    Dim enable, voltage, current
                    If board.GetHV(enable, voltage, current) Then

                    End If

                    TempTime = Now
                End If
            End If


            tDwnTime = Now - DwnTime
            ATQ.sAcqTime = tDwnTime.TotalMilliseconds


            DURATION = DateTime.Now - StartTime
            ATQ.sTime = DURATION.Hours.ToString.PadLeft(2, "0"c) & ":" &
                        DURATION.Minutes.ToString.PadLeft(2, "0"c) & ":" &
                        DURATION.Seconds.ToString.PadLeft(2, "0"c) & "." &
                        DURATION.Milliseconds.ToString.PadLeft(3, "0"c)

            ProcTime = Now



            'Events acquisition and decode
            board.GetRawBuffer(Buffer, TransferSize, 4000, BI.DigitalDataPacketSize, ValidWord)
            EventsBefore = TotalEvents
            DecodedEvents = board.DecodePetirocRowEvents(Buffer, ValidWord, Events, 0, ATS.InputPolarity)



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
                tx.WriteLine(strline)
                ATQ.sByteCounter += strline.Length

                TotalEvents += 1
                CurrentTimecode = e.RunEventTimecode_ns
            End While


            tProcTime = Now - ProcTime
            ATQ.sProcTime = tProcTime.TotalMilliseconds
            ATQ.sEventCounter = TotalEvents


            'Check for end conditions

            If RUN_TARGET_MODE = TargetMode.Time_ns Then
                ATQ.sTargetPercent = CurrentTimecode / RUN_TARGET_VALUE * 100
                If RUN_TARGET_VALUE <= CurrentTimecode Then
                    running = False
                    ATQ.RunCompleted = True
                End If

            End If
            If RUN_TARGET_MODE = TargetMode.Events Then
                ATQ.sTargetPercent = TotalEvents / RUN_TARGET_VALUE * 100
                If RUN_TARGET_VALUE <= TotalEvents Then
                    running = False
                    ATQ.RunCompleted = True

                End If
            End If

        End While

        If Not IsNothing(tx) Then
            tx.Close()
        End If

        board.RUNControl(False)
    End Sub


End Module
