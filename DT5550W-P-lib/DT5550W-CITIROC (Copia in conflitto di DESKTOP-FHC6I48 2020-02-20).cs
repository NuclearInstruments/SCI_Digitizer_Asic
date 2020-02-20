using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DT5550W_P_lib
{
    public class DT5550W_CITIROC
    {
        const UInt32 SCI_REG_ALL_FIFO_RESET = 0xFFFFF908;
        const UInt32 SCI_REG_T0_SOFT_FREQ = 0x00000000;
        const UInt32 SCI_REG_T0_SEL = 0x00000001;
        const UInt32 SCI_REG_TRIG_A_SEL = 0x00000003;
        const UInt32 SCI_REG_VET_A_EN = 0x00000004;
        const UInt32 SCI_REG_VET_B_EN = 0x00000005;
        const UInt32 SCI_REG_VET_C_EN = 0x00000006;
        const UInt32 SCI_REG_VET_D_EN = 0x00000007;
        const UInt32 SCI_REG_SW_VET_A = 0x00000008;
        const UInt32 SCI_REG_SW_VET_B = 0x00000009;
        const UInt32 SCI_REG_SW_VET_C = 0x0000000A;
        const UInt32 SCI_REG_SW_VET_D = 0x0000000B;
        const UInt32 SCI_REG_TRIG_GBL_SEL = 0x0000000C;
        const UInt32 SCI_REG_EXT_DELAY = 0x0000000D;
        const UInt32 SCI_REG_SW_TRIG_FREQ = 0x0000000E;
        const UInt32 SCI_REG_HOLD_WIN = 0x0000000F;
        const UInt32 SCI_REG_A_RATE = 0x00020007;
        const UInt32 SCI_REG_B_RATE = 0x00020008;
        const UInt32 SCI_REG_C_RATE = 0x00020009;
        const UInt32 SCI_REG_D_RATE = 0x0002000A;
        const UInt32 SCI_REG_T0_COUNT = 0x0002000B;
        const UInt32 SCI_REG_A_TRG = 0x0002000C;
        const UInt32 SCI_REG_B_TRG = 0x0002000D;
        const UInt32 SCI_REG_C_TRG = 0x0002000E;
        const UInt32 SCI_REG_D_TRG = 0x0002000F;
        const UInt32 SCI_REG_RUNSTART = 0x00020010;
        const UInt32 SCI_REG_RUN_TIME_LSB = 0x00020011;
        const UInt32 SCI_REG_RUN_TIME_MSB = 0x00020012;
        const UInt32 SCI_REG_DEAD_TIME_LSB = 0x00020013;
        const UInt32 SCI_REG_DEAD_TIME_MSB = 0x00020014;
        const UInt32 SCI_REG_A_LOST = 0x00020015;
        const UInt32 SCI_REG_B_LOST = 0x00020016;
        const UInt32 SCI_REG_C_LOST = 0x00020017;
        const UInt32 SCI_REG_D_LOST = 0x00020018;
        const UInt32 SCI_REG_i2c_master_0_CTRL = 0x1;
        const UInt32 SCI_REG_i2c_master_0_MON = 0x2;

        const UInt32 SCI_REG_CitirocCfg0_REG_CFG0 = 0x10007E;
        const UInt32 SCI_REG_CitirocCfg0_START_REG_CFG = 0x1000A2;

const UInt32 SCI_REG_Oscilloscope_0_FIFOADDRESS = 0x90000;
        const UInt32 SCI_REG_Oscilloscope_0_READ_STATUS = 0xA0000;
        const UInt32 SCI_REG_Oscilloscope_0_READ_POSITION = 0xA0001;
        const UInt32 SCI_REG_Oscilloscope_0_CONFIG_TRIGGER_MODE = 0xA0002;
        const UInt32 SCI_REG_Oscilloscope_0_CONFIG_PRETRIGGER = 0xA0003;
        const UInt32 SCI_REG_Oscilloscope_0_CONFIG_TRIGGER_LEVEL = 0xA0004;
        const UInt32 SCI_REG_Oscilloscope_0_CONFIG_ARM = 0xA0005;
        const UInt32 SCI_REG_Oscilloscope_0_CONFIG_DECIMATOR = 0xA0006;

const UInt32 SCI_REG_Oscilloscope_1_FIFOADDRESS = 0xB0000;
        const UInt32 SCI_REG_Oscilloscope_1_READ_STATUS = 0xC0000;
        const UInt32 SCI_REG_Oscilloscope_1_READ_POSITION = 0xC0001;
        const UInt32 SCI_REG_Oscilloscope_1_CONFIG_TRIGGER_MODE = 0xC0002;
        const UInt32 SCI_REG_Oscilloscope_1_CONFIG_PRETRIGGER = 0xC0003;
        const UInt32 SCI_REG_Oscilloscope_1_CONFIG_TRIGGER_LEVEL = 0xC0004;
        const UInt32 SCI_REG_Oscilloscope_1_CONFIG_ARM = 0xC0005;
        const UInt32 SCI_REG_Oscilloscope_1_CONFIG_DECIMATOR = 0xC0006;

const UInt32 SCI_REG_Oscilloscope_2_FIFOADDRESS = 0xF0000;
        const UInt32 SCI_REG_Oscilloscope_2_READ_STATUS = 0x100000;
        const UInt32 SCI_REG_Oscilloscope_2_READ_POSITION = 0x100001;
        const UInt32 SCI_REG_Oscilloscope_2_CONFIG_TRIGGER_MODE = 0x100002;
        const UInt32 SCI_REG_Oscilloscope_2_CONFIG_PRETRIGGER = 0x100003;
        const UInt32 SCI_REG_Oscilloscope_2_CONFIG_TRIGGER_LEVEL = 0x100004;
        const UInt32 SCI_REG_Oscilloscope_2_CONFIG_ARM = 0x100005;
        const UInt32 SCI_REG_Oscilloscope_2_CONFIG_DECIMATOR = 0x100006;

const UInt32 SCI_REG_Oscilloscope_3_FIFOADDRESS = 0x10000;
        const UInt32 SCI_REG_Oscilloscope_3_READ_STATUS = 0x20000;
        const UInt32 SCI_REG_Oscilloscope_3_READ_POSITION = 0x20001;
        const UInt32 SCI_REG_Oscilloscope_3_CONFIG_TRIGGER_MODE = 0x20002;
        const UInt32 SCI_REG_Oscilloscope_3_CONFIG_PRETRIGGER = 0x20003;
        const UInt32 SCI_REG_Oscilloscope_3_CONFIG_TRIGGER_LEVEL = 0x20004;
        const UInt32 SCI_REG_Oscilloscope_3_CONFIG_ARM = 0x20005;
        const UInt32 SCI_REG_Oscilloscope_3_CONFIG_DECIMATOR = 0x20006;

const UInt32 SCI_REG_CitirocFrame0_FIFOADDRESS = 0x2001A;
        const UInt32 SCI_REG_CitirocFrame0_CONTROL = 0x2001B;
        const UInt32 SCI_REG_CitirocFrame0_STATUS = 0x2001C;

const UInt32 SCI_REG_RateMeter_0_FIFOADDRESS = 0x70000;

const UInt32 SCI_REG_RateMeter_1_FIFOADDRESS = 0xD0000;

const UInt32 SCI_REG_RateMeter_2_FIFOADDRESS = 0x30000;

const UInt32 SCI_REG_RateMeter_3_FIFOADDRESS = 0x50000;

const UInt32 SCI_REG_CitirocCfg1_REG_CFG0 = 0x100009;
        const UInt32 SCI_REG_CitirocCfg1_START_REG_CFG = 0x10002D;

        const UInt32 SCI_REG_CitirocCfg2_REG_CFG0 = 0x100030;
        const UInt32 SCI_REG_CitirocCfg2_START_REG_CFG = 0x100054;

        const UInt32 SCI_REG_CitirocCfg3_REG_CFG0 = 0x100057;
        const UInt32 SCI_REG_CitirocCfg3_START_REG_CFG = 0x10007B;



        const UInt32 SCI_REG_FR_LIMIT =0x00000011;
        const UInt32 SCI_REG_FR_IFP2 = 0x00000012;
        const UInt32 SCI_REG_FR_MODE = 0x00000013;
        const UInt32 SCI_REG_FR_IFP =  0x00000010;
        const UInt32 SCI_REG_FR_CP0 =        0x00200000;
        const UInt32 SCI_REG_FR_CP0_STATUS = 0x00200001;
        const UInt32 SCI_REG_FR_CP0_VALIDWORDS = 0x00200002;
        const UInt32 SCI_REG_FR_CP0_CONFIG = 0x00200003;


        int DLL_ASIC_COUNT = 4;
        public CitirocConfig CitirocCfg;

        public List<CitirocConfig> pCFG;

        public string SerialNumber;

        t_AsicModels DLL_CURRENT_ASIC = t_AsicModels.CITIROC;
        
        PHY_LINK phy;

        public class CitirocConfig
        {

            public enum tAnalogProbe
            {
                NONE,
                LG_PRE,
                LG_SHAPER,
                HG_PRE,
                HG_SHAPER,
                FAST_SHAPER
            }

            public enum tDigitalProbe
            {
                NONE,
                HG_PEAK_DET_MODE,
                LG_PEAK_DET_MODE
            }

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

            public int ChannelOutputAnalog;
            public int ChannelOutputDigital;

            public tAnalogProbe  AnalogProble;
            public tDigitalProbe DigitalProble;

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
                

                bool result = false;
                char[] tmpProbeStream = new char[256];

                for (int i = 0; i < 256; i++) tmpProbeStream[i] = '0';

                tmpProbeStream[(int)ChannelOutputAnalog] = (AnalogProble == tAnalogProbe.FAST_SHAPER ) ? '1' : '0';
                tmpProbeStream[(int)ChannelOutputAnalog + 32] = (AnalogProble == tAnalogProbe.LG_SHAPER) ? '1' : '0';
                tmpProbeStream[(int)ChannelOutputDigital + 64] = (DigitalProble == tDigitalProbe.LG_PEAK_DET_MODE) ? '1' : '0';
                tmpProbeStream[(int)ChannelOutputAnalog + 96] = (AnalogProble == tAnalogProbe.HG_SHAPER) ? '1' : '0';
                tmpProbeStream[(int)ChannelOutputDigital + 128] = (DigitalProble == tDigitalProbe.HG_PEAK_DET_MODE) ? '1' : '0';
                tmpProbeStream[(int)ChannelOutputAnalog * 2 + 160] = (AnalogProble == tAnalogProbe.HG_PRE) ? '1' : '0';
                tmpProbeStream[(int)ChannelOutputAnalog * 2 + 160 + 1] = (AnalogProble == tAnalogProbe.LG_PRE) ? '1' : '0';

                string probeStream = new string(tmpProbeStream);

                int intLenProbeStream = probeStream.Length;
                byte[] bytProbe = new byte[intLenProbeStream / 8];

                probeStream = strRev(probeStream);

                for (int i = 0; i < (intLenProbeStream / 8); i++)
                {
                    string strProbeCmdTmp = probeStream.Substring(i * 8, 8);
                    strProbeCmdTmp = strRev(strProbeCmdTmp);
                    UInt32 intCmdTmp = Convert.ToUInt32(strProbeCmdTmp, 2);
                    bytProbe[i] = Convert.ToByte(intCmdTmp);
                }

                for (int i = 0; i < 256; i++)
                {
                    datavector[i] = tmpProbeStream[i] == '1' ? true : false;
                }

            }


            public string GenerateStringMonitor()
            {
                string bitstream = "";
                bool[] bitarray = new bool[256];
                GenerateBitMonitor(bitarray);
                for (int i = 0; i < 256; i++)
                {
                    bitstream = bitstream + (bitarray[i] ? "1" : "0");
                }
                return bitstream;
            }

            public void GenerateUint32Monitor(UInt32[] datavector)
            {
                int i, j;
                bool[] bitarray = new bool[32 * 8];
                GenerateBitMonitor(bitarray);
                for (i = 0; i < 8; i++)
                {
                    datavector[i] = 0;
                    for (j = 0; j < 32; j++)
                    {
                        datavector[i] += ((UInt32)(bitarray[(i * 32) + j] == true ? 1 : 0)) << j;
                    }
                }
            }

            public void GenerateBitConfig(ref bool[] datavector)
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
                GenerateBitConfig(ref bitarray);
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
                GenerateBitConfig(ref bitarray);

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
                  SCI_REG_i2c_master_0_CTRL,
                  SCI_REG_i2c_master_0_MON);


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
            UInt32[] data = new UInt32[1024];
            int i;
            uint read_word = 0;
            uint valid_data = 0;
            phy.NI_USB3_ReadData_M(data, (UInt32)32, SCI_REG_RateMeter_0_FIFOADDRESS, PHY_LINK.USB_BUS_MODE.REG_ACCESS, 500, ref read_word, ref valid_data);
            for (i=1;i<32;i++)
            {
                cps[i] = data[i-1];
            }
            cps[0] = data[31];

            phy.NI_USB3_ReadData_M(data, (UInt32)32, SCI_REG_RateMeter_1_FIFOADDRESS, PHY_LINK.USB_BUS_MODE.REG_ACCESS, 500, ref read_word, ref valid_data);
            for (i = 1; i < 32; i++)
            {
                cps[i + 32] = data[i - 1];
            }
            cps[32] = data[31];

            phy.NI_USB3_ReadData_M(data, (UInt32)32, SCI_REG_RateMeter_2_FIFOADDRESS, PHY_LINK.USB_BUS_MODE.REG_ACCESS, 500, ref read_word, ref valid_data);
            for (i = 1; i < 32; i++)
            {
                cps[i + 64] = data[i - 1];
            }
            cps[64] = data[31];

            phy.NI_USB3_ReadData_M(data, (UInt32)32, SCI_REG_RateMeter_3_FIFOADDRESS, PHY_LINK.USB_BUS_MODE.REG_ACCESS, 500, ref read_word, ref valid_data);
            for (i = 1; i < 32; i++)
            {
                cps[i + 96] = data[i - 1];
            }
            cps[96] = data[31];

            return true;
        }


        public void SelectTriggerMode(TriggerMode TGMode)
        {
            uint trig_mode=0;
            uint gbltrig_mode=0;
            switch (TGMode)
            {
                case TriggerMode.TIME_TRIG:
                    trig_mode = 0;
                    break;

                case TriggerMode.CHARGE_TRIG:
                    trig_mode = 1;
                    break;

                case TriggerMode.EXT_TRIG:
                    trig_mode = 2;
                    break;

                case TriggerMode.GBL_TRIG_TIME:
                    gbltrig_mode = 0;
                    trig_mode = 3;
                    break;

                case TriggerMode.GBL_TRIG_CHARGE:
                    gbltrig_mode = 0;
                    trig_mode = 3;
                    break;

                case TriggerMode.SELF_TRIG:
                    trig_mode = 4;
                    break;

              
            }
          //  phy.NI_USB3_WriteReg_M(0, SCI_REG_TRIG_A_SEL);
            phy.NI_USB3_WriteReg_M(trig_mode, SCI_REG_TRIG_A_SEL);
            phy.NI_USB3_WriteReg_M(gbltrig_mode, SCI_REG_TRIG_GBL_SEL);



        }


       
        public void EnableExternalVeto(bool enable)
        {
            if (enable)
            {
                phy.NI_USB3_WriteReg_M(1, SCI_REG_VET_A_EN);
                phy.NI_USB3_WriteReg_M(1, SCI_REG_VET_B_EN);
                phy.NI_USB3_WriteReg_M(1, SCI_REG_VET_C_EN);
                phy.NI_USB3_WriteReg_M(1, SCI_REG_VET_D_EN);
            }
            else
            {
                phy.NI_USB3_WriteReg_M(0, SCI_REG_VET_A_EN);
                phy.NI_USB3_WriteReg_M(0, SCI_REG_VET_B_EN);
                phy.NI_USB3_WriteReg_M(0, SCI_REG_VET_C_EN);
                phy.NI_USB3_WriteReg_M(0, SCI_REG_VET_D_EN);
            }
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

            phy.NI_USB3_WriteReg_M((uint)(A1 ? 1 : 0), SCI_REG_SW_VET_A);
            phy.NI_USB3_WriteReg_M((uint)(A2 ? 1 : 0), SCI_REG_SW_VET_B);
            phy.NI_USB3_WriteReg_M((uint)(A3 ? 1 : 0), SCI_REG_SW_VET_C);
            phy.NI_USB3_WriteReg_M((uint)(A4 ? 1 : 0), SCI_REG_SW_VET_D);


            //if (phy.NI_USB3_WriteReg_M((uint)((((A1 ? 0 : 1)) << 3) + (((A2 ? 0 : 1)) << 2) + (((A3 ? 0 : 1)) << 1) + (((A4 ? 0 : 1)) << 0)), RFA_PTROC_ASIC_DISABLE) != 0)
            //   return;
        }



        private bool CfgCitiroc(bool progA, bool progB, bool progC, bool progD, UInt32[] cfg)
        {

            if (progA)
            {
                for (int i = 0; i < 36; i++)
                    if (phy.NI_USB3_WriteReg_M(cfg[i], SCI_REG_CitirocCfg0_REG_CFG0 + (uint)i) != 0)
                        return false;

                if (phy.NI_USB3_WriteReg_M(1, SCI_REG_CitirocCfg0_START_REG_CFG) != 0)
                    return false;
                if (phy.NI_USB3_WriteReg_M(0, SCI_REG_CitirocCfg0_START_REG_CFG) != 0)
                    return false;
            }

            if (progB)
            {
                for (int i = 0; i < 36; i++)
                    if (phy.NI_USB3_WriteReg_M(cfg[i], SCI_REG_CitirocCfg1_REG_CFG0 + (uint)i) != 0)
                        return false;

                if (phy.NI_USB3_WriteReg_M(1, SCI_REG_CitirocCfg1_START_REG_CFG) != 0)
                    return false;
                if (phy.NI_USB3_WriteReg_M(0, SCI_REG_CitirocCfg1_START_REG_CFG) != 0)
                    return false;
            }

            if (progC)
            {
                for (int i = 0; i < 36; i++)
                    if (phy.NI_USB3_WriteReg_M(cfg[i], SCI_REG_CitirocCfg2_REG_CFG0 + (uint)i) != 0)
                        return false;

                if (phy.NI_USB3_WriteReg_M(1, SCI_REG_CitirocCfg2_START_REG_CFG) != 0)
                    return false;
                if (phy.NI_USB3_WriteReg_M(0, SCI_REG_CitirocCfg2_START_REG_CFG) != 0)
                    return false;
            }

            if (progD)
            {
                for (int i = 0; i < 36; i++)
                    if (phy.NI_USB3_WriteReg_M(cfg[i], SCI_REG_CitirocCfg3_REG_CFG0 + (uint)i) != 0)
                        return false;

                if (phy.NI_USB3_WriteReg_M(1, SCI_REG_CitirocCfg3_START_REG_CFG) != 0)
                    return false;
                if (phy.NI_USB3_WriteReg_M(0, SCI_REG_CitirocCfg3_START_REG_CFG) != 0)
                    return false;
            }
            return true;
        }




        public bool ConfigureMonitorCitiroc(bool progA, bool progB, bool progC, bool progD, UInt32[] cfg)
        {
            if (progA)
            {
                for (int i = 0; i < 8; i++)
                    if (phy.NI_USB3_WriteReg_M(cfg[i], SCI_REG_CitirocCfg0_REG_CFG0 + (uint)i) != 0)
                        return false;

                if (phy.NI_USB3_WriteReg_M(2, SCI_REG_CitirocCfg0_START_REG_CFG) != 0)
                    return false;
                if (phy.NI_USB3_WriteReg_M(0, SCI_REG_CitirocCfg0_START_REG_CFG) != 0)
                    return false;
            }

            if (progB)
            {
                for (int i = 0; i < 8; i++)
                    if (phy.NI_USB3_WriteReg_M(cfg[i], SCI_REG_CitirocCfg1_REG_CFG0 + (uint)i) != 0)
                        return false;

                if (phy.NI_USB3_WriteReg_M(2, SCI_REG_CitirocCfg1_START_REG_CFG) != 0)
                    return false;
                if (phy.NI_USB3_WriteReg_M(0, SCI_REG_CitirocCfg1_START_REG_CFG) != 0)
                    return false;
            }

            if (progC)
            {
                for (int i = 0; i < 8; i++)
                    if (phy.NI_USB3_WriteReg_M(cfg[i], SCI_REG_CitirocCfg2_REG_CFG0 + (uint)i) != 0)
                        return false;

                if (phy.NI_USB3_WriteReg_M(2, SCI_REG_CitirocCfg2_START_REG_CFG) != 0)
                    return false;
                if (phy.NI_USB3_WriteReg_M(0, SCI_REG_CitirocCfg2_START_REG_CFG) != 0)
                    return false;
            }

            if (progD)
            {
                for (int i = 0; i < 8; i++)
                    if (phy.NI_USB3_WriteReg_M(cfg[i], SCI_REG_CitirocCfg3_REG_CFG0 + (uint)i) != 0)
                        return false;

                if (phy.NI_USB3_WriteReg_M(2, SCI_REG_CitirocCfg3_START_REG_CFG) != 0)
                    return false;
                if (phy.NI_USB3_WriteReg_M(0, SCI_REG_CitirocCfg3_START_REG_CFG) != 0)
                    return false;
            }
            return true;
        }


     

        public void FlushFIFO()
        {
            phy.NI_USB3_WriteReg_M(0, SCI_REG_RUNSTART);
            System.Threading.Thread.Sleep(1);
            phy.NI_USB3_WriteReg_M(1, SCI_REG_RUNSTART);
            System.Threading.Thread.Sleep(1);
            phy.NI_USB3_WriteReg_M(0, SCI_REG_RUNSTART);
        }



        public void ExtVetoEnable(bool enable)
        {
            phy.NI_USB3_WriteReg_M((UInt32)(enable == true ? 1 : 0), SCI_REG_VET_A_EN);
            phy.NI_USB3_WriteReg_M((UInt32)(enable == true ? 1 : 0), SCI_REG_VET_B_EN);
            phy.NI_USB3_WriteReg_M((UInt32)(enable == true ? 1 : 0), SCI_REG_VET_C_EN);
            phy.NI_USB3_WriteReg_M((UInt32)(enable == true ? 1 : 0), SCI_REG_VET_D_EN);
        }


        public bool SetDigitalOutputSignalLength(double ns)
        {
            int clk_s = (int)(ns / 6.25);
            int retval = 0;
            /*retval = phy.NI_USB3_WriteReg_M((UInt32)clk_s, DIGOUT_LEN);
            if (retval != 0)
                return false;
            else
                return true;*/

            return true;
        }

        public void ConfigureSignalGenerator(bool enableA, bool enableB, bool enableC, bool enableD, int frequency)
        {
            
            int period = 160000000 / frequency;
            
            phy.NI_USB3_WriteReg_M((UInt32)period, SCI_REG_SW_TRIG_FREQ);
        }


        public void SetHoldWindowsWidth(double hwidth)
        {
            phy.NI_USB3_WriteReg_M((UInt32)hwidth, SCI_REG_HOLD_WIN);
        }


        public void ConfigureT0(T0Mode mode, int T0sw_freq)
        {
            int source = 0;
            int swmode = 0;
            switch (mode)
            {
                case T0Mode.SOFTWARE_PERIODIC:
                    source = 1;
                    break;
                case T0Mode.EXTERNAL:
                    source = 0;
                    break;

            }

            int period = 160000000 / T0sw_freq;
            phy.NI_USB3_WriteReg_M((UInt32)source, SCI_REG_T0_SEL);
            //phy.NI_USB3_WriteReg_M((UInt32)swmode, T0_SWMODE);
            phy.NI_USB3_WriteReg_M((UInt32)period, SCI_REG_T0_SOFT_FREQ);
            //phy.NI_USB3_WriteReg_M((UInt32)0, T0_SW);
        }


        public void PulseT0()
        {
            /*phy.NI_USB3_WriteReg_M((UInt32)0, T0_SW);
            phy.NI_USB3_WriteReg_M((UInt32)1, T0_SW);
            phy.NI_USB3_WriteReg_M((UInt32)0, T0_SW);*/
        }



        public bool ConfigCitiroc(bool progA, bool progB, bool progC, bool progD, UInt32[] cfg)
        {
  
            bool b = CfgCitiroc(progA, progB, progC, progD, cfg);
         
            return b;
        }

        const int PacketSize = 38;
        Queue<UInt32> InternalBuffer = new Queue<UInt32>();
        int internalBufferSize = 1000000;


        public bool PushRawDataInBuffer(ref UInt32[] bufferA, UInt32 valid_wordA)
        {
            if (InternalBuffer.Count + valid_wordA > internalBufferSize) return false;

            for (int i = 0; i < valid_wordA; i++)
            {
                InternalBuffer.Enqueue((UInt32)bufferA[i]);
            }

            return true;
        }



        public int DecodeCitirocRowEvents(ref Queue<t_DataCITIROC> pC, int ThresholdSoftware)
        {
            int DecodedPackets = 0;
            double Time = 0;
            double minTime = 0;
            UInt32 i, j, t, s;
            UInt32 bword = 0;
            uint[] datarow = new uint[97];

            t_DataCITIROC DataPETIROCA = null;
            t = 0;
            s = 0;
            while (InternalBuffer.Count > PacketSize)
            {
                switch (s)
                {
                    case 0:
                        bword = InternalBuffer.Dequeue();
                        if (((bword >> 4) & 0xc000000) == 0x8000000)
                        {
                            s = 1;
                            DataPETIROCA = new t_DataCITIROC();
                            DataPETIROCA.AsicID = (UInt16)(bword & 0xF);
                            DataPETIROCA.EventTimecode = ((UInt64)InternalBuffer.Dequeue());
                            DataPETIROCA.RunEventTimecode = (((UInt64)InternalBuffer.Dequeue())) + (((UInt64)InternalBuffer.Dequeue()) << 32);
                            DataPETIROCA.EventCounter = ((UInt64)InternalBuffer.Dequeue());

                            DataPETIROCA.EventTimecode_ns = DataPETIROCA.EventTimecode * 6.25;
                            DataPETIROCA.RunEventTimecode_ns = DataPETIROCA.RunEventTimecode * 6.25;

                            t = t + 5;
                            minTime = 100000000000000;
                        }
                        else
                            t++;
                        break;

                    case 1:
                        for (i = 0; i < 32; i++)
                        {
                            bword = InternalBuffer.Dequeue();
                            datarow[i * 3 + 0] = (bword >> 0) & 0x3FFF;
                            datarow[i * 3 + 1] = (bword >> 14) & 0x3FFF;
                            datarow[i * 3 + 2] = (bword >> 28) & 0x1;
                            t++;
                        }
                        for (i = 0; i < 32; i++)
                        {
                            DataPETIROCA.hit[31-i] = (bool)((datarow[i * 3 + 2] & 0x1) == 1 ? true : false);
                            int dataHG = (int)datarow[(i * 3) + 0];
                            int dataLG = (int)datarow[(i * 3) + 1];


                            if (dataHG > ThresholdSoftware)//(DataPETIROCA.hit[i])
                                DataPETIROCA.chargeHG[31-i] = (ushort)(dataHG);//(ushort) (data>100 ? (data):0);
                            else
                                DataPETIROCA.chargeHG[31-i] = 0;


                            if (dataHG > ThresholdSoftware)//(DataPETIROCA.hit[i])
                                DataPETIROCA.chargeLG[31-i] = (ushort)dataLG;//(ushort) (data>100 ? (data):0);
                            else
                                DataPETIROCA.chargeLG[31-i] = 0;

                        }
                        s = 2;
                        break;

                    case 2:
                        if ((InternalBuffer.Dequeue() & 0xc0000000) == 0xc0000000)
                        {
                            pC.Enqueue(DataPETIROCA);
                            DecodedPackets++;
                        }
                        t++;
                        s = 0;
                        break;
                }
            }
            return DecodedPackets;
        }

        public int GetRawBuffer(UInt32[] data, UInt32 event_count, UInt32 timeout, UInt32 PacketSize, ref UInt32 valid_word)
        {
            int retcode = 0;
            UInt32 transfer_length = PacketSize * event_count;
            UInt32 read_word = 0;
            UInt32 valid_data = 0;
 
            retcode = phy.NI_USB3_ReadData_M(data, (UInt32)transfer_length, SCI_REG_CitirocFrame0_FIFOADDRESS, PHY_LINK.USB_BUS_MODE.STREAMING, (UInt32)timeout, ref read_word, ref valid_data);

            valid_word = valid_data;
            return retcode;
        }


        public int DecodeCitirocRowEventsPC(ref Queue<t_DataCITIROCPC> pC)
        {
            int DecodedPackets = 0;
            double Time = 0;
            double minTime = 0;
            UInt32 i, j, t, s;
            UInt32 bword = 0;
            uint[] datarow = new uint[97];

            t_DataCITIROCPC DataPC = null;
            t = 0;
            s = 0;
            int pcPacketSize = 1 + 1 + 128 + 2 + 1 + 1;
            while (InternalBuffer.Count > pcPacketSize)
            {
                switch (s)
                {
                    case 0:
                        bword = InternalBuffer.Dequeue();
                        if (bword  == 0xFFFFFFFF)
                        {
                            s = 1;
                            DataPC = new t_DataCITIROCPC();
                            DataPC.PacketID = ((UInt64)InternalBuffer.Dequeue());                            
                            t = t + 2;

                        }
                        else
                            t++;
                        break;

                    case 1:
                        for (i = 0; i < 128; i++)
                        {
                            bword = InternalBuffer.Dequeue();
                            DataPC.counters[i] = bword;
                            t++;
                        }
                        s = 2;
                        break;

                    case 2:


                        DataPC.EventTimecode  = (((UInt64)InternalBuffer.Dequeue())) + (((UInt64)InternalBuffer.Dequeue()) << 32);
                        DataPC.WindowsID = ((UInt64)InternalBuffer.Dequeue());
                        DataPC.StartID = ((UInt64)InternalBuffer.Dequeue());

                        DataPC.EventTimecode_ns = ((double)DataPC.EventTimecode) * 6.25;
                        pC.Enqueue(DataPC);
                        DecodedPackets++;


                        t+=4;
                        s = 0;
                        break;
                }
            }
            return DecodedPackets;
        }

        public int GetRawBufferPC(UInt32[] data, UInt32 event_count, UInt32 timeout, UInt32 PacketSize, ref UInt32 valid_word)
        {
            int retcode = 0;
            UInt32 transfer_length = PacketSize * event_count;
            UInt32 read_word = 0;
            UInt32 valid_data = 0;

            UInt32 available_data = 0;

            phy.NI_USB3_ReadReg_M(ref available_data, SCI_REG_FR_CP0_VALIDWORDS);

            if (available_data>= PacketSize)
            {
                transfer_length = available_data > transfer_length ? transfer_length : available_data;
                transfer_length = ((UInt32)Math.Floor( ((double)transfer_length / (double)PacketSize)))* PacketSize;



                retcode = phy.NI_USB3_ReadData_M(data, (UInt32)transfer_length, SCI_REG_FR_CP0, PHY_LINK.USB_BUS_MODE.STREAMING, (UInt32)timeout, ref read_word, ref valid_data);

                Console.WriteLine(valid_data);

                valid_word = valid_data;
                return retcode;
            }
            else
            {
                valid_word = 0;
                return -1;
            }
            
        }


        public void ConfigurePC(PCMode mode, int PeriodicWidth, int PeriodicStart, int windows_count)
        {
            int period = 160000000 / PeriodicStart;
            phy.NI_USB3_WriteReg_M((UInt32)mode, SCI_REG_FR_MODE);
            phy.NI_USB3_WriteReg_M((UInt32)(PeriodicWidth/6.25), SCI_REG_FR_IFP);
            phy.NI_USB3_WriteReg_M((UInt32)period, SCI_REG_FR_IFP2);
            phy.NI_USB3_WriteReg_M((UInt32)windows_count, SCI_REG_FR_LIMIT);
        }

        public void StartPC()
        {
            phy.NI_USB3_WriteReg_M((UInt32)0, SCI_REG_FR_CP0_CONFIG); //STOP
            phy.NI_USB3_WriteReg_M((UInt32)2, SCI_REG_FR_CP0_CONFIG); //RESET
            phy.NI_USB3_WriteReg_M((UInt32)0, SCI_REG_FR_CP0_CONFIG); //STOP
            phy.NI_USB3_WriteReg_M((UInt32)1, SCI_REG_FR_CP0_CONFIG); //START
        }

        


        public bool GetDefaultChannelMapping(ref tChannelMapping[] chmap)
        {
            int i, j, q;
            t_BoardInfo BI = GetBoardInfo();

            //Dim xx = {1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 2, 3, 3, 2, 2, 3, 3, 2, 2, 3, 3, 2, 2, 3, 3, 2}
            //Dim yy = {3, 3, 2, 2, 1, 1, 0, 0, 7, 7, 6, 6, 5, 5, 4, 4, 4, 4, 5, 5, 6, 6, 7, 7, 0, 0, 1, 1, 2, 2, 3, 3}

            q = 0;
            chmap = new tChannelMapping[BI.totalAsics * BI.channelsPerAsic];
           //if (BI.AsicType == t_BoardInfo.t_AsicType.PETIROC)
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


        public bool GetMonitor(ref CitirocMonitorData PMD)
        {
            uint StatusRegister = 0;
            //NI_USB3_ReadReg_M(ref StatusRegister, 0x8007FFFF);
            //if (StatusRegister!=0)
            // {
            phy.NI_USB3_WriteReg_M(0, SCI_REG_Oscilloscope_0_CONFIG_TRIGGER_MODE);
            phy.NI_USB3_WriteReg_M(0, SCI_REG_Oscilloscope_1_CONFIG_TRIGGER_MODE);
            phy.NI_USB3_WriteReg_M(0, SCI_REG_Oscilloscope_2_CONFIG_TRIGGER_MODE);
            phy.NI_USB3_WriteReg_M(0, SCI_REG_Oscilloscope_3_CONFIG_TRIGGER_MODE);
            int retcode = 0;
            UInt32 samples = 1024;
            UInt32 OscWordSize = 1 ;
            UInt32 transfer_length = OscWordSize * samples * 2;
            UInt32[] data0 = new UInt32[transfer_length * 2];
            UInt32[] data1 = new UInt32[transfer_length * 2];
            UInt32[] data2 = new UInt32[transfer_length * 2];
            UInt32[] data3 = new UInt32[transfer_length * 2];

            UInt32 read_word = 0;
            UInt32 valid_data = 0;
            UInt32 read_pos0 = 0;
            UInt32 read_pos1 = 0;
            UInt32 read_pos2 = 0;
            UInt32 read_pos3 = 0;

            retcode = phy.NI_USB3_ReadData_M(data0, (UInt32)transfer_length, SCI_REG_Oscilloscope_0_FIFOADDRESS, PHY_LINK.USB_BUS_MODE.REG_ACCESS, (UInt32)4000, ref read_word, ref valid_data);
            retcode = phy.NI_USB3_ReadData_M(data1, (UInt32)transfer_length, SCI_REG_Oscilloscope_1_FIFOADDRESS, PHY_LINK.USB_BUS_MODE.REG_ACCESS, (UInt32)4000, ref read_word, ref valid_data);
            retcode = phy.NI_USB3_ReadData_M(data2, (UInt32)transfer_length, SCI_REG_Oscilloscope_2_FIFOADDRESS, PHY_LINK.USB_BUS_MODE.REG_ACCESS, (UInt32)4000, ref read_word, ref valid_data);
            retcode = phy.NI_USB3_ReadData_M(data3, (UInt32)transfer_length, SCI_REG_Oscilloscope_3_FIFOADDRESS, PHY_LINK.USB_BUS_MODE.REG_ACCESS, (UInt32)4000, ref read_word, ref valid_data);

            phy.NI_USB3_ReadReg_M(ref read_pos0, SCI_REG_Oscilloscope_0_READ_POSITION);
            phy.NI_USB3_ReadReg_M(ref read_pos1, SCI_REG_Oscilloscope_1_READ_POSITION);
            phy.NI_USB3_ReadReg_M(ref read_pos2, SCI_REG_Oscilloscope_2_READ_POSITION);
            phy.NI_USB3_ReadReg_M(ref read_pos3, SCI_REG_Oscilloscope_3_READ_POSITION);


            if (read_pos0 > transfer_length * 2)
                return false;
            if (read_pos1 > transfer_length * 2)
                return false;
            if (read_pos2 > transfer_length * 2)
                return false;
            if (read_pos3 > transfer_length * 2)
                return false;

            phy.NI_USB3_WriteReg_M(21, SCI_REG_Oscilloscope_0_CONFIG_DECIMATOR);
            phy.NI_USB3_WriteReg_M(21, SCI_REG_Oscilloscope_1_CONFIG_DECIMATOR);
            phy.NI_USB3_WriteReg_M(21, SCI_REG_Oscilloscope_2_CONFIG_DECIMATOR);
            phy.NI_USB3_WriteReg_M(21, SCI_REG_Oscilloscope_3_CONFIG_DECIMATOR);

            phy.NI_USB3_WriteReg_M(0, SCI_REG_Oscilloscope_0_CONFIG_ARM);
            phy.NI_USB3_WriteReg_M(1, SCI_REG_Oscilloscope_0_CONFIG_ARM);
            phy.NI_USB3_WriteReg_M(0, SCI_REG_Oscilloscope_0_CONFIG_ARM);

            phy.NI_USB3_WriteReg_M(0, SCI_REG_Oscilloscope_1_CONFIG_ARM);
            phy.NI_USB3_WriteReg_M(1, SCI_REG_Oscilloscope_1_CONFIG_ARM);
            phy.NI_USB3_WriteReg_M(0, SCI_REG_Oscilloscope_1_CONFIG_ARM);

            phy.NI_USB3_WriteReg_M(0, SCI_REG_Oscilloscope_2_CONFIG_ARM);
            phy.NI_USB3_WriteReg_M(1, SCI_REG_Oscilloscope_2_CONFIG_ARM);
            phy.NI_USB3_WriteReg_M(0, SCI_REG_Oscilloscope_2_CONFIG_ARM);

            phy.NI_USB3_WriteReg_M(0, SCI_REG_Oscilloscope_3_CONFIG_ARM);
            phy.NI_USB3_WriteReg_M(1, SCI_REG_Oscilloscope_3_CONFIG_ARM);
            phy.NI_USB3_WriteReg_M(0, SCI_REG_Oscilloscope_3_CONFIG_ARM);


            int soffset = 0;
            int q=0;

            for (int i = (int)read_pos0; i < samples-1; i++)
            {
                UInt32 t;
                PMD.A_chargeLG [q] = ((data0[soffset + (i * OscWordSize) + 0] ) & 0xFFFF);
                PMD.A_chargeHG[q] =  ((data0[soffset + (i * OscWordSize) + samples] ) & 0xFFFF);
                PMD.A_CLK [q] = ((data0[soffset + (i * OscWordSize) + 0] >> 16) & 0x1);
                PMD.A_SR[q] = ((data0[soffset + (i * OscWordSize) + 0] >> 17) & 0x1);
                PMD.A_Trig_Ext[q] = ((data0[soffset + (i * OscWordSize) + 0] >> 18) & 0x1);
                PMD.A_Veto_Ext[q] = ((data0[soffset + (i * OscWordSize) + 0] >> 19) & 0x1);
                PMD.A_TrigT[q] = ((data0[soffset + (i * OscWordSize) + samples] >> 16) & 0x1);
                PMD.A_TrigC[q] = ((data0[soffset + (i * OscWordSize) + samples] >> 17) & 0x1);
                PMD.A_Trig[q] = ((data0[soffset + (i * OscWordSize) + samples] >> 18) & 0x1);
                PMD.A_HIT[q] = ((data0[soffset + (i * OscWordSize) + samples] >> 19) & 0x1);
                q++;
            }

            for (int i = 0; i < (int)read_pos0; i++)
            {
                UInt32 t;
                PMD.A_chargeLG[q] = ((data0[soffset + (i * OscWordSize) + 0]) & 0xFFFF);
                PMD.A_chargeHG[q] = ((data0[soffset + (i * OscWordSize) + samples]) & 0xFFFF);
                PMD.A_CLK[q] = ((data0[soffset + (i * OscWordSize) + 0] >> 16) & 0x1);
                PMD.A_SR[q] = ((data0[soffset + (i * OscWordSize) + 0] >> 17) & 0x1);
                PMD.A_Trig_Ext[q] = ((data0[soffset + (i * OscWordSize) + 0] >> 18) & 0x1);
                PMD.A_Veto_Ext[q] = ((data0[soffset + (i * OscWordSize) + 0] >> 19) & 0x1);
                PMD.A_TrigT[q] = ((data0[soffset + (i * OscWordSize) + samples] >> 16) & 0x1);
                PMD.A_TrigC[q] = ((data0[soffset + (i * OscWordSize) + samples] >> 17) & 0x1);
                PMD.A_Trig[q] = ((data0[soffset + (i * OscWordSize) + samples] >> 18) & 0x1);
                PMD.A_HIT[q] = ((data0[soffset + (i * OscWordSize) + samples] >> 19) & 0x1);

                q++;
            }



            q = 0;

            for (int i = (int)read_pos1; i < samples - 1; i++)
            {
                PMD.B_chargeLG[q] = ((data1[soffset + (i * OscWordSize) + 0]) & 0xFFFF);
                PMD.B_chargeHG[q] = ((data1[soffset + (i * OscWordSize) + samples]) & 0xFFFF);
                PMD.B_CLK[q] = ((data1[soffset + (i * OscWordSize) + 0] >> 16) & 0x1);
                PMD.B_SR[q] = ((data1[soffset + (i * OscWordSize) + 0] >> 17) & 0x1);
                PMD.B_Trig_Ext[q] = ((data1[soffset + (i * OscWordSize) + 0] >> 18) & 0x1);
                PMD.B_Veto_Ext[q] = ((data1[soffset + (i * OscWordSize) + 0] >> 19) & 0x1);
                PMD.B_TrigT[q] = ((data1[soffset + (i * OscWordSize) + samples] >> 16) & 0x1);
                PMD.B_TrigC[q] = ((data1[soffset + (i * OscWordSize) + samples] >> 17) & 0x1);
                PMD.B_Trig[q] = ((data1[soffset + (i * OscWordSize) + samples] >> 18) & 0x1);
                PMD.B_HIT[q] = ((data1[soffset + (i * OscWordSize) + samples] >> 19) & 0x1);
                q++;
            }

            for (int i = 0; i < (int)read_pos1; i++)
            {
                PMD.B_chargeLG[q] = ((data1[soffset + (i * OscWordSize) + 0]) & 0xFFFF);
                PMD.B_chargeHG[q] = ((data1[soffset + (i * OscWordSize) + samples]) & 0xFFFF);
                PMD.B_CLK[q] = ((data1[soffset + (i * OscWordSize) + 0] >> 16) & 0x1);
                PMD.B_SR[q] = ((data1[soffset + (i * OscWordSize) + 0] >> 17) & 0x1);
                PMD.B_Trig_Ext[q] = ((data1[soffset + (i * OscWordSize) + 0] >> 18) & 0x1);
                PMD.B_Veto_Ext[q] = ((data1[soffset + (i * OscWordSize) + 0] >> 19) & 0x1);
                PMD.B_TrigT[q] = ((data1[soffset + (i * OscWordSize) + samples] >> 16) & 0x1);
                PMD.B_TrigC[q] = ((data1[soffset + (i * OscWordSize) + samples] >> 17) & 0x1);
                PMD.B_Trig[q] = ((data1[soffset + (i * OscWordSize) + samples] >> 18) & 0x1);
                PMD.B_HIT[q] = ((data1[soffset + (i * OscWordSize) + samples] >> 19) & 0x1);
                q++;
            }


            q = 0;

            for (int i = (int)read_pos2; i < samples - 1; i++)
            {
                PMD.C_chargeLG[q] = ((data2[soffset + (i * OscWordSize) + 0]) & 0xFFFF);
                PMD.C_chargeHG[q] = ((data2[soffset + (i * OscWordSize) + samples]) & 0xFFFF);
                PMD.C_CLK[q] = ((data2[soffset + (i * OscWordSize) + 0] >> 16) & 0x1);
                PMD.C_SR[q] = ((data2[soffset + (i * OscWordSize) + 0] >> 17) & 0x1);
                PMD.C_Trig_Ext[q] = ((data2[soffset + (i * OscWordSize) + 0] >> 18) & 0x1);
                PMD.C_Veto_Ext[q] = ((data2[soffset + (i * OscWordSize) + 0] >> 19) & 0x1);
                PMD.C_TrigT[q] = ((data2[soffset + (i * OscWordSize) + samples] >> 16) & 0x1);
                PMD.C_TrigC[q] = ((data2[soffset + (i * OscWordSize) + samples] >> 17) & 0x1);
                PMD.C_Trig[q] = ((data2[soffset + (i * OscWordSize) + samples] >> 18) & 0x1);
                PMD.C_HIT[q] = ((data2[soffset + (i * OscWordSize) + samples] >> 19) & 0x1);
                q++;
            }

            for (int i = 0; i < (int)read_pos2; i++)
            {
                PMD.C_chargeLG[q] = ((data2[soffset + (i * OscWordSize) + 0]) & 0xFFFF);
                PMD.C_chargeHG[q] = ((data2[soffset + (i * OscWordSize) + samples]) & 0xFFFF);
                PMD.C_CLK[q] = ((data2[soffset + (i * OscWordSize) + 0] >> 16) & 0x1);
                PMD.C_SR[q] = ((data2[soffset + (i * OscWordSize) + 0] >> 17) & 0x1);
                PMD.C_Trig_Ext[q] = ((data2[soffset + (i * OscWordSize) + 0] >> 18) & 0x1);
                PMD.C_Veto_Ext[q] = ((data2[soffset + (i * OscWordSize) + 0] >> 19) & 0x1);
                PMD.C_TrigT[q] = ((data2[soffset + (i * OscWordSize) + samples] >> 16) & 0x1);
                PMD.C_TrigC[q] = ((data2[soffset + (i * OscWordSize) + samples] >> 17) & 0x1);
                PMD.C_Trig[q] = ((data2[soffset + (i * OscWordSize) + samples] >> 18) & 0x1);
                PMD.C_HIT[q] = ((data2[soffset + (i * OscWordSize) + samples] >> 19) & 0x1);
                q++;
            }


            q = 0;

            for (int i = (int)read_pos3; i < samples - 1; i++)
            {
                PMD.D_chargeLG[q] = ((data3[soffset + (i * OscWordSize) + 0]) & 0xFFFF);
                PMD.D_chargeHG[q] = ((data3[soffset + (i * OscWordSize) + samples]) & 0xFFFF);
                PMD.D_CLK[q] = ((data3[soffset + (i * OscWordSize) + 0] >> 16) & 0x1);
                PMD.D_SR[q] = ((data3[soffset + (i * OscWordSize) + 0] >> 17) & 0x1);
                PMD.D_Trig_Ext[q] = ((data3[soffset + (i * OscWordSize) + 0] >> 18) & 0x1);
                PMD.D_Veto_Ext[q] = ((data3[soffset + (i * OscWordSize) + 0] >> 19) & 0x1);
                PMD.D_TrigT[q] = ((data3[soffset + (i * OscWordSize) + samples] >> 16) & 0x1);
                PMD.D_TrigC[q] = ((data3[soffset + (i * OscWordSize) + samples] >> 17) & 0x1);
                PMD.D_Trig[q] = ((data3[soffset + (i * OscWordSize) + samples] >> 18) & 0x1);
                PMD.D_HIT[q] = ((data3[soffset + (i * OscWordSize) + samples] >> 19) & 0x1);
                q++;
            }

            for (int i = 0; i < (int)read_pos3; i++)
            {
                PMD.D_chargeLG[q] = ((data3[soffset + (i * OscWordSize) + 0]) & 0xFFFF);
                PMD.D_chargeHG[q] = ((data3[soffset + (i * OscWordSize) + samples]) & 0xFFFF);
                PMD.D_CLK[q] = ((data3[soffset + (i * OscWordSize) + 0] >> 16) & 0x1);
                PMD.D_SR[q] = ((data3[soffset + (i * OscWordSize) + 0] >> 17) & 0x1);
                PMD.D_Trig_Ext[q] = ((data3[soffset + (i * OscWordSize) + 0] >> 18) & 0x1);
                PMD.D_Veto_Ext[q] = ((data3[soffset + (i * OscWordSize) + 0] >> 19) & 0x1);
                PMD.D_TrigT[q] = ((data3[soffset + (i * OscWordSize) + samples] >> 16) & 0x1);
                PMD.D_TrigC[q] = ((data3[soffset + (i * OscWordSize) + samples] >> 17) & 0x1);
                PMD.D_Trig[q] = ((data3[soffset + (i * OscWordSize) + samples] >> 18) & 0x1);
                PMD.D_HIT[q] = ((data3[soffset + (i * OscWordSize) + samples] >> 19) & 0x1);
                q++;
            }

            return true;
           


        }

    }
}

