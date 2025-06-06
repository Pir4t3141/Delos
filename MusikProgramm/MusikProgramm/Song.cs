    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using NAudio;
using NAudio.Wave;
using TagLib; //for metadata
using Serilog;

namespace MusikProgramm
{
    public class Song
    {
        //TODO: Add genre maybe
        //TODO: decide which props are public and which are private
        public string Name { get; set; }

        public int Length
        {
            get {
                return Convert.ToInt32(new AudioFileReader(Path).TotalTime.TotalSeconds);
            }

            private set {}
        }

        public uint ReleaseYear { get; set; } // Year of releasea
        public string[] Artists { get; set; } // Name of artists
        public string Path { get; set; } // Path of file

        public int ?Progress { get; set; } // How long you listened the last time TODO: save into here LOGIC: If next song Progress = null; if not null continue here; if playlist stops playing save TODO: Implement Logic where needed
        // Progress in seconds
        public string Album { get; set; }

        public Song(string path){
            Path = path;
            LoadFromMetaData();
        } 

        public void LoadFromMetaData()
        {
            TagLib.File TaglibFile = TagLib.File.Create(Path);
            TagLib.Tag FileTag = TaglibFile.Tag; // metadata

            Name = FileTag.Title;
            Artists = FileTag.Performers;
            Album = FileTag.Album;
            ReleaseYear = FileTag.Year; // unsigned integer (year can't be negative)
        }

        public void EditMetaData()
        {
            TagLib.File TaglibFile = TagLib.File.Create(Path);
            TagLib.Tag FileTag = TaglibFile.Tag; // metadata

            FileTag.Title = Name;
            FileTag.Performers = Artists;
            FileTag.Album = Album;
            FileTag.Year = ReleaseYear;
        }

        public override string ToString()
        {
            return $"{Name} released on {ReleaseYear} by {string.Join(",", Artists)}";
        }
        
        public string SerializeToString()
        {
            //Name]Length]ReleaseYear]Path]Progress]Album]Artist1[Artist2[Artist3[....
            //Seperate with ] because no title/Name of artist contains ] (to my knowledge)
            if (String.IsNullOrEmpty(Name))
            {
                Name = "noNameGiven";
            }
            if (String.IsNullOrEmpty(Album))
            {
                Album = "noAlbumGiven";
            }
            string serialized = $"{Name}]{Length}]{ReleaseYear}]{Path}]{Progress}]{Album}]";
            
            if (Artists != null && Artists.Length > 0)
            {
                foreach (string artist in Artists)
                {
                    serialized += $"{artist}[";
                }
            }
            else
            {
                serialized += "noArtistsGiven";
                Log.Error("No artists Serialized");
            }

            return serialized;
        }

        public static Song Deserialize(string SerializedString) // TODO: Ask Teacher if faster than reading metadata 
        {

            //]17]0]C:\Users\Familie_Reichart\Downloads\epic.mp3]]]
            string[] strings = SerializedString.Split(']'); // no try{}catch{} needed: add in playlist class TODO: Add in Playlist class
            Song song = new Song(strings[3])
            {
                Name = strings[0],
                Length = Convert.ToInt32(strings[1]),
                ReleaseYear = Convert.ToUInt32(strings[2]),
                Album = strings[5]
            };
            if (string.IsNullOrEmpty(strings[4]))
            {
                song.Progress = null;
            }
            else
            {
                song.Progress = Convert.ToInt32(strings[4]);
            }

            string[] stringsArtists = strings[6].Split('[');

            song.Artists = stringsArtists;

            return song;
        }
    }
}