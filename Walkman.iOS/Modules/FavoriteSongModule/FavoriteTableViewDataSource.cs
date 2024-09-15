using System;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;
using Walkman.Core.Interfaces.FavoriteSongModule;
using Walkman.Core.Models;
using Walkman.iOS.Views;

namespace Walkman.iOS.Modules.FavoriteSongModule
{
    public class FavoriteTableViewDataSource : UITableViewDataSource
	{
        private IFavoriteSongPresenter _presenter;

        public FavoriteTableViewDataSource(IFavoriteSongPresenter presenter)
		{
            _presenter = presenter;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(SongTableViewCell.Key) as SongTableViewCell;

            cell.Frame = new CGRect(cell.Frame.X, cell.Frame.Y, tableView.Frame.Width, cell.Frame.Height);

            cell.UpdateCell(_presenter.Songs[indexPath.Row]);

            SetAnimation(cell, indexPath);

            return cell;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return _presenter.Songs?.Count ?? 0;
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            _presenter.SelectSong();

            return true;
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            if (editingStyle == UITableViewCellEditingStyle.Delete)
            {
                var song = _presenter.Songs.ElementAt(indexPath.Row);

                song.IsFavorite = false;

                Task.Run(async () => await _presenter.DeleteSongAsync(song));              
            }
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
    }
}

