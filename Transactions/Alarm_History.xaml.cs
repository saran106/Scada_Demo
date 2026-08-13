using System;
using System.Windows;

using Scada_Demo.ViewModels.Transactions;

namespace Scada_Demo.Transactions
{
    /// <summary>
    /// Interaction logic for Alarm_History.xaml
    /// </summary>
    public partial class Alarm_History : Window
    {
        private AlarmHistoryViewModel vm;

        public Alarm_History()
        {
            InitializeComponent();

            // New ViewModel every time this window opens
            vm = new AlarmHistoryViewModel();

            DataContext = vm;
        }
    }
}