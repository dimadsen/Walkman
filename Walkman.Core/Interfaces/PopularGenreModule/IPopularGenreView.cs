using System;
using System.Collections.Generic;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.Core.Interfaces.PopularGenreModule
{
	public interface IPopularGenreView
	{
		void ConfigureView(СompilationInfo сompilation);

        void SetSongs(List<SongInfo> songs);

        void SetNewSong(SongInfo songInfo);

        void SetWarningView(string message);

        void SetPlay(SongInfo song);

        void SetPause(SongInfo song);
    }
}

