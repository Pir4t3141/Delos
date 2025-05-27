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

        //public bool Shuffle { get; set; } = false;

        public bool Repeat { get; set; } = false;

        public string Name { get; set; }

        public int Playtime { get; set; } //TODO: edit get; set;

        public List<Song> SongList { get; set; }

        public int currentSong { get; set; } = 0;

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
            SongList.Add(song);
        }

        public void removeSong(Song song)
        {
            SongList.Remove(song);
        }

        public void skip()
        {
            currentSong += 1;
        }

        public void sort()
        {
            //TODO: look how to do it
        }

        public Song nextSong(bool firstStart)
        {
            if (firstStart)
            {
                foreach (Song song in SongListSorted)
                {
                    if (song.Progress != null)
                    {
                        currentSong = SongListSorted.IndexOf(song);
                        return song;
                    }
                }
                return SongListSorted[0];
            }
            else if (Repeat)
            {
                return SongListSorted[currentSong];
            }
            else
            {
                currentSong++;
                return SongListSorted[currentSong];
            }
        }

        public Song previousSong()
        {
            currentSong--;
            return SongListSorted[currentSong];
        }

        public void serialized()
        {
            //TODO
        }

        public void stop(int progress)
        {
            SongListSorted[currentSong].Progress = progress;
        }

        public void shuffle()
        {
            List<Song> list = SongListSorted;

            // TODO
        }

        public void resetShuffleSort()
        {
            SongListSorted = SongList;
        }
    }
}
