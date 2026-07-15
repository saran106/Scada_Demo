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
using System.Windows.Threading;
using Scada_Demo.MQTT_Model;
using Scada_Demo.Services;
using Scada_Demo.ViewModels.BatchSettings;
using Sharp7;


namespace Scada_Demo.Batch_Settings
{
    /// <summary>
    /// Interaction logic for Jog_Time.xaml
    /// </summary>
    public partial class Jog_Time : Window
    {
        

        S7Client plc = new S7Client();
        DispatcherTimer timer = new DispatcherTimer();
        private JogTimeViewModel vm;
        public Jog_Time()
        {
            InitializeComponent();
            // PLC CONNECT
            vm = App.JogTimeVM;
            DataContext = vm;

            //txtAgg1.Text = "12345";
        }

    }
}
