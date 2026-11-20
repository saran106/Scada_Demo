//using System.ComponentModel;
//using System.Runtime.CompilerServices;
//using System.Text.Json;
//using System.Windows;
//using System.Windows.Input;
//using Microsoft.Data.SqlClient;
//using Scada_Demo.MQTT_Model;
//using Scada_Demo.Services;
//using Scada_Demo.Database;
//using Scada_Demo.ViewModels.BatchSettings;

//namespace Scada_Demo.ViewModels.BatchSettings
//{
//    public class DischargeDelayViewModel : INotifyPropertyChanged
//    {
//        #region INotifyPropertyChanged

//        public event PropertyChangedEventHandler? PropertyChanged;

//        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
//        {
//            if (Equals(field, value))
//                return false;

//            field = value;
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//            return true;
//        }

//        #endregion

//        private readonly MqttPublishSerice _mqttPublishSerice;

//        public DischargeDelayViewModel(MqttPublishSerice mqttPublishSerice)
//        {
//            _mqttPublishSerice = mqttPublishSerice;

//            ReadFromPlcCommand = new RelayCommand(_ => ReadFromPlc());
//            WriteToPlcCommand = new RelayCommand(_ => WriteToPlc());
//            ExitCommand = new RelayCommand(_ => Exit());

//            LoadDischargeDelay();
//        }

//        #region Properties

//        private string _cement;
//        public string Cement
//        {
//            get => _cement;
//            set => SetProperty(ref _cement, value);
//        }

//        private string _water;
//        public string Water
//        {
//            get => _water;
//            set => SetProperty(ref _water, value);
//        }

//        private string _admix;
//        public string Admix
//        {
//            get => _admix;
//            set => SetProperty(ref _admix, value);
//        }

//        private string _pumpCutOff;
//        public string PumpCutOff
//        {
//            get => _pumpCutOff;
//            set => SetProperty(ref _pumpCutOff, value);
//        }

//        private string _skip;
//        public string Skip
//        {
//            get => _skip;
//            set => SetProperty(ref _skip, value);
//        }

//        #endregion

//        #region Commands

//        public ICommand ReadFromPlcCommand { get; }
//        public ICommand WriteToPlcCommand { get; }
//        public ICommand ExitCommand { get; }

//        #endregion

//        private async Task ReadFromPlc()
//        {
//            try
//            {
//                var newObj = new
//                {
//                    Type = "DischargeDelay",
//                    activity = "Read"
//                };

//                await _mqttPublishSerice.PublishDataMqttTopic(
//                    "BatchSettings/Request",
//                    JsonSerializer.Serialize(newObj));
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message);
//            }
//        }

//        private async Task WriteToPlc()
//        {
//            try
//            {
//                var writeData = new DischargeDelay_Model();

//                writeData.Cement = Convert.ToInt32(_cement);
//                writeData.Water = Convert.ToInt32(_water);
//                writeData.Admix = Convert.ToInt32(_admix);
//                writeData.Skip = Convert.ToInt32(_skip);
//                writeData.PumpCutOff = Convert.ToInt16(_pumpCutOff);

//                var NewObj = new
//                {
//                    Type = "DischargeDelay",
//                    activity = "Write",
//                    batchSettings_DischargeDelay = writeData
//                };

//                await _mqttPublishSerice.PublishDataMqttTopic(
//                    "BatchSettings/Request",
//                    JsonSerializer.Serialize(NewObj));
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message);
//            }
//        }

//        private void Exit()
//        {
//            Application.Current?.Windows[0]?.Close();
//        }

//        public void MqttReadSuccessStatus(BatchSettingsModel data)
//        {

//            if (data.Type == "DischargeDelay")
//            {
//                if (data.activity == "Read") // Read Response
//                {
//                    Cement = data.batchSettings_DischargeDelay.Cement.ToString();
//                    Water = data.batchSettings_DischargeDelay.Water.ToString();
//                    Admix = data.batchSettings_DischargeDelay.Admix.ToString();
//                    Skip = data.batchSettings_DischargeDelay.Skip.ToString();
//                    PumpCutOff = data.batchSettings_DischargeDelay.PumpCutOff.ToString();
//                }
//                else // Write Response
//                {
//                    if (
//                        Convert.ToInt32(Cement) == data.batchSettings_DischargeDelay.Cement &&
//                        Convert.ToInt32(Water) == data.batchSettings_DischargeDelay.Water &&
//                        Convert.ToInt32(Admix) == data.batchSettings_DischargeDelay.Admix &&
//                        Convert.ToInt32(Skip) == data.batchSettings_DischargeDelay.Skip &&
//                        Convert.ToInt16(PumpCutOff) == data.batchSettings_DischargeDelay.PumpCutOff
//                    )
//                    {
//                        // Save database
//                        SaveDischargeDelay();

//                        MessageBox.Show("Data saved successfully.!");
//                    }
//                    else
//                    {
//                        MessageBox.Show("Failed.! Try again.!");
//                    }
//                }
//            }


//        }

//        private void SaveDischargeDelay()
//        {
//            try
//            {
//                using (SqlConnection con = DbConnection.GetConnection())
//                {
//                    con.Open();

//                    SqlCommand cmd = new SqlCommand(@"
//IF EXISTS (SELECT 1 FROM DisDelay_Setup)
//BEGIN
//    UPDATE DisDelay_Setup
//    SET
//        Cement = @Cement,
//        Water = @Water,
//        Admix12 = @Admix12,
//        Wtr_Dis_Pump_Cutoff = @PumpCutOff,
//        Wait_Hopper_Instant_Open_delay = @WaitHopper
//END
//ELSE
//BEGIN
//    INSERT INTO DisDelay_Setup
//    (
//        Cement,
//        Water,
//        Admix12,
//        Wtr_Dis_Pump_Cutoff,
//        Wait_Hopper_Instant_Open_delay
//    )
//    VALUES
//    (
//        @Cement,
//        @Water,
//        @Admix12,
//        @PumpCutOff,
//        @WaitHopper
//    )
//END", con);

//                    cmd.Parameters.AddWithValue("@Cement",
//                        string.IsNullOrWhiteSpace(Cement) ? (object)DBNull.Value : Convert.ToInt32(Cement));

//                    cmd.Parameters.AddWithValue("@Water",
//                        string.IsNullOrWhiteSpace(Water) ? (object)DBNull.Value : Convert.ToInt32(Water));

//                    cmd.Parameters.AddWithValue("@Admix12",
//                        string.IsNullOrWhiteSpace(Admix) ? (object)DBNull.Value : Convert.ToInt32(Admix));

//                    cmd.Parameters.AddWithValue("@WaitHopper",
//                        string.IsNullOrWhiteSpace(Skip) ? (object)DBNull.Value : Convert.ToInt32(Skip));

//                    cmd.Parameters.AddWithValue("@PumpCutOff",
//                        string.IsNullOrWhiteSpace(PumpCutOff) ? (object)DBNull.Value : Convert.ToInt32(PumpCutOff));

//                    cmd.ExecuteNonQuery();
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message);
//            }
//        }

//        private void LoadDischargeDelay()
//        {
//            try
//            {
//                using (SqlConnection con = DbConnection.GetConnection())
//                {
//                    con.Open();

//                    SqlCommand cmd = new SqlCommand(@"
//SELECT TOP 1
//    Cement,
//    Water,
//    Admix12,
//    Wtr_Dis_Pump_Cutoff,
//    Wait_Hopper_Instant_Open_delay
//FROM DisDelay_Setup", con);

//                    using (SqlDataReader reader = cmd.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            Cement = reader["Cement"]?.ToString() ?? "";
//                            Water = reader["Water"]?.ToString() ?? "";
//                            Admix = reader["Admix12"]?.ToString() ?? "";
//                            PumpCutOff = reader["Wtr_Dis_Pump_Cutoff"]?.ToString() ?? "";
//                            Skip = reader["Wait_Hopper_Instant_Open_delay"]?.ToString() ?? "";
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message);
//            }
//        }

//    }
//}



using System;
using System.ComponentModel;
using System.Globalization;
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

        protected bool SetProperty<T>(
            ref T field,
            T value,
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

        public DischargeDelayViewModel(MqttPublishSerice mqttPublishSerice)
        {
            _mqttPublishSerice = mqttPublishSerice;

            ReadFromPlcCommand = new RelayCommand(_ => ReadFromPlc());
            WriteToPlcCommand = new RelayCommand(_ => WriteToPlc());
            ExitCommand = new RelayCommand(_ => Exit());

            LoadDischargeDelay();
        }

        #region Properties

        private string _cement = string.Empty;
        public string Cement
        {
            get => _cement;
            set => SetProperty(ref _cement, value);
        }

        private string _water = string.Empty;
        public string Water
        {
            get => _water;
            set => SetProperty(ref _water, value);
        }

        private string _admix = string.Empty;
        public string Admix
        {
            get => _admix;
            set => SetProperty(ref _admix, value);
        }

        private string _pumpCutOff = string.Empty;
        public string PumpCutOff
        {
            get => _pumpCutOff;
            set => SetProperty(ref _pumpCutOff, value);
        }

        private string _skip = string.Empty;
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

        #region Unit Conversion

        // HMI displays seconds. PLC and the existing SQL storage use milliseconds.
        // Example: HMI 6.00 seconds <-> PLC/SQL 6000 milliseconds.
        private static decimal ParseDisplaySeconds(string? value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{fieldName} cannot be empty.");

            if (!decimal.TryParse(
                    value.Trim(),
                    NumberStyles.Number,
                    CultureInfo.CurrentCulture,
                    out decimal seconds))
            {
                throw new ArgumentException($"Enter a valid number for {fieldName}.");
            }

            if (seconds < 0)
                throw new ArgumentException($"{fieldName} cannot be negative.");

            return seconds;
        }

        private static int ToPlcInt(string? displayValue, string fieldName)
        {
            decimal milliseconds = ParseDisplaySeconds(displayValue, fieldName) * 1000m;
            return checked((int)Math.Round(
                milliseconds,
                0,
                MidpointRounding.AwayFromZero));
        }

        private static short ToPlcShort(string? displayValue, string fieldName)
        {
            decimal milliseconds = ParseDisplaySeconds(displayValue, fieldName) * 1000m;
            return checked((short)Math.Round(
                milliseconds,
                0,
                MidpointRounding.AwayFromZero));
        }

        private static string ToDisplaySeconds(int plcMilliseconds)
        {
            return (plcMilliseconds / 1000m).ToString(
                "0.00",
                CultureInfo.CurrentCulture);
        }

        #endregion

        #region MQTT Read / Write

        private async Task ReadFromPlc()
        {
            try
            {
                var request = new
                {
                    Type = "DischargeDelay",
                    activity = "Read"
                };

                await _mqttPublishSerice.PublishDataMqttTopic(
                    "BatchSettings/Request",
                    JsonSerializer.Serialize(request));
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to request Discharge Delay values.\n{ex.Message}",
                    "Discharge Delay",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task WriteToPlc()
        {
            try
            {
                var writeData = new DischargeDelay_Model
                {
                    Cement = ToPlcInt(Cement, nameof(Cement)),
                    Water = ToPlcInt(Water, nameof(Water)),
                    Admix = ToPlcInt(Admix, nameof(Admix)),
                    Skip = ToPlcInt(Skip, nameof(Skip)),
                    PumpCutOff = ToPlcShort(PumpCutOff, nameof(PumpCutOff))
                };

                var request = new
                {
                    Type = "DischargeDelay",
                    activity = "Write",
                    batchSettings_DischargeDelay = writeData
                };

                await _mqttPublishSerice.PublishDataMqttTopic(
                    "BatchSettings/Request",
                    JsonSerializer.Serialize(request));
            }
            catch (OverflowException)
            {
                MessageBox.Show(
                    "One or more values are outside the supported PLC range.",
                    "Invalid value",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Discharge Delay",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        #endregion

        private void Exit()
        {
            Application.Current?.Windows[0]?.Close();
        }

        public void MqttReadSuccessStatus(BatchSettingsModel data)
        {
            if (data?.Type != "DischargeDelay" ||
                data.batchSettings_DischargeDelay == null)
            {
                return;
            }

            var plcData = data.batchSettings_DischargeDelay;

            if (data.activity == "Read")
            {
                // PLC milliseconds -> HMI seconds.
                Cement = ToDisplaySeconds(plcData.Cement);
                Water = ToDisplaySeconds(plcData.Water);
                Admix = ToDisplaySeconds(plcData.Admix);
                Skip = ToDisplaySeconds(plcData.Skip);
                PumpCutOff = ToDisplaySeconds(plcData.PumpCutOff);
            }
            else
            {
                // Compare the PLC acknowledgement in milliseconds with the
                // HMI values converted back to milliseconds.
                try
                {
                    bool writeSucceeded =
                        ToPlcInt(Cement, nameof(Cement)) == plcData.Cement &&
                        ToPlcInt(Water, nameof(Water)) == plcData.Water &&
                        ToPlcInt(Admix, nameof(Admix)) == plcData.Admix &&
                        ToPlcInt(Skip, nameof(Skip)) == plcData.Skip &&
                        ToPlcShort(PumpCutOff, nameof(PumpCutOff)) == plcData.PumpCutOff;

                    if (writeSucceeded)
                    {
                        SaveDischargeDelay();
                        MessageBox.Show(
                            "Data saved successfully.!",
                            "Discharge Delay",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(
                            "Failed.! Try again.!",
                            "Discharge Delay",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Unable to verify the PLC write response.\n{ex.Message}",
                        "Discharge Delay",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        #region SQL Save / Load

        private void SaveDischargeDelay()
        {
            try
            {
                // Keep the existing SQL storage unit unchanged: milliseconds.
                // This preserves compatibility with the existing integer columns.
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    using SqlCommand cmd = new SqlCommand(@"
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

                    cmd.Parameters.Add("@Cement", System.Data.SqlDbType.Int).Value =
                        ToPlcInt(Cement, nameof(Cement));

                    cmd.Parameters.Add("@Water", System.Data.SqlDbType.Int).Value =
                        ToPlcInt(Water, nameof(Water));

                    cmd.Parameters.Add("@Admix12", System.Data.SqlDbType.Int).Value =
                        ToPlcInt(Admix, nameof(Admix));

                    cmd.Parameters.Add("@WaitHopper", System.Data.SqlDbType.Int).Value =
                        ToPlcInt(Skip, nameof(Skip));

                    cmd.Parameters.Add("@PumpCutOff", System.Data.SqlDbType.Int).Value =
                        ToPlcShort(PumpCutOff, nameof(PumpCutOff));

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to save Discharge Delay values.\n{ex.Message}",
                    "Database error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void LoadDischargeDelay()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    using SqlCommand cmd = new SqlCommand(@"
SELECT TOP 1
    Cement,
    Water,
    Admix12,
    Wtr_Dis_Pump_Cutoff,
    Wait_Hopper_Instant_Open_delay
FROM DisDelay_Setup", con);

                    using SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Existing SQL values are milliseconds -> HMI seconds.
                        Cement = reader["Cement"] == DBNull.Value
                            ? string.Empty
                            : ToDisplaySeconds(Convert.ToInt32(reader["Cement"]));

                        Water = reader["Water"] == DBNull.Value
                            ? string.Empty
                            : ToDisplaySeconds(Convert.ToInt32(reader["Water"]));

                        Admix = reader["Admix12"] == DBNull.Value
                            ? string.Empty
                            : ToDisplaySeconds(Convert.ToInt32(reader["Admix12"]));

                        PumpCutOff = reader["Wtr_Dis_Pump_Cutoff"] == DBNull.Value
                            ? string.Empty
                            : ToDisplaySeconds(Convert.ToInt32(reader["Wtr_Dis_Pump_Cutoff"]));

                        Skip = reader["Wait_Hopper_Instant_Open_delay"] == DBNull.Value
                            ? string.Empty
                            : ToDisplaySeconds(Convert.ToInt32(reader["Wait_Hopper_Instant_Open_delay"]));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to load Discharge Delay values.\n{ex.Message}",
                    "Database error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
