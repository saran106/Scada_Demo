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
using Sharp7;
using System.Windows.Threading;
using Scada_Demo.Services;
using Scada_Demo.MQTT_Model;
using Microsoft.Data.SqlClient;
using Scada_Demo.Database;
using Scada_Demo.ViewModels.BatchSettings;

namespace Scada_Demo.Batch_Settings
{
    /// <summary>
    /// Interaction logic for Material_Air.xaml
    /// </summary>
    public partial class Material_Air : Window
    {
        S7Client plc = new S7Client();
        DispatcherTimer timer = new DispatcherTimer();
        private MaterialInAirViewModel vm;


        public Material_Air()
        {
            InitializeComponent();
            // PLC CONNECT
            vm = App.MaterialVM;
            DataContext = vm;

            //txtAgg1.Text = "12345";
        }
    }
}
