using System.Windows;
using System.Windows.Controls;
using Scada_Demo.ViewModels.Login;

namespace Scada_Demo.Login
{
    public partial class Login : Window
    {
        private LoginViewModel vm;

        public Login()
        {
            InitializeComponent();

            vm = App.LoginVM;
            DataContext = vm;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            vm.Password = ((PasswordBox)sender).Password;
        }
    }
}