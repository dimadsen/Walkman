using Walkman.Core.Interfaces.Models;

namespace Walkman.Core.Interfaces.FavoriteSongModule
{
    public interface IFavoriteSongView
	{
        void ConfigureView();

        void SetSongs();

        void SetNewSong(SongInfo songInfo);

        void SetWarningView(string message);

        void UpdateView(SongInfo songInfo);

        void SetPlay(SongInfo song);

        void SetPause(SongInfo song);

        void SelectSong();
    }
}

