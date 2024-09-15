using System;
using AVFoundation;
using CoreGraphics;
using Foundation;
using Microsoft.Extensions.DependencyInjection;
using UIKit;
using VkNet.Model;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.ShortSongInfoModule;
using Walkman.iOS.Modules;
using Walkman.iOS.Utils;
using Xamarin.Essentials;

namespace Walkman.iOS.Modules.ShortSongInfoModule
{
	public partial class ShortSongInfoView : UIView, IShortSongInfoView
    {
        private IShortSongInfoPresenter _presenter;

        protected ShortSongInfoView(IntPtr handle) : base(handle)
        {            
        }

        public override void AwakeFromNib()
        {
            _presenter = ServiceProviderFactory.ServiceProvider.GetService<IShortSongInfoPresenter>();
            _presenter.ConfigureView(this);
        }

        public void ConfigureView()
        {
            PlayButton.TouchUpInside += PlayButton_TouchUpInside;

            var gesture = new UITapGestureRecognizer();
            gesture.AddTarget(x => _presenter.ShowPlayer());

            SongName.AddGestureRecognizer(gesture);
            Artist.AddGestureRecognizer(gesture);
            NotPerformed.AddGestureRecognizer(gesture);
            AddGestureRecognizer(gesture);
        }

        private void PlayButton_TouchUpInside(object sender, EventArgs e)
        {
            Animate(0.2, () =>
            {
                PlayButton.Transform = CGAffineTransform.MakeScale(0.9f, 0.9f);
            }, () =>
            {
                Animate(0.2, () =>
                {
                    PlayButton.Transform = CGAffineTransform.MakeIdentity();
                });
            });
            _presenter.ChangePlay();
        }

        public void SetColor()
        {
            BackgroundColor = ColorUtils.GetInterfaceStyle(Frame.Width, Frame.Height, Frame.Size);

            ParentView.Layer.CornerRadius = 10;
            Layer.CornerRadius = 10;
            ChildView.Layer.CornerRadius = 10;

            PlayButton.SetImage(UIImage.FromBundle("pause.png").ApplyTintColor(UIColor.FromName("ElementColor")), UIControlState.Normal);
        }

        public void SetNewSong(SongInfo currentSong)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                SongName.Text = currentSong.Name;
                SongName.Hidden = false;

                Artist.Text = currentSong.Artist;
                Artist.Hidden = false;

                NotPerformed.Hidden = true;

                PlayButton.Enabled = true;
            });
        }

        public void Play()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                PlayButton.SetImage(UIImage.FromBundle("play.png").ApplyTintColor(UIColor.FromName("ElementColor")), UIControlState.Normal);
            });
        }

        public void Pause()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                PlayButton.SetImage(UIImage.FromBundle("pause.png").ApplyTintColor(UIColor.FromName("ElementColor")), UIControlState.Normal);
            });
        }
    }
}

