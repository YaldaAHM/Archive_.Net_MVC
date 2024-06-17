using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.ClientCenterStrategy;

namespace YA.Archive.Service.UserActivity.UserLogTimeMdl.Sterategy
{
    public static class ClientCenterFactory
    {
        public static IClientCenterStrategy
        GetClientCenterStrategyFor(ClientCenterType clientCenterType)
        {
            switch (clientCenterType)
            {
                case ClientCenterType.Main:
                    return new MainClientCenterStrategy();
                case ClientCenterType.Other:
                    return new OtherClientCenterStrategy();
                default:
                    return new MainClientCenterStrategy();
            }
        }
    }
}
