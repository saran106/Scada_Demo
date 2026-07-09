using System.Windows;
using Microsoft.Data.SqlClient;

using Scada_Demo.Database;

namespace Scada_Demo.Login
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();

                con.Open();

                string query = @"
            SELECT COUNT(*)
            FROM U_Mas
            WHERE UserName = @UserName
              AND dbo.ufn_Decryption(PasswordHash) = @Password
              AND IsActive = 1";

                using SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@UserName", txtUser.Text.Trim());
                cmd.Parameters.AddWithValue("@Password", txtPassword.Password.Trim());

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count > 0)
                {
                    MainWindow main = new MainWindow();
                    main.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid Username or Password",
                                    "Login",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);

                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}