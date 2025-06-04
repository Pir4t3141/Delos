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
        private Playlist playlist;
        public MainWindow mainWindow;

        public WindowPlaylist(Playlist playlist, MainWindow mainWindow)
        {
            InitializeComponent();

            this.playlist = playlist;
            this.mainWindow = mainWindow;
            UserControlPlayPauseSkip.mainWindow = mainWindow;
            LabelName.Content = playlist.Name;

            foreach (Song song in playlist.SongListSorted)
            {
                ListViewPlaylists.Items.Add(song);
            }
        }
    }
}
