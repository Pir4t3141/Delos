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
        private string DirectoryName = "Playlists";
        public int SongNumber
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

        public List<Song> SongList { get; set; } = new List<Song>();

        public int currentSong { get; set; } = 0;

        public List<Song> SongListSorted { get; set; } = new List<Song>(); // TODO: Make it so that it is songlist on start
        
        public Playlist(string name)
        {
            Name = name;
        }

        public static Playlist Import(string path)
        {
            string[] pathSplit = path.Split('.');
            string[] pathSplitName = pathSplit[0].Split("\\");
            Log.Debug($"Name of Playlist when importing: {pathSplitName[1]}");
            Playlist playlist = new Playlist(pathSplitName[1]);
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
                            Log.Debug(line);
                            Song song = Song.Deserialize(line);
                            playlist.SongList.Add(song);
                        }
                    }
                }
                playlist.SongListSorted = playlist.SongList;
                return playlist;
            }
            catch
            {
                Log.Error("Problem Importing");
                return null;
            }
        }

        public void Save()
        {
            // using char "]" because it is in no name of a playlist
            if (!Directory.Exists(DirectoryName)){
                Directory.CreateDirectory(DirectoryName);
            }
            using (StreamWriter sw = new StreamWriter($"{DirectoryName}//{Name.Replace(" ", "]")}.txt")) // TODO: find better file format
            {
                Log.Debug("Saving Playlist");
                foreach (Song song in SongList)
                {
                    //Name]Length]ReleaseYear]Path]Progress]Album]Artist1[Artist2[Artist3[....
                    sw.WriteLine(song.SerializeToString());
                }
            }
        }

        public void AddSong(Song song)
        {
            SongList.Add(song);
            SongListSorted.Add(song);
        }

        public void RemoveSong(Song song)
        {
            SongList.Remove(song);
            SongListSorted.Remove(song);
        }

        public Song Skip()
        {
            currentSong += 1;
            return SongListSorted[currentSong];
        }

        public void Sort()
        {
            //TODO: look how to do it
        }

        public Song NextSong(bool firstStart)
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

        public Song PreviousSong()
        {
            currentSong--;
            return SongListSorted[currentSong];
        }

        public void Serialize()
        {
            // TODO: find out what to do here
        }

        public void Stop(int progress)
        {
            SongListSorted[currentSong].Progress = progress;
        }

        public void Shuffle()
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

        public void ResetShuffleSort()
        {
            Log.Debug("Reset shuffle");
            SongListSorted = SongList;
        }

        public override string ToString()
        {
            return $"{Name}, {SongNumber} Songs, Playtime: {Playtime}";
        }
    }
}
