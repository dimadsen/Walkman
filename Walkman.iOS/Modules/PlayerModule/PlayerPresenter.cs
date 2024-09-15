using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Walkman.Core.Interfaces.PlayerModule;
using Walkman.Core.Interfaces.Models;
using System.Threading;
using AVFoundation;
using Xamarin.Essentials;
using Walkman.Core.Models;
using VkNet.Model;

namespace Walkman.iOS.Modules.PlayerModule
{
    public class PlayerPresenter : IPlayerPresenter
    {
        private IPlayerRouter _router;
        private IPlayerInteractor _interactor;
        private IPlayerView _view;
        private PlayerUtils _player;

        private bool _canSetPlayBackPosition = true;

        public event Action<SongInfo> SetToFavorite;

        public PlayerPresenter(IPlayerRouter router, IPlayerInteractor interactor, PlayerUtils player)
        {
            _router = router;
            _interactor = interactor;

            _router.PlayerPresenter = this;
            _interactor.PlayerPresenter = this;
            _player = player;

            var timer = new Timer(new TimerCallback(SetSongProgress), null, 0, 1000);
        }

        public void ConfigureView(IPlayerView view)
        {
            _view = view;
            _view.ConfigureView();

            var model = new PlayerModel
            {
                CurrentTime = _player.GetCurrentTime(),
                IsLastSong = _player.IsLastSong(),
                IsFirstSong = _player.IsFirstSong(),
                PlayerStatus = _player.GetPlayerStatus(),
                CurrentSong = _player.GetCurrentSong(),
                SongStatistics = _player.GetSongStatistics(),
            };
            _view.SetCurrentSong(model);
        }

        public void SetCurrentSong(SongInfo songInfo, MoveSong move)
        {
            var model = new PlayerModel
            {
                CurrentTime = _player.GetCurrentTime(),
                IsLastSong = _player.IsLastSong(),
                IsFirstSong = _player.IsFirstSong(),
                PlayerStatus = _player.GetPlayerStatus(),
                CurrentSong = _player.GetCurrentSong(),
                SongStatistics = _player.GetSongStatistics(),
                Move = move,
            };

            _view?.SetCurrentSong(model);
        }

        public void SetPlay()
        {
            _view?.SetPlay();
        }

        public void SetPause()
        {
            _view?.SetPause();
        }

        public void SetPlaybackPosition(double value, bool canPlay)
        {
            _view?.SetPlaybackPosition(value);

            _canSetPlayBackPosition = canPlay;
        }

        public void ChangePlayPause()
        {
            _player.PlayPause();
        }

        public void ChangeNextSong()
        {
            _player.Next();
        }

        public void ChangePreviousSong()
        {
            _player.Previous();
        }

        public void ChangePlaybackPosition(double value)
        {
            _canSetPlayBackPosition = false;

            _player.ChangePlaybackPosition(value);
        }

        public void SetFavorite(SongInfo song)
        {
            SetToFavorite?.Invoke(song);
        }

        private void SetSongProgress(object obj)
        {
            if (_canSetPlayBackPosition)
                _view?.SetPlaybackPosition(_player.GetCurrentTime());
        }

        public void DownloadSong(SongInfo song)
        {
            song.DownloadStatus = DownloadStatus.Processing;

            Task.Run(async () =>
            {
                var isSuccess = await _router.DownloadSongAsync(song);

                if (isSuccess)
                {
                    song.DownloadStatus = DownloadStatus.Сompleted;
                    _view?.SetDownloadSucsessful(song);
                }
                else
                {
                    song.DownloadStatus = DownloadStatus.NotStarted;
                    _view?.SetDownloadError(song);
                }
            });
        }
    }
}

