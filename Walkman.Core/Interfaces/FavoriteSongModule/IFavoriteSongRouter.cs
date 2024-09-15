using System.Collections.Generic;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.Core.Interfaces.FavoriteSongModule
{
    public interface IFavoriteSongRouter
	{
        public IFavoriteSongPresenter FavoriteSongPresenter { get; set; }

        PlayerStatus GetPlayerStatus();

        void PlaySong(List<SongInfo> songs, int selectIndex);

        SongInfo GetCurrentSong();
    }
}

