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
	[Register ("PopularTableViewCell")]
	partial class PopularTableViewCell
	{
		[Outlet]
		Walkman.iOS.PopularCollectionView CollectionView { get; set; }

		[Outlet]
		UIKit.UILabel Genre { get; set; }

		[Outlet]
		UIKit.UIButton More { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CollectionView != null) {
				CollectionView.Dispose ();
				CollectionView = null;
			}

			if (More != null) {
				More.Dispose ();
				More = null;
			}

			if (Genre != null) {
				Genre.Dispose ();
				Genre = null;
			}
		}
	}
}
