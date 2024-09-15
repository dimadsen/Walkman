using Foundation;
using UIKit;
using Walkman.Core.Interfaces.FavoriteSongModule;

namespace Walkman.iOS.Modules.FavoriteSongModule
{
    public class FavoriteTableViewDelegate : UITableViewDelegate
	{
        private IFavoriteSongPresenter _presenter;

        public FavoriteTableViewDelegate(IFavoriteSongPresenter presenter)
		{
            _presenter = presenter;

		}

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            _presenter.PlaySong(_presenter.Songs, indexPath.Row);
        }
    }
}

