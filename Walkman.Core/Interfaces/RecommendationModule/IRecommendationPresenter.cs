using System.Collections.Generic;
using System.Threading.Tasks;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.Core.Interfaces.RecommendationModule
{
    public interface IRecommendationPresenter
	{
        uint Page { get; set; }

        void ConfigureView(IRecommendationView view);

        void PlaySong(List<SongInfo> songs, int selectIndex);

        Task SetRecommendationAsync();

        Task ChangeRecommendationsAsync();

        void SetNewSong(SongInfo songInfo);

        SongInfo GetCurrentSong();

        void SetPause(SongInfo song);

        void SetPlay(SongInfo song);

        PlayerStatus GetPlayerStatus();
    }
}

