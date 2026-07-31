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

                    string query = @"SELECT Truck_ID,
                                    Truck_Reg_No
                             FROM Truck_Master
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
    }
}
