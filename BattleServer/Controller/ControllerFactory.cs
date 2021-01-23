using BattleProtocol;
using BattleServer.Controller.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServer.Controller
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
