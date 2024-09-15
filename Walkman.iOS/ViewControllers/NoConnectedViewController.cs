using System;
using UIKit;
using Walkman.iOS.Utils;

namespace Walkman.iOS.ViewControllers
{
    public partial class NoConnectedViewController : UIViewController
    {
        public NoConnectedViewController(IntPtr handle) : base(handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = ColorUtils.GetInterfaceStyle(View.Frame.Width, View.Frame.Height, View.Frame.Size);

            InfoImage.TintColor = (UIColor.FromName("BackgroundColor"));
            InfoImage.Image = InfoImage.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);

            ModalInPresentation = true;
        }
    }
}


