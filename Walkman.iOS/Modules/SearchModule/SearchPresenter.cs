using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.SearchModule;
using Walkman.Core.Models;

namespace Walkman.iOS.Modules.SearchModule
{
    public class SearchPresenter : ISearchPresenter
    {
        private readonly ISearchRouter _router;
        private readonly ISearchInteractor _interactor;
        private ISearchView _view;

        public uint Page { get; set; } = 0;

        public SearchPresenter(ISearchRouter router, ISearchInteractor interactor)
        {
            _router = router;
            _interactor = interactor;

            _router.SearchPresenter = this;
            _interactor.SearchPresenter = this;
        }

        public void ConfigureView(ISearchView view)
        {
            _view = view;
            _view.ConfigureView();
        }

        public async Task SearchSongsAsync(string query)
        {
            try
            {
                var songs = await _interactor.SearchSongsAsync(query, Page);

                if (!songs.Any() && Page == 0)
                {
                    _view.SetWarningView("Ничего не найдено 😕");
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

