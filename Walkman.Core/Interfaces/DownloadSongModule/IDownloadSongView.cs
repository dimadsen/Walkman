using System;
using System.Collections.Generic;
using Walkman.Core.Interfaces.Models;

namespace Walkman.Core.Interfaces.DownloadSongModule
{
	public interface IDownloadSongView
	{
        void ConfigureView();

        void SetSongs(List<SongInfo> songs);

        void SetNewSong(SongInfo songInfo);

        void SetWarningView(string message);

        void InsertSong(SongInfo songInfo);

        void SetPlay(SongInfo song);

        void SetPause(SongInfo song);
    }
}

