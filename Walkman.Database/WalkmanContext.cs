using System;
using System.Threading.Tasks;
using Network;
using SQLite;
using Walkman.Database.Models;

namespace Walkman.Database
{
	public class WalkmanContext
	{
		private SQLiteAsyncConnection _db;

		public WalkmanContext(string path)
		{
			_db = new SQLiteAsyncConnection(path);

			_db.CreateTableAsync<SongEntity>().GetAwaiter().GetResult();
			_db.CreateTableAsync<FavoriteSongEntity>().GetAwaiter().GetResult();
			_db.CreateTableAsync<DownloadedSongEntity>().GetAwaiter().GetResult();
		}

		public AsyncTableQuery<SongEntity> RecentSongs => _db.Table<SongEntity>();
		public AsyncTableQuery<FavoriteSongEntity> FavoriteSongs => _db.Table<FavoriteSongEntity>();
		public AsyncTableQuery<DownloadedSongEntity> DownloadedSongs => _db.Table<DownloadedSongEntity>();

        public async Task<int> AddAsync<T>(T entity)
		{
			return await _db.InsertAsync(entity);
		}

		public async Task DeleteAsync<T>(T entity)
		{
			await _db.DeleteAsync(entity);
			await _db.ExecuteAsync("vacuum;");
        }

		public async Task UpdateAsync<T>(T entity)
		{
			await _db.UpdateAsync(entity);
		}
	}
}

