using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Walkman.Core.Interfaces.FavoriteSongModule;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.iOS.Modules.FavoriteSongModule
{
    public class FavoriteSongPresenter : IFavoriteSongPresenter
    {
        private readonly IFavoriteSongRouter _router;
        private readonly IFavoriteSongInteractor _interactor;
        private IFavoriteSongView _view;

        public List<SongInfo> Songs { get; set; } = new List<SongInfo>();

        public FavoriteSongPresenter(IFavoriteSongRouter router, IFavoriteSongInteractor interactor)
        {
            _router = router;
            _interactor = interactor;

            _router.FavoriteSongPresenter = this;
            _interactor.FavoriteSongPresenter = this;
        }

        public void ConfigureView(IFavoriteSongView view)
        {
            _view = view;
            _view.ConfigureView();
        }

        public async Task SearchSongsAsync()
        {
            Songs = await _interactor.GetSongsAsync();

            if (Songs.Any())
                _view.SetSongs();
            else
                _view.SetWarningView("Добавляй любимую музыку 😜");
        }

        public void PlaySong(List<SongInfo> songs, int selectIndex)
        {
            _router.PlaySong(songs, selectIndex);
        }

        public async Task AddSongAsync(SongInfo songInfo)
        {
            if (songInfo.IsFavorite)
                await _interactor.SaveSongAsync(songInfo);
            else
                await _interactor.DeleteSongAsync(songInfo);

            _view.UpdateView(songInfo);
        }

        public async Task DeleteSongAsync(SongInfo songInfo)
        {
            await _interactor.DeleteSongAsync(songInfo);

            _view.UpdateView(songInfo);
        }

        public void SetNewSong(SongInfo songInfo)
        {
            _view.SetNewSong(songInfo);
        }

        public void SetWarningView()
        {
            if (!Songs.Any())
                _view.SetWarningView("Добавляй любимую музыку 😜");
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

        public void SelectSong()
        {
            _view?.SelectSong();
        }
    }
}

