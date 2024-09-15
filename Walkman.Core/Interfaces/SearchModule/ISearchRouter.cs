using System.Collections.Generic;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.Core.Interfaces.SearchModule
{
    public interface ISearchRouter
	{
        ISearchPresenter SearchPresenter { get; set; }

        PlayerStatus GetPlayerStatus();

        void PlaySong(List<SongInfo> songs, int selectIndex);

        SongInfo GetCurrentSong();
    }
}

