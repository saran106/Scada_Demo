using Microsoft.Data.SqlClient;
using Scada_Demo.MQTT_Model;
using Scada_Demo.Services;
using System.Windows;
using Scada_Demo.Database;
using Scada_Demo.MQTT_Model;
using Sharp7;
using System.Windows.Threading;
using Scada_Demo.ViewModels.BatchSettings;

namespace Scada_Demo.Batch_Settings
{
    public partial class Empty_Value : Window
    {
        S7Client plc = new S7Client();
        DispatcherTimer timer = new DispatcherTimer();
        private EmptyValueViewModel vm;
        public Empty_Value()
        {
            InitializeComponent();
            vm = App.EmptyVM;
            DataContext = vm;

        }

       
    }
}