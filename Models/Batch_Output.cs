using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada_Demo.Models
{
    public class Batch_Output
    {
        public string BatchMode { get; set; }

        public int GateFullOpen { get; set; }

        public int GateHalfOpen { get; set; }
    }
}
