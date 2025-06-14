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

namespace MusikProgramm
{
    /// <summary>
    /// Interaction logic for WindowLink.xaml
    /// </summary>
    public partial class WindowLink : Window
    {
        public string Link { get; set; }
        public WindowLink()
        {
            InitializeComponent();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TextBoxLink.Text))
            {
                Link = TextBoxLink.Text;
                this.DialogResult = true;
            }
        }
    }
}
