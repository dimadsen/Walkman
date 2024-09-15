﻿using System;
using System.Collections.Generic;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.Core.Interfaces.DownloadSongModule
{
	public interface IDownloadSongRouter
	{
		IDownloadSongPresenter Presenter { get; set; }

        PlayerStatus GetPlayerStatus();

        void PlaySong(List<SongInfo> songs, int selectIndex);

        SongInfo GetCurrentSong();
    }
}

