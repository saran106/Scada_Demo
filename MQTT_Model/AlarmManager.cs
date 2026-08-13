using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Scada_Demo.MQTT_Model;
using Scada_Demo.Database;
using Scada_Demo.Common;
namespace Scada_Demo.Models
{
    public static class AlarmManager
    {
        public static readonly List<AlarmDefinition> Definitions = new()
        {
            // ===========================
            // ALARM 1 (MW200)
            // ===========================
            new AlarmDefinition { AlarmNo = 1, Bit = 0,  Text = "Water Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 1, Bit = 1,  Text = "Ice / Silica Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 1, Bit = 2,  Text = "Aggregate 1 Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 1, Bit = 3,  Text = "Aggregate 2 Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 1, Bit = 4,  Text = "Aggregate 3 Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 1, Bit = 5,  Text = "Aggregate 4 Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 1, Bit = 6,  Text = "Aggregate 5 Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 1, Bit = 7,  Text = "Cement 1 Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 1, Bit = 8,  Text = "Asc. Conv. Zero Speed / Pull Cord Fault" },
            new AlarmDefinition { AlarmNo = 1, Bit = 9,  Text = "Ascending Conveyor Tripped" },
            new AlarmDefinition { AlarmNo = 1, Bit = 10, Text = "Mixer Grease Pump Not Fault" },
            new AlarmDefinition { AlarmNo = 1, Bit = 11, Text = "Mixer Zero Speed Fault" },
            new AlarmDefinition { AlarmNo = 1, Bit = 12, Text = "NA" },
            new AlarmDefinition { AlarmNo = 1, Bit = 13, Text = "NA" },
            new AlarmDefinition { AlarmNo = 1, Bit = 14, Text = "NA" },
            new AlarmDefinition { AlarmNo = 1, Bit = 15, Text = "NA" },

            // ===========================
            // ALARM 2 (MW202)
            // ===========================
            new AlarmDefinition { AlarmNo = 2, Bit = 0,  Text = "Mixer Gate Not Closed" },
            new AlarmDefinition { AlarmNo = 2, Bit = 1,  Text = "Skip Not In Bottom" },
            new AlarmDefinition { AlarmNo = 2, Bit = 2,  Text = "Water Batching Time Exceeded" },
            new AlarmDefinition { AlarmNo = 2, Bit = 3,  Text = "Ice / Silica Batching Time Exceeded" },
            new AlarmDefinition { AlarmNo = 2, Bit = 4,  Text = "Aggregate Batching Time Exceeded" },
            new AlarmDefinition { AlarmNo = 2, Bit = 5,  Text = "Cement Batching Time Exceeded" },
            new AlarmDefinition { AlarmNo = 2, Bit = 6,  Text = "Admixture Batching Time Exceeded" },
            new AlarmDefinition { AlarmNo = 2, Bit = 7,  Text = "Skip Run Time Exceeded" },
            new AlarmDefinition { AlarmNo = 2, Bit = 8,  Text = "Cement 2 Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 2, Bit = 9,  Text = "Cement 3 Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 2, Bit = 10, Text = "NA" },
            new AlarmDefinition { AlarmNo = 2, Bit = 11, Text = "Admixture 1 Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 2, Bit = 12, Text = "Admixture 2 Tolerance Exceeded" },
            new AlarmDefinition { AlarmNo = 2, Bit = 13, Text = "NA" },
            new AlarmDefinition { AlarmNo = 2, Bit = 14, Text = "SIWAREX Initialization Error" },
            new AlarmDefinition { AlarmNo = 2, Bit = 15, Text = "NA" },

            // ===========================
            // ALARM 3 (MW204)
            // ===========================
            new AlarmDefinition { AlarmNo = 3, Bit = 0,  Text = "Admixture Discharge Time Exceeded" },
            new AlarmDefinition { AlarmNo = 3, Bit = 1,  Text = "Compressor Overloaded" },
            new AlarmDefinition { AlarmNo = 3, Bit = 2,  Text = "Sand Vibrator Overloaded" },
            new AlarmDefinition { AlarmNo = 3, Bit = 3,  Text = "Aggregate Weigher Not Empty" },
            new AlarmDefinition { AlarmNo = 3, Bit = 4,  Text = "Cement Weigher Not Empty" },
            new AlarmDefinition { AlarmNo = 3, Bit = 5,  Text = "Water Weigher Not Empty" },
            new AlarmDefinition { AlarmNo = 3, Bit = 6,  Text = "Admixture Weigher Not Empty" },
            new AlarmDefinition { AlarmNo = 3, Bit = 7,  Text = "Ice / Silica Weigher Not Empty" },
            new AlarmDefinition { AlarmNo = 3, Bit = 8,  Text = "Slack Steel Fault" },
            new AlarmDefinition { AlarmNo = 3, Bit = 9,  Text = "Single Phase Error" },
            new AlarmDefinition { AlarmNo = 3, Bit = 10, Text = "Skip Thermal Fault" },
            new AlarmDefinition { AlarmNo = 3, Bit = 11, Text = "Mixer / Power Pack Not Healthy" },
            new AlarmDefinition { AlarmNo = 3, Bit = 12, Text = "Cement Screw Conveyor Overload" },
            new AlarmDefinition { AlarmNo = 3, Bit = 13, Text = "Admixture Dosing Pump Overload" },
            new AlarmDefinition { AlarmNo = 3, Bit = 14, Text = "Cement Discharge Time Exceeded" },
            new AlarmDefinition { AlarmNo = 3, Bit = 15, Text = "Water Discharge Time Exceeded" },

            // ===========================
            // ALARM 4 (MW206)
            // ===========================
            new AlarmDefinition { AlarmNo = 4, Bit = 0,  Text = "NOT USED" },
            new AlarmDefinition { AlarmNo = 4, Bit = 1,  Text = "NOT USED" },
            new AlarmDefinition { AlarmNo = 4, Bit = 2,  Text = "Mixer Gate Not Opened / Closed" },
            new AlarmDefinition { AlarmNo = 4, Bit = 3,  Text = "Ice / Silica Discharge Time Exceeded" },
            new AlarmDefinition { AlarmNo = 4, Bit = 4,  Text = "Admixture Discharge Pump Not Healthy" },
            new AlarmDefinition { AlarmNo = 4, Bit = 5,  Text = "Water Discharge Pump Not Healthy" },
            new AlarmDefinition { AlarmNo = 4, Bit = 6,  Text = "" },
            new AlarmDefinition { AlarmNo = 4, Bit = 7,  Text = "" },
            new AlarmDefinition { AlarmNo = 4, Bit = 8,  Text = "Conveyor Motor Overloaded" },
            new AlarmDefinition { AlarmNo = 4, Bit = 9,  Text = "Conveyor Pull Cord Fault" },
            new AlarmDefinition { AlarmNo = 4, Bit = 10, Text = "Conveyor Zero Speed Fault" },
            new AlarmDefinition { AlarmNo = 4, Bit = 11, Text = "AUTO Pause Mode Selected" },
            new AlarmDefinition { AlarmNo = 4, Bit = 12, Text = "Ice / Silica Screw Not Healthy" },
            new AlarmDefinition { AlarmNo = 4, Bit = 13, Text = "Ice / Silica Vibrator Not Healthy" },
            new AlarmDefinition { AlarmNo = 4, Bit = 14, Text = "Batching Gate Not Closed" },
            new AlarmDefinition { AlarmNo = 4, Bit = 15, Text = "Cement Vibrator Not Healthy" }
        };

        private static readonly Dictionary<string, ActiveAlarm>
        _activeAlarms = new();

        private static readonly Dictionary<string, SemaphoreSlim>
            _alarmLocks = new();

        private static readonly object _lockObject = new();

        public static async Task ProcessAlarmAsync(
            int alarmNo,
            int bit,
            bool isActive,
            int batchNo)
        {
            string key = $"{alarmNo}_{bit}";

            AlarmDefinition? definition =
                Definitions.FirstOrDefault(x =>
                    x.AlarmNo == alarmNo &&
                    x.Bit == bit);

            if (definition == null)
                return;

            if (string.IsNullOrWhiteSpace(definition.Text) ||
                definition.Text == "NA" ||
                definition.Text == "NOT USED")
            {
                return;
            }

            // Get a separate lock for this particular alarm
            SemaphoreSlim alarmLock;

            lock (_lockObject)
            {
                if (!_alarmLocks.TryGetValue(key, out alarmLock!))
                {
                    alarmLock = new SemaphoreSlim(1, 1);
                    _alarmLocks[key] = alarmLock;
                }
            }

            // Only one request for this alarm can enter at a time
            await alarmLock.WaitAsync();

            try
            {
                // =====================================================
                // TRUE = ALARM ACTIVE
                // =====================================================

                if (isActive)
                {
                    // Already active in memory
                    if (_activeAlarms.ContainsKey(key))
                    {
                        return;
                    }

                    // Check DB also.
                    // This protects against duplicate insert after
                    // application restart / memory reset.
                    int existingRecordId =
                        await GetActiveAlarmRecordIdAsync(alarmId:
                            ((alarmNo - 1) * 16) + bit + 1);

                    if (existingRecordId > 0)
                    {
                        _activeAlarms[key] = new ActiveAlarm
                        {
                            AlarmId =
                                ((alarmNo - 1) * 16) + bit + 1,

                            AlarmNo = alarmNo,

                            Bit = bit,

                            RecordId = existingRecordId,

                            Description = definition.Text,

                            BatchNo = batchNo,

                            StartTime = DateTime.Now,

                            IsAcknowledged = false
                        };

                        return;
                    }

                    DateTime startTime = DateTime.Now;

                    int alarmId =
                        ((alarmNo - 1) * 16) + bit + 1;

                    // INSERT
                    int recordId =
                        await InsertAlarmAsync(
                            alarmId,
                            definition.Text,
                            startTime,
                            UserSession.UserId,
                            batchNo);

                    // Store active alarm only AFTER successful INSERT
                    _activeAlarms[key] = new ActiveAlarm
                    {
                        AlarmId = alarmId,

                        AlarmNo = alarmNo,

                        Bit = bit,

                        RecordId = recordId,

                        Description = definition.Text,

                        BatchNo = batchNo,

                        StartTime = startTime,

                        IsAcknowledged = false
                    };

                    return;
                }


                // =====================================================
                // FALSE = ALARM CLEARED
                // =====================================================

                if (!isActive)
                {
                    // Check whether alarm is active in memory
                    if (!_activeAlarms.TryGetValue(
                            key,
                            out ActiveAlarm? activeAlarm))
                    {
                        return;
                    }

                    // UPDATE
                    await ClearAlarmAsync(
                        activeAlarm.RecordId);

                    // Remove from active memory
                    _activeAlarms.Remove(key);
                }
            }
            finally
            {
                // Always release the lock
                alarmLock.Release();
            }
        }


        // =========================================================
        // CHECK ACTIVE ALARM IN DATABASE
        // =========================================================

        private static async Task<int> GetActiveAlarmRecordIdAsync(
            int alarmId)
        {
            const string query = @"
        SELECT TOP 1
            Record_Id
        FROM [AlarmTransaction]
        WHERE Alarm_Id = @Alarm_Id
          AND Alarm_Status = 'Active'
        ORDER BY Record_Id DESC;";


            await using SqlConnection connection =
                DbConnection.GetConnection();

            await using SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.AddWithValue(
                "@Alarm_Id",
                alarmId);

            await connection.OpenAsync();

            object? result =
                await command.ExecuteScalarAsync();

            if (result == null ||
                result == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt32(result);
        }


        // =========================================================
        // INSERT
        // =========================================================

        private static async Task<int> InsertAlarmAsync(
            int alarmId,
            string description,
            DateTime alarmTime,
            int userId,
            int batchNo)
        {
            const string query = @"
        INSERT INTO [AlarmTransaction]
        (
            Alarm_Id,
            Alarm_Description,
            Alarm_Date,
            User_Id,
            Alarm_Status,
            Ack_Date,
            alarm_grid_display,
            Alarm_Time,
            Ack_Time,
            Batch_No
        )
        OUTPUT INSERTED.Record_Id
        VALUES
        (
            @Alarm_Id,
            @Alarm_Description,
            @Alarm_Date,
            @User_Id,
            @Alarm_Status,
            NULL,
            @alarm_grid_display,
            @Alarm_Time,
            NULL,
            @Batch_No
        );";


            await using SqlConnection connection =
                DbConnection.GetConnection();

            await using SqlCommand command =
                new SqlCommand(query, connection);


            command.Parameters.AddWithValue(
                "@Alarm_Id",
                alarmId);

            command.Parameters.AddWithValue(
                "@Alarm_Description",
                description);

            command.Parameters.AddWithValue(
                "@Alarm_Date",
                alarmTime.Date);

            command.Parameters.AddWithValue(
                "@User_Id",
                userId);

            command.Parameters.AddWithValue(
                "@Alarm_Status",
                "Active");

            command.Parameters.AddWithValue(
                "@alarm_grid_display",
                1);

            command.Parameters.AddWithValue(
                "@Alarm_Time",
                alarmTime.TimeOfDay);

            command.Parameters.AddWithValue(
                "@Batch_No",
                batchNo);


            await connection.OpenAsync();

            object? result =
                await command.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }


        // =========================================================
        // CLEAR
        // =========================================================

        private static async Task ClearAlarmAsync(
            int recordId)
        {
            const string query = @"
        UPDATE [AlarmTransaction]
        SET
            Alarm_Status = @Alarm_Status
        WHERE Record_Id = @Record_Id;";


            await using SqlConnection connection =
                DbConnection.GetConnection();

            await using SqlCommand command =
                new SqlCommand(query, connection);


            command.Parameters.AddWithValue(
                "@Alarm_Status",
                "Cleared");

            command.Parameters.AddWithValue(
                "@Record_Id",
                recordId);


            await connection.OpenAsync();

            await command.ExecuteNonQueryAsync();
        }


        // =========================================================
        // ACKNOWLEDGE
        // =========================================================

        public static async Task AcknowledgeAlarmAsync(
            int recordId)
        {
            DateTime ackTime = DateTime.Now;

            const string query = @"
        UPDATE [AlarmTransaction]
        SET
            Ack_Date = @Ack_Date,
            Ack_Time = @Ack_Time
        WHERE Record_Id = @Record_Id;";


            await using SqlConnection connection =
                DbConnection.GetConnection();

            await using SqlCommand command =
                new SqlCommand(query, connection);


            command.Parameters.AddWithValue(
                "@Ack_Date",
                ackTime.Date);

            command.Parameters.AddWithValue(
                "@Ack_Time",
                ackTime.TimeOfDay);

            command.Parameters.AddWithValue(
                "@Record_Id",
                recordId);


            await connection.OpenAsync();

            await command.ExecuteNonQueryAsync();


            ActiveAlarm? activeAlarm =
                _activeAlarms.Values.FirstOrDefault(
                    x => x.RecordId == recordId);

            if (activeAlarm != null)
            {
                activeAlarm.IsAcknowledged = true;
            }
        }
    }
}