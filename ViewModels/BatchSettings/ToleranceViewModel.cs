using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
using Scada_Demo.Database;
using Scada_Demo.MQTT_Model;
using Scada_Demo.Services;

namespace Scada_Demo.ViewModels.BatchSettings
{
    public class ToleranceViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        #endregion

        private readonly MqttPublishSerice _mqttPublishSerice;

        public ToleranceViewModel(MqttPublishSerice mqttPublishSerice)
        {
            _mqttPublishSerice = mqttPublishSerice;

            ShowAggregateCommand = new RelayCommand(_ => ShowOnly(
                Visibility.Visible,
                Visibility.Collapsed,
                Visibility.Collapsed,
                Visibility.Collapsed,
                Visibility.Collapsed));

            ShowCementCommand = new RelayCommand(_ => ShowOnly(
                Visibility.Collapsed,
                Visibility.Visible,
                Visibility.Collapsed,
                Visibility.Collapsed,
                Visibility.Collapsed));

            ShowWaterCommand = new RelayCommand(_ => ShowOnly(
                Visibility.Collapsed,
                Visibility.Collapsed,
                Visibility.Visible,
                Visibility.Collapsed,
                Visibility.Collapsed));

            ShowAdmixCommand = new RelayCommand(_ => ShowOnly(
                Visibility.Collapsed,
                Visibility.Collapsed,
                Visibility.Collapsed,
                Visibility.Visible,
                Visibility.Collapsed));

            ShowSilicaCommand = new RelayCommand(_ => ShowOnly(
                Visibility.Collapsed,
                Visibility.Collapsed,
                Visibility.Collapsed,
                Visibility.Collapsed,
                Visibility.Visible));

            ReadFromPlcCommand = new RelayCommand(_ => ReadFromPlc());
            WriteToPlcCommand = new RelayCommand(_ => WriteToPlc());
            ExitCommand = new RelayCommand(_ => Exit());

            LoadTolerance();
        }

        #region Aggregate

        private string _agg1;
        public string txtAgg1
        {
            get => _agg1;
            set => SetProperty(ref _agg1, value);
        }

        private string _agg2;
        public string txtAgg2
        {
            get => _agg2;
            set => SetProperty(ref _agg2, value);
        }

        private string _agg3;
        public string txtAgg3
        {
            get => _agg3;
            set => SetProperty(ref _agg3, value);
        }

        private string _agg4;
        public string txtAgg4
        {
            get => _agg4;
            set => SetProperty(ref _agg4, value);
        }

        private string _agg5;
        public string txtAgg5
        {
            get => _agg5;
            set => SetProperty(ref _agg5, value);
        }

        private string _agg6;
        public string txtAgg6
        {
            get => _agg6;
            set => SetProperty(ref _agg6, value);
        }

        #endregion

        #region Cement

        private string _c1;
        public string txtC1
        {
            get => _c1;
            set => SetProperty(ref _c1, value);
        }

        private string _c2;
        public string txtC2
        {
            get => _c2;
            set => SetProperty(ref _c2, value);
        }

        private string _c3;
        public string txtC3
        {
            get => _c3;
            set => SetProperty(ref _c3, value);
        }

        private string _c4;
        public string txtC4
        {
            get => _c4;
            set => SetProperty(ref _c4, value);
        }

        private string _c5;
        public string txtC5
        {
            get => _c5;
            set => SetProperty(ref _c5, value);
        }

        #endregion

        #region Water

        private string _wtr1;
        public string txtWTR1
        {
            get => _wtr1;
            set => SetProperty(ref _wtr1, value);
        }

        private string _wtr2;
        public string txtWTR2
        {
            get => _wtr2;
            set => SetProperty(ref _wtr2, value);
        }

        private string _wtr3;
        public string txtWTR3
        {
            get => _wtr3;
            set => SetProperty(ref _wtr3, value);
        }

        #endregion

        #region Admix

        private string _ad1;
        public string txtAD1
        {
            get => _ad1;
            set => SetProperty(ref _ad1, value);
        }

        private string _ad2;
        public string txtAD2
        {
            get => _ad2;
            set => SetProperty(ref _ad2, value);
        }

        private string _ad3;
        public string txtAD3
        {
            get => _ad3;
            set => SetProperty(ref _ad3, value);
        }

        private string _ad4;
        public string txtAD4
        {
            get => _ad4;
            set => SetProperty(ref _ad4, value);
        }

        #endregion

        #region Silica

        private string _ice;
        public string txtICE
        {
            get => _ice;
            set => SetProperty(ref _ice, value);
        }

        #endregion

        #region Tab / Panel switching

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

        #region PLC Actions

        private async Task ReadFromPlc()
        {
            try
            {
                var request = new
                {
                    Type = "Tolerance",
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
                var writeData = new Tolerance_Model();

                // Aggregate
                writeData.Agg1 = string.IsNullOrWhiteSpace(txtAgg1) ? (short)0 : Convert.ToInt16(txtAgg1);
                writeData.Agg2 = string.IsNullOrWhiteSpace(txtAgg2) ? (short)0 : Convert.ToInt16(txtAgg2);
                writeData.Agg3 = string.IsNullOrWhiteSpace(txtAgg3) ? (short)0 : Convert.ToInt16(txtAgg3);
                writeData.Agg4 = string.IsNullOrWhiteSpace(txtAgg4) ? (short)0 : Convert.ToInt16(txtAgg4);

                // Cement
                writeData.Cem1 = string.IsNullOrWhiteSpace(txtC1) ? (short)0 : Convert.ToInt16(txtC1);
                writeData.Cem4 = string.IsNullOrWhiteSpace(txtC4) ? (short)0 : Convert.ToInt16(txtC4);

                // Water
                writeData.Water = string.IsNullOrWhiteSpace(txtWTR1) ? (short)0 : Convert.ToInt16(txtWTR1);

                // Admix
                writeData.Adm1 = string.IsNullOrWhiteSpace(txtAD1)
           ? (short)0
           : Convert.ToInt16(txtAD1);

                // Silica
                writeData.Ice = string.IsNullOrWhiteSpace(txtICE) ? (short)0 : Convert.ToInt16(txtICE);

                var request = new
                {
                    Type = "Tolerance",
                    activity = "Write",
                    batchSettings_Tolerance = writeData
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
                if (data.Type == "Tolerance")
                {
                    if (data.activity == "Read")
                    {
                        // Aggregate
                        txtAgg1 = data.batchSettings_Tolerance.Agg1.ToString();
                        txtAgg2 = data.batchSettings_Tolerance.Agg2.ToString();
                        txtAgg3 = data.batchSettings_Tolerance.Agg3.ToString();
                        txtAgg4 = data.batchSettings_Tolerance.Agg4.ToString();

                        // Cement
                        txtC1 = data.batchSettings_Tolerance.Cem1.ToString();
                        txtC2 = data.batchSettings_Tolerance.Cem1.ToString();
                        txtC3 = data.batchSettings_Tolerance.Cem1.ToString();
                        txtC4 = data.batchSettings_Tolerance.Cem4.ToString();

                        // Water
                        txtWTR1 = data.batchSettings_Tolerance.Water.ToString();

                        // Admix
                        txtAD1 = data.batchSettings_Tolerance.Adm1.ToString();
                        txtAD2 = data.batchSettings_Tolerance.Adm1.ToString();

                        // Silica
                        txtICE = data.batchSettings_Tolerance.Ice.ToString();
                    }
                    else
                    {
                        if (
                            Convert.ToInt16(txtAgg1) == data.batchSettings_Tolerance.Agg1 &&
                            Convert.ToInt16(txtAgg2) == data.batchSettings_Tolerance.Agg2 &&
                            Convert.ToInt16(txtAgg3) == data.batchSettings_Tolerance.Agg3 &&
                            Convert.ToInt16(txtAgg4) == data.batchSettings_Tolerance.Agg4 &&
                            Convert.ToInt16(txtC1) == data.batchSettings_Tolerance.Cem1 &&
                            Convert.ToInt16(txtC4) == data.batchSettings_Tolerance.Cem4 &&
                            Convert.ToInt16(txtWTR1) == data.batchSettings_Tolerance.Water &&
                            Convert.ToDecimal(txtAD1) == data.batchSettings_Tolerance.Adm1 &&
                              Convert.ToDecimal(txtAD2) == data.batchSettings_Tolerance.Adm1 &&
                            Convert.ToInt16(txtICE) == data.batchSettings_Tolerance.Ice
                            )
                        {
                            SaveTolerance();

                            MessageBox.Show("Data Saved Successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Write Failed.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveTolerance()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM Tolerance_Setup)
BEGIN
    UPDATE Tolerance_Setup
    SET
        Gate1=@Gate1,
        Gate2=@Gate2,
        Gate3=@Gate3,
        Gate4=@Gate4,
        Cement1=@Cement1,
        Cement4=@Cement4,
        Water=@Water,
        Admix1=@Admix1,
        Silica=@Silica
END
ELSE
BEGIN
    INSERT INTO Tolerance_Setup
    (
        Gate1,Gate2,Gate3,Gate4,
        Cement1,Cement4,
        Water,
        Admix1,
        Silica
    )
    VALUES
    (
        @Gate1,@Gate2,@Gate3,@Gate4,
        @Cement1,@Cement4,
        @Water,
        @Admix1,
        @Silica
    )
END", con);

                    cmd.Parameters.AddWithValue("@Gate1", string.IsNullOrWhiteSpace(txtAgg1) ? (object)DBNull.Value : Convert.ToInt32(txtAgg1));
                    cmd.Parameters.AddWithValue("@Gate2", string.IsNullOrWhiteSpace(txtAgg2) ? (object)DBNull.Value : Convert.ToInt32(txtAgg2));
                    cmd.Parameters.AddWithValue("@Gate3", string.IsNullOrWhiteSpace(txtAgg3) ? (object)DBNull.Value : Convert.ToInt32(txtAgg3));
                    cmd.Parameters.AddWithValue("@Gate4", string.IsNullOrWhiteSpace(txtAgg4) ? (object)DBNull.Value : Convert.ToInt32(txtAgg4));

                    cmd.Parameters.AddWithValue("@Cement1", string.IsNullOrWhiteSpace(txtC1) ? (object)DBNull.Value : Convert.ToInt32(txtC1));
                    cmd.Parameters.AddWithValue("@Cement4", string.IsNullOrWhiteSpace(txtC4) ? (object)DBNull.Value : Convert.ToInt32(txtC4));

                    cmd.Parameters.AddWithValue("@Water", string.IsNullOrWhiteSpace(txtWTR1) ? (object)DBNull.Value : Convert.ToInt32(txtWTR1));

                    cmd.Parameters.AddWithValue("@Admix1", string.IsNullOrWhiteSpace(txtAD1) ? (object)DBNull.Value : Convert.ToDecimal(txtAD1));

                    cmd.Parameters.AddWithValue("@Silica", string.IsNullOrWhiteSpace(txtICE) ? (object)DBNull.Value : Convert.ToInt32(txtICE));

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadTolerance()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(
                        "SELECT TOP 1 * FROM Tolerance_Setup", con);

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        txtAgg1 = dr["Gate1"]?.ToString();
                        txtAgg2 = dr["Gate2"]?.ToString();
                        txtAgg3 = dr["Gate3"]?.ToString();
                        txtAgg4 = dr["Gate4"]?.ToString();

                        txtC1 = dr["Cement1"]?.ToString();
                        txtC2 = dr["Cement1"]?.ToString();
                        txtC3 = dr["Cement1"]?.ToString();
                        txtC4 = dr["Cement4"]?.ToString();

                        txtWTR1 = dr["Water"]?.ToString();

                        txtAD1 = dr["Admix1"]?.ToString();

                        txtICE = dr["Silica"]?.ToString();
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}