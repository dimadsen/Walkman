using System;
using Walkman.Core.Interfaces.Models;

namespace Walkman.Core.Interfaces.ShortSongInfoModule
{
	public interface IShortSongInfoView
	{
		void ConfigureView();

		void SetColor();

        void SetNewSong(SongInfo currentSong);

		void Play();

		void Pause();
	}
}

