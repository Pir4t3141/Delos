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
using System.Windows.Shapes;
using Microsoft.Win32;

namespace MusikProgramm
{
    /// <summary>
    /// Interaction logic for WindowPlaylist.xaml
    /// </summary>
    public partial class WindowPlaylist : Window
    {
        public Playlist playlist { get; private set; } = new Playlist("Empty");
        private Player player = new Player();
        private bool manualIndexChange = false;

        public WindowPlaylist(Playlist playlist, Player player)
        {
            InitializeComponent();

            this.playlist = playlist;
            this.player = player;
            LabelName.Content = playlist.Name;

            foreach (Song song in playlist.SongListSorted)
            {
                ListViewSongs.Items.Add(song);
            }

            UserControlPlayPauseSkip.SetPlayer(player);

            player.PlayerStatusChanged += Player_PlayerStatusChanged;

            player.PlayerPlaylistStatusChanged += Player_PlayerPlaylistStatusChanged;
        }

        private void Player_PlayerPlaylistStatusChanged(object? sender, PlayerPlaylistStatus e)
        {
            // When shuffeling refresh order of song...
            switch (e)
            {
                case PlayerPlaylistStatus.UNKNOWN:
                    break;
                case PlayerPlaylistStatus.SHUFFLE:
                    break;
                case PlayerPlaylistStatus.REPEAT:
                    break;
                case PlayerPlaylistStatus.SHUFFLEREPEAT:
                    break;
                case PlayerPlaylistStatus.NONE:
                    break;
                default:
                    break;
            }
        }

        private void Player_PlayerStatusChanged(object? sender, PlayerStatus e)
        {
            // When stopped (next song change selected item on list view), needs own variable if change was made by code or wpf click (or else is playing double)
            switch (e)
            {
                case PlayerStatus.UNKNOWN:
                    break;
                case PlayerStatus.STOPPED:
                    break;
                case PlayerStatus.PLAYING:
                    break;
                case PlayerStatus.PAUSED:
                    break;
                default:
                    break;
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            string ?filePath = null;
            Song song;
            // get path
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "mp3 files (*.mp3)|*.mp3";
            openFileDialog.Title = "Select mp3 file";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
            }

            // make song and edit metadata

            if (filePath != null)
            {
                song = new Song(filePath);

                WindowSong windowSong = new WindowSong(song);

                if (windowSong.ShowDialog() == true)
                {
                    song = windowSong.song;
                    song.EditMetaData();
                    playlist.AddSong(song);
                    UpdateListView();
                }
            }
        }
        private void UpdateListView()
        {
            ListViewSongs.Items.Clear();
            foreach (Song song in playlist.SongListSorted)
            {
                ListViewSongs.Items.Add(song);
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewSongs.SelectedIndex >= 0 && ListViewSongs.SelectedIndex < playlist.SongListSorted.Count)
            {
                Song song = playlist.SongListSorted[ListViewSongs.SelectedIndex];

                WindowSong windowSong = new WindowSong(song, true);

                if (windowSong.ShowDialog() == true)
                {
                    song = windowSong.song;
                    song.EditMetaData();
                    UpdateListView();
                }
            }
            else
            {
                MessageBox.Show("No Song Selected");
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Song songToDelete;
            if (ListViewSongs.SelectedIndex >= 0 && ListViewSongs.SelectedIndex < playlist.SongListSorted.Count)
            {
                songToDelete = playlist.SongListSorted[ListViewSongs.SelectedIndex];
                MessageBoxResult messageBoxresult = MessageBox.Show(
                    $"Delete Song {playlist.SongListSorted[ListViewSongs.SelectedIndex].Name}",
                    "Confirmation Window",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                    );

                if (messageBoxresult == MessageBoxResult.Yes)
                {
                    playlist.RemoveSong(songToDelete);
                }
            }
            UpdateListView();
        }

        private void ListViewSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
