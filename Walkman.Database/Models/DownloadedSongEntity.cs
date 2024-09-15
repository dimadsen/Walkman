using System;
using SQLite;

namespace Walkman.Database.Models
{
    [Table("DownloadedSongs")]
    public class DownloadedSongEntity
	{
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public long SongId { get; set; }
        public string Name { get; set; }
        public string SongUrl { get; set; }
        public byte[] SongData { get; set; }
        public string Artist { get; set; }

        public string Album { get; set; }
        public string AlbumCover { get; set; }
        public long? AlbumId { get; set; }
        public long OwnerId { get; set; }
        /// <summary>
        /// Длительность
        /// </summary>
        public int Duration { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

