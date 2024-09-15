using System;
using Foundation;
using UIKit;
using Walkman.Core.Interfaces.Models;
using Walkman.iOS.Utils;

namespace Walkman.iOS
{
    public partial class PopularCollectionViewCell : UICollectionViewCell
    {
        public static readonly NSString Key = new NSString("PopularCollectionViewCell");
        public static readonly UINib Nib;

        public PopularCollectionViewCell(IntPtr handle) : base(handle) { }

        static PopularCollectionViewCell()
        {
            Nib = UINib.FromName("PopularCollectionViewCell", NSBundle.MainBundle);
        }

        public void UpdateCell(SongInfo song, int row)
        {
            var albumCoverExists = song.AlbumId.HasValue && ImageUtils.FileExists(song.AlbumId.Value);

            AlbumCover.Image = albumCoverExists ?
                UIImage.LoadFromData(ImageUtils.GetFileFromCache(song.AlbumId.Value)) :
                UIImage.FromFile("cd_player_min");

            AlbumCover.Layer.CornerRadius = 5;

            Song.Text = song.Name;
            Song.TextColor = UIColor.FromName("BackgroundColor");

            Artist.Text = song.Artist;
            Artist.TextColor = UIColor.FromName("BackgroundColor");

            Tag = row;
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
