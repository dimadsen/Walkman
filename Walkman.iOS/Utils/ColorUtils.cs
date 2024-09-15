using System;
using CoreGraphics;
using UIKit;

namespace Walkman.iOS.Utils
{
    public static class ColorUtils
    {
        public static UIColor GetInterfaceStyle(nfloat width, nfloat height, CGSize size)
        {
            return new UIColor((UITraitCollection arg) =>
            {
                switch (arg.UserInterfaceStyle)
                {
                    case UIUserInterfaceStyle.Dark:
                        {
                            var gradient = GradientColorUtils.Black(width, height);
                            var image = ImageUtils.GetGradientImage(gradient, size);
                            return new UIColor(image);
                        }
                    default:
                        {
                            var gradient = GradientColorUtils.Orange(width, height);
                            var image = ImageUtils.GetGradientImage(gradient, size);
                            return new UIColor(image);
                        }
                }
            });
        }

        public static UIColor GetRevertInterfaceStyle(nfloat width, nfloat height, CGSize size)
        {
            return new UIColor((UITraitCollection arg) =>
            {
                switch (arg.UserInterfaceStyle)
                {
                    case UIUserInterfaceStyle.Dark:
                        {
                            var gradient = GradientColorUtils.Orange(width, height);
                            var image = ImageUtils.GetGradientImage(gradient, size);
                            return new UIColor(image);
                        }
                    default:
                        {
                            var gradient = GradientColorUtils.Black(width, height);
                            var image = ImageUtils.GetGradientImage(gradient, size);
                            return new UIColor(image);
                        }
                }
            });
        }
    }
}

