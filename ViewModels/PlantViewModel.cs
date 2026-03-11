using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Scada_Demo.ViewModels
{
    public class PlantViewModel : INotifyPropertyChanged
    {
        // Aggregate Bins
        private int _agg1Level = 1200;
        private int _agg2Level = 900;
        private int _agg3Level = 1500;
        private int _agg4Level = 600;

        // Cement Silos
        private int _cement1Level = 4500;
        private int _cement2Level = 3200;

        // Water & Admixture
        private int _waterLevel = 800;
        private int _admixture1Level = 200;

        // Weigh Hopper
        private int _weighHopperValue = 2450;

        // Mixer Status
        private string _mixerGateStatus = "CLOSED";
        private string _mixerStatus = "RUNNING";
        private string _batchStatus = "RUNNING";

        // Status Lights
        private bool _isRunLight = true;
        private bool _isStopLight = false;
        private bool _isFaultLight = false;

        // Properties with INotifyPropertyChanged
        public int Agg1Level
        {
            get => _agg1Level;
            set { _agg1Level = value; OnPropertyChanged(); }
        }

        public int Agg2Level
        {
            get => _agg2Level;
            set { _agg2Level = value; OnPropertyChanged(); }
        }

        public int Agg3Level
        {
            get => _agg3Level;
            set { _agg3Level = value; OnPropertyChanged(); }
        }

        public int Agg4Level
        {
            get => _agg4Level;
            set { _agg4Level = value; OnPropertyChanged(); }
        }

        public int Cement1Level
        {
            get => _cement1Level;
            set { _cement1Level = value; OnPropertyChanged(); }
        }

        public int Cement2Level
        {
            get => _cement2Level;
            set { _cement2Level = value; OnPropertyChanged(); }
        }

        public int WaterLevel
        {
            get => _waterLevel;
            set { _waterLevel = value; OnPropertyChanged(); }
        }

        public int Admixture1Level
        {
            get => _admixture1Level;
            set { _admixture1Level = value; OnPropertyChanged(); }
        }

        public int WeighHopperValue
        {
            get => _weighHopperValue;
            set { _weighHopperValue = value; OnPropertyChanged(); }
        }

        public string MixerGateStatus
        {
            get => _mixerGateStatus;
            set { _mixerGateStatus = value; OnPropertyChanged(); }
        }

        public string MixerStatus
        {
            get => _mixerStatus;
            set { _mixerStatus = value; OnPropertyChanged(); }
        }

        public string BatchStatus
        {
            get => _batchStatus;
            set { _batchStatus = value; OnPropertyChanged(); }
        }

        public bool IsRunLight
        {
            get => _isRunLight;
            set { _isRunLight = value; OnPropertyChanged(); }
        }

        public bool IsStopLight
        {
            get => _isStopLight;
            set { _isStopLight = value; OnPropertyChanged(); }
        }

        public bool IsFaultLight
        {
            get => _isFaultLight;
            set { _isFaultLight = value; OnPropertyChanged(); }
        }

        // Command for toggling gate
        public ICommand ToggleGateCommand { get; private set; }

        // Constructor
        public PlantViewModel()
        {
            ToggleGateCommand = new RelayCommand(ToggleGate);
        }

        // Toggle gate method
        private void ToggleGate(object parameter)
        {
            MixerGateStatus = MixerGateStatus == "CLOSED" ? "OPEN" : "CLOSED";
        }

        // PLC Communication Simulator
        public async Task StartPlcSimulation()
        {
            var random = new Random();

            while (true)
            {
                await Task.Delay(1000); // Update every second

                // Simulate PLC data changes
                Agg1Level = random.Next(800, 1200);
                Agg2Level = random.Next(600, 900);
                Agg3Level = random.Next(1000, 1500);
                Agg4Level = random.Next(400, 600);
                Cement1Level = random.Next(4000, 4500);
                Cement2Level = random.Next(2800, 3200);
                WaterLevel = random.Next(600, 800);
                Admixture1Level = random.Next(150, 250);
                WeighHopperValue = random.Next(2000, 2500);

                // Toggle status randomly
                IsRunLight = !IsRunLight;
                IsStopLight = !IsRunLight;
                IsFaultLight = random.Next(0, 10) == 1; // 10% chance of fault

                MixerGateStatus = random.Next(0, 2) == 0 ? "CLOSED" : "OPEN";
                BatchStatus = IsRunLight ? "RUNNING" : "IDLE";
            }
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    // RelayCommand class for ICommand implementation
    public class RelayCommand : ICommand
    {
        private Action<object> _execute;
        private Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}