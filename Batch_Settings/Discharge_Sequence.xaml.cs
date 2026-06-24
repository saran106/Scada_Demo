using System;
using System.Windows;
using Scada_Demo.Services;

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

                Cem.Text = data.DischargeDelay.Cement.ToString();
                Wat.Text = data.DischargeDelay.Water.ToString();
                Admix12.Text = data.DischargeDelay.Admix.ToString();
                Skip.Text = data.DischargeDelay.Skip.ToString();
                PC.Text = data.DischargeDelay.PumpCutOff.ToString();
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

                MessageBox.Show("Values Written Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}