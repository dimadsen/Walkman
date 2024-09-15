using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using Microsoft.Extensions.DependencyInjection;
using ObjCRuntime;
using UIKit;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.PlayerModule;
using Walkman.Core.Interfaces.PopularGenreModule;
using Walkman.Core.Models;
using Walkman.iOS.Modules;
using Walkman.iOS.Utils;
using Walkman.iOS.Views;
using Xamarin.Essentials;

namespace Walkman.iOS.Modules.PopularGenreModule
{
    public partial class PopularGenreViewController : UITableViewController, IPopularGenreView
    {
        private IPopularGenrePresenter _presenter;
        private СompilationInfo _сompilation;

        private bool _maxCount;

        public PopularGenreViewController(IntPtr handle) : base(handle)
        {
            _presenter = ServiceProviderFactory.ServiceProvider.GetService<IPopularGenrePresenter>();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _presenter.ConfigureView(this);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(SongTableViewCell.Key) as SongTableViewCell;
            cell.Frame = new CGRect(cell.Frame.X, cell.Frame.Y, tableView.Frame.Width, cell.Frame.Height);

            cell.UpdateCell(_сompilation.Songs[indexPath.Row]);

            SetAnimation(cell, indexPath);

            return cell;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return _сompilation.Songs.Count;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            _presenter.PlaySong(_сompilation.Songs, indexPath.Row);
        }

        public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            Task.Run(async () =>
            {
                var lastCell = _сompilation.Songs.Count - 3;

                if (lastCell == indexPath.Row && !_maxCount)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        var indicator = (TableView.TableFooterView?.Subviews.FirstOrDefault(x => x is UIActivityIndicatorView)) as UIActivityIndicatorView;
                        indicator?.StartAnimating();
                    });

                    await _presenter.SetPopularSongsAsync();
                }
            });
        }

        public override void ViewDidAppear(bool animated)
        {
            var currentSong = _сompilation.Songs?.FirstOrDefault(x => x.Id == _presenter.GetCurrentSong()?.Id);

            if (currentSong == null && TableView.IndexPathForSelectedRow != null)
            {
                TableView.DeselectRow(TableView.IndexPathForSelectedRow, true);
            }
            else if (currentSong != null)
            {
                var index = _сompilation.Songs.IndexOf(currentSong);

                var indexPath = NSIndexPath.FromRowSection(index, 0);

                TableView.SelectRow(indexPath, true, UITableViewScrollPosition.Middle);

                var cell = TableView.CellAt(indexPath) as SongTableViewCell;

                SetAnimation(cell, indexPath);
            }
        }

        public void ConfigureView(СompilationInfo сompilation)
        {
            _сompilation = сompilation;

            NavigationItem.Title = _сompilation.Name;

            TableView.RegisterNibForCellReuse(SongTableViewCell.Nib, nameof(SongTableViewCell));
            TableView.BackgroundColor = ColorUtils.GetInterfaceStyle(TableView.Frame.Width, TableView.Frame.Height, TableView.Frame.Size);

            TableView.Delegate = this;
            TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            TableView.TableFooterView = new UIView(new CGRect(0, TableView.Frame.GetMaxY(), TableView.Frame.Width, 55));

            var indicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Medium)
            {
                Color = UIColor.FromName("ElementColor"),
                HidesWhenStopped = true,
            };

            indicator.Frame = new CGRect(indicator.Frame.X, indicator.Frame.Y, Indicator.Frame.Width, Indicator.Frame.Height);
            indicator.StopAnimating();

            TableView.TableFooterView.AddSubview(indicator);

            var bar = (UISearchBar)NavigationController.NavigationBar.Items.First().TitleView;
        }

        public void SetSongs(List<SongInfo> songs)
        {
            if (_presenter.Page == 0)
            {
                _сompilation.Songs = songs;

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    TableView.ReloadData();
                    Indicator.StopAnimating();

                    TableView.TableHeaderView = null;

                    var indicator = (TableView.TableFooterView?.Subviews.FirstOrDefault(x => x is UIActivityIndicatorView)) as UIActivityIndicatorView;
                    indicator?.StopAnimating();

                    TableView.DataSource = this;
                });
            }
            else
            {
                _сompilation.Songs.AddRange(songs);

                var indices = songs.Select(x =>
                {
                    var index = _сompilation.Songs.IndexOf(x);
                    return NSIndexPath.FromRowSection(index, 0);
                }).ToArray();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    TableView.InsertRows(indices, UITableViewRowAnimation.Fade);
                    TableView.EndUpdates();

                    var indicator = (TableView.TableFooterView?.Subviews.FirstOrDefault(x => x is UIActivityIndicatorView)) as UIActivityIndicatorView;
                    indicator?.StopAnimating();
                });
            }

            if (!songs.Any())
                _maxCount = true;
        }

        public void SetNewSong(SongInfo songInfo)
        {
            TableView.VisibleCells.OfType<SongTableViewCell>().ToList().ForEach(x => x.HideAnimation());

            var currentSong = _сompilation.Songs.FirstOrDefault(x => x.Id == songInfo.Id);

            var index = _сompilation.Songs.IndexOf(currentSong);
            var indexPath = NSIndexPath.FromRowSection(index, 0);

            TableView.SelectRow(indexPath, true, UITableViewScrollPosition.None);

            var cell = TableView.CellAt(indexPath) as SongTableViewCell;

            SetAnimation(cell, indexPath);
        }

        public void SetWarningView(string message)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var label = new UILabel(new CGRect(0, 0, 200, 50))
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

        private void SetAnimation(SongTableViewCell cell, NSIndexPath indexPath)
        {
            var currentSong = _сompilation.Songs.FirstOrDefault(x => x.Id == _presenter.GetCurrentSong()?.Id);

            if (_сompilation.Songs.IndexOf(currentSong) == indexPath.Row)
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


