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
	[Register ("RootViewController")]
	partial class RootViewController
	{
		[Outlet]
		UIKit.UIView ShortSongInfo { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ShortSongInfo != null) {
				ShortSongInfo.Dispose ();
				ShortSongInfo = null;
			}
		}
	}
}