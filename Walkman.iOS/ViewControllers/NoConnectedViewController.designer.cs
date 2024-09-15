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
	[Register ("NoConnectedViewController")]
	partial class NoConnectedViewController
	{
		[Outlet]
		UIKit.UIImageView InfoImage { get; set; }

		[Outlet]
		UIKit.UILabel Message { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (InfoImage != null) {
				InfoImage.Dispose ();
				InfoImage = null;
			}

			if (Message != null) {
				Message.Dispose ();
				Message = null;
			}
		}
	}
}
