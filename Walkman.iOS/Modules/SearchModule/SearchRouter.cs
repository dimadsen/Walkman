using System.Collections.Generic;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.SearchModule;
using Walkman.Core.Models;

namespace Walkman.iOS.Modules.SearchModule
{
    public class SearchRouter : ISearchRouter
    {
        private readonly PlayerUtils _player;

        public ISearchPresenter SearchPresenter { get ; set ; }

        public SearchRouter(PlayerUtils player)
        {
            _player = player;

            _player.NextSong += SongChanged;
            _player.PreviousSong += SongChanged;

            _player.PlayAction += PlayAction;
            _player.PauseAction += PauseAction;
        }

        private void SongChanged(SongInfo songInfo)
        {
            SearchPresenter.SetNewSong(songInfo);
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
            SearchPresenter.SetPause(song);
        }

        private void PlayAction(SongInfo song)
        {
            SearchPresenter.SetPlay(song);
        }
    }
}

