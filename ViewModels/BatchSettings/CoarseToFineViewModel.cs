using Scada_Demo.MQTT_Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
using Scada_Demo.Database;

namespace Scada_Demo.ViewModels.BatchSettings
{
    public class CoarseToFineViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        private readonly MqttPublishSerice _mqttPublishSerice;

        public CoarseToFineViewModel(MqttPublishSerice mqttPublishSerice)
        {
            _mqttPublishSerice = mqttPublishSerice;

            ReadFromPlcCommand = new RelayCommand(_ => ReadFromPlc());
            WriteToPlcCommand = new RelayCommand(_ => WriteToPlc());
            ExitCommand = new RelayCommand(_ => Exit());

            LoadCoarseToFine();
        }

        #region Properties

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

        private string _wtr1;
        public string txtWTR1
        {
            get => _wtr1;
            set => SetProperty(ref _wtr1, value);
        }

        #endregion

        public ICommand ReadFromPlcCommand { get; }
        public ICommand WriteToPlcCommand { get; }
        public ICommand ExitCommand { get; }

        private async Task ReadFromPlc()
        {
            try
            {
                var NewObj = new
                {
                    Type = "CTF",
                    activity = "Read"
                };

                await _mqttPublishSerice.PublishDataMqttTopic(
                    "BatchSettings/Request",
                    JsonSerializer.Serialize(NewObj));
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
                var writeData = new Coarse_to_Fine_model
                {
                    Agg1 = Convert.ToInt16(txtAgg1),
                    Agg2 = Convert.ToInt16(txtAgg2),
                    Agg3 = Convert.ToInt16(txtAgg3),
                    Agg4 = Convert.ToInt16(txtAgg4),
                    Agg5 = Convert.ToInt16(txtAgg5),

                    Water = Convert.ToInt16(txtWTR1)
                };

                var NewObj = new
                {
                    Type = "CTF",
                    activity = "Write",
                    batchSettings_CoarseFine = writeData
                };

                await _mqttPublishSerice.PublishDataMqttTopic(
                    "BatchSettings/Request",
                    JsonSerializer.Serialize(NewObj));
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
                if (data.Type == "CTF")
                {
                    if (data.activity == "Read")
                    {
                        txtAgg1 = data.batchSettings_CoarseFine.Agg1.ToString();
                        txtAgg2 = data.batchSettings_CoarseFine.Agg2.ToString();
                        txtAgg3 = data.batchSettings_CoarseFine.Agg3.ToString();
                        txtAgg4 = data.batchSettings_CoarseFine.Agg4.ToString();
                        txtAgg5 = data.batchSettings_CoarseFine.Agg5.ToString();

                        txtWTR1 = data.batchSettings_CoarseFine.Water.ToString();
                    }
                    else // Write Response
                    {
                        if (
                            Convert.ToInt16(txtAgg1) == data.batchSettings_CoarseFine.Agg1 &&
                            Convert.ToInt16(txtAgg2) == data.batchSettings_CoarseFine.Agg2 &&
                            Convert.ToInt16(txtAgg3) == data.batchSettings_CoarseFine.Agg3 &&
                            Convert.ToInt16(txtAgg4) == data.batchSettings_CoarseFine.Agg4 &&
                            Convert.ToInt16(txtAgg5) == data.batchSettings_CoarseFine.Agg5 &&
                            Convert.ToInt16(txtWTR1) == data.batchSettings_CoarseFine.Water
                           )
                        {
                            SaveCoarseToFine();

                            MessageBox.Show("Data saved successfully.!");
                        }
                        else
                        {
                            MessageBox.Show("Failed.! Try again.!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveCoarseToFine()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM CoarseToFine_Setup)
BEGIN
    UPDATE CoarseToFine_Setup
    SET
        Agg1=@Agg1,
        Agg2=@Agg2,
        Agg3=@Agg3,
        Agg4=@Agg4,
        Agg5=@Agg5,
        Water=@Water
END
ELSE
BEGIN
    INSERT INTO CoarseToFine_Setup
    (
        Agg1,
        Agg2,
        Agg3,
        Agg4,
        Agg5,
        Water
    )
    VALUES
    (
        @Agg1,
        @Agg2,
        @Agg3,
        @Agg4,
        @Agg5,
        @Water
    )
END", con);

                    cmd.Parameters.AddWithValue("@Agg1",
                        string.IsNullOrWhiteSpace(txtAgg1) ? (object)DBNull.Value : Convert.ToInt32(txtAgg1));

                    cmd.Parameters.AddWithValue("@Agg2",
                        string.IsNullOrWhiteSpace(txtAgg2) ? (object)DBNull.Value : Convert.ToInt32(txtAgg2));

                    cmd.Parameters.AddWithValue("@Agg3",
                        string.IsNullOrWhiteSpace(txtAgg3) ? (object)DBNull.Value : Convert.ToInt32(txtAgg3));

                    cmd.Parameters.AddWithValue("@Agg4",
                        string.IsNullOrWhiteSpace(txtAgg4) ? (object)DBNull.Value : Convert.ToInt32(txtAgg4));

                    cmd.Parameters.AddWithValue("@Agg5",
                        string.IsNullOrWhiteSpace(txtAgg5) ? (object)DBNull.Value : Convert.ToInt32(txtAgg5));

                    cmd.Parameters.AddWithValue("@Water",
                        string.IsNullOrWhiteSpace(txtWTR1) ? (object)DBNull.Value : Convert.ToInt32(txtWTR1));

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadCoarseToFine()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(
                        "SELECT TOP 1 * FROM CoarseToFine_Setup", con);

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        txtAgg1 = dr["Agg1"]?.ToString();
                        txtAgg2 = dr["Agg2"]?.ToString();
                        txtAgg3 = dr["Agg3"]?.ToString();
                        txtAgg4 = dr["Agg4"]?.ToString();
                        txtAgg5 = dr["Agg5"]?.ToString();

                        txtWTR1 = dr["Water"]?.ToString();
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Exit()
        {
            Application.Current.Windows[0]?.Close();
        }
    }
}