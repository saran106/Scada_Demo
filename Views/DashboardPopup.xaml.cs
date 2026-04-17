using System.Windows;

namespace Scada_Demo
{
    public partial class DashboardPopup : Window
    {
        public DashboardPopup()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}