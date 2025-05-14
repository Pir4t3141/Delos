using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MusikProgramm
{
    public class Song
    {
        public string Name { get; set; }

        public int Lenght { get; private set; } // Playtime in seconds TODO: get from NAudio
        public DateTime ReleaseDate { get; set; } // Date of release
        public string Artist { get; set; } // Name of artist
        public string Path { get; set; } // Path of file

        public int Progress { get; set; } // How long you listened the last time TODO: save into here

        public string Album { get; set; }

        public Song(string path){
            Path = path;
        }
        public void loadFromMetaData()
        {
            //TODO how to change
        }

        public void editMetaData()
        {
            //TODO
        }

        public override string ToString()
        {
            return $"{Name} released on {ReleaseDate} by {Artist}";
        }
        
        public string serializeToString()
        {
            //TODO
            return $"TODO"; //TODO Correct Return
        }

        public static Song deserialize(string SerializedString)
        {
            return new Song("TODO"); //TODO: Correct Return
        }
    }
}