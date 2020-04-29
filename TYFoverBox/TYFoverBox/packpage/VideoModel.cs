

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using LibVLCSharp.Forms.Shared;
using LibVLCSharp.Shared;

namespace TYFoverBox
{
    public class VideoModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of <see cref="MainViewModel"/> class.
        /// </summary>
        public VideoModel()
        {
        }

        private LibVLC _libVLC;
        /// <summary>
        /// Gets the <see cref="LibVLCSharp.Shared.LibVLC"/> instance.
        /// </summary>
        public LibVLC LibVLC
        {
            get => _libVLC;
            private set => Set(nameof(LibVLC), ref _libVLC, value);
        }

        private MediaPlayer _mediaPlayer;
        /// <summary>
        /// Gets the <see cref="LibVLCSharp.Shared.MediaPlayer"/> instance.
        /// </summary>
        public MediaPlayer MediaPlayer
        {
            get => _mediaPlayer;
            private set => Set(nameof(MediaPlayer), ref _mediaPlayer, value);
        }

        private PlaybackControls _playbackControls;

        public PlaybackControls PlaybackControls
        {
            get => _playbackControls;
            private set => Set(nameof(PlaybackControls), ref _playbackControls, value);
        }


        /// <summary>
        /// Initialize LibVLC and playback when page appears
        /// </summary>
        public void OnAppearing(string url)
        {
            Core.Initialize();

            if (LibVLC is null)
            {
                LibVLC = new LibVLC();

                /*
                 
                var media = new Media(LibVLC,
                    "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4",
                    FromType.FromLocation);

                 */
                //rtsp://111.70.4.36/712f059d-4c9c.sdp
                //https://video2.focusit.com.tw/flv?app=live&stream=0e860b16-c962-4e45-b5a2-5cebf493d371

                //GISCAM 單張與串流網址無效，只能用rtsp
                var media = new Media(LibVLC, url, FromType.FromLocation);



                PlaybackControls = new PlaybackControls { IsAspectRatioButtonVisible = false };

                //https://code.videolan.org/videolan/LibVLCSharp/blob/3.x/LibVLCSharp/Shared/MediaPlayerElement/AspectRatioManager.cs
                if (MediaPlayer is null)
                { //Scale 2 for fit screen
                    MediaPlayer = new MediaPlayer(media) { EnableHardwareDecoding = true, AspectRatio = null, Scale = 0, Fullscreen = true };
                    MediaPlayer.Play();
                }
            }
        }

        public void OnDisappearing()
        {
            if (MediaPlayer.IsPlaying)
            {
                MediaPlayer.Pause();
            }

            LibVLC.Dispose();
        }

        //With Prism, we can get the time, position and Media of the Full screen (OnlyVideoViewModel) MediaPlayer and set the MediaPlayer of this ViewModel, in the OnNavigatedTo method

        private void Set<T>(string propertyName, ref T field, T value)
        {
            if (field == null && value != null || field != null && !field.Equals(value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
