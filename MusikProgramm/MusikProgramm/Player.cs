using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusikProgramm
{
    public enum PlayerStatus
    {
        UNKNOWN = 0,
        STOPPED = 1,
        PLAYING = 2,
        PAUSED = 3
    }

    public enum PlayerPlaylistStuatus
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

        public AudioFileReader audiofile;
        public WasapiOut outputDevice;

        private Playlist currentPlaylist;

        public event EventHandler<PlayerStatus> PlayerStatusChanged;

        private bool shuffle = false;

        private bool repeat = false;

        public void Play()
        {
            if (outputDevice != null)
            {
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
                Status = PlayerStatus.PAUSED;
                outputDevice.Pause();
                NotifyStatusChanged();
            }
        }

        public void Skip() //Skip
        {
            outputDevice.Dispose();
            if (outputDevice != null)
            {
                audiofile = new AudioFileReader(currentPlaylist.NextSong(false).Path);
                outputDevice = new WasapiOut();
                outputDevice.Init(audiofile);
                outputDevice.Play();
            }
        }

        private void OutputDevice_PlaybackStopped(object? sender, StoppedEventArgs e)
        {
            audiofile = new AudioFileReader(currentPlaylist.NextSong(false).Path);
            outputDevice = new WasapiOut();
            outputDevice.Init(audiofile);
            outputDevice.Play();
            
        }

        public void Previous() 
        {
            outputDevice.Dispose();
            if (outputDevice != null)
            {
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

        public void SetPlaylist(Playlist playlist)
        {
            if (outputDevice != null)
            {
                outputDevice.Dispose();
            }

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
            }
            else
            {
                currentPlaylist.ResetShuffleSort();
                shuffle = false;
            }
        }

        public void Repeat()
        {
            if (!repeat)
            {
                currentPlaylist.Repeat = true;
                repeat = true;
            }
            else
            {
                currentPlaylist.Repeat = false;
                repeat = false;
            }
        }
    }
}
