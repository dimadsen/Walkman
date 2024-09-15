using System;
using CoreAnimation;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Walkman.iOS.Views
{
    public partial class MusicIndicatorView : UIView
    {
        private readonly int _barCount = 4;
        private nfloat _barWidth;
        private nfloat _barIdleHeight = 3f;
        private nfloat _barSpacing;
        private nfloat _barMinPeakHeight = 6.0f;
        private nfloat _barMaxPeakHeight;
        private readonly double _minBaseOscillationPeriod = 0.6;
        private readonly double _maxBaseOscillationPeriod = 0.8;
        private readonly string _key = "oscillation";

        private readonly List<CALayer> _bars = new List<CALayer>();

        public MusicIndicatorView(IntPtr intPtr) : base(intPtr)
        {
            Layer.CornerRadius = 5;
            Layer.Opacity = 0.7f;

            _barMaxPeakHeight = Bounds.Height * 54 / 100;
            _barWidth = Bounds.Width * 10.77f / 100;
            _barSpacing = Bounds.Width * 3.58f / 100;

            PrepareBarLayers();
        }

        private void PrepareBarLayers()
        {
            var xOffset = Bounds.Width * 23 / 100; 

            for (int i = 0; i < _barCount; i++)
            {
                var bar = CreateBarLayerWithOffset(xOffset, i);
                bar.BackgroundColor = UIColor.SystemOrange.CGColor;

                _bars.Add(bar);
                Layer.AddSublayer(bar);

                xOffset += _barSpacing + _barWidth;
            }
        }

        private CALayer CreateBarLayerWithOffset(nfloat xOffset, int barIndex)
        {
            var y = Bounds.Height * 77 / 100; 

            var layer = new CALayer()
            {
                AnchorPoint = new CGPoint(0.0, 1.0),
                Position = new CGPoint(xOffset, y),
                Bounds = new CGRect(0, 0, _barWidth, barIndex * _barMaxPeakHeight / _barCount),
            };

            return layer;
        }

        public void Start()
        {
            var random = new Random();

            var basePeriod = _minBaseOscillationPeriod + (random.NextDouble() * (_maxBaseOscillationPeriod - _minBaseOscillationPeriod));

            foreach (var bar in _bars)
            {
                var peakHeight = _barMinPeakHeight + random.Next(Convert.ToInt32(_barMaxPeakHeight - _barMinPeakHeight + 1));

                var fromBounds = bar.Bounds;
                fromBounds.Size = new CGSize(fromBounds.Size.Width, _barIdleHeight);

                var toBounds = bar.Bounds;
                toBounds.Size = new CGSize(toBounds.Size.Width, peakHeight);

                var animation = CABasicAnimation.FromKeyPath("bounds");
                animation.From = NSValue.FromCGRect(fromBounds);
                animation.To = NSValue.FromCGRect(toBounds);
                animation.RepeatCount = float.MaxValue;
                animation.AutoReverses = true;
                animation.Duration = basePeriod / 2 * (_barMaxPeakHeight / peakHeight);
                animation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseIn);
                animation.RemovedOnCompletion = false;

                bar.AddAnimation(animation, _key);
            }
        }

        public void Pause()
        {
            foreach (var bar in _bars)
            {
                bar.Bounds = new CGRect(0, 0, _barWidth, _barIdleHeight);

                bar.RemoveAnimation(_key);
            }
        }

        public void Remove()
        {
            foreach (var bar in _bars)
            {
                bar.RemoveFromSuperLayer();
            }
        }        
    }
}

