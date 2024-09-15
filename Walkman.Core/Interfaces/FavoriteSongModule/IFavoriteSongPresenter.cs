using System.Collections.Generic;
using System.Threading.Tasks;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.Core.Interfaces.FavoriteSongModule
{
    public interface IFavoriteSongPresenter
	{
        List<SongInfo> Songs { get; set; }

        void ConfigureView(IFavoriteSongView view);

        void PlaySong(List<SongInfo> songs, int selectIndex);

        Task SearchSongsAsync();

        void SetNewSong(SongInfo songInfo);

        SongInfo GetCurrentSong();

        Task AddSongAsync(SongInfo songInfo);

        Task DeleteSongAsync(SongInfo songInfo);

        void SetWarningView();

        void SetPause(SongInfo song);

        void SetPlay(SongInfo song);

        PlayerStatus GetPlayerStatus();

        void SelectSong();
    }
}

