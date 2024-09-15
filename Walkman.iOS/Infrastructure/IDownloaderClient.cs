using System;
using System.Threading.Tasks;
using Refit;
using System.Net.Http;
using Walkman.iOS.Infrastructure.Request;

namespace Walkman.iOS.Infrastructure
{
	public interface IDownloaderClient
	{
		[Get("/api/songs")]
		Task<ApiResponse<HttpContent>> DownloadSongAsync([Query] DownloadSongRequest request);
	}
}

