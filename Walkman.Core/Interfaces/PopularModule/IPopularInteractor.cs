using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Walkman.Core.Models;

namespace Walkman.Core.Interfaces.PopularModule
{
	public interface IPopularInteractor
	{
		Task<List<СompilationInfo>> GetPopularAsync();
	}
}

