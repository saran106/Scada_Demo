
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
using Scada_Demo.Database;
using Scada_Demo.Services;
using System.Text.Json;

namespace Scada_Demo.ViewModels.Transactions
{
    public class Start_ProductionViewModel : INotifyPropertyChanged
    {
        #region Collections

        public ObservableCollection<string> ScheduleIds { get; set; } = new();

        public ObservableCollection<string> OrderIds { get; set; } = new();

        public ObservableCollection<string> TruckNos { get; set; } = new();

        public ObservableCollection<string> Drivers { get; set; } = new();

        // Dynamic Recipe Details
        // No separate RecipeDetail.cs required.

        private readonly DataTable _recipeDetailsTable = new();

        public DataView RecipeDetails => _recipeDetailsTable.DefaultView;

        #endregion

        #region Selected Items

        private string _selectedScheduleId;

        public string SelectedScheduleId
        {
            get => _selectedScheduleId;
            set
            {
                if (_selectedScheduleId == value)
                    return;

                _selectedScheduleId = value;

                OnPropertyChanged(nameof(SelectedScheduleId));

                if (!string.IsNullOrWhiteSpace(value))
                {
                    LoadOrders();
                }
                else
                {
                    OrderIds.Clear();
                    SelectedOrderId = string.Empty;
                    ClearOrderDetails();
                }
            }
        }

        private string _selectedOrderId;

        public string SelectedOrderId
        {
            get => _selectedOrderId;
            set
            {
                if (_selectedOrderId == value)
                    return;

                _selectedOrderId = value;

                OnPropertyChanged(nameof(SelectedOrderId));

                if (!string.IsNullOrWhiteSpace(value))
                {
                    LoadOrderDetails();
                }
                else
                {
                    ClearOrderDetails();
                }
            }
        }

        private string _selectedTruckNo;

        public string SelectedTruckNo
        {
            get => _selectedTruckNo;
            set
            {
                _selectedTruckNo = value;
                OnPropertyChanged(nameof(SelectedTruckNo));
            }
        }

        private string _selectedDriver;

        public string SelectedDriver
        {
            get => _selectedDriver;
            set
            {
                _selectedDriver = value;
                OnPropertyChanged(nameof(SelectedDriver));
            }
        }

        #endregion

        #region Properties

        private string _customerID;

        public string CustomerID
        {
            get => _customerID;
            set
            {
                _customerID = value;
                OnPropertyChanged(nameof(CustomerID));
            }
        }

        private string _site;

        public string Site
        {
            get => _site;
            set
            {
                _site = value;
                OnPropertyChanged(nameof(Site));
            }
        }

        private string _recipeID;

        public string RecipeID
        {
            get => _recipeID;
            set
            {
                _recipeID = value;
                OnPropertyChanged(nameof(RecipeID));
            }
        }

        private string _recipeName;

        public string RecipeName
        {
            get => _recipeName;
            set
            {
                _recipeName = value;
                OnPropertyChanged(nameof(RecipeName));
            }
        }

        private string _productionQty;

        public string ProductionQty
        {
            get => _productionQty;
            set
            {
                _productionQty = value;
                OnPropertyChanged(nameof(ProductionQty));
            }
        }

        private string _returnedQty;

        public string ReturnedQty
        {
            get => _returnedQty;
            set
            {
                _returnedQty = value;
                OnPropertyChanged(nameof(ReturnedQty));
            }
        }

        private string _mixerCapacity;

        public string MixerCapacity
        {
            get => _mixerCapacity;
            set
            {
                _mixerCapacity = value;
                OnPropertyChanged(nameof(MixerCapacity));
            }
        }

        private string _mixingTime;

        public string MixingTime
        {
            get => _mixingTime;
            set
            {
                _mixingTime = value;
                OnPropertyChanged(nameof(MixingTime));
            }
        }

        private string _slurrySplit;

        public string SlurrySplit
        {
            get => _slurrySplit;
            set
            {
                _slurrySplit = value;
                OnPropertyChanged(nameof(SlurrySplit));
            }
        }

        #endregion

        #region MQTT

        private readonly MqttPublishSerice _mqttPublishService;

        public ICommand StartCommand { get; }

        private bool _isStarting;

        #endregion

        #region Constructor

        public Start_ProductionViewModel(
            MqttPublishSerice mqttPublishService)
        {
            _mqttPublishService = mqttPublishService;

            // Dynamic Recipe DataTable columns
            _recipeDetailsTable.Columns.Add(
                "Category", typeof(string));

            _recipeDetailsTable.Columns.Add(
                "MaterialName", typeof(string));

            _recipeDetailsTable.Columns.Add(
                "Weight", typeof(decimal));

            StartCommand = new RelayCommand(
                async _ => await StartProductionAsync());

            LoadScheduleIds();
        }

        #endregion

        #region Load Schedule IDs

        private void LoadScheduleIds()
        {
            try
            {
                ScheduleIds.Clear();

                using SqlConnection con = DbConnection.GetConnection();

                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(
                    @"SELECT DISTINCT Sch_ID
                      FROM ScheduleMaster
                      ORDER BY Sch_ID",
                    con);

                DataTable dt = new DataTable();

                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    ScheduleIds.Add(dr["Sch_ID"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Load Orders

        private void LoadOrders()
        {
            try
            {
                OrderIds.Clear();

                SelectedOrderId = string.Empty;

                ClearOrderDetails();

                using SqlConnection con = DbConnection.GetConnection();

                con.Open();

                using SqlCommand cmd = new SqlCommand(
                    "SP_StartBatch_GetOrders", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@Sch_ID", SelectedScheduleId);

                using SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    OrderIds.Add(dr["Order_ID"].ToString());
                }

                dr.Close();

                // Wait for the operator to select an order manually.
                // SelectedOrderId remains empty until the user clicks an order.
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Load Orders Failed: " + ex.Message);
            }
        }

        #endregion

        #region Load Order Details

        private void LoadOrderDetails()
        {
            try
            {
                ClearOrderDetails();

                using SqlConnection con = DbConnection.GetConnection();

                con.Open();

                using SqlCommand cmd = new SqlCommand(
                    "SP_StartBatch_GetOrderDetails", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@Order_ID", SelectedOrderId);

                using SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    CustomerID = dr["Cust_ID"].ToString();

                    Site = dr["SiteName"].ToString();

                    RecipeID = dr["Recipe_ID"].ToString();

                    RecipeName = dr["RecipeName"].ToString();

                    ProductionQty =
                        dr["Production_Qty"].ToString();

                    ReturnedQty = "0";

                    MixerCapacity =
                        dr["MixerCapacity"].ToString();

                    MixingTime =
                        dr["MixingTime"].ToString();

                    SlurrySplit = "0";

                    string truckNo = dr["Truck_No"].ToString();

                    string driver = dr["Truck_Driver"].ToString();

                    if (!string.IsNullOrWhiteSpace(truckNo))
                    {
                        TruckNos.Add(truckNo);
                        SelectedTruckNo = truckNo;
                    }

                    if (!string.IsNullOrWhiteSpace(driver))
                    {
                        Drivers.Add(driver);
                        SelectedDriver = driver;
                    }
                }

                dr.Close();

                // Load dynamic recipe details using Recipe ID and Name
                if (!string.IsNullOrWhiteSpace(RecipeID) &&
                    !string.IsNullOrWhiteSpace(RecipeName))
                {
                    LoadRecipeDetails();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Load Order Details Failed: " + ex.Message);
            }
        }

        #endregion

        #region Load Recipe Details

        private void LoadRecipeDetails()
        {
            try
            {
                _recipeDetailsTable.Rows.Clear();

                using SqlConnection con = DbConnection.GetConnection();

                con.Open();

                using SqlCommand cmd = new SqlCommand(
                    "SP_StartBatch_GetRecipeDetails", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@RecipeID", RecipeID);

                cmd.Parameters.AddWithValue(
                    "@RecipeName", RecipeName);

                using SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    DataRow row = _recipeDetailsTable.NewRow();

                    row["Category"] = dr["Category"].ToString();

                    row["MaterialName"] =
                        dr["MaterialName"].ToString();

                    row["Weight"] =
                        Convert.ToDecimal(dr["Weight"]);

                    _recipeDetailsTable.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                _recipeDetailsTable.Rows.Clear();

                MessageBox.Show(
                    "Recipe Details Loading Failed: " + ex.Message);
            }
        }

        #endregion

        #region Clear Order Details

        private void ClearOrderDetails()
        {
            CustomerID = string.Empty;

            Site = string.Empty;

            RecipeID = string.Empty;

            RecipeName = string.Empty;

            ProductionQty = string.Empty;

            ReturnedQty = "0";

            MixerCapacity = string.Empty;

            MixingTime = string.Empty;

            SlurrySplit = "0";

            TruckNos.Clear();

            Drivers.Clear();

            SelectedTruckNo = string.Empty;

            SelectedDriver = string.Empty;

            _recipeDetailsTable.Rows.Clear();
        }

        #endregion

        #region Start Production

        private async Task StartProductionAsync()
        {
            if (_isStarting)
                return;

            try
            {
                // 1. Validation

                if (string.IsNullOrWhiteSpace(SelectedScheduleId) ||
                    string.IsNullOrWhiteSpace(SelectedOrderId))
                {
                    MessageBox.Show(
                        "Please select Schedule and Order.");

                    return;
                }

                if (string.IsNullOrWhiteSpace(RecipeID) ||
                    string.IsNullOrWhiteSpace(RecipeName))
                {
                    MessageBox.Show(
                        "Recipe details missing.");

                    return;
                }

                if (string.IsNullOrWhiteSpace(SelectedTruckNo))
                {
                    MessageBox.Show(
                        "Please select Truck.");

                    return;
                }

                _isStarting = true;

                // 2. Fetch complete RecipeMaster record
                // Existing MQTT payload remains unchanged.

                Dictionary<string, object?> recipeDetails = new();

                using (SqlConnection con = DbConnection.GetConnection())
                {
                    await con.OpenAsync();

                    using SqlCommand cmd = new SqlCommand(@"
                        SELECT *
                        FROM RecipeMaster
                        WHERE RecipeID = @RecipeID
                          AND RecipeName = @RecipeName", con);

                    cmd.Parameters.AddWithValue(
                        "@RecipeID", RecipeID);

                    cmd.Parameters.AddWithValue(
                        "@RecipeName", RecipeName);

                    using SqlDataReader dr =
                        await cmd.ExecuteReaderAsync();

                    if (!await dr.ReadAsync())
                    {
                        MessageBox.Show(
                            "Selected recipe not found.");

                        return;
                    }

                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        recipeDetails[dr.GetName(i)] =
                            dr.IsDBNull(i)
                            ? null
                            : dr.GetValue(i);
                    }
                }

                // 3. Prepare MQTT Request

                var request = new
                {
                    Type = "StartProduction",

                    activity = "Write",

                    Schedule_ID = SelectedScheduleId,

                    Order_ID = SelectedOrderId,

                    Customer_ID = CustomerID,

                    Site = Site,

                    Truck_No = SelectedTruckNo,

                    Driver = SelectedDriver,

                    Production_Qty = ProductionQty,

                    Recipe = recipeDetails
                };

                // 4. Publish MQTT

                string json = JsonSerializer.Serialize(request);

                await _mqttPublishService.PublishDataMqttTopic(
                    "production/start",
                    json);

                MessageBox.Show(
                    "Start Production request published to Worker Service.\n" +
                    "Waiting for PLC-side confirmation is a separate step.",
                    "Start Production Request",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Start Production Failed!\n{ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                _isStarting = false;
            }
        }

        #endregion


        #region Reset Form

        public void ResetForm()
        {
            // Keep ScheduleIds loaded, but clear the current selection
            // and every value that depends on the selected schedule/order.
            SelectedScheduleId = null;
            SelectedOrderId = null;

            OrderIds.Clear();
            ClearOrderDetails();

            SelectedTruckNo = string.Empty;
            SelectedDriver = string.Empty;

            _recipeDetailsTable.Rows.Clear();
        }

        #endregion

        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
