using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.RecentSongModule;
using Walkman.Core.Models;

namespace Walkman.iOS.Modules.RecentSongModule
{
    public class RecentSongPresenter : IRecentSongPresenter
	{
        private readonly IRecentSongRouter _router;
        private readonly IRecentSongInteractor _interactor;
        private IRecentSongView _view;

		public RecentSongPresenter(IRecentSongRouter router, IRecentSongInteractor interactor)
		{
            _router = router;
            _interactor = interactor;

            _router.RecentSongPresenter = this;
            _interactor.RecentSongPresenter = this;
		}

        public void ConfigureView(IRecentSongView view)
        {
            _view = view;
            _view.ConfigureView();
        }

        public async Task SearchSongsAsync()
        {
            try
            {
                var songs = await _interactor.GetSongsAsync();

                if (songs.Any())
                    _view?.SetSongs(songs);
                else
                    _view?.SetWarningView("Начинай слушать музыку 🥹");
            }
            catch (FlurlHttpException)
            {
                _view?.SetWarningView("Ошибка загрузки 😓");
            }
        }

        public void PlaySong(List<SongInfo> songs, int selectIndex)
        {
            _router.PlaySong(songs, selectIndex);
        }

        public void SetNewSong(SongInfo songInfo)
        {
            _view?.SetNewSong(songInfo);
        }

        public async Task SaveSongAsync(SongInfo songInfo)
        {
            await _interactor.SaveSongAsync(songInfo);

            _view?.InsertSong(songInfo);
        }

        public async Task DeleteSongAsync(SongInfo songInfo)
        {
            await _interactor.DeleteSongAsync(songInfo);
        }

        public void SetWarningView()
        {
            _view?.SetWarningView("Начинай слушать музыку 🥹");
        }

        public SongInfo GetCurrentSong()
        {
            return _router.GetCurrentSong();
        }

        public PlayerStatus GetPlayerStatus()
        {
            return _router.GetPlayerStatus();
        }

        public void SetPause(SongInfo song)
        {
            _view?.SetPause(song);
        }

        public void SetPlay(SongInfo song)
        {
            _view?.SetPlay(song);
        }
    }
}

