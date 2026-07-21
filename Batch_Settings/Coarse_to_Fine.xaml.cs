using System.Windows;
using Scada_Demo.ViewModels.BatchSettings;

namespace Scada_Demo.Batch_Settings
{
    public partial class Coarse_to_Fine : Window
    {
        private CoarseToFineViewModel vm;

        public Coarse_to_Fine()
        {
            InitializeComponent();

            vm = App.CoarseToFineVM;
            DataContext = vm;
        }
    }
}