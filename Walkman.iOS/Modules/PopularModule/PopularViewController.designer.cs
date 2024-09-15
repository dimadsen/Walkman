// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Walkman.iOS.Modules.PopularModule
{
	[Register ("PopularViewController")]
	partial class PopularViewController
	{
		[Outlet]
		UIKit.UIActivityIndicatorView Indicator { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Indicator != null) {
				Indicator.Dispose ();
				Indicator = null;
			}
		}
	}
}
