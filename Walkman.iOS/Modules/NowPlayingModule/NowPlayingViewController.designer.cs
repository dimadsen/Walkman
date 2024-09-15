// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Walkman.iOS.Modules.NowPlayingModule
{
	[Register ("NowPlayingViewController")]
	partial class NowPlayingViewController
	{
		[Outlet]
		UIKit.UIButton MixButton { get; set; }

		[Outlet]
		UIKit.UITableView NowPlayingTablewView { get; set; }

		[Outlet]
		UIKit.UIButton RepeatButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (NowPlayingTablewView != null) {
				NowPlayingTablewView.Dispose ();
				NowPlayingTablewView = null;
			}

			if (MixButton != null) {
				MixButton.Dispose ();
				MixButton = null;
			}

			if (RepeatButton != null) {
				RepeatButton.Dispose ();
				RepeatButton = null;
			}
		}
	}
}
