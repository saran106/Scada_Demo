using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada_Demo.MQTT_Model
{
    public class BatchSettingsModel
    {
        public MaterialInAir_Model MaterialInAir { get; set; } = new();
        public DischargeDelay_Model DischargeDelay { get; set; } = new();
        public Step_Time_Model StepTime { get; set; } = new();
        public Tolerance_Model Tolerance { get; set; } = new();
        public GateSeq_Model GateSequence { get; set; } = new();
        public EmptyValue_Model EmptyValue { get; set; } = new();

        public JogTime_Model JogSettings { get; set; } = new();
        public Coarse_to_Fine CoarseFine { get; set; } = new();
        public MixerParameters_Model MixerParameters { get; set; } = new();
    }
}
