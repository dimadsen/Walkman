using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Walkman.Core.Interfaces.Models;

namespace Walkman.Core.Interfaces.RecentSongModule
{
	public interface IRecentSongInteractor
	{
        public IRecentSongPresenter RecentSongPresenter { get; set; }

        Task<List<SongInfo>> GetSongsAsync();

        Task SaveSongAsync(SongInfo songInfo);

        Task DeleteSongAsync(SongInfo songInfo);
    }
}

