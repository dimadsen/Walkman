using System;
using Foundation;
using UIKit;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;
using Walkman.iOS.Utils;

namespace Walkman.iOS.Views
{
    public partial class PopularTableViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("PopularTableViewCell");
        public static readonly UINib Nib;

        static PopularTableViewCell()
        {
            Nib = UINib.FromName("PopularTableViewCell", NSBundle.MainBundle);
        }

        protected PopularTableViewCell(IntPtr handle) : base(handle) { }

        public PopularTableViewCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId) { }

        public void UpdateCell(СompilationInfo compilation, int row)
        {
            BackgroundColor = ColorUtils.GetInterfaceStyle(Frame.Width, Frame.Height, Frame.Size);

            Genre.Text = compilation.Name;
            Genre.TextColor = UIColor.FromName("ElementColor");

            CollectionView.Compilation = compilation;
            CollectionView.Tag = row;
            CollectionView.ReloadData();
        }

        public void SetNewSong(SongInfo song)
        {
            CollectionView.SetNewSong(song);
        }

        public void SetPlay(SongInfo song)
        {
            CollectionView.SetPlay(song);
        }

        public void SetPause(SongInfo song)
        {
            CollectionView.SetPause(song);
        }

        public void Reload()
        {
            CollectionView.Reload();
        }
    }
}
