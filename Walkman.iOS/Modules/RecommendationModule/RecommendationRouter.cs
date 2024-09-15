using System.Collections.Generic;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.RecommendationModule;
using Walkman.Core.Models;

namespace Walkman.iOS.Modules.RecommendationModule
{
    public class RecommendationRouter : IRecommendationRouter
	{
        private PlayerUtils _player;

        public IRecommendationPresenter RecommendationPresenter { get ; set ; }

        public RecommendationRouter(PlayerUtils player)
		{
            _player = player;

            _player.NextSong += SongChanged;
            _player.PreviousSong += SongChanged;

            _player.PlayAction += PlayAction;
            _player.PauseAction += PauseAction;
        }

        private void SongChanged(SongInfo songInfo)
        {
            RecommendationPresenter.SetNewSong(songInfo);
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
            RecommendationPresenter.SetPause(song);
        }

        private void PlayAction(SongInfo song)
        {
            RecommendationPresenter.SetPlay(song);
        }
    }
}

