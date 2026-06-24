using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada_Demo.MQTT_Model
{
    public class MaterialInAir_Model
    {
        
            public short Agg92 { get; set; }
            public short Agg94 { get; set; }
            public short Agg96 { get; set; }
            public short Agg98 { get; set; }

            public short Water386 { get; set; }

            public short Admix108 { get; set; }
            public short Admix110 { get; set; }

            public short Ice410 { get; set; }
        
    }
}
