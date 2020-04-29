using System;
using System.Collections.Generic;

namespace TYFoverBox
{
    public class Global
    {
        public static List<Station> _Station
        {
            get;
            set;
        }
    }

    public class Station
    {
        public string TPSSN { get; set; }
        public string TPSDevice { get; set; }
        public string TPSDeviceAlias { get; set; }
        public string TPSTemperatureThreshold { get; set; }
        public string TPSDifferenceValue { get; set; }
        public string TPSDistanceMax { get; set; }
        public string TPSDistanceMin { get; set; }
        public string TPSDifferenceTimeEvent { get; set; }
        public string TPSDifferenceTimeTODB { get; set; }
        public string TPSTopValue { get; set; }
        public string TPSUrl { get; set; }
        public string TPSTransferUrl { get; set; }
        public string TPSVlcUrl { get; set; }
        public string TPSUUID { get; set; }
        public string TPSValid { get; set; }
        public string TPSNote { get; set; }
    }

    public class RealTimeData
    {
        public string TPDSN { get; set; }
        public string TPDDevice { get; set; }
        public string DeviceSN { get; set; }
        public string TPSDeviceAlias { get; set; }
        public string TPDStatus { get; set; }
        public string Status { get; set; }
        public string TPDDeviceTemperature { get; set; }
        public string TPDForeheadTemperature { get; set; }
        public DateTime Updatetime { get; set; }
        public string ThresholdFever { get; set; }
        public string TPSDifferenceValue { get; set; }
        public string DataTime { get; set; }
        public string TPDAlertDistanceStatus { get; set; }
    }
}
