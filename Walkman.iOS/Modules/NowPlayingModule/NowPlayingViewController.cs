using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using Microsoft.Extensions.DependencyInjection;
using UIKit;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.NowPlayingModule;
using Walkman.Core.Models;
using Walkman.iOS.Views;
using XLPagerTabStrip;

namespace Walkman.iOS.Modules.NowPlayingModule
{
    public partial class NowPlayingViewController : UIViewController, IUITableViewDataSource, INowPlayingView, IIndicatorInfoProvider
    {
        private INowPlayingPresenter _presenter;

        public NowPlayingViewController(IntPtr handle) : base(handle)
        {
            _presenter = ServiceProviderFactory.ServiceProvider.GetService<INowPlayingPresenter>();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _presenter.ConfigureView(this);
            _presenter.SetSongs();
        }

        #region Override

        public  UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(SongTableViewCell.Key, indexPath) as SongTableViewCell;

            cell.Frame = new CGRect(cell.Frame.X, cell.Frame.Y, tableView.Frame.Width, cell.Frame.Height);

            cell.UpdateCell(_presenter.Songs[indexPath.Row]);

            cell.BackgroundColor = UIColor.Clear;

            SetAnimation(cell, indexPath);

            return cell;
        }

        public  nint RowsInSection(UITableView tableView, nint section)
        {
            return _presenter.Songs.Count;
        }

        public override void ViewWillAppear(bool animated)
        {
            var currentSong = _presenter?.Songs.FirstOrDefault(x => x.Id == _presenter.GetCurrentSong()?.Id);

            if (currentSong == null && NowPlayingTablewView.IndexPathForSelectedRow != null)
            {
                NowPlayingTablewView.DeselectRow(NowPlayingTablewView.IndexPathForSelectedRow, true);
            }
            else if (currentSong != null)
            {
                var index = _presenter.Songs.IndexOf(currentSong);

                var indexPath = NSIndexPath.FromRowSection(index, 0);

                NowPlayingTablewView.SelectRow(indexPath, true, UITableViewScrollPosition.Middle);

                var cell = NowPlayingTablewView.CellAt(indexPath) as SongTableViewCell;

                SetAnimation(cell, indexPath);
            }
        }
        #endregion

        public void ConfigureView()
        {
            NowPlayingTablewView.RegisterNibForCellReuse(SongTableViewCell.Nib, nameof(SongTableViewCell));

            NowPlayingTablewView.Delegate = new NowPlayingTablewViewDelegate(_presenter);
            NowPlayingTablewView.DataSource = this;

            NowPlayingTablewView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            NowPlayingTablewView.BackgroundColor = UIColor.Clear;

            MixButton.TouchUpInside += MixSongs;
            RepeatButton.TouchUpInside += Repeate;

            var repeateImage = _presenter.IsRepeat() ? UIImage.GetSystemImage("repeat.1") : UIImage.GetSystemImage("repeat");
            RepeatButton.Selected = _presenter.IsRepeat();
            RepeatButton.SetImage(repeateImage, UIControlState.Normal);
        }

        public void SetSongs()
        {
            NowPlayingTablewView.ReloadData();

            ViewWillAppear(true);
        }

        public void SetNewSong(SongInfo songInfo)
        {
            NowPlayingTablewView.VisibleCells.OfType<SongTableViewCell>().ToList().ForEach(x => x.HideAnimation());

            var currentSong = _presenter.Songs.FirstOrDefault(x => x.Id == songInfo.Id);

            var index = _presenter.Songs.IndexOf(currentSong);
            var indexPath = NSIndexPath.FromRowSection(index, 0);

            NowPlayingTablewView.SelectRow(indexPath, true, UITableViewScrollPosition.None);

            var cell = NowPlayingTablewView.CellAt(indexPath) as SongTableViewCell;

            SetAnimation(cell, indexPath);
        }

        public void SetPlay(SongInfo song)
        {
            if (NowPlayingTablewView.IndexPathForSelectedRow != null)
            {
                var cell = NowPlayingTablewView.CellAt(NowPlayingTablewView.IndexPathForSelectedRow) as SongTableViewCell;

                cell?.ContinueAnimation();
            }
        }

        public void SetPause(SongInfo song)
        {
            if (NowPlayingTablewView.IndexPathForSelectedRow != null)
            {
                var cell = NowPlayingTablewView.CellAt(NowPlayingTablewView.IndexPathForSelectedRow) as SongTableViewCell;

                cell?.PauseAnimation();
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

        private void Repeate(object sender, EventArgs e)
        {
            UIView.Animate(0.2, () =>
            {
                RepeatButton.Transform = CGAffineTransform.MakeScale(0.9f, 0.9f);
            }, () =>
            {
                UIView.Animate(0.2, () =>
                {
                    _presenter.Repeat();

                    var image = _presenter.IsRepeat() ? UIImage.GetSystemImage("repeat.1") : UIImage.GetSystemImage("repeat");

                    RepeatButton.Selected = _presenter.IsRepeat();
                    RepeatButton.SetImage(image, UIControlState.Normal);

                    RepeatButton.Transform = CGAffineTransform.MakeIdentity();
                });
            });
        }

        private void MixSongs(object sender, EventArgs e)
        {
            UIView.Animate(0.2, () =>
            {
                MixButton.Transform = CGAffineTransform.MakeScale(0.9f, 0.9f);
            }, () =>
            {
                UIView.Animate(0.2, () =>
                {
                    MixButton.Transform = CGAffineTransform.MakeIdentity();
                });
            });

            _presenter.MixSongs();
        }        

        public IndicatorInfo IndicatorInfoForPagerTabStrip(PagerTabStripViewController pagerTabStripController)
        {
            var image = UIImage.GetSystemImage("music.note.list").ApplyTintColor(UIColor.FromName("ElementColor"));

            return new IndicatorInfo("", image, this);
        }
    }
}
