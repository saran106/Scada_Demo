using Scada_Demo.MQTT_Model;
using Scada_Demo.Services;
using System.Windows;

namespace Scada_Demo.Batch_Settings
{
    public partial class Empty_Value : Window
    {
        private readonly Services.Empty_Value _batchSettings_EmptyValueService =
            new Services.Empty_Value();

        public Empty_Value()
        {
            InitializeComponent();
        }

        private async void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            MaterialinAir batchSettings = new MaterialinAir();

            BatchSettingsModel data = await batchSettings.ReadValues();

            Agg.Text = data.batchSettings_EmptyValue.Agg.ToString();
            Cem.Text = data.batchSettings_EmptyValue.Cem.ToString();
            Water.Text = data.batchSettings_EmptyValue.Water.ToString();

            // Same PLC value for both Admix boxes
            Adm12.Text = data.batchSettings_EmptyValue.Admix.ToString();
            //adm34.Text = data.batchSettings_EmptyValue.Admix.ToString();

            // Ice / Silica
            silica.Text = data.batchSettings_EmptyValue.Ice.ToString();

            //WTR.Text = data.batchSettings_EmptyValue.Water.ToString();
        }


        private async void BtnWrite_Click(object sender, RoutedEventArgs e)
        {
            await _batchSettings_EmptyValueService.WriteEMPTY_112(Agg.Text);
            await _batchSettings_EmptyValueService.WriteEMPTY_114(Cem.Text);
            await _batchSettings_EmptyValueService.WriteEMPTY_428(Water.Text);

            // Admixture 1&2 value PLC-ku write
            await _batchSettings_EmptyValueService.WriteEMPTY_116(Adm12.Text);

            // Ice / Silica value PLC-ku write
            await _batchSettings_EmptyValueService.WriteEMPTY_430(silica.Text);

            MessageBox.Show(
                "Empty Values Written Successfully",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}