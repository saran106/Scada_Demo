using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using Scada_Demo.Database;

namespace Scada_Demo.Views
{
    public partial class Site_Details : Window
    {
        public Site_Details()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT CustomerID FROM CustomerMaster ORDER BY CustomerID", con);

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

        private void cmbCustomerID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCustomerID.SelectedValue == null)
                return;

            LoadSites(cmbCustomerID.SelectedValue.ToString());

            ClearControls(false);
        }

        private void LoadSites(string customerId)
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                string query = @"SELECT SiteID,
                                        SiteName
                                 FROM SiteMaster
                                 WHERE CustomerID=@CustomerID
                                 ORDER BY SiteName";

                SqlDataAdapter da = new SqlDataAdapter(query, con);

                da.SelectCommand.Parameters.AddWithValue("@CustomerID", customerId);

                DataTable dt = new();
                da.Fill(dt);

                cmbSiteID.ItemsSource = dt.DefaultView;

                // User-ku SiteName theriyum
                cmbSiteID.DisplayMemberPath = "SiteName";

                // Backend-la SiteID store aagum
                cmbSiteID.SelectedValuePath = "SiteID";

                cmbSiteID.SelectedIndex = -1;
                cmbSiteID.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbSiteID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCustomerID.SelectedValue == null ||
                cmbSiteID.SelectedValue == null)
                return;

            LoadSite(
                cmbCustomerID.SelectedValue.ToString(),
                Convert.ToInt32(cmbSiteID.SelectedValue));
        }

        private void LoadSite(string customerId, int siteId)
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                string query = @"SELECT *
                                 FROM SiteMaster
                                 WHERE CustomerID=@CustomerID
                                 AND SiteID=@SiteID";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                cmd.Parameters.AddWithValue("@SiteID", siteId);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtAddress1.Text = dr["Address1"].ToString();
                    txtAddress2.Text = dr["Address2"].ToString();
                    txtAddress3.Text = dr["Address3"].ToString();
                    txtCity.Text = dr["City"].ToString();
                    txtPincode.Text = dr["Pincode"].ToString();
                    txtPhone.Text = dr["Phone"].ToString();
                    txtContact.Text = dr["ContactPerson"].ToString();
                    txtDistance.Text = dr["Distance"].ToString();
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearControls(bool clearSite = true)
        {
            if (clearSite)
            {
                cmbSiteID.ItemsSource = null;
                cmbSiteID.Text = "";
            }

            txtAddress1.Clear();
            txtAddress2.Clear();
            txtAddress3.Clear();
            txtCity.Clear();
            txtPincode.Clear();
            txtPhone.Clear();
            txtContact.Clear();
            txtDistance.Clear();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbCustomerID.SelectedValue == null)
                {
                    MessageBox.Show("Please select Customer.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(cmbSiteID.Text))
                {
                    MessageBox.Show("Please enter Site Name.");
                    return;
                }

                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                string customerId = cmbCustomerID.SelectedValue.ToString();
                string siteName = cmbSiteID.Text.Trim();

                // Check whether Site already exists
                SqlCommand checkCmd = new SqlCommand(
                    @"SELECT SiteID
                      FROM SiteMaster
                      WHERE CustomerID=@CustomerID
                      AND SiteName=@SiteName", con);

                checkCmd.Parameters.AddWithValue("@CustomerID", customerId);
                checkCmd.Parameters.AddWithValue("@SiteName", siteName);

                object result = checkCmd.ExecuteScalar();

                SqlCommand cmd;

                if (result != null)
                {
                    // UPDATE
                    cmd = new SqlCommand(
                        @"UPDATE SiteMaster
                          SET Address1=@Address1,
                              Address2=@Address2,
                              Address3=@Address3,
                              City=@City,
                              Pincode=@Pincode,
                              Phone=@Phone,
                              ContactPerson=@ContactPerson,
                              Distance=@Distance
                          WHERE SiteID=@SiteID", con);

                    cmd.Parameters.AddWithValue("@SiteID", Convert.ToInt32(result));
                }
                else
                {
                    // INSERT
                    cmd = new SqlCommand(
                        @"INSERT INTO SiteMaster
                        (
                            CustomerID,
                            SiteName,
                            Address1,
                            Address2,
                            Address3,
                            City,
                            Pincode,
                            Phone,
                            ContactPerson,
                            Distance,
                            IsActive,
                            CreatedDate
                        )
                        VALUES
                        (
                            @CustomerID,
                            @SiteName,
                            @Address1,
                            @Address2,
                            @Address3,
                            @City,
                            @Pincode,
                            @Phone,
                            @ContactPerson,
                            @Distance,
                            1,
                            GETDATE()
                        )", con);
                }

                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                cmd.Parameters.AddWithValue("@SiteName", siteName);
                cmd.Parameters.AddWithValue("@Address1", txtAddress1.Text.Trim());
                cmd.Parameters.AddWithValue("@Address2", txtAddress2.Text.Trim());
                cmd.Parameters.AddWithValue("@Address3", txtAddress3.Text.Trim());
                cmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                cmd.Parameters.AddWithValue("@Pincode", txtPincode.Text.Trim());
                cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@ContactPerson", txtContact.Text.Trim());
                cmd.Parameters.AddWithValue("@Distance", txtDistance.Text.Trim());

                cmd.ExecuteNonQuery();

                MessageBox.Show(result != null
                    ? "Site Updated Successfully."
                    : "Site Saved Successfully.");

                LoadSites(customerId);
                ClearControls(false);
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
                if (cmbSiteID.SelectedValue == null)
                {
                    MessageBox.Show("Please select Site.");
                    return;
                }

                if (MessageBox.Show("Delete this Site?",
                    "Confirm",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) != MessageBoxResult.Yes)
                    return;

                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    @"DELETE FROM SiteMaster
                      WHERE SiteID=@SiteID", con);

                cmd.Parameters.AddWithValue("@SiteID",
                    Convert.ToInt32(cmbSiteID.SelectedValue));

                cmd.ExecuteNonQuery();

                MessageBox.Show("Site Deleted Successfully.");

                LoadSites(cmbCustomerID.SelectedValue.ToString());

                ClearControls(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}