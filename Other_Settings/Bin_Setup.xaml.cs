using System.Windows;
using Scada_Demo.ViewModels.Other_Settings;

namespace Scada_Demo.Other_Settings
{
    public partial class Bin_Setup : Window
    {
        private readonly Bin_SetupViewModel vm;

        public Bin_Setup()
        {
            InitializeComponent();

            vm = App.BinSetupVM;

            DataContext = vm;

            // Every time Bin Setup page opens
            vm.LoadBinSetup();
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}