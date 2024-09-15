using System.Collections.Generic;
using System.Threading.Tasks;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.Core.Interfaces.PopularModule
{
    public interface IPopularPresenter
	{
        void ConfigureView(IPopularView view);

        Task GetPopularAsync();

		void PlaySong(List<SongInfo> songs, int selectIndex);

		void PrepareForSegue(СompilationInfo сompilation);

        SongInfo GetCurrentSong();

        void SetPause(SongInfo song);

        void SetPlay(SongInfo song);

        PlayerStatus GetPlayerStatus();

        void SetNewSong(SongInfo songInfo);
    }
}

