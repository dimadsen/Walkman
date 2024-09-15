using System;
using System.Collections.Generic;
using System.Linq;
using AVFoundation;
using CoreMedia;
using Foundation;
using MediaPlayer;
using UIKit;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;
using Walkman.iOS.Utils;

namespace Walkman.iOS
{
    public class PlayerUtils : NSObject
    {
        private readonly AVPlayer _player;

        private List<SongInfo> _songs;
        private SongInfo _currentSong;

        public bool Repeat { get; set; }

        public event Action<SongInfo> SongDidPlayToEnd;

        public event Action<SongInfo> NextSong;
        public event Action<SongInfo> PreviousSong;

        public event Action<SongInfo> PlayAction;
        public event Action<SongInfo> PauseAction;

        public event Action<double> ChangePlaybackPositionAction;

        public event Action<PlayerStatus> ChangePlayerStatus;

        public PlayerUtils()
        {
            AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
            AVAudioSession.SharedInstance().SetActive(true);

            _player = new AVPlayer();

            SetPlayToEndNotification();
            SetInterruptionNotification();

            _player.AddObserver(this, "timeControlStatus", NSKeyValueObservingOptions.New, (IntPtr)null);
        }

        public void SetSongs(List<SongInfo> songs, int selectedSong)
        {
            _songs = songs; //Todo: подумать о проверке на пустой список

            _currentSong = _songs[selectedSong];

            var asset = new AVUrlAsset(GetFileUrl(_currentSong));
            var playerItem = new AVPlayerItem(asset);

            _player.ReplaceCurrentItemWithPlayerItem(playerItem);

            _player.Pause();

            NextSong?.Invoke(_currentSong);
        }

        public void Next()
        {
            try
            {
                var currentIndex = _songs.IndexOf(_currentSong);

                _currentSong = _songs[currentIndex + 1];

                var asset = new AVUrlAsset(GetFileUrl(_currentSong));
                var playerItem = new AVPlayerItem(asset);

                _player.ReplaceCurrentItemWithPlayerItem(playerItem);

                NextSong?.Invoke(_currentSong);
            }
            catch (ArgumentOutOfRangeException)
            {
                _player.Seek(CMTime.Zero);
                _player.Pause();

                PauseAction?.Invoke(_currentSong);
            }
        }

        public void Previous()
        {
            try
            {
                var currentIndex = _songs.IndexOf(_currentSong);

                _currentSong = _songs[currentIndex - 1];
            }
            catch (ArgumentOutOfRangeException)
            {
                _currentSong = _songs.FirstOrDefault();
            }

            var asset = new AVUrlAsset(GetFileUrl(_currentSong));
            var playerItem = new AVPlayerItem(asset);

            _player.ReplaceCurrentItemWithPlayerItem(playerItem);

            PreviousSong?.Invoke(_currentSong);
        }

        public void PlayPause()
        {
            if (_player.TimeControlStatus == AVPlayerTimeControlStatus.Playing)
            {
                _player.Pause();

                PauseAction?.Invoke(_currentSong);
            }
            else
            {
                _player.Play();

                PlayAction?.Invoke(_currentSong);
            }
        }

        public double GetCurrentTime()
        {
            return _player.CurrentItem.Duration.IsNumeric ? _player.CurrentTime.Seconds : 0;
        }

        public PlayerStatus GetPlayerStatus()
        {
            return (PlayerStatus)_player.TimeControlStatus;
        }

        public SongInfo GetCurrentSong()
        {
            return _currentSong;
        }

        public bool IsLastSong()
        {
            var lastSong = _songs.LastOrDefault();

            return _currentSong?.Id == lastSong.Id;
        }

        public bool IsFirstSong()
        {
            var firstSong = _songs.FirstOrDefault();

            return _currentSong?.Id == firstSong.Id;
        }

        public string GetSongStatistics()
        {
            var index = _songs.IndexOf(_currentSong);

            return $"{index + 1} / {_songs.Count}";
        }

        public void ChangePlaybackPosition(double currentTime)
        {
            _player.Seek(new CMTime((long)currentTime * 60000, 60000));

            ChangePlaybackPositionAction?.Invoke(currentTime);
        }

        public List<SongInfo> GetSongs()
        {
            return _songs;
        }

        private void SetPlayToEndNotification()
        {
            AVPlayerItem.Notifications.ObserveDidPlayToEndTime((sender, args) =>
            {
                try
                {
                    SongDidPlayToEnd?.Invoke(_currentSong);

                    if (!Repeat)
                    {
                        var currentIndex = _songs.IndexOf(_currentSong);

                        _currentSong = _songs[currentIndex + 1];
                        var asset = new AVUrlAsset(GetFileUrl(_currentSong));
                        var playerItem = new AVPlayerItem(asset);

                        _player.ReplaceCurrentItemWithPlayerItem(playerItem);

                        _player.Play();
                        NextSong?.Invoke(_currentSong);
                    }
                    else
                    {
                        _player.Seek(CMTime.Zero);
                        _player.Play();
                    }

                }
                catch (ArgumentOutOfRangeException)
                {
                    _player.Seek(CMTime.Zero);
                    _player.Pause();

                    PauseAction?.Invoke(_currentSong);
                }
            });
        }

        private void SetInterruptionNotification()
        {
            NSNotificationCenter.DefaultCenter.AddObserver(AVAudioSession.InterruptionNotification, (notification) =>
            {
                if (_player.Status == AVPlayerStatus.ReadyToPlay)
                {
                    var interruptionType = (AVAudioSessionInterruptionType)int.Parse(notification.UserInfo.ValueForKey(new NSString("AVAudioSessionInterruptionTypeKey")).ToString());

                    if (interruptionType == AVAudioSessionInterruptionType.Began)
                    {
                        _player.Pause();

                        PauseAction?.Invoke(_currentSong);
                    }
                    else
                    {
                        _player.Play();
                        PlayAction?.Invoke(_currentSong);
                    }
                }
            });
        }

        public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
        {
            var observedVal = new NSObservedChange(change);

            var status = (PlayerStatus)int.Parse(observedVal.NewValue.ToString());

            ChangePlayerStatus?.Invoke(status);
        }

        private NSUrl GetFileUrl(SongInfo currentSong)
        {
            if (_currentSong.DownloadStatus == DownloadStatus.Сompleted)
            {
                var tempPath = NSFileManager.DefaultManager.GetTemporaryDirectory().RelativePath + "/currentSong.mp3";
                NSFileManager.DefaultManager.CreateFile(tempPath, NSData.FromArray(_currentSong.SongData), new NSFileAttributes());

                return NSUrl.FromFilename(tempPath);
            }
            else
            {
                return new NSUrl(currentSong.SongUrl);
            }
        }
    }
}

