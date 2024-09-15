using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Walkman.Core.Interfaces.Models;

namespace Walkman.Core.Interfaces.PlayerModule
{
	public interface IPlayerInteractor
	{
        IPlayerPresenter PlayerPresenter { get; set; }
    }
}

