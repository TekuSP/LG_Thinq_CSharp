using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static LGThingApi.Enums;
using LGThingApi.Extensions;

namespace LGThingApi.Structures
{
    public class Device
    {
        public Device(Device anotherDevice)
        {
            var properties = typeof(Device).GetProperties();
            foreach (var item in properties)
            {
                var value = item.GetValue(anotherDevice);
                if (value != null)
                    item.SetValue(this, value);
            }
        }
        public Device()
        {

        }
        [JsonProperty("modelNm")]
        public string ModelNm { get; set; }

        [JsonProperty("subModelNm")]
        public string SubModelNm { get; set; }

        [JsonProperty("deviceType")]
        public DeviceType DeviceType { get; set; }

        [JsonProperty("deviceCode")]
        public string DeviceCode { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("workId")]
        public string WorkID { get; set; }
        [JsonProperty("deviceId")]
        public string DeviceID { get; set; }
        [JsonProperty("fwVer")]
        public string FwVer { get; set; }

        [JsonProperty("modelJsonVer")]
        public long ModelJsonVer { get; set; }

        [JsonProperty("modelJsonUrl")]
        public Uri ModelJsonUrl { get; set; }

        [JsonProperty("imageUrl")]
        public Uri ImageUrl { get; set; }

        [JsonProperty("smallImageUrl")]
        public Uri SmallImageUrl { get; set; }

        [JsonProperty("deviceState")]
        public string DeviceState { get; set; }

        [JsonProperty("regDt")]
        public long RegDt { get; set; }

        [JsonProperty("ssid")]
        public string Ssid { get; set; }

        [JsonProperty("ssidPass")]
        public string SsidPass { get; set; }

        [JsonProperty("remoteControlType")]
        public string RemoteControlType { get; set; }

        [JsonProperty("macAddress")]
        public string MacAddress { get; set; }

        [JsonProperty("networkType")]
        public string NetworkType { get; set; }

        [JsonProperty("timezoneCode")]
        public string TimezoneCode { get; set; }

        [JsonProperty("timezoneCodeAlias")]
        public string TimezoneCodeAlias { get; set; }

        [JsonProperty("utcOffset")]
        public long UtcOffset { get; set; }

        [JsonProperty("utcOffsetDisplay")]
        public string UtcOffsetDisplay { get; set; }

        [JsonProperty("dstOffset")]
        public long DstOffset { get; set; }

        [JsonProperty("dstOffsetDisplay")]
        public string DstOffsetDisplay { get; set; }

        [JsonProperty("curOffset")]
        public long CurOffset { get; set; }

        [JsonProperty("curOffsetDisplay")]
        public string CurOffsetDisplay { get; set; }

        [JsonProperty("langPackProductTypeVer")]
        public double LangPackProductTypeVer { get; set; }

        [JsonProperty("langPackProductTypeUri")]
        public Uri LangPackProductTypeUri { get; set; }

        [JsonProperty("langPackModelVer")]
        public long LangPackModelVer { get; set; }

        [JsonProperty("langPackModelUri")]
        public Uri LangPackModelUri { get; set; }

        [JsonProperty("sdsGuide")]
        public string SdsGuide { get; set; }

        [JsonProperty("appModuleVer")]
        public double AppModuleVer { get; set; }

        [JsonProperty("appModuleUri")]
        public Uri AppModuleUri { get; set; }

        [JsonProperty("appModuleSize")]
        public long AppModuleSize { get; set; }

        [JsonProperty("appRestartYn")]
        public string AppRestartYn { get; set; }

        [JsonProperty("newRegYn")]
        public string NewRegYn { get; set; }
        [JsonIgnore]
        public Translate DeviceCapabilities { get; set; }
        [JsonIgnore]
        public Translate ProductTranslate { get; set; }
        [JsonIgnore]
        public ModelInfo ModelInfo { get; set; }
    }
    public class Translate
    {
        [JsonProperty("subtype")]
        public string Subtype { get; set; }

        [JsonProperty("pack")]
        public Dictionary<string, string> Pack { get; set; }

        [JsonProperty("ver")]
        public string Ver { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }
    }
    public class ModelInfo
    {
        [JsonProperty("Info")]
        public Info Info { get; set; }

        [JsonProperty("Module")]
        public Module Module { get; set; }

        [JsonProperty("Config")]
        public Config Config { get; set; }

        [JsonProperty("Value")]
        public Value Value { get; set; }

        [JsonProperty("Error")]
        public Dictionary<string, Error> Error { get; set; }

        [JsonProperty("Monitoring")]
        public Monitoring Monitoring { get; set; }

        [JsonProperty("Push")]
        public Push[] Push { get; set; }

        [JsonProperty("EnergyMonitoring")]
        public EnergyMonitoring EnergyMonitoring { get; set; }

        [JsonProperty("SmartMode")]
        public SmartMode SmartMode { get; set; }

        [JsonProperty("ControlWifi")]
        public ControlWifi ControlWifi { get; set; }

        [JsonProperty("Course")]
        public Dictionary<string, CourseValue> Course { get; set; }

        [JsonProperty("SmartCourse")]
        public Dictionary<string, CourseValue> SmartCourse { get; set; }
    }

    public class Config
    {
        [JsonProperty("downloadPanelLabel")]
        public string DownloadPanelLabel { get; set; }

        [JsonProperty("remoteStartLabel")]
        public string RemoteStartLabel { get; set; }

        [JsonProperty("maxDownloadCourseNum")]
        public long MaxDownloadCourseNum { get; set; }

        [JsonProperty("defaultCourseId")]
        public long DefaultCourseId { get; set; }

        [JsonProperty("downloadCourseAPId")]
        public long DownloadCourseApId { get; set; }

        [JsonProperty("defaultSmartCourseId")]
        public long DefaultSmartCourseId { get; set; }

        [JsonProperty("tubCleanCourseId")]
        public long TubCleanCourseId { get; set; }

        [JsonProperty("standbyEnable")]
        public bool StandbyEnable { get; set; }

        [JsonProperty("fota")]
        public bool Fota { get; set; }

        [JsonProperty("expectedStartTime")]
        public bool ExpectedStartTime { get; set; }

        [JsonProperty("SmartCourseCategory")]
        public SmartCourseCategory[] SmartCourseCategory { get; set; }
    }

    public class SmartCourseCategory
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("courseIdList")]
        public long[] CourseIdList { get; set; }
    }

    public class ControlWifi
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("action")]
        public Action Action { get; set; }
    }

    public class Action
    {
        [JsonProperty("CourseDownload")]
        public CourseDownload CourseDownload { get; set; }

        [JsonProperty("PowerOff")]
        public OperationStop PowerOff { get; set; }

        [JsonProperty("OperationStart")]
        public Start OperationStart { get; set; }

        [JsonProperty("OperationStop")]
        public OperationStop OperationStop { get; set; }

        [JsonProperty("OperationWakeUp")]
        public OperationStop OperationWakeUp { get; set; }

        [JsonProperty("MODE020Start")]
        public Start Mode020Start { get; set; }
    }

    public class CourseDownload
    {
        [JsonProperty("tag")]
        public string[] Tag { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }
    }

    public class Start
    {
        [JsonProperty("cmd")]
        public string Cmd { get; set; }

        [JsonProperty("cmdOpt")]
        public string CmdOpt { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("encode")]
        public bool Encode { get; set; }
    }

    public class OperationStop
    {
        [JsonProperty("cmd")]
        public string Cmd { get; set; }

        [JsonProperty("cmdOpt")]
        public string CmdOpt { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class CourseValue
    {
        [JsonProperty("_comment")]
        public string Comment { get; set; }

        [JsonProperty("courseType")]
        public string CourseType { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("script")]
        public string Script { get; set; }

        [JsonProperty("controlEnable")]
        public bool ControlEnable { get; set; }

        [JsonProperty("imgIndex")]
        public long ImgIndex { get; set; }

        [JsonProperty("function")]
        public Function[] Function { get; set; }

        [JsonProperty("Course", NullValueHandling = NullValueHandling.Ignore)]
        public long? Course { get; set; }

        [JsonProperty("downloadEnable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? DownloadEnable { get; set; }
    }

    public class Function
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("default")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Default { get; set; }

        [JsonProperty("visibility", NullValueHandling = NullValueHandling.Ignore)]
        public string Visibility { get; set; }

        [JsonProperty("showing", NullValueHandling = NullValueHandling.Ignore)]
        public string Showing { get; set; }
    }

    public class EnergyMonitoring
    {
        [JsonProperty("option")]
        public string[] Option { get; set; }

        [JsonProperty("powertable")]
        public Dictionary<string, long> Powertable { get; set; }
    }

    public class Error
    {
        [JsonProperty("_comment")]
        public string Comment { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }

    public class Info
    {
        [JsonProperty("productType")]
        public string ProductType { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("modelType")]
        public string ModelType { get; set; }

        [JsonProperty("MP Project")]
        public string MpProject { get; set; }

        [JsonProperty("ProjectName")]
        public string ProjectName { get; set; }

        [JsonProperty("modelName")]
        public string ModelName { get; set; }

        [JsonProperty("networkType")]
        public string NetworkType { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }

    public class Module
    {
        [JsonProperty("WPM")]
        public Wpm Wpm { get; set; }

        [JsonProperty("Menu")]
        public string[] Menu { get; set; }
    }

    public class Wpm
    {
        [JsonProperty("GWM_CEN01_Main")]
        public string GwmCen01Main { get; set; }

        [JsonProperty("GWM_CRS01_Main")]
        public string GwmCrs01Main { get; set; }

        [JsonProperty("GWM_CRS02_CourseList")]
        public string GwmCrs02CourseList { get; set; }

        [JsonProperty("GWM_CRS03_CourseDetail")]
        public string GwmCrs03CourseDetail { get; set; }

        [JsonProperty("GWM_WCH01_Main")]
        public string GwmWch01Main { get; set; }

        [JsonProperty("GWM_WCH01_UserGuide2")]
        public string GwmWch01UserGuide2 { get; set; }

        [JsonProperty("GWM_ENM01_Main")]
        public string GwmEnm01Main { get; set; }

        [JsonProperty("GCM_SDS01_SdsMain")]
        public string GcmSds01SdsMain { get; set; }

        [JsonProperty("GWM_SET01_Main")]
        public string GwmSet01Main { get; set; }

        [JsonProperty("GWM_SET02_PushList")]
        public string GwmSet02PushList { get; set; }

        [JsonProperty("GWM_SET03_NickName")]
        public string GwmSet03NickName { get; set; }
    }

    public class Monitoring
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("protocol")]
        public Protocol[] Protocol { get; set; }
    }

    public class Protocol
    {
        [JsonProperty("_comment")]
        public string Comment { get; set; }

        [JsonProperty("startByte")]
        public long StartByte { get; set; }

        [JsonProperty("length")]
        public long Length { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        public override string ToString()
        {
            return Comment;
        }
    }

    public class Push
    {
        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("groupCode")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long GroupCode { get; set; }

        [JsonProperty("pushList")]
        public Dictionary<string, string>[] PushList { get; set; }
    }

    public class SmartMode
    {
        [JsonProperty("MODE020")]
        public Mode020 Mode020 { get; set; }
    }

    public class Mode020
    {
        [JsonProperty("_comment")]
        public string Comment { get; set; }

        [JsonProperty("modeCase")]
        public long ModeCase { get; set; }

        [JsonProperty("actionName")]
        public string ActionName { get; set; }

        [JsonProperty("control")]
        public Control[] Control { get; set; }
    }

    public class Control
    {
        [JsonProperty("command")]
        public string Command { get; set; }
    }

    public class Value
    {
        [JsonProperty("State")]
        public State State { get; set; }

        [JsonProperty("PreState")]
        public State PreState { get; set; }

        [JsonProperty("RemoteStart")]
        public OptionProperty RemoteStart { get; set; }

        [JsonProperty("InitialBit")]
        public InitialBit InitialBit { get; set; }

        [JsonProperty("ChildLock")]
        public OptionProperty ChildLock { get; set; }

        [JsonProperty("TCLCount")]
        public InitialTimeH TclCount { get; set; }

        [JsonProperty("Reserve_Time_H")]
        public InitialTimeH ReserveTimeH { get; set; }

        [JsonProperty("Reserve_Time_M")]
        public InitialTimeH ReserveTimeM { get; set; }

        [JsonProperty("Remain_Time_H")]
        public InitialTimeH RemainTimeH { get; set; }

        [JsonProperty("Remain_Time_M")]
        public InitialTimeH RemainTimeM { get; set; }

        [JsonProperty("Initial_Time_H")]
        public InitialTimeH InitialTimeH { get; set; }

        [JsonProperty("Initial_Time_M")]
        public InitialTimeH InitialTimeM { get; set; }

        [JsonProperty("Wash")]
        public OptionProperty Wash { get; set; }

        [JsonProperty("SpinSpeed")]
        public OptionProperty SpinSpeed { get; set; }

        [JsonProperty("WaterTemp")]
        public OptionProperty WaterTemp { get; set; }

        [JsonProperty("RinseOption")]
        public OptionProperty RinseOption { get; set; }

        [JsonProperty("DryLevel")]
        public OptionProperty DryLevel { get; set; }

        [JsonProperty("TurboWash")]
        public OptionProperty TurboWash { get; set; }

        [JsonProperty("Steam")]
        public OptionProperty Steam { get; set; }

        [JsonProperty("PreWash")]
        public OptionProperty PreWash { get; set; }

        [JsonProperty("MedicRinse")]
        public OptionProperty MedicRinse { get; set; }

        [JsonProperty("SteamSoftener")]
        public OptionProperty SteamSoftener { get; set; }

        [JsonProperty("LoadItem")]
        public OptionProperty LoadItem { get; set; }

        [JsonProperty("Standby")]
        public OptionProperty Standby { get; set; }

        [JsonProperty("CreaseCare")]
        public OptionProperty CreaseCare { get; set; }

        [JsonProperty("Option1")]
        public Option1Class Option1 { get; set; }

        [JsonProperty("Option2")]
        public Option1Class Option2 { get; set; }

        [JsonProperty("Course")]
        public ErrorClass Course { get; set; }

        [JsonProperty("Error")]
        public ErrorClass Error { get; set; }

        [JsonProperty("SmartCourse")]
        public ErrorClass SmartCourse { get; set; }
    }

    public class OptionProperty
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("default")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Default { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("option")]
        public Dictionary<string, string> Option { get; set; }
    }

    public class ErrorClass
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("option")]
        public string[] Option { get; set; }
    }

    public class InitialBit
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("default")]
        public bool Default { get; set; }
    }

    public class InitialTimeH
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("default")]
        public long Default { get; set; }

        [JsonProperty("option")]
        public InitialTimeHOption Option { get; set; }

        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; set; }
    }

    public class InitialTimeHOption
    {
        [JsonProperty("min")]
        public long Min { get; set; }

        [JsonProperty("max")]
        public long Max { get; set; }
    }

    public class Option1Class
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("default")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Default { get; set; }

        [JsonProperty("option")]
        public OptionElement[] Option { get; set; }
    }

    public class OptionElement
    {
        [JsonProperty("startbit")]
        public long Startbit { get; set; }

        [JsonProperty("length")]
        public long Length { get; set; }

        [JsonProperty("default")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Default { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class State
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("default")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Default { get; set; }

        [JsonProperty("option")]
        public Dictionary<string, string> Option { get; set; }
    }


    public class WorkList
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("deviceState")]
        public string DeviceState { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("returnCode")]
        public string ReturnCode { get; set; }

        [JsonProperty("returnData")]
        public string ReturnData { get; set; }

        [JsonProperty("stateCode")]
        public string StateCode { get; set; }

        [JsonProperty("workId")]
        public string WorkId { get; set; }
    }
}
