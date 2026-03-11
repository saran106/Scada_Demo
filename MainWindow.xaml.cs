using System.Windows;
using Scada_Demo.ViewModels;
using System.Windows.Media.Animation;

namespace Scada_Demo
{
    public partial class MainWindow : Window
    {
        private PlantViewModel _viewModel;

        public MainWindow()
            {
            InitializeComponent();

            // Set ViewModel as DataContext
            _viewModel = new PlantViewModel();
            DataContext = _viewModel;

            // Start PLC simulation
            _ = _viewModel.StartPlcSimulation();
        }

        private void StartVanAnimation_Click(object sender, RoutedEventArgs e)
        {
            // Method 1: Using Storyboard from Resources
            Storyboard storyboard = this.Resources["VanMoveAnimation"] as Storyboard;
            if (storyboard != null)
            {
                storyboard.Begin();
            }

            // OR Method 2: Direct animation in code (simpler)
            // DoubleAnimation moveLeftAnimation = new DoubleAnimation();
            // moveLeftAnimation.From = 250;
            // moveLeftAnimation.To = 1069;
            // moveLeftAnimation.Duration = TimeSpan.FromSeconds(4);
            // moveLeftAnimation.AutoReverse = true;
            // moveLeftAnimation.RepeatBehavior = RepeatBehavior.Forever;

            // DoubleAnimation moveTopAnimation = new DoubleAnimation();
            // moveTopAnimation.From = 75;
            // moveTopAnimation.To = 472;
            // moveTopAnimation.Duration = TimeSpan.FromSeconds(4);
            // moveTopAnimation.AutoReverse = true;
            // moveTopAnimation.RepeatBehavior = RepeatBehavior.Forever;

            // CargoVanCanvas.BeginAnimation(Canvas.LeftProperty, moveLeftAnimation);
            // CargoVanCanvas.BeginAnimation(Canvas.TopProperty, moveTopAnimation);
        }


    }
}