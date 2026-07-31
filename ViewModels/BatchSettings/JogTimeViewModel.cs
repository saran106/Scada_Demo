using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
using Scada_Demo.MQTT_Model;
using Scada_Demo.Services;
using Scada_Demo.Database;
using Scada_Demo.ViewModels.BatchSettings;

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

        private string _selectedTab = "Aggregate";

        public string SelectedTab
        {
            get => _selectedTab;
            set => SetProperty(ref _selectedTab, value);
        }
        public JogTimeViewModel(MqttPublishSerice mqttPublishSerice)
        {
            _mqttPublishSerice = mqttPublishSerice;



            // Aggregate
            ShowAggregateCommand = new RelayCommand(_ =>
            {
                SelectedTab = "Aggregate";

                ShowOnly(
                    Visibility.Visible,
                    Visibility.Collapsed,
                    Visibility.Collapsed,
                    Visibility.Collapsed,
                    Visibility.Collapsed);
            });

            // Cement
            ShowCementCommand = new RelayCommand(_ =>
            {
                SelectedTab = "Cement";

                ShowOnly(
                    Visibility.Collapsed,
                    Visibility.Visible,
                    Visibility.Collapsed,
                    Visibility.Collapsed,
                    Visibility.Collapsed);
            });

            // Water
            ShowWaterCommand = new RelayCommand(_ =>
            {
                SelectedTab = "Water";

                ShowOnly(
                    Visibility.Collapsed,
                    Visibility.Collapsed,
                    Visibility.Visible,
                    Visibility.Collapsed,
                    Visibility.Collapsed);
            });

            // Admixture
            ShowAdmixCommand = new RelayCommand(_ =>
            {
                SelectedTab = "Admixture";

                ShowOnly(
                    Visibility.Collapsed,
                    Visibility.Collapsed,
                    Visibility.Collapsed,
                    Visibility.Visible,
                    Visibility.Collapsed);
            });

            // Micro Silica
            ShowSilicaCommand = new RelayCommand(_ =>
            {
                SelectedTab = "Micro Silica";

                ShowOnly(
                    Visibility.Collapsed,
                    Visibility.Collapsed,
                    Visibility.Collapsed,
                    Visibility.Collapsed,
                    Visibility.Visible);
            });

            ReadFromPlcCommand = new RelayCommand(_ => ReadFromPlc());
            WriteToPlcCommand = new RelayCommand(_ => WriteToPlc());
            ExitCommand = new RelayCommand(_ => Exit());

            // Default Tab
            SelectedTab = "Aggregate";

            LoadJogTime();
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

        private void LoadJogTime()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(
                        "SELECT TOP 1 * FROM JogTime_Setup", con);

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        Agg1JogOn = dr["Agg1_On"]?.ToString();
                        Agg2JogOn = dr["Agg2_On"]?.ToString();
                        Agg3JogOn = dr["Agg3_On"]?.ToString();
                        Agg4JogOn = dr["Agg4_On"]?.ToString();

                        CemJogOn = dr["Cement_On"]?.ToString();

                        WaterJogOn = dr["Water_On"]?.ToString();

                        AdmixJogOn = dr["Admix_On"]?.ToString();

                        SilicaJogOn = dr["Silica_On"]?.ToString();

                        Agg1JogOff = dr["Agg1_Off"]?.ToString();
                        Agg2JogOff = dr["Agg2_Off"]?.ToString();
                        Agg3JogOff = dr["Agg3_Off"]?.ToString();
                        Agg4JogOff = dr["Agg4_Off"]?.ToString();
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveJogTime()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM JogTime_Setup)
BEGIN
    UPDATE JogTime_Setup
    SET
        Agg1_On=@Agg1_On,
        Agg2_On=@Agg2_On,
        Agg3_On=@Agg3_On,
        Agg4_On=@Agg4_On,

        Cement_On=@Cement_On,

        Water_On=@Water_On,

        Admix_On=@Admix_On,

        Silica_On=@Silica_On,

        Agg1_Off=@Agg1_Off,
        Agg2_Off=@Agg2_Off,
        Agg3_Off=@Agg3_Off,
        Agg4_Off=@Agg4_Off
END
ELSE
BEGIN
    INSERT INTO JogTime_Setup
    (
        Agg1_On,
        Agg2_On,
        Agg3_On,
        Agg4_On,

        Cement_On,

        Water_On,

        Admix_On,

        Silica_On,

        Agg1_Off,
        Agg2_Off,
        Agg3_Off,
        Agg4_Off
    )
    VALUES
    (
        @Agg1_On,
        @Agg2_On,
        @Agg3_On,
        @Agg4_On,

        @Cement_On,

        @Water_On,

        @Admix_On,

        @Silica_On,

        @Agg1_Off,
        @Agg2_Off,
        @Agg3_Off,
        @Agg4_Off
    )
END", con);

                    cmd.Parameters.AddWithValue("@Agg1_On",
                        string.IsNullOrWhiteSpace(Agg1JogOn) ? (object)DBNull.Value : Convert.ToInt32(Agg1JogOn));

                    cmd.Parameters.AddWithValue("@Agg2_On",
                        string.IsNullOrWhiteSpace(Agg2JogOn) ? (object)DBNull.Value : Convert.ToInt32(Agg2JogOn));

                    cmd.Parameters.AddWithValue("@Agg3_On",
                        string.IsNullOrWhiteSpace(Agg3JogOn) ? (object)DBNull.Value : Convert.ToInt32(Agg3JogOn));

                    cmd.Parameters.AddWithValue("@Agg4_On",
                        string.IsNullOrWhiteSpace(Agg4JogOn) ? (object)DBNull.Value : Convert.ToInt32(Agg4JogOn));

                    cmd.Parameters.AddWithValue("@Cement_On",
                        string.IsNullOrWhiteSpace(CemJogOn) ? (object)DBNull.Value : Convert.ToInt32(CemJogOn));

                    cmd.Parameters.AddWithValue("@Water_On",
                        string.IsNullOrWhiteSpace(WaterJogOn) ? (object)DBNull.Value : Convert.ToInt32(WaterJogOn));

                    cmd.Parameters.AddWithValue("@Admix_On",
                        string.IsNullOrWhiteSpace(AdmixJogOn) ? (object)DBNull.Value : Convert.ToInt32(AdmixJogOn));

                    cmd.Parameters.AddWithValue("@Silica_On",
                        string.IsNullOrWhiteSpace(SilicaJogOn) ? (object)DBNull.Value : Convert.ToInt32(SilicaJogOn));

                    cmd.Parameters.AddWithValue("@Agg1_Off",
                        string.IsNullOrWhiteSpace(Agg1JogOff) ? (object)DBNull.Value : Convert.ToInt32(Agg1JogOff));

                    cmd.Parameters.AddWithValue("@Agg2_Off",
                        string.IsNullOrWhiteSpace(Agg2JogOff) ? (object)DBNull.Value : Convert.ToInt32(Agg2JogOff));

                    cmd.Parameters.AddWithValue("@Agg3_Off",
                        string.IsNullOrWhiteSpace(Agg3JogOff) ? (object)DBNull.Value : Convert.ToInt32(Agg3JogOff));

                    cmd.Parameters.AddWithValue("@Agg4_Off",
                        string.IsNullOrWhiteSpace(Agg4JogOff) ? (object)DBNull.Value : Convert.ToInt32(Agg4JogOff));

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
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
                        SaveJogTime();
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