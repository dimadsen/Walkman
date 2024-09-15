using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using Microsoft.Extensions.DependencyInjection;
using UIKit;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.RecentSongModule;
using Walkman.Core.Models;
using Walkman.iOS.Utils;
using Walkman.iOS.Views;
using Xamarin.Essentials;

namespace Walkman.iOS.Modules.RecentSongModule
{
    public partial class RecentSongViewController : UITableViewController, IRecentSongView
    {
        private IRecentSongPresenter _presenter;

        private List<SongInfo> _songs = new List<SongInfo>();

        public RecentSongViewController(IntPtr handle) : base(handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _presenter = ServiceProviderFactory.ServiceProvider.GetService<IRecentSongPresenter>();

            _presenter.ConfigureView(this);

            Task.Run(async () => await _presenter.SearchSongsAsync());
        }

        #region Override
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(SongTableViewCell.Key) as SongTableViewCell;
            cell.Frame = new CGRect(cell.Frame.X, cell.Frame.Y, tableView.Frame.Width, cell.Frame.Height);

            cell.UpdateCell(_songs[indexPath.Row]);

            SetAnimation(cell, indexPath);

            return cell;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return _songs?.Count ?? 0;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            _presenter.PlaySong(_songs, indexPath.Row);
        }

        public override void ViewDidAppear(bool animated)
        {
            var currentSong = _songs?.FirstOrDefault(x => x.Id == _presenter.GetCurrentSong()?.Id);

            if (currentSong == null && TableView.IndexPathForSelectedRow != null)
            {
                TableView.DeselectRow(TableView.IndexPathForSelectedRow, true);
            }
            else if (currentSong != null)
            {
                var index = _songs.IndexOf(currentSong);

                var indexPath = NSIndexPath.FromRowSection(index, 0);

                TableView.SelectRow(indexPath, true, UITableViewScrollPosition.Middle);

                var cell = TableView.CellAt(indexPath) as SongTableViewCell;

                SetAnimation(cell, indexPath);
            }
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            if (editingStyle == UITableViewCellEditingStyle.Delete)
            {
                var song = _songs.ElementAt(indexPath.Row);

                _songs.Remove(song);

                Task.Run(async () => await _presenter.DeleteSongAsync(song));

                tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);

                if (!_songs.Any())
                    _presenter.SetWarningView();
            }
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            var currentSong = _songs.FirstOrDefault(x => x.Id == _presenter.GetCurrentSong()?.Id);
            var index = _songs.IndexOf(currentSong);

            TableView.SelectRow(NSIndexPath.FromRowSection(index, 0), true, UITableViewScrollPosition.None);

            return true;
        }

        #endregion

        public void ConfigureView()
        {
            TableView.RegisterNibForCellReuse(SongTableViewCell.Nib, nameof(SongTableViewCell));

            Indicator.StartAnimating();

            TableView.BackgroundColor = ColorUtils.GetInterfaceStyle(TableView.Frame.Width, TableView.Frame.Height, TableView.Frame.Size);

            TableView.Delegate = this;
            TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            TableView.TableFooterView = new UIView(new CGRect(0, TableView.Frame.GetMaxY(), TableView.Frame.Width, 55));

            NavigationController.NavigationBar.BarTintColor = ColorUtils.GetInterfaceStyle(TableView.Frame.Width, TableView.Frame.Height, TableView.Frame.Size);
        }

        public void SetNewSong(SongInfo songInfo)
        {
            TableView.VisibleCells.OfType<SongTableViewCell>().ToList().ForEach(x => x.HideAnimation());

            var currentSong = _songs.FirstOrDefault(x => x.Id == songInfo.Id);

            var index = _songs.IndexOf(currentSong);
            var indexPath = NSIndexPath.FromRowSection(index, 0);

            TableView.SelectRow(indexPath, true, UITableViewScrollPosition.None);

            var cell = TableView.CellAt(indexPath) as SongTableViewCell;
            
            SetAnimation(cell, indexPath);
        }

        public void SetSongs(List<SongInfo> songs)
        {
            _songs = songs;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                TableView.ReloadData();
                Indicator.StopAnimating();

                TableView.TableHeaderView = null;
                TableView.DataSource = this;

                if (songs.Any())
                {
                    var warningLabel = TableView.Subviews.FirstOrDefault(x => x.Tag == 1);
                    warningLabel?.RemoveFromSuperview();
                }
            });
        }

        public void SetWarningView(string message)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var label = new UILabel(new CGRect(0, 0, TableView.Frame.Width, 50))
                {
                    Text = message,
                    Tag = 1,
                    TextAlignment = UITextAlignment.Center,
                    TextColor = UIColor.FromName("BackgroundColor")
                };

                label.Center = new CGPoint(TableView.Center.X, TableView.Center.Y - NavigationController.NavigationBar.Frame.Height);

                TableView.AddSubview(label);

                Indicator.StopAnimating();
            });
        }

        public void InsertSong(SongInfo songInfo)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var insertSong = _songs?.FirstOrDefault(x => x.Id == songInfo.Id);

                if (insertSong == null)
                {
                    _songs.Insert(0, songInfo);

                    var index = _songs.IndexOf(songInfo);

                    TableView.InsertRows(new NSIndexPath[] { NSIndexPath.FromRowSection(index, 0) }, UITableViewRowAnimation.Automatic);

                    var warningLabel = TableView.Subviews.FirstOrDefault(x => x.Tag == 1);
                    warningLabel?.RemoveFromSuperview();
                }
                else
                {
                    var index = _songs.IndexOf(insertSong);
                    _songs.RemoveAt(index);
                    _songs.Insert(0, insertSong);

                    TableView.MoveRow(NSIndexPath.FromRowSection(index, 0), NSIndexPath.FromRowSection(0, 0));
                }

                TableView.TableHeaderView = null;
                TableView.DataSource = this;
            });
        }

        private void SetAnimation(SongTableViewCell cell, NSIndexPath indexPath)
        {
            var currentSong = _songs.FirstOrDefault(x => x.Id == _presenter.GetCurrentSong()?.Id);

            if (_songs.IndexOf(currentSong) == indexPath.Row)
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
            if (TableView.IndexPathForSelectedRow != null)
            {
                var cell = TableView.CellAt(TableView.IndexPathForSelectedRow) as SongTableViewCell;

                cell?.ContinueAnimation();
            }
        }

        public void SetPause(SongInfo song)
        {
            if (TableView.IndexPathForSelectedRow != null)
            {
                var cell = TableView.CellAt(TableView.IndexPathForSelectedRow) as SongTableViewCell;

                cell?.PauseAnimation();
            }
        }
    }
}


