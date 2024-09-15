using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Walkman.Core.Interfaces.FavoriteSongModule;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.PlayerModule;
using Walkman.Core.Interfaces.ShortSongInfoModule;
using Walkman.Core.Models;
using Walkman.iOS.Modules.PopularGenreModule;
using Walkman.iOS.Modules.RecentSongModule;

namespace Walkman.iOS.Modules.FavoriteSongModule
{
    public class FavoriteSongRouter : IFavoriteSongRouter
    {
        public IFavoriteSongPresenter FavoriteSongPresenter { get; set; }
        private  IPlayerPresenter _playerPresenter;
        private PlayerUtils _player;

        public FavoriteSongRouter(PlayerUtils player)
        {
            _player = player;

            _player.NextSong += SongChanged;
            _player.PreviousSong += SongChanged;

            _player.PlayAction += PlayAction;
            _player.PauseAction += PauseAction;

            _playerPresenter = ServiceProviderFactory.ServiceProvider.GetService<IPlayerPresenter>();
            _playerPresenter.SetToFavorite += async (SongInfo songInfo) => await SetToFavoriteAsync(songInfo);
        }

        private void SongChanged(SongInfo songInfo)
        {
            FavoriteSongPresenter.SetNewSong(songInfo);
        }

        private async Task SetToFavoriteAsync(SongInfo song)
        {
            await FavoriteSongPresenter.AddSongAsync(song);
        }

        public PlayerStatus GetPlayerStatus()
        {
            return _player.GetPlayerStatus();
        }

        public void PlaySong(List<SongInfo> songs, int selectIndex)
        {
            _player.SetSongs(songs, selectIndex);
            _player.PlayPause();
        }

        public SongInfo GetCurrentSong()
        {
            return _player.GetCurrentSong();
        }

        private void PauseAction(SongInfo song)
        {
            FavoriteSongPresenter.SetPause(song);
        }

        private void PlayAction(SongInfo song)
        {
            FavoriteSongPresenter.SetPlay(song);
        }
    }
}

