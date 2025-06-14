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
using Microsoft.Win32;
using Serilog;
using YoutubeDLSharp;
using YoutubeDLSharp.Options;
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.Design.AxImporter;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace MusikProgramm
{
    /// <summary>
    /// Interaction logic for WindowPlaylist.xaml
    /// </summary>
    /// 

    public enum SortTypes
    {
        UNKNOWN = 0,
        RELEASEYEAR = 1,
        ARTIST = 2,
        NAME = 3,
        DEFAULT = 4
    }

    public partial class WindowPlaylist : Window
    {
        string ytDlpPath = "yt-dlp.exe";
        string ffmpegPath = "ffmpeg.exe";
        string pathToytdlpAndffmpeg = "YTDLPSharpRequirements";

        public Playlist playlist { get; private set; } = new Playlist("Empty");
        private Player player = new Player();
        private bool manualIndexChange = false;
        private bool sortedUp = true;
        public SortTypes sortType { get; set; } = SortTypes.DEFAULT;
        public List<Song> songsMetaDataGoingToBeChanged = new List<Song>();
        private YoutubeDL ytdl = new YoutubeDL();
        private string defaultPathToYTDownload = "downloadedSongs";

        public WindowPlaylist(Playlist playlist, Player player)
        {
            InitializeComponent();

            if (!Directory.Exists(pathToytdlpAndffmpeg)) 
            {
                Directory.CreateDirectory(pathToytdlpAndffmpeg);
            }

            if (!Directory.Exists(defaultPathToYTDownload))
            {
                Directory.CreateDirectory(defaultPathToYTDownload);
            }

            ytdl.OutputFolder = defaultPathToYTDownload;

            this.Title = $"{playlist.Name}";

            this.playlist = playlist;
            this.player = player;
            LabelName.Content = playlist.Name;

            UserControlPlayPauseSkip.SetPlayer(this.player);

            foreach (Song song in playlist.SongListSorted)
            {
                ListViewSongs.Items.Add(song);
            }

            if (playlist.SongListSorted.Count > 0)
            {
                manualIndexChange = true;
                ListViewSongs.SelectedIndex = player.currentPlaylist.currentSong;
            }

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
                    UpdateListView();
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
                    manualIndexChange = true;
                    ListViewSongs.SelectedIndex = player.currentPlaylist.currentSong;
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
                AddSong(filePath);
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
                    UpdateListView();
                    songsMetaDataGoingToBeChanged.Add(song);
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
                    $"Delete Song {songToDelete.Name}",
                    "Confirmation Window",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                    );

                if (messageBoxresult == MessageBoxResult.Yes)
                {
                    if (player.currentPlaylist.SongListSorted[player.currentPlaylist.currentSong] == songToDelete)
                    {
                        player.Skip();
                    }
                    playlist.RemoveSong(songToDelete);
                }
            }
            UpdateListView();
        }

        private void ListViewSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewSongs.SelectedIndex >= 0 && ListViewSongs.SelectedIndex < playlist.SongListSorted.Count && !manualIndexChange)
            {
                player.SkipTo(playlist.SongListSorted[ListViewSongs.SelectedIndex]);
            }
            manualIndexChange = false;
        }

        private void ButtonSortDirection_Click(object sender, RoutedEventArgs e)
        {
            if ($"{ButtonSortDirection.Content}" == "⭡")
            {
                sortedUp = false;
                ButtonSortDirection.Content = "⭣";
            }
            else
            {
                sortedUp = true;
                ButtonSortDirection.Content = "⭡";
            }
            Sort();
        }

        private void ComboBoxSortType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (player == null || playlist == null || ListViewSongs == null)
            {
                return;
            }
            Sort();
        }

        private void Sort()
        {
            var selectedItem = (ComboBoxItem)ComboBoxSortType.SelectedItem;
            string selectedItemString = selectedItem.Content.ToString();

            switch (selectedItemString)
            {
                case "Release Year":
                    sortType = SortTypes.RELEASEYEAR;
                    break;
                case "Artist Number 1":
                    sortType = SortTypes.ARTIST;
                    break;
                case "Name":
                    sortType = SortTypes.NAME;
                    break;
                case "Default":
                    sortType = SortTypes.DEFAULT;
                    break;
                default:
                    break;
            }

            playlist.Sort(sortType, sortedUp);
            UpdateListView();

            manualIndexChange = true;
        }

        private void ButtonSaveMetadata_Click(object sender, RoutedEventArgs e)
        {
            // FIX: dkkdk
            try
            {
                //Playlist previousPlaylist = player.currentPlaylist;
                //player.SetPlaylist(new Playlist("empty Playlist"), true);
                player.Stop();

                foreach(Song songNeedsChange in new List<Song>(songsMetaDataGoingToBeChanged))
                {
                    songNeedsChange.EditMetaData();
                    songsMetaDataGoingToBeChanged.Remove(songNeedsChange);
                }

                //player.SetPlaylist(previousPlaylist, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem Saving Metadata");
                Log.Error($"Error saving Metadata: {ex}");
            }
        }

        private async void ButtonAddFromYT_Click(object sender, RoutedEventArgs e)
        {
            string link = null;
            string path = null;
            WindowLink windowLink = new WindowLink();
            if (windowLink.ShowDialog() == true)
            {
                link = windowLink.Link;
            }
            else
            {
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "MP3-Datei (*.mp3)|*.mp3";
            dialog.Title = "Save as MP3";
            dialog.DefaultExt = "mp3";
            dialog.AddExtension = true;
            dialog.DefaultDirectory = defaultPathToYTDownload;

            if (dialog.ShowDialog() == true)
            {
                path = dialog.FileName;

                Log.Debug($"Saved MP3 to {path}");
            }
            else
            {
                return;
            }

            try
            {
                if (!File.Exists(Path.Combine(pathToytdlpAndffmpeg, ytDlpPath)))
                {
                    await Utils.DownloadYtDlp(pathToytdlpAndffmpeg);
                }

                if (!File.Exists(Path.Combine(pathToytdlpAndffmpeg, ffmpegPath)))
                {
                    await Utils.DownloadFFmpeg(pathToytdlpAndffmpeg);
                }

                ytdl.YoutubeDLPath = Path.Combine(pathToytdlpAndffmpeg, ytDlpPath);
                ytdl.FFmpegPath = Path.Combine(pathToytdlpAndffmpeg, ffmpegPath);

                ytdl.OutputFolder = Path.GetDirectoryName(path);

                var res = await ytdl.RunAudioDownload(link, AudioConversionFormat.Mp3);

                if (res.Success && File.Exists(res.Data))
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                    File.Move(res.Data, path);
                    AddSong(path);
                }
                else
                {
                    MessageBox.Show("Error: noFileFound");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error downloading song");
                Log.Error($"Download Error: {ex}");
            }

        }

        public void AddSong(string filePath)
        {
            Song song = new Song(filePath);
            song.LoadFromMetaData();

            WindowSong windowSong = new WindowSong(song);

            if (windowSong.ShowDialog() == true)
            {
                song = windowSong.song;
                playlist.AddSong(song);
                songsMetaDataGoingToBeChanged.Add(song);

                UpdateListView();
            }
        }
    }
}
    