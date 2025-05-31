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
using TagLib.Mpeg;

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
            if (mainWindow.currentPlaylist != null && mainWindow.currentPlaylist.SongList.Count >= 1 && mainWindow.outputDevice != null)
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
            // Doesn't work completely TODO: Fix when switching between playlists and playing/pausing to fast

        }

        private void ButtonShuffle_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow.currentPlaylist != null && mainWindow.currentPlaylist.SongList.Count >= 1 && mainWindow.outputDevice != null)
            {
                if (mainWindow.shuffle == false)
                {
                    mainWindow.shuffle = true;
                    mainWindow.currentPlaylist.Shuffle();
                }
                else
                {
                    mainWindow.shuffle = false;
                    mainWindow.currentPlaylist.ResetShuffleSort();
                }
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            var prevPlaybState = mainWindow.outputDevice.PlaybackState;
            if (mainWindow.currentPlaylist != null && mainWindow.currentPlaylist.SongList.Count >= 1 && mainWindow.outputDevice != null)
            {
                Song nextSong = mainWindow.currentPlaylist.PreviousSong();
                mainWindow.StopOutputDevice();
                mainWindow.PlaySong(nextSong);
                if (prevPlaybState == PlaybackState.Paused)
                {
                    mainWindow.outputDevice.Pause();
                }
            }
        }

        private void ButtonSkip_Click(object sender, RoutedEventArgs e)
        {
            var prevPlaybState = mainWindow.outputDevice.PlaybackState;
            if (mainWindow.currentPlaylist != null && mainWindow.currentPlaylist.SongList.Count >= 1 && mainWindow.outputDevice != null)
            {
                Song nextSong = mainWindow.currentPlaylist.Skip();
                mainWindow.StopOutputDevice();
                mainWindow.PlaySong(nextSong);
                if (prevPlaybState == PlaybackState.Paused)
                {
                    mainWindow.outputDevice.Pause();
                }
            }
        }

        private void ButtonRepeat_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow.currentPlaylist != null && mainWindow.currentPlaylist.SongList.Count >= 1 && mainWindow.outputDevice != null)
            {
                if (mainWindow.currentPlaylist.Repeat == false)
                {
                    mainWindow.currentPlaylist.Repeat = true;
                    ButtonRepeat.Background = Brushes.Green;
                }
                else
                {
                    mainWindow.currentPlaylist.Repeat = false;
                    ButtonRepeat.Background = null;
                }
            }
        }

        public void ResetAfterPlaylistSwitch()
        {
            ButtonPlayPause.Background = imageBrushPause;
        }
    }
}
