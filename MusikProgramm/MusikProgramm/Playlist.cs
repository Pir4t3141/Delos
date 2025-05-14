using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusikProgramm
{
    public class Playlist
    {
        public int SongAnzahl { get; set; } //TODO: edit get; set;

        public bool Shuffle { get; set; } = false;

        public bool Repeat { get; set; } = false;

        public string Name { get; set; }

        public int Playtime { get; set; } //TODO: edit get; set;

        public List<Song> SongList { get; set; }

        public int currentSongs { get; set; } = 0;

        public List<Song> SongListSorted { get; set; } // TODO: Make it so that it is songlist on start

        public Playlist(string name)
        {
            //TODO  
        }

        public void import(string path)
        {
            //TODO: Import playlist
        }

        public void save(string path)
        {
            //TODO: save
        }

        public void addSong(Song song)
        {
            //TODO
        }

        public void removeSong(Song song)
        {
            //TODO
        }

        public void skip()
        {
            //TODO
        }

        public void sort()
        {
            //TODO: look how to do it
        }

        public void nextSong()
        {
            //TODO
        }

        public void previousSong()
        {
            //TODO
        }

        public void serialized()
        {
            //TODO
        }
    }
}
