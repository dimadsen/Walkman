using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model.Attachments;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.RecommendationModule;
using Walkman.Core.Models;
using Walkman.Database;
using Walkman.Database.Models;
using Walkman.iOS.Utils;

namespace Walkman.iOS.Modules.RecommendationModule
{
    public class RecommendationInteractor : IRecommendationInteractor
    {
        private readonly VkApi _vkApi;
        private readonly WalkmanContext _db;

        private List<string> _targetAudioIds = new List<string>();

        private List<List<Audio>> _recommendationSongs = new List<List<Audio>>();

        public RecommendationInteractor(VkApi vkApi, WalkmanContext db)
        {
            _vkApi = vkApi;
            _db = db;
        }

        public async Task ChangeRecommendationsAsync()
        {
            _recommendationSongs = new List<List<Audio>>();
            _targetAudioIds = new List<string>();

            var favoriteSongs = await _db.FavoriteSongs.Where(x => x.OwnerId != 0)
                .OrderByDescending(x => x.CreateDate).ToListAsync();

            if (favoriteSongs.Any())
            {
                var random = new Random();

                for (int i = 0; i < 5; i++)
                {
                    var v = random.Next(0, favoriteSongs.Count);

                    var song = favoriteSongs[v];

                    var targetAudio = $"{song.OwnerId}_{song.SongId}";

                    if (!_targetAudioIds.Any(x => x == targetAudio))
                        _targetAudioIds.Add(targetAudio);
                }
            }

            var tasks = _targetAudioIds.Select(async audio =>
            {
                var result = (await _vkApi.Audio.GetRecommendationsAsync(targetAudio: audio))
                .Where(x => x.Url != null && x.Id.HasValue && x.OwnerId.HasValue)
                .ToList();

                return result;
            });

            _recommendationSongs = (await Task.WhenAll(tasks)).ToList();
        }

        public async Task<List<SongInfo>> GetRecommendationsAsync(uint page)
        {
            var take = 5;
            var skip = take * (int)page;

            var recommendationSongs = _recommendationSongs.Select(audios =>
            {
                var songs = audios
                 .Skip(skip)
                 .Take(take)
                 .Select(x => new SongInfo
                 {
                     Id = x.Id.Value,
                     Album = x.Album?.Title,
                     AlbumCover = x.Album?.Thumb?.Photo600,
                     Artist = x.Artist,
                     Duration = x.Duration,
                     Name = x.Title,
                     SongUrl = x.Url.AbsoluteUri,
                     OwnerId = x.OwnerId.Value,
                     AlbumId = x.Album?.Id
                 }).ToList();

                return songs;
            })
            .SelectMany(x => x)
            .GroupBy(x => x.Id)
            .Select(x => x.First())
            .ToList();

            var downloadAlbumTasks = recommendationSongs.Select(async song =>
            {
                song.IsFavorite = (await _db.FavoriteSongs.FirstOrDefaultAsync(x => x.SongId == song.Id)) != null;

                var downloadedSong = await _db.DownloadedSongs.FirstOrDefaultAsync(x => x.SongId == song.Id);

                song.DownloadStatus = downloadedSong?.SongData == null ? DownloadStatus.NotStarted : DownloadStatus.Сompleted;
                song.SongData = downloadedSong?.SongData;

                if (song.AlbumId.HasValue && !ImageUtils.FileExists(song.AlbumId.Value))
                {
                    await ImageUtils.DownloadFileAsync(song.AlbumCover, song.AlbumId.Value);
                }
            });

            await Task.WhenAll(downloadAlbumTasks);

            return recommendationSongs;
        }
    }
}

