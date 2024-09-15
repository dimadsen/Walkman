using System.Collections.Generic;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.Core.Interfaces.PopularModule
{
    public interface IPopularRouter
	{
        public IPopularPresenter PopularPresenter { get; set; }

        void PrepareForGenre(СompilationInfo сompilation);

        PlayerStatus GetPlayerStatus();

        void PlaySong(List<SongInfo> songs, int selectIndex);

        SongInfo GetCurrentSong();
    }
}

