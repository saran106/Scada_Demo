using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using Scada_Demo.MQTT_Model;
using Scada_Demo.Services;

namespace Scada_Demo.ViewModels.BatchSettings
{
    public class JogTimeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected bool SetProperty<T>(ref T field, T value,
            [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));

            return true;
        }

        private readonly MqttPublishSerice _mqttPublishSerice;

        public JogTimeViewModel(MqttPublishSerice mqttPublishSerice)
        {
            _mqttPublishSerice = mqttPublishSerice;

            ShowAggregateCommand = new RelayCommand(_ =>
                ShowOnly(Visibility.Visible,
                         Visibility.Collapsed,
                         Visibility.Collapsed,
                         Visibility.Collapsed,
                         Visibility.Collapsed));

            ShowCementCommand = new RelayCommand(_ =>
                ShowOnly(Visibility.Collapsed,
                         Visibility.Visible,
                         Visibility.Collapsed,
                         Visibility.Collapsed,
                         Visibility.Collapsed));

            ShowWaterCommand = new RelayCommand(_ =>
                ShowOnly(Visibility.Collapsed,
                         Visibility.Collapsed,
                         Visibility.Visible,
                         Visibility.Collapsed,
                         Visibility.Collapsed));

            ShowAdmixCommand = new RelayCommand(_ =>
                ShowOnly(Visibility.Collapsed,
                         Visibility.Collapsed,
                         Visibility.Collapsed,
                         Visibility.Visible,
                         Visibility.Collapsed));

            ShowSilicaCommand = new RelayCommand(_ =>
                ShowOnly(Visibility.Collapsed,
                         Visibility.Collapsed,
                         Visibility.Collapsed,
                         Visibility.Collapsed,
                         Visibility.Visible));

            ReadFromPlcCommand = new RelayCommand(_ => ReadFromPlc());
            WriteToPlcCommand = new RelayCommand(_ => WriteToPlc());
            ExitCommand = new RelayCommand(_ => Exit());
        }

        #region Commands

        public ICommand ShowAggregateCommand { get; }
        public ICommand ShowCementCommand { get; }
        public ICommand ShowWaterCommand { get; }
        public ICommand ShowAdmixCommand { get; }
        public ICommand ShowSilicaCommand { get; }

        public ICommand ReadFromPlcCommand { get; }
        public ICommand WriteToPlcCommand { get; }
        public ICommand ExitCommand { get; }

        #endregion


        #region Aggregate Jog ON

        private string _agg1JogOn;
        public string Agg1JogOn
        {
            get => _agg1JogOn;
            set => SetProperty(ref _agg1JogOn, value);
        }

        private string _agg2JogOn;
        public string Agg2JogOn
        {
            get => _agg2JogOn;
            set => SetProperty(ref _agg2JogOn, value);
        }

        private string _agg3JogOn;
        public string Agg3JogOn
        {
            get => _agg3JogOn;
            set => SetProperty(ref _agg3JogOn, value);
        }

        private string _agg4JogOn;
        public string Agg4JogOn
        {
            get => _agg4JogOn;
            set => SetProperty(ref _agg4JogOn, value);
        }

        #endregion

        #region Aggregate Jog OFF

        private string _agg1JogOff;
        public string Agg1JogOff
        {
            get => _agg1JogOff;
            set => SetProperty(ref _agg1JogOff, value);
        }

        private string _agg2JogOff;
        public string Agg2JogOff
        {
            get => _agg2JogOff;
            set => SetProperty(ref _agg2JogOff, value);
        }

        private string _agg3JogOff;
        public string Agg3JogOff
        {
            get => _agg3JogOff;
            set => SetProperty(ref _agg3JogOff, value);
        }

        private string _agg4JogOff;
        public string Agg4JogOff
        {
            get => _agg4JogOff;
            set => SetProperty(ref _agg4JogOff, value);
        }

        #endregion

        #region Cement

        private string _cemJogOn;
        public string CemJogOn
        {
            get => _cemJogOn;
            set => SetProperty(ref _cemJogOn, value);
        }

        #endregion

        #region Water

        private string _waterJogOn;
        public string WaterJogOn
        {
            get => _waterJogOn;
            set => SetProperty(ref _waterJogOn, value);
        }

        #endregion

        #region Admix

        private string _admixJogOn;
        public string AdmixJogOn
        {
            get => _admixJogOn;
            set => SetProperty(ref _admixJogOn, value);
        }

        #endregion

        #region Silica

        private string _silicaJogOn;
        public string SilicaJogOn
        {
            get => _silicaJogOn;
            set => SetProperty(ref _silicaJogOn, value);
        }

        #endregion

        #region Panel Visibility

        private Visibility _aggregateVisibility = Visibility.Visible;
        public Visibility AggregateVisibility
        {
            get => _aggregateVisibility;
            set => SetProperty(ref _aggregateVisibility, value);
        }

        private Visibility _cementVisibility = Visibility.Collapsed;
        public Visibility CementVisibility
        {
            get => _cementVisibility;
            set => SetProperty(ref _cementVisibility, value);
        }

        private Visibility _waterVisibility = Visibility.Collapsed;
        public Visibility WaterVisibility
        {
            get => _waterVisibility;
            set => SetProperty(ref _waterVisibility, value);
        }

        private Visibility _admixVisibility = Visibility.Collapsed;
        public Visibility AdmixVisibility
        {
            get => _admixVisibility;
            set => SetProperty(ref _admixVisibility, value);
        }

        private Visibility _silicaVisibility = Visibility.Collapsed;
        public Visibility SilicaVisibility
        {
            get => _silicaVisibility;
            set => SetProperty(ref _silicaVisibility, value);
        }

        private void ShowOnly(
            Visibility agg,
            Visibility cem,
            Visibility water,
            Visibility admix,
            Visibility silica)
        {
            AggregateVisibility = agg;
            CementVisibility = cem;
            WaterVisibility = water;
            AdmixVisibility = admix;
            SilicaVisibility = silica;
        }

        #endregion


        #region PLC Actions

        private async Task ReadFromPlc()
        {
            try
            {
                var request = new
                {
                    Type = "JogTime",
                    activity = "Read"
                };

                await _mqttPublishSerice.PublishDataMqttTopic(
                    "BatchSettings/Request",
                    JsonSerializer.Serialize(request));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task WriteToPlc()
        {
            try
            {
                var jog = new JogTime_Model
                {
                    // Aggregate Jog ON
                    Agg1 = string.IsNullOrWhiteSpace(Agg1JogOn) ? (short)0 : Convert.ToInt16(Agg1JogOn),
                    Agg2 = string.IsNullOrWhiteSpace(Agg2JogOn) ? (short)0 : Convert.ToInt16(Agg2JogOn),
                    Agg3 = string.IsNullOrWhiteSpace(Agg3JogOn) ? (short)0 : Convert.ToInt16(Agg3JogOn),
                    Agg4 = string.IsNullOrWhiteSpace(Agg4JogOn) ? (short)0 : Convert.ToInt16(Agg4JogOn),

                    // Aggregate Jog OFF
                    Agg1Off = string.IsNullOrWhiteSpace(Agg1JogOff) ? (short)0 : Convert.ToInt16(Agg1JogOff),
                    Agg2Off = string.IsNullOrWhiteSpace(Agg2JogOff) ? (short)0 : Convert.ToInt16(Agg2JogOff),
                    Agg3Off = string.IsNullOrWhiteSpace(Agg3JogOff) ? (short)0 : Convert.ToInt16(Agg3JogOff),
                    Agg4Off = string.IsNullOrWhiteSpace(Agg4JogOff) ? (short)0 : Convert.ToInt16(Agg4JogOff),

                    // Cement
                    Cem = string.IsNullOrWhiteSpace(CemJogOn) ? (short)0 : Convert.ToInt16(CemJogOn),

                    // Water
                    Water = string.IsNullOrWhiteSpace(WaterJogOn) ? 0 : Convert.ToInt32(WaterJogOn),

                    // Admix
                    Admix = string.IsNullOrWhiteSpace(AdmixJogOn) ? 0 : Convert.ToInt32(AdmixJogOn),

                    // Ice / Silica
                    Ice = string.IsNullOrWhiteSpace(SilicaJogOn) ? 0 : Convert.ToInt32(SilicaJogOn)
                };

                var request = new
                {
                    Type = "JogTime",
                    activity = "Write",
                    batchSettings_JogSettings = jog
                };

                await _mqttPublishSerice.PublishDataMqttTopic(
                    "BatchSettings/Request",
                    JsonSerializer.Serialize(request));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Exit()
        {
            Application.Current?.Windows[0]?.Close();
        }

        #endregion

        public void MqttReadSuccessStatus(BatchSettingsModel data)
        {
            try
            {
                if (data.Type != "JogTime")
                    return;

                if (data.activity == "Read")
                {
                    Agg1JogOn = data.batchSettings_JogSettings.Agg1.ToString();
                    Agg2JogOn = data.batchSettings_JogSettings.Agg2.ToString();
                    Agg3JogOn = data.batchSettings_JogSettings.Agg3.ToString();
                    Agg4JogOn = data.batchSettings_JogSettings.Agg4.ToString();

                    Agg1JogOff = data.batchSettings_JogSettings.Agg1Off.ToString();
                    Agg2JogOff = data.batchSettings_JogSettings.Agg2Off.ToString();
                    Agg3JogOff = data.batchSettings_JogSettings.Agg3Off.ToString();
                    Agg4JogOff = data.batchSettings_JogSettings.Agg4Off.ToString();

                    CemJogOn = data.batchSettings_JogSettings.Cem.ToString();

                    WaterJogOn = data.batchSettings_JogSettings.Water.ToString();

                    AdmixJogOn = data.batchSettings_JogSettings.Admix.ToString();

                    SilicaJogOn = data.batchSettings_JogSettings.Ice.ToString();
                }
                else
                {
                    if (
                        Convert.ToInt16(Agg1JogOn) == data.batchSettings_JogSettings.Agg1 &&
                        Convert.ToInt16(Agg2JogOn) == data.batchSettings_JogSettings.Agg2 &&
                        Convert.ToInt16(Agg3JogOn) == data.batchSettings_JogSettings.Agg3 &&
                        Convert.ToInt16(Agg4JogOn) == data.batchSettings_JogSettings.Agg4 &&

                        Convert.ToInt16(Agg1JogOff) == data.batchSettings_JogSettings.Agg1Off &&
                        Convert.ToInt16(Agg2JogOff) == data.batchSettings_JogSettings.Agg2Off &&
                        Convert.ToInt16(Agg3JogOff) == data.batchSettings_JogSettings.Agg3Off &&
                        Convert.ToInt16(Agg4JogOff) == data.batchSettings_JogSettings.Agg4Off &&

                        Convert.ToInt16(CemJogOn) == data.batchSettings_JogSettings.Cem &&
                        Convert.ToInt32(WaterJogOn) == data.batchSettings_JogSettings.Water &&
                        Convert.ToInt32(AdmixJogOn) == data.batchSettings_JogSettings.Admix &&
                        Convert.ToInt32(SilicaJogOn) == data.batchSettings_JogSettings.Ice
                        )
                    {
                        MessageBox.Show("Data Saved Successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Write Failed.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}