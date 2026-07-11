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
using Scada_Demo.MQTT_Model;
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


        }

        private async void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MaterialinAir batchSettings = new MaterialinAir();

                BatchSettingsModel data = await batchSettings.ReadValues();

                if (AggregatePanel.Visibility == Visibility.Visible)
                {
                    vm.txtAgg1 = data.batchSettings_MaterialInAir.Agg92.ToString();
                    vm.txtAgg2 = data.batchSettings_MaterialInAir.Agg94.ToString();
                    vm.txtAgg3 = data.batchSettings_MaterialInAir.Agg96.ToString();
                    vm.txtAgg4 = data.batchSettings_MaterialInAir.Agg98.ToString();
                }
                else if (CementPanel.Visibility == Visibility.Visible)
                {
                    
                }
                else if (WaterPanel.Visibility == Visibility.Visible)
                {
                    vm.txtWTR1 = data.batchSettings_MaterialInAir.Water386.ToString();
                }
                else if (AdmixPanel.Visibility == Visibility.Visible)
                {
                    vm.txtAD1 = data.batchSettings_MaterialInAir.Admix108.ToString();
                    vm.txtAD2 = data.batchSettings_MaterialInAir.Admix110.ToString();
                }
                else if (SilicaPanel.Visibility == Visibility.Visible)
                {
                     vm.txtICE = data.batchSettings_MaterialInAir.Ice410.ToString();
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
        //            vm.txtAgg1 = data.Agg92.ToString();
        //            vm.txtAgg2 = data.Agg94.ToString();
        //            vm.txtAgg3 = data.Agg96.ToString();
        //            vm.txtAgg4 = data.Agg98.ToString();
        //        }
        //        else if (CementPanel.Visibility == Visibility.Visible)
        //        {
        //            MessageBox.Show("Cement JSON migrate pannala innum");
        //        }
        //        else if (WaterPanel.Visibility == Visibility.Visible)
        //        {
        //            vm.txtWTR1 = data.Water386.ToString();
        //        }
        //        else if (AdmixPanel.Visibility == Visibility.Visible)
        //        {
        //            vm.txtAD1 = data.Admix108.ToString();
        //            vm.txtAD2 = data.Admix110.ToString();
        //        }
        //        else if (SilicaPanel.Visibility == Visibility.Visible)
        //        {
        //            vm.txtICE = data.Ice410.ToString();
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

                //await mqtt.WriteAGG_92(vm.txtAgg1);
                //await Task.Delay(1000);
                await mqtt.WriteAGG_94(vm.txtAgg2);
                //await Task.Delay(1000);
                await mqtt.WriteAGG_96(vm.txtAgg3);
                //await Task.Delay(1000);
                await mqtt.WriteAGG_98(vm.txtAgg4);
                //await Task.Delay(1000);
            }
            if (CementPanel.Visibility == Visibility.Visible) 
            {
                await mqtt.WriteCEM_100(txtC1.Text);
               // await Task.Delay(1000);


            }
            if (WaterPanel.Visibility == Visibility.Visible)
            {
                await mqtt.WriteWTR_386(vm.txtWTR1);
              //  await Task.Delay(1000);


            }

            if (AdmixPanel.Visibility == Visibility.Visible)
            {
                await mqtt.WriteADM_108(vm.txtAD1);
              //  await Task.Delay(1000);
                await mqtt.WriteADM_110(vm.txtAD2);
               // await Task.Delay(1000);


            }

            if (SilicaPanel.Visibility == Visibility.Visible)
            {
                await mqtt.WriteICE_410(vm.txtICE);
               // await Task.Delay(1000);


            }


            // MQTT write mudinja apram
            SaveMaterialInAir();

        }

        private void SaveMaterialInAir()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM MaterialEmptyValue_Setup)
BEGIN
    UPDATE MaterialEmptyValue_Setup
    SET
        Gate1=@Gate1,
        Gate2=@Gate2,
        Gate3=@Gate3,
        Gate4=@Gate4,
        Gate5=@Gate5,
        Gate6=@Gate6,

        Cement1=@Cement1,
        Cement2=@Cement2,
        Cement3=@Cement3,
        Cement4=@Cement4,
        Cement5=@Cement5,

        Water=@Water,
        Water2=@Water2,
        Water3=@Water3,

        Admix1=@Admix1,
        Admix2=@Admix2,
        Admix3=@Admix3,
        Admix4=@Admix4,

        Silica=@Silica
END
ELSE
BEGIN
    INSERT INTO MaterialEmptyValue_Setup
    (
        Gate1,Gate2,Gate3,Gate4,Gate5,Gate6,
        Cement1,Cement2,Cement3,Cement4,Cement5,
        Water,Water2,Water3,
        Admix1,Admix2,Admix3,Admix4,
        Silica
    )
    VALUES
    (
        @Gate1,@Gate2,@Gate3,@Gate4,@Gate5,@Gate6,
        @Cement1,@Cement2,@Cement3,@Cement4,@Cement5,
        @Water,@Water2,@Water3,
        @Admix1,@Admix2,@Admix3,@Admix4,
        @Silica
    )
END", con);

                    cmd.Parameters.AddWithValue("@Gate1", string.IsNullOrWhiteSpace(vm.txtAgg1) ? (object)DBNull.Value : Convert.ToInt32(vm.txtAgg1));
                    cmd.Parameters.AddWithValue("@Gate2", string.IsNullOrWhiteSpace(vm.txtAgg2) ? (object)DBNull.Value : Convert.ToInt32(vm.txtAgg2));
                    cmd.Parameters.AddWithValue("@Gate3", string.IsNullOrWhiteSpace(vm.txtAgg3) ? (object)DBNull.Value : Convert.ToInt32(vm.txtAgg3));
                    cmd.Parameters.AddWithValue("@Gate4", string.IsNullOrWhiteSpace(vm.txtAgg4) ? (object)DBNull.Value : Convert.ToInt32(vm.txtAgg4));
                    cmd.Parameters.AddWithValue("@Gate5", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Gate6", DBNull.Value);

                    cmd.Parameters.AddWithValue("@Cement1", string.IsNullOrWhiteSpace(txtC1.Text) ? (object)DBNull.Value : Convert.ToInt32(txtC1.Text));
                    cmd.Parameters.AddWithValue("@Cement2", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Cement3", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Cement4", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Cement5", DBNull.Value);

                    cmd.Parameters.AddWithValue("@Water", string.IsNullOrWhiteSpace(vm.txtWTR1) ? (object)DBNull.Value : Convert.ToInt32(vm.txtWTR1));
                    cmd.Parameters.AddWithValue("@Water2", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Water3", DBNull.Value);

                    cmd.Parameters.AddWithValue("@Admix1", string.IsNullOrWhiteSpace(vm.txtAD1) ? (object)DBNull.Value : Convert.ToDecimal(vm.txtAD1));
                    cmd.Parameters.AddWithValue("@Admix2", string.IsNullOrWhiteSpace(vm.txtAD2) ? (object)DBNull.Value : Convert.ToDecimal(vm.txtAD2));
                    cmd.Parameters.AddWithValue("@Admix3", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Admix4", DBNull.Value);

                    cmd.Parameters.AddWithValue("@Silica", string.IsNullOrWhiteSpace(vm.txtICE) ? (object)DBNull.Value : Convert.ToInt32(vm.txtICE));

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private async void WriteToPLC_New_Click(object sender, RoutedEventArgs e)
        {
            //MaterialinAir mqtt = new MaterialinAir();
           

            //BatchSettingsModel model = new BatchSettingsModel();

            //model.Type = "MaterialInAir";

            //model.batchSettings_MaterialInAir.Agg92 = Convert.ToInt16(vm.txtAgg1);
            ////model.batchSettings_MaterialInAir.Agg92 = Convert.ToInt16(vm.txtAgg1);

            //model.batchSettings_MaterialInAir.Agg94 = Convert.ToInt16(vm.txtAgg2);
            //model.batchSettings_MaterialInAir.Agg96 = Convert.ToInt16(vm.txtAgg3);
            //model.batchSettings_MaterialInAir.Agg98 = Convert.ToInt16(vm.txtAgg4);

            //model.batchSettings_MaterialInAir.Water386 = Convert.ToInt16(vm.txtWTR1);

            //model.batchSettings_MaterialInAir.Admix108 = Convert.ToInt16(vm.txtAD1);
            //model.batchSettings_MaterialInAir.Admix110 = Convert.ToInt16(vm.txtAD2);

            //model.batchSettings_MaterialInAir.Ice410 = Convert.ToInt16(vm.txtICE);

            //// Publish request

            


            //await mqtt.WriteBatchSettings(model);



            // Latest response from ViewModel/Store
            bool result = await vm.WriteToPLCAsync();

            if (result)
            {
                MessageBox.Show("PLC Write Success");
                SaveMaterialInAir();
            }
            else
            {
                MessageBox.Show("PLC Write Failed");
            }
        }
    }
}
