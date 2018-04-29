Imports System.Runtime.InteropServices
Imports System.IO

    Module ImportDLL_niusb3
        Const dllpath = ""


        <DllImport(dllpath & "niusb3_core.dll", CallingConvention:=CallingConvention.Cdecl)>
        Function NI_USB3_Init() As UInteger
        End Function

        <DllImport(dllpath & "niusb3_core.dll", CallingConvention:=CallingConvention.Cdecl)>
        Function NI_USB3_ConnectDevice(ByVal SN As String,
                                   handle As IntPtr) As UInteger
        End Function

        <DllImport(dllpath & "niusb3_core.dll", CallingConvention:=CallingConvention.Cdecl)>
        Function NI_USB3_ListDevices(ByRef ListOfDevice As Char,
                                 ByRef model As Char,
                                 ByRef count As Integer) As UInteger
        End Function

        <DllImport(dllpath & "niusb3_core.dll", CallingConvention:=CallingConvention.Cdecl)>
        Function NI_USB3_WriteReg(ByVal data As UInt32,
                              ByVal address As UInt32,
                              handle As IntPtr) As UInteger
        End Function

        <DllImport(dllpath & "niusb3_core.dll", CallingConvention:=CallingConvention.Cdecl)>
        Function NI_USB3_ReadReg(ByRef data As UInt32,
                             ByVal address As UInt32,
                             handle As IntPtr) As UInteger
        End Function

        <DllImport(dllpath & "niusb3_core.dll", CallingConvention:=CallingConvention.Cdecl)>
        Function NI_USB3_WriteData(data As IntPtr,
                               ByVal count As Int32,
                               ByVal address As UInt32,
                               ByVal bus_mode As UInteger,
                               ByVal timeout As UInt32,
                               handle As IntPtr,
                               ByRef written_data As UInt32) As UInteger
        End Function

    <DllImport(dllpath & "niusb3_core.dll", CallingConvention:=CallingConvention.Cdecl)>
    Function NI_USB3_ReadData(data As IntPtr,
                              ByVal count As UInt32,
                              ByVal address As UInt32,
                              ByVal bus_mode As UInteger,
                              ByVal timeout As UInt32,
                              handle As IntPtr,
                              ByRef read_data As UInt32,
                              ByRef valid_data As UInt32) As UInteger
    End Function

    <DllImport(dllpath & "niusb3_core.dll", CallingConvention:=CallingConvention.Cdecl)>
    Function NI_USB3_SetHV(ByVal Enable As Boolean,
                         ByVal voltage As Single,
                         handle As IntPtr) As UInteger
    End Function

    <DllImport(dllpath & "niusb3_core.dll", CallingConvention:=CallingConvention.Cdecl)>
    Function NI_USB3_SetOffset(ByVal Top As Boolean,
                         ByVal voltage As UInt32,
                         handle As IntPtr) As UInteger
    End Function


    <DllImport(dllpath & "niusb3_core.dll", CallingConvention:=CallingConvention.Cdecl)>
    Function NI_USB3_SetImpedance(ByVal R50 As Boolean,
                         handle As IntPtr) As UInteger
    End Function


    <DllImport(dllpath & "niusb3_core.dll", CallingConvention:=CallingConvention.Cdecl)>
    Function NI_USB3_SetIICControllerBaseAddress(ByVal ControlAddress As UInt32,
                         ByVal StatusAddress As UInt32,
                         handle As IntPtr) As UInteger
    End Function

    Public Function USB3_Init() As UInteger
            Return NI_USB3_Init()
        End Function

        Public Function USB3_ConnectDevice(ByVal SN As String,
                                       ByRef handle As IntPtr) As UInteger
            handle = Marshal.AllocHGlobal(1000)
            Return NI_USB3_ConnectDevice(SN, handle)
        End Function

        Public Function USB3_ListDevices(ByRef ListOfDevice As Char,
                                     ByRef model As Char,
                                     ByRef count As Integer) As UInteger
            Return NI_USB3_ListDevices(ListOfDevice, model, count)
        End Function

        Public Function USB3_WriteReg(data As UInt32,
                                  address As UInt32,
                                  handle As IntPtr) As UInteger
            Return NI_USB3_WriteReg(data, address, handle)
        End Function

        Public Function USB3_ReadReg(ByRef data As UInt32,
                                 address As UInt32,
                                 handle As IntPtr) As UInteger

            '  Dim sizes = Marshal.SizeOf(GetType(UInt32))
            ' Dim ptr As IntPtr = Marshal.AllocHGlobal(sizes * 2)

            Return NI_USB3_ReadReg(data, address, handle)

            ' Marshal.Copy(ptr, data, 0, 1)
            ' Marshal.FreeHGlobal(ptr)

            ' Return status


        End Function


    Public Function USB3_WriteData(ByRef data() As Long,
                               count As Int32,
                               address As UInt32,
                               bus_mode As UInteger,
                               timeout As UInt32,
                               handle As IntPtr,
                               ByRef written_data As UInt32) As UInteger
        Dim datab(count * 4) As Byte
        Buffer.BlockCopy(data, 0, datab, 0, count * 4)

        Dim sizes = Marshal.SizeOf(GetType(UInt32))
        Dim ptr As IntPtr = Marshal.AllocHGlobal(sizes * count * 2)
        Marshal.Copy(data, 0, ptr, count)
        Dim status = NI_USB3_WriteData(ptr, count, address, bus_mode, timeout, handle, written_data)

        Marshal.FreeHGlobal(ptr)
        Return status

    End Function

    Public Function USB3_ReadData(ByRef data() As UInt32,
                               count As Int32,
                               address As UInt32,
                               bus_mode As UInteger,
                               timeout As UInt32,
                               handle As IntPtr,
                               ByRef read_data As UInt32,
                               ByRef valid_data As UInt32) As UInteger
        Dim datab(count * 4) As Byte
        Dim sizes = Marshal.SizeOf(GetType(UInt32))
        Dim ptr As IntPtr = Marshal.AllocHGlobal(sizes * count * 2)
        Dim status = NI_USB3_ReadData(ptr, count, address, bus_mode, timeout, handle, read_data, valid_data)
        Marshal.Copy(ptr, datab, 0, count * 4)
        Marshal.FreeHGlobal(ptr)
        Buffer.BlockCopy(datab, 0, data, 0, count * 4)

        Return status

    End Function


    Function USB3_SetHV(ByVal Enable As Boolean,
                         ByVal voltage As Single,
                         handle As IntPtr) As UInteger
        Return NI_USB3_SetHV(Enable, voltage, handle)
    End Function


    Function USB3_SetOffset(ByVal Top As Boolean,
                         ByVal voltage As UInt32,
                         handle As IntPtr) As UInteger

        Return NI_USB3_SetOffset(Top, voltage, handle)
    End Function


    Function USB3_SetImpedance(ByVal R50 As Boolean,
                         handle As IntPtr) As UInteger

        Return NI_USB3_SetImpedance(R50, handle)
    End Function

    Function USB3_SetIICControllerBaseAddress(ByVal ControlAddress As UInt32,
                           ByVal StatusAddress As UInt32,
                           handle As IntPtr) As UInteger
        Return NI_USB3_SetIICControllerBaseAddress(ControlAddress, StatusAddress, handle)
    End Function


End Module


