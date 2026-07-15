using Microsoft.Data.SqlClient;
using Scada_Demo.Database;
using Scada_Demo.MQTT_Model;
using Scada_Demo.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace Scada_Demo.ViewModels.BatchSettings
{
    public class GateSequenceViewModel : INotifyPropertyChanged
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

            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));

            return true;
        }

        #endregion

        private readonly MqttPublishSerice _mqttPublishSerice;

        public GateSequenceViewModel(
            MqttPublishSerice mqttPublishSerice)
        {
            _mqttPublishSerice = mqttPublishSerice;

            CurrentItems = new ObservableCollection<string>
            {
                "Agg1 : 1",
                "Agg2 : 2",
                "Agg3 : 3",
                "Agg4 : 4",
                "Agg5 : 5",
                "Agg6 : 6"
            };

            PreferredItems = new ObservableCollection<string>();

            MoveCommand = new RelayCommand(_ => MoveToPreferred());

            RefreshCommand = new RelayCommand(_ => Refresh());

            ReadFromPlcCommand = new RelayCommand(async _ => await ReadFromPlc());

            WriteToPlcCommand = new RelayCommand(async _ => await WriteToPlc());

            ExitCommand = new RelayCommand(_ => Exit());

            LoadGateSequence();
        }

        #region Collections

        public ObservableCollection<string> CurrentItems { get; }

        public ObservableCollection<string> PreferredItems { get; }

        #endregion

        #region Properties

        private string? _selectedCurrentItem;

        public string? SelectedCurrentItem
        {
            get => _selectedCurrentItem;
            set => SetProperty(ref _selectedCurrentItem, value);
        }

        #endregion

        #region Commands

        public ICommand MoveCommand { get; }

        public ICommand RefreshCommand { get; }

        public ICommand ReadFromPlcCommand { get; }

        public ICommand WriteToPlcCommand { get; }

        public ICommand ExitCommand { get; }

        #endregion

        private short _lastAgg1;
        private short _lastAgg3;
        private short _lastAgg4;
        private void MoveToPreferred()
        {
            if (string.IsNullOrWhiteSpace(SelectedCurrentItem))
            {
                MessageBox.Show("Select an item");
                return;
            }

            if (SelectedCurrentItem.StartsWith("Agg2"))
            {
                MessageBox.Show("Agg2 is fixed");
                return;
            }

            bool exists = PreferredItems.Any(x =>
                x.StartsWith(SelectedCurrentItem.Split(':')[0]));

            if (exists)
                return;

            // Add selected aggregate
            PreferredItems.Add(SelectedCurrentItem);

            // Agg2 always 2nd position
            if (!PreferredItems.Any(x => x.StartsWith("Agg2")))
            {
                if (PreferredItems.Count >= 1)
                    PreferredItems.Insert(1, "Agg2 : 2");
                else
                    PreferredItems.Add("Agg2 : 2");
            }
        }

        private void Refresh()
        {
            PreferredItems.Clear();
        }

        private async Task ReadFromPlc()
        {
            try
            {
                var request = new
                {
                    Type = "GateSequence",
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
                if (PreferredItems.Count != 4)
                {
                    MessageBox.Show("Sequence must contain Agg1, Agg2, Agg3 and Agg4.");
                    return;
                }

                _lastAgg1 = 0;
                _lastAgg3 = 0;
                _lastAgg4 = 0;

                for (int i = 0; i < PreferredItems.Count; i++)
                {
                    short position = (short)(i + 1);

                    if (PreferredItems[i].StartsWith("Agg1"))
                        _lastAgg1 = position;
                    else if (PreferredItems[i].StartsWith("Agg3"))
                        _lastAgg3 = position;
                    else if (PreferredItems[i].StartsWith("Agg4"))
                        _lastAgg4 = position;
                }

                var model = new GateSeq_Model
                {
                    Agg1 = _lastAgg1,
                    Agg3 = _lastAgg3,
                    Agg4 = _lastAgg4
                };

                var request = new
                {
                    Type = "GateSequence",
                    activity = "Write",
                    batchSettings_GateSequence = model
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
            if (data.Type != "GateSequence")
                return;

            var newItems = new List<string>();

            string[] positions = new string[4];
            positions[1] = "Agg2 : 2";

            if (data.batchSettings_GateSequence.Agg1 >= 1 &&
                data.batchSettings_GateSequence.Agg1 <= 4)
            {
                positions[data.batchSettings_GateSequence.Agg1 - 1] =
                    $"Agg1 : {data.batchSettings_GateSequence.Agg1}";
            }

            if (data.batchSettings_GateSequence.Agg3 >= 1 &&
                data.batchSettings_GateSequence.Agg3 <= 4)
            {
                positions[data.batchSettings_GateSequence.Agg3 - 1] =
                    $"Agg3 : {data.batchSettings_GateSequence.Agg3}";
            }

            if (data.batchSettings_GateSequence.Agg4 >= 1 &&
                data.batchSettings_GateSequence.Agg4 <= 4)
            {
                positions[data.batchSettings_GateSequence.Agg4 - 1] =
                    $"Agg4 : {data.batchSettings_GateSequence.Agg4}";
            }

            foreach (var item in positions)
            {
                if (!string.IsNullOrWhiteSpace(item))
                    newItems.Add(item);
            }

            if (newItems.Count == 4)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    PreferredItems.Clear();

                    foreach (var item in newItems)
                    {
                        PreferredItems.Add(item);
                    }
                });
            }

            if (data.activity == "Write")
            {
                if (_lastAgg1 == data.batchSettings_GateSequence.Agg1 &&
                    _lastAgg3 == data.batchSettings_GateSequence.Agg3 &&
                    _lastAgg4 == data.batchSettings_GateSequence.Agg4)
                {
                    SaveGateSequence(_lastAgg1, _lastAgg3, _lastAgg4);
                    MessageBox.Show("Data saved successfully.");
                }
                else
                {
                    MessageBox.Show("Failed..! Try again.");
                }
            }
        }
        private void SaveGateSequence(int gate1, int gate3, int gate4)
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();

                con.Open();

                SqlCommand cmd = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM GateSeQ_Setup)
BEGIN
    UPDATE GateSeQ_Setup
    SET Gate1=@Gate1,
        Gate2=2,
        Gate3=@Gate3,
        Gate4=@Gate4
END
ELSE
BEGIN
    INSERT INTO GateSeQ_Setup
    (
        Gate1,
        Gate2,
        Gate3,
        Gate4
    )
    VALUES
    (
        @Gate1,
        2,
        @Gate3,
        @Gate4
    )
END", con);

                cmd.Parameters.AddWithValue("@Gate1", gate1);
                cmd.Parameters.AddWithValue("@Gate3", gate3);
                cmd.Parameters.AddWithValue("@Gate4", gate4);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadGateSequence()
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();

                con.Open();

                SqlCommand cmd = new SqlCommand(@"
SELECT TOP 1
Gate1,
Gate2,
Gate3,
Gate4
FROM GateSeQ_Setup", con);

                using SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                    return;

                PreferredItems.Clear();

                string[] positions = new string[4];

                positions[Convert.ToInt32(reader["Gate1"]) - 1] = $"Agg1 : {reader["Gate1"]}";
                positions[Convert.ToInt32(reader["Gate2"]) - 1] = $"Agg2 : {reader["Gate2"]}";
                positions[Convert.ToInt32(reader["Gate3"]) - 1] = $"Agg3 : {reader["Gate3"]}";
                positions[Convert.ToInt32(reader["Gate4"]) - 1] = $"Agg4 : {reader["Gate4"]}";

                foreach (var item in positions)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                        PreferredItems.Add(item);
                }
            }
            catch
            {
                // Ignore if table is empty
            }
        }

        private void Exit()
        {
            Application.Current?.Windows
                .OfType<Window>()
                .FirstOrDefault(x => x is Batch_Settings.Gate_Sequence)
                ?.Close();
        }
    }
}