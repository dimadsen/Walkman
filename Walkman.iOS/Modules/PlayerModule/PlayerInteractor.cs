using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Walkman.Core.Interfaces.PlayerModule;
using Walkman.Core.Interfaces.Models;
using VkNet;
using Microsoft.Extensions.DependencyInjection;
using VkNet.Model.RequestParams;
using System.Linq;
using Walkman.Database.Models;
using Walkman.iOS.Infrastructure;
using Walkman.iOS.Infrastructure.Request;
using Walkman.Database;
using Newtonsoft.Json;

namespace Walkman.iOS.Modules.PlayerModule
{
    public class PlayerInteractor : IPlayerInteractor
    {
        public IPlayerPresenter PlayerPresenter { get; set; }
    }
}

