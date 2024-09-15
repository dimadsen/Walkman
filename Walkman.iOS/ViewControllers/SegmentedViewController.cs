using System;
using UIKit;
using Walkman.iOS.Modules.NowPlayingModule;
using Walkman.iOS.Modules.PlayerModule;
using Walkman.iOS.Utils;
using XLPagerTabStrip;

namespace Walkman.iOS.ViewControllers
{
    public partial class SegmentedViewController : ButtonBarPagerTabStripViewController
    {
        private UISearchController _searchViewC;


        public SegmentedViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            Settings.Style.ButtonBarItemFont = UIFont.SystemFontOfSize(15);

            Settings.Style.ButtonBarBackgroundColor = ColorUtils.GetInterfaceStyle(View.Frame.Width, View.Frame.Height, View.Frame.Size);
            Settings.Style.SelectedBarBackgroundColor = UIColor.FromName("ElementColor");
            Settings.Style.SelectedBarHeight = 0;
            Settings.Style.SelectedBarWidth = View.Frame.Width / 5;

            Settings.Style.ButtonBarMinimumLineSpacing = 0;
            Settings.Style.ButtonBarItemTitleColor = UIColor.FromName("ElementColor");
            Settings.Style.ButtonBarItemsShouldFillAvailiableWidth = false;
            Settings.Style.ButtonBarLeftContentInset = 0;
            Settings.Style.ButtonBarRightContentInset = 0;
            Settings.Style.ButtonBarHeight = ((UITabBarController)UIApplication.SharedApplication.KeyWindow.RootViewController).TabBar.Frame.Height / 2;
            Settings.Style.LabelWidth = View.Frame.Width / 2;

            base.ViewDidLoad();
        }

        public override UIViewController[] CreateViewControllersForPagerTabStrip(PagerTabStripViewController pagerTabStripViewController)
        {
            View.BackgroundColor = ColorUtils.GetInterfaceStyle(View.Frame.Width, View.Frame.Height, View.Frame.Size);
            ContainerView.Bounces = false;

            var storyboard = UIStoryboard.FromName("Main", null);

            var playerViewController = storyboard.InstantiateViewController(nameof(PlayerViewController));
            var nowPlayingViewController = storyboard.InstantiateViewController(nameof(NowPlayingViewController));

            return new UIViewController[] { playerViewController, nowPlayingViewController };
        }
    }
}


