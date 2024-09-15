using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;
using Walkman.iOS.Modules.ShortSongInfoModule;
using Walkman.iOS.ViewControllers;
using Xamarin.Essentials;

namespace NewSingleViewTemplate
{
    [Register("SceneDelegate")]
    public class SceneDelegate : UIResponder, IUIWindowSceneDelegate
    {
        private NoConnectedViewController _noConnectedViewController;

        [Export("window")]
        public UIWindow Window { get; set; }

        [Export("scene:willConnectToSession:options:")]
        public void WillConnect(UIScene scene, UISceneSession session, UISceneConnectionOptions connectionOptions)
        {
            var tabBarViewController = Window.RootViewController as UITabBarController;

            var view = NSBundle.MainBundle.LoadNib("ShortSongInfoView", this, null).FirstOrDefault() as ShortSongInfoView;

            var y = tabBarViewController.View.Frame.Height - tabBarViewController.TabBar.Frame.Height * 2 - Window.SafeAreaInsets.Bottom - 5;
            view.Frame = new CGRect(5, y, tabBarViewController.View.Frame.Width - 10 , 50);

            view.SetColor();

            Window.RootViewController.View.AddSubview(view);
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                _noConnectedViewController = UIStoryboard.FromName("Main", null).InstantiateViewController(nameof(NoConnectedViewController)) as NoConnectedViewController;

                _noConnectedViewController.ModalPresentationStyle = UIModalPresentationStyle.PageSheet;
                _noConnectedViewController.SheetPresentationController.Detents = new UISheetPresentationControllerDetent[] { UISheetPresentationControllerDetent.CreateMediumDetent() };
                _noConnectedViewController.SheetPresentationController.PrefersGrabberVisible = true;

                Window.RootViewController.PresentViewController(_noConnectedViewController, true, null);
            }
            else
            {
                _noConnectedViewController?.DismissViewController(true, null);
            }
        }
    }
}
