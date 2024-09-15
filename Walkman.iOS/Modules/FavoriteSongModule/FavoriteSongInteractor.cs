using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkNet;
using Walkman.Core.Interfaces.FavoriteSongModule;
using Walkman.Core.Interfaces.Models;
using Walkman.Core.Models;
using Walkman.Database;
using Walkman.Database.Models;
using Walkman.iOS.Utils;

namespace Walkman.iOS.Modules.FavoriteSongModule
{
    public class FavoriteSongInteractor : IFavoriteSongInteractor
    {
        private readonly WalkmanContext _db;
        private readonly VkApi _vkApi;

        public FavoriteSongInteractor(WalkmanContext db, VkApi vkApi)
        {
            _db = db;
            _vkApi = vkApi;
        }

        public IFavoriteSongPresenter FavoriteSongPresenter { get; set; }

        public async Task DeleteSongAsync(SongInfo songInfo)
        {
            var entity = await _db.FavoriteSongs.FirstOrDefaultAsync(x => x.SongId == songInfo.Id);
            await _db.DeleteAsync(entity);
            songInfo.IsFavorite = false;
        }

        public async Task<List<SongInfo>> GetSongsAsync()
        {
            var songsEntity = (await _db.FavoriteSongs.ToListAsync())
                .OrderByDescending(x => x.CreateDate)
                .ToList();

            var songs = songsEntity
                .Select(x => new SongInfo
                {
                    Album = x.Album,
                    AlbumCover = x.AlbumCover,
                    Artist = x.Artist,
                    Duration = x.Duration,
                    Name = x.Name,
                    SongUrl = x.SongUrl,
                    Id = x.SongId,
                    OwnerId = x.OwnerId,
                    AlbumId = x.AlbumId,
                    IsFavorite = true
                })
                .ToList();

            var downloadTasks = songs.Select(async song =>
            {
                var downloadedSong =  await _db.DownloadedSongs.FirstOrDefaultAsync(x => x.SongId == song.Id);

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
            var existingSong = await _db.FavoriteSongs.FirstOrDefaultAsync(x => x.SongId == songInfo.Id);

            if (existingSong == null)
            {
                existingSong = new FavoriteSongEntity
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
                    OwnerId = songInfo.OwnerId
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

