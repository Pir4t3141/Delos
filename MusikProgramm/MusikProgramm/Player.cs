using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace MusikProgramm
{
    public enum PlayerStatus
    {
        UNKNOWN = 0,
        STOPPED = 1,
        PLAYING = 2,
        PAUSED = 3
    }

    public enum PlayerPlaylistStatus
    {
        UNKNOWN = 0,
        SHUFFLE = 1,
        REPEAT = 2,
        SHUFFLEREPEAT = 3,
        NONE = 4
    }

    public class Player
    {
        public PlayerStatus Status { get; set; } = PlayerStatus.STOPPED;
        public PlayerPlaylistStatus StatusPlaylist { get; set; } = PlayerPlaylistStatus.NONE;

        public AudioFileReader? audiofile;
        public WasapiOut? outputDevice;

        private Playlist currentPlaylist;

        public event EventHandler<PlayerStatus> PlayerStatusChanged;

        public event EventHandler<PlayerPlaylistStatus> PlayerPlaylistStatusChanged;

        private bool shuffle = false;

        private bool repeat = false;

        private int previousVolume = 100;

        public void Play()
        {
            if (outputDevice != null)
            {
                audiofile.Volume = 1.0f;
                Status = PlayerStatus.PLAYING;
                outputDevice.Play();
                NotifyStatusChanged();
            }
        }

        public void Stop() 
        {
            if (outputDevice != null)
            {
                Status = PlayerStatus.STOPPED;
                outputDevice.Dispose();
                NotifyStatusChanged();
            }   
        }
        
        public void Pause() 
        {
            if (outputDevice != null)
            {
                audiofile.Volume = 0.0f; // kein nachklang mehr
                outputDevice.Pause();
                Status = PlayerStatus.PAUSED;
                NotifyStatusChanged();
            }
        }

        public void Skip() //Skip
        {
            if (outputDevice != null)
            {
                outputDevice.Dispose();
                audiofile = new AudioFileReader(currentPlaylist.NextSong(false).Path);
                outputDevice = new WasapiOut();
                outputDevice.Init(audiofile);
                outputDevice.Play();
            }
        }

        private void OutputDevice_PlaybackStopped(object? sender, StoppedEventArgs e)
        {
            Log.Debug("Playing Next song in playlist");
            audiofile = new AudioFileReader(currentPlaylist.NextSong(false).Path);
            outputDevice = new WasapiOut();
            outputDevice.Init(audiofile);
            outputDevice.Play();
            
        }

        public void Previous() 
        {
            if (outputDevice != null)
            {
                outputDevice.Dispose();
                audiofile = new AudioFileReader(currentPlaylist.PreviousSong().Path);
                outputDevice = new WasapiOut();
                outputDevice.Init(audiofile);
                outputDevice.Play();
            }
        }

        public void NotifyStatusChanged() 
        {
            PlayerStatusChanged?.Invoke(this, Status);
        }

        public void NotifyStatusPlaylistChange()
        {
            PlayerPlaylistStatusChanged?.Invoke(this, StatusPlaylist);
        }

        public void SetPlaylist(Playlist playlist)
        {
            if (outputDevice != null)
            {
                outputDevice.Dispose();
            }

            Status = PlayerStatus.PLAYING;
            currentPlaylist = playlist;

            audiofile = new AudioFileReader(playlist.NextSong(true).Path);
            outputDevice = new WasapiOut();
            outputDevice.Init(audiofile);
            outputDevice.Play();

            outputDevice.PlaybackStopped += OutputDevice_PlaybackStopped;
        }

        public void Shuffle()
        {
            if (!shuffle)
            {
                currentPlaylist.Shuffle();
                shuffle = true;

                if (repeat)
                {
                    StatusPlaylist = PlayerPlaylistStatus.SHUFFLEREPEAT;
                }
                else
                {
                    StatusPlaylist = PlayerPlaylistStatus.SHUFFLE;
                }
            }
            else
            {
                if (repeat)
                {
                    StatusPlaylist = PlayerPlaylistStatus.REPEAT;
                }
                else
                {
                    StatusPlaylist = PlayerPlaylistStatus.NONE;
                }
                currentPlaylist.ResetShuffleSort();
                shuffle = false;
            }
            NotifyStatusPlaylistChange();
        }

        public void Repeat()
        {
            if (!repeat)
            {
                currentPlaylist.Repeat = true;
                repeat = true;
                if (shuffle)
                {
                    StatusPlaylist = PlayerPlaylistStatus.SHUFFLEREPEAT;
                }
                else
                {
                    StatusPlaylist = PlayerPlaylistStatus.REPEAT;
                }
            }
            else
            {
                currentPlaylist.Repeat = false;
                repeat = false;

                if (shuffle)
                {
                    StatusPlaylist = PlayerPlaylistStatus.SHUFFLE;
                }
                else
                {
                    StatusPlaylist = PlayerPlaylistStatus.NONE;
                }
            }
            NotifyStatusPlaylistChange();
        }
    }
}
