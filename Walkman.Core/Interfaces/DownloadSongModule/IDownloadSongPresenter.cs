using System.Collections.Generic;
using System.Threading.Tasks;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.Core.Interfaces.DownloadSongModule
{
    public interface IDownloadSongPresenter
	{
        List<SongInfo> Songs { get; set; }

        void ConfigureView(IDownloadSongView view);

        void PlaySong(List<SongInfo> songs, int selectIndex);

        void SetNewSong(SongInfo songInfo);

        SongInfo GetCurrentSong();

        void SetWarningView();

        Task SetSongsAsync();

        Task<bool> DownloadAsync(SongInfo songInfo);

        Task DeleteSongAsync(SongInfo songInfo);

        PlayerStatus GetPlayerStatus();

        void SetPause(SongInfo song);

        void SetPlay(SongInfo song);
    }
}

