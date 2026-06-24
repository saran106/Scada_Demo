using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada_Demo.MQTT_Model
{
    public class Tolerance_Model
    {
        // Aggregate
        public short Agg1 { get; set; }      // DB78.DBW118
        public short Agg2 { get; set; }      // DB78.DBW120
        public short Agg3 { get; set; }      // DB78.DBW122
        public short Agg4 { get; set; }      // DB78.DBW124

        // Cement
        public short Cem1 { get; set; }      // DB78.DBW126
        public short Cem4 { get; set; }      // DB184.DBW78

        // Water
        public short Water { get; set; }     // DB78.DBW376

        // Admixture
        public short Adm1 { get; set; }      // DB78.DBW128

        // Ice / Silica
        public short Ice { get; set; }       // DB78.DBW400
    }
}
