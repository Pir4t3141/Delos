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
        MainWindow mainWindow;
        public string Name;
        private bool Contains = false;

        public WindowAddEditPlaylist(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void TextBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool _contains = false;

            foreach (Playlist playlist in mainWindow.playlists)
            {
                if (playlist.Name == TextBoxName.Text)
                {
                    _contains = true;
                }
            }

            if (_contains)
            {
                Contains = true;
            }
            else
            {
                Contains = false;
            }

            if (!String.IsNullOrEmpty(TextBoxName.Text) && !Contains)
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
