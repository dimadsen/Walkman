using System.Collections.Generic;
using System.Threading.Tasks;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.Core.Interfaces.PopularGenreModule
{
    public interface IPopularGenrePresenter
	{
        uint Page { get; set; }

        СompilationInfo Сompilation { get; set; }

        void ConfigureView(IPopularGenreView view);

        Task SetPopularSongsAsync();

        void PlaySong(List<SongInfo> songs, int selectIndex);

        void SetNewSong(SongInfo songInfo);

        SongInfo GetCurrentSong();

        void SetPause(SongInfo song);

        void SetPlay(SongInfo song);

        PlayerStatus GetPlayerStatus();
    }
}

