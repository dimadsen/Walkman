using System.Collections.Generic;
using System.Threading.Tasks;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.Core.Interfaces.RecentSongModule
{
    public interface IRecentSongPresenter
	{
        void ConfigureView(IRecentSongView view);

        Task SearchSongsAsync();

        void PlaySong(List<SongInfo> songs, int selectIndex);

        void SetNewSong(SongInfo songInfo);

        Task SaveSongAsync(SongInfo songInfo);

        Task DeleteSongAsync(SongInfo songInfo);

        void SetWarningView();

        void SetPause(SongInfo song);

        void SetPlay(SongInfo song);

        SongInfo GetCurrentSong();

        PlayerStatus GetPlayerStatus();
    }
}

