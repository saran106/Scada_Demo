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
using Microsoft.Data.SqlClient;
using System.Data;
using Scada_Demo.Database;

namespace Scada_Demo.Views
{
    /// <summary>
    /// Interaction logic for Customer_Master.xaml
    /// </summary>
    public partial class Customer_Master : Window
    {
        public Customer_Master()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();

                if (string.IsNullOrWhiteSpace(cmbCustomerID.Text))
                {
                    MessageBox.Show("Enter Customer ID");
                    return;
                }
                con.Open();

                string checkQuery = "SELECT COUNT(*) FROM CustomerMaster WHERE CustomerID=@CustomerID";

                SqlCommand checkCmd = new(checkQuery, con);

                checkCmd.Parameters.AddWithValue("@CustomerID", cmbCustomerID.Text.Trim());

                bool isExists = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0;

                string query;

                if (isExists)
                {
                    query = @"UPDATE CustomerMaster
                      SET CustomerName=@CustomerName,
                          Address1=@Address1,
                          City=@City,
                          Pincode=@Pincode,
                          Phone=@Phone,
                          ContactPerson=@ContactPerson,
                          RegistrationNo=@RegistrationNo,
                          GSTNo=@GSTNo
                      WHERE CustomerID=@CustomerID";
                }
                else
                {
                    query = @"INSERT INTO CustomerMaster
                      (
                          CustomerID,
                          CustomerName,
                          Address1,
                          City,
                          Pincode,
                          Phone,
                          ContactPerson,
                          RegistrationNo,
                          GSTNo
                      )
                      VALUES
                      (
                          @CustomerID,
                          @CustomerName,
                          @Address1,
                          @City,
                          @Pincode,
                          @Phone,
                          @ContactPerson,
                          @RegistrationNo,
                          @GSTNo
                      )";
                }

                SqlCommand cmd = new(query, con);

                cmd.Parameters.AddWithValue("@CustomerID", cmbCustomerID.Text.Trim());
                cmd.Parameters.AddWithValue("@CustomerName", txtCustomerName.Text.Trim());
                cmd.Parameters.AddWithValue("@Address1", txtAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                cmd.Parameters.AddWithValue("@Pincode", txtPincode.Text.Trim());
                cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@ContactPerson", txtContact.Text.Trim());
                cmd.Parameters.AddWithValue("@RegistrationNo", txtRegNo.Text.Trim());
                cmd.Parameters.AddWithValue("@GSTNo", txtGST.Text.Trim());

                cmd.ExecuteNonQuery();

                MessageBox.Show(isExists
                    ? "Customer Updated Successfully."
                    : "Customer Saved Successfully.");

                ClearControls();
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
                using SqlConnection con = DbConnection.GetConnection();

                con.Open();

                string query = @"SELECT CustomerID
                         FROM CustomerMaster
                         ORDER BY CustomerID";

                SqlDataAdapter da = new(query, con);

                DataTable dt = new();

                da.Fill(dt);

                cmbCustomerID.ItemsSource = dt.DefaultView;
                cmbCustomerID.DisplayMemberPath = "CustomerID";
                cmbCustomerID.SelectedValuePath = "CustomerID";
                cmbCustomerID.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearControls()
        {
            txtCustomerName.Clear();
            txtAddress.Clear();
            txtCity.Clear();
            txtPincode.Clear();
            txtPhone.Clear();
            txtContact.Clear();
            txtRegNo.Clear();
            txtGST.Clear();

            txtCustomerName.Focus();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Delete button clicked.");
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cmbCustomerID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCustomerID.SelectedValue == null)
                return;

            LoadCustomer(cmbCustomerID.SelectedValue.ToString());
        }

        private void LoadCustomer(string customerId)
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();

                con.Open();

                string query = @"SELECT *
                         FROM CustomerMaster
                         WHERE CustomerID=@CustomerID";

                using SqlCommand cmd = new(query, con);

                cmd.Parameters.AddWithValue("@CustomerID", customerId);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtCustomerName.Text = dr["CustomerName"].ToString();
                    txtAddress.Text = dr["Address1"].ToString();
                    txtCity.Text = dr["City"].ToString();
                    txtPincode.Text = dr["Pincode"].ToString();
                    txtPhone.Text = dr["Phone"].ToString();
                    txtContact.Text = dr["ContactPerson"].ToString();
                    txtRegNo.Text = dr["RegistrationNo"].ToString();
                    txtGST.Text = dr["GSTNo"].ToString();
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
