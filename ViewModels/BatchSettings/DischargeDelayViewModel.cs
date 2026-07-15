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
    public class DischargeDelayViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        #endregion

        private readonly MqttPublishSerice _mqttPublishSerice;

        public DischargeDelayViewModel(MqttPublishSerice mqttPublishSerice)
        {
            _mqttPublishSerice = mqttPublishSerice;

            ReadFromPlcCommand = new RelayCommand(_ => ReadFromPlc());
            WriteToPlcCommand = new RelayCommand(_ => WriteToPlc());
            ExitCommand = new RelayCommand(_ => Exit());

            LoadDischargeDelay();
        }

        #region Properties

        private string _cement;
        public string Cement
        {
            get => _cement;
            set => SetProperty(ref _cement, value);
        }

        private string _water;
        public string Water
        {
            get => _water;
            set => SetProperty(ref _water, value);
        }

        private string _admix;
        public string Admix
        {
            get => _admix;
            set => SetProperty(ref _admix, value);
        }

        private string _pumpCutOff;
        public string PumpCutOff
        {
            get => _pumpCutOff;
            set => SetProperty(ref _pumpCutOff, value);
        }

        private string _skip;
        public string Skip
        {
            get => _skip;
            set => SetProperty(ref _skip, value);
        }

        #endregion

        #region Commands

        public ICommand ReadFromPlcCommand { get; }
        public ICommand WriteToPlcCommand { get; }
        public ICommand ExitCommand { get; }

        #endregion

        private async Task ReadFromPlc()
        {
            try
            {
                var newObj = new
                {
                    Type = "DischargeDelay",
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
                var writeData = new DischargeDelay_Model();

                writeData.Cement = Convert.ToInt32(_cement);
                writeData.Water = Convert.ToInt32(_water);
                writeData.Admix = Convert.ToInt32(_admix);
                writeData.Skip = Convert.ToInt32(_skip);
                writeData.PumpCutOff = Convert.ToInt16(_pumpCutOff);

                var NewObj = new
                {
                    Type = "DischargeDelay",
                    activity = "Write",
                    batchSettings_DischargeDelay = writeData
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

        private void Exit()
        {
            Application.Current?.Windows[0]?.Close();
        }

        public void MqttReadSuccessStatus(BatchSettingsModel data)
        {

            if (data.Type == "DischargeDelay")
            {
                if (data.activity == "Read") // Read Response
                {
                    Cement = data.batchSettings_DischargeDelay.Cement.ToString();
                    Water = data.batchSettings_DischargeDelay.Water.ToString();
                    Admix = data.batchSettings_DischargeDelay.Admix.ToString();
                    Skip = data.batchSettings_DischargeDelay.Skip.ToString();
                    PumpCutOff = data.batchSettings_DischargeDelay.PumpCutOff.ToString();
                }
                else // Write Response
                {
                    if (
                        Convert.ToInt32(Cement) == data.batchSettings_DischargeDelay.Cement &&
                        Convert.ToInt32(Water) == data.batchSettings_DischargeDelay.Water &&
                        Convert.ToInt32(Admix) == data.batchSettings_DischargeDelay.Admix &&
                        Convert.ToInt32(Skip) == data.batchSettings_DischargeDelay.Skip &&
                        Convert.ToInt16(PumpCutOff) == data.batchSettings_DischargeDelay.PumpCutOff
                    )
                    {
                        // Save database
                        SaveDischargeDelay();

                        MessageBox.Show("Data saved successfully.!");
                    }
                    else
                    {
                        MessageBox.Show("Failed.! Try again.!");
                    }
                }
            }


        }

        private void SaveDischargeDelay()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM DisDelay_Setup)
BEGIN
    UPDATE DisDelay_Setup
    SET
        Cement = @Cement,
        Water = @Water,
        Admix12 = @Admix12,
        Wtr_Dis_Pump_Cutoff = @PumpCutOff,
        Wait_Hopper_Instant_Open_delay = @WaitHopper
END
ELSE
BEGIN
    INSERT INTO DisDelay_Setup
    (
        Cement,
        Water,
        Admix12,
        Wtr_Dis_Pump_Cutoff,
        Wait_Hopper_Instant_Open_delay
    )
    VALUES
    (
        @Cement,
        @Water,
        @Admix12,
        @PumpCutOff,
        @WaitHopper
    )
END", con);

                    cmd.Parameters.AddWithValue("@Cement",
                        string.IsNullOrWhiteSpace(Cement) ? (object)DBNull.Value : Convert.ToInt32(Cement));

                    cmd.Parameters.AddWithValue("@Water",
                        string.IsNullOrWhiteSpace(Water) ? (object)DBNull.Value : Convert.ToInt32(Water));

                    cmd.Parameters.AddWithValue("@Admix12",
                        string.IsNullOrWhiteSpace(Admix) ? (object)DBNull.Value : Convert.ToInt32(Admix));

                    cmd.Parameters.AddWithValue("@WaitHopper",
                        string.IsNullOrWhiteSpace(Skip) ? (object)DBNull.Value : Convert.ToInt32(Skip));

                    cmd.Parameters.AddWithValue("@PumpCutOff",
                        string.IsNullOrWhiteSpace(PumpCutOff) ? (object)DBNull.Value : Convert.ToInt32(PumpCutOff));

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadDischargeDelay()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"
SELECT TOP 1
    Cement,
    Water,
    Admix12,
    Wtr_Dis_Pump_Cutoff,
    Wait_Hopper_Instant_Open_delay
FROM DisDelay_Setup", con);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Cement = reader["Cement"]?.ToString() ?? "";
                            Water = reader["Water"]?.ToString() ?? "";
                            Admix = reader["Admix12"]?.ToString() ?? "";
                            PumpCutOff = reader["Wtr_Dis_Pump_Cutoff"]?.ToString() ?? "";
                            Skip = reader["Wait_Hopper_Instant_Open_delay"]?.ToString() ?? "";
                        }
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