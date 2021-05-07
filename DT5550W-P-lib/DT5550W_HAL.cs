using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DT5550W_P_lib
{
    
    public class DT5550W_HAL
    {
        const UInt32 RFA_IIC_CONTROL = 0x80050008;
        const UInt32 RFA_IIC_STATUS = 0x80050009;
        public string SerialNumber;
        public bool isMaster = true;
        private PHY_LINK phy = new PHY_LINK();
        public DT5550W_PETIROC PetirocClass;
        public DT5550W_CITIROC CitirocClass;
        private t_AsicModels ConnectedAsic;
        
        public  DT5550W_HAL()
        {
            ConnectedAsic = t_AsicModels.INVALID; 
        }


        public List<String> GetDeviceList()
        {

            return phy.GetDeviceList();

        }
        public bool Connect(String DeviceSerialNumber)
        {
            String model = "AAAAAAAAAAAAAAAAAAA";
            int asic_count = 0;
            int SN = 0;

            SerialNumber = DeviceSerialNumber;
            int retcode = phy.NI_USB3_ConnectDevice_M(DeviceSerialNumber);

            if (retcode != 0)
                return false;

            phy.NI_USB3_SetIICControllerBaseAddress_M(
                RFA_IIC_CONTROL,
                RFA_IIC_STATUS);

            if (GetDGCardModel(ref model, ref asic_count, ref SN) != true)
            {
                return false;
            }

            if (model == "PET")
            {
                ConnectedAsic = t_AsicModels.PETIROC;
                PetirocClass = new DT5550W_PETIROC(ref phy);
                PetirocClass.SetManualAsicInfo("PET", asic_count);
                PetirocClass.PowerASIC(false);
                PetirocClass.PowerASIC(true);
            }
            else
            {
                if (model == "CIT")
                {
                    ConnectedAsic = t_AsicModels.CITIROC;
                    CitirocClass = new DT5550W_CITIROC(ref phy);
                    CitirocClass.SetManualAsicInfo("CIT", asic_count);
                }
                else
                {
                    return false;
                }

            }
      
            if (retcode == 0)
                return true;
            else
                return false;

        }


        public bool Connect(String DeviceSerialNumber, t_AsicModels ASICMODEL, int AsicCount)
        {

            SerialNumber = DeviceSerialNumber;
            int retcode = phy.NI_USB3_ConnectDevice_M(DeviceSerialNumber);
            if (retcode == 0)
            {
                ConnectedAsic = ASICMODEL;
                if (ConnectedAsic == t_AsicModels.PETIROC)
                {
                    PetirocClass = new DT5550W_PETIROC(ref phy);
                    PetirocClass.SetManualAsicInfo("PET", AsicCount);
                    PetirocClass.PowerASIC(false);
                    PetirocClass.PowerASIC(true);

                }
                else
                if(ConnectedAsic == t_AsicModels.CITIROC)
                {
                    CitirocClass = new DT5550W_CITIROC(ref phy);
                    CitirocClass.SetManualAsicInfo("CIT", AsicCount);
                }

                phy.NI_USB3_SetIICControllerBaseAddress_M(
                      RFA_IIC_CONTROL,
                      RFA_IIC_STATUS);
                return true;
            }
            else
                return false;

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


        public t_BoardInfo GetBoardInfo()
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
            {
                t_BoardInfo BoardInfo = PetirocClass.GetBoardInfo();
                BoardInfo.SerialNumber = SerialNumber;
                return BoardInfo;
            }
            else
            if (ConnectedAsic == t_AsicModels.CITIROC)
            {
                t_BoardInfo BoardInfo = CitirocClass.GetBoardInfo();
                BoardInfo.SerialNumber = SerialNumber;
                return BoardInfo;
            }
            else
            {
                t_BoardInfo BoardInfo = new t_BoardInfo();
                BoardInfo.AsicType = t_BoardInfo.t_AsicType.INVALID;
                return BoardInfo;
            }
        }

        public int SetHV(bool Enable, float voltage, float compliance)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                return PetirocClass.SetHV(Enable, voltage, compliance);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                return CitirocClass.SetHV(Enable, voltage, compliance);
            return -1;
        }

        public int SetHVTempFB(bool Enable, float voltage, float compliance, float tempCof, float temp)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                return PetirocClass.SetHVTempFB(Enable, voltage, compliance, tempCof, temp);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                return CitirocClass.SetHVTempFB(Enable, voltage, compliance, tempCof, temp);
            return -1;
        }

        public int GetHV(ref bool Enable, ref float voltage, ref float current)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                return PetirocClass.GetHV(ref Enable, ref voltage, ref current);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                return CitirocClass.GetHV(ref Enable, ref voltage, ref current);
            return -1;
        }

        public int GetSensTemperature(int sensId, ref float temperature)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                return PetirocClass.GetSensTemperature(sensId, ref temperature);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                return CitirocClass.GetSensTemperature(sensId, ref temperature);
            return -1;
        }

        public void EnableAnalogReadoutMonitor(bool enable)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                PetirocClass.EnableAnalogReadoutMonitor(enable);

        }

        public bool GetCountRate(ref UInt32[] cps)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                return PetirocClass.GetCountRate(ref cps);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                return CitirocClass.GetCountRate(ref cps);
            return false;
        }

        public void SelectTriggerMode(bool charge_trigger)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                PetirocClass.SelectTriggerMode(charge_trigger);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                ;
            
        }

        public void EnableTriggerFrame(bool enable)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                PetirocClass.EnableTriggerFrame(enable);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                ;

        }

        public void EnableExternalVeto(bool enable)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                PetirocClass.EnableExternalVeto(enable);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                CitirocClass.EnableExternalVeto(enable);
        }

        public void EnableExternalTrigger(bool enable)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                PetirocClass.EnableExternalTrigger(enable);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                ;
        }

        public void EnableResetTDCOnT0(bool enable)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                PetirocClass.EnableResetTDCOnT0(enable);
            
        }

        public void GetBuild(ref UInt32 build)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                PetirocClass.GetBuild(ref build);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                CitirocClass.GetBuild(ref build);
        }

        public void SetASICVeto(bool A1, bool A2, bool A3, bool A4)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                PetirocClass.SetASICVeto(A1, A2, A3, A4);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                CitirocClass.SetASICVeto(A1, A2, A3, A4);
        }
        public void ConfigureT0(T0Mode mode, int T0sw_freq)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                PetirocClass.ConfigureT0(mode, T0sw_freq);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                CitirocClass.ConfigureT0(mode, T0sw_freq);
        }
        public void PulseT0()
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                PetirocClass.PulseT0();
            if (ConnectedAsic == t_AsicModels.CITIROC)
                CitirocClass.PulseT0();
        }
        public bool GetMonitorPetiroc(ref DT5550W_PETIROC.PetirocMonitorData PMD, uint samples)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                return PetirocClass.GetMonitor(ref PMD, samples);

            return false;
        }

        public bool GetMonitorCitiroc(ref CitirocMonitorData PMD)
        {
            if (ConnectedAsic == t_AsicModels.CITIROC)
                return CitirocClass.GetMonitor(ref PMD);

            return false;
        }


        public bool GetDefaultChannelMapping(ref tChannelMapping[] chmap)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                return PetirocClass.GetDefaultChannelMapping(ref chmap);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                return CitirocClass.GetDefaultChannelMapping(ref chmap);

            return false;
        }

        public int GetRawBuffer(UInt32[] data, UInt32 event_count, UInt32 timeout, UInt32 PacketSize, ref UInt32 valid_word)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                return PetirocClass.GetRawBuffer(data, event_count, timeout, PacketSize, ref valid_word);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                return CitirocClass.GetRawBuffer(data, event_count, timeout, PacketSize, ref valid_word);

            return -1;
        }

        public int DecodePetirocRowEvents(ref UInt32[] bufferA, UInt32 valid_wordA, ref Queue<DT5550W_PETIROC.t_DataPETIROC> pC, int ThresholdSoftware, int Polarity)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                return PetirocClass.DecodePetirocRowEvents(ref bufferA, valid_wordA, ref pC, ThresholdSoftware, Polarity);

            return -1;
        }

        public int DecodeCitirocRowEvents(ref Queue<t_DataCITIROC> pC, int ThresholdSoftware)
        {
            if (ConnectedAsic == t_AsicModels.CITIROC)
                return CitirocClass.DecodeCitirocRowEvents(ref pC, ThresholdSoftware);

            return -1;
        }


        public int DecodeCitirocRowEventsPC(ref Queue<t_DataCITIROCPC> pC)
        {
            if (ConnectedAsic == t_AsicModels.CITIROC)
                return CitirocClass.DecodeCitirocRowEventsPC(ref pC);

            return -1;

        }

        public bool  CitirocPushRawDataInBuffer(ref UInt32[] bufferA, UInt32 valid_wordA)
        {
            if (ConnectedAsic == t_AsicModels.CITIROC)
                return CitirocClass.PushRawDataInBuffer(ref bufferA, valid_wordA);

            return false;
        }


        public bool PetirocAcquisitionDelay(uint value)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                return PetirocClass.PetirocAcquisitionDelay(value);

            return false;
        }

        public bool ConfigureAsic(bool progA, bool progB, bool progC, bool progD, UInt32[] cfg)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                return PetirocClass.ConfigPetiroc(progA, progB, progC, progD,  cfg);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                return CitirocClass.ConfigCitiroc(progA, progB, progC, progD, cfg);

            return false;
        }

        public bool ConfigureMonitor(bool progA, bool progB, bool progC, bool progD, UInt32[] cfg)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                return PetirocClass.ConfigureMonitorPetiroc(progA, progB, progC, progD, cfg);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                return CitirocClass.ConfigureMonitorCitiroc(progA, progB, progC, progD, cfg);

            return false;
        }
        public void ConfigureSignalGenerator(bool enableA, bool enableB, bool enableC, bool enableD, int frequency)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                PetirocClass.ConfigureSignalGenerator(enableA, enableB, enableC, enableD, frequency);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                CitirocClass.ConfigureSignalGenerator(enableA, enableB, enableC, enableD, frequency);

           
        }

        public bool SetDigitalOutputSignalLength(double ns)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                return PetirocClass.SetDigitalOutputSignalLength(ns);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                return CitirocClass.SetDigitalOutputSignalLength(ns);

            return false;
        }

        public void ExtVetoEnable(bool enable)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                PetirocClass.ExtVetoEnable(enable);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                CitirocClass.ExtVetoEnable(enable);

        }

        public void VetoSw(bool veto)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                PetirocClass.VetoSw(veto);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                ;
        }

        public bool PetirocResetB(bool value)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                return PetirocClass.PetirocResetB(value);

            return false;
        }

        public bool PetirocADCStartExternal(bool value)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                return PetirocClass.PetirocADCStartExternal(value);

            return false;
        }

        public bool PetirocValEvent(bool value)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                return PetirocClass.PetirocValEvent(value);

            return false;
        }

        public bool PetirocRaZForce(bool value)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                return PetirocClass.PetirocRaZForce(value);

            return false;
        }

        public void FlushFIFO()
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                PetirocClass.FlushFIFO();
            if (ConnectedAsic == t_AsicModels.CITIROC)
                CitirocClass.FlushFIFO();        
        }

        public void RUNControl(bool run)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                PetirocClass.RUNControl(run);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                ;
        }

        public void ExtRunEnable(bool run_ext)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                PetirocClass.ExtRunEnable(run_ext);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                ;
        }


        public void ConfigureExtHold(float delay, bool ext_hold_enable)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                PetirocClass.ConfigureExtHold(delay, ext_hold_enable);
            if (ConnectedAsic == t_AsicModels.CITIROC)
                ;
        }


        public void CITIROC_SetTriggerMode(TriggerMode TM)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                ;
            if (ConnectedAsic == t_AsicModels.CITIROC)
                CitirocClass.SelectTriggerMode(TM);
        }

        public void CITIROC_SetHoldDelay(double hd)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                ;
            if (ConnectedAsic == t_AsicModels.CITIROC)
                CitirocClass.SetHoldWindowsWidth(hd);
        }



        public void ConfigurePC(PCMode mode, int PeriodicWidth, int PeriodicStart, int windows_count)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                ;
            if (ConnectedAsic == t_AsicModels.CITIROC)
                CitirocClass.ConfigurePC(mode, PeriodicWidth, PeriodicStart, windows_count);
        }

        public void StartPC()
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                ;
            if (ConnectedAsic == t_AsicModels.CITIROC)
                CitirocClass.StartPC();
        }

        public int GetRawBufferPC(UInt32[] data, UInt32 event_count, UInt32 timeout, UInt32 PacketSize, ref UInt32 valid_word)
        {
            if (ConnectedAsic == t_AsicModels.PETIROC)
                ;
            if (ConnectedAsic == t_AsicModels.CITIROC)
                return CitirocClass.GetRawBufferPC(data, event_count, timeout, PacketSize, ref valid_word);

            return -1;
        }
    }
}
