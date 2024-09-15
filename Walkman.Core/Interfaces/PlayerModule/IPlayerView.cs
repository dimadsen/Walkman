using System;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.Core.Interfaces.PlayerModule
{
	public interface IPlayerView
	{
		void ConfigureView();

		void SetCurrentSong(PlayerModel model);

		void SetPlay();

		void SetPause();

		void SetPlaybackPosition(double value);

		void SetDownloadSucsessful(SongInfo songInfo);

		void SetDownloadError(SongInfo songInfo);
	}
}

