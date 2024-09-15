using System;
using System.Collections.Generic;
using AVFoundation;
using Foundation;
using MediaPlayer;
using UIKit;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.ShortSongInfoModule;
using Walkman.iOS.ViewControllers;
using Xamarin.Essentials;

namespace Walkman.iOS.Modules.ShortSongInfoModule
{
	public class ShortSongInfoPresenter : IShortSongInfoPresenter
    {
        private IShortSongInfoRouter _shortSongInfoRouter;
        private PlayerUtils _player;
        private IShortSongInfoView _view;

        public ShortSongInfoPresenter(IShortSongInfoRouter shortSongInfoRouter,PlayerUtils player)
        {
            _shortSongInfoRouter = shortSongInfoRouter;
            _player = player;

            _shortSongInfoRouter.ShortSongInfoPresenter = this;          
        }

        public void ConfigureView(IShortSongInfoView view)
        {
            _view = view;
            _view.ConfigureView();
        }

        public void SetNewSong(SongInfo songInfo)
        {
            _view.SetNewSong(songInfo);
        }

        public void SetPlay()
        {
            _view.Play();
        }

        public void SetPause()
        {
            _view.Pause();
        }

        public void ShowPlayer()
        {
            if (_player.GetCurrentSong() != null)
                _shortSongInfoRouter.ShowPlayer(_view);
        }

        public void ChangePlay()
        {
            _player.PlayPause();
        }
    }
}

