using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Walkman.Core.Interfaces.DownloadSongModule;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.iOS.Modules.DownloadSongModule
{
    public class DownloadSongPresenter : IDownloadSongPresenter
    {
        private readonly IDownloadSongInteractor _interactor;
        private readonly IDownloadSongRouter _router;
        private IDownloadSongView _view;

        public List<SongInfo> Songs { get; set; }

        public DownloadSongPresenter(IDownloadSongInteractor interactor, IDownloadSongRouter router)
        {
            _interactor = interactor;
            _router = router;

            _interactor.Presenter = this;
            _router.Presenter = this;
        }

        public void ConfigureView(IDownloadSongView view)
        {
            _view = view;

            _view.ConfigureView();
        }

        public SongInfo GetCurrentSong()
        {
            return _router.GetCurrentSong();
        }

        public void PlaySong(List<SongInfo> songs, int selectIndex)
        {
            _router.PlaySong(songs, selectIndex);
        }

        public void SetNewSong(SongInfo songInfo)
        {
            _view?.SetNewSong(songInfo);
        }

        public async Task SetSongsAsync()
        {
            try
            {
                var songs = await _interactor.GetSongsAsync();

                if (songs.Any())
                    _view?.SetSongs(songs);
                else
                    _view?.SetWarningView("Начинай скачивать музыку 🥹");
            }
            catch (FlurlHttpException)
            {
                _view?.SetWarningView("Ошибка загрузки 😓");
            }
        }

        public void SetWarningView()
        {
            _view?.SetWarningView("Начинай скачивать музыку 🥹");
        }

        public async Task<bool> DownloadAsync(SongInfo songInfo)
        {
            try
            {
                await _interactor.DownloadAsync(songInfo);
                _view?.InsertSong(songInfo);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task DeleteSongAsync(SongInfo songInfo)
        {
            await _interactor.DeleteSongAsync(songInfo);
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

