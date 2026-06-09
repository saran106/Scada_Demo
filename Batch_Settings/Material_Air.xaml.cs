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
namespace Scada_Demo.Batch_Settings 
{
    /// <summary>
    /// Interaction logic for Material_Air.xaml
    /// </summary>
    public partial class Material_Air : Window
    {
        S7Client plc = new S7Client();
        DispatcherTimer timer = new DispatcherTimer();

        public Material_Air()
        {
            InitializeComponent();
            // PLC CONNECT
           
        }

        private async void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MaterialinAir material = new MaterialinAir();
                if (AggregatePanel.Visibility == Visibility.Visible)
                {
                   

                    var data = await material.ReadValues_Agg();

                    txtAgg1.Text = data.Value92;
                    txtAgg2.Text = data.Value94;
                    txtAgg3.Text = data.Value96;
                    txtAgg4.Text = data.Value98;
                }
                else if (CementPanel.Visibility == Visibility.Visible)
                {
                   // MaterialinAir material = new MaterialinAir();

                    var data = await material.ReadValues_Cement();

                    txtC1.Text = data.Cement1;
                    txtC2.Text = data.Cement2;
                    txtC3.Text = data.Cement3;
                    txtC4.Text = data.Cement2;
                }
                else if (WaterPanel.Visibility == Visibility.Visible)
                {
                    var data = await material.ReadValues_WTR();

                    txtWTR1.Text = data.Wtr1;
                }
                else if (AdmixPanel.Visibility == Visibility.Visible)
                {
                    var data = await material.ReadValues_Admix();

                    txtAD1.Text = data.Admix1;
                    txtAD2.Text = data.Admix2;
                }
                else if (SilicaPanel.Visibility == Visibility.Visible)
                {
                    var data = await material.ReadValues_ICE();

                    txtICE.Text = data.Silica;
                  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       

        private async void WriteToPLC_Click(object sender, RoutedEventArgs e)
        {
        
            MaterialinAir mqtt = new MaterialinAir();

            if (AggregatePanel.Visibility == Visibility.Visible)
            {

                await mqtt.WriteAGG_92(txtAgg1.Text);
                await mqtt.WriteAGG_94(txtAgg2.Text);
                await mqtt.WriteAGG_96(txtAgg3.Text);
                await mqtt.WriteAGG_98(txtAgg4.Text);
            }
            if (CementPanel.Visibility == Visibility.Visible) 
            {
                await mqtt.WriteCEM_100(txtC1.Text);

                
            }
            if (WaterPanel.Visibility == Visibility.Visible)
            {
                await mqtt.WriteWTR_386(txtWTR1.Text);


            }

            if (AdmixPanel.Visibility == Visibility.Visible)
            {
                await mqtt.WriteADM_108(txtAD1.Text);
                await mqtt.WriteADM_110(txtAD2.Text);


            }

            if (SilicaPanel.Visibility == Visibility.Visible)
            {
                await mqtt.WriteICE_410(txtICE.Text);


            }


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
