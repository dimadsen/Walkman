using System;
using System.Collections.Generic;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.Core.Interfaces.NowPlayingModule
{
	public interface INowPlayingPresenter
	{
        List<SongInfo> Songs { get; set; }

        void ConfigureView(INowPlayingView view);

		void SetSongs();

        SongInfo GetCurrentSong();

        void PlaySong(List<SongInfo> songs, int selectIndex);

		void MixSongs();

		void SetNewSong(SongInfo song);

		void Repeat();

		bool IsRepeat();

		void SetPause(SongInfo song);

		void SetPlay(SongInfo song);

        PlayerStatus GetPlayerStatus();
    }
}

