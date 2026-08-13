using System;

namespace Scada_Demo.Models
{
    public class ActiveAlarm
    {
        public int AlarmId { get; set; }

        public int AlarmNo { get; set; }

        public int Bit { get; set; }

        public int RecordId { get; set; }

        public string Description { get; set; } = "";

        public int BatchNo { get; set; }

        public DateTime StartTime { get; set; }

        public bool IsAcknowledged { get; set; }
    }
}