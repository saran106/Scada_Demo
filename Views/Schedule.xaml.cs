using Microsoft.Data.SqlClient;
using Scada_Demo.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace Scada_Demo.Views
{
    public partial class Schedule : Window
    {
        public Schedule()
        {
            InitializeComponent();
            LoadAvailableOrders();
            LoadScheduleIDs();
        }

        private void LoadScheduleIDs()
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT DISTINCT Sch_ID FROM ScheduleMaster ORDER BY Sch_ID", con);

                DataTable dt = new();
                da.Fill(dt);

                cmbScheduleID.ItemsSource = dt.DefaultView;
                cmbScheduleID.DisplayMemberPath = "Sch_ID";
                cmbScheduleID.SelectedValuePath = "Sch_ID";
                cmbScheduleID.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void cmbScheduleID_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbScheduleID.SelectedValue == null)
                return;

            LoadSchedule(cmbScheduleID.SelectedValue.ToString());
        }


        private void LoadSchedule(string schId)
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM ScheduleMaster WHERE Sch_ID=@Sch_ID ORDER BY Sch_Index", con);

                cmd.Parameters.AddWithValue("@Sch_ID", schId);

                SqlDataReader dr = cmd.ExecuteReader();

                lstSelectedOrders.Items.Clear();

                bool firstRow = true;

                while (dr.Read())
                {
                    if (firstRow)
                    {
                        dpScheduleDate.SelectedDate = Convert.ToDateTime(dr["Sch_Date"]);

                        cmbScheduleType.Text = dr["Sch_Type"].ToString();

                        if (dr["Sch_Start_Time"] != DBNull.Value)
                            txtTime.Text = Convert.ToDateTime(dr["Sch_Start_Time"]).ToString("HH:mm");

                        firstRow = false;
                    }

                    lstSelectedOrders.Items.Add(dr["Order_ID"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Load Available Orders
        private void LoadAvailableOrders()
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT Order_ID FROM OrderMaster ORDER BY Order_ID", con);

                DataTable dt = new();
                da.Fill(dt);

                lstAvailableOrders.ItemsSource = dt.DefaultView;
                lstAvailableOrders.DisplayMemberPath = "Order_ID";
                lstAvailableOrders.SelectedValuePath = "Order_ID";
                lstAvailableOrders.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // > Button
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (lstAvailableOrders.SelectedItem == null)
                return;

            DataRowView row = (DataRowView)lstAvailableOrders.SelectedItem;
            string orderId = row["Order_ID"].ToString();

            // Duplicate Check
            foreach (var item in lstSelectedOrders.Items)
            {
                if (item.ToString() == orderId)
                {
                    MessageBox.Show("Order already selected.");
                    return;
                }
            }

            lstSelectedOrders.Items.Add(orderId);
        }

        // < Button
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lstSelectedOrders.SelectedItem == null)
                return;

            lstSelectedOrders.Items.Remove(lstSelectedOrders.SelectedItem);
        }

        // Exit
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                string schId = cmbScheduleID.Text.Trim();

                SqlCommand check = new SqlCommand(
                    "SELECT Sch_ID FROM ScheduleMaster WHERE Sch_ID=@Sch_ID", con);

                check.Parameters.AddWithValue("@Sch_ID", schId);

                object result = check.ExecuteScalar();

                SqlCommand cmd;

                // UPDATE
                if (result != null)
                {
                    cmd = new SqlCommand(
                    @"UPDATE ScheduleMaster
              SET Sch_Date=@Sch_Date,
                  Order_ID=@Order_ID,
                  Sch_Index=@Sch_Index,
                  Status=@Status,
                  Sch_Type=@Sch_Type,
                  Sch_Start_Time=@Sch_Start_Time
              WHERE Sch_ID=@Sch_ID", con);
                }
                // INSERT
                else
                {
                    cmd = new SqlCommand(
                    @"INSERT INTO ScheduleMaster
            (
                Sch_ID,
                Sch_Date,
                Order_ID,
                Sch_Index,
                Status,
                Sch_Type,
                Sch_Start_Time
            )
            VALUES
            (
                @Sch_ID,
                @Sch_Date,
                @Order_ID,
                @Sch_Index,
                @Status,
                @Sch_Type,
                @Sch_Start_Time
            )", con);
                }

                // Selected Order
                string orderId = "";

                if (lstSelectedOrders.Items.Count > 0)
                    orderId = lstSelectedOrders.Items[0].ToString();

                cmd.Parameters.AddWithValue("@Sch_ID", schId);
                cmd.Parameters.AddWithValue("@Sch_Date", dpScheduleDate.SelectedDate ?? DateTime.Now);
                cmd.Parameters.AddWithValue("@Order_ID", orderId);
                cmd.Parameters.AddWithValue("@Sch_Index", 1);
                cmd.Parameters.AddWithValue("@Status", "N");
                cmd.Parameters.AddWithValue("@Sch_Type", cmbScheduleType.Text);

                // Time Validation
                if (string.IsNullOrWhiteSpace(txtTime.Text))
                {
                    cmd.Parameters.AddWithValue("@Sch_Start_Time", DBNull.Value);
                }
                else
                {
                    if (DateTime.TryParse(txtTime.Text.Trim(), out DateTime startTime))
                    {
                        cmd.Parameters.AddWithValue("@Sch_Start_Time", startTime);
                    }
                    else
                    {
                        MessageBox.Show("Please enter valid time (Example: 08:30 or 08:30 AM)");
                        txtTime.Focus();
                        return;
                    }
                }

                cmd.ExecuteNonQuery();

                MessageBox.Show(result != null
                    ? "Schedule Updated Successfully."
                    : "Schedule Saved Successfully.");

                LoadAvailableOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}