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

namespace Scada_Demo.Views
{
    /// <summary>
    /// Interaction logic for Recipe_Details.xaml
    /// </summary>
    public partial class Recipe_Details : Window
    {
        public Recipe_Details()
        {
            InitializeComponent();
        }

        private void HideAll()
        {
            AggregatePanel.Visibility = Visibility.Collapsed;
            CementPanel.Visibility = Visibility.Collapsed;
            WaterPanel.Visibility = Visibility.Collapsed;
            AdmixPanel.Visibility = Visibility.Collapsed;
            SilicaPanel.Visibility = Visibility.Collapsed;
        }

        private void ShowAggregate(object sender, RoutedEventArgs e)
        {
            HideAll();
            AggregatePanel.Visibility = Visibility.Visible;
        }

        private void ShowCement(object sender, RoutedEventArgs e)
        {
            HideAll();
            CementPanel.Visibility = Visibility.Visible;
        }

        private void ShowWater(object sender, RoutedEventArgs e)
        {
            HideAll();
            WaterPanel.Visibility = Visibility.Visible;
        }

        private void ShowAdmix(object sender, RoutedEventArgs e)
        {
            HideAll();
            AdmixPanel.Visibility = Visibility.Visible;
        }

        private void ShowSilica(object sender, RoutedEventArgs e)
        {
            HideAll();
            SilicaPanel.Visibility = Visibility.Visible;
        }
    }
}
