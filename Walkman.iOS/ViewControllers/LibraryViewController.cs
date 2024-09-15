using System;
using Foundation;
using Microsoft.Extensions.DependencyInjection;
using UIKit;
using Walkman.Core.Interfaces.RecentSongModule;
using Walkman.iOS.Utils;

namespace Walkman.iOS.ViewControllers
{
	public partial class LibraryViewController : UIViewController
	{
		public LibraryViewController(IntPtr handle) : base(handle) { }

		public override void ViewDidLoad ()
		{
            ServiceProviderFactory.ServiceProvider.GetService<IRecentSongPresenter>();

            base.ViewDidLoad ();
		}

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            FavoriteTableView.AddObserver(this, "contentSize", NSKeyValueObservingOptions.New, (IntPtr)null);

            FavoriteTableView.ViewWillAppear();
        }

        public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
        {
            NavigationController.NavigationBar.BarTintColor = ColorUtils.GetInterfaceStyle(FavoriteTableView.Frame.Width, FavoriteTableView.Frame.Height, FavoriteTableView.Frame.Size);
            TabBarController.TabBar.BarTintColor = ColorUtils.GetInterfaceStyle(FavoriteTableView.Frame.Width, FavoriteTableView.Frame.Height, FavoriteTableView.Frame.Size);
            TabBarController.TabBar.UnselectedItemTintColor = UIColor.FromName("SearchColor");

            LibraryScrollView.BackgroundColor = ColorUtils.GetInterfaceStyle(FavoriteTableView.Frame.Width, FavoriteTableView.Frame.Height, FavoriteTableView.Frame.Size);
            MainView.BackgroundColor = ColorUtils.GetInterfaceStyle(FavoriteTableView.Frame.Width, FavoriteTableView.Frame.Height, FavoriteTableView.Frame.Size);
            MoreView.BackgroundColor = ColorUtils.GetInterfaceStyle(FavoriteTableView.Frame.Width, FavoriteTableView.Frame.Height, FavoriteTableView.Frame.Size);

            FavoriteTableView.BackgroundColor = ColorUtils.GetInterfaceStyle(FavoriteTableView.Frame.Width, FavoriteTableView.Frame.Height, FavoriteTableView.Frame.Size);

            if (FavoriteTableView.TableFooterView != null)
                FavoriteTableView.TableFooterView.BackgroundColor = ColorUtils.GetInterfaceStyle(FavoriteTableView.Frame.Width, FavoriteTableView.Frame.Height, FavoriteTableView.Frame.Size);

            if (FavoriteTableView.TableHeaderView != null)
                FavoriteTableView.TableHeaderView.BackgroundColor = ColorUtils.GetInterfaceStyle(FavoriteTableView.Frame.Width, FavoriteTableView.Frame.Height, FavoriteTableView.Frame.Size);

            var tableViewSize = new NSObservedChange(change);

            var newValue = ((NSValue)tableViewSize.NewValue).CGSizeValue;

            var height = (nfloat)Math.Round(LibraryScrollView.Frame.Height / 2, MidpointRounding.AwayFromZero);

            FavoriteTableViewHeightConstraint.Constant = height > newValue.Height ? height : newValue.Height - NavigationController.NavigationBar.Frame.Height;
            MainViewHeightConstraint.Constant = FavoriteTableViewHeightConstraint.Constant;
        }
    }
}


