using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Walkman.Core.Interfaces.DownloadSongModule;
using Walkman.Core.Interfaces.FavoriteSongModule;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.PlayerModule;
using Walkman.Core.Models;

namespace Walkman.iOS.Modules.PlayerModule
{
    public class PlayerRouter : IPlayerRouter
    {
        private PlayerUtils _player;

        public IPlayerPresenter PlayerPresenter { get; set; }
        private IDownloadSongPresenter _downloadPresenter { get; set; }

        public PlayerRouter(PlayerUtils player)
        {
            _player = player;

            _player.PauseAction += PauseAction;
            _player.PlayAction += PlayAction;
            _player.NextSong += NextSong;
            _player.PreviousSong += PreviousSong;
            _player.ChangePlaybackPositionAction += ChangePlaybackPositionAction;

            _player.ChangePlayerStatus += ChangePlayerStatus;

            _downloadPresenter = ServiceProviderFactory.ServiceProvider.GetRequiredService<IDownloadSongPresenter>();
        }

        private void ChangePlayerStatus(PlayerStatus obj)
        {
            if (obj == PlayerStatus.Paused)
                PlayerPresenter.SetPause();
            else
                PlayerPresenter.SetPlay();
        }

        private void ChangePlaybackPositionAction(double value)
        {
            PlayerPresenter.SetPlaybackPosition(value, true);
        }

        private void PreviousSong(SongInfo songInfo)
        {
            PlayerPresenter.SetCurrentSong(songInfo, MoveSong.Back);
        }

        private void NextSong(SongInfo songInfo)
        {
            PlayerPresenter.SetCurrentSong(songInfo, MoveSong.Forward);
        }

        private void PlayAction(SongInfo songInfo)
        {
            PlayerPresenter.SetPlay();
        }

        private void PauseAction(SongInfo songInfo)
        {
            PlayerPresenter.SetPause();
        }

        public async Task<bool> DownloadSongAsync(SongInfo songInfo)
        {
            return await _downloadPresenter.DownloadAsync(songInfo);
        }
    }
}

