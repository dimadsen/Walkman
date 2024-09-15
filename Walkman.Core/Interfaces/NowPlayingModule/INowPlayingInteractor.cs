using System;
using System.Collections.Generic;
using Walkman.Core.Interfaces.Models;

namespace Walkman.Core.Interfaces.NowPlayingModule
{
	public interface INowPlayingInteractor
	{
		INowPlayingPresenter NowPlayingPresenter { get; set; }

		List<SongInfo> GetSongs();

		SongInfo GetCurrentSong();

        void PlaySong(List<SongInfo> songs, int selectIndex);

		void Repeat();

		bool IsRepeat();
    }
}

