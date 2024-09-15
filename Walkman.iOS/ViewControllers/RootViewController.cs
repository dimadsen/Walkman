using System;
using Foundation;
using UIKit;
using Walkman.iOS.Utils;
using Xamarin.Essentials;

namespace Walkman.iOS.ViewControllers
{
    public partial class RootViewController : UIViewController
    {
        private UISearchController _searchViewC;
        private NoConnectedViewController _noConnectedViewController;

        public RootViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _searchViewC = new UISearchController();

            DefinesPresentationContext = true;
            _searchViewC.HidesNavigationBarDuringPresentation = false;

            UIBarButtonItem.Appearance.SetTitleTextAttributes(new UITextAttributes { TextColor = UIColor.FromName("ElementColor") }, UIControlState.Normal);

            var textField = _searchViewC.SearchBar.ValueForKey(new NSString("searchField")) as UITextField;

            var glassIconView = textField.LeftView as UIImageView;
            glassIconView.TintColor = UIColor.FromName("SearchColor");
            glassIconView.Image = glassIconView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);

            _searchViewC.SearchBar.SearchTextField.AttributedPlaceholder = new NSAttributedString("Поиск музыки", new CoreText.CTStringAttributes()
            {
                UnderlineColor = UIColor.FromName("SearchColor").CGColor,
                StrokeColor = UIColor.FromName("SearchColor").CGColor,
                ForegroundColor = UIColor.FromName("SearchColor").CGColor
            });

            var closeButton = textField.ValueForKey(new NSString("_clearButton")) as UIButton;
            closeButton.SetImage(closeButton.ImageView.Image.ApplyTintColor(UIColor.FromName("SearchColor")), UIControlState.Normal);

            NavigationItem.TitleView = _searchViewC.SearchBar;

            NavigationController.NavigationBar.BackgroundColor = ColorUtils.GetInterfaceStyle(NavigationController.NavigationBar.Frame.Width,
                NavigationController.NavigationBar.Frame.Height, NavigationController.NavigationBar.Frame.Size);

            var statusBarView = new UIView(UIApplication.SharedApplication.StatusBarFrame);
            statusBarView.BackgroundColor = ColorUtils.GetInterfaceStyle(View.Frame.Width, View.Frame.Height, View.Frame.Size);
            View.AddSubview(statusBarView);

            View.BackgroundColor = ColorUtils.GetInterfaceStyle(View.Frame.Width, View.Frame.Height, View.Frame.Size);

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }


        public override void ViewDidAppear(bool animated)
        {
            ShortSongInfo.Hidden = true;
            ShortSongInfo.BackgroundColor = ColorUtils.GetInterfaceStyle(ShortSongInfo.Frame.Width, ShortSongInfo.Frame.Height, ShortSongInfo.Frame.Size);
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                _noConnectedViewController = UIStoryboard.FromName("Main", null).InstantiateViewController(nameof(NoConnectedViewController)) as NoConnectedViewController;

                _noConnectedViewController.ModalPresentationStyle = UIModalPresentationStyle.PageSheet;
                _noConnectedViewController.SheetPresentationController.Detents = new UISheetPresentationControllerDetent[] { UISheetPresentationControllerDetent.CreateMediumDetent() };
                _noConnectedViewController.SheetPresentationController.PrefersGrabberVisible = true;

                PresentViewController(_noConnectedViewController, true, null);
            }
            else
            {
                _noConnectedViewController?.DismissViewController(true, null);
            }
        }
    }
}


