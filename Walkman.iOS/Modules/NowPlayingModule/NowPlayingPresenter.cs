using System;
using System.Collections.Generic;
using System.Linq;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.NowPlayingModule;
using Walkman.Core.Models;

namespace Walkman.iOS.Modules.NowPlayingModule
{
    public class NowPlayingPresenter : INowPlayingPresenter
    {
        private readonly INowPlayingRouter _router;
        private readonly INowPlayingInteractor _interactor;
        private INowPlayingView _view;

        public List<SongInfo> Songs { get; set; }

        public NowPlayingPresenter(INowPlayingRouter nowPlayingRouter, INowPlayingInteractor nowPlayingInteractor)
        {
            _router = nowPlayingRouter;
            _interactor = nowPlayingInteractor;

            _interactor.NowPlayingPresenter = this;
            _router.NowPlayingPresenter = this;
        }

        public void ConfigureView(INowPlayingView view)
        {
            _view = view;

            _view.ConfigureView();
        }

        public SongInfo GetCurrentSong()
        {
            return _interactor.GetCurrentSong();
        }

        public PlayerStatus GetPlayerStatus()
        {
            return _router.GetPlayerStatus();
        }

        public bool IsRepeat()
        {
            return _interactor.IsRepeat();
        }

        public void MixSongs()
        {
            var random = new Random();

            var mixSongs = Songs.OrderBy(x => random.Next()).ToList();

            _interactor.PlaySong(mixSongs, 0);

            Songs = mixSongs;

            _view.SetSongs();
        }

        public void PlaySong(List<SongInfo> songs, int selectIndex)
        {
            _interactor.PlaySong(songs, selectIndex);
        }

        public void Repeat()
        {
            _interactor.Repeat();
        }

        public void SetNewSong(SongInfo song)
        {
            _view?.SetNewSong(song);
        }

        public void SetPause(SongInfo song)
        {
            _view?.SetPause(song);
        }

        public void SetPlay(SongInfo song)
        {
            _view?.SetPlay(song);
        }

        public void SetSongs()
        {
            Songs = _interactor.GetSongs();

            _view.SetSongs();
        }
    }
}

