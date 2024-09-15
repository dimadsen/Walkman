using System;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using Microsoft.Extensions.DependencyInjection;
using UIKit;
using Walkman.Core.Interfaces.FavoriteSongModule;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;
using Walkman.iOS.Utils;
using Walkman.iOS.Views;
using Xamarin.Essentials;

namespace Walkman.iOS.Modules.FavoriteSongModule
{
    public partial class FavoriteTableView : UITableView, IFavoriteSongView
    {
        private IFavoriteSongPresenter _presenter;

        private UIActivityIndicatorView _indicator;

        public FavoriteTableView(IntPtr handle) : base(handle)
        {
            _presenter = ServiceProviderFactory.ServiceProvider.GetService<IFavoriteSongPresenter>();

            _presenter.ConfigureView(this);

            Task.Run(async () => await _presenter.SearchSongsAsync());
        }

        public void ViewWillAppear()
        {
            var currentSong = _presenter.Songs?.FirstOrDefault(x => x.Id == _presenter.GetCurrentSong()?.Id);

            if (currentSong == null && IndexPathForSelectedRow != null)
            {
                DeselectRow(IndexPathForSelectedRow, true);
            }
            else if (currentSong != null)
            {
                var index = _presenter.Songs.IndexOf(currentSong);

                var indexPath = NSIndexPath.FromRowSection(index, 0);

                SelectRow(indexPath, true, UITableViewScrollPosition.Middle);

                var cell = CellAt(indexPath) as SongTableViewCell;

                SetAnimation(cell, indexPath);
            }
        }

        public void ConfigureView()
        {
            RegisterNibForCellReuse(SongTableViewCell.Nib, nameof(SongTableViewCell));

            BackgroundColor = ColorUtils.GetInterfaceStyle(Frame.Width, Frame.Height, Frame.Size);

            SeparatorStyle = UITableViewCellSeparatorStyle.None;
            TableFooterView = new UIView(new CGRect(0, Frame.GetMaxY(), Frame.Width, 100));

            _indicator = TableHeaderView as UIActivityIndicatorView;

            Delegate = new FavoriteTableViewDelegate(_presenter);

            DataSource = new FavoriteTableViewDataSource(_presenter);
        }

        public void SetNewSong(SongInfo songInfo)
        {
            VisibleCells.OfType<SongTableViewCell>().ToList().ForEach(x => x.HideAnimation());

            var currentSong = _presenter.Songs.FirstOrDefault(x => x.Id == songInfo.Id);

            var index = _presenter.Songs.IndexOf(currentSong);
            var indexPath = NSIndexPath.FromRowSection(index, 0);

            SelectRow(indexPath, true, UITableViewScrollPosition.None);

            var cell = CellAt(indexPath) as SongTableViewCell;

            SetAnimation(cell, indexPath);
        }

        public void SetSongs()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _indicator.StopAnimating();

                TableHeaderView = null;

                ReloadData();

                if (_presenter.Songs.Any())
                {
                    var warningLabel = Subviews.FirstOrDefault(x => x.Tag == 1);
                    warningLabel?.RemoveFromSuperview();
                }
            });
        }

        public void SetWarningView(string message)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var label = new UILabel(new CGRect(0, 0, Frame.Width, 50))
                {
                    Text = message,
                    Tag = 1,
                    TextAlignment = UITextAlignment.Center,
                    TextColor = UIColor.FromName("BackgroundColor")
                };

                label.Center = new CGPoint(Center.X, label.Center.Y + 50);

                AddSubview(label);

                _indicator.StopAnimating();
            });
        }

        public void UpdateView(SongInfo songInfo)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var currentSong = _presenter.Songs?.FirstOrDefault(x => x.Id == songInfo.Id);

                if (currentSong == null)
                {
                    _presenter.Songs.Insert(0, songInfo);

                    var index = _presenter.Songs.FindIndex(x => x.Id == songInfo.Id);
                    var indexPath = NSIndexPath.FromRowSection(index, 0);

                    InsertRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
                    EndUpdates();

                    SelectRow(indexPath, true, UITableViewScrollPosition.None);

                    var warningLabel = Subviews.FirstOrDefault(x => x.Tag == 1);
                    warningLabel?.RemoveFromSuperview();
                }
                else
                {
                    var index = _presenter.Songs.FindIndex(x => x.Id == songInfo.Id);
                    _presenter.Songs.RemoveAt(index);

                    DeleteRows(new NSIndexPath[] { NSIndexPath.FromRowSection(index, 0) }, UITableViewRowAnimation.Left);
                    EndUpdates();

                    _presenter.SetWarningView();
                }

                TableHeaderView = null;
            });
        }

        private void SetAnimation(SongTableViewCell cell, NSIndexPath indexPath)
        {
            var currentSong = _presenter.Songs.FirstOrDefault(x => x.Id == _presenter.GetCurrentSong()?.Id);

            if (_presenter.Songs.IndexOf(currentSong) == indexPath.Row)
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
            if (IndexPathForSelectedRow != null)
            {
                var cell = CellAt(IndexPathForSelectedRow) as SongTableViewCell;

                cell?.ContinueAnimation();
            }
        }

        public void SetPause(SongInfo song)
        {
            if (IndexPathForSelectedRow != null)
            {
                var cell = CellAt(IndexPathForSelectedRow) as SongTableViewCell;

                cell?.PauseAnimation();
            }
        }

        public void SelectSong()
        {
            var currentSong = _presenter.Songs.FirstOrDefault(x => x.Id == _presenter.GetCurrentSong()?.Id);
            var index = _presenter.Songs.IndexOf(currentSong);

            SelectRow(NSIndexPath.FromRowSection(index, 0), true, UITableViewScrollPosition.None);
        }
    }
}
