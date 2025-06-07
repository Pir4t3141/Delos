using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using System.Runtime.InteropServices.Marshalling;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Reflection.Metadata;

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

        private bool manualStop = false; // used to see if stop of outputdevice was stopped because song is finished or Stop() was used (continue playing next song or don't) 


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
                manualStop = true;
                Status = PlayerStatus.STOPPED;
                outputDevice.Dispose();
                NotifyStatusChanged();
            }   
        }

        public void SaveProgress()
        {
            if (outputDevice != null)
            {
                currentPlaylist.SaveProgress(Convert.ToInt32(audiofile.CurrentTime.TotalSeconds));
                currentPlaylist.Save();
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
                Stop();

                SetupNextSong(currentPlaylist.NextSong(false).Path);
            }
        }

        private void OutputDevice_PlaybackStopped(object? sender, StoppedEventArgs e)
        {
            if (!manualStop)
            {
                Log.Debug("Playing Next song in playlist");

                SetupNextSong(currentPlaylist.NextSong(false).Path);
            }
            else
            {
                Log.Debug("Manual stop received, not playing next song");
            }
            manualStop = false;
        }

        public void Previous() 
        {
            if (outputDevice != null)
            {
                Stop();

                SetupNextSong(currentPlaylist.PreviousSong().Path);
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
            SaveProgress();
            Stop();

            currentPlaylist = playlist;

            Song nextSong = playlist.NextSong(true);
            manualStop = false;
            SetupNextSong(nextSong.Path);

            if (nextSong.Progress != null)
            {
                audiofile.CurrentTime = TimeSpan.FromSeconds(Convert.ToDouble(nextSong.Progress));
                nextSong.Progress = null;
            }

            Status = PlayerStatus.PLAYING;
            NotifyStatusChanged();
        }

        public void Shuffle()
        {
            if (!shuffle & currentPlaylist != null)
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
            else if (shuffle & currentPlaylist != null)
            {
                if (repeat)
                {
                    StatusPlaylist = PlayerPlaylistStatus.REPEAT;
                }
                else
                {
                    StatusPlaylist = PlayerPlaylistStatus.NONE;
                }
                shuffle = false;
                currentPlaylist.ResetShuffleSort();
            }
            NotifyStatusPlaylistChange();
        }

        public void Repeat()
        {
            if (!repeat & currentPlaylist != null)
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
            else if (repeat & currentPlaylist != null)
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

        private void SetupNextSong(String path)
        {
            audiofile = new AudioFileReader(path);
            outputDevice = new WasapiOut();
            outputDevice.Init(audiofile);
            outputDevice.Play();

            outputDevice.PlaybackStopped += OutputDevice_PlaybackStopped;
        }
    }
}
