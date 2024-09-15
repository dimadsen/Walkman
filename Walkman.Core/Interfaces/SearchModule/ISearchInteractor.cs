using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Walkman.Core.Interfaces.Models;
namespace Walkman.Core.Interfaces.SearchModule
{
	public interface ISearchInteractor
	{
		ISearchPresenter SearchPresenter { get; set; }

		Task<List<SongInfo>> SearchSongsAsync(string query, uint page);
	}
}

