using System.Collections.Generic;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.PopularGenreModule;
using Walkman.Core.Models;

namespace Walkman.iOS.Modules.PopularGenreModule
{
    public class PopularGenreRouter : IPopularGenreRouter
	{
        private PlayerUtils _player;

        public IPopularGenrePresenter PopularGenrePresenter { get; set; }        

        public PopularGenreRouter(PlayerUtils player)
		{
            _player = player;

            _player.NextSong += SongChanged;
            _player.PreviousSong += SongChanged;

            _player.PlayAction += PlayAction;
            _player.PauseAction += PauseAction;
        }

        private void SongChanged(SongInfo songInfo)
        {
            PopularGenrePresenter.SetNewSong(songInfo);
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
            PopularGenrePresenter.SetPause(song);
        }

        private void PlayAction(SongInfo song)
        {
            PopularGenrePresenter.SetPlay(song);
        }
    }
}

