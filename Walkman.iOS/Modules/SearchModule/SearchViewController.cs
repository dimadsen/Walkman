using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using Microsoft.Extensions.DependencyInjection;
using UIKit;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.SearchModule;
using Walkman.iOS.Utils;
using Walkman.iOS.Views;
using Xamarin.Essentials;
using Walkman.Core.Models;

namespace Walkman.iOS.Modules.SearchModule
{
    public partial class SearchViewController : UITableViewController, ISearchView
    {
        private ISearchPresenter _presenter;

        private List<SongInfo> _songs = new List<SongInfo>();

        private string _query;
        private bool _maxCount;

        public SearchViewController(IntPtr handle) : base(handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _presenter = ServiceProviderFactory.ServiceProvider.GetService<ISearchPresenter>();
            _presenter.ConfigureView(this);
        }

        #region TablewView override

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

        public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            Task.Run(async () =>
            {
                var lastCell = _songs.Count - 3;

                if (lastCell == indexPath.Row && !_maxCount)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        var indicator = (TableView.TableFooterView?.Subviews.FirstOrDefault(x => x is UIActivityIndicatorView)) as UIActivityIndicatorView;
                        indicator?.StartAnimating();
                    });

                    await _presenter.SearchSongsAsync(_query);
                }
            });
        }

        #endregion        


        #region Others

        public void ConfigureView()
        {
            TableView.RegisterNibForCellReuse(SongTableViewCell.Nib, nameof(SongTableViewCell));


            TableView.BackgroundColor = ColorUtils.GetInterfaceStyle(TableView.Frame.Width, TableView.Frame.Height, TableView.Frame.Size);

            TableView.Delegate = this;
            TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            TableView.TableFooterView = new UIView(new CGRect(0, TableView.Frame.GetMaxY(), TableView.Frame.Width, 100));

            var indicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Medium)
            {
                Color = UIColor.FromName("ElementColor"),
                HidesWhenStopped = true,
            };
            indicator.Frame = new CGRect(indicator.Frame.X, indicator.Frame.Y, Indicator.Frame.Width, Indicator.Frame.Height);
            indicator.StopAnimating();

            TableView.TableFooterView.AddSubview(indicator);

            NavigationController.NavigationBar.BarTintColor = ColorUtils.GetInterfaceStyle(TableView.Frame.Width, TableView.Frame.Height, TableView.Frame.Size);
            TabBarController.TabBar.BarTintColor = ColorUtils.GetInterfaceStyle(TableView.Frame.Width, TableView.Frame.Height, TableView.Frame.Size);
            TabBarController.TabBar.UnselectedItemTintColor = UIColor.FromName("SearchColor");


            NavigationItem.SearchController = new UISearchController();

            NavigationController.NavigationBar.SizeToFit();

            UIBarButtonItem.Appearance.SetTitleTextAttributes(new UITextAttributes { TextColor = UIColor.FromName("ElementColor") }, UIControlState.Normal);

            var textField = NavigationItem.SearchController.SearchBar.ValueForKey(new NSString("searchField")) as UITextField;

            var glassIconView = textField.LeftView as UIImageView;
            glassIconView.TintColor = UIColor.FromName("SearchColor");
            glassIconView.Image = glassIconView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);

            NavigationItem.SearchController.SearchBar.SearchTextField.AttributedPlaceholder = new NSAttributedString("Поиск музыки", new CoreText.CTStringAttributes()
            {
                UnderlineColor = UIColor.FromName("SearchColor").CGColor,
                StrokeColor = UIColor.FromName("SearchColor").CGColor,
                ForegroundColor = UIColor.FromName("SearchColor").CGColor
            });

            var closeButton = textField.ValueForKey(new NSString("_clearButton")) as UIButton;
            closeButton.SetImage(closeButton.ImageView.Image.ApplyTintColor(UIColor.FromName("SearchColor")), UIControlState.Normal);

            NavigationItem.SearchController.SearchBar.SearchButtonClicked += Search;
        }

        private void Search(object sender, EventArgs e)
        {
            _songs = new List<SongInfo>();
            _maxCount = false;

            _query = ((UISearchBar)sender).Text;

            Indicator.StartAnimating();
            TableView.TableHeaderView = Indicator;
            TableView.DataSource = null;
            TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            var warningLabel = TableView.Subviews.FirstOrDefault(x => x.Tag == 1);
            warningLabel?.RemoveFromSuperview();

            _presenter.Page = 0;

            Task.Run(async () => await _presenter.SearchSongsAsync(_query));
        }

        public void SetSongs(List<SongInfo> songs)
        {
            if (_presenter.Page == 0)
            {
                _songs = songs;

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
                _songs.AddRange(songs);

                var indices = songs.Select(x =>
                {
                    var index = _songs.IndexOf(x);
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

            var currentSong = _songs.FirstOrDefault(x => x.Id == songInfo.Id);

            var index = _songs.IndexOf(currentSong);
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

        #endregion
    }
}


