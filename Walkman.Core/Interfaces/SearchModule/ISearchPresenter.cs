using System.Collections.Generic;
using System.Threading.Tasks;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.Core.Interfaces.SearchModule
{
    public interface ISearchPresenter
	{
        uint Page { get; set; }
        void ConfigureView(ISearchView view);

        void PlaySong(List<SongInfo> songs, int selectIndex);

        Task SearchSongsAsync(string query);

        void SetNewSong(SongInfo songInfo);

        SongInfo GetCurrentSong();

        void SetPause(SongInfo song);

        void SetPlay(SongInfo song);

        PlayerStatus GetPlayerStatus();
    }
}

