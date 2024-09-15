using System;
using Foundation;
using System.Threading.Tasks;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace Walkman.iOS.Utils
{
    public static class ImageUtils
    {
        public static async Task<string> DownloadFileAsync(string url, long fileName)
        {
            try
            {
                var downloadResponse = await NSUrlSession.SharedSession.CreateDataTaskAsync(new NSUrlRequest(new NSUrl(url)));

                var cachePath = SetCachePath(fileName);

                NSFileManager.DefaultManager.CreateFile(cachePath, downloadResponse.Data, new NSFileAttributes { });

                return cachePath;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static NSData GetFileFromCache(long fileName)
        {
            try
            {
                var url = SetCachePath(fileName);

                var file = NSFileManager.DefaultManager.Contents(url);

                return file;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool FileExists(long id)
        {
            var url = SetCachePath(id);

            return NSFileManager.DefaultManager.FileExists(url);
        }

        private static string SetCachePath(long fileName)
        {
            var cachesFolder = NSFileManager.DefaultManager.GetTemporaryDirectory();

            var cachePath = cachesFolder.RelativePath + "/albums/";

            NSFileManager.DefaultManager.CreateDirectory(cachePath, false, new NSFileAttributes { });

            return cachePath + fileName;
        }

        public static UIImage GetGradientImage(CALayer gradient, CGSize size)
        {
            UIGraphics.BeginImageContext(size);
            var ctx = UIGraphics.GetCurrentContext();
            gradient.RenderInContext(ctx);

            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return image;
        }
    }
}

