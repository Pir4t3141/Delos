using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace MusikProgramm
{
    public class Playlist
    {
        public int SongAnzahl
        {
            get {
                return SongList.Count();
            }
            private set { }
        }

        //public bool Shuffle { get; set; } = false;

        public bool Repeat { get; set; } = false;

        public string Name { get; set; }

        public int Playtime
        {
            get
            {
                int playtimeS = 0;
                foreach (Song song in SongList)
                {
                    playtimeS += song.Length;
                }
                return playtimeS;
            }
            private set { }
        } 

        public List<Song> SongList { get; set; }

        public int currentSong { get; set; } = 0;

        public List<Song> SongListSorted { get; set; } // TODO: Make it so that it is songlist on start
        
        public Playlist(string name)
        {
            Name = name;
        }

        public static Playlist import(string path)
        {
            string[] pathSplit = path.Split('.');
            Playlist playlist = new Playlist(pathSplit[0]);
            Log.Debug(pathSplit[0]);

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                        {
                            Song song = new Song(line);
                            playlist.SongList.Add(song);
                        }
                    }
                }
                return playlist;
            }
            catch
            {
                Log.Error("Problem Importing");
                return null;
            }
        }

        public void save(string path)
        {
            // using char "]" because it is in no name of a playlist
            using (StreamWriter sw = new StreamWriter($"{Name.Replace(" ", "]")}.txt")) // TODO: find better file format
            {
                Log.Debug("Saving Playlist");
                foreach (Song song in SongList)
                {
                    //Name]Length]ReleaseYear]Path]Progress]Album]Artist1[Artist2[Artist3[....
                    sw.WriteLine(song.serializeToString());
                }
            }
        }

        public void addSong(Song song)
        {
            SongList.Add(song);
        }

        public void removeSong(Song song)
        {
            SongList.Remove(song);
        }

        public Song skip()
        {
            currentSong += 1;
            return SongListSorted[currentSong];
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

        public void serialize()
        {
            // TODO: find out what to do here
        }

        public void stop(int progress)
        {
            SongListSorted[currentSong].Progress = progress;
        }

        public void shuffle()
        {
            Log.Debug($"Before shuffle: {SongListSorted}");
            List<Song> list = SongListSorted;
            SongListSorted.Clear();

            Random random = new Random();
            // Fisher-Yates shuffle
            int n = list.Count;
            int k;
            while (n > 1)
            {
                n--;
                k = random.Next(n + 1);
                SongListSorted.Add(list[k]);
            }
            Log.Debug($"After shuffle: {SongListSorted}");
        }

        public void resetShuffleSort()
        {
            Log.Debug("Reset shuffle");
            SongListSorted = SongList;
        }
    }
}
