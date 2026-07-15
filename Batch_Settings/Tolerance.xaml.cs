using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;
using Scada_Demo.Calibration;
using Scada_Demo.MQTT_Model;
using Scada_Demo.Services;
using Scada_Demo.Database;
using Scada_Demo.MQTT_Model;
using Scada_Demo.ViewModels.BatchSettings;
using Sharp7;
using System.Windows.Threading;

namespace Scada_Demo.Batch_Settings
{
    /// <summary>
    /// Interaction logic for Tolerance.xaml
    /// </summary>
    public partial class Tolerance : Window
    {
      
        S7Client plc = new S7Client();
        DispatcherTimer timer = new DispatcherTimer();
        private ToleranceViewModel vm;
        public Tolerance()
        {
            InitializeComponent();
            // PLC CONNECT
            vm = App.TolVM;
            DataContext = vm;

            //txtAgg1.Text = "12345";
        }

    }
}
