using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Walkman.Core.Interfaces.Models;
using Walkman.iOS.Utils;

namespace Walkman.iOS.Views
{
    public partial class SongTableViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("SongTableViewCell");
        public static readonly UINib Nib;

        private nfloat _height;

        static SongTableViewCell()
        {
            Nib = UINib.FromName("SongTableViewCell", NSBundle.MainBundle);
        }

        protected SongTableViewCell(IntPtr handle) : base(handle) { }

        public SongTableViewCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId) { }

        public void UpdateCell(SongInfo song)
        {
            if (_height == 0 && Frame.Height != 0) // Потому что продадает значение высоты последней ячейки после удаления
                _height = Frame.Height;

            BackgroundColor = ColorUtils.GetInterfaceStyle(Frame.Width, _height, new CGSize(Frame.Width, _height));

            SelectedBackgroundView = new UIView()
            {
                BackgroundColor = UIColor.SystemFill,
                Frame = new CGRect(5, Frame.Width, Frame.Width - 10, Frame.Height)
            };

            SelectedBackgroundView.Layer.CornerRadius = 10;

            var albumCoverExists = song.AlbumId.HasValue && ImageUtils.FileExists(song.AlbumId.Value);

            AlbumCover.Image = albumCoverExists ?
                UIImage.LoadFromData(ImageUtils.GetFileFromCache(song.AlbumId.Value)) :
                UIImage.FromFile("cd_player_min");

            AlbumCover.Layer.CornerRadius = 5;
            Song.Text = song.Name;
            Song.TextColor = UIColor.FromName("BackgroundColor");

            Artist.Text = song.Artist;
            Artist.TextColor = UIColor.FromName("BackgroundColor");

            Time.Text = TimeSpan.FromSeconds(song.Duration).ToString(@"mm\:ss");
            Time.TextColor = UIColor.FromName("BackgroundColor");
        }        

        public void ShowAnimation()
        {
            AnimationView.Hidden = false;
        }

        public void HideAnimation()
        {
            AnimationView.Hidden = true;
        }

        public void PauseAnimation()
        {
            AnimationView?.Pause();
        }

        public void ContinueAnimation()
        {
            AnimationView?.Start();
        }
    }
}
