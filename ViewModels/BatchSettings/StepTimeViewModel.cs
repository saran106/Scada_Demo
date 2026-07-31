using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using Scada_Demo.Services;
using Scada_Demo.MQTT_Model;
using Microsoft.Data.SqlClient;
using Scada_Demo.Database;

namespace Scada_Demo.ViewModels.BatchSettings
{
    public class StepTimeViewModel : INotifyPropertyChanged
    {


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected bool SetProperty<T>(ref T field, T value,
            [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));

            return true;
        }

        #endregion

        private readonly MqttPublishSerice _mqttPublishSerice;

        private string _selectedTab = "Aggregate";

        public string SelectedTab
        {
            get => _selectedTab;
            set => SetProperty(ref _selectedTab, value);
        }

        public StepTimeViewModel(MqttPublishSerice mqttPublishSerice)
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

            // Default Selected Tab
            SelectedTab = "Aggregate";

            LoadStepTime();
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

        #region Admixture

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

        #region Micro Silica

        private string _silica;
        public string txtSilica
        {
            get => _silica;
            set => SetProperty(ref _silica, value);
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
            Visibility aggregate,
            Visibility cement,
            Visibility water,
            Visibility admix,
            Visibility silica)
        {
            AggregateVisibility = aggregate;
            CementVisibility = cement;
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
                    Type = "StepTime",
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
                Step_Time_Model writeData = new();

                // Aggregate
                writeData.Step136 = Convert.ToInt16(txtAgg1);

                // Admix
                writeData.Step140 = Convert.ToInt16(txtAD1);

                // Cement
                writeData.Step548 = Convert.ToInt32(txtC1);
                writeData.Step552 = Convert.ToInt32(txtC4);

                var request = new
                {
                    Type = "StepTime",
                    activity = "Write",
                    batchSettings_StepTime = writeData
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

        public void MqttReadSuccessStatus(BatchSettingsModel data)
        {
            try
            {
                if (data.Type != "StepTime")
                    return;

                if (data.activity == "Read")
                {
                    // Aggregate
                    txtAgg1 = data.batchSettings_StepTime.Step136.ToString();
                    txtAgg2 = data.batchSettings_StepTime.Step136.ToString();
                    txtAgg3 = data.batchSettings_StepTime.Step136.ToString();
                    txtAgg4 = data.batchSettings_StepTime.Step136.ToString();
                    txtAgg5 = data.batchSettings_StepTime.Step136.ToString();
                    txtAgg6 = data.batchSettings_StepTime.Step136.ToString();

                    // Cement
                    txtC1 = data.batchSettings_StepTime.Step548.ToString();
                    //txtC2 = data.batchSettings_StepTime.Step548.ToString();
                   // txtC3 = data.batchSettings_StepTime.Step548.ToString();
                    txtC4 = data.batchSettings_StepTime.Step552.ToString();
                   // txtC5 = data.batchSettings_StepTime.Step552.ToString();

                    // Water
                    txtWTR1 = "";
                    txtWTR2 = "";
                    txtWTR3 = "";

                    // Admix
                    txtAD1 = data.batchSettings_StepTime.Step140.ToString();
                    txtAD2 = data.batchSettings_StepTime.Step140.ToString();
                   // txtAD3 = data.batchSettings_StepTime.Step140.ToString();
                   // txtAD4 = data.batchSettings_StepTime.Step140.ToString();

                    // Silica
                    txtSilica = "";
                }
                else
                {
                    if (
                        Convert.ToInt16(txtAgg1) == data.batchSettings_StepTime.Step136 &&
                        Convert.ToInt16(txtAgg2) == data.batchSettings_StepTime.Step136 &&
                        Convert.ToInt16(txtAgg3) == data.batchSettings_StepTime.Step136 &&
                        Convert.ToInt16(txtAgg4) == data.batchSettings_StepTime.Step136 &&
                        Convert.ToInt16(txtAgg5) == data.batchSettings_StepTime.Step136 &&
                        Convert.ToInt16(txtAgg6) == data.batchSettings_StepTime.Step136 &&
                        Convert.ToInt16(txtAD1) == data.batchSettings_StepTime.Step140 &&
                        Convert.ToInt16(txtAD2) == data.batchSettings_StepTime.Step140 &&
                        Convert.ToInt32(txtC1) == data.batchSettings_StepTime.Step548 &&
                        Convert.ToInt32(txtC4) == data.batchSettings_StepTime.Step552
                        )
                    {
                        SaveStepTime();

                        MessageBox.Show("Data Saved Successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Failed..! Try Again.");
                    }
                }
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

        private void SaveStepTime()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM StepTime_Setup)
BEGIN
    UPDATE StepTime_Setup
    SET
        Gate1 = @Gate1,
        Gate2 = @Gate2,
        Gate3 = @Gate3,
        Gate4 = @Gate4,
        Gate5 = @Gate5,
        Gate6 = @Gate6,

        Cement1 = @Cement1,
        --Cement2 = @Cement2,
        --Cement3 = @Cement3,
        Cement4 = @Cement4,
       -- Cement5 = @Cement5,

        --Water1 = @Water1,
        --Water2 = @Water2,
        --Water3 = @Water3,

        Admix1 = @Admix1,
        Admix2 = @Admix2
        --Admix3 = @Admix3,
        --Admix4 = @Admix4,

        --Silica = @Silica
END
ELSE
BEGIN
    INSERT INTO StepTime_Setup
    (
        Gate1,
Gate2,
Gate3,
Gate4,
Gate5,
Gate6,
Cement1,
--Cement2,
--Cement3,
Cement4,
--Cement5,
        
--Water1,
--Water2,
--Water3,
        Admix1,Admix2
--Admix3,Admix4,Silica
    )
    VALUES
    (
        @Gate1,@Gate2,@Gate3,@Gate4,@Gate5,@Gate6,
        @Cement1,
--@Cement2,@Cement3,
@Cement4,
--@Cement5,
        --@Water1,@Water2,@Water3,
        @Admix1,@Admix2
--,@Admix3,@Admix4, @Silica
    )
END", con);

                    // Aggregate
                    cmd.Parameters.AddWithValue("@Gate1", string.IsNullOrWhiteSpace(txtAgg1) ? (object)DBNull.Value : Convert.ToInt32(txtAgg1));
                    cmd.Parameters.AddWithValue("@Gate2", string.IsNullOrWhiteSpace(txtAgg2) ? (object)DBNull.Value : Convert.ToInt32(txtAgg2));
                    cmd.Parameters.AddWithValue("@Gate3", string.IsNullOrWhiteSpace(txtAgg3) ? (object)DBNull.Value : Convert.ToInt32(txtAgg3));
                    cmd.Parameters.AddWithValue("@Gate4", string.IsNullOrWhiteSpace(txtAgg4) ? (object)DBNull.Value : Convert.ToInt32(txtAgg4));
                    cmd.Parameters.AddWithValue("@Gate5", string.IsNullOrWhiteSpace(txtAgg5) ? (object)DBNull.Value : Convert.ToInt32(txtAgg5));
                    cmd.Parameters.AddWithValue("@Gate6", string.IsNullOrWhiteSpace(txtAgg6) ? (object)DBNull.Value : Convert.ToInt32(txtAgg6));

                    // Cement
                    cmd.Parameters.AddWithValue("@Cement1", string.IsNullOrWhiteSpace(txtC1) ? (object)DBNull.Value : Convert.ToInt32(txtC1));
                    //cmd.Parameters.AddWithValue("@Cement2", string.IsNullOrWhiteSpace(txtC2) ? (object)DBNull.Value : Convert.ToInt32(txtC2));
                    //cmd.Parameters.AddWithValue("@Cement3", string.IsNullOrWhiteSpace(txtC3) ? (object)DBNull.Value : Convert.ToInt32(txtC3));
                    cmd.Parameters.AddWithValue("@Cement4", string.IsNullOrWhiteSpace(txtC4) ? (object)DBNull.Value : Convert.ToInt32(txtC4));
                    //cmd.Parameters.AddWithValue("@Cement5", string.IsNullOrWhiteSpace(txtC5) ? (object)DBNull.Value : Convert.ToInt32(txtC5));

                    // Water
                    //cmd.Parameters.AddWithValue("@Water1", string.IsNullOrWhiteSpace(txtWTR1) ? (object)DBNull.Value : Convert.ToInt32(txtWTR1));
                    //cmd.Parameters.AddWithValue("@Water2", string.IsNullOrWhiteSpace(txtWTR2) ? (object)DBNull.Value : Convert.ToInt32(txtWTR2));
                   // cmd.Parameters.AddWithValue("@Water3", string.IsNullOrWhiteSpace(txtWTR3) ? (object)DBNull.Value : Convert.ToInt32(txtWTR3));

                    // Admix
                    cmd.Parameters.AddWithValue("@Admix1", string.IsNullOrWhiteSpace(txtAD1) ? (object)DBNull.Value : Convert.ToInt32(txtAD1));
                    cmd.Parameters.AddWithValue("@Admix2", string.IsNullOrWhiteSpace(txtAD2) ? (object)DBNull.Value : Convert.ToInt32(txtAD2));
                    //cmd.Parameters.AddWithValue("@Admix3", string.IsNullOrWhiteSpace(txtAD3) ? (object)DBNull.Value : Convert.ToInt32(txtAD3));
                    //cmd.Parameters.AddWithValue("@Admix4", string.IsNullOrWhiteSpace(txtAD4) ? (object)DBNull.Value : Convert.ToInt32(txtAD4));

                    // Silica
                    //cmd.Parameters.AddWithValue("@Silica", string.IsNullOrWhiteSpace(txtSilica) ? (object)DBNull.Value : Convert.ToInt32(txtSilica));

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadStepTime()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(
                        "SELECT TOP 1 * FROM StepTime_Setup", con);

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        // Aggregate
                        txtAgg1 = dr["Gate1"]?.ToString();
                        txtAgg2 = dr["Gate2"]?.ToString();
                        txtAgg3 = dr["Gate3"]?.ToString();
                        txtAgg4 = dr["Gate4"]?.ToString();
                        txtAgg5 = dr["Gate5"]?.ToString();
                        txtAgg6 = dr["Gate6"]?.ToString();

                        // Cement
                        txtC1 = dr["Cement1"]?.ToString();
                        //txtC2 = dr["Cement2"]?.ToString();
                        //txtC3 = dr["Cement3"]?.ToString();
                        txtC4 = dr["Cement4"]?.ToString();
                        //txtC5 = dr["Cement5"]?.ToString();

                        // Water
                        //txtWTR1 = dr["Water1"]?.ToString();
                        //txtWTR2 = dr["Water2"]?.ToString();
                        //txtWTR3 = dr["Water3"]?.ToString();

                        // Admix
                        txtAD1 = dr["Admix1"]?.ToString();
                        txtAD2 = dr["Admix2"]?.ToString();
                        //txtAD3 = dr["Admix3"]?.ToString();
                        //txtAD4 = dr["Admix4"]?.ToString();

                        // Silica
                        //txtSilica = dr["Silica"]?.ToString();
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