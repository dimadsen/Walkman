// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Walkman.iOS
{
	[Register ("PopularCollectionViewCell")]
	partial class PopularCollectionViewCell
	{
		[Outlet]
		UIKit.UIImageView AlbumCover { get; set; }

		[Outlet]
		Walkman.iOS.Views.MusicIndicatorView AnimationView { get; set; }

		[Outlet]
		UIKit.UILabel Artist { get; set; }

		[Outlet]
		UIKit.UILabel Song { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AlbumCover != null) {
				AlbumCover.Dispose ();
				AlbumCover = null;
			}

			if (Artist != null) {
				Artist.Dispose ();
				Artist = null;
			}

			if (Song != null) {
				Song.Dispose ();
				Song = null;
			}

			if (AnimationView != null) {
				AnimationView.Dispose ();
				AnimationView = null;
			}
		}
	}
}
