using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using Microsoft.Data.SqlClient;
using Scada_Demo.Database;

namespace Scada_Demo.Views
{
    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class Order : Window
    {
        public Order()
        {
            InitializeComponent();

            LoadOrderIds();
            LoadCustomers();
            LoadSites();
            LoadTrucks();
            LoadRecipes();
        }
        private void LoadRecipes()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    string query = @"
                SELECT RecipeID
                FROM RecipeMaster
                ORDER BY RecipeID";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbRecipe.ItemsSource = dt.DefaultView;

                    cmbRecipe.DisplayMemberPath = "RecipeID";
                    cmbRecipe.SelectedValuePath = "RecipeID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadOrderIds()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(
                        "SELECT Order_ID FROM OrderMaster ORDER BY Order_ID", con);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbOrderId.ItemsSource = dt.DefaultView;
                    cmbOrderId.DisplayMemberPath = "Order_ID";
                    cmbOrderId.SelectedValuePath = "Order_ID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadCustomers()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(
                    @"SELECT CustomerID,
                     CustomerName
              FROM CustomerMaster
              ORDER BY CustomerName", con);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbCustomer.ItemsSource = dt.DefaultView;
                    cmbCustomer.DisplayMemberPath = "CustomerName";
                    cmbCustomer.SelectedValuePath = "CustomerID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void cmbOrderId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbOrderId.SelectedValue == null)
                return;

            LoadOrder(cmbOrderId.SelectedValue.ToString());
        }
        private void LoadOrder(string orderId)
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    string query = @"
                SELECT *
                FROM OrderMaster
                WHERE Order_ID = @OrderID";

                    using SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@OrderID", orderId);

                    using SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        dpOrderDate.SelectedDate =
                            Convert.ToDateTime(dr["Order_Date"]);

                        cmbCustomer.SelectedValue =
                            dr["Cust_ID"].ToString().Trim();

                        cmbRecipe.SelectedValue =
                            dr["Recipe_ID"].ToString().Trim();

                        cmbSite.SelectedValue =
                            dr["Site"].ToString().Trim();

                        cmbTruck.SelectedValue =
                            dr["Truck_No"].ToString().Trim();

                        txtOrderedQty.Text =
                            dr["Ordered_Qty"].ToString();

                        txtProductionQty.Text =
                            dr["Production_Qty"].ToString();

                        txtAdjustQty.Text =
                            dr["Adjust_Qty"].ToString();

                        txtLoadSentQty.Text =
                            dr["Load_Send_Qty"].ToString();

                        txtAggregateUsed.Text =
                            dr["Aggregate_Used"].ToString().Trim();

                        txtCementUsed.Text =
                            dr["Cement_Used"].ToString().Trim();

                        txtWaterUsed.Text =
                            dr["Water_Used"].ToString().Trim();

                        txtAdmixUsed.Text =
                            dr["Admix_Used"].ToString().Trim();

                        txtSpecialNote.Text =
                            dr["Special_Note"].ToString().Trim();

                        txtOrderType.Text =
                            dr["Order_Type"].ToString().Trim();

                        txtJobNo.Text =
                            dr["job_no"].ToString().Trim();

                        txtAccNo.Text =
                            dr["Acc_No"].ToString().Trim();

                        txtMaxCem.Text =
                            dr["Max_Cem"].ToString().Trim();

                        txtWaterRatio.Text =
                            dr["Cem_Wtr_Ratio"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadSites()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(
                    @"SELECT SiteID,
                     SiteName
              FROM SiteMaster
              ORDER BY SiteName", con);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbSite.ItemsSource = dt.DefaultView;
                    cmbSite.DisplayMemberPath = "SiteName";
                    cmbSite.SelectedValuePath = "SiteID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadTrucks()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    string query = @"
                SELECT Truck_No
                FROM TruckMaster
                ORDER BY Truck_No";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbTruck.ItemsSource = dt.DefaultView;

                    cmbTruck.DisplayMemberPath = "Truck_No";
                    cmbTruck.SelectedValuePath = "Truck_No";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveOrder();
        }
        private void SaveOrder()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    // Check whether Order already exists
                    SqlCommand chk = new SqlCommand(
                        "SELECT COUNT(*) FROM OrderMaster WHERE Order_ID = @OrderID",
                        con);

                    chk.Parameters.AddWithValue(
                        "@OrderID",
                        cmbOrderId.Text.Trim());

                    int count = Convert.ToInt32(chk.ExecuteScalar());

                    SqlCommand cmd;

                    if (count == 0)
                    {
                        // INSERT
                        cmd = new SqlCommand(@"
                    INSERT INTO OrderMaster
                    (
                        Order_ID,
                        Order_Date,
                        Cust_ID,
                        User_ID,
                        Recipe_ID,
                        Site,
                        Truck_No,
                        Ordered_Qty,
                        Production_Qty,
                        Adjust_Qty,
                        Load_Send_Qty,
                        Aggregate_Used,
                        Cement_Used,
                        Water_Used,
                        Admix_Used,
                        Special_Note,
                        Order_Type,
                        job_no,
                        Acc_No,
                        Max_Cem,
                        Cem_Wtr_Ratio
                    )
                    VALUES
                    (
                        @OrderID,
                        @OrderDate,
                        @CustID,
                        @UserID,
                        @RecipeID,
                        @Site,
                        @TruckNo,
                        @OrderedQty,
                        @ProductionQty,
                        @AdjustQty,
                        @LoadQty,
                        @Aggregate,
                        @Cement,
                        @Water,
                        @Admix,
                        @Special,
                        @OrderType,
                        @JobNo,
                        @AccNo,
                        @MaxCem,
                        @Ratio
                    )", con);
                    }
                    else
                    {
                        // UPDATE
                        cmd = new SqlCommand(@"
                    UPDATE OrderMaster
                    SET
                        Order_Date = @OrderDate,
                        Cust_ID = @CustID,
                        User_ID = @UserID,
                        Recipe_ID = @RecipeID,
                        Site = @Site,
                        Truck_No = @TruckNo,
                        Ordered_Qty = @OrderedQty,
                        Production_Qty = @ProductionQty,
                        Adjust_Qty = @AdjustQty,
                        Load_Send_Qty = @LoadQty,
                        Aggregate_Used = @Aggregate,
                        Cement_Used = @Cement,
                        Water_Used = @Water,
                        Admix_Used = @Admix,
                        Special_Note = @Special,
                        Order_Type = @OrderType,
                        job_no = @JobNo,
                        Acc_No = @AccNo,
                        Max_Cem = @MaxCem,
                        Cem_Wtr_Ratio = @Ratio
                    WHERE Order_ID = @OrderID",
                            con);
                    }

                    // =========================
                    // REQUIRED FIELDS
                    // =========================

                    cmd.Parameters.AddWithValue(
                        "@OrderID",
                        cmbOrderId.Text.Trim());

                    cmd.Parameters.AddWithValue(
                        "@OrderDate",
                        dpOrderDate.SelectedDate ?? DateTime.Now);

                    cmd.Parameters.AddWithValue(
                        "@CustID",
                        cmbCustomer.SelectedValue?.ToString() ?? "");

                    cmd.Parameters.AddWithValue(
                        "@UserID",
                        "ADMIN");

                    cmd.Parameters.AddWithValue(
                        "@RecipeID",
                        cmbRecipe.SelectedValue?.ToString() ?? "");

                    cmd.Parameters.AddWithValue(
                        "@Site",
                        cmbSite.SelectedValue?.ToString() ?? "");

                    // Truck_No is nullable
                    cmd.Parameters.AddWithValue(
                        "@TruckNo",
                        cmbTruck.SelectedValue?.ToString() ?? (object)DBNull.Value);


                    // =========================
                    // DECIMAL VALUES
                    // =========================

                    decimal.TryParse(txtOrderedQty.Text, out decimal orderedQty);
                    decimal.TryParse(txtProductionQty.Text, out decimal productionQty);
                    decimal.TryParse(txtAdjustQty.Text, out decimal adjustQty);
                    decimal.TryParse(txtLoadSentQty.Text, out decimal loadQty);

                    cmd.Parameters.Add("@OrderedQty", SqlDbType.Decimal)
                        .Value = orderedQty;

                    cmd.Parameters.Add("@ProductionQty", SqlDbType.Decimal)
                        .Value = productionQty;

                    cmd.Parameters.Add("@AdjustQty", SqlDbType.Decimal)
                        .Value = adjustQty;

                    cmd.Parameters.Add("@LoadQty", SqlDbType.Decimal)
                        .Value = loadQty;


                    // =========================
                    // NCHAR VALUES
                    // =========================

                    cmd.Parameters.AddWithValue(
                        "@Aggregate",
                        txtAggregateUsed.Text.Trim());

                    cmd.Parameters.AddWithValue(
                        "@Cement",
                        txtCementUsed.Text.Trim());

                    cmd.Parameters.AddWithValue(
                        "@Water",
                        txtWaterUsed.Text.Trim());

                    cmd.Parameters.AddWithValue(
                        "@Admix",
                        txtAdmixUsed.Text.Trim());

                    cmd.Parameters.AddWithValue(
                        "@Special",
                        txtSpecialNote.Text.Trim());

                    cmd.Parameters.AddWithValue(
                        "@OrderType",
                        txtOrderType.Text.Trim());

                    cmd.Parameters.AddWithValue(
                        "@JobNo",
                        txtJobNo.Text.Trim());

                    cmd.Parameters.AddWithValue(
                        "@AccNo",
                        txtAccNo.Text.Trim());

                    cmd.Parameters.AddWithValue(
                        "@MaxCem",
                        txtMaxCem.Text.Trim());

                    cmd.Parameters.AddWithValue(
                        "@Ratio",
                        txtWaterRatio.Text.Trim());


                    // =========================
                    // EXECUTE
                    // =========================

                    cmd.ExecuteNonQuery();

                    MessageBox.Show(
                        count == 0
                            ? "Order Saved Successfully"
                            : "Order Updated Successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cmbOrderId.Text))
                {
                    MessageBox.Show("Select Order ID.");
                    return;
                }

                MessageBoxResult result = MessageBox.Show(
                    "Are you sure you want to delete this order?",
                    "Delete Order",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result != MessageBoxResult.Yes)
                    return;

                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    string query = @"
                DELETE FROM OrderMaster
                WHERE Order_ID = @OrderID";

                    using SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue(
                        "@OrderID",
                        cmbOrderId.Text.Trim());

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show("Order Deleted Successfully.");

                        ClearOrderControls();
                        LoadOrderIds();
                    }
                    else
                    {
                        MessageBox.Show("Order not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearOrderControls()
        {
            cmbOrderId.SelectedIndex = -1;
            cmbCustomer.SelectedIndex = -1;
            cmbRecipe.SelectedIndex = -1;
            cmbSite.SelectedIndex = -1;
            cmbTruck.SelectedIndex = -1;

            dpOrderDate.SelectedDate = null;

            txtOrderedQty.Clear();
            txtProductionQty.Clear();
            txtAdjustQty.Clear();
            txtLoadSentQty.Clear();

            txtAggregateUsed.Clear();
            txtCementUsed.Clear();
            txtWaterUsed.Clear();
            txtAdmixUsed.Clear();
            txtSpecialNote.Clear();
            txtOrderType.Clear();
            txtJobNo.Clear();
            txtAccNo.Clear();
            txtMaxCem.Clear();
            txtWaterRatio.Clear();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
