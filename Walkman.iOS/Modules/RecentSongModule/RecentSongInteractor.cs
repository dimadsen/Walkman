using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Interfaces.RecentSongModule;
using Walkman.Database;
using System.Linq;
using Walkman.Database.Models;
using Walkman.iOS.Utils;
using VkNet;
using Walkman.Core.Models;

namespace Walkman.iOS.Modules.RecentSongModule
{
    public class RecentSongInteractor : IRecentSongInteractor
    {
        private readonly WalkmanContext _db;
        private readonly VkApi _vkApi;

        public RecentSongInteractor(WalkmanContext db, VkApi vkApi)
        {
            _db = db;
            _vkApi = vkApi;
        }

        public IRecentSongPresenter RecentSongPresenter { get; set; }

        public async Task DeleteSongAsync(SongInfo songInfo)
        {
            var entity = await _db.RecentSongs.FirstOrDefaultAsync(x => x.SongId == songInfo.Id);
            await _db.DeleteAsync(entity);
        }

        public async Task<List<SongInfo>> GetSongsAsync()
        {
            var songsEntity = (await _db.RecentSongs.ToListAsync())
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
                    OwnerId = x.OwnerId
                })
                .Take(50)
                .ToList();

            var downloadTasks = songs.Select(async song =>
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

            await Task.WhenAll(downloadTasks);

            return songs;
        }

        public async Task SaveSongAsync(SongInfo songInfo)
        {
            var existingSong = await _db.RecentSongs.FirstOrDefaultAsync(x => x.SongId == songInfo.Id);

            if (existingSong == null)
            {
                existingSong = new SongEntity
                {
                    Album = songInfo.Album,
                    AlbumCover = songInfo.AlbumCover,
                    Artist = songInfo.Artist,
                    Duration = songInfo.Duration,
                    Name = songInfo.Name,
                    SongId = songInfo.Id,
                    SongUrl = songInfo.SongUrl,
                    CreateDate = DateTime.Now,
                    OwnerId = songInfo.OwnerId,
                    AlbumId = songInfo.AlbumId
                };
            }
            else
            {
                existingSong.CreateDate = DateTime.Now;
                await _db.DeleteAsync(existingSong);
            }

            await _db.AddAsync(existingSong);
        }
    }
}

