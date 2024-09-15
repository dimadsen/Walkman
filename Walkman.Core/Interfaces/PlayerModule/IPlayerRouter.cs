using System;
using System.Threading.Tasks;
using Walkman.Core.Interfaces.Models;

namespace Walkman.Core.Interfaces.PlayerModule
{
	public interface IPlayerRouter
	{
        IPlayerPresenter PlayerPresenter { get; set; }
        Task<bool> DownloadSongAsync(SongInfo songInfo);
    }
}

