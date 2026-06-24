namespace Scada_Demo.MQTT_Model
{
    public class JogTime_Model
    {
        // Jog ON
        public short Agg1 { get; set; }
        public short Agg2 { get; set; }
        public short Agg3 { get; set; }
        public short Agg4 { get; set; }

        public short Cem { get; set; }

        public int Water { get; set; }
        public int Admix { get; set; }
        public int Ice { get; set; }

        // Jog OFF
        public short Agg1Off { get; set; }
        public short Agg2Off { get; set; }
        public short Agg3Off { get; set; }
        public short Agg4Off { get; set; }
    }
}