using System.Windows;
using System.Windows.Controls;
using Scada_Demo.MQTT_Model;
using Scada_Demo.Services;

namespace Scada_Demo.Batch_Settings
{
    public partial class Gate_Sequence : Window
    {
        public Gate_Sequence()
        {
            InitializeComponent();
        }

        private void MoveToPreferred_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentList.SelectedItem is not ListBoxItem selectedItem)
            {
                MessageBox.Show("Select an item");
                return;
            }

            string value = selectedItem.Content.ToString();

            if (value.StartsWith("Agg2"))
            {
                MessageBox.Show("Agg2 is fixed");
                return;
            }

            bool exists = PreferredList.Items
                .Cast<ListBoxItem>()
                .Any(x => x.Content.ToString().StartsWith(
                    value.Split(':')[0]));

            if (exists)
                return;

            // First add selected item
            PreferredList.Items.Add(new ListBoxItem
            {
                Content = value
            });

            // Agg2 fixed at 2nd position
            bool agg2Exists = PreferredList.Items
                .Cast<ListBoxItem>()
                .Any(x => x.Content.ToString().StartsWith("Agg2"));

            if (!agg2Exists)
            {
                PreferredList.Items.Insert(1, new ListBoxItem
                {
                    Content = "Agg2 : 2"
                });
            }
        }

        private async void ReadFromPLC_Click(object sender, RoutedEventArgs e)
        {
            MaterialinAir batchSettings = new MaterialinAir();

            BatchSettingsModel data = await batchSettings.ReadValues();

            PreferredList.Items.Clear();

            string[] positions = new string[4];

            // Agg2 is always sequence 2
            positions[1] = "Agg2 : 2";

            if (data.batchSettings_GateSequence.Agg1 >= 1 && data.batchSettings_GateSequence.Agg1 <= 4)
                positions[data.batchSettings_GateSequence.Agg1 - 1] = $"Agg1 : {data.batchSettings_GateSequence.Agg1}";

            if (data.batchSettings_GateSequence.Agg3 >= 1 && data.batchSettings_GateSequence.Agg3 <= 4)
                positions[data.batchSettings_GateSequence.Agg3 - 1] = $"Agg3 : {data.batchSettings_GateSequence.Agg3}";

            if (data.batchSettings_GateSequence.Agg4 >= 1 && data.batchSettings_GateSequence.Agg4 <= 4)
                positions[data.batchSettings_GateSequence.Agg4 - 1] = $"Agg4 : {data.batchSettings_GateSequence.Agg4}";

            foreach (string item in positions)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    PreferredList.Items.Add(new ListBoxItem
                    {
                        Content = item
                    });
                }
            }
        }

        private async void WriteToPLC_Click(object sender, RoutedEventArgs e)
        {
            // Disable button while writing (optional)
         

            try
            {
                // Validate
                if (PreferredList.Items.Count != 4)
                {
                    MessageBox.Show(
                        "Sequence must contain Agg1, Agg2, Agg3 and Agg4.",
                        "Validation",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                short agg1Pos = 0;
                short agg3Pos = 0;
                short agg4Pos = 0;

                // Find positions
                for (int i = 0; i < PreferredList.Items.Count; i++)
                {
                    if (PreferredList.Items[i] is ListBoxItem item)
                    {
                        string text = item.Content?.ToString() ?? "";
                        short position = (short)(i + 1);

                        if (text.StartsWith("Agg1"))
                            agg1Pos = position;
                        else if (text.StartsWith("Agg3"))
                            agg3Pos = position;
                        else if (text.StartsWith("Agg4"))
                            agg4Pos = position;
                    }
                }

                // Extra validation
                if (agg1Pos == 0 || agg3Pos == 0 || agg4Pos == 0)
                {
                    MessageBox.Show(
                        "Unable to determine Aggregate positions.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }

                // MQTT Service
                Gate_Seq service = new Gate_Seq();

                // Publish one by one
                await service.WriteGATESEQ_210(agg1Pos.ToString());
                //await Task.Delay(1000);

                await service.WriteGATESEQ_212(agg3Pos.ToString());
                //await Task.Delay(1000);

                await service.WriteGATESEQ_214(agg4Pos.ToString());

                MessageBox.Show(
                    "Gate Sequence Sent Successfully.",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            
        }
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            PreferredList.Items.Clear();
        }
    }
}