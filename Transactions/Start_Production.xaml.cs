using System;
using System.Windows;
using Scada_Demo.ViewModels.Transactions;

namespace Scada_Demo.Transactions
{
    public partial class Start_Production : Window
    {
        private readonly Start_ProductionViewModel vm;

        public Start_Production()
        {
            InitializeComponent();

            vm = App.StartProductionVM;
            DataContext = vm;

            // Reset when closed using the title-bar X or Exit button.
            Closed += Start_Production_Closed;
        }

        private void Start_Production_Closed(object? sender, EventArgs e)
        {
            vm.ResetForm();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
