using BattleClient.Controllers.ForBattleServer;
using BattleProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleClient.Controllers
{
    public static class ControllerFactory
    {
        public static BaseController CreateController(MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.Test:
                    return new TestController();
                default:
                    throw new Exception("Invalid messageType");
            }
        }
    }
}
