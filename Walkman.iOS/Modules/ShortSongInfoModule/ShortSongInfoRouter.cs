using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using UIKit;
using Walkman.Core.Interfaces.PlayerModule;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.ShortSongInfoModule;
using Walkman.iOS.ViewControllers;
using Walkman.iOS.Modules.PlayerModule;
using Walkman.Core.Models;

namespace Walkman.iOS.Modules.ShortSongInfoModule
{
	public class ShortSongInfoRouter : IShortSongInfoRouter
	{
        private PlayerUtils _player;

        public IShortSongInfoPresenter ShortSongInfoPresenter { get; set; }

		public ShortSongInfoRouter(PlayerUtils player)
		{
            _player = player;

            _player.PlayAction += PlayAction;
            _player.PauseAction += PauseAction;
            _player.NextSong += SongChanged;
            _player.PreviousSong += SongChanged;

            _player.ChangePlayerStatus += ChangePlayerStatus;
        }

        private void ChangePlayerStatus(PlayerStatus obj)
        {
            if (obj == PlayerStatus.Paused)
                ShortSongInfoPresenter.SetPause();
            else
                ShortSongInfoPresenter.SetPlay();
        }

        private void PlayAction(SongInfo song)
        {
            ShortSongInfoPresenter.SetNewSong(song);
            ShortSongInfoPresenter.SetPlay();
        }

        private void PauseAction(SongInfo song)
        {
            ShortSongInfoPresenter.SetNewSong(song);
            ShortSongInfoPresenter.SetPause();
        }

        public void ShowPlayer(IShortSongInfoView source)
        {
            var destController = UIStoryboard.FromName("Main", null).InstantiateViewController(nameof(SegmentedViewController));

            destController.ModalPresentationStyle = UIModalPresentationStyle.PageSheet;

            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(destController, true, null);
        }

        private void SongChanged(SongInfo songInfo)
        {
            ShortSongInfoPresenter.SetNewSong(songInfo);
        }
    }
}

