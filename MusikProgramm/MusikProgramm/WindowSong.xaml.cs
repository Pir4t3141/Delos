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
    /// Interaction logic for WindowSong.xaml
    /// </summary>
    public partial class WindowSong : Window
    {
        public Song song;

        private bool nameOK = false;
        private bool artistsOK = false;
        private bool albumOK = false;
        private bool releaseYearOK = false;

        public WindowSong(Song song)
        {
            InitializeComponent();
            this.song = song;
            EditTextBoxesData();
        }

        public WindowSong(Song song, bool editNotAdd) : this(song)
        {
            ButtonAdd.Content = "Edit";
            this.Title = $"Edit {song.Name}";
        }

        private void TextBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxName.Text.Contains("]") || TextBoxName.Text.Contains("["))
            {
                TextBoxName.Background = Brushes.LightCoral;
                nameOK = false;
            }
            else
            {
                TextBoxName.Background = null;
                nameOK = true;
            }
        }

        private void TextBoxArtist_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxArtist.Text.Contains("]") || TextBoxArtist.Text.Contains("[")) // can't use the symbol which is used for sepperating the values when serialized
            {
                TextBoxArtist.Background = Brushes.LightCoral;
                artistsOK = false;
            }
            else
            {
                TextBoxArtist.Background = null;
                artistsOK = true;
            }
        }

        private void TextBoxAlbum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxAlbum.Text.Contains("]") || TextBoxAlbum.Text.Contains("[")) // can't use the symbol which is used for sepperating the values when serialized
            {
                TextBoxAlbum.Background = Brushes.LightCoral;
                albumOK = false;
            }
            else
            {
                TextBoxAlbum.Background = null;
                albumOK = true;
            }
        }

        private void TextBoxReleaseYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                uint releaseYear = Convert.ToUInt32(TextBoxReleaseYear.Text);
                TextBoxReleaseYear.Background = null;
                releaseYearOK = true;
            }
            catch
            {
                TextBoxReleaseYear.Background = Brushes.LightCoral;
                releaseYearOK = false;
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!nameOK || !artistsOK || !albumOK || !releaseYearOK)
            {
                return;
            }
            song.Name = TextBoxName.Text;
            song.Artists = TextBoxArtist.Text.Split(',');
            song.Album = TextBoxAlbum.Text;
            song.ReleaseYear = Convert.ToUInt32(TextBoxReleaseYear.Text);

            this.DialogResult = true;
        }

        private void EditTextBoxesData()
        {
            TextBoxName.Text = song.Name;
            TextBoxArtist.Text = String.Join(',', song.Artists);
            TextBoxAlbum.Text = song.Album;
            TextBoxReleaseYear.Text = $"{song.ReleaseYear}";
        }
    }
}
