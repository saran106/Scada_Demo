using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using Scada_Demo.MQTT_Model;
using Scada_Demo.Services;
using Microsoft.Data.SqlClient;
using Scada_Demo.Database;
using Scada_Demo.MQTT_Model;

namespace Scada_Demo.Batch_Settings
{
    public partial class Gate_Sequence : Window
    {
        public Gate_Sequence()
        {
            InitializeComponent();
            DataContext = App.GateSeqVM;
        }
    }
}