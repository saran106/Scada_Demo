using System.Windows;
using Scada_Demo.ViewModels.User;

namespace Scada_Demo.User
{
    public partial class Modify_user : Window
    {
        public Modify_user()
        {
            InitializeComponent();

            DataContext = new ModifyuserViewModel();
        }
    }
}