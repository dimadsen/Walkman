using System;
using System.Threading;
using AVFoundation;
using Foundation;
using MediaPlayer;
using UIKit;
using VkNet.Model;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;

namespace Walkman.iOS.Utils
{
    public class LockScreenUtils
    {
        private readonly PlayerUtils _player;
        private SongInfo _currentSong;

        public LockScreenUtils(PlayerUtils player)
        {
            var commandCenter = MPRemoteCommandCenter.Shared;

            commandCenter.PlayCommand.Enabled = true;
            commandCenter.PauseCommand.Enabled = true;
            commandCenter.ChangePlaybackPositionCommand.Enabled = true;
            commandCenter.PreviousTrackCommand.Enabled = true;
            commandCenter.NextTrackCommand.Enabled = true;

            _player = player;

            _player.PlayAction += Play;
            _player.PauseAction += (SongInfo song) => ChangeNowPlaying(0, _player.GetCurrentTime());
            _player.NextSong += Next;
            _player.PreviousSong += Previous;
            _player.ChangePlaybackPositionAction += ChangePlaybackPositionAction;
            _player.ChangePlayerStatus += ChangePlayerStatus;

            AddChangePlaybackPositionCommand();
            AddNextSongCommand();
            AddPauseCommand();
            AddPlayCommand();
            AddPreviousSongCommand();
        }

        private void ChangePlaybackPositionAction(double value)
        {
            var rate = _player.GetPlayerStatus() == PlayerStatus.Playing ? 1 : 0;

           ChangeNowPlaying(rate, value);
        }

        private void ChangePlayerStatus(PlayerStatus status)
        {
            var rate = status == PlayerStatus.Playing ? 1 : 0;

            ChangeNowPlaying(rate, _player.GetCurrentTime());
        }

        private void Play(SongInfo song)
        {
            if (song.Id != _currentSong?.Id)
            {
                _currentSong = song;
                SetNowPlaying(song);
            }
            else
                ChangeNowPlaying(1, _player.GetCurrentTime());
        }

        private void Previous(SongInfo song)
        {
            _currentSong = song;

            SetNowPlaying(song);
        }

        private void Next(SongInfo song)
        {
            _currentSong = song;

            SetNowPlaying(song);
        }

        private void SetNowPlaying(SongInfo song)
        {
            var albumImage = song.AlbumId.HasValue && ImageUtils.FileExists(song.AlbumId.Value) ?
                    UIImage.LoadFromData(ImageUtils.GetFileFromCache(song.AlbumId.Value)) :
                    UIImage.FromFile("cd_player");

            MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = new MPNowPlayingInfo
            {
                AlbumTitle = song.Album,
                Artist = song.Artist,
                Title = song.Name,
                PlaybackDuration = song.Duration,
                PlaybackRate = 1,
                Artwork = new MPMediaItemArtwork(albumImage),
                MediaType = MPNowPlayingInfoMediaType.Audio,
                ElapsedPlaybackTime = _player.GetCurrentTime()
            };

            var commandCenter = MPRemoteCommandCenter.Shared;

            commandCenter.NextTrackCommand.Enabled = !_player.IsLastSong();
            commandCenter.PreviousTrackCommand.Enabled = !_player.IsFirstSong();
        }

        private void ChangeNowPlaying(double rate, double currentTime)
        {
            var nowPlaying = MPNowPlayingInfoCenter.DefaultCenter.NowPlaying;

            nowPlaying.ElapsedPlaybackTime = Math.Round(currentTime);
            nowPlaying.PlaybackRate = rate;

            MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = nowPlaying;

            var commandCenter = MPRemoteCommandCenter.Shared;
            commandCenter.NextTrackCommand.Enabled = !_player.IsLastSong();
        }

        #region Commands

        private void AddPreviousSongCommand()
        {
            var commandCenter = MPRemoteCommandCenter.Shared;

            commandCenter.PreviousTrackCommand.AddTarget((command) =>
            {
                _player.Previous();
                return MPRemoteCommandHandlerStatus.Success;
            });
        }

        private void AddNextSongCommand()
        {
            var commandCenter = MPRemoteCommandCenter.Shared;

            commandCenter.NextTrackCommand.AddTarget((command) =>
            {
                _player.Next();
                return MPRemoteCommandHandlerStatus.Success;
            });
        }

        private void AddChangePlaybackPositionCommand()
        {
            var commandCenter = MPRemoteCommandCenter.Shared;

            commandCenter.ChangePlaybackPositionCommand.AddTarget((command) =>
            {
                var positionTime = Math.Round(((MPChangePlaybackPositionCommandEvent)command).PositionTime);

                _player.ChangePlaybackPosition(positionTime);

                return MPRemoteCommandHandlerStatus.Success;
            });
        }

        private void AddPlayCommand()
        {
            var commandCenter = MPRemoteCommandCenter.Shared;

            commandCenter.PlayCommand.AddTarget((command) =>
            {
                _player.PlayPause();
                return MPRemoteCommandHandlerStatus.Success;
            });
        }

        private void AddPauseCommand()
        {
            var commandCenter = MPRemoteCommandCenter.Shared;

            commandCenter.PauseCommand.AddTarget((command) =>
            {
                _player.PlayPause();
                return MPRemoteCommandHandlerStatus.Success;
            });
        }

        #endregion
    }
}

