using System.Collections.Generic;
using Walkman.Core.Interfaces.Models;

namespace Walkman.Core.Interfaces.SearchModule
{
    public interface ISearchView
	{
		void ConfigureView();

		void SetSongs(List<SongInfo> songs);

		void SetNewSong(SongInfo songInfo);

		void SetWarningView(string message);

        void SetPlay(SongInfo song);

        void SetPause(SongInfo song);
    }
}

