using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkNet;
using VkNet.Enums;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.PopularGenreModule;
using Walkman.Core.Models;
using Walkman.Database;
using Walkman.iOS.Utils;

namespace Walkman.iOS.Modules.PopularGenreModule
{
	public class PopularGenreInteractor : IPopularGenreInteractor
	{
        private readonly VkApi _vkApi;
        private readonly WalkmanContext _db;

        public PopularGenreInteractor(VkApi vkApi, WalkmanContext db)
		{
            _vkApi = vkApi;
            _db = db;
        }

        public IPopularGenrePresenter Presenter { get ; set ; }

        public async Task<List<SongInfo>> GetPopularSongsAsync(Genre genre, uint page)
        {
            var songsVk = await _vkApi.Audio.GetPopularAsync(count: 20, genre: (AudioGenre)genre, offset: 20 * page);

            var songs = songsVk.Where(x => x.Url != null && x.Id.HasValue && x.OwnerId.HasValue)
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

            var favorites = await _db.FavoriteSongs.ToListAsync();

            var tasks = songs.Select(async song =>
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

            await Task.WhenAll(tasks);

            return songs;
        }
    }
}

