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
using System.Windows.Shapes;

namespace MusikProgramm
{
    /// <summary>
    /// Interaction logic for WindowPlaylist.xaml
    /// </summary>
    public partial class WindowPlaylist : Window
    {
        public Playlist playlist { get; private set; } = new Playlist("Empty");
        private Player player = new Player();

        public WindowPlaylist(Playlist playlist, Player player)
        {
            InitializeComponent();

            this.playlist = playlist;
            this.player = player;
            LabelName.Content = playlist.Name;

            foreach (Song song in playlist.SongListSorted)
            {
                ListViewPlaylists.Items.Add(song);
            }

            UserControlPlayPauseSkip.SetPlayer(player);
        }
    }
}
