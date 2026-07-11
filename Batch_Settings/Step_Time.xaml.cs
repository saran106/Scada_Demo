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

namespace Scada_Demo.Batch_Settings
{
    /// <summary>
    /// Interaction logic for Step_Time.xaml
    /// </summary>
    public partial class Step_Time : Window
    {
        public Step_Time()
        {
            InitializeComponent();
            ShowAggregate(null, null);
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


        private async void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MaterialinAir mqtt = new MaterialinAir();

                var data = await mqtt.ReadValues();

                if (AggregatePanel.Visibility == Visibility.Visible)
                {
                    Agg1.Text = data.batchSettings_StepTime.Step136.ToString();
                    Agg2.Text = data.batchSettings_StepTime.Step136.ToString();
                    Agg3.Text = data.batchSettings_StepTime.Step136.ToString();
                    Agg4.Text = data.batchSettings_StepTime.Step136.ToString();
                    Agg5.Text = data.batchSettings_StepTime.Step136.ToString();
                    Agg6.Text = data.batchSettings_StepTime.Step136.ToString();
                }
                else if (CementPanel.Visibility == Visibility.Visible)
                {
                    Cem1.Text = data.batchSettings_StepTime.Step548.ToString();
                    Cem4.Text = data.batchSettings_StepTime.Step552.ToString();
                }
                else if (AdmixPanel.Visibility == Visibility.Visible)
                {
                    Admix1.Text = data.batchSettings_StepTime.Step140.ToString();
                    Admix2.Text = data.batchSettings_StepTime.Step140.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveStepTime()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM StepTime_Setup)
BEGIN
    UPDATE StepTime_Setup
    SET
        Gate1 = @Gate1,
        Gate2= @Gate1,
        Gate3= @Gate1,
        Gate4= @Gate1,
        Gate5= @Gate1,
        Gate6= @Gate1,
        Cement1 = @Cement1,
        Cement4 = @Cement4,
        Admix1 = @Admix1
        --Silica = @Silica
END
ELSE
BEGIN
    INSERT INTO StepTime_Setup
    (
        Gate1,
        Gate2,
        Gate3,
        Gate4,
        Gate5,
        Gate6,
        Cement1,
        Cement4,
        Admix1
        --Silica
    )
    VALUES
    (
        @Gate1,
          @Gate1,
          @Gate1,
          @Gate1,
          @Gate1,
          @Gate1,
        @Cement1,
        @Cement4,
        @Admix1
        --@Silica
    )
END", con);

                    cmd.Parameters.AddWithValue("@Gate1",
                        string.IsNullOrWhiteSpace(Agg1.Text) ? (object)DBNull.Value : Convert.ToDecimal(Agg1.Text));

                    cmd.Parameters.AddWithValue("@Cement1",
                        string.IsNullOrWhiteSpace(Cem1.Text) ? (object)DBNull.Value : Convert.ToDecimal(Cem1.Text));

                    cmd.Parameters.AddWithValue("@Cement4",
                        string.IsNullOrWhiteSpace(Cem4.Text) ? (object)DBNull.Value : Convert.ToDecimal(Cem4.Text));

                    cmd.Parameters.AddWithValue("@Admix1",
                        string.IsNullOrWhiteSpace(Admix1.Text) ? (object)DBNull.Value : Convert.ToDecimal(Admix1.Text));

                    //cmd.Parameters.AddWithValue("@Silica",
                    //    string.IsNullOrWhiteSpace(Silica.Text) ? (object)DBNull.Value : Convert.ToDecimal(Silica.Text));

                    cmd.ExecuteNonQuery();
                }
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
                Services.Step_Time step = new Services.Step_Time();

                if (AggregatePanel.Visibility == Visibility.Visible)
                {
                    await step.WriteSTEP_136(Agg1.Text);
                }
                else if (CementPanel.Visibility == Visibility.Visible)
                {
                    await step.WriteSTEP_548(Cem1.Text);
                    await step.WriteSTEP_552(Cem4.Text);
                }
                //else if (WaterPanel.Visibility == Visibility.Visible)
                //{
                //    await step.WriteSTEP_386(Wtr1.Text);
                //}
                else if (AdmixPanel.Visibility == Visibility.Visible)
                {
                    await step.WriteSTEP_140(Admix1.Text);
                }
                else if (SilicaPanel.Visibility == Visibility.Visible)
                {
                    await step.WriteSTEP_410(Silica.Text);
                }

                SaveStepTime();
                MessageBox.Show("Values Written Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
