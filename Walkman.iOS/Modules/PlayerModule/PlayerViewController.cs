using System;
using System.Linq;
using System.Threading;
using AVFoundation;
using CoreGraphics;
using Foundation;
using MediaPlayer;
using Microsoft.Extensions.DependencyInjection;
using UIKit;
using VkNet.Model;
using Walkman.Core.Interfaces.PlayerModule;
using Walkman.Core.Interfaces.Models;
using Walkman.iOS.Modules;
using Walkman.iOS.Modules.PlayerModule;
using Walkman.iOS.Utils;
using Xamarin.Essentials;
using System.Threading.Tasks;
using Walkman.Core.Models;
using XLPagerTabStrip;

namespace Walkman.iOS.Modules.PlayerModule
{
    public partial class PlayerViewController : UIViewController, IPlayerView, IIndicatorInfoProvider
    {
        private IPlayerPresenter _presenter;
        private SongInfo _currentSong;

        private UIActivityIndicatorView _downloadIndicator;

        public PlayerViewController(IntPtr handle) : base(handle)
        {
            _presenter = ServiceProviderFactory.ServiceProvider.GetService<IPlayerPresenter>();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _presenter.ConfigureView(this);
        }

        public void ConfigureView()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.BeginReceivingRemoteControlEvents();
                PlayButton.TouchUpInside += ChangePlayPause;

                SongProgress.Continuous = true;
                SongProgress.TouchDragInside += SetPlaybackPosition;
                SongProgress.TouchUpInside += ChangePlaybackPosition;

                BackButton.TouchUpInside += BackButton_TouchUpInside;
                ForwardButton.TouchUpInside += ForwardButton_TouchUpInside;

                DownloadButton.TouchUpInside += DownloadButton_TouchUpInside;

                BackButton.SetImage(UIImage.FromBundle("rewind-back.png").ApplyTintColor(UIColor.FromName("ElementColor")), UIControlState.Normal);
                ForwardButton.SetImage(UIImage.FromBundle("rewind.png").ApplyTintColor(UIColor.FromName("ElementColor")), UIControlState.Normal);

                WalkmanImage.TintColor = (UIColor.FromName("ElementColor"));
                WalkmanImage.Image = WalkmanImage.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);

                AlbumCover.Layer.CornerRadius = 10;
                InfoView.Layer.CornerRadius = 10;

                TrackIcon.TintColor = (UIColor.FromName("BackgroundColor"));
                TrackIcon.Image = TrackIcon.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);

                ArtistIcon.TintColor = (UIColor.FromName("BackgroundColor"));
                ArtistIcon.Image = ArtistIcon.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);

                AlbumIcon.TintColor = (UIColor.FromName("BackgroundColor"));
                AlbumIcon.Image = AlbumIcon.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);

                MainView.BackgroundColor = UIColor.Clear;
                AlbumView.BackgroundColor = UIColor.Clear;
                PlayView.BackgroundColor = UIColor.Clear;
                View.BackgroundColor = UIColor.Clear;

                FavoriteButton.TouchUpInside += (sender, e) => SetFavorite();

                _downloadIndicator = new UIActivityIndicatorView()
                {
                    HidesWhenStopped = true,
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    Color = UIColor.FromName("ElementColor")
                };

                PlayView.AddSubview(_downloadIndicator);

                var xConstraint = NSLayoutConstraint.Create(DownloadButton, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, _downloadIndicator, NSLayoutAttribute.CenterX, 1, 0);
                PlayView.AddConstraint(xConstraint);

                var yConstraint = NSLayoutConstraint.Create(DownloadButton, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, _downloadIndicator, NSLayoutAttribute.CenterY, 1, 0);
                PlayView.AddConstraint(yConstraint);


                //Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            });
        }

        private void ChangePlayPause(object sender, EventArgs e)
        {
            UIView.Animate(0.2, () =>
            {
                PlayButton.Transform = CGAffineTransform.MakeScale(0.9f, 0.9f);
            }, () =>
            {
                UIView.Animate(0.2, () =>
                {
                    PlayButton.Transform = CGAffineTransform.MakeIdentity();
                });
            });
            _presenter.ChangePlayPause();
        }

        private void DownloadButton_TouchUpInside(object sender, EventArgs e)
        {
            DownloadButton.Hidden = true;
            _downloadIndicator.StartAnimating();

            _presenter.DownloadSong(_currentSong);

        }

        private void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            UIView.Animate(0.2, () =>
            {
                BackButton.Transform = CGAffineTransform.MakeScale(0.9f, 0.9f);
            }, () =>
            {
                UIView.Animate(0.2, () =>
                {
                    BackButton.Transform = CGAffineTransform.MakeIdentity();
                });
            });
            _presenter.ChangePreviousSong();
        }

        private void ForwardButton_TouchUpInside(object sender, EventArgs e)
        {
            UIView.Animate(0.2, () =>
            {
                ForwardButton.Transform = CGAffineTransform.MakeScale(0.9f, 0.9f);
            }, () =>
            {
                UIView.Animate(0.2, () =>
                {
                    ForwardButton.Transform = CGAffineTransform.MakeIdentity();
                });
            });
            _presenter.ChangeNextSong();
        }

        private void ChangePlaybackPosition(object sender, EventArgs e)
        {
            var currentTime = ((UISlider)sender).Value;
            _presenter.ChangePlaybackPosition(currentTime);
        }

        private void SetPlaybackPosition(object sender, EventArgs e)
        {
            var currentTime = ((UISlider)sender).Value;
            _presenter.SetPlaybackPosition(currentTime, false);
        }

        public void SetCurrentSong(PlayerModel playerModel)
        {
            _currentSong = playerModel.CurrentSong;

            if (_currentSong.DownloadStatus == DownloadStatus.Processing)
            {
                _downloadIndicator.StartAnimating();
                DownloadButton.Hidden = true;
            }
            else if (_currentSong.DownloadStatus == DownloadStatus.NotStarted)
            {
                DownloadButton.SetImage(UIImage.GetSystemImage("square.and.arrow.down").ApplyTintColor(UIColor.FromName("ElementColor")), UIControlState.Normal);

                DownloadButton.Hidden = false;
                _downloadIndicator?.StopAnimating();
                DownloadButton.Enabled = true;
            }
            else
            {
                DownloadButton.SetImage(UIImage.GetSystemImage("square.and.arrow.down.fill").ApplyTintColor(UIColor.FromName("ElementColor")), UIControlState.Normal);

                DownloadButton.Hidden = false;
                _downloadIndicator?.StopAnimating();
                DownloadButton.Enabled = false;
            }

            Song.Text = _currentSong.Name;
            Artist.Text = _currentSong.Artist;
            Album.Text = _currentSong.Album;

            TimePassed.Text = TimeSpan.FromSeconds(playerModel.CurrentTime).ToString(@"mm\:ss");
            TimeLeft.Text = $"-{TimeSpan.FromSeconds(playerModel.CurrentTime - _currentSong.Duration).ToString(@"mm\:ss")}";

            SongProgress.MaxValue = (float)_currentSong.Duration;

            SongProgress.Value = (float)Math.Round(playerModel.CurrentTime);
            SongStatistics.Text = playerModel.SongStatistics;

            if (playerModel.PlayerStatus == PlayerStatus.Paused)
            {
                PlayButton.SetImage(UIImage.FromBundle("pause.png").ApplyTintColor(UIColor.FromName("ElementColor")), UIControlState.Normal);
                UIView.Animate(0.5, () =>
                {
                    AlbumImageConstraint.Constant = -10;
                    AlbumView.LayoutIfNeeded();
                }, null);
            }
            else
            {
                PlayButton.SetImage(UIImage.FromBundle("play.png").ApplyTintColor(UIColor.FromName("ElementColor")), UIControlState.Normal);
                UIView.Animate(0.5, () =>
                {
                    AlbumImageConstraint.Constant = 25;
                    AlbumView.LayoutIfNeeded();
                }, null);
            }

            var transition = playerModel.Move == MoveSong.Back ? UIViewAnimationOptions.TransitionCurlDown : UIViewAnimationOptions.TransitionCurlUp;
            UIView.Transition(AlbumCover, 0.5, transition, () =>
            {
                var albumCoverExists = _currentSong.AlbumId.HasValue && ImageUtils.FileExists(_currentSong.AlbumId.Value);
                AlbumCover.Image = albumCoverExists ? UIImage.LoadFromData(ImageUtils.GetFileFromCache(_currentSong.AlbumId.Value)) : UIImage.FromFile("cd_player");

            }, null);

            var favoriteImage = _currentSong.IsFavorite ? UIImage.GetSystemImage("heart.fill") : UIImage.GetSystemImage("heart");
            FavoriteButton.SetImage(favoriteImage.ApplyTintColor(UIColor.FromName("ElementColor")), UIControlState.Normal);

            ForwardButton.Enabled = !playerModel.IsLastSong;
            BackButton.Enabled = !playerModel.IsFirstSong;

        }

        private void ChangeCurrentTimeValue(double currentTime, double duration)
        {
            TimePassed.Text = TimeSpan.FromSeconds(Math.Round(currentTime)).ToString(@"mm\:ss");
            TimeLeft.Text = $"-{TimeSpan.FromSeconds(Math.Round(currentTime) - Math.Round(duration)).ToString(@"mm\:ss")}";
        }

        private void SetFavorite()
        {
            _currentSong.IsFavorite = !_currentSong.IsFavorite;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                UIView.Animate(0.1, () =>
                {
                    FavoriteButton.Transform = CGAffineTransform.MakeScale(0.9f, 0.9f);

                    _presenter.SetFavorite(_currentSong);

                    var image = _currentSong.IsFavorite ? UIImage.GetSystemImage("heart.fill") : UIImage.GetSystemImage("heart");

                    FavoriteButton.SetImage(image.ApplyTintColor(UIColor.FromName("ElementColor")), UIControlState.Normal);
                }, () =>
                {
                    UIView.Animate(0.1, () => FavoriteButton.Transform = CGAffineTransform.MakeIdentity());
                });
            });
        }

        public void SetPlaybackPosition(double value)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                SongProgress.Value = (float)value;
                ChangeCurrentTimeValue(value, _currentSong.Duration);
            });
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    PlayButton.SetImage(UIImage.FromBundle("pause.png").ApplyTintColor(UIColor.FromName("ElementColor")), UIControlState.Normal);
                    PlayButton.Enabled = false;
                    BackButton.Enabled = false;
                    ForwardButton.Enabled = false;

                    var errorView = new UIView(new CGRect(0, PlayView.Frame.Y + WalkmanImage.Frame.Y, View.Frame.Width, WalkmanImage.Frame.Height))
                    {
                        BackgroundColor = UIColor.SystemRed,
                        Tag = 2,
                    };

                    var errorMessage = new UILabel(errorView.Frame)
                    {
                        Text = "Нет соединения с интернетом 😢",
                        TextAlignment = UITextAlignment.Center,
                        TextColor = UIColor.White,
                        Tag = 3
                    };

                    ModalInPresentation = true;

                    View.AddSubview(errorView);
                    View.AddSubview(errorMessage);
                });
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    PlayButton.SetImage(UIImage.FromBundle("play.png").ApplyTintColor(UIColor.FromName("ElementColor")), UIControlState.Normal);
                    PlayButton.Enabled = true;
                    BackButton.Enabled = true;
                    ForwardButton.Enabled = true;

                    ModalInPresentation = false;

                    var errorView = View.Subviews.FirstOrDefault(x => x.Tag == 2);
                    errorView?.RemoveFromSuperview();

                    var errorMessage = View.Subviews.FirstOrDefault(x => x.Tag == 3);
                    errorMessage?.RemoveFromSuperview();
                });
            }

            _presenter.ChangePlayPause();
        }

        public void SetPlay()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                PlayButton.SetImage(UIImage.FromBundle("play.png").ApplyTintColor(UIColor.FromName("ElementColor")), UIControlState.Normal);
                UIView.Animate(0.5, () =>
                {
                    AlbumImageConstraint.Constant = 25;
                    AlbumView.LayoutIfNeeded();
                });
            });
        }

        public void SetPause()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                PlayButton.SetImage(UIImage.FromBundle("pause.png").ApplyTintColor(UIColor.FromName("ElementColor")), UIControlState.Normal);
                UIView.Animate(0.5, () =>
                {
                    AlbumImageConstraint.Constant = -10;
                    AlbumView.LayoutIfNeeded();
                });
            });
        }

        public IndicatorInfo IndicatorInfoForPagerTabStrip(PagerTabStripViewController pagerTabStripController)
        {
            var image = UIImage.GetSystemImage("play.circle").ApplyTintColor(UIColor.FromName("ElementColor"));

            return new IndicatorInfo("", image, this);
        }

        public void SetDownloadSucsessful(SongInfo songInfo)
        {
            if (_currentSong.Id == songInfo.Id)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var config = UIImageSymbolConfiguration.Create(new UIColor[] { UIColor.White, UIColor.SystemGreen });
                    var image = UIImage.GetSystemImage("checkmark.circle.fill");
                    image = image.ApplyConfiguration(config);

                    UIView.Transition(DownloadButton, 1, UIViewAnimationOptions.TransitionFlipFromLeft, () =>
                    {

                        DownloadButton.SetImage(image, UIControlState.Normal);

                        DownloadButton.Enabled = false;
                        _downloadIndicator.StopAnimating();
                        DownloadButton.Hidden = false;
                    },null);
                });

                Thread.Sleep(5000);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    UIView.Transition(DownloadButton, 1, UIViewAnimationOptions.TransitionFlipFromRight, () =>
                    {
                        DownloadButton.SetImage(UIImage.GetSystemImage("square.and.arrow.down.fill"), UIControlState.Normal);
                    }, null);
                });
            }
        }

        public void SetDownloadError(SongInfo songInfo)
        {
            if (_currentSong.Id == songInfo.Id)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var config = UIImageSymbolConfiguration.Create(new UIColor[] { UIColor.White, UIColor.SystemRed });
                    var image = UIImage.GetSystemImage("xmark.circle.fill");
                    image = image.ApplyConfiguration(config);

                    UIView.Transition(DownloadButton, 1, UIViewAnimationOptions.TransitionFlipFromLeft, () =>
                    {

                        DownloadButton.SetImage(image, UIControlState.Normal);

                        DownloadButton.Enabled = true;
                        _downloadIndicator.StopAnimating();
                        DownloadButton.Hidden = false;
                    }, null);
                });

                Thread.Sleep(5000);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    UIView.Transition(DownloadButton, 1, UIViewAnimationOptions.TransitionFlipFromRight, () =>
                    {
                        DownloadButton.SetImage(UIImage.GetSystemImage("square.and.arrow.down"), UIControlState.Normal);
                    }, null);
                });
            }
        }
    }
}


