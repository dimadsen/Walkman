using System.Collections.Generic;
using System.Threading.Tasks;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.RecentSongModule;
using Walkman.Core.Models;
using Walkman.iOS.Modules.NowPlayingModule;

namespace Walkman.iOS.Modules.RecentSongModule
{
    public class RecentSongRouter : IRecentSongRouter
    {
        public IRecentSongPresenter RecentSongPresenter { get; set; }

        private PlayerUtils _player;

        public RecentSongRouter(PlayerUtils player)
        {
            _player = player;

            _player.NextSong += SongChanged;
            _player.PreviousSong += SongChanged;

            _player.PlayAction += PlayAction;
            _player.PauseAction += PauseAction;

            _player.SongDidPlayToEnd += async (song) => await SongPlayToEndTime(song);
        }

        private void PauseAction(SongInfo song)
        {
            RecentSongPresenter.SetPause(song);
        }

        private void PlayAction(SongInfo song)
        {
            RecentSongPresenter.SetPlay(song);
        }

        private void SongChanged(SongInfo songInfo)
        {
            RecentSongPresenter.SetNewSong(songInfo);
        }

        private async Task SongPlayToEndTime(SongInfo song)
        {
            await RecentSongPresenter.SaveSongAsync(song);
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

