using System;
using Foundation;
using UIKit;
using Walkman.Core.Interfaces.NowPlayingModule;

namespace Walkman.iOS.Modules.NowPlayingModule
{
	public class NowPlayingTablewViewDelegate : UITableViewDelegate
    {
		private INowPlayingPresenter _presenter;

        public NowPlayingTablewViewDelegate(INowPlayingPresenter presenter)
		{
			_presenter = presenter;
		}

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            _presenter.PlaySong(_presenter.Songs, indexPath.Row);
        }
    }
}

