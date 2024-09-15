using System;
using Walkman.Core.Models;

namespace Walkman.Core.Interfaces.Models
{
	public class SongInfo
	{
		public long Id { get; set; }
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
		public bool IsFavorite { get; set; }
		public DownloadStatus DownloadStatus { get; set; }
	}
}

