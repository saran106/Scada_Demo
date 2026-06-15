namespace Scada_Demo.MQTT_Model
{
    public class JogTime_Model
    {
        // Jog ON
        public short Agg1On { get; set; }
        public short Agg2On { get; set; }
        public short Agg3On { get; set; }
        public short Agg4On { get; set; }

        public short Cem3On { get; set; }

        public int Adm1On { get; set; }
        public int IceOn { get; set; }
        public int WaterOn { get; set; }

        // Jog OFF
        public short Agg1Off { get; set; }
        public short Agg2Off { get; set; }
        public short Agg3Off { get; set; }
        public short Agg4Off { get; set; }
    }
}