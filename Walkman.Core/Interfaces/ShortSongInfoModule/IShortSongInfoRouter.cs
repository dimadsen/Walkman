using System;
using System.Collections.Generic;
using Walkman.Core.Interfaces.Models;

namespace Walkman.Core.Interfaces.ShortSongInfoModule
{
	public interface IShortSongInfoRouter
	{
		IShortSongInfoPresenter  ShortSongInfoPresenter { get; set; }

		void ShowPlayer(IShortSongInfoView source);
    }
}

