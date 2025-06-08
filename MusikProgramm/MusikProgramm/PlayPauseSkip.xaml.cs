using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        public Player? player = null;


        public PlayPauseSkip()
        {
            InitializeComponent();
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
                    player.audiofile.Volume = ((float)SliderVolume.Value) / 100;
                    ImagePlayPause.Source = new BitmapImage(new Uri("images/pause.png", UriKind.Relative));
                    // TODO: change icon
                    break;
                case PlayerStatus.PAUSED:
                    ImagePlayPause.Source = new BitmapImage(new Uri("images/play.png", UriKind.Relative));
                    // TODO: change icon
                    break;
                default:
                    break;
            }
        }

        private void ButtonPlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (player != null)
            {
                if (player.Status == PlayerStatus.PLAYING)
                {
                    player.Pause();
                }
                else
                {
                    player.Play();
                }
            }
        }

        private void ButtonShuffle_Click(object sender, RoutedEventArgs e)
        {
            if (player != null)
            {
                player.Shuffle();
            }
        }

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (player != null)
            {
                player.Previous();
            }
        }

        private void ButtonSkip_Click(object sender, RoutedEventArgs e)
        {
            if (player != null)
            {
                player.Skip();
            }
        }

        private void ButtonRepeat_Click(object sender, RoutedEventArgs e)
        {
            if (player != null)
            {
                player.Repeat();
            }
        }

        public void ResetAfterPlaylistSwitch()
        {
            //TODO  
        }

        public void SetPlayer(Player player)
        {
            this.player = player;


            player.PlayerStatusChanged += Player_PlayerStatusChanged;
            player.PlayerPlaylistStatusChanged += Player_PlayerPlaylistStatusChanged;

            switch (player.Status)
            {
                case PlayerStatus.UNKNOWN:
                    break;
                case PlayerStatus.STOPPED:
                    break;
                case PlayerStatus.PLAYING:
                    player.audiofile.Volume = ((float)SliderVolume.Value) / 100;
                    ImagePlayPause.Source = new BitmapImage(new Uri("images/pause.png", UriKind.Relative));
                    break;
                case PlayerStatus.PAUSED:
                    ImagePlayPause.Source = new BitmapImage(new Uri("images/play.png", UriKind.Relative));
                    break;
                default:
                    break;
            }

            switch (player.StatusPlaylist)
            {
                case PlayerPlaylistStatus.UNKNOWN:
                    break;
                case PlayerPlaylistStatus.SHUFFLE:
                    ImageShuffle.Source = new BitmapImage(new Uri("images/shuffle_thin_on.png", UriKind.Relative));
                    ImageRepeat.Source = new BitmapImage(new Uri("images/repeat_thin.png", UriKind.Relative));
                    break;
                case PlayerPlaylistStatus.REPEAT:
                    ImageShuffle.Source = new BitmapImage(new Uri("images/shuffle_thin.png", UriKind.Relative));
                    ImageRepeat.Source = new BitmapImage(new Uri("images/repeat_thin_on.png", UriKind.Relative));
                    break;
                case PlayerPlaylistStatus.SHUFFLEREPEAT:
                    ImageRepeat.Source = new BitmapImage(new Uri("images/repeat_thin_on.png", UriKind.Relative));
                    ImageShuffle.Source = new BitmapImage(new Uri("images/shuffle_thin_on.png", UriKind.Relative));
                    break;
                case PlayerPlaylistStatus.NONE:
                    ImageShuffle.Source = new BitmapImage(new Uri("images/shuffle_thin.png", UriKind.Relative));
                    ImageRepeat.Source = new BitmapImage(new Uri("images/repeat_thin.png", UriKind.Relative));
                    break;
                default:
                    break;
            }
        }

        private void Player_PlayerPlaylistStatusChanged(object? sender, PlayerPlaylistStatus e)
        {
            switch (e)
            {
                case PlayerPlaylistStatus.UNKNOWN:
                    break;
                case PlayerPlaylistStatus.SHUFFLE:
                    ImageShuffle.Source = new BitmapImage(new Uri("images/shuffle_thin_on.png", UriKind.Relative));
                    ImageRepeat.Source = new BitmapImage(new Uri("images/repeat_thin.png", UriKind.Relative));
                    break;
                case PlayerPlaylistStatus.REPEAT:
                    ImageShuffle.Source = new BitmapImage(new Uri("images/shuffle_thin.png", UriKind.Relative));
                    ImageRepeat.Source = new BitmapImage(new Uri("images/repeat_thin_on.png", UriKind.Relative));
                    break;
                case PlayerPlaylistStatus.SHUFFLEREPEAT:
                    ImageRepeat.Source = new BitmapImage(new Uri("images/repeat_thin_on.png", UriKind.Relative));
                    ImageShuffle.Source = new BitmapImage(new Uri("images/shuffle_thin_on.png", UriKind.Relative));
                    break;
                case PlayerPlaylistStatus.NONE:
                    ImageShuffle.Source = new BitmapImage(new Uri("images/shuffle_thin.png", UriKind.Relative));
                    ImageRepeat.Source = new BitmapImage(new Uri("images/repeat_thin.png", UriKind.Relative));
                    break;
                default:
                    break;
            }
        }

        private void SliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (LabelVolume != null)
            {
                if (player.audiofile != null)
                {
                    player.audiofile.Volume = ((float)SliderVolume.Value) / 100; // 100 = 1.0; 42 = 0.42
                }
                LabelVolume.Content = $"Volume: {(int)SliderVolume.Value}";
            }
        }

        private void SliderProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            /* funktioniert nicht ganz richtig
            if (LabelProgress != null && player.audiofile != null)
            {
                if (player.audiofile != null)
                {
                    player.audiofile.CurrentTime = TimeSpan.FromSeconds((SliderProgress.Value / 100) * (double)player.audiofile.TotalTime.TotalSeconds);
                }
                LabelProgress.Content = $"Progress: {player.audiofile.CurrentTime:mm\\:ss}/{player.audiofile.TotalTime:mm\\:ss}";
            }*/
        }
    }
}
