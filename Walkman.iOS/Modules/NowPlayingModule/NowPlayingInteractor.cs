using System;
using System.Collections.Generic;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.NowPlayingModule;

namespace Walkman.iOS.Modules.NowPlayingModule
{
    public class NowPlayingInteractor : INowPlayingInteractor
    {
        private readonly PlayerUtils _playerUtils;

        public NowPlayingInteractor(PlayerUtils playerUtils)
        {
            _playerUtils = playerUtils;
        }

        public INowPlayingPresenter NowPlayingPresenter { get; set ; }

        public SongInfo GetCurrentSong()
        {
            return _playerUtils.GetCurrentSong();
        }

        public bool IsRepeat()
        {
            return _playerUtils.Repeat;
        }

        public List<SongInfo> GetSongs()
        {
            return _playerUtils.GetSongs();
        }

        public void PlaySong(List<SongInfo> songs, int selectIndex)
        {
            _playerUtils.SetSongs(songs, selectIndex);
            _playerUtils.PlayPause();
        }

        public void Repeat()
        {
            _playerUtils.Repeat = !_playerUtils.Repeat;
        }
    }
}

