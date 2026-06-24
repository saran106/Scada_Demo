using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada_Demo.MQTT_Model
{
    public class DischargeDelay_Model
    {
        public int Cement { get; set; }
        public int Water { get; set; }
        public int Admix { get; set; }
        public int Skip { get; set; }
        public short PumpCutOff { get; set; }
    }
}
