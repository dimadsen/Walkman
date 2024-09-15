using System;
using System.Threading.Tasks;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;
using System.Collections.Generic;

namespace Walkman.Core.Interfaces.RecommendationModule
{
	public interface IRecommendationInteractor
	{
		Task<List<SongInfo>> GetRecommendationsAsync(uint page);

		Task ChangeRecommendationsAsync();
	}
}

