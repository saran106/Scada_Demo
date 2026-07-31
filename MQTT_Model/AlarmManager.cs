using System.Collections.Generic;
using Scada_Demo.MQTT_Model;

namespace Scada_Demo.Models
{
    public static class AlarmManager
    {
        public static readonly List<AlarmDefinition> Definitions = new()
        {
            // ===========================
            // ALARM 1 (MW200)
            // ===========================
            new AlarmDefinition { AlarmNo = 1, Bit = 0,  Text = "Water Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 1, Bit = 1,  Text = "Ice / Silica Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 1, Bit = 2,  Text = "Aggregate 1 Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 1, Bit = 3,  Text = "Aggregate 2 Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 1, Bit = 4,  Text = "Aggregate 3 Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 1, Bit = 5,  Text = "Aggregate 4 Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 1, Bit = 6,  Text = "Aggregate 5 Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 1, Bit = 7,  Text = "Cement 1 Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 1, Bit = 8,  Text = "Asc. Conv. Zero Speed / Pull Cord Fault" },
            new AlarmDefinition { AlarmNo = 1, Bit = 9,  Text = "Ascending Conveyor Tripped" },
            new AlarmDefinition { AlarmNo = 1, Bit = 10, Text = "Mixer Grease Pump Not Healthy" },
            new AlarmDefinition { AlarmNo = 1, Bit = 11, Text = "Mixer Zero Speed Fault" },
            new AlarmDefinition { AlarmNo = 1, Bit = 12, Text = "NA" },
            new AlarmDefinition { AlarmNo = 1, Bit = 13, Text = "NA" },
            new AlarmDefinition { AlarmNo = 1, Bit = 14, Text = "NA" },
            new AlarmDefinition { AlarmNo = 1, Bit = 15, Text = "NA" },

            // ===========================
            // ALARM 2 (MW202)
            // ===========================
            new AlarmDefinition { AlarmNo = 2, Bit = 0,  Text = "Mixer Gate Not Closed" },
            new AlarmDefinition { AlarmNo = 2, Bit = 1,  Text = "Skip Not In Bottom" },
            new AlarmDefinition { AlarmNo = 2, Bit = 2,  Text = "Water Batching Time Exceeded" },
            new AlarmDefinition { AlarmNo = 2, Bit = 3,  Text = "Ice / Silica Batching Time Exceeded" },
            new AlarmDefinition { AlarmNo = 2, Bit = 4,  Text = "Aggregate Batching Time Exceeded" },
            new AlarmDefinition { AlarmNo = 2, Bit = 5,  Text = "Cement Batching Time Exceeded" },
            new AlarmDefinition { AlarmNo = 2, Bit = 6,  Text = "Admixture Batching Time Exceeded" },
            new AlarmDefinition { AlarmNo = 2, Bit = 7,  Text = "Skip Run Time Exceeded" },
            new AlarmDefinition { AlarmNo = 2, Bit = 8,  Text = "Cement 2 Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 2, Bit = 9,  Text = "Cement 3 Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 2, Bit = 10, Text = "NA" },
            new AlarmDefinition { AlarmNo = 2, Bit = 11, Text = "Admixture 1 Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 2, Bit = 12, Text = "Admixture 2 Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 2, Bit = 13, Text = "NA" },
            new AlarmDefinition { AlarmNo = 2, Bit = 14, Text = "SIWAREX Initialization Error" },
            new AlarmDefinition { AlarmNo = 2, Bit = 15, Text = "NA" },

            // ===========================
            // ALARM 3 (MW204)
            // ===========================
            new AlarmDefinition { AlarmNo = 3, Bit = 0,  Text = "Admixture Discharge Time Exceeded" },
            new AlarmDefinition { AlarmNo = 3, Bit = 1,  Text = "Compressor Overloaded" },
            new AlarmDefinition { AlarmNo = 3, Bit = 2,  Text = "Sand Vibrator Overloaded" },
            new AlarmDefinition { AlarmNo = 3, Bit = 3,  Text = "Aggregate Weigher Not Empty" },
            new AlarmDefinition { AlarmNo = 3, Bit = 4,  Text = "Cement Weigher Not Empty" },
            new AlarmDefinition { AlarmNo = 3, Bit = 5,  Text = "Water Weigher Not Empty" },
            new AlarmDefinition { AlarmNo = 3, Bit = 6,  Text = "Admixture Weigher Not Empty" },
            new AlarmDefinition { AlarmNo = 3, Bit = 7,  Text = "Ice / Silica Weigher Not Empty" },
            new AlarmDefinition { AlarmNo = 3, Bit = 8,  Text = "Slack Steel Fault" },
            new AlarmDefinition { AlarmNo = 3, Bit = 9,  Text = "Single Phase Error" },
            new AlarmDefinition { AlarmNo = 3, Bit = 10, Text = "Skip Thermal Fault" },
            new AlarmDefinition { AlarmNo = 3, Bit = 11, Text = "Mixer / Power Pack Not Healthy" },
            new AlarmDefinition { AlarmNo = 3, Bit = 12, Text = "Cement Screw Conveyor Overload" },
            new AlarmDefinition { AlarmNo = 3, Bit = 13, Text = "Admixture Dosing Pump Overload" },
            new AlarmDefinition { AlarmNo = 3, Bit = 14, Text = "Cement Discharge Time Exceeded" },
            new AlarmDefinition { AlarmNo = 3, Bit = 15, Text = "Water Discharge Time Exceeded" },

            // ===========================
            // ALARM 4 (MW206)
            // ===========================
            new AlarmDefinition { AlarmNo = 4, Bit = 0,  Text = "NOT USED" },
            new AlarmDefinition { AlarmNo = 4, Bit = 1,  Text = "NOT USED" },
            new AlarmDefinition { AlarmNo = 4, Bit = 2,  Text = "Mixer Gate Not Opened / Closed" },
            new AlarmDefinition { AlarmNo = 4, Bit = 3,  Text = "Ice / Silica Discharge Time Exceeded" },
            new AlarmDefinition { AlarmNo = 4, Bit = 4,  Text = "Admixture Discharge Pump Not Healthy" },
            new AlarmDefinition { AlarmNo = 4, Bit = 5,  Text = "Water Discharge Pump Not Healthy" },
            new AlarmDefinition { AlarmNo = 4, Bit = 6,  Text = "" },
            new AlarmDefinition { AlarmNo = 4, Bit = 7,  Text = "" },
            new AlarmDefinition { AlarmNo = 4, Bit = 8,  Text = "Conveyor Motor Overloaded" },
            new AlarmDefinition { AlarmNo = 4, Bit = 9,  Text = "Conveyor Pull Cord Fault" },
            new AlarmDefinition { AlarmNo = 4, Bit = 10, Text = "Conveyor Zero Speed Fault" },
            new AlarmDefinition { AlarmNo = 4, Bit = 11, Text = "AUTO Pause Mode Selected" },
            new AlarmDefinition { AlarmNo = 4, Bit = 12, Text = "Ice / Silica Screw Not Healthy" },
            new AlarmDefinition { AlarmNo = 4, Bit = 13, Text = "Ice / Silica Vibrator Not Healthy" },
            new AlarmDefinition { AlarmNo = 4, Bit = 14, Text = "Batching Gate Not Closed" },
            new AlarmDefinition { AlarmNo = 4, Bit = 15, Text = "Cement Vibrator Not Healthy" }
        };
    }
}