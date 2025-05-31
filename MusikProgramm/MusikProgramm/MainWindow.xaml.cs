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
using System.Windows.Shapes;
using NAudio;
using NAudio.Wave;
using Serilog;

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

        public AudioFileReader audiofile;
        public WasapiOut outputDevice;

        public List<Playlist> playlists { get; private set; } = new List<Playlist>();
        public Playlist? currentPlaylist;
        public bool shuffle;
        private WindowPlaylist ?playlistWindow;
        public MainWindow()
        {
            InitializeComponent();

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
                        if (fileSeperated[0] == "txt") // only looks for playlists //TODO: Change .txt to playlist format
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

            UserControlPlayPauseSkip.mainWindow = this;
        }

        private void OutputDevice_PlaybackStopped(object? sender, StoppedEventArgs e)
        {
            Song nextSong = currentPlaylist.NextSong(false);
            PlaySong(nextSong);
        }

        private void ListViewPlaylists_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // view playlist

            //currentPlaylist = playlists[ListViewPlaylists.SelectedIndex];

            if (currentPlaylist != null && playlistWindow == null)
            {
                playlistWindow = new WindowPlaylist(currentPlaylist);
                playlistWindow.mainWindow = this;
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
            playlistWindow = null;
        }

        private void ListViewPlaylists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Switch Playlist
            if (outputDevice!= null)
            {
                StopOutputDevice();
            }

            currentPlaylist = playlists[ListViewPlaylists.SelectedIndex];

            if (currentPlaylist != null && currentPlaylist.SongList.Count >= 1)
            {
                Song song = currentPlaylist.NextSong(true); // starts playlist
                PlaySong(song);
                UserControlPlayPauseSkip.ResetAfterPlaylistSwitch();
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            WindowAddEditPlaylist windowAddPlaylist = new WindowAddEditPlaylist(this);

            if (windowAddPlaylist.ShowDialog() == true)
            {
                String? Name = windowAddPlaylist.Name;
                if (Name != null)
                {
                    playlists.Add(new Playlist(Name));
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

        public void PlaySong(Song nextSong)
        {
            audiofile = new AudioFileReader(nextSong.Path);
            outputDevice = new WasapiOut();
            outputDevice.Init(audiofile);
            outputDevice.Play();
            outputDevice.PlaybackStopped += OutputDevice_PlaybackStopped;
        }

        public void StopOutputDevice()
        {
            outputDevice.PlaybackStopped -= OutputDevice_PlaybackStopped; // sonst wird das event beim stoppen ausgelöst
            outputDevice.Stop();
            outputDevice.Dispose();
            outputDevice = null;
        }
    }
}