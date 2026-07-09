using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada_Demo.MQTT_Model
{
    public class BatchSettingsModel
    {
        public MaterialInAir_Model batchSettings_MaterialInAir { get; set; } = new();
        public DischargeDelay_Model batchSettings_DischargeDelay { get; set; } = new();
        public Step_Time_Model batchSettings_StepTime { get; set; } = new();
        public Tolerance_Model batchSettings_Tolerance { get; set; } = new();
        public GateSeq_Model batchSettings_GateSequence { get; set; } = new();
        public EmptyValue_Model batchSettings_EmptyValue { get; set; } = new();

        public JogTime_Model batchSettings_JogSettings { get; set; } = new();
        public Coarse_to_Fine_model batchSettings_CoarseFine { get; set; } = new();
        public MixerParameters_Model Setparameters_MixerParameters { get; set; } = new();

        public Recipe_Model batchSettings_Recipe { get; set; } = new();

        // Home Top
        public Home_Top Home_Top { get; set; } = new();
    }
}
