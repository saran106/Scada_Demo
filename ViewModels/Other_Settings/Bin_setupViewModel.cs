
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
using Scada_Demo.Database;
using Scada_Demo.MQTT_Model;
using Scada_Demo.Services;

namespace Scada_Demo.ViewModels.Other_Settings
{
    public class Bin_SetupViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<BinSetupItem> Bins { get; set; }
            = new ObservableCollection<BinSetupItem>();

        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }

        public Bin_SetupViewModel()
        {
            SaveCommand = new RelayCommand(_ => SaveBinSetup());
            LoadCommand = new RelayCommand(_ => LoadBinSetup());

            LoadBinSetup();
        }

        // =========================================
        // DEFAULT FIXED BINS
        // =========================================

        private void EnsureDefaultBins()
        {
            var defaultBins = new[]
            {
                // Aggregate
                new BinSetupItem { BinCode = "Aggregate1", BinType = "Aggregate", BinNumber = 1 },
                new BinSetupItem { BinCode = "Aggregate2", BinType = "Aggregate", BinNumber = 2 },
                new BinSetupItem { BinCode = "Aggregate3", BinType = "Aggregate", BinNumber = 3 },
                new BinSetupItem { BinCode = "Aggregate4", BinType = "Aggregate", BinNumber = 4 },
                new BinSetupItem { BinCode = "Aggregate5", BinType = "Aggregate", BinNumber = 5 },
                new BinSetupItem { BinCode = "Aggregate6", BinType = "Aggregate", BinNumber = 6 },

                // Cement
                new BinSetupItem { BinCode = "Cement1", BinType = "Cement", BinNumber = 1 },
                new BinSetupItem { BinCode = "Cement2", BinType = "Cement", BinNumber = 2 },
                new BinSetupItem { BinCode = "Cement3", BinType = "Cement", BinNumber = 3 },
                new BinSetupItem { BinCode = "Cement4", BinType = "Cement", BinNumber = 4 },
                new BinSetupItem { BinCode = "Cement5", BinType = "Cement", BinNumber = 5 },

                // Water
                new BinSetupItem { BinCode = "Water1", BinType = "Water", BinNumber = 1 },
                new BinSetupItem { BinCode = "Water2", BinType = "Water", BinNumber = 2 },
                new BinSetupItem { BinCode = "Water3", BinType = "Water", BinNumber = 3 },

                // Admixture
                new BinSetupItem { BinCode = "Admixture1", BinType = "Admixture", BinNumber = 1 },
                new BinSetupItem { BinCode = "Admixture2", BinType = "Admixture", BinNumber = 2 },
                new BinSetupItem { BinCode = "Admixture3", BinType = "Admixture", BinNumber = 3 },
                new BinSetupItem { BinCode = "Admixture4", BinType = "Admixture", BinNumber = 4 },

                // Silica
                new BinSetupItem { BinCode = "Silica1", BinType = "Silica", BinNumber = 1 }
            };

            foreach (var item in defaultBins)
            {
                if (!Bins.Any(x =>
                    string.Equals(
                        x.BinCode,
                        item.BinCode,
                        StringComparison.OrdinalIgnoreCase)))
                {
                    Bins.Add(item);
                }
            }
        }

        // =========================================
        // LOAD BIN DETAILS
        // =========================================

        public void LoadBinSetup()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    string query = @"
                        SELECT
                            BinCode,
                            BinType,
                            BinNumber,
                            MaterialName
                        FROM dbo.BinSetup
                        ORDER BY
                            CASE BinType
                                WHEN 'Aggregate' THEN 1
                                WHEN 'Cement' THEN 2
                                WHEN 'Water' THEN 3
                                WHEN 'Admixture' THEN 4
                                WHEN 'Silica' THEN 5
                                ELSE 6
                            END,
                            BinNumber";

                    using SqlCommand cmd = new SqlCommand(query, con);
                    using SqlDataReader dr = cmd.ExecuteReader();

                    Bins.Clear();

                    while (dr.Read())
                    {
                        Bins.Add(new BinSetupItem
                        {
                            BinCode = dr["BinCode"].ToString(),
                            BinType = dr["BinType"].ToString(),
                            BinNumber = Convert.ToInt32(dr["BinNumber"]),

                            MaterialName = dr["MaterialName"] == DBNull.Value
                                ? string.Empty
                                : dr["MaterialName"].ToString()
                        });
                    }
                }

                // Add missing fixed bins to the collection.
                // Existing material names remain unchanged.
                EnsureDefaultBins();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load Bin Setup!\n" + ex.Message);
            }
        }

        // =========================================
        // SAVE BIN DETAILS
        // =========================================


        private void SaveBinSetup()
        {
            try
            {
                EnsureDefaultBins();

                // 1. Validate duplicate names in the final screen values

                var enteredNames = Bins
                    .Where(x => !string.IsNullOrWhiteSpace(x.MaterialName))
                    .Select(x => x.MaterialName.Trim())
                    .ToList();

                var duplicates = enteredNames
                    .GroupBy(x => x, StringComparer.OrdinalIgnoreCase)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if (duplicates.Any())
                {
                    MessageBox.Show(
                        "Duplicate Material Names are not allowed:\n" +
                        string.Join(", ", duplicates));

                    return;
                }

                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    using SqlTransaction transaction = con.BeginTransaction();

                    try
                    {
                        // STEP 1:
                        // Temporarily clear existing names for these bins.
                        // This releases the unique index values for swapping.

                        foreach (var bin in Bins)
                        {
                            string clearQuery = @"
                        UPDATE dbo.BinSetup
                        SET
                            MaterialName = NULL,
                            UpdatedAt = GETDATE()
                        WHERE BinCode = @BinCode";

                            using SqlCommand clearCmd =
                                new SqlCommand(clearQuery, con, transaction);

                            clearCmd.Parameters.Add(
                                "@BinCode",
                                SqlDbType.VarChar, 30).Value = bin.BinCode;

                            clearCmd.ExecuteNonQuery();
                        }

                        // STEP 2:
                        // Assign the final names to their respective bins.

                        foreach (var bin in Bins)
                        {
                            string query = @"
                        UPDATE dbo.BinSetup
                        SET
                            MaterialName = @MaterialName,
                            UpdatedAt = GETDATE()
                        WHERE BinCode = @BinCode;

                        IF @@ROWCOUNT = 0
                        BEGIN
                            INSERT INTO dbo.BinSetup
                            (
                                BinCode,
                                BinType,
                                BinNumber,
                                MaterialName
                            )
                            VALUES
                            (
                                @BinCode,
                                @BinType,
                                @BinNumber,
                                @MaterialName
                            );
                        END";

                            using SqlCommand cmd =
                                new SqlCommand(query, con, transaction);

                            cmd.Parameters.Add(
                                "@BinCode",
                                SqlDbType.VarChar, 30).Value = bin.BinCode;

                            cmd.Parameters.Add(
                                "@BinType",
                                SqlDbType.VarChar, 30).Value = bin.BinType;

                            cmd.Parameters.Add(
                                "@BinNumber",
                                SqlDbType.Int).Value = bin.BinNumber;

                            string materialName =
                                string.IsNullOrWhiteSpace(bin.MaterialName)
                                    ? null
                                    : bin.MaterialName.Trim();

                            cmd.Parameters.Add(
                                "@MaterialName",
                                SqlDbType.NVarChar, 100).Value =
                                    (object)materialName ?? DBNull.Value;

                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                MessageBox.Show(
                    "Bin Setup Saved Successfully!",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                LoadBinSetup();
            }
            catch (SqlException ex) when (
                ex.Number == 2601 || ex.Number == 2627)
            {
                MessageBox.Show(
                    "This Material Name is already assigned to another Bin.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Bin Setup Save Failed!\n" + ex.Message);
            }
        }


        // =========================================
        // PROPERTY CHANGED
        // =========================================

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}
