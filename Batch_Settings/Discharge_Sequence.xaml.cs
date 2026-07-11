using System;
using System.Windows;
using Microsoft.Data.SqlClient;
using Scada_Demo.Services;

using Scada_Demo.Database;
using Scada_Demo.MQTT_Model;
namespace Scada_Demo.Batch_Settings
{
    public partial class Discharge_Sequence : Window
    {
        public Discharge_Sequence()
        {
            InitializeComponent();
        }

        //private async void BtnRead_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        Discharge_Delay service = new Discharge_Delay();

        //        var data = await service.ReadValues();

        //        Cem.Text = data.Cement.ToString();
        //        Wat.Text = data.Water.ToString();
        //        Admix12.Text = data.Admix.ToString();
        //        Skip.Text = data.Skip.ToString();
        //        PC.Text = data.PumpCutOff.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        private async void BtnRead_Click(object sender, RoutedEventArgs e)
            {
            try
            {
                MaterialinAir service = new MaterialinAir();

                var data = await service.ReadValues();

                Cem.Text = data.batchSettings_DischargeDelay.Cement.ToString();
                Wat.Text = data.batchSettings_DischargeDelay.Water.ToString();
                Admix12.Text = data.batchSettings_DischargeDelay.Admix.ToString();
                Skip.Text = data.batchSettings_DischargeDelay.Skip.ToString();
                PC.Text = data.batchSettings_DischargeDelay.PumpCutOff.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void BtnWrite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Discharge_Delay service = new Discharge_Delay();

                await service.WriteCement(Cem.Text);
                //await Task.Delay(1000);

                await service.WriteWater(Wat.Text);
                //await Task.Delay(1000);

                await service.WriteAdmix(Admix12.Text);
                //await Task.Delay(1000);

                await service.WriteSkip(Skip.Text);
                //await Task.Delay(1000);

                await service.WritePumpCutOff(PC.Text);

                SaveDischargeDelay();

                MessageBox.Show("Values Written Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveDischargeDelay()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM DisDelay_Setup)
BEGIN
    UPDATE DisDelay_Setup
    SET
        Cement = @Cement,
        Water = @Water,
        Admix12 = @Admix12,
        --Silica = @Silica,
        Wtr_Dis_Pump_Cutoff = @PumpCutOff,
        Wait_Hopper_Instant_Open_delay = @WaitHopper
END
ELSE
BEGIN
    INSERT INTO DisDelay_Setup
    (
        Cement,
        Water,
        Admix12,
        --Silica,
        Wtr_Dis_Pump_Cutoff,
        Wait_Hopper_Instant_Open_delay
    )
    VALUES
    (
        @Cement,
        @Water,
        @Admix12,
        --@Silica,
        @PumpCutOff,
        @WaitHopper
    )
END", con);

                    cmd.Parameters.AddWithValue("@Cement",
                        string.IsNullOrWhiteSpace(Cem.Text) ? (object)DBNull.Value : Convert.ToInt32(Cem.Text));

                    cmd.Parameters.AddWithValue("@Water",
                        string.IsNullOrWhiteSpace(Wat.Text) ? (object)DBNull.Value : Convert.ToInt32(Wat.Text));

                    cmd.Parameters.AddWithValue("@Admix12",
                        string.IsNullOrWhiteSpace(Admix12.Text) ? (object)DBNull.Value : Convert.ToInt32(Admix12.Text));

                    cmd.Parameters.AddWithValue("@WaitHopper",
                        string.IsNullOrWhiteSpace(Skip.Text) ? (object)DBNull.Value : Convert.ToInt32(Skip.Text));

                    cmd.Parameters.AddWithValue("@PumpCutOff",
                        string.IsNullOrWhiteSpace(PC.Text) ? (object)DBNull.Value : Convert.ToInt32(PC.Text));

                   

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}