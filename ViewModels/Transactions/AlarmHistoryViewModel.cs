using Microsoft.Data.SqlClient;
using Scada_Demo.Common;
using Scada_Demo.Database;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace Scada_Demo.ViewModels.Transactions
{
    public class AlarmHistoryViewModel
    {
        public ObservableCollection<AlarmHistoryRecord> AlarmRecords { get; }
            = new();

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public RelayCommand ViewCommand { get; }

        public RelayCommand ExitCommand { get; }

        public AlarmHistoryViewModel()
        {
            ViewCommand = new RelayCommand(
                async _ => await ViewAsync());

            ExitCommand = new RelayCommand(
                _ => Exit());

            _ = LoadAllAlarmHistoryAsync();
        }

        public async Task LoadAllAlarmHistoryAsync()
        {
            AlarmRecords.Clear();

            const string query = @"
                SELECT
                    Alarm_Id,
                    Alarm_Description,
                    Alarm_Date,
                    User_Id,
                    Alarm_Status,
                    Ack_Date
                FROM [AlarmTransaction]
                ORDER BY Alarm_Date DESC, Alarm_Time DESC;";

            await LoadFromDatabaseAsync(query);
        }

        private async Task ViewAsync()
        {
            if (FromDate == null || ToDate == null)
            {
                MessageBox.Show(
                    "Please select From Date and To Date.",
                    "Alarm History",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (FromDate > ToDate)
            {
                MessageBox.Show(
                    "From Date cannot be greater than To Date.",
                    "Alarm History",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            await LoadAlarmHistoryAsync(
                FromDate.Value,
                ToDate.Value);
        }

        public async Task LoadAlarmHistoryAsync(
            DateTime fromDate,
            DateTime toDate)
        {
            AlarmRecords.Clear();

            DateTime from = fromDate.Date;
            DateTime to = toDate.Date.AddDays(1);

            const string query = @"
                SELECT
                    Alarm_Id,
                    Alarm_Description,
                    Alarm_Date,
                    User_Id,
                    Alarm_Status,
                    Ack_Date
                FROM [AlarmTransaction]
                WHERE Alarm_Date >= @FromDate
                  AND Alarm_Date < @ToDate
                ORDER BY Alarm_Date DESC, Alarm_Time DESC;";

            await LoadFromDatabaseAsync(
                query,
                from,
                to);
        }

        private async Task LoadFromDatabaseAsync(
            string query,
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            await using SqlConnection connection =
                DbConnection.GetConnection();

            await using SqlCommand command =
                new SqlCommand(query, connection);

            if (fromDate.HasValue && toDate.HasValue)
            {
                command.Parameters.AddWithValue(
                    "@FromDate",
                    fromDate.Value);

                command.Parameters.AddWithValue(
                    "@ToDate",
                    toDate.Value);
            }

            await connection.OpenAsync();

            await using SqlDataReader reader =
                await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                AlarmRecords.Add(new AlarmHistoryRecord
                {
                    AlarmId =
                        Convert.ToInt32(
                            reader["Alarm_Id"]),

                    Description =
                        reader["Alarm_Description"]?.ToString()
                        ?? "",

                    Date =
                        Convert.ToDateTime(
                            reader["Alarm_Date"]),

                    User =
                        reader["User_Id"] == DBNull.Value
                            ? ""
                            : reader["User_Id"].ToString()
                              ?? "",

                    Status =
                        reader["Alarm_Status"]?.ToString()
                        ?? "",

                    AckDate =
                        reader["Ack_Date"] == DBNull.Value
                            ? ""
                            : Convert.ToDateTime(
                                reader["Ack_Date"])
                                .ToString("dd-MM-yyyy")
                });
            }
        }

        private void Exit()
        {
            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(x =>
                    x.DataContext == this)
                ?.Close();
        }
    }

    public class AlarmHistoryRecord
    {
        public int AlarmId { get; set; }

        public string Description { get; set; } = "";

        public DateTime Date { get; set; }

        public string User { get; set; } = "";

        public string Status { get; set; } = "";

        public string AckDate { get; set; } = "";
    }
}