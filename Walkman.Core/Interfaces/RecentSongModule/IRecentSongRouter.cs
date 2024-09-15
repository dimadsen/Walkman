using System;
using System.Collections.Generic;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.Core.Interfaces.RecentSongModule
{
	public interface IRecentSongRouter
	{
        public IRecentSongPresenter RecentSongPresenter { get; set; }

        PlayerStatus GetPlayerStatus();

        void PlaySong(List<SongInfo> songs, int selectIndex);

        SongInfo GetCurrentSong();
    }
}

