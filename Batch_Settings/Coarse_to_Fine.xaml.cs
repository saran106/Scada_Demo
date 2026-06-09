using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Scada_Demo.Services;

namespace Scada_Demo.Batch_Settings
{
    /// <summary>
    /// Interaction logic for Coarse_to_Fine.xaml
    /// </summary>
    public partial class Coarse_to_Fine : Window
    {
        public Coarse_to_Fine()
        {
            InitializeComponent();
        }

        private async void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Coarse_to_FIne material = new Coarse_to_FIne();
                


                    var data = await material.ReadValues_CF();

                Agg1.Text = data.Agg1;
                Agg2.Text = data.Agg2;
                Agg3.Text = data.Agg3;
                Agg4.Text = data.Agg4;
                Agg5.Text = data.Agg5;
                    Wtr1.Text = data.Wtr1;

               
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void BtnWrite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Coarse_to_FIne material = new Coarse_to_FIne();

                await material.WriteCF_538(Agg1.Text);
                await material.WriteCF_540(Agg2.Text);
                await material.WriteCF_542(Agg3.Text);
                await material.WriteCF_544(Agg4.Text);
                await material.WriteCF_546(Agg5.Text);
                await material.WriteCF_606(Wtr1.Text);

                MessageBox.Show("Values Written Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
