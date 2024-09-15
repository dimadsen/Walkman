using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.PopularGenreModule;
using Walkman.Core.Interfaces.PopularModule;
using Walkman.Core.Models;

namespace Walkman.iOS.Modules.PopularModule
{
    public class PopularRouter : IPopularRouter
    {
        private IPopularGenrePresenter _popularGenrePresenter;
        private PlayerUtils _player;

        public IPopularPresenter PopularPresenter { get; set; }

        public PopularRouter(PlayerUtils player)
        {
            _popularGenrePresenter = ServiceProviderFactory.ServiceProvider.GetService<IPopularGenrePresenter>();
            _player = player;

            _player.NextSong += SongChanged;
            _player.PreviousSong += SongChanged;

            _player.PlayAction += PlayAction;
            _player.PauseAction += PauseAction;
        }

        public void PrepareForGenre(СompilationInfo сompilation)
        {
            _popularGenrePresenter.Сompilation = сompilation;
            _popularGenrePresenter.Page = 1;
        }

        private void SongChanged(SongInfo song)
        {
            PopularPresenter.SetNewSong(song);
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
            PopularPresenter.SetPause(song);
        }

        private void PlayAction(SongInfo song)
        {
            PopularPresenter.SetPlay(song);
        }
    }
}

