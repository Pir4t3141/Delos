using NAudio.Wave;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusikProgramm
{
    /// <summary>
    /// Interaction logic for PlayPauseSkip.xaml
    /// </summary>
    public partial class PlayPauseSkip : UserControl
    {
        public MainWindow mainWindow;

        private ImageBrush imageBrushPlay = new ImageBrush();
        private ImageBrush imageBrushPause = new ImageBrush();
        public PlayPauseSkip()
        {
            InitializeComponent();

            imageBrushPlay.ImageSource = new BitmapImage(new Uri("images//play.png", UriKind.Relative));
            imageBrushPause.ImageSource = new BitmapImage(new Uri("images//pause.png", UriKind.Relative));

            imageBrushPlay.Stretch = Stretch.Uniform;
            imageBrushPause.Stretch = Stretch.Uniform;

            imageBrushPlay.TileMode = TileMode.None;
            imageBrushPause.TileMode = TileMode.None;
        }

        private void ButtonPlayPause_Click(object sender, RoutedEventArgs e)
        {
            // Doesn't work completely TODO: Fix when switching between playlists and playing/pausing to fast
            if (mainWindow.outputDevice != null)
            {
                if (mainWindow.outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    ButtonPlayPause.Background = imageBrushPlay;
                    mainWindow.outputDevice.Pause();
                }
                else
                {
                    ButtonPlayPause.Background = imageBrushPause;
                    mainWindow.outputDevice.Play();
                }
            }
        }

        private void ButtonShuffle_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonSkip_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonRepeat_Click(object sender, RoutedEventArgs e)
        {

        }

        public void ResetAfterPlaylistSwitch()
        {
            ButtonPlayPause.Background = imageBrushPause;
        }
    }
}
