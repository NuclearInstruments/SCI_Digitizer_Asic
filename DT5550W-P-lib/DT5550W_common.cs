using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DT5550W_P_lib
{

    public enum T0Mode { SOFTWARE_STARTRUN = 0, SOFTWARE_PERIODIC = 1, EXTERNAL = 2 };

    public enum t_AsicModels
    {
        PETIROC = 0,
        MAROC = 1,
        CITIROC = 2,
        GEMROC = 3,
        PHOTOROC = 4,
        INVALID = 99
    }
    public struct tChannelMapping
    {
        public int X;
        public int Y;
        public int ASICID;
    }


    public struct t_BoardInfo
        {
            public enum t_AsicType { PETIROC = 0, MAROC = 1, CITIROC = 2, INVALID=99 };
            public t_AsicType AsicType;
            public int totalAsics;
            public int channelsPerAsic;
            public int totalChannels;
            public double FPGATimecode_ns;
            public double Coarse_ns;
            public double Fine_ns;
            public string SerialNumber;
            public int DigitalDataPacketSize;
            public enum t_DefaultDetectorLayout { MATRIX_4x4 = 0, MATRIX_8x8 = 1, LINEAR = 2 };
            public t_DefaultDetectorLayout DefaultDetectorLayout;
        }


    public class PHY_LINK
    {


        public enum USB_BUS_MODE
        {
            REG_ACCESS = 0,
            STREAMING = 1
        };


        IntPtr Handle;

        [DllImport("niusb3_core.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern int NI_USB3_Init(
        );

        [DllImport("niusb3_core.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern int NI_USB3_ListDevices(
            StringBuilder ListOfDevice,
            String model,
            ref int Count
        );


        [DllImport("niusb3_core.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern int NI_USB3_ConnectDevice(
                String serial_number,
                ref IntPtr handle);

        public PHY_LINK()
        {
            Handle = new IntPtr();
            NI_USB3_Init();
        }

        [DllImport("niusb3_core.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern int NI_USB3_WriteData(
                UInt32[] data,
                UInt32 count,
                UInt32 address,
                USB_BUS_MODE bus_mode,
                UInt32 timeout_ms,
                ref IntPtr handle,
                ref IntPtr written_data);

        [DllImport("niusb3_core.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern int NI_USB3_ReadData(
                UInt32[] data,
                UInt32 count,
                UInt32 address,
                USB_BUS_MODE bus_mode,
                UInt32 timeout_ms,
                ref IntPtr handle,
                ref IntPtr read_data,
                ref IntPtr valid_data
                );

        [DllImport("niusb3_core.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern int NI_USB3_WriteReg(
                UInt32 data,
                UInt32 address,
                ref IntPtr handle
                );

        [DllImport("niusb3_core.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern int NI_USB3_ReadReg(
                ref UInt32 data,
                UInt32 address,
                ref IntPtr handle
                );

        [DllImport("niusb3_core.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern int NI_USB3_IIC_WriteData(
                Byte address,
                Byte[] value,
                int len, ref IntPtr handle);

        [DllImport("niusb3_core.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern int NI_USB3_IIC_ReadData(
        Byte address,
        Byte[] value,
        int len,
        Byte[] value_read,
        int len_read,
        ref IntPtr handle);

        [DllImport("niusb3_core.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern int NI_USB3_SetIICControllerBaseAddress(
            UInt32 ControlAddress,
            UInt32 StatusAddress,
            ref IntPtr handle);


        [DllImport("niusb3_core.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern int NI_USB3_SetHV(
                   bool Enable,
                   float voltage,
                   ref IntPtr handle);

        [DllImport("niusb3_core.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern int NI_USB3_GetHV(
            ref bool Enable,
            ref float voltage,
            ref float current,
            ref IntPtr handle);


        [DllImport("niusb3_core.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern int NI_USB3_GetDT5550_DGBoardInfo(
            StringBuilder model,
            ref int asic_count,
            ref int SN,
            ref IntPtr handle);


        [DllImport("niusb3_core.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern int NI_USB3_GetDT5550WTempSens(
            int address,
            ref float temperature,
            ref IntPtr handle);





        public int NI_USB3_WriteData_M(
                UInt32[] data,
                UInt32 count,
                UInt32 address,
                USB_BUS_MODE bus_mode,
                UInt32 timeout_ms,
                ref UInt32 written_data)
        {
            //IntPtr unmanagedPointer = Marshal.AllocHGlobal((Int32) count);
            IntPtr written_data_ptr = new IntPtr(written_data);
            int retcode = NI_USB3_WriteData(
               data,
               count,
               address,
               bus_mode,
               timeout_ms,
               ref Handle,
               ref written_data_ptr);
            written_data = (UInt32)written_data_ptr.ToInt32();

            return retcode;
        }

        public int NI_USB3_ReadData_M(
                UInt32[] data,
                UInt32 count,
                UInt32 address,
                USB_BUS_MODE bus_mode,
                UInt32 timeout_ms,
                ref UInt32 read_data,
                ref UInt32 valid_data
                )
        {
            IntPtr read_data_ptr = new IntPtr(1);
            IntPtr valid_data_ptr = new IntPtr(1);
            int retcode = NI_USB3_ReadData(
                    data,
                    count,
                    address,
                    bus_mode,
                    timeout_ms,
                    ref Handle,
                    ref read_data_ptr,
                    ref valid_data_ptr
                    );
            read_data = (UInt32)read_data_ptr.ToInt32();
            valid_data = (UInt32)valid_data_ptr.ToInt32();

            return retcode;
        }


        public int NI_USB3_WriteReg_M(UInt32 data,
                UInt32 address)
        {
            return NI_USB3_WriteReg(data, address, ref Handle);
        }

        public int NI_USB3_ReadReg_M(ref UInt32 data,
          UInt32 address)
        {
            return NI_USB3_ReadReg(ref data, address, ref Handle);
        }



        unsafe public int IIC_WriteData(byte address, byte[] value, int len)
        {

            if (NI_USB3_IIC_WriteData(
                address,
                value,
                len, ref Handle) != 0) return -1; ;
            //int i;

            //if (NI_USB3_WriteReg(0, RFA_IIC_BA + 0, ref Handle) != 0)
            //    return -1;
            //System.Threading.Thread.Sleep(5);
            //if (NI_USB3_WriteReg(1 << 15, RFA_IIC_BA + 0, ref Handle) != 0)
            //    return -1;
            //System.Threading.Thread.Sleep(5);
            //if (NI_USB3_WriteReg(0, RFA_IIC_BA + 0, ref Handle) != 0)
            //    return -1;
            //System.Threading.Thread.Sleep(5);
            //if (NI_USB3_WriteReg(1 << 8, RFA_IIC_BA + 0, ref Handle) != 0)
            //    return -1;
            //System.Threading.Thread.Sleep(5);

            //if (NI_USB3_WriteReg((UInt32)(address << 1) + (1 << 11), RFA_IIC_BA + 0, ref Handle) != 0)
            //    return -1;
            //System.Threading.Thread.Sleep(5);

            //for (i = 0; i < len; i++)
            //{
            //    if (NI_USB3_WriteReg((UInt32)(value[i]) + (1 << 11), RFA_IIC_BA + 0, ref Handle) != 0)
            //        return -1;
            //    System.Threading.Thread.Sleep(5);
            //}

            //if (NI_USB3_WriteReg((1 << 9), RFA_IIC_BA + 0, ref Handle) != 0)
            //    return -1;

            //System.Threading.Thread.Sleep(5);

            return 0;
        }



        public List<String> GetDeviceList()
        {


            StringBuilder sb = new StringBuilder(2048);
            int count = 0;
            int retcode = NI_USB3_ListDevices(
            sb,
            null,
            ref count);

            if (retcode != 0)
                return null;

            List<String> devs = new List<string>();

            foreach (String a in sb.ToString().Split(';'))
            {
                if (a.Length > 1)
                    devs.Add(a);
            }

            return devs;
        }

        public int NI_USB3_ConnectDevice_M(String serial_number)
        {
            return NI_USB3_ConnectDevice(
                  serial_number,
                  ref Handle);
        }

        public int NI_USB3_SetIICControllerBaseAddress_M(UInt32 ControlAddress,
            UInt32 StatusAddress)
        {
            return NI_USB3_SetIICControllerBaseAddress(
            ControlAddress,
            StatusAddress,
            ref Handle);
        }

        public int NI_USB3_SetHV_M(
           bool Enable,
           float voltage)
        {
            return NI_USB3_SetHV(
                               Enable,
                               voltage,
                               ref Handle);
        }


        public int NI_USB3_GetHV_M(
                 ref bool Enable,
                 ref float voltage,
                 ref float current
                 )
        {
            return NI_USB3_GetHV(
                ref Enable,
                ref voltage,
                ref current,
                ref Handle);
        }


        public int NI_USB3_GetDT5550WTempSens_M(
               int address,
               ref float temperature)
        {
            return NI_USB3_GetDT5550WTempSens(
                address,
                ref temperature,
                ref Handle);
        }

        public int NI_USB3_GetDT5550_DGBoardInfo_M(
                StringBuilder model,
                ref int asic_count,
                ref int SN)
        {
            return NI_USB3_GetDT5550_DGBoardInfo(
                 model,
                ref asic_count,
                ref SN,
                ref Handle);
        }

    }

}
