using System;
using System.Linq;
using Foundation;
using Microsoft.Extensions.DependencyInjection;
using UIKit;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.PopularModule;
using Walkman.Core.Models;

namespace Walkman.iOS
{
    public partial class PopularCollectionView : UICollectionView, IUICollectionViewDataSource, IUICollectionViewDelegate
    {
        public СompilationInfo Compilation;
        private readonly IPopularPresenter _presenter;

        public PopularCollectionView(IntPtr handle) : base(handle)
        {
            DataSource = this;
            Delegate = this;

            _presenter = ServiceProviderFactory.ServiceProvider.GetService<IPopularPresenter>();
        }

        public nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return Compilation?.Songs.Count ?? 0;
        }

        public UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = collectionView.DequeueReusableCell("PopularCollectionViewCell", indexPath) as PopularCollectionViewCell;

            BackgroundColor = UIColor.Clear;

            cell.UpdateCell(Compilation.Songs[indexPath.Row], indexPath.Row);

            SetAnimation(cell, indexPath);

            var gesture = new UITapGestureRecognizer();

            gesture.AddTarget(x =>
            {
                _presenter.PlaySong(Compilation.Songs, (int)cell.Tag);
            });

            cell.AddGestureRecognizer(gesture);

            return cell;
        }

        public void SetNewSong(SongInfo songInfo)
        {
            var currentSong = Compilation.Songs.FirstOrDefault(x => x.Id == songInfo.Id);

            var index = Compilation.Songs.IndexOf(currentSong);
            var indexPath = NSIndexPath.FromRowSection(index, 0);

            var cell = DequeueReusableCell("PopularCollectionViewCell", indexPath) as PopularCollectionViewCell;

            SetAnimation(cell, indexPath);

            ReloadItems(new NSIndexPath[] { indexPath });
        }

        private void SetAnimation(PopularCollectionViewCell cell, NSIndexPath indexPath)
        {
            var currentSong = Compilation.Songs.FirstOrDefault(x => x.Id == _presenter.GetCurrentSong()?.Id);

            if (Compilation.Songs.IndexOf(currentSong) == indexPath.Row)
            {
                cell?.ShowAnimation();

                var status = _presenter.GetPlayerStatus();

                if (status != PlayerStatus.Paused)
                    cell?.ContinueAnimation();
                else
                    cell?.PauseAnimation();
            }
            else
                cell?.HideAnimation();
        }

        public void SetPlay(SongInfo song)
        {
            var currentSong = Compilation.Songs.FirstOrDefault(x => x.Id == song.Id);

            var index = Compilation.Songs.IndexOf(currentSong);
            var indexPath = NSIndexPath.FromRowSection(index, 0);

            var cell = DequeueReusableCell("PopularCollectionViewCell", indexPath) as PopularCollectionViewCell;

            cell?.ShowAnimation();

            ReloadItems(new NSIndexPath[] { indexPath });
        }

        public void SetPause(SongInfo song)
        {
            var currentSong = Compilation.Songs.FirstOrDefault(x => x.Id == song.Id);

            var index = Compilation.Songs.IndexOf(currentSong);
            var indexPath = NSIndexPath.FromRowSection(index, 0);

            var cell = DequeueReusableCell("PopularCollectionViewCell", indexPath) as PopularCollectionViewCell;

            cell?.HideAnimation();

            ReloadItems(new NSIndexPath[] { indexPath });
        }

        public void Reload()
        {
            ReloadData();
        }
    }
}