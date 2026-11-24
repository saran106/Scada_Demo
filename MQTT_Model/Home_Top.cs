using System;

namespace Scada_Demo.MQTT_Model
{
    public class HomeTopItem
    {
        public short counter { get; set; }

        public short set_wt { get; set; }

        public short act_wt { get; set; }

        public short weighervalue { get; set; }
    }

    public class Home_Top
    {
        public HomeTopItem Home_Top_aggregate { get; set; } = new();

        public HomeTopItem Home_Top_cement { get; set; } = new();

        public HomeTopItem Home_Top_water { get; set; } = new();

        public HomeTopItem Home_Top_admix { get; set; } = new();

        public HomeTopItem Home_Top_ice { get; set; } = new();
        public int BatchRemaining { get; set; } = new();
        public int WaterCorr { get; set; } = new();
        public int Mositure1 { get; set; } = new();
        public short MixerCapacity { get; set; }

        // PLC material completion status
        public bool Agg1Completed { get; set; }
        public bool CementCompleted { get; set; }
        public bool Admix1Completed { get; set; }
        public float MixerTime { get; set; }



    }
}