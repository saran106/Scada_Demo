using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada_Demo.MQTT_Model
{
   
        public class MixerParameters_Model
        {
            public short BatchSize { get; set; }

            public int DiscTime { get; set; }

            public int MidPosTime { get; set; }

            public int GateCloseMid { get; set; }
        }
    
}
