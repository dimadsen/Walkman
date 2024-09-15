using System;
using System.Collections.Generic;
using Walkman.Core.Interfaces.Models;

namespace Walkman.Core.Models
{
	public class СompilationInfo
	{
		public string Name { get; set; }
		public Genre Genre { get; set; }
		public List<SongInfo> Songs { get; set; }
	}
}

