using System;
using Walkman.Core.Interfaces.Models;

namespace Walkman.Core.Models
{
	public class PlayerModel
	{
		public SongInfo CurrentSong { get; set; }
		public double CurrentTime { get; set; }
		public PlayerStatus PlayerStatus { get; set; }
		public bool IsLastSong { get; set; }
		public string SongStatistics { get; set; }
		public MoveSong Move { get; set; }
		public bool IsFirstSong { get; set; }
	}
}

