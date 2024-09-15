using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Walkman.Core.Interfaces.Models;

namespace Walkman.Core.Interfaces.DownloadSongModule
{
	public interface IDownloadSongInteractor
	{
        IDownloadSongPresenter Presenter { get; set; }

        Task DownloadAsync(SongInfo songInfo);

        Task<List<SongInfo>> GetSongsAsync();

        Task DeleteSongAsync(SongInfo songInfo);
    }
}

