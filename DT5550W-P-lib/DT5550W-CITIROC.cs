using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DT5550W_P_lib
{
    public class DT5550W_CITIROC
    {

        const UInt32 RFA_PTROC_BA_A = 0x80010000;
        const UInt32 RFA_PTROC_BA_B = 0x80020000;
        const UInt32 RFA_PTROC_BA_C = 0x80030000;
        const UInt32 RFA_PTROC_BA_D = 0x80040000;
        const UInt32 RFA_PTROC_PROG = 20;
        const UInt32 RFA_PTROC_PROGMONITOR = 21;

        const UInt32 DAQ_VETO = 0xFFFFF907;
        const UInt32 DAQ_VETO_sw = 0xFFFFF90B;
        const UInt32 DAQ_ANALOG_MONITOR = 0xFFFFF916;
        const UInt32 DAQ_FLUSH_FIFO = 0xFFFFF908;
        const UInt32 DIGOUT_LEN = 0xFFFFF90C;

        const UInt32 SIGGEN_PERIOD = 0xFFFFF90E;
        const UInt32 SIGGEN_ENABLE = 0xFFFFF90D;

        const UInt32 T0_SOURCE = 0xFFFFF911;
        const UInt32 T0_SW = 0xFFFFF912;
        const UInt32 T0_SWMODE = 0xFFFFF913;
        const UInt32 T0_FREQ = 0xFFFFF914;

        const UInt32 DAQ_TRIGGER_MODE = 0xFFFFF917;
        const UInt32 DAQ_TRIGGER_FRAME = 0xFFFFF918;

        const UInt32 DAQ_EXTERNAL_VETO = 0xFFFFF919;
        const UInt32 DAQ_TRIGGER_EXT = 0xFFFFF91A;
        const UInt32 DAQ_RESET_TDC_T0 = 0xFFFFF91B;

        const UInt32 RFA_IIC_CONTROL = 0x80050008;
        const UInt32 RFA_IIC_STATUS = 0x80050009;

        const UInt32 RFA_PTROC_ASIC_DISABLE = 0xFFFFF915;
        int DLL_ASIC_COUNT = 4;
        public CitirocConfig CitirocCfg;

        public List<CitirocConfig> pCFG;

        public string SerialNumber;

        t_AsicModels DLL_CURRENT_ASIC = t_AsicModels.CITIROC;
        
        PHY_LINK phy;

        public class CitirocConfig
        {
            const int NbChannels = 32;
            public int[] sc_calibDacT = new int[NbChannels];
            public int[] sc_calibDacQ = new int[NbChannels];
            public int sc_enDiscri;
            public int sc_ppDiscri;
            public int sc_latchDiscri;
            public int sc_enDiscriT;
            public int sc_ppDiscriT;
            public int sc_enCalibDacQ;
            public int sc_ppCalibDacQ;
            public int sc_enCalibDacT;
            public int sc_ppCalibDacT;
            public int[] sc_mask = new int[NbChannels];
            public int sc_ppThHg;
            public int sc_enThHg;
            public int sc_ppThLg;
            public int sc_enThLg;
            public int sc_biasSca;
            public int sc_ppPdetHg;
            public int sc_enPdetHg;
            public int sc_ppPdetLg;
            public int sc_enPdetLg;
            public int sc_scaOrPdHg;
            public int sc_scaOrPdLg;
            public int sc_bypassPd;
            public int sc_selTrigExtPd;
            public int sc_ppFshBuffer;
            public int sc_enFsh;
            public int sc_ppFsh;
            public int sc_ppSshLg;
            public int sc_enSshLg;
            public int sc_shapingTimeLg;
            public int sc_ppSshHg;
            public int sc_enSshHg;
            public int sc_shapingTimeHg;
            public int sc_paLgBias;
            public int sc_ppPaLg;
            public int sc_enPaLg;
            public int sc_ppPaHg;
            public int sc_enPaHg;
            public int sc_fshOnLg;
            public int sc_enInputDac;
            public int sc_dacRef;
            public int[] sc_inputDac = new int[NbChannels];
            public int[] sc_cmdInputDac = new int[NbChannels];
            public int[] sc_paHgGain = new int[NbChannels];
            public int[] sc_paLgGain = new int[NbChannels];
            public int[] sc_CtestHg = new int[NbChannels];
            public int[] sc_CtestLg = new int[NbChannels];
            public int[] sc_enPa = new int[NbChannels];
            public int sc_ppTemp;
            public int sc_enTemp;
            public int sc_ppBg;
            public int sc_enBg;
            public int sc_enThresholdDac1;
            public int sc_ppThresholdDac1;
            public int sc_enThresholdDac2;
            public int sc_ppThresholdDac2;
            public int sc_threshold1;
            public int sc_threshold2;
            public int sc_enHgOtaQ;
            public int sc_ppHgOtaQ;
            public int sc_enLgOtaQ;
            public int sc_ppLgOtaQ;
            public int sc_enProbeOtaQ;
            public int sc_ppProbeOtaQ;
            public int sc_testBitOtaQ;
            public int sc_enValEvtReceiver;
            public int sc_ppValEvtReceiver;
            public int sc_enRazChnReceiver;
            public int sc_ppRazChnReceiver;
            public int sc_enDigitalMuxOutput;
            public int sc_enOr32;
            public int sc_enNor32Oc;
            public int sc_triggerPolarity;
            public int sc_enNor32TOc;
            public int sc_enTriggersOutput;


            private static string IntToBin(int value, int len) // To convert a value from integer to binary representation into a string
            {
                return (len > 1 ? IntToBin(value >> 1, len - 1) : null) + "01"[value & 1];
            }

            private static string strRev(string s) // To reverse a string
            {
                char[] charArray = s.ToCharArray();
                Array.Reverse(charArray);
                return new string(charArray);
            }
            public CitirocConfig()
            {

            }


            public void GenerateBitMonitor(bool[] datavector)
            {
                bool[] bitarray = new bool[195];

                for (int i = 0; i < 195; i++)
                {
                    datavector[i] = bitarray[i];
                }

            }

            public void GenerateBitConfig(bool[] datavector)
            {

                string strSC;

                strSC = "";

                for (int i = 0; i < NbChannels; i++)
                    strSC += strRev(IntToBin(sc_calibDacT[i], 4));

                for (int i = 0; i < NbChannels; i++)
                    strSC += strRev(IntToBin(sc_calibDacQ[i], 4));

                strSC += sc_enDiscri.ToString()
                + sc_ppDiscri.ToString()
                + sc_latchDiscri.ToString()
                + sc_enDiscriT.ToString()
                + sc_ppDiscriT.ToString()
                + sc_enCalibDacQ.ToString()
                + sc_ppCalibDacQ.ToString()
                + sc_enCalibDacT.ToString()
                + sc_ppCalibDacT.ToString();

                for (int i = 0; i < NbChannels; i++)
                    strSC += sc_mask[i].ToString();

                strSC += sc_ppThHg.ToString()
                + sc_enThHg.ToString()
                + sc_ppThLg.ToString()
                + sc_enThLg.ToString()
                + sc_biasSca.ToString()
                + sc_ppPdetHg.ToString()
                + sc_enPdetHg.ToString()
                + sc_ppPdetLg.ToString()
                + sc_enPdetLg.ToString()
                + sc_scaOrPdHg.ToString()
                + sc_scaOrPdLg.ToString()
                + sc_bypassPd.ToString()
                + sc_selTrigExtPd.ToString()
                + sc_ppFshBuffer.ToString()
                + sc_enFsh.ToString()
                + sc_ppFsh.ToString()
                + sc_ppSshLg.ToString()
                + sc_enSshLg.ToString()
                + strRev(IntToBin(sc_shapingTimeLg, 3))
                + sc_ppSshHg.ToString()
                + sc_enSshHg.ToString()
                + strRev(IntToBin(sc_shapingTimeHg, 3))
                + sc_paLgBias.ToString()
                + sc_ppPaHg.ToString()
                + sc_enPaHg.ToString()
                + sc_ppPaLg.ToString()
                + sc_enPaLg.ToString()
                + sc_fshOnLg.ToString()
                + sc_enInputDac.ToString()
                + sc_dacRef.ToString();

                for (int i = 0; i < NbChannels; i++)
                    strSC += IntToBin(sc_inputDac[i], 8) + sc_cmdInputDac[i].ToString();

                for (int i = 0; i < NbChannels; i++)
                    strSC += IntToBin(sc_paHgGain[i], 6) + IntToBin(sc_paLgGain[i], 6) + sc_CtestHg[i].ToString() + sc_CtestLg[i].ToString() + sc_enPa[i].ToString();

                strSC += sc_ppTemp.ToString()
                + sc_enTemp.ToString()
                + sc_ppBg.ToString()
                + sc_enBg.ToString()
                + sc_enThresholdDac1.ToString()
                + sc_ppThresholdDac1.ToString()
                + sc_enThresholdDac2.ToString()
                + sc_ppThresholdDac2.ToString()
                + IntToBin(sc_threshold1, 10)
                + IntToBin(sc_threshold2, 10)
                + sc_enHgOtaQ.ToString()
                + sc_ppHgOtaQ.ToString()
                + sc_enLgOtaQ.ToString()
                + sc_ppLgOtaQ.ToString()
                + sc_enProbeOtaQ.ToString()
                + sc_ppProbeOtaQ.ToString()
                + sc_testBitOtaQ.ToString()
                + sc_enValEvtReceiver.ToString()
                + sc_ppValEvtReceiver.ToString()
                + sc_enRazChnReceiver.ToString()
                + sc_ppRazChnReceiver.ToString()
                + sc_enDigitalMuxOutput.ToString()
                + sc_enOr32.ToString()
                + sc_enNor32Oc.ToString()
                + sc_triggerPolarity.ToString()
                + sc_enNor32TOc.ToString()
                + sc_enTriggersOutput.ToString();


                for (int i = 0; i < 1144; i++)
                {
                    datavector[i] = strSC.Substring(i, 1) == "1" ? true : false;
                }

            }


            public void ConvertStringToDatavector(String cfgb, bool[] datavector)
            {
                for (int i = 0; i < cfgb.Length; i++)
                {
                    datavector[i] = cfgb.Substring(i, 1) == "0" ? false : true;
                }

            }

            public string GenerateStringConfig()
            {
                string bitstream = "";
                bool[] bitarray = new bool[1144];
                GenerateBitConfig(bitarray);
                for (int i = 0; i < 1144; i++)
                {
                    bitstream = bitstream + (bitarray[i] ? "1" : "0");
                }
                return bitstream;
            }

            public void GenerateUint32Config(UInt32[] datavector)
            {
                int i, j;
                bool[] bitarray = new bool[1152];
                GenerateBitConfig(bitarray);

                String CFG = GenerateStringConfig();
                ConvertStringToDatavector(CFG, bitarray);
                for (i = 0; i < 36; i++)
                {
                    datavector[i] = 0;
                    for (j = 0; j < 32; j++)
                    {
                        datavector[i] += ((UInt32)(bitarray[(i * 32) + j] == true ? 1 : 0)) << j;
                    }
                }
            }

            public string GenerateStringMonitor()
            {
                string bitstream = "";
                bool[] bitarray = new bool[195];
                GenerateBitMonitor(bitarray);
                for (int i = 0; i < 195; i++)
                {
                    bitstream = bitstream + (bitarray[i] ? "1" : "0");
                }
                return bitstream;
            }

            public void GenerateUint32Monitor(UInt32[] datavector)
            {
                int i, j;
                bool[] bitarray = new bool[32 * 7];
                GenerateBitMonitor(bitarray);
                for (i = 0; i < 7; i++)
                {
                    datavector[i] = 0;
                    for (j = 0; j < 32; j++)
                    {
                        datavector[i] += ((UInt32)(bitarray[(i * 32) + j] == true ? 1 : 0)) << j;
                    }
                }
            }

        }


        public class t_CitirocDATA
        {
            public UInt64 EventTimecode;
            public UInt64 RunEventTimecode;
            public UInt64 EventCounter;
            public UInt16 AsicID;

            public double EventTimecode_ns;
            public double RunEventTimecode_ns;

            public ushort[] chargeHG;
            public ushort[] chargeLG;
            public bool[] hit;
            public t_CitirocDATA()
            {
                chargeLG = new ushort[32];
                chargeHG = new ushort[32];
                hit = new bool[32];
            }

        }

        public DT5550W_CITIROC(ref PHY_LINK _PHY_LINK)
        {
            phy = _PHY_LINK;
            CitirocCfg = new CitirocConfig();
            pCFG = new List<CitirocConfig>();
            for (int i = 0; i < 4; i++)
                pCFG.Add(new CitirocConfig());
        }
        
        public void SetManualAsicInfo(string AsicModel, int AsicCount)
        {
            DLL_ASIC_COUNT = AsicCount;

            if (AsicCount == 1)
            {
                if (phy.NI_USB3_WriteReg_M(0XE, RFA_PTROC_ASIC_DISABLE) != 0)
                    return;
            }
            if (AsicCount == 2)
            {
                if (phy.NI_USB3_WriteReg_M(0xC, RFA_PTROC_ASIC_DISABLE) != 0)
                    return;
            }
            if (AsicCount == 4)
            {
                if (phy.NI_USB3_WriteReg_M(0, RFA_PTROC_ASIC_DISABLE) != 0)
                    return;
            }

        }

        public t_BoardInfo GetBoardInfo()
        {
            t_BoardInfo BI = new t_BoardInfo();
            BI.AsicType = t_BoardInfo.t_AsicType.CITIROC;
            BI.DefaultDetectorLayout = t_BoardInfo.t_DefaultDetectorLayout.MATRIX_8x8;
            BI.totalAsics = DLL_ASIC_COUNT;
            BI.channelsPerAsic = 32;
            BI.DigitalDataPacketSize = 38;
            BI.FPGATimecode_ns = 25;
            BI.Coarse_ns = 25;
            BI.Fine_ns = 0.037;
            BI.totalChannels = BI.channelsPerAsic * BI.totalAsics;
            //BI.SerialNumber = SerialNumber;
            return BI;
        }


        public bool Connect(String DeviceSerialNumber)
        {

            int retcode = phy.NI_USB3_ConnectDevice_M(DeviceSerialNumber);

            phy.NI_USB3_SetIICControllerBaseAddress_M(
                  RFA_IIC_CONTROL,
                  RFA_IIC_STATUS);


            SerialNumber = DeviceSerialNumber;
            if (retcode == 0)
                return true;
            else
                return false;
        }

        public bool Connect(String DeviceSerialNumber, bool initialize)
        {

            int retcode = phy.NI_USB3_ConnectDevice_M(DeviceSerialNumber);
            if (initialize == true)
            {

            }
            if (retcode == 0)
                return true;
            else
                return false;
        }


        public int SetHV(bool Enable, float voltage, float compliance)
        {
            voltage = voltage > compliance ? compliance : voltage;
            return phy.NI_USB3_SetHV_M(Enable, voltage);




            return 0;
        }

        public int SetHVTempFB(bool Enable, float voltage, float compliance, float tempCof, float temp)
        {
            float newVolt;

            newVolt = voltage + (temp - 25) * (tempCof / 1000);
            newVolt = newVolt > compliance ? compliance : newVolt;
            return phy.NI_USB3_SetHV_M(Enable, newVolt);
        }


        public int GetHV(ref bool Enable, ref float voltage, ref float current)
        {
            //  return 0;
            return phy.NI_USB3_GetHV_M(ref Enable, ref voltage, ref current);
        }

        public int GetSensTemperature(int sensId, ref float temperature)
        {
            //return 0;
            return phy.NI_USB3_GetDT5550WTempSens_M(sensId, ref temperature);
        }


        public bool GetDGCardModel(
            ref String model,
            ref int asic_count,
            ref int SN)
        {
            StringBuilder nModel = new StringBuilder();

            int res = phy.NI_USB3_GetDT5550_DGBoardInfo_M(nModel, ref asic_count, ref SN);
            //int res = 0;
            if (res == 0)
            {
                model = nModel.ToString();
                return true;
            }
            else
                return false;

        }

        public bool GetCountRate(ref UInt32[] cps)
        {
            //  UInt32[] data = new UInt32[1024];
            uint read_word = 0;
            uint valid_data = 0;
            phy.NI_USB3_ReadData_M(cps, (UInt32)128, 0x80080000, PHY_LINK.USB_BUS_MODE.REG_ACCESS, 500, ref read_word, ref valid_data);

            return true;
        }


        public void SelectTriggerMode(bool charge_trigger)
        {
            if (charge_trigger)
                phy.NI_USB3_WriteReg_M(1, DAQ_TRIGGER_MODE);
            else
                phy.NI_USB3_WriteReg_M(0, DAQ_TRIGGER_MODE);
        }


        public void EnableTriggerFrame(bool enable)
        {
            if (enable)
                phy.NI_USB3_WriteReg_M(0xF, DAQ_TRIGGER_FRAME);
            else
                phy.NI_USB3_WriteReg_M(0, DAQ_TRIGGER_FRAME);
        }
        public void EnableExternalVeto(bool enable)
        {
            if (enable)
                phy.NI_USB3_WriteReg_M(1, DAQ_EXTERNAL_VETO);
            else
                phy.NI_USB3_WriteReg_M(0, DAQ_EXTERNAL_VETO);
        }

        public void EnableExternalTrigger(bool enable)
        {
            if (enable)
                phy.NI_USB3_WriteReg_M(1, DAQ_TRIGGER_EXT);
            else
                phy.NI_USB3_WriteReg_M(0, DAQ_TRIGGER_EXT);
        }

        public void GetBuild(ref UInt32 build)
        {
            phy.NI_USB3_ReadReg_M(ref build, 0xFFFFFFFA);
        }

        public void SetASICVeto(bool A1, bool A2, bool A3, bool A4)
        {
            if (DLL_ASIC_COUNT == 1)
            {
                A2 = true;
                A3 = true;
                A4 = true;
            }
            if (DLL_ASIC_COUNT == 2)
            {
                A3 = true;
                A4 = true;
            }

            if (phy.NI_USB3_WriteReg_M((uint)((((A1 ? 0 : 1)) << 3) + (((A2 ? 0 : 1)) << 2) + (((A3 ? 0 : 1)) << 1) + (((A4 ? 0 : 1)) << 0)), RFA_PTROC_ASIC_DISABLE) != 0)
                return;
        }



        private bool CfgCitiroc(bool progA, bool progB, bool progC, bool progD, UInt32[] cfg)
        {

            if (progA)
            {
                for (int i = 0; i < 20; i++)
                    if (phy.NI_USB3_WriteReg_M(cfg[i], RFA_PTROC_BA_A + (uint)i) != 0)
                        return false;

                if (phy.NI_USB3_WriteReg_M(1, RFA_PTROC_BA_A + RFA_PTROC_PROG) != 0)
                    return false;
                if (phy.NI_USB3_WriteReg_M(0, RFA_PTROC_BA_A + RFA_PTROC_PROG) != 0)
                    return false;
            }

            if (progB)
            {
                for (int i = 0; i < 20; i++)
                    if (phy.NI_USB3_WriteReg_M(cfg[i], RFA_PTROC_BA_B + (uint)i) != 0)
                        return false;

                if (phy.NI_USB3_WriteReg_M(1, RFA_PTROC_BA_B + RFA_PTROC_PROG) != 0)
                    return false;
                if (phy.NI_USB3_WriteReg_M(0, RFA_PTROC_BA_B + RFA_PTROC_PROG) != 0)
                    return false;
            }

            if (progC)
            {
                for (int i = 0; i < 20; i++)
                    if (phy.NI_USB3_WriteReg_M(cfg[i], RFA_PTROC_BA_C + (uint)i) != 0)
                        return false;

                if (phy.NI_USB3_WriteReg_M(1, RFA_PTROC_BA_C + RFA_PTROC_PROG) != 0)
                    return false;
                if (phy.NI_USB3_WriteReg_M(0, RFA_PTROC_BA_C + RFA_PTROC_PROG) != 0)
                    return false;
            }

            if (progD)
            {
                for (int i = 0; i < 20; i++)
                    if (phy.NI_USB3_WriteReg_M(cfg[i], RFA_PTROC_BA_D + (uint)i) != 0)
                        return false;

                if (phy.NI_USB3_WriteReg_M(1, RFA_PTROC_BA_D + RFA_PTROC_PROG) != 0)
                    return false;
                if (phy.NI_USB3_WriteReg_M(0, RFA_PTROC_BA_D + RFA_PTROC_PROG) != 0)
                    return false;
            }
            return true;
        }




        public bool ConfigureMonitorCitiroc(bool progA, bool progB, bool progC, bool progD, UInt32[] cfg)
        {
            if (progA)
            {
                for (int i = 0; i < 7; i++)
                    if (phy.NI_USB3_WriteReg_M(cfg[i], RFA_PTROC_BA_A + (uint)i) != 0)
                        return false;

                if (phy.NI_USB3_WriteReg_M(1, RFA_PTROC_BA_A + RFA_PTROC_PROGMONITOR) != 0)
                    return false;
                if (phy.NI_USB3_WriteReg_M(0, RFA_PTROC_BA_A + RFA_PTROC_PROGMONITOR) != 0)
                    return false;
            }

            if (progB)
            {
                for (int i = 0; i < 7; i++)
                    if (phy.NI_USB3_WriteReg_M(cfg[i], RFA_PTROC_BA_B + (uint)i) != 0)
                        return false;

                if (phy.NI_USB3_WriteReg_M(1, RFA_PTROC_BA_B + RFA_PTROC_PROGMONITOR) != 0)
                    return false;
                if (phy.NI_USB3_WriteReg_M(0, RFA_PTROC_BA_B + RFA_PTROC_PROGMONITOR) != 0)
                    return false;
            }

            if (progC)
            {
                for (int i = 0; i < 7; i++)
                    if (phy.NI_USB3_WriteReg_M(cfg[i], RFA_PTROC_BA_C + (uint)i) != 0)
                        return false;

                if (phy.NI_USB3_WriteReg_M(1, RFA_PTROC_BA_C + RFA_PTROC_PROGMONITOR) != 0)
                    return false;
                if (phy.NI_USB3_WriteReg_M(0, RFA_PTROC_BA_C + RFA_PTROC_PROGMONITOR) != 0)
                    return false;
            }

            if (progD)
            {
                for (int i = 0; i < 7; i++)
                    if (phy.NI_USB3_WriteReg_M(cfg[i], RFA_PTROC_BA_D + (uint)i) != 0)
                        return false;

                if (phy.NI_USB3_WriteReg_M(1, RFA_PTROC_BA_D + RFA_PTROC_PROGMONITOR) != 0)
                    return false;
                if (phy.NI_USB3_WriteReg_M(0, RFA_PTROC_BA_D + RFA_PTROC_PROGMONITOR) != 0)
                    return false;
            }
            return true;
        }


     

        public void FlushFIFO()
        {
            phy.NI_USB3_WriteReg_M(0, DAQ_FLUSH_FIFO);
            System.Threading.Thread.Sleep(1);
            phy.NI_USB3_WriteReg_M(1, DAQ_FLUSH_FIFO);
            System.Threading.Thread.Sleep(1);
            phy.NI_USB3_WriteReg_M(0, DAQ_FLUSH_FIFO);
        }


        public void VetoSw(bool veto)
        {
            phy.NI_USB3_WriteReg_M((UInt32)(veto == true ? 1 : 0), DAQ_VETO_sw);
        }

        public void ExtVetoEnable(bool enable)
        {
            phy.NI_USB3_WriteReg_M((UInt32)(enable == true ? 1 : 0), DAQ_VETO);
        }


        public bool SetDigitalOutputSignalLength(double ns)
        {
            int clk_s = (int)(ns / 6.25);
            int retval = 0;
            retval = phy.NI_USB3_WriteReg_M((UInt32)clk_s, DIGOUT_LEN);
            if (retval != 0)
                return false;
            else
                return true;
        }

        public void ConfigureSignalGenerator(bool enableA, bool enableB, bool enableC, bool enableD, int frequency)
        {
            int enable = (enableA ? 1 : 0) + ((enableB ? 1 : 0) << 1) + ((enableC ? 1 : 0) << 2) + ((enableD ? 1 : 0) << 3);
            int period = 160000000 / frequency;
            phy.NI_USB3_WriteReg_M((UInt32)enable, SIGGEN_ENABLE);
            phy.NI_USB3_WriteReg_M((UInt32)period, SIGGEN_PERIOD);
        }



        public void ConfigureT0(T0Mode mode, int T0sw_freq)
        {
            int source = 0;
            int swmode = 0;
            switch (mode)
            {
                case T0Mode.SOFTWARE_STARTRUN:
                    source = 0;
                    break;
                case T0Mode.SOFTWARE_PERIODIC:
                    source = 0;
                    swmode = 1;
                    break;
                case T0Mode.EXTERNAL:
                    source = 1;
                    swmode = 0;
                    break;

            }

            int period = 160000000 / T0sw_freq;
            phy.NI_USB3_WriteReg_M((UInt32)source, T0_SOURCE);
            phy.NI_USB3_WriteReg_M((UInt32)swmode, T0_SWMODE);
            phy.NI_USB3_WriteReg_M((UInt32)period, T0_FREQ);
            phy.NI_USB3_WriteReg_M((UInt32)0, T0_SW);
        }


        public void PulseT0()
        {
            phy.NI_USB3_WriteReg_M((UInt32)0, T0_SW);
            phy.NI_USB3_WriteReg_M((UInt32)1, T0_SW);
            phy.NI_USB3_WriteReg_M((UInt32)0, T0_SW);
        }



        public bool ConfigCitiroc(bool progA, bool progB, bool progC, bool progD, UInt32[] cfg)
        {
  
            bool b = CfgCitiroc(progA, progB, progC, progD, cfg);
         
            return b;
        }

        public int DecodeCitirocRowEvents(ref UInt32[] bufferA, UInt32 valid_wordA, ref Queue<t_CitirocDATA> pC, int ThresholdSoftware)
        {
            
            return 0;
        }
        public int GetRawBuffer(UInt32[] data, UInt32 event_count, UInt32 timeout, UInt32 PacketSize, ref UInt32 valid_word)
        {
            int retcode = 0;
            UInt32 transfer_length = PacketSize * event_count;
            UInt32 read_word = 0;
            UInt32 valid_data = 0;
            UInt32 address = 0;
            address = 0x80000000;
            retcode = phy.NI_USB3_ReadData_M(data, (UInt32)transfer_length, address, PHY_LINK.USB_BUS_MODE.STREAMING, (UInt32)timeout, ref read_word, ref valid_data);

            valid_word = valid_data;
            return retcode;
        }


        public bool GetDefaultChannelMapping(ref tChannelMapping[] chmap)
        {
            int i, j, q;
            t_BoardInfo BI = GetBoardInfo();

            //Dim xx = {1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 2, 3, 3, 2, 2, 3, 3, 2, 2, 3, 3, 2, 2, 3, 3, 2}
            //Dim yy = {3, 3, 2, 2, 1, 1, 0, 0, 7, 7, 6, 6, 5, 5, 4, 4, 4, 4, 5, 5, 6, 6, 7, 7, 0, 0, 1, 1, 2, 2, 3, 3}

            q = 0;
            chmap = new tChannelMapping[BI.totalAsics * BI.channelsPerAsic];
            if (BI.AsicType == t_BoardInfo.t_AsicType.PETIROC)
            {
                int[] xx = { 2, 3, 3, 2, 2, 3, 3, 2, 2, 3, 3, 2, 2, 3, 3, 2, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1 };
                int[] yy = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 7, 7, 6, 6, 5, 5, 4, 4, 3, 3, 2, 2, 1, 1, 0, 0 };

                for (i = 0; i < BI.totalAsics; i++)
                {
                    for (j = 0; j < BI.channelsPerAsic; j++)
                    {
                        chmap[q].ASICID = i;
                        chmap[q].X = (4 * i) + xx[j];
                        chmap[q].Y = yy[j];
                        q++;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
