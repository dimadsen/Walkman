using System;
namespace Walkman.iOS.Infrastructure.Request
{
	public class DownloadSongRequest
	{
		public string Url { get; set; }
		public long SongId { get; set; }
		public SongType Type { get; set; }
	}
}

