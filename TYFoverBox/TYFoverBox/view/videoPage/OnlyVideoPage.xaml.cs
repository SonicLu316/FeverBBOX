using System;
using System.Collections.Generic;

using Xamarin.Forms;

using LibVLCSharp.Shared;
using Xamarin.Essentials;

using Xamarin.Forms.Xaml;

namespace TYFoverBox.view.videoPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnlyVideoPage : ContentPage
    {
        private readonly OnlyVideoViewModel _viewModel;

        public OnlyVideoPage(MediaPlayer mediaPlayer)
        {
            InitializeComponent();

            _viewModel = BindingContext as OnlyVideoViewModel;
            _viewModel.PreviousMediaPlayer = mediaPlayer;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
            mediaPlayer.PlaybackControls = _viewModel.PlaybackControls;
            DeviceDisplay.MainDisplayInfoChanged += OnMainDisplayInfoChanged;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel.OnDisappearing();
            DeviceDisplay.MainDisplayInfoChanged -= OnMainDisplayInfoChanged;
        }

        void OnMainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (e.DisplayInfo.Orientation == DisplayOrientation.Portrait)
                {
                    await Navigation.PopAsync();
                }
            });
        }
    }
}
