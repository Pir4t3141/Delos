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
    /// Interaction logic for WindowAddEditPlaylist.xaml
    /// </summary>
    public partial class WindowAddEditPlaylist : Window
    {
        public string Name;
        public WindowAddEditPlaylist()
        {
            InitializeComponent();
        }

        private void TextBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TextBoxName.Text))
            {
                TextBoxName.Background = null;
            }
            else
            {
                TextBoxName.Background = Brushes.LightCoral;
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxName.Text))
            {
                Name = TextBoxName.Text;
                this.DialogResult = true;
            }
        }
    }
}
