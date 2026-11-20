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
using Scada_Demo.Services;
using Scada_Demo.Database;
using Scada_Demo.MQTT_Model;
using Scada_Demo.ViewModels.Transactions;
using Sharp7;
using System.Windows.Threading;

namespace Scada_Demo.Transactions
{
    /// <summary>
    /// Interaction logic for Manual_Operation2.xaml
    /// </summary>
    public partial class Manual_Operation2 : Window
    {
        S7Client plc = new S7Client();
        DispatcherTimer timer = new DispatcherTimer();
        private ManualOperation2ViewModel vm;

        public Manual_Operation2()
        {
            InitializeComponent();

            // PLC CONNECT
            vm = App.ManualOperation2VM;
            DataContext = vm;
        }
    }
}