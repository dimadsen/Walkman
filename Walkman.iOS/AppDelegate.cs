using System;
using Foundation;
using Microsoft.Extensions.DependencyInjection;
using UIKit;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VkNet.Model;
using Walkman.Core.Interfaces.PlayerModule;
using Walkman.Core.Interfaces.SearchModule;
using Walkman.iOS.Modules.PlayerModule;
using Walkman.iOS.Modules.SearchModule;
using Walkman.Core.Interfaces.ShortSongInfoModule;
using Walkman.iOS.Modules.ShortSongInfoModule;
using Walkman.Core.Interfaces.RecentSongModule;
using Walkman.iOS.Modules.RecentSongModule;
using System.IO;
using Walkman.Database;
using Walkman.Core.Interfaces.PopularModule;
using Walkman.iOS.Modules.PopularModule;
using Walkman.iOS.Utils;
using Walkman.Core.Interfaces.PopularGenreModule;
using Walkman.iOS.Modules.PopularGenreModule;
using System.Threading;
using Walkman.Core.Interfaces.FavoriteSongModule;
using Walkman.iOS.Modules.FavoriteSongModule;
using Walkman.Core.Interfaces.RecommendationModule;
using Walkman.iOS.Modules.RecommendationModule;
using Walkman.Core.Interfaces.NowPlayingModule;
using Walkman.iOS.Modules.NowPlayingModule;
using Refit;
using System.Net.Http.Headers;
using Walkman.iOS.Infrastructure;
using Walkman.Core.Interfaces.DownloadSongModule;
using Walkman.iOS.Modules.DownloadSongModule;
using Xamarin.Essentials;
using Newtonsoft.Json;

namespace Walkman.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : UIResponder, IUIApplicationDelegate
    {
        [Export("window")]
        public UIWindow Window { get; set; }

        [Export("application:didFinishLaunchingWithOptions:")]
        public bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            var services = new ServiceCollection();
            services.AddAudioBypass();

            var player = new PlayerUtils();
            services.AddSingleton(player);

            var lockScreen = new LockScreenUtils(player);
            services.AddSingleton(lockScreen);

            services.AddScoped<IPlayerInteractor, PlayerInteractor>();
            services.AddScoped<IPlayerRouter, PlayerRouter>();
            services.AddScoped<IPlayerPresenter, PlayerPresenter>();

            services.AddScoped<ISearchInteractor, SearchInteractor>();
            services.AddScoped<ISearchRouter, SearchRouter>();
            services.AddScoped<ISearchPresenter, SearchPresenter>();

            services.AddScoped<IShortSongInfoPresenter, ShortSongInfoPresenter>();
            services.AddScoped<IShortSongInfoRouter, ShortSongInfoRouter>();

            services.AddScoped<IRecentSongInteractor, RecentSongInteractor>();
            services.AddScoped<IRecentSongRouter, RecentSongRouter>();
            services.AddScoped<IRecentSongPresenter, RecentSongPresenter>();

            services.AddScoped<IPopularInteractor, PopularInteractor>();
            services.AddScoped<IPopularPresenter, PopularPresenter>();
            services.AddScoped<IPopularRouter, PopularRouter>();

            services.AddScoped<IPopularGenreInteractor, PopularGenreInteractor>();
            services.AddScoped<IPopularGenrePresenter, PopularGenrePresenter>();
            services.AddScoped<IPopularGenreRouter, PopularGenreRouter>();

            services.AddScoped<IFavoriteSongInteractor, FavoriteSongInteractor>();
            services.AddScoped<IFavoriteSongRouter, FavoriteSongRouter>();
            services.AddScoped<IFavoriteSongPresenter, FavoriteSongPresenter>();

            services.AddScoped<IRecommendationInteractor, RecommendationInteractor>();
            services.AddScoped<IRecommendationRouter, RecommendationRouter>();
            services.AddScoped<IRecommendationPresenter, RecommendationPresenter>();

            services.AddScoped<INowPlayingInteractor, NowPlayingInteractor>();
            services.AddScoped<INowPlayingPresenter, NowPlayingPresenter>();
            services.AddScoped<INowPlayingRouter, NowPlayingRouter>();

            services.AddScoped<IDownloadSongInteractor, DownloadSongInteractor>();
            services.AddScoped<IDownloadSongPresenter, DownloadSongPresenter>();
            services.AddScoped<IDownloadSongRouter, DownloadSongRouter>();

            var secrets = JsonConvert.DeserializeObject<Secrets>(File.ReadAllText("secret.json"));

            services.AddRefitClient<IDownloaderClient>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(secrets.ApiUrl);
                    c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(secrets.ApiKey);
                    c.Timeout = TimeSpan.FromMinutes(5);
                });

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Walkman.db");

            services.AddScoped(x => new WalkmanContext(path));

            services.AddTransient(x =>
            {
                var vkApi = new VkApi(services);
                vkApi.Authorize(new ApiAuthParams { AccessToken = secrets.AccessToken });

                return vkApi;
            });

            ServiceProviderFactory.ServiceProvider = services.BuildServiceProvider();

            Thread.Sleep(1000);

            return true;
        }
    }
}


