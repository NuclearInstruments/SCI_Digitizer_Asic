

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DT5550W_P_lib
{

    public class DT5550W
    {
        const UInt32 RFA_OSC_BA = 0x80070000;
        const UInt32 RFA_OSC_AVAL = 0xFFFF;
        const UInt32 RFA_OSC_ARM = 0xFFFE;
        const UInt32 RFA_OSC_TRIGMODE = 0xFFFD;

        const UInt32 RFA_CLKGEN_BA = 0x80060000;
        const UInt32 RFA_IIC_CONTROL = 0x80050008;
        const UInt32 RFA_IIC_STATUS = 0x80050009;
        const UInt32 RFA_CLKGEN_STROBE = 0x09;
        const UInt32 RFA_CLKGEN_CFG0 = 0x00;
        const UInt32 RFA_CLKGEN_CFG1 = 0x01;
        const UInt32 RFA_CLKGEN_CFG2 = 0x02;
        const UInt32 RFA_CLKGEN_CFG3 = 0x03;
        const UInt32 RFA_CLKGEN_CFG4 = 0x04;
        const UInt32 RFA_CLKGEN_CFG5 = 0x05;
        const UInt32 RFA_CLKGEN_CFG6 = 0x06;
        const UInt32 RFA_CLKGEN_CFG7 = 0x07;
        const UInt32 RFA_CLKGEN_CFG8 = 0x08;

        const UInt32 RFA_PTROC_BA_A = 0x80010000;
        const UInt32 RFA_PTROC_BA_B = 0x80020000;
        const UInt32 RFA_PTROC_BA_C = 0x80030000;
        const UInt32 RFA_PTROC_BA_D = 0x80040000;
        const UInt32 RFA_PTROC_PROG = 20;
        const UInt32 RFA_PTROC_PROGMONITOR = 21;

        const UInt32 PETIROC_RAZ_FORCE = 0xFFFFF903;
        const UInt32 PETIROC_RSTB_FORCE = 0xFFFFF902;
        const UInt32 PETIROC_VALID_EVNT = 0xFFFFF900;
        const UInt32 PETIROC_STARTB = 0xFFFFF905;
        const UInt32 PETIROC_DELAY_TRIGGER = 0xFFFFF906;
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
        public PetirocConfig PetirocCfg;

        public List<PetirocConfig> pCFG;

        public string SerialNumber;

        public enum t_AsicModels
        {
            PETIROC =0,
            MAROC = 1,
            CITIROC = 2,
            GEMROC = 3,
            PHOTOROC = 4
        }

        t_AsicModels DLL_CURRENT_ASIC = 0;
        int DLL_ASIC_COUNT = 4;

        public class PetirocMonitorData
        {
            public float[] global_trigger;
            public float[] trig_b_mux_a;
            public float[] trig_b_mux_b;
            public float[] trig_b_mux_c;
            public float[] trig_b_mux_d;
            public float[] sr_clk_a;
            public float[] sr_clk_b;
            public float[] sr_clk_c;
            public float[] sr_clk_d;
            public float[] sr_din_a;
            public float[] sr_din_b;
            public float[] sr_din_c;
            public float[] sr_din_d;
            public float[] trigger_a;
            public float[] trigger_b;
            public float[] trigger_c;
            public float[] trigger_d;
            public float[] lemo1;
            public float[] lemo2;
            public float[] lemo3;
            public float[] lemo4;
            public float[] chrage_trig_a;
            public float[] chrage_trig_b;
            public float[] chrage_trig_c;
            public float[] chrage_trig_d;
            public float[] dig_probe_a;
            public float[] dig_probe_b;
            public float[] dig_probe_c;
            public float[] dig_probe_d;
            public float[,] trgs_a;
            public float[,] trgs_b;
            public float[,] trgs_c;
            public float[,] trgs_d;
            public float[] an_probe_a;
            public float[] charge_mux_a;
            public float[] an_probe_b;
            public float[] charge_mux_b;
            public float[] an_probe_c;
            public float[] charge_mux_c;
            public float[] an_probe_d;
            public float[] charge_mux_d;

            public PetirocMonitorData(int size)
            {
                global_trigger = new float[size];
                trig_b_mux_a = new float[size];
                trig_b_mux_b = new float[size];
                trig_b_mux_c = new float[size];
                trig_b_mux_d = new float[size];
                sr_clk_a = new float[size];
                sr_clk_b = new float[size];
                sr_clk_c = new float[size];
                sr_clk_d = new float[size];
                sr_din_a = new float[size];
                sr_din_b = new float[size];
                sr_din_c = new float[size];
                sr_din_d = new float[size];
                trigger_a = new float[size];
                trigger_b = new float[size];
                trigger_c = new float[size];
                trigger_d = new float[size];
                lemo1 = new float[size];
                lemo2 = new float[size];
                lemo3 = new float[size];
                lemo4 = new float[size];
                chrage_trig_a = new float[size];
                chrage_trig_b = new float[size];
                chrage_trig_c = new float[size];
                chrage_trig_d = new float[size];
                dig_probe_a = new float[size];
                dig_probe_b = new float[size];
                dig_probe_c = new float[size];
                dig_probe_d = new float[size];
                trgs_a = new float[size,32];
                trgs_b = new float[size,32];
                trgs_c = new float[size,32];
                trgs_d = new float[size,32];
                an_probe_a = new float[size];
                charge_mux_a = new float[size];
                an_probe_b = new float[size];
                charge_mux_b = new float[size];
                an_probe_c = new float[size];
                charge_mux_c = new float[size];
                an_probe_d = new float[size];
                charge_mux_d = new float[size];
            }

        }
        public class PetirocConfig
        {
            public struct tDAC
            {
                public bool enable;
                public int value;
            };

            public struct tInputDiscriminator
            {
                public bool maskDiscriminatorQ;
                public bool maskDiscriminatorT;
                public int DACValue;
            };

            public struct tPowerControl
            {
                public bool Dual_10bit_DAC;
                public bool ADC_Ramp;
                public bool DacDelay;
                public bool Delay_Ramp;
                public bool Delay_Discr;
                public bool TemperatureSensor;
                public bool PreAmplifier;
                public bool DiscrT;
                public bool DACs_6bit;
                public bool TDC_ramp;
                public bool Slow_Shaper;
                public bool SCA;
                public bool DiscrQ;
                public bool DiscrADCT;
                public bool DiscrADCQ;
                public bool LVDS_Rx_Slow;
                public bool LVDS_Rx_Fast;
                public bool LVDS_Tx;
                public bool InputDac;
                public bool OtaQ;
                public bool OtaMux;
                public bool OtaProbe;
            };

            public struct tPulseMode
            {
                public bool Dual_10bit_DAC;
                public bool ADC_Ramp;
                public bool Input_DACs;
                public bool Delay_Ramp;
                public bool Delay_Discr;
                public bool TemperatureSensor;
                public bool PreAmplifier;
                public bool DiscrT;
                public bool DACs_6bit;
                public bool TDC_ramp;
                public bool Slow_Shaper;
                public bool SCA;
                public bool DiscrQ;
                public bool DiscrADCT;
                public bool DiscrADCQ;
                public bool LVDS_Rx_Slow;
                public bool LVDS_Rx_Fast;
                public bool LVDS_Tx;
            };

            public enum tPOLARITY
            {
                POSITIVE = 0,
                NEGATIVE = 1
            }


            public enum tFBC
            {
                fF100 = 0,
                fF200 = 1,
                fF300 = 2,
                fF400 = 3
            }

            public enum tFIC
            {
                pF1_25 = 0,
                pF2_5 = 1,
                pF3_75 = 2,
                pF5 = 3
            }

            public enum tLVDS_mA
            {
                mA1 = 0,
                mA2 = 1,
                mA3 = 2,
                mA4 = 3
            }

            public enum tPetirocMonitorSelector
            {
                None = 0,
                BiasDac ,
                PreamOutput,
                TimeDiscriminator ,
                PreampDummy,
                RampTDC ,
                ChrageDiscriminator,
                CRRC ,
                StartOfADCRamp,
                Hold
            }

            public struct tCorrection
            {
                public double Gain;
                public int Offset;
            }

            public tDAC[] inputDAC;
            public tInputDiscriminator[] InputDiscriminator;
            public tPowerControl PowerControl;
            public tPulseMode PulseMode;
            public tCorrection[] Correction;
            public tPOLARITY InputPolarity;
            public bool TriggerLatch;
            public tFBC SlowFeedbackC;
            public tFIC SlowInputC;
            public tLVDS_mA LvdsTxmA;
            public bool ADC_ramp_compensation;
            public bool Select_80_MHz;
            public bool Enable_80_MHz;
            public bool Raz_Ext;
            public bool Raz_Int;
            public bool Startb_Ext;
            public bool Trigger_Mux;
            public bool NOR32_Time;
            public bool NOR32_Change;
            public bool TriggerOut;
            public bool Dout_Oc;
            public bool TransmitOn_Oc;

            public int DAC_Q_threshold;
            public int DAC_T_threshold;
            public int DelayTrigger;


            public int MonitorChannel = 0;
            public tPetirocMonitorSelector PetirocMonitorSelector = 0;
            public PetirocConfig()
            {
                this.Correction = new tCorrection[32];
                this.inputDAC = new tDAC[33];
                this.InputDiscriminator = new tInputDiscriminator[32];
                for (int i = 0; i < 32; i++)
                {
                    this.inputDAC[i].enable = true;
                    this.inputDAC[i].value = 128;
                    this.InputDiscriminator[i].maskDiscriminatorQ = false;
                    this.InputDiscriminator[i].maskDiscriminatorT = false;
                    this.InputDiscriminator[i].DACValue = 32;
                    this.Correction[i].Gain = 1.0;
                    this.Correction[i].Offset = 0;
                }
                this.inputDAC[32].enable = true;
                this.inputDAC[32].value = 128;

                this.PowerControl.ADC_Ramp = true;
                this.PowerControl.DACs_6bit = true;
                this.PowerControl.Delay_Discr = true;
                this.PowerControl.Delay_Ramp = true;
                this.PowerControl.DiscrADCQ = true;
                this.PowerControl.DiscrADCT = true;
                this.PowerControl.DiscrQ = true;
                this.PowerControl.DiscrT = true;
                this.PowerControl.Dual_10bit_DAC = true;
                this.PowerControl.InputDac = true;
                this.PowerControl.OtaMux = true;
                this.PowerControl.OtaProbe = true;
                this.PowerControl.OtaQ = true;
                this.PowerControl.DacDelay = true;
                this.PowerControl.LVDS_Rx_Fast = true;
                this.PowerControl.LVDS_Rx_Slow = true;
                this.PowerControl.LVDS_Tx = true;
                this.PowerControl.PreAmplifier = true;
                this.PowerControl.SCA = true;
                this.PowerControl.Slow_Shaper = true;
                this.PowerControl.TDC_ramp = true;
                this.PowerControl.TemperatureSensor = true;

                this.PulseMode.ADC_Ramp = false;
                this.PulseMode.DACs_6bit = false;
                this.PulseMode.Delay_Discr = false;
                this.PulseMode.Delay_Ramp = false;
                this.PulseMode.DiscrADCQ = false;
                this.PulseMode.DiscrADCT = false;
                this.PulseMode.DiscrQ = false;
                this.PulseMode.DiscrT = false;
                this.PulseMode.Dual_10bit_DAC = false;
                this.PulseMode.Input_DACs = false;
                this.PulseMode.LVDS_Rx_Fast = false;
                this.PulseMode.LVDS_Rx_Slow = false;
                this.PulseMode.LVDS_Tx = false;
                this.PulseMode.PreAmplifier = false;
                this.PulseMode.SCA = false;
                this.PulseMode.Slow_Shaper = false;
                this.PulseMode.TDC_ramp = false;
                this.PulseMode.TemperatureSensor = false;


                this.InputPolarity = tPOLARITY.NEGATIVE;
                this.TriggerLatch = true;
                this.SlowFeedbackC = tFBC.fF300;
                this.SlowInputC = tFIC.pF1_25;
                this.LvdsTxmA = tLVDS_mA.mA4;
                this.ADC_ramp_compensation = false;
                this.Select_80_MHz = true;
                this.Enable_80_MHz = true;
                this.Raz_Ext = false;
                this.Raz_Int = true;
                this.Startb_Ext = false;
                this.Trigger_Mux = true;
                this.NOR32_Time = false;
                this.NOR32_Change = false;
                this.TriggerOut = true;
                this.Dout_Oc = true;
                this.TransmitOn_Oc = true;

                this.DAC_Q_threshold = 1024;
                this.DAC_T_threshold = 1024;
                this.DelayTrigger = 31;

                this.PetirocMonitorSelector = tPetirocMonitorSelector.None;
                this.MonitorChannel = 0;
            }

            void SetSingleBit(int bit_position, bool value, bool[] bitarray)
            {
                bitarray[bit_position] = value;
            }

            void SetValue(int bit_position, int value, UInt32 bit_count, bool[] bitarray)
            {
                BitArray b = new BitArray(new int[] { value });
                int[] bits = b.Cast<bool>().Select(bit => bit ? 1 : 0).ToArray();

                for (int i = 0; i < bit_count; i++)
                    bitarray[bit_position + i] = bits[i] == 0 ? false : true;
            }

            void SetValueR(int bit_position, int value, UInt32 bit_count, bool[] bitarray)
            {
                BitArray b = new BitArray(new int[] { value });
                int[] bits = b.Cast<bool>().Select(bit => bit ? 1 : 0).ToArray();

                for (int i = 0; i < bit_count; i++)
                    bitarray[bit_position + i] = bits[bit_count - i] == 0 ? false : true;
            }

            public void GenerateBitMonitor(bool[] datavector)
            {
                bool[] bitarray = new bool[195];
                for (int i = 0; i < 195; i++)
                {
                    bitarray[i] = false;
                }
                    switch (PetirocMonitorSelector)
                {
                    case tPetirocMonitorSelector.None:
                        break;
                    case tPetirocMonitorSelector.BiasDac:
                        bitarray[0 + MonitorChannel] = true;
                        break;
                    case tPetirocMonitorSelector.PreamOutput:
                        bitarray[32 + MonitorChannel*2] = true;
                        break;
                    case tPetirocMonitorSelector.TimeDiscriminator:
                        bitarray[32 + MonitorChannel*2+1] = true;
                        break;
                    case tPetirocMonitorSelector.PreampDummy:
                        bitarray[96] = true;
                        break;
                    case tPetirocMonitorSelector.RampTDC:
                        bitarray[97 + MonitorChannel] = true;
                        break;
                    case tPetirocMonitorSelector.ChrageDiscriminator:
                        bitarray[129 + MonitorChannel*2] = true;
                        break;
                    case tPetirocMonitorSelector.CRRC:
                        bitarray[129 + MonitorChannel*2+1] = true;
                        break;
                    case tPetirocMonitorSelector.StartOfADCRamp:
                        bitarray[193] = true;
                        break;
                    case tPetirocMonitorSelector.Hold:
                        bitarray[194] = true;
                        break;

                }
                for (int i = 0; i < 195; i++)
                {
                    datavector[i] = bitarray[i];
                }

            }

            public void GenerateBitConfig(bool[] datavector)
            {
                bool[] bitarray = new bool[641];
                for (int i = 0; i < 640; i++)
                {
                    bitarray[i] = false;
                }
                for (int i = 0; i < 32; i++)
                {
                    SetSingleBit(32 + i * 9 + 8, this.inputDAC[i].enable, bitarray);
                    SetValue(32 + i * 9, (int)this.inputDAC[i].value, 8, bitarray);
                    SetSingleBit(0 + i, this.InputDiscriminator[i].maskDiscriminatorQ, bitarray);
                    SetSingleBit(328 + i, this.InputDiscriminator[i].maskDiscriminatorT, bitarray);
                    SetValue(360 + i * 6, this.InputDiscriminator[i].DACValue, 6, bitarray);
                }

                //Dummy DAC
                SetValue(320, (int)this.inputDAC[32].value, 8, bitarray);

                //vth_discri charge
                SetValueR(554, this.DAC_Q_threshold * 2, 10, bitarray);

                //vth_time
                SetValueR(564, this.DAC_T_threshold * 2, 10, bitarray);

                //sel_starb_ramp_adc_ext
                SetSingleBit(576, this.Startb_Ext, bitarray);



                //usebcompensation
                SetSingleBit(577, this.ADC_ramp_compensation, bitarray);


                SetValue(582, this.DelayTrigger, 8, bitarray);

                //cmd_polarity
                SetSingleBit(598, this.InputPolarity == tPOLARITY.POSITIVE ? true : false, bitarray);

                //latch
                SetSingleBit(599, this.TriggerLatch, bitarray);

                //CF3,2
                SetValueR(607, (int)this.SlowFeedbackC, 2, bitarray);

                //CF1,0
                SetValueR(609, (int)this.SlowInputC, 2, bitarray);

                //DIS_razchn_int
                SetSingleBit(619, !this.Raz_Int, bitarray);

                //DisableRazInt
                SetSingleBit(620, !this.Raz_Ext, bitarray);

                //SEL_80M
                SetSingleBit(621, this.Select_80_MHz, bitarray);

                //EN_80M
                SetSingleBit(622, this.Enable_80_MHz, bitarray);

                //ON/OFF_1mA
                SetValueR(629, (int)this.LvdsTxmA, 2, bitarray);

                SetSingleBit(634, !this.Trigger_Mux, bitarray);
                SetSingleBit(635, this.NOR32_Time, bitarray);
                SetSingleBit(636, this.NOR32_Change, bitarray);
                SetSingleBit(637, !this.TriggerOut, bitarray);
                SetSingleBit(638, this.Dout_Oc, bitarray);
                SetSingleBit(639, this.TransmitOn_Oc, bitarray);


                SetSingleBit(574, this.PowerControl.ADC_Ramp, bitarray);
                SetSingleBit(600, this.PowerControl.DACs_6bit, bitarray);
                SetSingleBit(590, this.PowerControl.Delay_Discr, bitarray);
                SetSingleBit(580, this.PowerControl.Delay_Ramp, bitarray);
                SetSingleBit(615, this.PowerControl.DiscrADCQ, bitarray);
                SetSingleBit(617, this.PowerControl.DiscrADCT, bitarray);
                SetSingleBit(613, this.PowerControl.DiscrQ, bitarray);
                SetSingleBit(596, this.PowerControl.DiscrT, bitarray);
                SetSingleBit(552, this.PowerControl.Dual_10bit_DAC, bitarray);
                SetSingleBit(604, this.PowerControl.InputDac, bitarray);
                SetSingleBit(631, this.PowerControl.OtaMux, bitarray);
                SetSingleBit(632, this.PowerControl.OtaProbe, bitarray);
                SetSingleBit(633, this.PowerControl.OtaQ, bitarray);
                SetSingleBit(578, this.PowerControl.DacDelay, bitarray);
                SetSingleBit(625, this.PowerControl.LVDS_Rx_Fast, bitarray);
                SetSingleBit(623, this.PowerControl.LVDS_Rx_Slow, bitarray);
                SetSingleBit(627, this.PowerControl.LVDS_Tx, bitarray);
                SetSingleBit(594, this.PowerControl.PreAmplifier, bitarray);
                SetSingleBit(611, this.PowerControl.SCA, bitarray);
                SetSingleBit(605, this.PowerControl.Slow_Shaper, bitarray);
                SetSingleBit(602, this.PowerControl.TDC_ramp, bitarray);
                SetSingleBit(593, this.PowerControl.TemperatureSensor, bitarray);

                SetSingleBit(575, this.PulseMode.ADC_Ramp, bitarray);
                SetSingleBit(601, this.PulseMode.DACs_6bit, bitarray);
                SetSingleBit(591, this.PulseMode.Delay_Discr, bitarray);
                SetSingleBit(581, this.PulseMode.Delay_Ramp, bitarray);
                SetSingleBit(618, this.PulseMode.DiscrADCQ, bitarray);
                SetSingleBit(616, this.PulseMode.DiscrADCT, bitarray);
                SetSingleBit(614, this.PulseMode.DiscrQ, bitarray);
                SetSingleBit(597, this.PulseMode.DiscrT, bitarray);
                SetSingleBit(553, this.PulseMode.Dual_10bit_DAC, bitarray);
                SetSingleBit(579, this.PulseMode.Input_DACs, bitarray);
                SetSingleBit(624, this.PulseMode.LVDS_Rx_Fast, bitarray);
                SetSingleBit(626, this.PulseMode.LVDS_Rx_Slow, bitarray);
                SetSingleBit(628, this.PulseMode.LVDS_Tx, bitarray);
                SetSingleBit(595, this.PulseMode.PreAmplifier, bitarray);
                SetSingleBit(612, this.PulseMode.SCA, bitarray);
                SetSingleBit(606, this.PulseMode.Slow_Shaper, bitarray);
                SetSingleBit(603, this.PulseMode.TDC_ramp, bitarray);
                SetSingleBit(592, this.PulseMode.TemperatureSensor, bitarray);

                for (int i = 0; i < 640; i++)
                {
                    datavector[i] = bitarray[i];
                }

            }

            public string GenerateStringConfig()
            {
                string bitstream = "";
                bool[] bitarray = new bool[641];
                GenerateBitConfig(bitarray);
                for (int i = 0; i < 640; i++)
                {
                    bitstream = bitstream + (bitarray[i] ? "1" : "0");
                }
                return bitstream;
            }

            public void GenerateUint32Config(UInt32[] datavector)
            {
                int i, j;
                bool[] bitarray = new bool[641];
                GenerateBitConfig(bitarray);
                for (i = 0; i < 20; i++)
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
                bool[] bitarray = new bool[32*7];
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

        public enum EventPacketSize
        {
            None = 0,
            PetirocPacket = 38
        }


        public class t_DataPETIROC
        {
            public UInt64 EventTimecode;
            public UInt64 RunEventTimecode;
            public UInt64 EventCounter;
            public UInt16 AsicID;

            public double EventTimecode_ns;
            public double RunEventTimecode_ns;

            public ushort[] CoarseTime;
            public ushort[] FineTime;
            public ushort[] charge;
            public double[] relative_time;
            public bool[] hit;
            public t_DataPETIROC()
            {
                CoarseTime = new ushort[32];
                FineTime = new ushort[32];
                charge = new ushort[32];
                hit = new bool[32];
                relative_time = new double[32];
            }

        }

        public struct t_BoardInfo
        {
            public enum t_AsicType { PETIROC = 0, MAROC = 1 };
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

        public void SetManualAsicInfo(int AsicCount)
        {
            DLL_ASIC_COUNT = AsicCount;
        }
        public t_BoardInfo GetBoardInfo()
        {
            t_BoardInfo BI = new t_BoardInfo();
            BI.AsicType = t_BoardInfo.t_AsicType.PETIROC;
            BI.DefaultDetectorLayout = t_BoardInfo.t_DefaultDetectorLayout.MATRIX_8x8;
            BI.totalAsics = DLL_ASIC_COUNT;
            BI.channelsPerAsic = 32;
            BI.DigitalDataPacketSize = 38;
            BI.FPGATimecode_ns = 25;
            BI.Coarse_ns = 25;
            BI.Fine_ns = 0.037;
            BI.totalChannels = BI.channelsPerAsic * BI.totalAsics;
            BI.SerialNumber = SerialNumber;
            return BI;
        }

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

        public DT5550W()
        {
            Handle = new IntPtr();
            NI_USB3_Init();
            PetirocCfg = new PetirocConfig();
            pCFG = new List<PetirocConfig>() ;
            for (int i=0;i<4;i++)
                pCFG.Add(new PetirocConfig());
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


        int CfgClockGenerator()
        {
            //CFG0 0xEB0A0320 //ADC 80 MHZ  
            if (NI_USB3_WriteReg(0xEB140320, RFA_CLKGEN_BA + RFA_CLKGEN_CFG0, ref Handle) != 0) //83800320
                return -1;

            System.Threading.Thread.Sleep(5);
            //CFG1

            if (NI_USB3_WriteReg(0xEB140301, RFA_CLKGEN_BA + RFA_CLKGEN_CFG1, ref Handle) != 0) //83800301
                return -1;

            System.Threading.Thread.Sleep(5);
            //CFG2
            if (NI_USB3_WriteReg(0xEB020302, RFA_CLKGEN_BA + RFA_CLKGEN_CFG2, ref Handle) != 0) //EB0C0302
                return -1;

            System.Threading.Thread.Sleep(5);
            //CFG3 EB020303  EB0A0303
            if (NI_USB3_WriteReg(0xEB020303, RFA_CLKGEN_BA + RFA_CLKGEN_CFG3, ref Handle) != 0)
                return -1;
            System.Threading.Thread.Sleep(5);

            //CFG4
            if (NI_USB3_WriteReg(0x68860314, RFA_CLKGEN_BA + RFA_CLKGEN_CFG4, ref Handle) != 0)
                return -1;

            System.Threading.Thread.Sleep(5);

            //CFG5
            if (NI_USB3_WriteReg(0x10100B25, RFA_CLKGEN_BA + RFA_CLKGEN_CFG5, ref Handle) != 0)
                return -1;

            System.Threading.Thread.Sleep(5);
            //CFG6
            if (NI_USB3_WriteReg(0x80BE1B66, RFA_CLKGEN_BA + RFA_CLKGEN_CFG6, ref Handle) != 0)
                return -1;

            System.Threading.Thread.Sleep(5);
            //CFG7
            if (NI_USB3_WriteReg(0x950037F7, RFA_CLKGEN_BA + RFA_CLKGEN_CFG7, ref Handle) != 0)
                return -1;

            System.Threading.Thread.Sleep(5);
            //CFG8
            if (NI_USB3_WriteReg(0x20009D98, RFA_CLKGEN_BA + RFA_CLKGEN_CFG8, ref Handle) != 0)
                return -1;
            System.Threading.Thread.Sleep(25);


            //STROBE
            if (NI_USB3_WriteReg(1, RFA_CLKGEN_BA + RFA_CLKGEN_STROBE, ref Handle) != 0)
                return -1;
            if (NI_USB3_WriteReg(0, RFA_CLKGEN_BA + RFA_CLKGEN_STROBE, ref Handle) != 0)
                return -1;

            if (NI_USB3_WriteReg(0x84BE1B66, RFA_CLKGEN_BA + RFA_CLKGEN_CFG6, ref Handle) != 0)
                return -1;

            System.Threading.Thread.Sleep(25);
            if (NI_USB3_WriteReg(1, RFA_CLKGEN_BA + RFA_CLKGEN_STROBE, ref Handle) != 0)
                return -1;
            if (NI_USB3_WriteReg(0, RFA_CLKGEN_BA + RFA_CLKGEN_STROBE, ref Handle) != 0)
                return -1;

            if (NI_USB3_WriteReg(0x80BE1B66, RFA_CLKGEN_BA + RFA_CLKGEN_CFG6, ref Handle) != 0)
                return -1;
            System.Threading.Thread.Sleep(25);

            if (NI_USB3_WriteReg(1, RFA_CLKGEN_BA + RFA_CLKGEN_STROBE, ref Handle) != 0)
                return -1;
            if (NI_USB3_WriteReg(0, RFA_CLKGEN_BA + RFA_CLKGEN_STROBE, ref Handle) != 0)
                return -1;
            System.Threading.Thread.Sleep(25);

            return 0;
        }

        unsafe private int IIC_WriteData(byte address, byte[] value, int len)
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

        public bool PowerASIC(bool on_off)
        {
            int retvalue;
            byte[] payload = new byte[5];
            byte address;

            payload[0] = 0x03;
            payload[1] = 0x00;
            address = 0x18;
            if (IIC_WriteData(address, payload, 2) != 0)
                return false;
            address = 0x19;
            if (IIC_WriteData(address, payload, 2) != 0)
                return false;


            if (on_off)
            {
                payload[0] = 0x01;
                payload[1] = 0xFF;
            }
            else
            {
                payload[0] = 0x01;
                payload[1] = 0x00;
            }

            address = 0x18;
            if (IIC_WriteData(address, payload, 2) != 0)
                return false;
            address = 0x19;
            if (IIC_WriteData(address, payload, 2) != 0)
                return false;

            return true;
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


        private bool CfgPetiroc(bool progA, bool progB, bool progC, bool progD, UInt32[] cfg)
        {
            /*UInt32 []cfg = {
                0xffffffe0,
                0x06030180,
                0x6030180c,
                0x030180c0,
                0x30180c06,
                0x0180c060,
                0x180c0603,
                0x80c06030,
                0x0c060301,
                0xc0603018,
                0xffffe080,
                0x820820ff,
                0x20820820,
                0x08208208,
                0x82082082,
                0x20820820,
                0x08208208,
                0x401ff982,
                0x35964014,
                0xc3cae2a9};*/

            if (progA)
            {
                for (int i = 0; i < 20; i++)
                    if (NI_USB3_WriteReg(cfg[i], RFA_PTROC_BA_A + (uint)i, ref Handle) != 0)
                        return false;

                if (NI_USB3_WriteReg(1, RFA_PTROC_BA_A + RFA_PTROC_PROG, ref Handle) != 0)
                    return false;
                if (NI_USB3_WriteReg(0, RFA_PTROC_BA_A + RFA_PTROC_PROG, ref Handle) != 0)
                    return false;
            }

            if (progB)
            {
                for (int i = 0; i < 20; i++)
                    if (NI_USB3_WriteReg(cfg[i], RFA_PTROC_BA_B + (uint)i, ref Handle) != 0)
                        return false;

                if (NI_USB3_WriteReg(1, RFA_PTROC_BA_B + RFA_PTROC_PROG, ref Handle) != 0)
                    return false;
                if (NI_USB3_WriteReg(0, RFA_PTROC_BA_B + RFA_PTROC_PROG, ref Handle) != 0)
                    return false;
            }

            if (progC)
            {
                for (int i = 0; i < 20; i++)
                    if (NI_USB3_WriteReg(cfg[i], RFA_PTROC_BA_C + (uint)i, ref Handle) != 0)
                        return false;

                if (NI_USB3_WriteReg(1, RFA_PTROC_BA_C + RFA_PTROC_PROG, ref Handle) != 0)
                    return false;
                if (NI_USB3_WriteReg(0, RFA_PTROC_BA_C + RFA_PTROC_PROG, ref Handle) != 0)
                    return false;
            }

            if (progD)
            {
                for (int i = 0; i < 20; i++)
                    if (NI_USB3_WriteReg(cfg[i], RFA_PTROC_BA_D + (uint)i, ref Handle) != 0)
                        return false;

                if (NI_USB3_WriteReg(1, RFA_PTROC_BA_D + RFA_PTROC_PROG, ref Handle) != 0)
                    return false;
                if (NI_USB3_WriteReg(0, RFA_PTROC_BA_D + RFA_PTROC_PROG, ref Handle) != 0)
                    return false;
            }
            return true;
        }




        public bool ConfigureMonitorPetiroc(bool progA, bool progB, bool progC, bool progD, UInt32[] cfg)
        {
            if (progA)
            {
                for (int i = 0; i < 7; i++)
                    if (NI_USB3_WriteReg(cfg[i], RFA_PTROC_BA_A + (uint)i, ref Handle) != 0)
                        return false;

                if (NI_USB3_WriteReg(1, RFA_PTROC_BA_A + RFA_PTROC_PROGMONITOR, ref Handle) != 0)
                    return false;
                if (NI_USB3_WriteReg(0, RFA_PTROC_BA_A + RFA_PTROC_PROGMONITOR, ref Handle) != 0)
                    return false;
            }

            if (progB)
            {
                for (int i = 0; i < 7; i++)
                    if (NI_USB3_WriteReg(cfg[i], RFA_PTROC_BA_B + (uint)i, ref Handle) != 0)
                        return false;

                if (NI_USB3_WriteReg(1, RFA_PTROC_BA_B + RFA_PTROC_PROGMONITOR, ref Handle) != 0)
                    return false;
                if (NI_USB3_WriteReg(0, RFA_PTROC_BA_B + RFA_PTROC_PROGMONITOR, ref Handle) != 0)
                    return false;
            }

            if (progC)
            {
                for (int i = 0; i < 7; i++)
                    if (NI_USB3_WriteReg(cfg[i], RFA_PTROC_BA_C + (uint)i, ref Handle) != 0)
                        return false;

                if (NI_USB3_WriteReg(1, RFA_PTROC_BA_C + RFA_PTROC_PROGMONITOR, ref Handle) != 0)
                    return false;
                if (NI_USB3_WriteReg(0, RFA_PTROC_BA_C + RFA_PTROC_PROGMONITOR, ref Handle) != 0)
                    return false;
            }

            if (progD)
            {
                for (int i = 0; i < 7; i++)
                    if (NI_USB3_WriteReg(cfg[i], RFA_PTROC_BA_D + (uint)i, ref Handle) != 0)
                        return false;

                if (NI_USB3_WriteReg(1, RFA_PTROC_BA_D + RFA_PTROC_PROGMONITOR, ref Handle) != 0)
                    return false;
                if (NI_USB3_WriteReg(0, RFA_PTROC_BA_D + RFA_PTROC_PROGMONITOR, ref Handle) != 0)
                    return false;
            }
            return true;
        }


        public bool PetirocRaZForce(bool value)
        {
            int retval = 0;
            if (value == false)
                retval = NI_USB3_WriteReg(0xF, PETIROC_RAZ_FORCE, ref Handle);
            else
                retval = NI_USB3_WriteReg(0x0, PETIROC_RAZ_FORCE, ref Handle);
            if (retval != 0)
                return false;
            else
                return true;
        }

        public bool PetirocValEvent(bool value)
        {
            int retval = 0;
            if (value == true)
                retval = NI_USB3_WriteReg(0xF, PETIROC_VALID_EVNT, ref Handle);
            else
                retval = NI_USB3_WriteReg(0x0, PETIROC_VALID_EVNT, ref Handle);
            if (retval != 0)
                return false;
            else
                return true;
        }


        public bool PetirocADCStartExternal(bool value)
        {
            int retval = 0;
            if (value == true)
                retval = NI_USB3_WriteReg(0xF, PETIROC_STARTB, ref Handle);
            else
                retval = NI_USB3_WriteReg(0x0, PETIROC_STARTB, ref Handle);
            if (retval != 0)
                return false;
            else
                return true;
        }

        public bool PetirocResetB(bool value)
        {
            int retval = 0;
            if (value == false)
                retval = NI_USB3_WriteReg(0xF, PETIROC_RSTB_FORCE, ref Handle);
            else
                retval = NI_USB3_WriteReg(0x0, PETIROC_RSTB_FORCE, ref Handle);
            if (retval != 0)
                return false;
            else
                return true;
        }


        public bool PetirocAcquisitionDelay(uint value)
        {
            int retval = 0;
            retval = NI_USB3_WriteReg(value, PETIROC_DELAY_TRIGGER, ref Handle);
            if (retval != 0)
                return false;
            else
                return true;
        }

        public void FlushFIFO()
        {
            NI_USB3_WriteReg(0, DAQ_FLUSH_FIFO, ref Handle);
            System.Threading.Thread.Sleep(1);
            NI_USB3_WriteReg(1, DAQ_FLUSH_FIFO, ref Handle);
            System.Threading.Thread.Sleep(1);
            NI_USB3_WriteReg(0, DAQ_FLUSH_FIFO, ref Handle);
        }


        public void VetoSw(bool veto)
        {
            NI_USB3_WriteReg((UInt32)(veto == true ? 1 : 0), DAQ_VETO_sw, ref Handle);
        }

        public void ExtVetoEnable(bool enable)
        {
            NI_USB3_WriteReg((UInt32)(enable == true ? 1 : 0), DAQ_VETO, ref Handle);
        }


        public bool SetDigitalOutputSignalLength(double ns)
        {
            int clk_s = (int)(ns / 6.25);
            int retval = 0;
            retval = NI_USB3_WriteReg((UInt32)clk_s, DIGOUT_LEN, ref Handle);
            if (retval != 0)
                return false;
            else
                return true;
        }

        public void ConfigureSignalGenerator(bool enableA, bool enableB, bool enableC, bool enableD, int frequency)
        {
            int enable = (enableA ? 1 : 0) + ((enableB ? 1 : 0) << 1) + ((enableC ? 1 : 0) << 2) + ((enableD ? 1 : 0) << 3);
            int period = 160000000 / frequency;
            NI_USB3_WriteReg((UInt32)enable, SIGGEN_ENABLE, ref Handle);
            NI_USB3_WriteReg((UInt32)period, SIGGEN_PERIOD, ref Handle);
        }

        public enum T0Mode { SOFTWARE_STARTRUN = 0, SOFTWARE_PERIODIC = 1, EXTERNAL=2};

        public void ConfigureT0(T0Mode mode, int T0sw_freq)
        {
            int source = 0;
            int swmode = 0;
            switch(mode)
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
            NI_USB3_WriteReg((UInt32)source, T0_SOURCE, ref Handle);
            NI_USB3_WriteReg((UInt32)swmode, T0_SWMODE, ref Handle);
            NI_USB3_WriteReg((UInt32)period, T0_FREQ, ref Handle);
            NI_USB3_WriteReg((UInt32)0, T0_SW, ref Handle);
        }


        public void PulseT0(T0Mode mode, int T0sw_freq)
        {
            NI_USB3_WriteReg((UInt32)0, T0_SW, ref Handle);
            NI_USB3_WriteReg((UInt32)1, T0_SW, ref Handle);
            NI_USB3_WriteReg((UInt32)0, T0_SW, ref Handle);
        }



        public bool ConfigPetiroc(bool progA, bool progB, bool progC, bool progD, UInt32[] cfg)
        {
            PetirocRaZForce(false);
            bool b = CfgPetiroc(progA,progB,progC,progD,cfg);
            PetirocValEvent(true);
            System.Threading.Thread.Sleep(5);
            PetirocResetB(true);
            System.Threading.Thread.Sleep(5);
            PetirocResetB(false);
            System.Threading.Thread.Sleep(5);
            PetirocADCStartExternal(false);
            
            System.Threading.Thread.Sleep(5);
            PetirocRaZForce(true);
            System.Threading.Thread.Sleep(100);
            PetirocRaZForce(false);
            System.Threading.Thread.Sleep(5);
            return b;
        }
        public uint gray_to_bin(uint num, int nbit)
        {
            uint temp = num ^ (num >> 8);
            temp ^= (temp >> 4);
            temp ^= (temp >> 2);
            temp ^= (temp >> 1);
            return temp;
        }

        public int DecodePetirocRowEvents(ref UInt32[] bufferA, UInt32 valid_wordA, ref Queue<t_DataPETIROC> pC)
        {
            int DecodedPackets = 0;
            double Time=0;
            double minTime=0;
            UInt32 i, j, t, s;
            uint[] datarow = new uint[97];
            
            t_DataPETIROC DataPETIROCA = null;
            t = 0;
            s = 0;
            while (t < valid_wordA)
            {
                switch (s)
                {
                    case 0:
                        if (((bufferA[t] >> 4) & 0xc000000) == 0x8000000)
                        {
                            s = 1;
                            DataPETIROCA = new t_DataPETIROC();
                            DataPETIROCA.AsicID = (UInt16) (bufferA[t] & 0xF);
                            DataPETIROCA.EventTimecode = ((UInt64)bufferA[t + 1]);
                            DataPETIROCA.RunEventTimecode = (((UInt64)bufferA[t + 2]) ) + (((UInt64)bufferA[t + 3]) << 32);
                            DataPETIROCA.EventCounter = ((UInt64)bufferA[t + 4]);

                            DataPETIROCA.EventTimecode_ns = DataPETIROCA.EventTimecode * 25;
                            DataPETIROCA.RunEventTimecode_ns = DataPETIROCA.RunEventTimecode * 25;

                            t = t + 5;
                            minTime = 100000000000000;
                        }
                        else
                            t++;
                        break;

                    case 1:
                        for (i = 0; i < 32; i++)
                        {

                            datarow[i * 3 + 2] = (bufferA[t] >> 0) & 0x3FF;
                            datarow[i * 3 + 1] = (bufferA[t] >> 10) & 0x3FF;
                            datarow[i * 3 + 0] = (bufferA[t] >> 20) & 0x3FF;
                            t++;
                        }
                        for (i = 0; i < 32; i++)
                        {
                            DataPETIROCA.FineTime[i] = (ushort)gray_to_bin(datarow[(i * 2) + 0], 10);
                            DataPETIROCA.charge[i] = (ushort)gray_to_bin(datarow[(i * 2) + 1], 10);
                            DataPETIROCA.CoarseTime[i] = (ushort)gray_to_bin(datarow[64 + i] >> 1, 9);
                            DataPETIROCA.hit[i] = (bool)((datarow[64 + i] & 0x1) == 1 ? true : false);
                            Time = (((double)DataPETIROCA.CoarseTime[i] + 1) * 25) - (((double)(DataPETIROCA.FineTime[i] - 150)) * 0.037);
                            if (DataPETIROCA.hit[i])
                                minTime = Time < minTime ? Time : minTime;
                            DataPETIROCA.relative_time[i] = Time;
                        }
                        s = 2;
                        break;

                    case 2:
                        if ((bufferA[t] & 0xc0000000) == 0xc0000000)
                        {
                            if (minTime == 100000000000000) minTime = 0;
                            for (i = 0; i < 32; i++)
                            {
                                DataPETIROCA.relative_time[i] -= minTime;
                            }
                            
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
        public int GetRawBuffer(UInt32 [] data, UInt32 event_count, UInt32 timeout, UInt32 PacketSize, ref UInt32 valid_word)
        {
            int retcode=0;
            UInt32 transfer_length = PacketSize * event_count;
            UInt32 read_word=0;
            UInt32 valid_data=0;
            UInt32 address = 0;
                    address = 0x80000000;
                    retcode = NI_USB3_ReadData_M(data, (UInt32)transfer_length, address, USB_BUS_MODE.STREAMING, (UInt32)timeout, ref read_word, ref valid_data);
            
            valid_word = valid_data;
            return retcode;
        }

        public struct tChannelMapping
        {
            public int X;
            public int Y;
            public int ASICID;
        }

        public bool GetDefaultChannelMapping(ref tChannelMapping [] chmap)
        {
            int i, j,q;
            t_BoardInfo BI = GetBoardInfo();
            
            //Dim xx = {1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 2, 3, 3, 2, 2, 3, 3, 2, 2, 3, 3, 2, 2, 3, 3, 2}
            //Dim yy = {3, 3, 2, 2, 1, 1, 0, 0, 7, 7, 6, 6, 5, 5, 4, 4, 4, 4, 5, 5, 6, 6, 7, 7, 0, 0, 1, 1, 2, 2, 3, 3}

            q = 0;
            chmap = new tChannelMapping[BI.totalAsics * BI.channelsPerAsic];
            if (BI.AsicType == t_BoardInfo.t_AsicType.PETIROC)
            {
                int[] xx = { 2, 3, 3, 2, 2, 3, 3, 2, 2, 3, 3, 2, 2, 3, 3, 2, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1 };
                int[] yy = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 7, 7, 6, 6, 5, 5, 4, 4, 3, 3, 2, 2, 1, 1, 0, 0 };

                for (i=0;i< BI.totalAsics;i++)
                {
                    for (j=0;j< BI.channelsPerAsic; j++)
                    {
                        chmap[q].ASICID = i;
                        chmap[q].X = (4*i) + xx[j];
                        chmap[q].Y = yy[j];
                        q++;
                    }
                }
                return true;
            }
            return false;
        }
        public bool GetMoitor(ref PetirocMonitorData PMD, uint samples)
        {
            uint StatusRegister=0;
            //NI_USB3_ReadReg_M(ref StatusRegister, 0x8007FFFF);
            //if (StatusRegister!=0)
            // {
            NI_USB3_WriteReg_M(1, DT5550W.RFA_OSC_BA + DT5550W.RFA_OSC_TRIGMODE);
            int retcode = 0;
            UInt32 OscWordSize = 512 / (4 * 8);
                UInt32 transfer_length = OscWordSize* samples;
                UInt32[] data = new UInt32[transfer_length*2];
                UInt32 read_word = 0;
                UInt32 valid_data = 0;
                UInt32 address = 0;
                address = DT5550W.RFA_OSC_BA;
                retcode = NI_USB3_ReadData_M(data, (UInt32)transfer_length, address, USB_BUS_MODE.REG_ACCESS, (UInt32)4000, ref read_word, ref valid_data);

                NI_USB3_WriteReg_M(0, DT5550W.RFA_OSC_BA + DT5550W.RFA_OSC_ARM);
                NI_USB3_WriteReg_M(1, DT5550W.RFA_OSC_BA + DT5550W.RFA_OSC_ARM);
                NI_USB3_WriteReg_M(0, DT5550W.RFA_OSC_BA + DT5550W.RFA_OSC_ARM);

            for (int i = 0; i < samples; i++)
            {
                UInt32 t;
                PMD.an_probe_d[i] = -1*((data[1+(i * OscWordSize) + 0] >> 16) & 0xFFFF);
                PMD.charge_mux_d[i] = -1 * ((data[1 + (i * OscWordSize) + 0] >> 0) & 0xFFFF);
                PMD.an_probe_c[i] = -1 * ((data[1 + (i * OscWordSize) + 1] >> 16) & 0xFFFF);
                PMD.charge_mux_c[i] = -1 * ((data[1 + (i * OscWordSize) + 1] >> 0) & 0xFFFF);
                PMD.an_probe_b[i] = -1 * ((data[1 + (i * OscWordSize) + 2] >> 16) & 0xFFFF);
                PMD.charge_mux_b[i] = -1 * ((data[1 + (i * OscWordSize) + 2] >> 0) & 0xFFFF);
                PMD.an_probe_a[i] = -1 * ((data[1 + (i * OscWordSize) + 3] >> 16) & 0xFFFF);
                PMD.charge_mux_a[i] = -1 * ((data[1 + (i * OscWordSize) + 3] >> 0) & 0xFFFF);
                PMD.charge_mux_a[i] = -1 * ((data[(1 + i * OscWordSize) + 3] >> 0) & 0xFFFF);

                t = data[(i * OscWordSize) + 4];
                for (int j = 0; j < 32; j++)
                    PMD.trgs_d[i, j] = (t >> j) & 0x01;

                t = data[(i * OscWordSize) + 5];
                for (int j = 0; j < 32; j++)
                    PMD.trgs_c[i, j] = (t >> j) & 0x01;

                t = data[(i * OscWordSize) + 6];
                for (int j = 0; j < 32; j++)
                    PMD.trgs_b[i, j] = (t >> j) & 0x01;

                t = data[(i * OscWordSize) + 7];
                for (int j = 0; j < 32; j++)
                    PMD.trgs_a[i, j] = (t >> j) & 0x01;

                PMD.dig_probe_d[i] = (data[1 + (i * OscWordSize) + 8] >> 0) & 0x1;
                PMD.dig_probe_c[i] = (data[1 + (i * OscWordSize) + 8] >> 1) & 0x1;
                PMD.dig_probe_b[i] = (data[1 + (i * OscWordSize) + 8] >> 2) & 0x1;
                PMD.dig_probe_a[i] = (data[1 + (i * OscWordSize) + 8] >> 3) & 0x1;

                PMD.chrage_trig_d[i] = (data[1 + (i * OscWordSize) + 8] >> 4) & 0x1;
                PMD.chrage_trig_c[i] = (data[1 + (i * OscWordSize) + 8] >> 5) & 0x1;
                PMD.chrage_trig_b[i] = (data[1 + (i * OscWordSize) + 8] >> 6) & 0x1;
                PMD.chrage_trig_a[i] = (data[1 + (i * OscWordSize) + 8] >> 7) & 0x1;

                PMD.lemo4[i] = (data[1 + (i * OscWordSize) + 8] >> 8) & 0x1;
                PMD.lemo3[i] = (data[1 + (i * OscWordSize) + 8] >> 9) & 0x1;
                PMD.lemo2[i] = (data[1 + (i * OscWordSize) + 8] >> 10) & 0x1;
                PMD.lemo1[i] = (data[1 + (i * OscWordSize) + 8] >> 11) & 0x1;

                PMD.trigger_d[i] = (data[1 + (i * OscWordSize) + 8] >> 12) & 0x1;
                PMD.trigger_c[i] = (data[1 + (i * OscWordSize) + 8] >> 13) & 0x1;
                PMD.trigger_b[i] = (data[1 + (i * OscWordSize) + 8] >> 14) & 0x1;
                PMD.trigger_a[i] = (data[1 + (i * OscWordSize) + 8] >> 15) & 0x1;

                PMD.trig_b_mux_d[i] = (data[1 + (i * OscWordSize) + 8] >> 16) & 0x1;
                PMD.trig_b_mux_c[i] = (data[1 + (i * OscWordSize) + 8] >> 17) & 0x1;
                PMD.trig_b_mux_b[i] = (data[1 + (i * OscWordSize) + 8] >> 18) & 0x1;
                PMD.trig_b_mux_a[i] = (data[1 + (i * OscWordSize) + 8] >> 19) & 0x1;

                PMD.sr_din_d[i] = (data[1 + (i * OscWordSize) + 8] >> 20) & 0x1;
                PMD.sr_din_c[i] = (data[1 + (i * OscWordSize) + 8] >> 21) & 0x1;
                PMD.sr_din_b[i] = (data[1 + (i * OscWordSize) + 8] >> 22) & 0x1;
                PMD.sr_din_a[i] = (data[1 + (i * OscWordSize) + 8] >> 23) & 0x1;

                PMD.sr_clk_d [i] = (data[1 + (i * OscWordSize) + 8] >> 24) & 0x1;
                PMD.sr_clk_c[i] = (data[1 + (i * OscWordSize) + 8] >> 25) & 0x1;
                PMD.sr_clk_b[i] = (data[1 + (i * OscWordSize) + 8] >> 26) & 0x1;
                PMD.sr_clk_a[i] = (data[1 + (i * OscWordSize) + 8] >> 27) & 0x1;

                PMD.global_trigger[i] = (data[1 + (i * OscWordSize) + 8] >> 28) & 0x1;

            }



            return true;
           /* }
            else
            {
                return false;
            }*/


            
        }

        public bool Connect(String DeviceSerialNumber)
        {

            int retcode = NI_USB3_ConnectDevice(DeviceSerialNumber, ref Handle);

            NI_USB3_SetIICControllerBaseAddress(
      RFA_IIC_CONTROL,
      RFA_IIC_STATUS,
      ref Handle);

            CfgClockGenerator();
            PowerASIC(true);
            SerialNumber = DeviceSerialNumber;
            if (retcode == 0)
                return true;
            else
                return false;
        }

        public bool Connect(String DeviceSerialNumber, bool initialize)
        {

            int retcode = NI_USB3_ConnectDevice(DeviceSerialNumber, ref Handle);
            if (initialize == true)
            {
                CfgClockGenerator();
                PowerASIC(true);
            }
            if (retcode == 0)
                return true;
            else
                return false;
        }


        unsafe public int SetHV(bool Enable, float voltage )
        {
           byte [] vv = new byte[16];

            vv[0] = 1;
            vv[1] = 2;
            vv[2] = 0;
            vv[3] = 0;
            vv[4] = 0;
            vv[5] = 0;
            IIC_WriteData(0x70, vv, 6);

            vv[0] = 6;
            vv[1] = 2;
            vv[2] = 10;
            vv[3] = 0;
            vv[4] = 0;
            vv[5] = 0;
            IIC_WriteData(0x70, vv, 6);

            byte[] ff= BitConverter.GetBytes(voltage);
            vv[0] = 2;
            vv[1] = 3;
            vv[2] = ff[0];
            vv[3] = ff[1];
            vv[4] = ff[2];
            vv[5] = ff[3];
            
            
            IIC_WriteData(0x70, vv, 6);

            vv[0] = 0;
            vv[1] = 2;
            vv[2] = (byte) (Enable == true ? 1 : 0);
            vv[3] = 0;
            vv[4] = 0;
            vv[5] = 0;
            IIC_WriteData(0x70, vv, 6);

            return 0;
        }


        public void EnableAnalogReadoutMonitor(bool enable)
        {
            NI_USB3_WriteReg((UInt32)(enable == true ? 1 : 0), DAQ_ANALOG_MONITOR, ref Handle);
        }

    }


}

