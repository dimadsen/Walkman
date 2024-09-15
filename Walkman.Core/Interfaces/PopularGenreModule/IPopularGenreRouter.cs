using System.Collections.Generic;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.Core.Interfaces.PopularGenreModule
{
    public interface IPopularGenreRouter
	{
        IPopularGenrePresenter PopularGenrePresenter { get; set; }

        PlayerStatus GetPlayerStatus();

        void PlaySong(List<SongInfo> songs, int selectIndex);

        SongInfo GetCurrentSong();
    }
}

