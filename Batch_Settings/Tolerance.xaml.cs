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
using Scada_Demo.Calibration;
using Scada_Demo.MQTT_Model;
using Scada_Demo.Services;

namespace Scada_Demo.Batch_Settings
{
    /// <summary>
    /// Interaction logic for Tolerance.xaml
    /// </summary>
    public partial class Tolerance : Window
    {
        public Tolerance()
        {
            InitializeComponent();
        }

        private async void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MaterialinAir batchSettings = new MaterialinAir();

                BatchSettingsModel data = await batchSettings.ReadValues();

                if (AggregatePanel.Visibility == Visibility.Visible)
                {
                    Agg1.Text = data.batchSettings_Tolerance.Agg1.ToString();
                    Agg2.Text = data.batchSettings_Tolerance.Agg2.ToString();
                    Agg3.Text = data.batchSettings_Tolerance.Agg3.ToString();
                    Agg4.Text = data.batchSettings_Tolerance.Agg4.ToString();
                }
                else if (CementPanel.Visibility == Visibility.Visible)
                {
                    Cem1.Text = data.batchSettings_Tolerance.Cem1.ToString();
                    Cem2.Text = data.batchSettings_Tolerance.Cem1.ToString();
                    Cem3.Text = data.batchSettings_Tolerance.Cem1.ToString();

                    Cem4.Text = data.batchSettings_Tolerance.Cem4.ToString();
                }
                else if (WaterPanel.Visibility == Visibility.Visible)
                {
                    Wtr1.Text = data.batchSettings_Tolerance.Water.ToString();
                }
                else if (AdmixPanel.Visibility == Visibility.Visible)
                {
                    Adm1.Text = data.batchSettings_Tolerance.Adm1.ToString();
                }
                else if (SilicaPanel.Visibility == Visibility.Visible)
                {
                    Ice1.Text = data.batchSettings_Tolerance.Ice.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void BtnWrite_Click(object sender, RoutedEventArgs e)
        {
            Services.Tolerance mqtt = new Services.Tolerance();

            if (AggregatePanel.Visibility == Visibility.Visible)
            {
                await mqtt.WriteTOL_118(Agg1.Text);
                await mqtt.WriteTOL_120(Agg2.Text);
                await mqtt.WriteTOL_122(Agg3.Text);
                await mqtt.WriteTOL_124(Agg4.Text);
            }

            else if (CementPanel.Visibility == Visibility.Visible)
            {
                // Cem1, Cem2, Cem3 common tolerance
                await mqtt.WriteTOL_126(Cem1.Text);


                // Cem4 separate DB
                await mqtt.WriteTOL_DB184_78(Cem4.Text);
            }

            else if (WaterPanel.Visibility == Visibility.Visible)
            {
                await mqtt.WriteTOL_376(Wtr1.Text);
            }

            else if (AdmixPanel.Visibility == Visibility.Visible)
            {
                await mqtt.WriteTOL_128(Adm1.Text);
            }

            else if (SilicaPanel.Visibility == Visibility.Visible)
            {
                await mqtt.WriteTOL_400(Ice1.Text);
            }

            MessageBox.Show("Tolerance Values Written Successfully");
        }
        void HideAll()
        {
            AggregatePanel.Visibility = Visibility.Collapsed;
            CementPanel.Visibility = Visibility.Collapsed;
            WaterPanel.Visibility = Visibility.Collapsed;
            AdmixPanel.Visibility = Visibility.Collapsed;
            SilicaPanel.Visibility = Visibility.Collapsed;
        }

        void SetActiveButton(Button active)
        {
            foreach (Button btn in TabButtons.Children)
                btn.Tag = null;

            active.Tag = "Active";
        }

        private void ShowAggregate(object sender, RoutedEventArgs e)
        {
            HideAll();
            AggregatePanel.Visibility = Visibility.Visible;
            if (sender != null) SetActiveButton((Button)sender);
        }

        private void ShowCement(object sender, RoutedEventArgs e)
        {
            HideAll();
            CementPanel.Visibility = Visibility.Visible;
            SetActiveButton((Button)sender);
        }

        private void ShowWater(object sender, RoutedEventArgs e)
        {
            HideAll();
            WaterPanel.Visibility = Visibility.Visible;
            SetActiveButton((Button)sender);
        }

        private void ShowAdmix(object sender, RoutedEventArgs e)
        {
            HideAll();
            AdmixPanel.Visibility = Visibility.Visible;
            SetActiveButton((Button)sender);
        }

        private void ShowSilica(object sender, RoutedEventArgs e)
        {
            HideAll();
            SilicaPanel.Visibility = Visibility.Visible;
            SetActiveButton((Button)sender);
        }

    }
}
