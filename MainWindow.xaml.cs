using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Input;
using Scada_Demo.ViewModels;
using System.Windows.Controls;
using System.Windows.Media;

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

            // Start PLC simulation
            _ = _viewModel.StartPlcSimulation();
        }

        bool isOpen = false;
        private void MenuToggle_Clicks(object sender, RoutedEventArgs e)
        {
            if (!isOpen)
            {
                isOpen = true;

                // Sidebar expand
                SideMenuColumn.Width = new GridLength(220);

                TxtDashboard.Visibility = Visibility.Visible;
                TxtUser.Visibility = Visibility.Visible;

                // ✅ ENABLE scroll ONLY NOW
                MainScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
            else
            {
                isOpen = false;

                // Sidebar collapse
                SideMenuColumn.Width = new GridLength(60);

                TxtDashboard.Visibility = Visibility.Collapsed;
                TxtUser.Visibility = Visibility.Collapsed;

                // ✅ DISABLE scroll again
                MainScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            }
        }

        private void MenuToggle_Click(object sender, RoutedEventArgs e)
        {
            if (!isOpen)
            {
                isOpen = true;

                // Sidebar expand
                SideMenuColumn.Width = new GridLength(220);

                TxtDashboard.Visibility = Visibility.Visible;
                TxtUser.Visibility = Visibility.Visible;

                // Enable scroll
                MainScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
            else
            {
                isOpen = false;

                // Sidebar collapse
                SideMenuColumn.Width = new GridLength(60);

                TxtDashboard.Visibility = Visibility.Collapsed;
                TxtUser.Visibility = Visibility.Collapsed;

                // Disable scroll
                MainScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            }

            
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
    }
}