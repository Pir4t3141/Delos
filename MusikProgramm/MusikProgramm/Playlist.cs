using System;
using System.Collections.Generic;
using System.Drawing.Design;
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

        public int currentSong { get; private set; } = 0;

        public List<Song> SongListSorted { get; set; }
        
        public Playlist(string name)
        {
            Name = name;
            SongListSorted = new List<Song>(SongList);
        }

        public static Playlist Import(string path)
        {
            string[] pathSplit = path.Split('.');
            string[] pathSplitName = pathSplit[0].Split("\\");
            Log.Debug($"Name of Playlist when importing: {pathSplitName[1]}");
            Playlist playlist = new Playlist(pathSplitName[1].Replace(']', ' '));
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
                playlist.SongListSorted = new List<Song>(playlist.SongList);
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
            if (currentSong != SongListSorted.Count-1)
            {
                currentSong++;
            }
            else
            {
                currentSong = 0;
            }

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
                if (currentSong == SongListSorted.Count - 1)
                {
                    currentSong = 0;
                }
                else
                {
                    currentSong++;
                }
                return SongListSorted[currentSong];
            }
        }

        public Song PreviousSong()
        {
            if (currentSong != 0)
            {
                currentSong--;   
            }
            else
            {
                currentSong = SongListSorted.Count - 1;
            }

            return SongListSorted[currentSong];
        }

        public void SaveProgress(int progress)
        {
            SongListSorted[currentSong].Progress = progress;
        }

        public void Shuffle()
        {
            Song lastSong = SongListSorted[currentSong];
            Log.Debug($"Before shuffle: {String.Join(',', SongListSorted)}");
            List<Song> list = new List<Song>(SongListSorted);
            SongListSorted.Clear();

            Random random = new Random();
            while (list.Count > 0)
            {
                int k = random.Next(list.Count);
                SongListSorted.Add(list[k]);
                list.RemoveAt(k);
            }
            Log.Debug($"After shuffle: {String.Join(',', SongListSorted)}");

            currentSong = SongListSorted.IndexOf(lastSong);
        }

        public void ResetShuffleSort()
        {
            Log.Debug("Reset shuffle");

            Song songPrevPlayed = SongListSorted[currentSong];

            SongListSorted = new List<Song>(SongList);

            foreach (Song song in SongListSorted)
            {
                if (songPrevPlayed == song)
                {
                    currentSong = SongListSorted.IndexOf(song);
                }
            }
        }

        public override string ToString()
        {
            return $"{Name}, {SongNumber} Songs, Playtime: {TimeSpan.FromSeconds(Playtime):mm\\:ss}";
        }
    }
}
