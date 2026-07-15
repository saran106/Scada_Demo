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
    public class EmptyValueViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected bool SetProperty<T>(ref T field, T value,
            [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        #endregion

        private readonly MqttPublishSerice _mqttPublishSerice;

        public EmptyValueViewModel(MqttPublishSerice mqttPublishSerice)
        {
            _mqttPublishSerice = mqttPublishSerice;

            ReadFromPlcCommand = new RelayCommand(_ => ReadFromPlc());
            WriteToPlcCommand = new RelayCommand(_ => WriteToPlc());
            ExitCommand = new RelayCommand(_ => Exit());

            LoadEmptyValues();
        }

        #region Properties

        private string _aggregate = "";
        public string Aggregate
        {
            get => _aggregate;
            set => SetProperty(ref _aggregate, value);
        }

        private string _cement = "";
        public string Cement
        {
            get => _cement;
            set => SetProperty(ref _cement, value);
        }

        private string _water = "";
        public string Water
        {
            get => _water;
            set => SetProperty(ref _water, value);
        }

        // UI-la irukku, PLC-la use pannala naalum binding-ku vechirukkom
        private string _water2 = "";
        public string Water2
        {
            get => _water2;
            set => SetProperty(ref _water2, value);
        }

        private string _admix = "";
        public string Admix
        {
            get => _admix;
            set => SetProperty(ref _admix, value);
        }

        // UI-la irukku
        private string _admix34 = "";
        public string Admix34
        {
            get => _admix34;
            set => SetProperty(ref _admix34, value);
        }

        private string _silica = "";
        public string Silica
        {
            get => _silica;
            set => SetProperty(ref _silica, value);
        }

        #endregion

        #region Commands

        public ICommand ReadFromPlcCommand { get; }
        public ICommand WriteToPlcCommand { get; }
        public ICommand ExitCommand { get; }

        #endregion

        // Next implement pannuvom
        private async Task ReadFromPlc()
        {
            try
            {
                var newObj = new
                {
                    Type = "EmptyValue",
                    activity = "Read"
                };

                await _mqttPublishSerice.PublishDataMqttTopic(
                    "BatchSettings/Request",
                    JsonSerializer.Serialize(newObj));
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
                var writeData = new EmptyValue_Model();

                writeData.Agg = Convert.ToInt16(Aggregate);
                writeData.Cem = Convert.ToInt16(Cement);
                writeData.Water = Convert.ToInt16(Water);
                writeData.Admix = Convert.ToInt16(Admix);
                writeData.Ice = Convert.ToInt16(Silica);

                var newObj = new
                {
                    Type = "EmptyValue",
                    activity = "Write",
                    batchSettings_EmptyValue = writeData
                };

                await _mqttPublishSerice.PublishDataMqttTopic(
                    "BatchSettings/Request",
                    JsonSerializer.Serialize(newObj));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void MqttReadSuccessStatus(BatchSettingsModel data)
        {
            if (data.Type == "EmptyValue")
            {
                if (data.activity == "Read") // Read Response
                {
                    Aggregate = data.batchSettings_EmptyValue.Agg.ToString();
                    Cement = data.batchSettings_EmptyValue.Cem.ToString();
                    Water = data.batchSettings_EmptyValue.Water.ToString();
                    Admix = data.batchSettings_EmptyValue.Admix.ToString();
                    Silica = data.batchSettings_EmptyValue.Ice.ToString();

                    // UI-la mattum irukku
                    Water2 = "";
                    Admix34 = "";
                }
                else // Write Response
                {
                    if (
                        Convert.ToInt16(Aggregate) == data.batchSettings_EmptyValue.Agg &&
                        Convert.ToInt16(Cement) == data.batchSettings_EmptyValue.Cem &&
                        Convert.ToInt16(Water) == data.batchSettings_EmptyValue.Water &&
                        Convert.ToInt16(Admix) == data.batchSettings_EmptyValue.Admix &&
                        Convert.ToInt16(Silica) == data.batchSettings_EmptyValue.Ice
                    )
                    {
                        SaveEmptyValues();

                        MessageBox.Show("Data saved successfully.!");
                    }
                    else
                    {
                        MessageBox.Show("Failed.! Try again.!");
                    }
                }
            }
        }

        private void SaveEmptyValues()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM EmptyValue_Setup)
BEGIN
    UPDATE EmptyValue_Setup
    SET
        Aggregate = @Aggregate,
        Cement = @Cement,
        Water = @Water,
        Admix12 = @Admix12,
        Silica = @Silica
END
ELSE
BEGIN
    INSERT INTO EmptyValue_Setup
    (
        Aggregate,
        Cement,
        Water,
        Admix12,
        Silica
    )
    VALUES
    (
        @Aggregate,
        @Cement,
        @Water,
        @Admix12,
        @Silica
    )
END", con);

                    cmd.Parameters.AddWithValue("@Aggregate",
                        string.IsNullOrWhiteSpace(Aggregate)
                            ? (object)DBNull.Value
                            : Convert.ToInt32(Aggregate));

                    cmd.Parameters.AddWithValue("@Cement",
                        string.IsNullOrWhiteSpace(Cement)
                            ? (object)DBNull.Value
                            : Convert.ToInt32(Cement));

                    cmd.Parameters.AddWithValue("@Water",
                        string.IsNullOrWhiteSpace(Water)
                            ? (object)DBNull.Value
                            : Convert.ToInt32(Water));

                    cmd.Parameters.AddWithValue("@Admix12",
                        string.IsNullOrWhiteSpace(Admix)
                            ? (object)DBNull.Value
                            : Convert.ToInt32(Admix));

                    cmd.Parameters.AddWithValue("@Silica",
                        string.IsNullOrWhiteSpace(Silica)
                            ? (object)DBNull.Value
                            : Convert.ToInt32(Silica));

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadEmptyValues()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"
SELECT TOP 1
    Aggregate,
    Cement,
    Water,
    Admix12,
    Silica
FROM EmptyValue_Setup", con);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Aggregate = reader["Aggregate"]?.ToString() ?? "";
                            Cement = reader["Cement"]?.ToString() ?? "";
                            Water = reader["Water"]?.ToString() ?? "";
                            Admix = reader["Admix12"]?.ToString() ?? "";
                            Silica = reader["Silica"]?.ToString() ?? "";

                            // Currently not stored in DB
                            Water2 = "";
                            Admix34 = "";
                        }
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
    }
}