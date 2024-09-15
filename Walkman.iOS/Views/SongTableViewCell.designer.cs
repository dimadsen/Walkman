// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Walkman.iOS.Views
{
	[Register ("SongTableViewCell")]
	partial class SongTableViewCell
	{
		[Outlet]
		UIKit.UIImageView AlbumCover { get; set; }

		[Outlet]
		Walkman.iOS.Views.MusicIndicatorView AnimationView { get; set; }

		[Outlet]
		UIKit.UILabel Artist { get; set; }

		[Outlet]
		UIKit.UILabel Song { get; set; }

		[Outlet]
		UIKit.UILabel Time { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AlbumCover != null) {
				AlbumCover.Dispose ();
				AlbumCover = null;
			}

			if (AnimationView != null) {
				AnimationView.Dispose ();
				AnimationView = null;
			}

			if (Artist != null) {
				Artist.Dispose ();
				Artist = null;
			}

			if (Song != null) {
				Song.Dispose ();
				Song = null;
			}

			if (Time != null) {
				Time.Dispose ();
				Time = null;
			}
		}
	}
}
