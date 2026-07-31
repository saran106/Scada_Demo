using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada_Demo.MQTT_Model
{
    public class AlarmDefinition
    {
        public int AlarmNo { get; set; }     // 1,2,3,4
        public int Bit { get; set; }         // 0-15
        public string Text { get; set; } = "";
    }
}
