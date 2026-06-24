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
                MaterialinAir batchSettings = new MaterialinAir();

                BatchSettingsModel data = await batchSettings.ReadValues();

                if (AggregatePanel.Visibility == Visibility.Visible)
                {
                    txtAgg1.Text = data.MaterialInAir.Agg92.ToString();
                    txtAgg2.Text = data.MaterialInAir.Agg94.ToString();
                    txtAgg3.Text = data.MaterialInAir.Agg96.ToString();
                    txtAgg4.Text = data.MaterialInAir.Agg98.ToString();
                }
                else if (CementPanel.Visibility == Visibility.Visible)
                {
                    
                }
                else if (WaterPanel.Visibility == Visibility.Visible)
                {
                    txtWTR1.Text = data.MaterialInAir.Water386.ToString();
                }
                else if (AdmixPanel.Visibility == Visibility.Visible)
                {
                    txtAD1.Text = data.MaterialInAir.Admix108.ToString();
                    txtAD2.Text = data.MaterialInAir.Admix110.ToString();
                }
                else if (SilicaPanel.Visibility == Visibility.Visible)
                {
                     txtICE.Text = data.MaterialInAir.Ice410.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //private async void BtnRead_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        MaterialinAir material = new MaterialinAir();

        //        var data = await material.ReadValues();

        //        if (AggregatePanel.Visibility == Visibility.Visible)
        //        {
        //            txtAgg1.Text = data.Agg92.ToString();
        //            txtAgg2.Text = data.Agg94.ToString();
        //            txtAgg3.Text = data.Agg96.ToString();
        //            txtAgg4.Text = data.Agg98.ToString();
        //        }
        //        else if (CementPanel.Visibility == Visibility.Visible)
        //        {
        //            MessageBox.Show("Cement JSON migrate pannala innum");
        //        }
        //        else if (WaterPanel.Visibility == Visibility.Visible)
        //        {
        //            txtWTR1.Text = data.Water386.ToString();
        //        }
        //        else if (AdmixPanel.Visibility == Visibility.Visible)
        //        {
        //            txtAD1.Text = data.Admix108.ToString();
        //            txtAD2.Text = data.Admix110.ToString();
        //        }
        //        else if (SilicaPanel.Visibility == Visibility.Visible)
        //        {
        //            txtICE.Text = data.Ice410.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}



        private async void WriteToPLC_Click(object sender, RoutedEventArgs e)
        {
        
            MaterialinAir mqtt = new MaterialinAir();

            if (AggregatePanel.Visibility == Visibility.Visible)
            {

                await mqtt.WriteAGG_92(txtAgg1.Text);
                //await Task.Delay(1000);
                await mqtt.WriteAGG_94(txtAgg2.Text);
                //await Task.Delay(1000);
                await mqtt.WriteAGG_96(txtAgg3.Text);
                //await Task.Delay(1000);
                await mqtt.WriteAGG_98(txtAgg4.Text);
                //await Task.Delay(1000);
            }
            if (CementPanel.Visibility == Visibility.Visible) 
            {
                await mqtt.WriteCEM_100(txtC1.Text);
               // await Task.Delay(1000);


            }
            if (WaterPanel.Visibility == Visibility.Visible)
            {
                await mqtt.WriteWTR_386(txtWTR1.Text);
              //  await Task.Delay(1000);


            }

            if (AdmixPanel.Visibility == Visibility.Visible)
            {
                await mqtt.WriteADM_108(txtAD1.Text);
              //  await Task.Delay(1000);
                await mqtt.WriteADM_110(txtAD2.Text);
               // await Task.Delay(1000);


            }

            if (SilicaPanel.Visibility == Visibility.Visible)
            {
                await mqtt.WriteICE_410(txtICE.Text);
               // await Task.Delay(1000);


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
