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
                    // TODO: change icon
                    break;
                case PlayerStatus.PAUSED:
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
                player.Play();
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
        }
    }
}
