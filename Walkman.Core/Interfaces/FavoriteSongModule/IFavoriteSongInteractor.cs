using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.RecentSongModule;

namespace Walkman.Core.Interfaces.FavoriteSongModule
{
	public interface IFavoriteSongInteractor
	{
        public IFavoriteSongPresenter FavoriteSongPresenter { get; set; }

        Task<List<SongInfo>> GetSongsAsync();

        Task SaveSongAsync(SongInfo songInfo);

        Task DeleteSongAsync(SongInfo songInfo);
    }
}

