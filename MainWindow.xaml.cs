using System.Windows;
using Scada_Demo.ViewModels;

namespace Scada_Demo
{
    public partial class MainWindow : Window
    {
        private PlantViewModel _viewModel;

        public MainWindow()
            {
            InitializeComponent();

            // Set ViewModel as DataContext
            _viewModel = new PlantViewModel();
            DataContext = _viewModel;

            // Start PLC simulation
            _ = _viewModel.StartPlcSimulation();
        }


    }
}