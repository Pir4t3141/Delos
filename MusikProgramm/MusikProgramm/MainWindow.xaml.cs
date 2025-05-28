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
        public MainWindow()
        {
            InitializeComponent();

            //Serilog setup
            Log.Logger = new LoggerConfiguration().
                MinimumLevel.Debug().
                WriteTo.File("musikprogramm.log", rollingInterval: RollingInterval.Month).
                CreateLogger();

            Log.Debug("Started MainWindow");
            // TODO: Delete Following commet (Adding playlist for test purposes)
            /* Adds Playlist for Test Purposes
            Playlist pltest = new Playlist("PlaylistTest");
            pltest.addSong(new Song("C:\\Users\\Familie_Reichart\\Downloads\\epic.mp3"));

            Playlist pltest2 = new Playlist("PlaylistTest2");
            pltest2.addSong(new Song("C:\\Users\\Familie_Reichart\\Downloads\\epic.mp3"));

            pltest.save();
            pltest2.save();*/


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


        }

        private void ListViewPlaylists_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // view playlist
        }
    }
}