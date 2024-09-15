using System;
using System.Collections.Generic;
using Walkman.Core.Interfaces.Models;

namespace Walkman.Core.Interfaces.ShortSongInfoModule
{
	public interface IShortSongInfoPresenter
	{
        void ConfigureView(IShortSongInfoView view);

		void ShowPlayer();

		void ChangePlay();

		void SetNewSong(SongInfo songInfo);

		void SetPlay();

		void SetPause();
    }
}

