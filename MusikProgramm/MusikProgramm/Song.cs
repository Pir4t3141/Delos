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
                return Convert.ToInt32(new AudioFileReader(Path).TotalTime); //TODO: Fix
            }

            private set {}
        }

        public uint ReleaseYear { get; set; } // Year of release
        public string[] Artists { get; set; } // Name of artists
        public string Path { get; set; } // Path of file

        public int ?Progress { get; set; } // How long you listened the last time TODO: save into here LOGIC: If next song Progress = null; if not null continue here; if playlist stops playing save TODO: Implement Logic where needed
        // Progress in seconds
        public string Album { get; set; }

        public Song(string path){
            Path = path;
            loadFromMetaData();
        } 

        public void loadFromMetaData()
        {
            TagLib.File TaglibFile = TagLib.File.Create(Path);
            TagLib.Tag FileTag = TaglibFile.Tag; // metadata

            Name = FileTag.Title;
            Artists = FileTag.Performers;
            Album = FileTag.Album;
            ReleaseYear = FileTag.Year; // unsigned integer (year can't be negative)
        }

        public void editMetaData()
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
        
        public string serializeToString()
        {
            //Name]Length]ReleaseYear]Path]Progress]Album]Artist1[Artist2[Artist3[....
            //Seperate with ] because no title/Name of artist contains ] (to my knowledge)
            string serialized = $"{Name}]{Length}]{ReleaseYear}]{Path}]{Progress}]{Album}]";
            foreach (string artist in Artists)
            {
                serialized += $"{artist}[";
            }

            return serialized; //TODO Correct Return
        }

        public static Song deserialize(string SerializedString) // TODO: Ask Teacher if faster than reading metadata 
        {
            string[] strings = SerializedString.Split(']'); // no try{}catch{} needed: add in playlist class TODO: Add in Playlist class
            Song song = new Song(strings[3])
            {
                Name = strings[0],
                Length = Convert.ToInt32(strings[1]),
                ReleaseYear = Convert.ToUInt32(strings[2]),
                Album = strings[5]
            };
            if (strings[4] == "null")
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