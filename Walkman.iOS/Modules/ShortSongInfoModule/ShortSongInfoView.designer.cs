// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Walkman.iOS.Modules.ShortSongInfoModule
{
	[Register ("ShortSongInfoView")]
	partial class ShortSongInfoView
	{
		[Outlet]
		UIKit.UILabel Artist { get; set; }

		[Outlet]
		UIKit.UIView ChildView { get; set; }

		[Outlet]
		UIKit.UILabel NotPerformed { get; set; }

		[Outlet]
		UIKit.UIView ParentView { get; set; }

		[Outlet]
		UIKit.UIButton PlayButton { get; set; }

		[Outlet]
		UIKit.UILabel SongName { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Artist != null) {
				Artist.Dispose ();
				Artist = null;
			}

			if (NotPerformed != null) {
				NotPerformed.Dispose ();
				NotPerformed = null;
			}

			if (ParentView != null) {
				ParentView.Dispose ();
				ParentView = null;
			}

			if (PlayButton != null) {
				PlayButton.Dispose ();
				PlayButton = null;
			}

			if (SongName != null) {
				SongName.Dispose ();
				SongName = null;
			}

			if (ChildView != null) {
				ChildView.Dispose ();
				ChildView = null;
			}
		}
	}
}
