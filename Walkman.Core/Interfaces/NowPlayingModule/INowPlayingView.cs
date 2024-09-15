using System;
using System.Collections.Generic;
using Walkman.Core.Interfaces.Models;

namespace Walkman.Core.Interfaces.NowPlayingModule
{
	public interface INowPlayingView
	{
		void ConfigureView();

		void SetSongs();

        void SetNewSong(SongInfo songInfo);

		void SetPlay(SongInfo song);

		void SetPause(SongInfo song);
    }
}

