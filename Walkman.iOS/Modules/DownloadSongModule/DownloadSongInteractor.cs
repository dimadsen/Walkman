using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Walkman.Core.Interfaces.DownloadSongModule;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;
using Walkman.Database;
using Walkman.Database.Models;
using Walkman.iOS.Infrastructure;
using Walkman.iOS.Infrastructure.Request;
using Walkman.iOS.Utils;

namespace Walkman.iOS.Modules.DownloadSongModule
{
	public class DownloadSongInteractor : IDownloadSongInteractor
	{
        private readonly WalkmanContext _db;
        private readonly IDownloaderClient _client;

        public DownloadSongInteractor(WalkmanContext db, IDownloaderClient client)
		{
            _db = db;
            _client = client;
        }

        public IDownloadSongPresenter Presenter { get ; set ; }

        public async Task DeleteSongAsync(SongInfo songInfo)
        {
            var entity = await _db.DownloadedSongs.FirstOrDefaultAsync(x => x.SongId == songInfo.Id);
            await _db.DeleteAsync(entity);

            songInfo.DownloadStatus = DownloadStatus.Сompleted;
        }

        public async Task DownloadAsync(SongInfo songInfo)
        {
            var request = new DownloadSongRequest
            {
                SongId = songInfo.Id,
                Url = songInfo.SongUrl,
                Type = SongType.mp3
            };

            var response = await _client.DownloadSongAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsByteArrayAsync();

                var existingSong = await _db.DownloadedSongs.FirstOrDefaultAsync(x => x.SongId == songInfo.Id);

                if (existingSong == null)
                {
                    existingSong = new DownloadedSongEntity
                    {
                        Album = songInfo.Album,
                        AlbumCover = songInfo.AlbumCover,
                        AlbumId = songInfo.AlbumId,
                        Artist = songInfo.Artist,
                        Duration = songInfo.Duration,
                        Name = songInfo.Name,
                        SongId = songInfo.Id,
                        SongUrl = songInfo.SongUrl,
                        CreateDate = DateTime.Now,
                        OwnerId = songInfo.OwnerId,
                        SongData = data
                    };
                }

                await _db.AddAsync(existingSong);

                songInfo.SongData = data;
                songInfo.DownloadStatus = DownloadStatus.Сompleted;
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();

                var message = JsonConvert.DeserializeAnonymousType(error, new { Error = "" });

                throw new Exception(message.Error);
            }
        }

        public async Task<List<SongInfo>> GetSongsAsync()
        {
            var songsEntity = (await _db.DownloadedSongs.ToListAsync())
                .OrderByDescending(x => x.CreateDate)
                .ToList();

            var songs = songsEntity
                .Select(x => new SongInfo
                {
                    Album = x.Album,
                    AlbumCover = x.AlbumCover,
                    AlbumId = x.AlbumId,
                    Artist = x.Artist,
                    Duration = x.Duration,
                    Name = x.Name,
                    SongUrl = x.SongUrl,
                    Id = x.SongId,
                    OwnerId = x.OwnerId,
                    SongData = x.SongData,
                    DownloadStatus = DownloadStatus.Сompleted
                })
                .ToList();

            var downloadTasks = songs.Select(async song =>
            {
                song.IsFavorite = (await _db.FavoriteSongs.FirstOrDefaultAsync(x => x.SongId == song.Id)) != null;

                if (song.AlbumId.HasValue && !ImageUtils.FileExists(song.AlbumId.Value))
                {
                    await ImageUtils.DownloadFileAsync(song.AlbumCover, song.AlbumId.Value);
                }
            });

            await Task.WhenAll(downloadTasks);

            return songs;
        }
    }
}

