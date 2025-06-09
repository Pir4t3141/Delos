using System;
using System.Collections.Generic;
using System.IO;
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

namespace MusikProgramm
{
    /// <summary>
    /// Interaction logic for WindowAddEditPlaylist.xaml
    /// </summary>
    public partial class WindowAddEditPlaylist : Window
    {
        List <Playlist> playlists;
        public string Name;
        private bool Contains = false;

        public WindowAddEditPlaylist(List<Playlist> playlists)
        {
            InitializeComponent();
            this.playlists = playlists;
        }

        public WindowAddEditPlaylist(List<Playlist> playlists, bool editNotAdd) : this(playlists)
        {
            ButtonAdd.Content = "Edit";
        }

        private void TextBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool alreadyExists = false;
            bool invalidCharacterFound = false;

            foreach (Playlist playlist in playlists)
            {
                if (playlist.Name == TextBoxName.Text)
                {
                    alreadyExists = true;
                }
            }
            string text = TextBoxName.Text;

            foreach (char invalidCharacter in Path.GetInvalidFileNameChars())
            {
                if (text.Contains(invalidCharacter))
                {
                    invalidCharacterFound = true;
                }
            }

            if (!String.IsNullOrEmpty(text) && !alreadyExists && !text.Contains("]") && !text.Contains("[") && !invalidCharacterFound)
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
