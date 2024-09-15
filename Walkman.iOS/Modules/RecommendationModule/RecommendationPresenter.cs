using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.RecommendationModule;
using Walkman.Core.Models;

namespace Walkman.iOS.Modules.RecommendationModule
{
    public class RecommendationPresenter : IRecommendationPresenter
	{
        private readonly IRecommendationRouter _router;
        private readonly IRecommendationInteractor _interactor;
        private IRecommendationView _view;


        public uint Page { get; set ; }

		public RecommendationPresenter(IRecommendationRouter router, IRecommendationInteractor interactor)
		{
            _router = router;
            _router.RecommendationPresenter = this;

            _interactor = interactor;
		}

        public void ConfigureView(IRecommendationView view)
        {
            _view = view;
            _view.ConfigureView();
        }

        public async Task SetRecommendationAsync()
        {
            try
            {
                if (Page == 0)
                    await _interactor.ChangeRecommendationsAsync();

                var songs = await _interactor.GetRecommendationsAsync(Page);

                if (!songs.Any() && Page == 0)
                {
                    _view.SetWarningView("Формируется на основе Избранного 😉");
                }
                else
                {
                    _view.SetSongs(songs);

                    Page++;
                }
            }
            catch (FlurlHttpException)
            {
                _view.SetWarningView("Ошибка загрузки 😓");
            }
        }

        public async Task ChangeRecommendationsAsync()
        {
            await _interactor.ChangeRecommendationsAsync();
        }

        public void PlaySong(List<SongInfo> songs, int selectIndex)
        {
            _router.PlaySong(songs, selectIndex);
        }

        public void SetNewSong(SongInfo songInfo)
        {
            _view.SetNewSong(songInfo);
        }

        public SongInfo GetCurrentSong()
        {
            return _router.GetCurrentSong();
        }

        public void SetPause(SongInfo song)
        {
            _view?.SetPause(song);
        }

        public void SetPlay(SongInfo song)
        {
            _view?.SetPlay(song);
        }

        public PlayerStatus GetPlayerStatus()
        {
            return _router.GetPlayerStatus();
        }
    }
}

