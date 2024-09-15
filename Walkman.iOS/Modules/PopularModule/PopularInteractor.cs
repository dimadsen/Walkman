using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkNet;
using VkNet.Enums;
using Walkman.Core.Interfaces.PopularModule;
using Walkman.Core.Models;
using System.Linq;
using VkNet.Model.Attachments;
using Walkman.Core.Interfaces.Models;
using Walkman.iOS.Utils;
using Walkman.Database;

namespace Walkman.iOS.Modules.PopularModule
{
    public class PopularInteractor : IPopularInteractor
    {
        private readonly VkApi _vkApi;
        private readonly WalkmanContext _db;

        public PopularInteractor(VkApi vkApi, WalkmanContext db)
        {
            _vkApi = vkApi;
            _db = db;
        }

        public async Task<List<СompilationInfo>> GetPopularAsync()
        {
            var randomGenres = GetRandomGenres();

            var tasks = randomGenres.Select(async genre => new
            {
                Genre = genre.Key,
                Songs = await _vkApi.Audio.GetPopularAsync(count: 20, genre: genre.Key)
            });

            var result = await Task.WhenAll(tasks);

            var songs = result.Select(x => new СompilationInfo
            {
                Name = randomGenres.First(g => g.Key == x.Genre).Value,
                Genre = (Genre)x.Genre,
                Songs = x.Songs.Where(s => s.Url != null && s.Id.HasValue && s.OwnerId.HasValue)
                .Select(s => new SongInfo
                {
                    Id = s.Id.Value,
                    Album = s.Album?.Title,
                    AlbumCover = s.Album?.Thumb?.Photo600,
                    AlbumId = s.Album?.Id,
                    Artist = s.Artist,
                    Duration = s.Duration,
                    Name = s.Title,
                    SongUrl = s.Url.AbsoluteUri,
                    OwnerId = s.OwnerId.Value
                }).ToList()
            }).ToList();

            var favorites = await _db.FavoriteSongs.ToListAsync();

            var dowloadTasks = songs.SelectMany(x => x.Songs).Select(async song =>
            {
                song.IsFavorite = favorites.FirstOrDefault(x => x.SongId == song.Id) != null;

                var downloadedSong = await _db.DownloadedSongs.FirstOrDefaultAsync(x => x.SongId == song.Id);

                song.DownloadStatus = downloadedSong?.SongData == null ? DownloadStatus.NotStarted : DownloadStatus.Сompleted;
                song.SongData = downloadedSong?.SongData;

                if (song.AlbumId.HasValue && !ImageUtils.FileExists(song.AlbumId.Value))
                {
                    await ImageUtils.DownloadFileAsync(song.AlbumCover, song.AlbumId.Value);
                }
            });

            await Task.WhenAll(dowloadTasks);

            return songs;
        }

        private Dictionary<AudioGenre, string> GetRandomGenres()
        {
            var random = new Random();

            var randomGenres = new Dictionary<AudioGenre, string>();

            for (int i = 0; i < 6; )
            {
                var v = random.Next(0, _genres.Count);

                var genre = _genres.ElementAt(v);

                if (!randomGenres.Any(x=> x.Key == genre.Key))
                {
                    randomGenres.Add(genre.Key, genre.Value);
                    i++;
                }
            }

            return randomGenres;
        }

        private Dictionary<AudioGenre, string> _genres = new Dictionary<AudioGenre, string>()
        {
            [AudioGenre.Rock] = "Рок",
            [AudioGenre.Trance] = "Транс",
            [AudioGenre.Pop] = "Популярная музыка",
            [AudioGenre.RapAndHipHop] = "Реп и хипхоп",
            [AudioGenre.DrumAndBass] = "Драм-н-бэйс",
            [AudioGenre.Ethnic] = "Этническая музыка",
            [AudioGenre.AcousticAndVocal] = "Акустическая музыка и вокал",
            [AudioGenre.Alternative] = "Альтернативная музыка",
            [AudioGenre.Classical] = "Классическая музыка",
            [AudioGenre.DanceAndHouse] = "Танцевальная и хаус музыка",
            [AudioGenre.Dubstep] = "Дабстеп",
            [AudioGenre.EasyListening] = "Лёгкая музыка",
            [AudioGenre.ElectropopAndDisco] = "Электро-поп и диско",
            [AudioGenre.IndiePop] = "Инди-поп",
            [AudioGenre.Instrumental] = "Инструментальная музыка",
            [AudioGenre.JazzAndBlues] = "Джаз и блюз",
            [AudioGenre.Metal] = "Метал",
            [AudioGenre.Other] = "Другая музыка",
            [AudioGenre.Reggae] = "Регги",
        };
    }
}

