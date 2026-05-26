using System.Windows;
using Scada_Demo.Models;
using Scada_Demo.Services.Interface;
using Scada_Demo.Services.PLC;

namespace Scada_Demo.Batch_Settings
{
    public partial class Batch_OutputMode : Window
    {
        IPLCService plc = new Dummy_PLC();

        public Batch_OutputMode()
        {
            InitializeComponent();

            Loaded += Batch_OutputMode_Loaded;
        }
        private void btnRead_Click(
    object sender,
    RoutedEventArgs e)
        {
            var data =
                plc.ReadBatchOutput();

            cmbBatchMode.Text =
                data.BatchMode;

            txtFullOpen.Text =
                data.GateFullOpen.ToString();

            txtHalfOpen.Text =
                data.GateHalfOpen.ToString();

            MessageBox.Show(
                "Data Read From PLC");
        }
        private void Batch_OutputMode_Loaded( object sender, RoutedEventArgs e)
        {
            var data = plc.ReadBatchOutput();

            cmbBatchMode.Text = data.BatchMode;

            txtFullOpen.Text = data.GateFullOpen.ToString();

            txtHalfOpen.Text = data.GateHalfOpen.ToString();
        }

        private void btnWrite_Click(object sender,RoutedEventArgs e)
        {
            Batch_Output model =
                new Batch_Output()
                {
                    BatchMode =
                        cmbBatchMode.Text,

                    GateFullOpen =
                        int.Parse(
                            txtFullOpen.Text),

                    GateHalfOpen =
                        int.Parse(
                            txtHalfOpen.Text)
                };

            plc.WriteBatchOutput(model);

            MessageBox.Show(
                "Data Written To PLC");
        }
    }
}