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
                Services.Step_Time step = new Services.Step_Time();

                if (AggregatePanel.Visibility == Visibility.Visible)
                {
                    var data = await step.ReadValue();

                    Agg1.Text = data.Agg;
                    Agg2.Text = data.Agg;
                    Agg3.Text = data.Agg;
                    Agg4.Text = data.Agg;
                    Agg5.Text = data.Agg;
                    Agg6.Text = data.Agg;
                }
                else if (CementPanel.Visibility == Visibility.Visible)
                {
                    var data = await step.ReadValue();

                    Cem1.Text = data.Cem1;
                    Cem4.Text = data.Cem4;
                }
                else if (WaterPanel.Visibility == Visibility.Visible)
                {
                    var data = await step.ReadValue();

                    Wtr1.Text = data.Water;
                    Wtr2.Text = data.Water;
                    Wtr3.Text = data.Water;
                }
                else if (AdmixPanel.Visibility == Visibility.Visible)
                {
                    var data = await step.ReadValue();

                    Admix1.Text = data.Admix1;
                }
                else if (SilicaPanel.Visibility == Visibility.Visible)
                {
                    var data = await step.ReadValue();

                    Silica.Text = data.Silica;
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
                else if (WaterPanel.Visibility == Visibility.Visible)
                {
                    await step.WriteSTEP_386(Wtr1.Text);
                }
                else if (AdmixPanel.Visibility == Visibility.Visible)
                {
                    await step.WriteSTEP_140(Admix1.Text);
                }
                else if (SilicaPanel.Visibility == Visibility.Visible)
                {
                    await step.WriteSTEP_410(Silica.Text);
                }

                MessageBox.Show("Values Written Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
