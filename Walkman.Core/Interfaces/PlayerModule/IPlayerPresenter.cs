using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.Core.Interfaces.PlayerModule
{
	public interface IPlayerPresenter
	{
	    event Action<SongInfo> SetToFavorite;

        void ConfigureView(IPlayerView playerView);

		void SetCurrentSong(SongInfo songInfo, MoveSong move);

		void SetPlay();

		void SetPause();

		void SetPlaybackPosition(double value, bool canPlay);


        void ChangePlayPause();

		void ChangeNextSong();

		void ChangePreviousSong();

        void ChangePlaybackPosition(double value);

		void SetFavorite(SongInfo song);

		void DownloadSong(SongInfo song);
    }
}

