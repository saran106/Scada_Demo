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
    /// Interaction logic for Configuration_Order.xaml
    /// </summary>
    public partial class Configuration_Order : Window
    {
        public Configuration_Order()
        {
            InitializeComponent();

            LoadOrderIds();
            LoadCustomers();
            //LoadSites();
            LoadTrucks();
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
                    cmbCustomer.DisplayMemberPath = "CustomerID";
                    cmbCustomer.SelectedValuePath = "CustomerID";
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

                    string query = @"SELECT Truck_ID,
                                    Truck_Reg_No
                             FROM TruckMaster
                             ORDER BY Truck_Reg_No";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbTruck.ItemsSource = dt.DefaultView;

                    // User-ku kaatradhu
                    cmbTruck.DisplayMemberPath = "Truck_Reg_No";

                    // Save aaguradhu
                    cmbTruck.SelectedValuePath = "Truck_ID";
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

                    // Check Exists
                    SqlCommand chk = new SqlCommand(
                        "SELECT COUNT(*) FROM OrderMaster WHERE Order_ID=@OrderID", con);

                    chk.Parameters.AddWithValue("@OrderID", cmbOrderId.Text.Trim());

                    int count = Convert.ToInt32(chk.ExecuteScalar());

                    SqlCommand cmd;

                    if (count == 0)
                    {
                        cmd = new SqlCommand(@"INSERT INTO OrderMaster
(
Order_ID,
Order_Date,
Cust_ID,
User_ID,
Recipe_ID,
Site,
Truck_ID,
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
Job_No,
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
@TruckID,
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
                        cmd = new SqlCommand(@"UPDATE OrderMaster SET

Order_Date=@OrderDate,
Cust_ID=@CustID,
User_ID=@UserID,
Recipe_ID=@RecipeID,
Site=@Site,
Truck_ID=@TruckID,
Ordered_Qty=@OrderedQty,
Production_Qty=@ProductionQty,
Adjust_Qty=@AdjustQty,
Load_Send_Qty=@LoadQty,
Aggregate_Used=@Aggregate,
Cement_Used=@Cement,
Water_Used=@Water,
Admix_Used=@Admix,
Special_Note=@Special,
Order_Type=@OrderType,
Job_No=@JobNo,
Acc_No=@AccNo,
Max_Cem=@MaxCem,
Cem_Wtr_Ratio=@Ratio

WHERE Order_ID=@OrderID", con);
                    }

                    cmd.Parameters.AddWithValue("@OrderID", cmbOrderId.Text);
                    cmd.Parameters.AddWithValue("@OrderDate", dpOrderDate.SelectedDate ?? DateTime.Now);
                    cmd.Parameters.AddWithValue("@CustID", cmbCustomer.SelectedValue ?? "");
                    cmd.Parameters.AddWithValue("@UserID", "ADMIN");   // Dummy User
                    cmd.Parameters.AddWithValue("@RecipeID", cmbRecipe.Text);
                    cmd.Parameters.AddWithValue("@Site", cmbSite.SelectedValue ?? "");
                    cmd.Parameters.AddWithValue("@TruckID", cmbTruck.SelectedValue ?? "");

                    cmd.Parameters.AddWithValue("@OrderedQty", txtOrderedQty.Text);
                    cmd.Parameters.AddWithValue("@ProductionQty", txtProductionQty.Text);
                    cmd.Parameters.AddWithValue("@AdjustQty", txtAdjustQty.Text);
                    cmd.Parameters.AddWithValue("@LoadQty", txtLoadSentQty.Text);

                    cmd.Parameters.AddWithValue("@Aggregate", txtAggregateUsed.Text);
                    cmd.Parameters.AddWithValue("@Cement", txtCementUsed.Text);
                    cmd.Parameters.AddWithValue("@Water", txtWaterUsed.Text);
                    cmd.Parameters.AddWithValue("@Admix", txtAdmixUsed.Text);

                    cmd.Parameters.AddWithValue("@Special", txtSpecialNote.Text);
                    cmd.Parameters.AddWithValue("@OrderType", txtOrderType.Text);
                    cmd.Parameters.AddWithValue("@JobNo", txtJobNo.Text);
                    cmd.Parameters.AddWithValue("@AccNo", txtAccNo.Text);
                    cmd.Parameters.AddWithValue("@MaxCem", txtMaxCem.Text);
                    cmd.Parameters.AddWithValue("@Ratio", txtWaterRatio.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show(count == 0
                        ? "Order Saved Successfully"
                        : "Order Updated Successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCustomer.SelectedValue != null)
            {
                LoadSites(cmbCustomer.SelectedValue.ToString());
            }
        }

        private void LoadSites(string customerId)
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    string query = @"
                SELECT SiteID,
                       SiteName
                FROM SiteMaster
                WHERE CustomerID=@CustomerID
                ORDER BY SiteName";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

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

        private void cmbOrderId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbOrderId.SelectedValue != null)
            {
                LoadOrder(cmbOrderId.SelectedValue.ToString());
            }
        }

        private void LoadOrder(string orderId)
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(
                        "SELECT * FROM OrderMaster WHERE Order_ID=@OrderID", con);

                    cmd.Parameters.AddWithValue("@OrderID", orderId);

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        dpOrderDate.SelectedDate = Convert.ToDateTime(dr["Order_Date"]);

                        cmbCustomer.SelectedValue = dr["Cust_ID"].ToString();

                        // Customer select aana apram Site load aaganum
                        LoadSites(dr["Cust_ID"].ToString());

                        cmbSite.SelectedValue = dr["Site"].ToString();
                        cmbTruck.SelectedValue = dr["Truck_ID"].ToString();

                        // Recipe later
                        // cmbRecipe.SelectedValue = dr["Recipe_ID"].ToString();

                        txtOrderedQty.Text = dr["Ordered_Qty"].ToString();
                        txtProductionQty.Text = dr["Production_Qty"].ToString();
                        txtAdjustQty.Text = dr["Adjust_Qty"].ToString();
                        txtLoadSentQty.Text = dr["Load_Send_Qty"].ToString();

                        txtAggregateUsed.Text = dr["Aggregate_Used"].ToString();
                        txtCementUsed.Text = dr["Cement_Used"].ToString();
                        txtWaterUsed.Text = dr["Water_Used"].ToString();
                        txtAdmixUsed.Text = dr["Admix_Used"].ToString();

                        txtSpecialNote.Text = dr["Special_Note"].ToString();
                        txtOrderType.Text = dr["Order_Type"].ToString();
                        txtJobNo.Text = dr["Job_No"].ToString();
                        txtAccNo.Text = dr["Acc_No"].ToString();
                        txtMaxCem.Text = dr["Max_Cem"].ToString();
                        txtWaterRatio.Text = dr["Cem_Wtr_Ratio"].ToString();
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
