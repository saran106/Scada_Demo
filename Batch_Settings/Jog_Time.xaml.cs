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
using Scada_Demo.Services;


namespace Scada_Demo.Batch_Settings
{
    /// <summary>
    /// Interaction logic for Jog_Time.xaml
    /// </summary>
    public partial class Jog_Time : Window
    {
        public Jog_Time()
        {
            InitializeComponent();
            ShowAggregate(null, null); // default tab
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
                JogTime jog = new JogTime();

                var data = await jog.ReadValue();

                if (AggregatePanel.Visibility == Visibility.Visible)
                {
                    Agg1JogOn.Text = data.Agg1On.ToString();
                    Agg2JogOn.Text = data.Agg2On.ToString();
                    Agg3JogOn.Text = data.Agg3On.ToString();
                    Agg4JogOn.Text = data.Agg4On.ToString();

                    Agg1JogOff.Text = data.Agg1Off.ToString();
                    Agg2JogOff.Text = data.Agg2Off.ToString();
                    Agg3JogOff.Text = data.Agg3Off.ToString();
                    Agg4JogOff.Text = data.Agg4Off.ToString();
                }
                else if (CementPanel.Visibility == Visibility.Visible)
                {
                    Cem3JogOn.Text = data.Cem3On.ToString();
                }
                else if (WaterPanel.Visibility == Visibility.Visible)
                {
                    Wtr1JogOn.Text = data.WaterOn.ToString();
                }
                else if (AdmixPanel.Visibility == Visibility.Visible)
                {
                    Admix1JogOn.Text = data.Adm1On.ToString();
                }
                else if (SilicaPanel.Visibility == Visibility.Visible)
                {
                    SilicaJogOn.Text = data.IceOn.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private async void WriteToPLC_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                JogTime jog = new JogTime();

                if (AggregatePanel.Visibility == Visibility.Visible)
                {
                    await jog.WriteJOGON_142(Agg1JogOn.Text);
                    await jog.WriteJOGON_146(Agg2JogOn.Text);
                    await jog.WriteJOGON_150(Agg3JogOn.Text);
                    await jog.WriteJOGON_154(Agg4JogOn.Text);

                    await jog.WriteJOGOFF_144(Agg1JogOff.Text);
                    await jog.WriteJOGOFF_148(Agg2JogOff.Text);
                    await jog.WriteJOGOFF_152(Agg3JogOff.Text);
                    await jog.WriteJOGOFF_156(Agg4JogOff.Text);
                }

                else if (CementPanel.Visibility == Visibility.Visible)
                {
                    await jog.WriteJOGON_236(Cem3JogOn.Text);
                }

                else if (WaterPanel.Visibility == Visibility.Visible)
                {
                    await jog.WriteJOGON_420(Wtr1JogOn.Text);
                }

                else if (AdmixPanel.Visibility == Visibility.Visible)
                {
                    await jog.WriteJOGON_240(Admix1JogOn.Text);
                }

                else if (SilicaPanel.Visibility == Visibility.Visible)
                {
                    await jog.WriteJOGON_416(SilicaJogOn.Text);
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
