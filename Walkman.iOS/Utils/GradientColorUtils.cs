using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Walkman.iOS.Utils
{
	public static class GradientColorUtils
	{
        public static CALayer Orange(nfloat width, nfloat height)
        {
            var top = UIColor.FromRGB(255, 155, 25).CGColor;
            var bottom = UIColor.FromRGB(255, 98, 2).CGColor;

            var gradientlayer = new CAGradientLayer()
            {
                Colors = new CGColor[] { top, bottom },
                Locations = new NSNumber[] { 0.0, 1.0 },
                Frame = new CGRect(0, 0, width, height),
                StartPoint = new CGPoint(0, 1),
                EndPoint = new CGPoint(1, 1)
            };

            return gradientlayer;
        }

        public static CALayer Black(nfloat width, nfloat height)
        {
            var top = UIColor.FromRGB(90, 90, 90).CGColor;
            var bottom = UIColor.FromRGB(32, 32, 32).CGColor;

            var gradientlayer = new CAGradientLayer()
            {
                Colors = new CGColor[] { top, bottom },
                Locations = new NSNumber[] { 0.0, 1.0 },
                Frame = new CGRect(0, 0, width, height),
                StartPoint = new CGPoint(0, 1),
                EndPoint = new CGPoint(1, 1)
            };

            return gradientlayer;
        }

    }
}

