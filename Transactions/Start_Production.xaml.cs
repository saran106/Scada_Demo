using System.Windows;
using Scada_Demo.ViewModels.Transactions;
using Scada_Demo.ViewModels.User;

namespace Scada_Demo.Transactions
{
    public partial class Start_Production : Window
    {
        private Start_ProductionViewModel vm;

        public Start_Production()
        {
            InitializeComponent();

            //vm = App.StartProductionVM;
            //DataContext = vm;

            DataContext = new Start_ProductionViewModel();
        }
    }
}