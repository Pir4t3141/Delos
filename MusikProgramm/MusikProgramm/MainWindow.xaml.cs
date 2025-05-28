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

        public MainWindow()
        {
            InitializeComponent();

            //Serilog Einrichten

            Log.Logger = new LoggerConfiguration().
                MinimumLevel.Debug().
                WriteTo.File("musikprogramm.log", rollingInterval: RollingInterval.Month).
                CreateLogger();

            Log.Debug("Started MainWindow");
        }
    }
}