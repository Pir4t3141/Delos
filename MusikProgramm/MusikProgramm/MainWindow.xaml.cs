using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using NAudio;
using NAudio.Wave;
using Serilog;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MusikProgramm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Zum musik abspielen: Naudio (WASAPI)

        /* musik abspielen: 
        string path = "C:\\Users\\tobias\\.... .mp3";
        AudioFileReader audiofile = new AudioFileReader(path);
        WasapiOut outputDevice = new WasapiOut();
        outputDevice.Init(audiofile);
        outputDevice.Play();*/

        public List<Playlist> playlists { get; private set; } = new List<Playlist>();
        public Playlist? currentPlaylist;
        public bool shuffle;
        private WindowPlaylist ?playlistWindow;
        private Playlist lastEditedPlaylist;

        private Player player = new Player();

        public MainWindow()
        {
            InitializeComponent();

            player.PlayerStatusChanged += Player_PlayerStatusChanged;

            //Serilog setup
            Log.Logger = new LoggerConfiguration().
                MinimumLevel.Debug().
                WriteTo.File("musikprogramm.log", rollingInterval: RollingInterval.Month).
                CreateLogger();

            Log.Debug("Started MainWindow");

            // Loads in all Playlists
            if (Directory.Exists("Playlists") && Directory.GetFiles("Playlists") != null)
            {
                string[] files = Directory.GetFiles("Playlists");
                foreach (string file in files)
                {
                    try
                    {
                        string[] fileSeperated = file.Split('.');
                        Array.Reverse(fileSeperated);
                        if (fileSeperated[0] == "delos") // only looks for playlists //TODO: Change .txt to playlist format
                        {
                            Playlist playlist = Playlist.Import(file);
                            playlists.Add(playlist);
                            ListViewPlaylists.Items.Add(playlist);
                        }
                        else
                        {
                            Log.Debug("File not correct format");
                        }
                    }
                    catch
                    {
                        Log.Error("Error loading Playlist");
                    }
                }
            }
            else
            {
                Log.Debug("No Playlists exist");
            }

            if (!Directory.Exists("Songs"))
            {
                Directory.CreateDirectory("Songs");
            }

            // See if there is a favourite song playlist
            bool favoritesFound = false;

            foreach (Playlist playlistInList in playlists)
            {
                if (playlistInList.Name == "Favorite Songs")
                {
                    favoritesFound = true;
                }
            }
            
            if (favoritesFound == false)
            {
                Playlist favoriteSongs = new Playlist("Favorite Songs");
                favoriteSongs.Save();
                playlists.Add(favoriteSongs);
            }

            UpdateListView();

            UserControlPlayPauseSkip.SetPlayer(player);

            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            player.SaveProgress();

            if (playlistWindow != null)
            {
                playlistWindow.playlist.Save();
                playlistWindow.Close();
            }
        }

        private void Player_PlayerStatusChanged(object? sender, PlayerStatus e)
        {
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

        private void ListViewPlaylists_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // view playlist

            //currentPlaylist = playlists[ListViewPlaylists.SelectedIndex];

            if (currentPlaylist != null && playlistWindow == null)
            {
                lastEditedPlaylist = playlists[ListViewPlaylists.SelectedIndex];

                playlistWindow = new WindowPlaylist(currentPlaylist, player);
                playlistWindow.Show();
                playlistWindow.Closed += PlaylistWindow_Closed;
            }
            else
            {
                Log.Debug("No playlist detected");
            }
        }

        private void PlaylistWindow_Closed(object? sender, EventArgs e)
        { 
            lastEditedPlaylist = playlistWindow.playlist;
            lastEditedPlaylist.Save();
            playlistWindow = null;
            UpdateListView();
        }

        private void ListViewPlaylists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewPlaylists.SelectedIndex >= 0 && ListViewPlaylists.SelectedIndex < playlists.Count) // if no item is selected it breaks otherwise
            {
                currentPlaylist = playlists[ListViewPlaylists.SelectedIndex];
                

                if (currentPlaylist.SongListSorted.Count > 0)
                {
                    player.SetPlaylist(currentPlaylist, true);
                    UserControlPlayPauseSkip.SetPlayer(player);
                }
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            WindowAddEditPlaylist windowAddPlaylist = new WindowAddEditPlaylist(playlists);

            if (windowAddPlaylist.ShowDialog() == true)
            {
                String? Name = windowAddPlaylist.Name;
                if (Name != null)
                {
                    Playlist playlistNew = new Playlist(Name);
                    playlists.Add(playlistNew);
                    playlistNew.Save();
                }
            }
            UpdateListView();
        }

        private void UpdateListView()
        {
            ListViewPlaylists.Items.Clear();
            foreach (Playlist playlist in playlists)
            {
                ListViewPlaylists.Items.Add(playlist);
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Playlist playlistToDelete;
            if (ListViewPlaylists.SelectedIndex >= 0 && ListViewPlaylists.SelectedIndex < playlists.Count)
            {
                playlistToDelete = playlists[ListViewPlaylists.SelectedIndex];
                MessageBoxResult messageBoxresult = MessageBox.Show(
                    $"Delete Playlist {playlistToDelete.Name}",
                    "Confirmation Window",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                    );

                if (messageBoxresult == MessageBoxResult.Yes)
                {
                    string filePath = Path.Combine("Playlists", $"{playlistToDelete.Name.Replace(" ", "]")}.delos");
                    playlists.Remove(playlistToDelete);
                    player.Stop();

                    if (File.Exists(filePath))
                    {
                        try
                        {
                            File.Delete(filePath);
                        }
                        catch
                        {
                            MessageBox.Show($"Error deleting the playlist {playlistToDelete.Name}");
                        }
                    }
                }
            }
            UpdateListView();
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (currentPlaylist == null)
            {
                MessageBox.Show("No Playlist Selected");
                return;
            }

            WindowAddEditPlaylist windowAddPlaylist = new WindowAddEditPlaylist(playlists, true);

            if (windowAddPlaylist.ShowDialog() == true)
            {
                String? Name = windowAddPlaylist.Name;
                if (Name != null)
                {
                    currentPlaylist.Name = Name;
                }
            }
            UpdateListView();
        }
    }
}