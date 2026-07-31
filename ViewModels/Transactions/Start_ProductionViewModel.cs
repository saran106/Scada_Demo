using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Windows;
using Scada_Demo.Database;

namespace Scada_Demo.ViewModels.Transactions
{
    public class Start_ProductionViewModel : INotifyPropertyChanged
    {
        #region Collections

        public ObservableCollection<string> ScheduleIds { get; set; } = new();

        public ObservableCollection<string> OrderIds { get; set; } = new();

        public ObservableCollection<string> TruckNos { get; set; } = new();

        public ObservableCollection<string> Drivers { get; set; } = new();

        #endregion

        #region Selected Items

        private string _selectedScheduleId;
        public string SelectedScheduleId
        {
            get => _selectedScheduleId;
            set
            {
                _selectedScheduleId = value;
                OnPropertyChanged(nameof(SelectedScheduleId));

                if (!string.IsNullOrWhiteSpace(value))
                    LoadOrders();
            }
        }

        private string _selectedOrderId;
        public string SelectedOrderId
        {
            get => _selectedOrderId;
            set
            {
                _selectedOrderId = value;
                OnPropertyChanged(nameof(SelectedOrderId));

                if (!string.IsNullOrWhiteSpace(value))
                    LoadOrderDetails();
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

        public Start_ProductionViewModel()
        {
            LoadScheduleIds();
        }

        #region Load Schedule

        private void LoadScheduleIds()
        {
            ScheduleIds.Clear();

            using (SqlConnection con = DbConnection.GetConnection())
            {
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT Sch_ID FROM ScheduleMaster ORDER BY Sch_ID", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    ScheduleIds.Add(dr["Sch_ID"].ToString());
                }
            }
        }

        #endregion

        // Next
        private void LoadOrders()
        {
            try
            {
                OrderIds.Clear();
                // Add these
                CustomerID = "";
                Site = "";
                RecipeID = "";
                RecipeName = "";
                ProductionQty = "";
                ReturnedQty = "0";
                MixerCapacity = "";
                MixingTime = "";
                SlurrySplit = "0";

                TruckNos.Clear();
                Drivers.Clear();

                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SP_StartBatch_GetOrders", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Sch_ID", SelectedScheduleId);

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        OrderIds.Add(dr["Order_ID"].ToString());
                    }

                    dr.Close();
                }

                // Optional
                if (OrderIds.Count > 0)
                    SelectedOrderId = OrderIds[0];
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void LoadOrderDetails()
        {
            try
            {
                TruckNos.Clear();
                Drivers.Clear();

                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SP_StartBatch_GetOrderDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Order_ID", SelectedOrderId);

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        CustomerID = dr["Cust_ID"].ToString();
                        Site = dr["SiteName"].ToString();
                        RecipeID = dr["Recipe_ID"].ToString();
                        RecipeName = dr["RecipeName"].ToString();

                        ProductionQty = dr["Production_Qty"].ToString();
                       ReturnedQty = "0";

                        MixerCapacity = dr["MixerCapacity"].ToString();
                        MixingTime = dr["MixingTime"].ToString();
                        SlurrySplit ="0";

                        TruckNos.Add(dr["Truck_No"].ToString());
                        SelectedTruckNo = dr["Truck_No"].ToString();

                        Drivers.Add(dr["Truck_Driver"].ToString());
                        SelectedDriver = dr["Truck_Driver"].ToString();
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}