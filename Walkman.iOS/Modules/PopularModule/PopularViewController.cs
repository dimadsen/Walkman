using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using Microsoft.Extensions.DependencyInjection;
using UIKit;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.PopularModule;
using Walkman.Core.Interfaces.SearchModule;
using Walkman.Core.Models;
using Walkman.iOS.Modules;
using Walkman.iOS.Utils;
using Walkman.iOS.Views;
using Xamarin.Essentials;

namespace Walkman.iOS.Modules.PopularModule
{
    public partial class PopularViewController : UITableViewController, IPopularView
    {
        private IPopularPresenter _presenter;
        private List<СompilationInfo> _compilations = new List<СompilationInfo>();

        public PopularViewController(IntPtr handle) : base(handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _presenter = ServiceProviderFactory.ServiceProvider.GetService<IPopularPresenter>();
            _presenter.ConfigureView(this);

            Task.Run(async () => await _presenter.GetPopularAsync());
        }

        public void ConfigureView()
        {
            TableView.BackgroundColor = ColorUtils.GetInterfaceStyle(TableView.Frame.Width, TableView.Frame.Height, TableView.Frame.Size);
            TableView.SeparatorColor = UIColor.FromName("ElementColor");

            NavigationController.NavigationBar.BarTintColor = ColorUtils.GetInterfaceStyle(TableView.Frame.Width, TableView.Frame.Height, TableView.Frame.Size);
            TabBarController.TabBar.BarTintColor = ColorUtils.GetInterfaceStyle(TableView.Frame.Width, TableView.Frame.Height, TableView.Frame.Size);
            TabBarController.TabBar.UnselectedItemTintColor = UIColor.FromName("SearchColor");

            TableView.TableFooterView = new UIView(new CGRect(0, TableView.Frame.GetMaxY(), TableView.Frame.Width, 55));
        }

        public void SetCompilations(List<СompilationInfo> сompilations)
        {
            _compilations = сompilations;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                TableView.ReloadData();
                Indicator.StopAnimating();

                TableView.TableHeaderView = null;

                TableView.DataSource = this;
            });
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(PopularTableViewCell.Key) as PopularTableViewCell;
            cell.Frame = new CGRect(cell.Frame.X, cell.Frame.Y, tableView.Frame.Width, cell.Frame.Height);

            cell.UpdateCell(_compilations[indexPath.Row], indexPath.Row);

            return cell;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return _compilations.Count;
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            var row = TableView.IndexPathForCell(sender as UITableViewCell).Row;

            var compilation = _compilations[row];

            _presenter.PrepareForSegue(compilation);
        }

        public void SetPlay(SongInfo songInfo)
        {
            for (int i = 0; i < _compilations.Count; i++)
            {
                var hasSong = _compilations[i].Songs.Any(x => x.Id == songInfo.Id);

                if (hasSong)
                {
                    var indexPath = NSIndexPath.FromRowSection(i, 0);

                    var cell = TableView.CellAt(indexPath) as PopularTableViewCell;

                    cell?.SetPlay(songInfo);
                }
            }
        }

        public void SetPause(SongInfo songInfo)
        {
            for (int i = 0; i < _compilations.Count; i++)
            {
                var hasSong = _compilations[i].Songs.Any(x => x.Id == songInfo.Id);

                if (hasSong)
                {
                    var indexPath = NSIndexPath.FromRowSection(i, 0);

                    var cell = TableView.CellAt(indexPath) as PopularTableViewCell;

                    cell?.SetPause(songInfo);
                }
            }
        }

        public void SetNewSong(SongInfo songInfo)
        {
            for (int i = 0; i < _compilations.Count; i++)
            {
                var indexPath = NSIndexPath.FromRowSection(i, 0);

                var cell = TableView.CellAt(indexPath) as PopularTableViewCell;
                cell?.Reload();

                var hasSong = _compilations[i].Songs.Any(x => x.Id == songInfo.Id);

                if (hasSong)
                {

                    cell?.SetNewSong(songInfo);
                }
            }
        }
    }
}


