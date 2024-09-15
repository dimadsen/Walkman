using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl.Http;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.PopularModule;
using Walkman.Core.Models;

namespace Walkman.iOS.Modules.PopularModule
{
    public class PopularPresenter : IPopularPresenter
	{
        private readonly IPopularInteractor _interactor;
        private readonly IPopularRouter _router;
        private IPopularView _view;


        public PopularPresenter(IPopularInteractor interactor, IPopularRouter router)
		{
            _interactor = interactor;
            _router = router;

            _router.PopularPresenter = this;
        }

        public void ConfigureView(IPopularView view)
        {
            _view = view;
            _view.ConfigureView();
        }

        public SongInfo GetCurrentSong()
        {
            return _router.GetCurrentSong();
        }

        public PlayerStatus GetPlayerStatus()
        {
            return _router.GetPlayerStatus();
        }

        public async Task GetPopularAsync()
        {
            try
            {
                var compilations = await _interactor.GetPopularAsync();

                _view.SetCompilations(compilations);

            }
            catch (FlurlHttpException)
            {
                //Todo: Добавить warning
            }
        }

        public void PlaySong(List<SongInfo> songs, int selectIndex)
        {
            _router.PlaySong(songs, selectIndex);
        }

        public void PrepareForSegue(СompilationInfo сompilation)
        {
            _router.PrepareForGenre(сompilation);
        }

        public void SetNewSong(SongInfo songInfo)
        {
            _view?.SetNewSong(songInfo);
        }

        public void SetPause(SongInfo song)
        {
            _view?.SetPause(song);
        }

        public void SetPlay(SongInfo song)
        {
            _view.SetPlay(song);
        }
    }
}

