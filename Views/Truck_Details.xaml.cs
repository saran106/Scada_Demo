using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using Scada_Demo.Database;

namespace Scada_Demo.Views
{
    public partial class Truck_Details : Window
    {
        public Truck_Details()
        {
            InitializeComponent();
            LoadTrucks();
        }

        private void LoadTrucks()
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT Truck_No FROM TruckMaster ORDER BY Truck_No", con);

                DataTable dt = new();
                da.Fill(dt);

                cmbTruckNo.ItemsSource = dt.DefaultView;
                cmbTruckNo.DisplayMemberPath = "Truck_No";
                cmbTruckNo.SelectedValuePath = "Truck_No";
                cmbTruckNo.SelectedIndex = -1;
                cmbTruckNo.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbTruckNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTruckNo.SelectedValue == null)
                return;

            LoadTruck(cmbTruckNo.SelectedValue.ToString());
        }

        private void LoadTruck(string truckNo)
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    @"SELECT *
                      FROM TruckMaster
                      WHERE Truck_No=@Truck_No", con);

                cmd.Parameters.AddWithValue("@Truck_No", truckNo);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtDriver.Text = dr["Truck_Driver"].ToString();
                    txtCapacity.Text = dr["Truck_Cap"].ToString();
                    txtTruckType.Text = dr["Truck_Type"].ToString();
                    txtMixerGateTimer.Text = dr["Mixer_Gate_Timer"].ToString();
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearControls(bool clearTruck = true)
        {
            if (clearTruck)
            {
                cmbTruckNo.SelectedIndex = -1;
                cmbTruckNo.Text = "";
            }

            txtDriver.Clear();
            txtCapacity.Clear();
            txtTruckType.Clear();
            txtMixerGateTimer.Clear();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cmbTruckNo.Text))
                {
                    MessageBox.Show("Please Enter Truck Number.");
                    return;
                }

                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                string truckNo = cmbTruckNo.Text.Trim();

                SqlCommand checkCmd = new SqlCommand(
                    @"SELECT Truck_No
                      FROM TruckMaster
                      WHERE Truck_No=@Truck_No", con);

                checkCmd.Parameters.AddWithValue("@Truck_No", truckNo);

                object result = checkCmd.ExecuteScalar();

                SqlCommand cmd;

                if (result != null)
                {
                    cmd = new SqlCommand(
                        @"UPDATE TruckMaster
                          SET Truck_Driver=@Truck_Driver,
                              Truck_Cap=@Truck_Cap,
                              Truck_Type=@Truck_Type,
                              Mixer_Gate_Timer=@Mixer_Gate_Timer
                          WHERE Truck_No=@Truck_No", con);
                }
                else
                {
                    cmd = new SqlCommand(
                        @"INSERT INTO TruckMaster
                        (
                            Truck_No,
                            Truck_Driver,
                            Truck_Cap,
                            Truck_Type,
                            Mixer_Gate_Timer
                        )
                        VALUES
                        (
                            @Truck_No,
                            @Truck_Driver,
                            @Truck_Cap,
                            @Truck_Type,
                            @Mixer_Gate_Timer
                        )", con);
                }

                cmd.Parameters.AddWithValue("@Truck_No", truckNo);
                cmd.Parameters.AddWithValue("@Truck_Driver", txtDriver.Text.Trim());
                cmd.Parameters.AddWithValue("@Truck_Cap", Convert.ToDecimal(txtCapacity.Text));
                cmd.Parameters.AddWithValue("@Truck_Type", txtTruckType.Text.Trim());
                cmd.Parameters.AddWithValue("@Mixer_Gate_Timer", Convert.ToInt32(txtMixerGateTimer.Text));

                cmd.ExecuteNonQuery();

                MessageBox.Show(result != null
                    ? "Truck Updated Successfully."
                    : "Truck Saved Successfully.");

                LoadTrucks();
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
                if (cmbTruckNo.SelectedValue == null)
                {
                    MessageBox.Show("Please Select Truck.");
                    return;
                }

                if (MessageBox.Show("Delete this Truck?",
                    "Confirm",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) != MessageBoxResult.Yes)
                    return;

                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    @"DELETE FROM TruckMaster
                      WHERE Truck_No=@Truck_No", con);

                cmd.Parameters.AddWithValue("@Truck_No",
                    cmbTruckNo.SelectedValue.ToString());

                cmd.ExecuteNonQuery();

                MessageBox.Show("Truck Deleted Successfully.");

                LoadTrucks();
                ClearControls();
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