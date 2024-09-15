using System.Collections.Generic;
using Walkman.Core.Interfaces.DownloadSongModule;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.iOS.Modules.DownloadSongModule
{
    public class DownloadSongRouter : IDownloadSongRouter
	{
        private readonly PlayerUtils _player;

        public IDownloadSongPresenter Presenter { get ; set ; }

        public DownloadSongRouter(PlayerUtils player)
		{
            _player = player;

            _player.NextSong += SongChanged;
            _player.PreviousSong += SongChanged;

            _player.PlayAction += PlayAction;
            _player.PauseAction += PauseAction;
        }

        private void PauseAction(SongInfo song)
        {
            Presenter.SetPause(song);
        }

        private void PlayAction(SongInfo song)
        {
            Presenter.SetPlay(song);
        }

        private void SongChanged(SongInfo songInfo)
        {
            Presenter.SetNewSong(songInfo);
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
    }
}

