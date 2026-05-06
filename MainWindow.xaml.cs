using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Input;
using Scada_Demo.ViewModels;
using System.Windows.Controls;
using System.Windows.Media;
using Scada_Demo.Views;
using Scada_Demo.Calibration;
using Scada_Demo.Batch_Settings;
using Scada_Demo.Set_Parameters;
using Scada_Demo.Other_Settings;
using Scada_Demo.User;
using Scada_Demo.Transactions;

namespace Scada_Demo
{
    public partial class MainWindow : Window
    {
        private PlantViewModel _viewModel;
        private bool isMenuOpen = false;

        public MainWindow()
        {
            InitializeComponent();

            // Set ViewModel as DataContext
            _viewModel = new PlantViewModel();
            DataContext = _viewModel;
            MasterSubMenu.ItemsSource = MasterMenuItems;
            CalibrationSubMenu.ItemsSource = CalibrationMenuItems;
            BatchSubMenu.ItemsSource = BatchMenuItems;
            ParamSubMenu.ItemsSource = ParameterMenuItems;
            OtherSubMenu.ItemsSource = OtherMenuItems;
            UserSubMenu.ItemsSource = UserMenuItems;

            MasterPopup.DataContext = this;
            CalibrationPopup.DataContext = this;
            BatchPopup.DataContext = this;
            ParamPopup.DataContext = this;
            OtherPopup.DataContext = this;
            UserPopup.DataContext = this;

            // Start PLC simulation
            _ = _viewModel.StartPlcSimulation();
        }

        bool isOpen = false;



        private void Dashboard_Click(object sender, MouseButtonEventArgs e)
        {
            Alarm_History obj = new Alarm_History();
            obj.Show();   // open window
        }

        private void Dashboard_Click1(object sender, MouseButtonEventArgs e)
        {
            Delete_User obj = new Delete_User();
            //Conveyor_Settings obj = new Conveyor_Settings();
            obj.Show();   // open window
        }



        public class SubMenuItem
        {
            public string Name { get; set; }
            public string ViewKey { get; set; }
        }

        //        public List<SubMenuItem> MasterMenuItemsd = new List<SubMenuItem>()
        //{
        //    new SubMenuItem { Name = "Customer",NavigateAction = () => new Customer_Master().Show() },
        //    new SubMenuItem { Name = "Site" , NavigateAction = () => new Site_Details().Show()},
        //    new SubMenuItem { Name = "Recipe",NavigateAction = () => new Configuration_Order().Show() },
        //    new SubMenuItem { Name = "Truck" },

        //    new SubMenuItem { Name = "Order" },
        //    new SubMenuItem { Name = "Schedule" },
        //    new SubMenuItem { Name = "Inward" },
        //    new SubMenuItem { Name = "Maintenance" },
        //};

        private void OpenWindowOnce<T>() where T : Window, new()
        {
            foreach (Window win in Application.Current.Windows)
            {
                if (win is T)
                {
                    win.Activate();   // already open → bring front
                    return;
                }
            }

            // not open → create new
            new T().Show();
        }

        public List<SubMenuItem> MasterMenuItems = new List<SubMenuItem>()
{
    new SubMenuItem { Name = "Customer", ViewKey = "Customer" },
    new SubMenuItem { Name = "Site", ViewKey = "Site" },
    new SubMenuItem { Name = "Order", ViewKey = "Order" },
    new SubMenuItem { Name = "Schedule", ViewKey = "Schedule" }
};

        public List<SubMenuItem> CalibrationMenuItems = new List<SubMenuItem>()
{
    new SubMenuItem { Name = "Aggregate", ViewKey = "Aggregate" },
    new SubMenuItem { Name = "Cement", ViewKey = "Cement" },
    new SubMenuItem { Name = "Water", ViewKey = "Water" },
    new SubMenuItem { Name = "Admixture 1", ViewKey = "Admixture1" },
    new SubMenuItem { Name = "Admixture 2", ViewKey = "Admixture2" },
    new SubMenuItem { Name = "Ice", ViewKey = "Ice" },
    new SubMenuItem { Name = "Mixer", ViewKey = "Mixer" },
    new SubMenuItem { Name = "Moisture First", ViewKey = "MoistureFirst" },
    new SubMenuItem { Name = "Moisture Second", ViewKey = "MoistureSecond" },
    new SubMenuItem { Name = "Silica", ViewKey = "Silica" }
};

        public List<SubMenuItem> BatchMenuItems = new List<SubMenuItem>()
{
    new SubMenuItem { Name = "Batch Output Mode", ViewKey = "Batch_Output_Mode" },

    new SubMenuItem { Name = "Gate Sequence", ViewKey = "Gate_Sequence" },

    new SubMenuItem { Name = "Jog Time", ViewKey = "Jog_Time" },

    new SubMenuItem { Name = "Empty Value", ViewKey = "Empty_Value" },

    new SubMenuItem { Name = "Tolerance", ViewKey = "Tolerance" },

    new SubMenuItem { Name = "Discharge Sequence", ViewKey = "Discharge_Sequence" },

    new SubMenuItem { Name = "Step Time", ViewKey = "Step_Time" },

    new SubMenuItem { Name = "Material in Air", ViewKey = "Material_Air" },

    new SubMenuItem { Name = "Coarse to Fine", ViewKey = "Coarse_to_Fine" }
};

        public List<SubMenuItem> ParameterMenuItems = new List<SubMenuItem>()
{
    new SubMenuItem
    {
        Name = "Mixer Grease Parameter",
        ViewKey = "Mixer_Grease_Parameter"
    },

    new SubMenuItem
    {
        Name = "Mixer Parameter",
        ViewKey = "Mixer_Parameter"
    },

    new SubMenuItem
    {
        Name = "Skip Parameter",
        ViewKey = "Skip_Parameter"
    },

    new SubMenuItem
    {
        Name = "Conveyor Parameter",
        ViewKey = "Conveyor_Parameter"
    },

    new SubMenuItem
    {
        Name = "Moisture Parameter",
        ViewKey = "Moisture_Parameter"
    },

    new SubMenuItem
    {
        Name = "Vibrator Parameter",
        ViewKey = "Vibrator_Parameter"
    }
};

        public List<SubMenuItem> OtherMenuItems = new List<SubMenuItem>()
{
    new SubMenuItem
    {
        Name = "Bin Setup",
        ViewKey = "Bin_Setup"
    },

    new SubMenuItem
    {
        Name = "Capacity",
        ViewKey = "Capacity"
    },

    new SubMenuItem
    {
        Name = "Enable/Disable Other Modes",
        ViewKey = "Enable_Disable_OtherModes"
    },

    new SubMenuItem
    {
        Name = "Inair/Inflow Mode",
        ViewKey = "In_Air_Inflow_Mode"
    },

    new SubMenuItem
    {
        Name = "Plant Profile Settings",
        ViewKey = "Plant_Profile"
    },

    new SubMenuItem
    {
        Name = "Auto Backup Settings",
        ViewKey = "Auto_Backup_Settings"
    }
};

        public List<SubMenuItem> UserMenuItems = new List<SubMenuItem>()
{
    new SubMenuItem
    {
        Name = "Register New User",
        ViewKey = "Register_New_User"
    },

    new SubMenuItem
    {
        Name = "Delete User",
        ViewKey = "Delete_User"
    }
};
        private void SubMenu_Click(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            SubMenuItem item = element.DataContext as SubMenuItem;
            if (item == null) return;

            switch (item.ViewKey)
            {
                case "Customer":
                    OpenWindowOnce<Customer_Master>();
                    break;

                case "Site":
                    OpenWindowOnce<Site_Details>();
                    break;

                case "Order":
                    OpenWindowOnce<Configuration_Order>();
                    break;

                case "Schedule":
                    OpenWindowOnce<Schedule>();
                    break;
            }

            MasterPopup.IsOpen = false;
        }

        private void CalibrationSubMenu_Click(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            SubMenuItem item = element.DataContext as SubMenuItem;
            if (item == null) return;

            switch (item.ViewKey)
            {
                case "Aggregate":
                    OpenWindowOnce<Aggregate_Calib>();
                    break;

                case "Cement":
                    OpenWindowOnce<Cement_Calib>();
                    break;

                case "Water":
                    OpenWindowOnce<Water_Calib>();
                    break;

                case "Admixture1":
                    OpenWindowOnce<Admixture1>();
                    break;

                case "Admixture2":
                    OpenWindowOnce<Admixture2>();
                    break;

                case "Ice":
                    OpenWindowOnce<Ice>();
                    break;

                case "Mixer":
                    OpenWindowOnce<Mixer>();
                    break;

                case "MoistureFirst":
                    OpenWindowOnce<Moisture_First>();
                    break;

                case "MoistureSecond":
                    OpenWindowOnce<Moisture_Second>();
                    break;

                case "Silica":
                    OpenWindowOnce<Silica>();
                    break;
            }

            CalibrationPopup.IsOpen = false;
        }

        private void BatchSubMenu_Click(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            SubMenuItem item = element.DataContext as SubMenuItem;
            if (item == null) return;

            switch (item.ViewKey)
            {
                case "Batch_Output_Mode":
                    OpenWindowOnce<Batch_OutputMode>();
                    break;

                case "Gate_Sequence":
                    OpenWindowOnce<Gate_Sequence>();
                    break;

                case "Jog_Time":
                    OpenWindowOnce<Jog_Time>();
                    break;

                case "Empty_Value":
                    OpenWindowOnce<Empty_Value>();
                    break;

                case "Tolerance":
                    OpenWindowOnce<Tolerance>();
                    break;

                case "Discharge_Sequence":
                    OpenWindowOnce<Discharge_Sequence>();
                    break;

                case "Step_Time":
                    OpenWindowOnce<Step_Time>();
                    break;

                case "Material_Air":
                    OpenWindowOnce<Material_Air>();
                    break;

                case "Coarse_to_Fine":
                    OpenWindowOnce<Coarse_to_Fine>();
                    break;
            }

            BatchPopup.IsOpen = false;
        }

        private void ParamSubMenu_Click(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            SubMenuItem item = element.DataContext as SubMenuItem;
            if (item == null) return;

            switch (item.ViewKey)
            {
                case "Mixer_Grease_Parameter":
                    OpenWindowOnce<Mixer_Grease_Paramter>();
                    break;

                case "Mixer_Parameter":
                    OpenWindowOnce<Mixer_Paramter>();
                    break;

                case "Skip_Parameter":
                    OpenWindowOnce<Skip_Paramter_Settings>();
                    break;

                case "Conveyor_Parameter":
                    OpenWindowOnce<Conveyor_Settings>();
                    break;

                case "Moisture_Parameter":
                    OpenWindowOnce<Moisture_Settings>();
                    break;

                case "Vibrator_Parameter":
                    OpenWindowOnce<Vibrator_Settings>();
                    break;
            }

            ParamPopup.IsOpen = false;
        }

        private void OtherSubMenu_Click(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            SubMenuItem item = element.DataContext as SubMenuItem;
            if (item == null) return;

            switch (item.ViewKey)
            {
                case "Bin_Setup":
                    OpenWindowOnce<Bin_Setup>();
                    break;

                case "Capacity":
                    OpenWindowOnce<Capacity>();
                    break;

                case "Enable_Disable_OtherModes":
                    OpenWindowOnce<Enable_Disable_OtherModes>();
                    break;

                case "In_Air_Inflow_Mode":
                    OpenWindowOnce<In_Air_Inflow_Mode>();
                    break;

                case "Plant_Profile":
                    OpenWindowOnce<Plant_Profile>();
                    break;

                case "Auto_Backup_Settings":
                    OpenWindowOnce<Auto_BackupSettings>();
                    break;
            }

            OtherPopup.IsOpen = false;
        }

        private void UserSubMenu_Click(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            SubMenuItem item = element.DataContext as SubMenuItem;
            if (item == null) return;

            switch (item.ViewKey)
            {
                case "Register_New_User":
                    OpenWindowOnce<Register_New_User>();
                    break;

                case "Delete_User":
                    OpenWindowOnce<Delete_User>();
                    break;
            }

            UserPopup.IsOpen = false;
        }
        private void StartVanAnimation_Click(object sender, RoutedEventArgs e)
        {
            // Method 1: Using Storyboard from Resources
            Storyboard storyboard = this.Resources["VanMoveAnimation"] as Storyboard;
            if (storyboard != null)
            {
                storyboard.Begin();
            }
        }


        private void Master_Click(object sender, MouseButtonEventArgs e)
        {
            if (isOpen)
            {
                MasterSubMenu.Visibility =
                    MasterSubMenu.Visibility == Visibility.Visible
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }
            else
            {
                // toggle popup
                MasterPopup.IsOpen = !MasterPopup.IsOpen;
            }
        }
        private void Calibration_Click(object sender, MouseButtonEventArgs e)
        {
            if (isOpen)
            {
                CalibrationSubMenu.Visibility =
                    CalibrationSubMenu.Visibility == Visibility.Visible
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }
            else
            {
                CalibrationPopup.IsOpen = !CalibrationPopup.IsOpen;
            }
        }
        private void Batch_Click(object sender, MouseButtonEventArgs e)
        {
            if (isOpen)
            {
                BatchSubMenu.Visibility =
                    BatchSubMenu.Visibility == Visibility.Visible
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }
            else
            {
                BatchPopup.IsOpen = !BatchPopup.IsOpen;
            }
        }
        private void Param_Click(object sender, MouseButtonEventArgs e)
        {
            if (isOpen)
            {
                ParamSubMenu.Visibility =
                    ParamSubMenu.Visibility == Visibility.Visible
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }
            else
            {
                ParamPopup.IsOpen = !ParamPopup.IsOpen;
            }
        }
        private void Other_Click(object sender, MouseButtonEventArgs e)
        {
            if (isOpen)
            {
                OtherSubMenu.Visibility =
                    OtherSubMenu.Visibility == Visibility.Visible
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }
            else
            {
                OtherPopup.IsOpen = !OtherPopup.IsOpen;
            }
        }
        private void User_Click(object sender, MouseButtonEventArgs e)
        {
            if (isOpen)
            {
                UserSubMenu.Visibility =
                    UserSubMenu.Visibility == Visibility.Visible
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }
            else
            {
                UserPopup.IsOpen = !UserPopup.IsOpen;
            }
        }

        private void Trans_Click(object sender, MouseButtonEventArgs e) { }
        private void Options_Click(object sender, MouseButtonEventArgs e) { }
        private void DB_Click(object sender, MouseButtonEventArgs e) { }
        private void Reports_Click(object sender, MouseButtonEventArgs e) { }




        private void MenuToggle_Click(object sender, RoutedEventArgs e)
        {
            if (!isOpen)
            {
                isOpen = true;

                // Sidebar expand
                SideMenuColumn.Width = new GridLength(220);

                //   TxtDashboard.Visibility = Visibility.Visible;
                //TxtUser.Visibility = Visibility.Visible;
                TxtMaster.Visibility = Visibility.Visible;
                TxtCalibration.Visibility = Visibility.Visible;
                TxtBatch.Visibility = Visibility.Visible;
                TxtParam.Visibility = Visibility.Visible;
                TxtOther.Visibility = Visibility.Visible;
                TxtUser.Visibility = Visibility.Visible;
                TxtTrans.Visibility = Visibility.Visible;
                TxtOptions.Visibility = Visibility.Visible;
                TxtDB.Visibility = Visibility.Visible;
                TxtReports.Visibility = Visibility.Visible;

                // Enable scroll
                MainScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                MenuIcon.Text = "✕";
                MenuToggle.HorizontalAlignment = HorizontalAlignment.Right;

                // AnimateMenu(220); // expand
            }
            else
            {

                if (isOpen)
                {
                    MasterSubMenu.Visibility = Visibility.Collapsed;
                    CalibrationSubMenu.Visibility = Visibility.Collapsed;
                    BatchSubMenu.Visibility = Visibility.Collapsed;
                }

                isOpen = false;
                // Sidebar collapse
                SideMenuColumn.Width = new GridLength(60);

                //TxtDashboard.Visibility = Visibility.Collapsed;
                // TxtUser.Visibility = Visibility.Collapsed;
                TxtMaster.Visibility = Visibility.Collapsed;
                TxtCalibration.Visibility = Visibility.Collapsed;
                TxtBatch.Visibility = Visibility.Collapsed;
                TxtParam.Visibility = Visibility.Collapsed;
                TxtOther.Visibility = Visibility.Collapsed;
                TxtUser.Visibility = Visibility.Collapsed;
                TxtTrans.Visibility = Visibility.Collapsed;
                TxtOptions.Visibility = Visibility.Collapsed;
                TxtDB.Visibility = Visibility.Collapsed;
                TxtReports.Visibility = Visibility.Collapsed;

                // Disable scroll
                MainScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                MenuIcon.Text = "☰";
                MenuToggle.HorizontalAlignment = HorizontalAlignment.Left;

                //  AnimateMenu(60); // collapse
            }


        }
    }
}