using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.Core.Interfaces.PopularGenreModule
{
	public interface IPopularGenreInteractor
	{
        IPopularGenrePresenter Presenter { get; set; }

        Task<List<SongInfo>> GetPopularSongsAsync(Genre genre, uint page);
    }
}

