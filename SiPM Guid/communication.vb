Imports DT5550W_P_lib

Public Class communication



    '  Public JsonFilePath As String

    Public Enum tConnectionMode
        USB = 0
        ETHERNET = 1
        VME = 2
    End Enum

    Public Enum tModel
        V2495 = 0
        DT5550 = 1
    End Enum

    Public Enum tError
        OK = 0
        NOT_CONNECTED = 1
        ALREADY_CONNECTED = 2
        UNSUPPORTED_DEVICE = 3
        ALREADY_DISCONNECTED = 4
        ERROR_GENERIC = 5
        ERROR_INTERFACE = 6
        ERROR_FPGA = 7
        ERROR_TRANSFER_MAX_LENGTH = 8
        NO_DATA_AVAILABLE = 9
        TOO_MANY_DEVICES_CONNECTED = 10
        INVALID_HANDLE = 11
        INVALID_HARDWARE = 12
    End Enum

    Private _boardModel As tModel
    Private _isconnected As Boolean
    Private V2495Handle As UInt32
    Private DT5550Handle As New IntPtr


    Dim mtx As New Threading.Mutex


    Public Sub New()

        Try
            mtx.ReleaseMutex()
        Catch ex As Exception

        End Try

    End Sub

    'Public Sub SetFile(filepath As String)
    'JsonFilePath = filepath
    'End Sub

    Public Function IsFileCompatible(model As String) As Boolean
        Dim modelcode As New tModel
        If model = "V2495" Then
            modelcode = tModel.V2495
        ElseIf model = "DT5550" Then
            modelcode = tModel.DT5550
        End If
        If modelcode = _boardModel Then
            Return True
        Else
            Return False
        End If

    End Function
    Public Function Connect(ConnectionMode As tConnectionMode, model As tModel, param0 As String) As tError

        If _isconnected = True Then
            Return tError.ALREADY_CONNECTED
        End If

        Select Case model
            'Case tModel.V2495
            '    V2495_Startup()
            '    Select Case ConnectionMode
            '        Case tConnectionMode.USB
            '            mtx.WaitOne()
            '            Dim ierror = V2495_AttachNewDevice(0, param0, 0, 0, V2495Handle)
            '            mtx.ReleaseMutex()
            '            Dim error_t = IErrorV2495ToNETError(ierror)
            '            If error_t = tError.OK Then
            '                _boardModel = model
            '                _isconnected = True
            '            Else
            '                _isconnected = False
            '            End If
            '            Return error_t
            'Case tConnectionMode.ETHERNET

            '            mtx.WaitOne()
            '            Dim ierror = V2495_AttachNewDevice(1, param0, 9764, 6234, V2495Handle)
            '            mtx.ReleaseMutex()
            '            Dim error_t = IErrorV2495ToNETError(ierror)
            '            If error_t = tError.OK Then
            '                _boardModel = model
            '                _isconnected = True
            '            Else
            '                _isconnected = False
            '            End If
            '            Return error_t
            '        Case tConnectionMode.VME

            '    End Select
            Case tModel.DT5550
                USB3_Init()
                Select Case ConnectionMode
                    Case tConnectionMode.USB
                        mtx.WaitOne()
                        Dim ierror = USB3_ConnectDevice(param0, DT5550Handle)
                        mtx.ReleaseMutex()
                        ' Dim error_t = IErrorV2495ToNETError(ierror)
                        If ierror = 0 Then
                            _boardModel = model
                            _isconnected = True
                            Return tError.OK
                        Else
                            _isconnected = False
                            Return tError.ERROR_GENERIC
                        End If
                        'Return error_t
                    Case tConnectionMode.ETHERNET


                    Case tConnectionMode.VME
                End Select
            Case Else
                Return tError.UNSUPPORTED_DEVICE
        End Select






        ' Return tError.OK
    End Function

    Public Function Disconnect() As tError
        If _isconnected = False Then
            Return tError.ALREADY_DISCONNECTED
        End If

        Select Case _boardModel
            'Case tModel.V2495
            '    mtx.WaitOne()
            '    Dim ierror = V2495_DeleteDevice(V2495Handle)
            '    mtx.ReleaseMutex()
            '    _isconnected = False
            '    Return IErrorV2495ToNETError(ierror)

            Case tModel.DT5550
            Case Else
                Return tError.UNSUPPORTED_DEVICE
        End Select

        ' Return tError.OK
    End Function

    Public Function IErrorV2495ToNETError(ierror As ULong) As tError

        Select Case ierror
            Case 0
                Return tError.OK
            Case 1
                Return tError.ERROR_GENERIC
            Case 2
                Return tError.ERROR_INTERFACE
            Case 3
                Return tError.ERROR_FPGA
            Case 4
                Return tError.ERROR_TRANSFER_MAX_LENGTH
            Case 5
                Return tError.NOT_CONNECTED
            Case 6
                Return tError.NO_DATA_AVAILABLE
            Case 7
                Return tError.TOO_MANY_DEVICES_CONNECTED
            Case 8
                Return tError.INVALID_HANDLE
            Case 9
                Return tError.INVALID_HARDWARE
            Case Else
                Return tError.ERROR_GENERIC
        End Select

    End Function

    Public Sub GetMessage(errorcode As tError)
        If errorcode = 1 Then
            MsgBox("The device is not connected!", vbCritical + vbOKOnly)
        ElseIf errorcode = 2 Then
            MsgBox("The device is already connected!", vbCritical + vbOKOnly)
        ElseIf errorcode = 3 Then
            MsgBox("The device is not supported!", vbCritical + vbOKOnly)
        ElseIf errorcode = 4 Then
            MsgBox("The device is already disconnected!", vbCritical + vbOKOnly)
        ElseIf errorcode = 5 Then
            MsgBox("Error!", vbCritical + vbOKOnly)
        ElseIf errorcode = 6 Then
            MsgBox("Interface error!", vbCritical + vbOKOnly)
        ElseIf errorcode = 7 Then
            MsgBox("FPGA error!", vbCritical + vbOKOnly)
        ElseIf errorcode = 8 Then
            MsgBox("Exceeding the maximum transfer length!", vbCritical + vbOKOnly)
        ElseIf errorcode = 9 Then
            MsgBox("No data available!", vbCritical + vbOKOnly)
        ElseIf errorcode = 10 Then
            MsgBox("Too many devices connected!", vbCritical + vbOKOnly)
        ElseIf errorcode = 11 Then
            MsgBox("The handle is invalid!", vbCritical + vbOKOnly)
        ElseIf errorcode = 12 Then
            MsgBox("The hardware is invalid!", vbCritical + vbOKOnly)
        End If

    End Sub

    Public Function SetRegister(address As UInt32, value As UInt32) As tError

        If _isconnected = False Then
            Return tError.NOT_CONNECTED
        End If

        Select Case _boardModel
            'Case tModel.V2495
            '    mtx.WaitOne()
            '    Dim ierror = V2495_DHA_WriteReg(value, address, V2495Handle, 0)
            '    mtx.ReleaseMutex()
            '    Return IErrorV2495ToNETError(ierror)
            Case tModel.DT5550
                mtx.WaitOne()
                Dim ierror = USB3_WriteReg(value, address, DT5550Handle)
                mtx.ReleaseMutex()
                If ierror < &HFFFFFFFF& Then
                    Return ierror
                Else
                    Return tError.ERROR_FPGA
                End If

            Case Else
                Return tError.UNSUPPORTED_DEVICE
        End Select

        '  Return tError.OK
    End Function

    Public Function GetRegister(address As UInt32, ByRef value As UInt32) As tError
        If _isconnected = False Then
            Return tError.NOT_CONNECTED
        End If

        Select Case _boardModel
            'Case tModel.V2495
            '    mtx.WaitOne()
            '    Dim ierror = V2495_DHA_ReadReg(value, address, V2495Handle, 0)
            '    mtx.ReleaseMutex()
            '    Return IErrorV2495ToNETError(ierror)
            Case tModel.DT5550
                mtx.WaitOne()
                Dim ierror = USB3_ReadReg(value, address, DT5550Handle)
                mtx.ReleaseMutex()
                If ierror < &HFFFFFFFF& Then
                    Return ierror
                Else
                    Return tError.ERROR_FPGA
                End If


            Case Else
                Return tError.UNSUPPORTED_DEVICE
        End Select

        'Return tError.OK
    End Function

    Public Function SetIICBA(cntrl As UInt32, status As UInt32)
        USB3_SetIICControllerBaseAddress(cntrl, status, DT5550Handle)
    End Function

    Public Function HVControl(onoff As Boolean, voltage As Single)
        USB3_SetHV(onoff, voltage, DT5550Handle)
    End Function

    Public Function OffsetControl(top As Boolean, voltage As Single)
        USB3_SetOffset(top, voltage, DT5550Handle)
    End Function


    Public Function ImpedenceControl(R50 As Boolean)
        USB3_SetImpedance(R50, DT5550Handle)
    End Function

    Public Function WriteMem(address As UInt32, size As UInt32, value() As UInt32) As tError
        If _isconnected = False Then
            Return tError.NOT_CONNECTED
        End If

        Select Case _boardModel
            Case tModel.V2495

            Case tModel.DT5550

            Case Else
                Return tError.UNSUPPORTED_DEVICE
        End Select

        '  Return tError.OK
    End Function

    Public Function ReadMed(address As UInt32, size As UInt32, ByRef value() As UInt32) As tError
        If _isconnected = False Then
            Return tError.NOT_CONNECTED
        End If

        Select Case _boardModel
            Case tModel.V2495

            Case tModel.DT5550

            Case Else
                Return tError.UNSUPPORTED_DEVICE
        End Select
        '
        ' Return tError.OK
    End Function

    Public Function WriteFIFO(address As UInt32, size As UInt32, value() As UInt32) As tError
        If _isconnected = False Then
            Return tError.NOT_CONNECTED
        End If

        Select Case _boardModel
            Case tModel.V2495

            Case tModel.DT5550

            Case Else
                Return tError.UNSUPPORTED_DEVICE
        End Select

        ' Return tError.OK
    End Function

    Public Function ReadFIFO(address As UInt32, ByRef value() As Int32, ByRef lenght As Integer, ch As Integer) As tError
        If _isconnected = False Then
            Return tError.NOT_CONNECTED
        End If

        Select Case _boardModel
            'Case tModel.V2495
            '    mtx.WaitOne()
            '    Dim ierror = V2495_DHA_ReadArray(value, address, lenght, False, V2495Handle, ch)
            '    mtx.ReleaseMutex()
            '    ' Return tError.OK
            '    Return IErrorV2495ToNETError(ierror)
            Case tModel.DT5550

            Case Else
                Return tError.UNSUPPORTED_DEVICE
        End Select

        ' Return tError.OK
    End Function

    Public Function ReadData(address As UInt32, ByRef value() As UInt32, lenght As Integer, bus_mode As Integer, timeout As UInt32, ByRef read_data As UInt32, ByRef valid_data As UInt32) As tError
        If _isconnected = False Then
            Return tError.NOT_CONNECTED
        End If

        Select Case _boardModel
            Case tModel.V2495

            Case tModel.DT5550
                mtx.WaitOne()
                Dim ierror = USB3_ReadData(value, lenght, address, bus_mode, timeout, DT5550Handle, read_data, valid_data)
                mtx.ReleaseMutex()
                ' Return tError.OK
                Return ierror
            Case Else
                Return tError.UNSUPPORTED_DEVICE
        End Select

        ' Return tError.OK
    End Function


End Class
