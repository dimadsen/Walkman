using System;
using Walkman.Core.Models;

namespace Walkman.Core.Interfaces.NowPlayingModule
{
	public interface INowPlayingRouter
	{
        INowPlayingPresenter NowPlayingPresenter { get; set; }

        PlayerStatus GetPlayerStatus();
    }
}

