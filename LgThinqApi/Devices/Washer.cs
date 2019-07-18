using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using  static LGThingApi.Extensions;
using  LGThingApi.Structures;

namespace LGThingApi.Devices
{
    public class Washer : SmartDevice
    {
        public Washer(Device fromDevice) : base (fromDevice)
        {
            if (fromDevice.DeviceType != Enums.DeviceType.WASHER)
                throw new ArgumentException("Not washer!");
            State = GetOptions(ModelInfo.Value.State.Option);
            SpinSpeed = GetOptions(ModelInfo.Value.SpinSpeed.Option);
            Wash = GetOptions(ModelInfo.Value.Wash.Option);
            WaterTemp = GetOptions(ModelInfo.Value.WaterTemp.Option);
            RinseOption = GetOptions(ModelInfo.Value.RinseOption.Option);
            DryLevel = GetOptions(ModelInfo.Value.DryLevel.Option);
            LoadItem = GetOptions(ModelInfo.Value.LoadItem.Option);
            Standby = GetOptions(ModelInfo.Value.Standby.Option);
            MedicRinse = GetOptions(ModelInfo.Value.MedicRinse.Option);
            PreWash = GetOptions(ModelInfo.Value.PreWash.Option);
            Steam = GetOptions(ModelInfo.Value.Steam.Option);
            TurboWash = GetOptions(ModelInfo.Value.TurboWash.Option);
            ChildLock = GetOptions(ModelInfo.Value.ChildLock.Option);
            CreaseCare = GetOptions(ModelInfo.Value.CreaseCare.Option);
        }
        List<string> GetOptions(Dictionary<string,string> input)
        {
            List<string> output = new List<string>();
            foreach (var item in input.Values)
            {
                if (ProductTranslate.Pack.ContainsKey(item))
                    output.Add(ProductTranslate.Pack[item]);
                else if (DeviceCapabilities.Pack.ContainsKey(item))
                    output.Add(DeviceCapabilities.Pack[item]);
                else if (Communication.LGGateway.LgedmRoot.CommonTranslate.Pack.ContainsKey(item))
                    output.Add(Communication.LGGateway.LgedmRoot.CommonTranslate.Pack[item]);
                else if (Communication.LGGateway.LgedmRoot.InitTranslate.Pack.ContainsKey(item))
                    output.Add(Communication.LGGateway.LgedmRoot.InitTranslate.Pack[item]);
                else
                {
                    Console.WriteLine("Unknown item " + item);
                    output.Add(item);
                }
            }
            return output;
        }
        public override void PollMonitor(object sender, byte[] polledData)
        {
            byte hour = 0;
            foreach (var item in ModelInfo.Monitoring.Protocol)
            {
                //polledData[item.StartByte];
                var bytes = polledData[item.StartByte];
                Console.Write(item.Comment + ": ");
                switch (item.Value)
                {
                    case "Remain_Time_H":
                        hour = bytes;
                        break;
                    case "Remain_Time_M":
                        RemainingTime = new TimeSpan(hour, bytes, 0);
                        Console.WriteLine(RemainingTime);
                        break;
                    case "Initial_Time_H":
                        hour = bytes;
                        break;
                    case "Initial_Time_M":
                        InitialTime = new TimeSpan(hour, bytes, 0);
                        Console.WriteLine(InitialTime);

                        break;
                    case "Reserve_Time_H":
                        hour = bytes;
                        break;
                    case "Reserve_Time_M":
                        ReserveTime = new TimeSpan(hour, bytes, 0);
                        Console.WriteLine(ReserveTime);
                        break;
                    case "State":
                        StateID = bytes;
                        Console.WriteLine(State[StateID]);
                        break;
                    case "SpinSpeed":
                        SpinSpeedID = bytes;
                        Console.WriteLine(SpinSpeed[SpinSpeedID]);
                        break;
                    case "Wash":
                        WashID = bytes;
                        Console.WriteLine(Wash[WashID]);
                        break;
                    case "WaterTemp":
                        WaterTempID = bytes;
                        Console.WriteLine(WaterTemp[WaterTempID]);
                        break;
                    case "RinseOption":
                        RinseOptionID = bytes;
                        Console.WriteLine(RinseOption[RinseOptionID]);
                        break;
                    case "DryLevel":
                        DryLevelID = bytes;
                        Console.WriteLine(DryLevel[DryLevelID]);
                        break;
                    case "Option1":
                        Option1 = new BitArray(bytes);
                        Console.WriteLine();
                        break;
                    case "Option2":
                        Option2 = new BitArray(bytes);
                        Console.WriteLine();
                        break;
                    case "PreState":
                        PreStateID = bytes;
                        Console.WriteLine(State[PreStateID]);
                        break;
                    case "TCLCount":
                        TCLCount = bytes;
                        Console.WriteLine(TCLCount);
                        break;
                    case "LoadItem":
                        LoadItemID = bytes;
                        Console.WriteLine(LoadItem[LoadItemID]);
                        break;
                    case "Standby":
                        StandbyID = bytes;
                        Console.WriteLine(Standby[StandbyID]);
                        break;
                    default:
                        Console.WriteLine("Unknown value " + item.Value);
                        break;
                }
            }
        }
        public int StateID { get; set; }
        public int PreStateID { get; set; }

        public List<string> State { get; set; }

        public int RemoteStartID { get; set; }
        public List<string> RemoteStart { get; set; }

        public bool InitialBit { get; set; }

        public int ChildLockID { get; set; }
        public List<string> ChildLock { get; set; }

        public int TCLCount { get; set; }

        public TimeSpan ReserveTime { get; set; }
        public TimeSpan RemainingTime { get; set; }
        public TimeSpan InitialTime { get; set; }

        public int WashID { get; set; }
        public List<string> Wash { get; set; }

        public int SpinSpeedID { get; set; }
        public List<string> SpinSpeed { get; set; }

        public int WaterTempID { get; set; }
        public List<string> WaterTemp { get; set; }

        public int RinseOptionID { get; set; }
        public List<string> RinseOption { get; set; }

        public int DryLevelID { get; set; }
        public List<string> DryLevel { get; set; }

        public int TurboWashID { get; set; }
        public List<string> TurboWash { get; set; }

        public int SteamID { get; set; }
        public List<string> Steam { get; set; }

        public int PreWashID { get; set; }
        public List<string> PreWash { get; set; }

        public int MedicRinseID { get; set; }
        public List<string> MedicRinse { get; set; }

        public int SteamSoftenerID { get; set; }
        public List<string> SteamSoftener { get; set; }

        public int LoadItemID { get; set; }
        public List<string> LoadItem { get; set; }

        public int StandbyID { get; set; }
        public List<string> Standby { get; set; }

        public int CreaseCareID { get; set; }
        public List<string> CreaseCare { get; set; }

        private BitArray Option1 { get; set; } = new BitArray((byte)0);

        public bool TurboWashStatus { get => Option1[0]; set => Option1[0] = value; }
        public bool CreaseCareStatus { get => Option1[1]; set => Option1[1] = value; }
        public bool SteamSoftenerStatus { get => Option1[2]; set => Option1[2] = value; }
        public bool EcoHybridStatus { get => Option1[3]; set => Option1[3] = value; }
        public bool MedicRinseStatus { get => Option1[4]; set => Option1[4] = value; }
        public bool RinseSpinStatus { get => Option1[5]; set => Option1[5] = value; }
        public bool PreWashStatus { get => Option1[6]; set => Option1[6] = value; }
        public bool SteamStatus { get => Option1[7]; set => Option1[7] = value; }

        private BitArray Option2 { get; set; } = new BitArray((byte)0);
        public bool InitialBitStatus { get => Option2[0]; set => Option2[0] = value; }
        public bool RemoteStartStatus { get => Option2[1]; set => Option2[1] = value; }

        // 2 - 5 IS MISSING

        public bool DoorLockStatus { get => Option2[6]; set => Option2[6] = value; }
        public bool ChildLockStatus { get => Option2[7]; set => Option2[7] = value; }


    }
}
