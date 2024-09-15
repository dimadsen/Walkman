using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl.Http;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.PopularGenreModule;
using Walkman.Core.Models;

namespace Walkman.iOS.Modules.PopularGenreModule
{
	public class PopularGenrePresenter : IPopularGenrePresenter
	{
        private readonly IPopularGenreInteractor _interactor;
        private readonly IPopularGenreRouter _router;
        private IPopularGenreView _view;


        public uint Page { get; set; }
        public СompilationInfo Сompilation { get ; set ; }

        public PopularGenrePresenter(IPopularGenreInteractor interactor, IPopularGenreRouter router)
		{
            _interactor = interactor;
            _interactor.Presenter = this;

            _router = router;
            _router.PopularGenrePresenter = this;
        }

        public void ConfigureView(IPopularGenreView view)
        {
            _view = view;
            _view.ConfigureView(Сompilation);
        }

        public async Task SetPopularSongsAsync()
        {
            try
            {
                var songs = await _interactor.GetPopularSongsAsync(Сompilation.Genre, Page);

                _view.SetSongs(songs);

                Page++;
            }
            catch (FlurlHttpException )
            {
                //Todo: Добавить warning
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

