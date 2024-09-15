using System;
using System.Collections.Generic;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.Core.Interfaces.PopularModule
{
	public interface IPopularView
	{
        void ConfigureView();

        void SetCompilations(List<СompilationInfo> сompilations);

        void SetPlay(SongInfo song);

        void SetPause(SongInfo song);

        void SetNewSong(SongInfo songInfo);
    }
}

