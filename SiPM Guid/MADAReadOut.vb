Imports System.IO
Imports System.Threading

Public Class MADAReadOut
    Public Const PackSize = 32
    Public Const ProducerCount = 2
    Public Const maxQueueSize = 100000

    Private RealTimeSpectra(PackSize * ProducerCount, 16383) As Double
    Private CumulativeEnergy(PackSize * ProducerCount) As Double
    Private SingleEventEnergy(PackSize * ProducerCount) As Double
    Dim MutexSpe As New Mutex
    Dim MutexCumulative As New Mutex
    Dim MutexImmediatea As New Mutex
    Dim MutexFile As New Mutex
    Dim fileName As String
    Dim fileEnable As Boolean = False
    Dim objRawWriter As StreamWriter
    Public board1 As FPGA_FX3
    Public board2 As FPGA_FX3
    Dim _nboard As Integer

    Public ScaleFactor As Double = 1
    Public Header As String

    Dim _acqExit = False
    Public smallpack = false
    Public caller As Object
    Public dropped_sync_package As Integer = 0
    Public SS_runid As Integer = 0
    Public SS_startTime As DateTime = Now
    Public SS_lastspill As DateTime = Now
    Dim SS_totalEvents As ULong = 0
    Dim SS_HighestTrigger As ULong = 0
    Dim isRunning = False
    Dim ssMADA1 As ULong
    Dim ssMADA2 As ULong
    Public Gains() As Double = {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
    Public Offsets() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    Dim StringaDati As String

    Public SyncMode As Integer = 0
    Public cTargetMode As Integer = 0
    Public cTargetValue As Integer = 0
    Public Function GetDroppedPackage() As Integer
        Dim t = dropped_sync_package
        dropped_sync_package = 0
        Return t
    End Function
    Public Function ResetAll() As Double()
        MutexSpe.WaitOne()
        For k = 0 To 63
            For i = 0 To 16383
                RealTimeSpectra(k, i) = 0
            Next

        Next
        MutexSpe.ReleaseMutex()

        MutexCumulative.WaitOne()
        For k = 0 To 63
            CumulativeEnergy(k) = 0
        Next
        MutexCumulative.ReleaseMutex()
        caller.AppendToLog("[INFO] Reset spectrum")
    End Function

    Public Function GetRunInfo(ByRef runId As ULong, ByRef runtime As String, ByRef triggercount As Integer, ByRef eventcounter As ULong, ByRef lastspilltime As String, ByRef board1 As ULong, ByRef board2 As ULong, ByRef dummypackets1 As ULong, ByRef dummypackets2 As ULong, ByRef runnig As Boolean)
        triggercount = SS_HighestTrigger
        eventcounter = SS_totalEvents
        If isRunning = True Then
            runtime = Math.Round((Now - SS_startTime).TotalSeconds, 0)
            lastspilltime = Math.Round((Now - SS_lastspill).TotalSeconds, 0)
        Else
            runtime = 0
            lastspilltime = 0
        End If

        runId = SS_runid
        board1 = ssMADA1
        board2 = ssMADA2
        dummypackets1 = Filler_Packet(0)
        dummypackets2 = Filler_Packet(1)

        runnig = isRunning
    End Function
    Public Function GetSpectrum(ByRef channel As Integer) As Double()
        Dim outspe(16384) As Double
        MutexSpe.WaitOne()
        For i = 0 To 16383
            outspe(i) = RealTimeSpectra(channel, i)
        Next
        MutexSpe.ReleaseMutex()
        Return outspe
    End Function
    Public Function GetCumulativeEnergy() As Double()
        Dim outCE(PackSize * ProducerCount - 1) As Double
        MutexCumulative.WaitOne()
        For i = 0 To PackSize * ProducerCount - 1
            outCE(TranslatePosition(i)) = CumulativeEnergy(i)
        Next
        MutexCumulative.ReleaseMutex()
        Return outCE
    End Function

    Public Function GetSingleEventEnergy() As Double()
        Dim outCE(PackSize * ProducerCount - 1) As Double
        MutexImmediatea.WaitOne()
        For i = 0 To PackSize * ProducerCount - 1
            outCE(TranslatePosition(i)) = SingleEventEnergy(i)
        Next
        MutexImmediatea.ReleaseMutex()
        Return outCE
    End Function

    Public Sub StartDataCaptureOnFile(file As String)
        SS_totalEvents = 0
        MutexFile.WaitOne()
        If fileEnable = False Then
            saveQueue = New Queue
            saveQueue.Clear()
            fileName = file
            fileEnable = True

        End If
        MutexFile.ReleaseMutex()

    End Sub

    Public Sub StopDataCaptureOnFile()
        caller.AppendToLog("[INFO] Stopping file capture")
        MutexFile.WaitOne()
        SS_totalEvents = 0
        If fileEnable = True Then
            objRawWriter = New System.IO.StreamWriter(fileName)
            objRawWriter.Write(Header & vbCrLf & vbCrLf)
            While saveQueue.Count > 0
                Dim ev As Evento = saveQueue.Dequeue
                objRawWriter.WriteLine(ev.eventId & ";" & ev.timecode.ToString & ";" & String.Join(";", ev.energy).Replace(",", "."))
            End While

        End If

        fileEnable = False

        If IsNothing(objRawWriter) Then
        Else

            objRawWriter.Close()
        End If

        MutexFile.ReleaseMutex()
        caller.AppendToLog("[INFO] Capture ended")
        SS_totalEvents = 0
    End Sub

    Class Evento
        Public timecode As UInt64
        Public energy(63) As Double
        Public valid As Boolean
        Public eventId As UInt32
        Public size
        Public Sub New(_size As Integer)
         size = _size
        End Sub
    End Class

    Dim MadaQueues(ProducerCount - 1) As Queue
    Dim semQueue(ProducerCount - 1) As Mutex

    Dim EventQueue As New Queue
    Dim semEventQueue As New Mutex
    Public noncorrelate As Boolean
    Dim overrun_time As DateTime = Now
    Dim Filler_Packet(ProducerCount - 1) As Long
    Public Sub UnpackMadaPacket(ByRef MadaPack As UInt32(), ssize As Integer, queueid As Integer)
        Dim ev As Evento
        Dim insync = 0
        Dim pxcnt As Integer = 0

        'ReDim ev.energy(PackSize)
        Dim packid
        Dim mpe
        Dim discard As Boolean = False
        '  For Each mpe In MadaPack
        For iiii = 0 To ssize - 1
            mpe = MadaPack(iiii)
            Select Case insync
                Case 0
                    If mpe = &HABBAFF10& Then
                        ev = New Evento(PackSize)
                        For i = 0 To 31
                            ev.energy(i) = 0

                        Next
                        discard = False

                        insync = 1
                    End If
                Case 1
                    insync = 2
                    If mpe = 1 Then
                        discard = True
                        Filler_Packet(queueid) += 1
                    End If

                Case 2
                    insync = 3
                Case 3
                    insync = 4
                Case 4
                    If mpe > 0 Then
                        If queueid = 0 Then
                            ssMADA1 = mpe
                        Else
                            ssMADA2 = mpe
                        End If
                    End If
                    ev.eventId = mpe
                    insync = 5
                Case 5
                    insync = 6
                    packid = mpe
                Case 6
                    ev.timecode = mpe
                    insync = 7
                Case 7
                    ev.timecode += mpe << 32
                    insync = 8
                    pxcnt = 0
                Case 8
                    If mpe = &HABBAFF10& Then
                        ev = New Evento(PackSize)
                        For i = 0 To 31
                            ev.energy(i) = 0

                        Next
                        insync = 1
                    Else
                        Dim uy As Integer = mpe And (&HFFFFFF)
                        If uy > &H800000 Then
                            uy = 0
                        End If
                        ' If uy = 0 Then
                        ' insync = 1
                        ' Else
                        ev.energy(pxcnt) = uy / ScaleFactor
                        pxcnt += 1
                        If (pxcnt = PackSize) Then
                            insync = 0
                            If discard = False Then
                                semQueue(queueid).WaitOne()

                                'Dim objWriter As New System.IO.StreamWriter("c:\temp\dump2.txt", True)
                                'objWriter.WriteLine("<--------------------------------------------------> ")
                                'objWriter.WriteLine("  ID: " & ev.eventId)
                                'objWriter.WriteLine("  TIMECODE: " & ev.timecode)
                                'objWriter.WriteLine(String.Join(";", ev.energy))
                                'objWriter.Close()


                                SS_lastspill = Now
                                If MadaQueues(queueid).Count > maxQueueSize Then
                                Else
                                    MadaQueues(queueid).Enqueue(ev)
                                    '   MadaQueues(queueid).Dequeue()
                                    'If (Now - overrun_time).TotalSeconds > 4 Then
                                    '    caller.AppendToLog("[WARNING] Raw fifo of board " & queueid + 1 & " overrun")
                                    '    overrun_time = Now
                                    'End If
                                End If

                                'If MadaQueues(0).Count > 0 Then
                                '    Dim tmpe As Evento = MadaQueues(0).Peek
                                '    Dim objWriter2 As New System.IO.StreamWriter("c:\temp\dump4_" & queueid & ".txt", True)
                                '    objWriter2.WriteLine("<-------------------------------------------------->  ID: " & tmpe.eventId & "  TC: " & tmpe.timecode & " MADA Q:" & MadaQueues(0).Count)
                                '    objWriter2.WriteLine(String.Join(";", tmpe.energy))
                                '    objWriter2.Close()
                                'End If

                                'File.AppendAllText("c:\temp\" & queueid, ev.eventId & ";" & packid & vbCrLf)
                                semQueue(queueid).ReleaseMutex()
                            End If
                        End If
                            'End If
                        End If
            End Select
        Next
    End Sub
    Public Sub LockAllQueueMutex()
        For i = 0 To ProducerCount - 1
            semQueue(i).WaitOne()
        Next
    End Sub
    Public Sub UNLockAllQueueMutex()
        For i = 0 To ProducerCount - 1
            semQueue(i).ReleaseMutex()
        Next
    End Sub

    Public Function VerifyAllValid(ByRef validArray() As Boolean) As Boolean
        For i = 0 To validArray.Length - 1
            If validArray(i) = False Then
                Return False
            End If
        Next
        Return True
    End Function

    Dim dropping_packet As DateTime = Now
    Dim droppingcount As Integer = 0

    Dim evnt_packet As DateTime = Now
    Dim droppingenvt As Integer = 0
    Public Sub MergeQueues()
        While 1


            Dim maxId As UInt64 = 0
            Dim valid(ProducerCount - 1) As Boolean
            LockAllQueueMutex()

            'find max in the queue
            For i = 0 To _nboard - 1
                If MadaQueues(i).Count = 0 Then
                    UNLockAllQueueMutex()
                    Return
                Else
                    Dim evt As Evento = MadaQueues(i).Peek
                    If SyncMode = 0 Then
                        maxId = Math.Max(maxId, evt.eventId)
                    Else
                        maxId = Math.Max(maxId, evt.timecode)
                    End If

                    ' maxId = Math.Max(maxId, evt.timecode)
                End If
            Next
            If _nboard > 1 And noncorrelate = False Then
                'allign all queue to the max
                For i = 0 To ProducerCount - 1
                    valid(i) = False
                Next
                While VerifyAllValid(valid) = False
                    For i = 0 To ProducerCount - 1

                        If MadaQueues(i).Count = 0 Then
                            UNLockAllQueueMutex()
                            Return
                        End If
                        Dim evt As Evento = MadaQueues(i).Peek
                        'drop events in queue older than max

                        If SyncMode = 0 Then
                            If evt.eventId < maxId Then
                                MadaQueues(i).Dequeue()
                                dropped_sync_package = dropped_sync_package + 1
                            Else
                                valid(i) = True
                                '  Exit While
                            End If
                        Else

                            Dim delta As Long
                            Dim a1 As ULong
                            Dim a2 As ULong
                            a1 = maxId
                            a2 = evt.timecode
                            If a1 > a2 Then
                                delta = a1 - a2
                            Else
                                delta = a2 - a1
                            End If

                            If delta > 10 Then
                                MadaQueues(i).Dequeue()
                                dropped_sync_package = dropped_sync_package + 1
                                'If (Now - dropping_packet).TotalSeconds > 4 Then
                                '    caller.AppendToLog("[INFO] Dropping data on queue " & i & " looking for sync. Message repeated: " & droppingcount)
                                '    dropping_packet = Now
                                '    droppingcount = 0
                                'Else
                                '    droppingcount += 1
                                'End If
                            Else
                                valid(i) = True
                                '  Exit While
                            End If
                        End If


                    Next
                End While
            End If


            Dim masterEvnt As New Evento(PackSize * ProducerCount - 1)
            '  ReDim masterEvnt.energy(PackSize * ProducerCount - 1)
            masterEvnt.valid = False
            Dim q = 0

            For i = 0 To ProducerCount - 1
                Dim evt As Evento
                If _nboard > 1 Then
                    evt = MadaQueues(i).Dequeue
                Else
                    If i >= 1 Then
                        evt = New Evento(32)
                        'ReDim evt.energy(32)
                    Else
                        evt = MadaQueues(i).Dequeue
                    End If
                End If
                If i = 0 Then
                    masterEvnt.eventId = evt.eventId
                    masterEvnt.timecode = evt.timecode
                    masterEvnt.valid = True

                End If
                For k = 0 To PackSize - 1
                    masterEvnt.energy(i * PackSize + k) = evt.energy(k)
                Next

            Next

            'Dim objWriter As New System.IO.StreamWriter("c:\temp\dump3.txt", True)
            'objWriter.WriteLine("<-------------------------------------------------->  ID: " & masterEvnt.eventId)
            'objWriter.WriteLine(String.Join(";", masterEvnt.energy))
            'objWriter.Close()

            UNLockAllQueueMutex()
            semEventQueue.WaitOne()
            If EventQueue.Count < 1000 Then
                EventQueue.Enqueue(masterEvnt)
            Else
                'If (Now - evnt_packet).TotalSeconds > 4 Then
                '    caller.AppendToLog("[WARNING] Loosing data on output queue")
                '    evnt_packet = Now
                '    droppingenvt = 0
                'Else
                '    droppingenvt += 1
                'End If
            End If
            semEventQueue.ReleaseMutex()
        End While
    End Sub
    Public Sub MADACommandsIface()
        While _acqExit = False
            Dim prodReady As Integer
            prodReady = 0
            For i = 0 To ProducerCount - 1
                If MadaQueues(i).Count > 10 Then
                    prodReady += 1
                End If
            Next
            If _nboard > 1 Then
                If prodReady = ProducerCount Then
                    MergeQueues()
                End If
            Else
                If prodReady = 1 Then
                    MergeQueues()
                End If
            End If


            System.Threading.Thread.Sleep(1)
        End While

    End Sub
    Dim saveQueue As Queue
    Public Sub ConsumerThrd()

        While _acqExit = False
            If EventQueue.Count > 10 Then
                While EventQueue.Count > 0
                    Dim envt As New Evento(63)
                    semEventQueue.WaitOne()
                    Dim envtt As Evento = EventQueue.Dequeue
                    envt.eventId = envtt.eventId
                    envt.size = envtt.size
                    envt.timecode = envtt.timecode
                    envt.valid = envtt.valid
                    For i = 0 To 63
                        envt.energy(i) = envtt.energy(i)
                    Next
                    semEventQueue.ReleaseMutex()
                    If envt.valid = True Then

                        For i = 0 To envt.energy.Length - 1
                            Dim eh As Double
                            If envt.energy(i) > 16383 Then
                                eh = 16383
                            Else
                                eh = envt.energy(i)
                            End If
                            MutexSpe.WaitOne()
                            RealTimeSpectra(i, eh) += 1
                            MutexSpe.ReleaseMutex()

                            MutexCumulative.WaitOne()
                            CumulativeEnergy(i) += envt.energy(i)
                            MutexCumulative.ReleaseMutex()

                            MutexImmediatea.WaitOne()
                            SingleEventEnergy(i) = envt.energy(i)
                            MutexImmediatea.ReleaseMutex()


                        Next

                        MutexFile.WaitOne()

                        If envt.eventId > 0 Then
                            SS_HighestTrigger = envt.eventId
                        End If

                        If fileEnable Then
                                Dim line As String
                                Try
                                    ' Dim EnergyReordered(envt.energy.Length - 1)
                                    Dim a As New Evento(63)
                                    For i = 0 To 63
                                        If i < envt.energy.Length Then
                                            a.energy(TranslatePosition(i)) = envt.energy(i)
                                            'EnergyReordered(TranslatePosition(i)) = envt.energy(i)
                                        End If
                                    Next
                                    a.timecode = envt.timecode
                                    a.size = envt.size
                                    a.eventId = envt.eventId
                                    a.valid = True
                                    saveQueue.Enqueue(a)
                                    SS_totalEvents = saveQueue.Count
                                    'line = envt.eventId & ";" & String.Join(";", EnergyReordered).Replace(",", ".")
                                    ' line = envt.eventId & ";" & envt.timecode & ";" & String.Join(";", EnergyReordered).Replace(",", ".")
                                    ' StringaDati = StringaDati & line & vbCrLf
                                    'objRawWriter.WriteLine(line)
                                Catch ex As Exception

                                End Try

                            End If
                            MutexFile.ReleaseMutex()
                        End If


                End While
            Else
                System.Threading.Thread.Sleep(1)
            End If
        End While
    End Sub

    Public Sub DummyProducer1()
        Dim ts
        If smallpack = False Then
            ts = 16384 * 1
        Else
            ts = 400
        End If

        Dim DataChunk(ts * 10) As UInt32

        Dim evntCnt = 6
        Dim EVNTSize As Integer = 4 + 32


        caller.AppendToLog("[INFO] MADA 1 started")
        While (_acqExit = False)
            'Dim q = 0
            'While q + EVNTSize < DataChunk.Length - 1


            '    DataChunk(q) = &HFFAABBAA&

            '    DataChunk(q + 1) = evntCnt * 175
            '    DataChunk(q + 2) = 0
            '    DataChunk(q + 3) = evntCnt
            '    For i = 0 To 31
            '        DataChunk(q + 4 + i) = i * 100
            '    Next

            '    evntCnt += 1
            '    q += EVNTSize



            'End While
            If board1.FPGAReadBurstUint(DataChunk, ts, &HFE000000&) = False Then
                'MsgBox("Acquisition timeout", MsgBoxStyle.Information + vbOK)
                Exit Sub
            End If
            'Dim objWriter As New System.IO.StreamWriter("c: \temp\dump1.txt", True)
            'For i = 0 To ts - 1
            '    If DataChunk(i) = &HABBAFF10& Then
            '        objWriter.WriteLine("<-------------------------------------------------->")
            '    End If
            '    objWriter.WriteLine(Hex(DataChunk(i)))
            'Next
            'objWriter.Close()
            UnpackMadaPacket(DataChunk, ts, 0)

            If _nboard > 1 Then
                If board2.FPGAReadBurstUint(DataChunk, ts, &HFE000000&) = False Then
                    'MsgBox("Acquisition timeout", MsgBoxStyle.Information + vbOK)
                    Exit Sub
                End If
                UnpackMadaPacket(DataChunk, ts, 1)
            End If
            '    System.Threading.Thread.Sleep(100)
        End While

        caller.AppendToLog("[INFO] MADA 1 stopped")
    End Sub

    Public Sub DummyProducer2()
        Dim ts = 16384 * 1
        Dim DataChunk(ts * 10) As UInt32

        Dim evntCnt = 18
        Dim EVNTSize As Integer = 4 + 32
        caller.AppendToLog("[INFO] MADA 2 started")
        While (_acqExit = False)
            'Dim q = 0
            'While q + EVNTSize < DataChunk.Length - 1

            'DataChunk(q) = &HFFAABBAA&

            'DataChunk(q + 1) = evntCnt * 175
            'DataChunk(q + 2) = 0
            'DataChunk(q + 3) = evntCnt
            'For i = 0 To 31
            'DataChunk(q + 4 + i) = i * 100
            'Next

            'evntCnt += 1
            'q += EVNTSize

            ' End While

            'FPGAclass.CapturePackets(thisTransfer, 32, NumericUpDown3.Value, buffer)
            If board2.FPGAReadBurstUint(DataChunk, ts, &HFE000000&) = False Then
                'MsgBox("Acquisition timeout", MsgBoxStyle.Information + vbOK)
                Exit Sub
            End If



            UnpackMadaPacket(DataChunk, ts, 1)
            ' System.Threading.Thread.Sleep(100)
        End While
        caller.AppendToLog("[INFO] MADA 2 stopped")
    End Sub
    Public Sub New()


        For i = 0 To ProducerCount - 1
            MadaQueues(i) = New Queue
            semQueue(i) = New Mutex
        Next

        '  StartDataCaptureOnFile("c:\temp\prova.txt")

        System.Threading.Thread.Sleep(1000)
        ' MergeQueues()
    End Sub
    Public Sub ConnectToBoard(bd1 As FPGA_FX3, bd2 As FPGA_FX3)
        board1 = bd1
        board2 = bd2
    End Sub
    Dim t1, t2, t3, t4 As Thread
    Public Sub StartAcquisition(nboard As Integer)
        caller.AppendToLog("[INFO] Starting Acquisition")
        dropped_sync_package = 0
        _acqExit = False
        For i = 0 To Filler_Packet.Length - 1
            Filler_Packet(i) = 0
        Next


        Dim ts As New ThreadStart(AddressOf MADACommandsIface)
        t4 = New Thread(ts)
        t4.Start()



        Dim pc As New ThreadStart(AddressOf ConsumerThrd)
        t3 = New Thread(pc)
        t3.Start()
        System.Threading.Thread.Sleep(150)


        For i = 0 To ProducerCount - 1
            MadaQueues(i).Clear()
        Next
        EventQueue.Clear()

        SS_HighestTrigger = 0
        SS_totalEvents = 0
        SS_startTime = Now

        caller.board1.FPGAWriteReg(0, &H601)
        caller.board1.FPGAWriteReg(1, &H601)
        caller.board1.FPGAWriteReg(0, &H601)


        ' If nboard > 1 Then
        ' Dim pd2 As New ThreadStart(AddressOf DummyProducer2)
        ' t2 = New Thread(pd2)
        ' t2.Start()
        'End If

        If nboard > 0 Then
            Dim pd1 As New ThreadStart(AddressOf DummyProducer1)
            t1 = New Thread(pd1)
            t1.Start()

        End If
        _nboard = nboard
        isRunning = True
    End Sub

    Public Sub StopAcquisition()
        caller.AppendToLog("[INFO] Stopping")
        isRunning = False
        _acqExit = True
        While t1.IsAlive
            System.Threading.Thread.Sleep(20)
            'wait
        End While
        'If Not IsNothing(t2) Then
        '    While t2.IsAlive
        '        System.Threading.Thread.Sleep(20)
        '        'wait
        '    End While
        'End If
        If Not IsNothing(t3) Then
            While t3.IsAlive
                System.Threading.Thread.Sleep(20)
                'wait
            End While
        End If

        If Not IsNothing(t4) Then
            While t4.IsAlive
                System.Threading.Thread.Sleep(20)
                'wait
            End While
        End If


    End Sub
End Class
