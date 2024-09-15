// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Walkman.iOS.ViewControllers
{
	[Register ("LibraryViewController")]
	partial class LibraryViewController
	{
		[Outlet]
		Walkman.iOS.Modules.FavoriteSongModule.FavoriteTableView FavoriteTableView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint FavoriteTableViewHeightConstraint { get; set; }

		[Outlet]
		UIKit.UIScrollView LibraryScrollView { get; set; }

		[Outlet]
		UIKit.UIView MainView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint MainViewHeightConstraint { get; set; }

		[Outlet]
		UIKit.UIView MoreView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (FavoriteTableView != null) {
				FavoriteTableView.Dispose ();
				FavoriteTableView = null;
			}

			if (FavoriteTableViewHeightConstraint != null) {
				FavoriteTableViewHeightConstraint.Dispose ();
				FavoriteTableViewHeightConstraint = null;
			}

			if (LibraryScrollView != null) {
				LibraryScrollView.Dispose ();
				LibraryScrollView = null;
			}

			if (MainView != null) {
				MainView.Dispose ();
				MainView = null;
			}

			if (MoreView != null) {
				MoreView.Dispose ();
				MoreView = null;
			}

			if (MainViewHeightConstraint != null) {
				MainViewHeightConstraint.Dispose ();
				MainViewHeightConstraint = null;
			}
		}
	}
}
