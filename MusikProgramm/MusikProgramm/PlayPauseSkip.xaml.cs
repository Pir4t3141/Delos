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
using System.Windows.Threading;
using TagLib.Mpeg;

namespace MusikProgramm
{
    /// <summary>  
    /// Interaction logic for PlayPauseSkip.xaml  
    /// </summary>  
    public partial class PlayPauseSkip : UserControl
    {
        public Player? player = null;
        private bool manualVolumeChange = false;

        private DispatcherTimer timer = new DispatcherTimer();

        public PlayPauseSkip()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(0.25);
            timer.Tick += Timer_Tick;
        }

        private void Player_PlayerStatusChanged(object? sender, PlayerStatus e)
        {
            switch (e)
            {
                case PlayerStatus.UNKNOWN:
                    break;
                case PlayerStatus.STOPPED:
                    timer.Stop();
                    break;
                case PlayerStatus.PLAYING:
                    timer.Start();
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

        private void Timer_Tick(object? sender, EventArgs e)
        {
            player.NotifyProgressChanged();
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

        public void SetPlayer(Player player)
        {
            this.player = player;
            if (player.Status != PlayerStatus.PLAYING && player.Status != PlayerStatus.PAUSED)
            {
                player.Play();
            }

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.25);
            timer.Tick += Timer_Tick;

            player.PlayerStatusChanged += Player_PlayerStatusChanged;
            player.PlayerPlaylistStatusChanged += Player_PlayerPlaylistStatusChanged;
            player.AudioFileReaderVolumeChanged += Player_AudioFileReaderVolumeChanged;
            player.AudioFileProgressChanged += Player_AudioFileProgressChanged;

            switch (player.Status)
            {
                case PlayerStatus.UNKNOWN:
                    break;
                case PlayerStatus.STOPPED:
                    break;
                case PlayerStatus.PLAYING:
                    timer.Start();
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

            if (player.audiofile != null)
            {
                manualVolumeChange = true;
                SliderVolume.Value = player.previousVolume * 100;
                LabelVolume.Content = $"Volume: {(int)SliderVolume.Value}";

                SliderProgress.Value = (player.audiofile.CurrentTime * 100) / player.audiofile.TotalTime;
                LabelProgress.Content = $"Progress: {player.audiofile.CurrentTime:mm\\:ss}/{player.audiofile.TotalTime:mm\\:ss}";
            }
        }

        private void Player_AudioFileProgressChanged(object? sender, EventArgs e)
        {
            SliderProgress.Value = (player.audiofile.CurrentTime * 100) / player.audiofile.TotalTime;
            LabelProgress.Content = $"Progress: {player.audiofile.CurrentTime:mm\\:ss}/{player.audiofile.TotalTime:mm\\:ss}";
        }

        private void Player_AudioFileReaderVolumeChanged(object? sender, EventArgs e)
        {
            manualVolumeChange = true;
            SliderVolume.Value = player.audiofile.Volume * 100;
            LabelVolume.Content = $"Volume: {(int)SliderVolume.Value}";
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
            if (LabelVolume != null && !manualVolumeChange)
            {
                player.SetVolume((float)SliderVolume.Value / 100);
                LabelVolume.Content = $"Volume: {(int)SliderVolume.Value}";
            }
            manualVolumeChange = false;
        }

        private void SliderProgress_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            player.Pause();
            timer.Stop();

        }

        private void SliderProgress_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            if (LabelProgress != null && player != null && player.audiofile != null)
            {
                timer.Start();

                player.SetProgress(TimeSpan.FromSeconds((SliderProgress.Value / 100) * (double)player.audiofile.TotalTime.TotalSeconds));

                LabelProgress.Content = $"Progress: {player.audiofile.CurrentTime:mm\\:ss}/{player.audiofile.TotalTime:mm\\:ss}";
            }
            player.Play();
        }
    }
}
