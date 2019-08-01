using System;
using System.Collections.Generic;
using System.Text;

namespace LGThingApi
{
    /// <summary>
    /// Implemented enums
    /// </summary>
    public static class Enums
    {
        /// <summary>
        /// What device is it?
        /// </summary>
        public enum DeviceType
        {
            REFRIGERATOR = 101,
            KIMCHI_REFRIGERATOR = 102,
            WATER_PURIFIER = 103,
            WASHER = 201,
            DRYER = 202,
            STYLER = 203,
            DISHWASHER = 204,
            OVEN = 301,
            MICROWAVE = 302,
            COOKTOP = 303,
            HOOD = 304,
            AC = 401,
            AIR_PURIFIER = 402,
            DEHUMIDIFIER = 403,
            ROBOT_KING = 501,
            ARCH = 1001,
            MISSG = 3001,
            SENSOR = 3002,
            SOLAR_SENSOR = 3102,
            IOT_LIGHTING = 3003,
            IOT_MOTION_SENSOR = 3004,
            IOT_SMART_PLUG = 3005,
            IOT_DUST_SENSOR = 3006,
            EMS_AIR_STATION = 4001,
            AIR_SENSOR = 4003
        }
        /// <summary>
        /// For washing machines only, state of washing machine
        /// </summary>
        public enum WasherState
        {
            ADD_DRAIN,
            COMPLETE,
            DETECTING,
            DETERGENT_AMOUNT,
            DRYING,
            END,
            ERROR_AUTO_OFF,
            FRESH_CARE,
            FROZEN_PREVENT_INITIAL,
            FROZEN_PREVENT_PAUSE,
            FROZEN_PREVENT_RUNNING,
            INITIAL,
            OFF,
            PAUSE,
            PRE_WASH,
            RESERVE,
            RINSING,
            RINSE_HOLD,
            RUNNING,
            SMART_DIAGNOSIS,
            SMART_DIAGNOSIS_DATA,
            SPINNING,
            TCL_ALARM_NORMAL,
            TUBCLEAN_COUNT_ALARM
        }
    }
}
