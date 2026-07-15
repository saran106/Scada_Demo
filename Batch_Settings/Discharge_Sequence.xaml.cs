using System.Windows;
using Scada_Demo.ViewModels.BatchSettings;

namespace Scada_Demo.Batch_Settings
{
    public partial class Discharge_Sequence : Window
    {
        private DischargeDelayViewModel vm;

        public Discharge_Sequence()
        {
            InitializeComponent();

            vm = App.DDVM;
            DataContext = vm;
        }
    }
}