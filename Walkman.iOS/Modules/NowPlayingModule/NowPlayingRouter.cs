using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.NowPlayingModule;
using Walkman.Core.Models;

namespace Walkman.iOS.Modules.NowPlayingModule
{
    public class NowPlayingRouter : INowPlayingRouter
	{
        private readonly PlayerUtils _player;

        public INowPlayingPresenter NowPlayingPresenter { get; set; }

        public NowPlayingRouter(PlayerUtils player)
		{
            _player = player;

            _player.NextSong += SongChanged;
            _player.PreviousSong += SongChanged;

            _player.PlayAction += PlayAction;
            _player.PauseAction += PauseAction;
        }

        private void PauseAction(SongInfo song)
        {
            NowPlayingPresenter.SetPause(song);
        }

        private void PlayAction(SongInfo song)
        {
            NowPlayingPresenter.SetPlay(song);
        }

        private void SongChanged(SongInfo songInfo)
        {
            NowPlayingPresenter.SetNewSong(songInfo);
        }

        public PlayerStatus GetPlayerStatus()
        {
            return _player.GetPlayerStatus();
        }
    }
}

