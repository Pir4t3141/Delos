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

        private List<Playlist> playlists = new List<Playlist>();
        private Playlist? currentPlaylist;
        private bool repeat;
        public MainWindow()
        {
            InitializeComponent();

            //Serilog setup
            Log.Logger = new LoggerConfiguration().
                MinimumLevel.Debug().
                WriteTo.File("musikprogramm.log", rollingInterval: RollingInterval.Month).
                CreateLogger();

            Log.Debug("Started MainWindow");

            
            Playlist playlistAJR = new Playlist("AJR");
            playlistAJR.AddSong(new Song("C:\\Users\\ratte\\Downloads\\tf_nemesis.mp3"));
            playlistAJR.AddSong(new Song("C:\\Users\\ratte\\Downloads\\AJR - BANG! (Official Video).mp3"));
            playlistAJR.AddSong(new Song("C:\\Users\\ratte\\Downloads\\AJR - World's Smallest Violin (Official Video).mp3"));
            playlistAJR.AddSong(new Song("C:\\Users\\ratte\\Downloads\\AJR - Yes I'm A Mess (Official Video).mp3"));
            playlistAJR.AddSong(new Song("C:\\Users\\ratte\\Downloads\\AJR - BANG! (Official Video).mp3"));
            playlistAJR.Save();

            Playlist playlistAJR2 = new Playlist("AJR2");
            playlistAJR2.AddSong(new Song("C:\\Users\\ratte\\Downloads\\tf_nemesis.mp3"));
            playlistAJR2.AddSong(new Song("C:\\Users\\ratte\\Downloads\\AJR - World's Smallest Violin (Official Video).mp3"));
            playlistAJR2.AddSong(new Song("C:\\Users\\ratte\\Downloads\\AJR - BANG! (Official Video).mp3"));
            playlistAJR2.AddSong(new Song("C:\\Users\\ratte\\Downloads\\AJR - Yes I'm A Mess (Official Video).mp3"));
            playlistAJR2.AddSong(new Song("C:\\Users\\ratte\\Downloads\\AJR - BANG! (Official Video).mp3"));
            playlistAJR2.Save();

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
            // next song

            Song nextSong = currentPlaylist.NextSong(false);
            audiofile = new AudioFileReader(nextSong.Path);
            outputDevice = new WasapiOut();
            outputDevice.Init(audiofile);
            outputDevice.Play();
            UserControlPlayPauseSkip.ResetAfterPlaylistSwitch();
        }

        private void ListViewPlaylists_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // view playlist
        }

        private void ListViewPlaylists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Switch Playlist
            currentPlaylist = playlists[ListViewPlaylists.SelectedIndex];

            if (currentPlaylist != null)
            {
                if (outputDevice != null)
                {
                    outputDevice.Stop();
                    currentPlaylist.currentSong = 0; // TODO: Remove this, add progress
                }
                Song song = currentPlaylist.NextSong(true); // starts playlist
                audiofile = new AudioFileReader(song.Path);
                outputDevice = new WasapiOut();
                outputDevice.Init(audiofile);
                outputDevice.Play();

                outputDevice.PlaybackStopped += OutputDevice_PlaybackStopped;
                UserControlPlayPauseSkip.ResetAfterPlaylistSwitch();
            }
        }
    }
}