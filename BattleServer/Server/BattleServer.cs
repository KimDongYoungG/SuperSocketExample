using BattleProtocol;
using BattleServer.Controller;
using SuperSocket.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServer.Server
{
    public class BattleServer : AppServer<BattleSession, BinaryRequestInfo>
    {
        private BattleServer()
            : base(new DefaultReceiveFilterFactory<BattleReceiveFilter, BinaryRequestInfo>())
        {
            NewRequestReceived += new RequestHandler<BattleSession, BinaryRequestInfo>(RequestReceived);
            NewSessionConnected += BattleServer_NewSessionConnected;
            SessionClosed += new SessionHandler<BattleSession, CloseReason>(BattleServer_SessionClosed);
        }

        private static BattleServer _instance = null;
        public static BattleServer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BattleServer();
                }
                return _instance;
            }
        }

        private async void RequestReceived(BattleSession session, BinaryRequestInfo requestInfo)
        {
            int offset = 0;
            var sizeHeader = sizeof(MessageType);
            var intSize = sizeof(int);

            while (offset < requestInfo.Body.Length)
            {
                BaseController controller = null;
                MessageType msgType = MessageType.None;
                byte[] body = null;

                try
                {
                    var byteArrayHeader = requestInfo.Body.CloneRange(offset, sizeHeader);
                    var byteArrayBodyLength = requestInfo.Body.CloneRange(offset + sizeHeader, intSize);
                    var bodyLength = BitConverter.ToInt32(byteArrayBodyLength, 0);

                    body = requestInfo.Body.CloneRange(offset + intSize + sizeHeader, bodyLength);
                    offset += (sizeHeader + intSize + bodyLength);
                    msgType = (MessageType)BitConverter.ToInt32(byteArrayHeader, 0);
                }
                catch (Exception)
                {
                    var msg = $"{msgType.ToString()}_{ (requestInfo.Body?.Length ?? 0).ToString()}_{session.SocketSession.RemoteEndPoint.ToString()}";
                    Console.WriteLine(msg);
                    return;
                }

                BaseProtocol proto = null;
                proto = ProtocolFactory.DeserializeProtocol(msgType, body);

                try
                {
                    controller = ControllerFactory.CreateController(msgType);
                    await controller.DoPipeline(session, proto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return;
                }
            }
        }

        private void BattleServer_SessionClosed(BattleSession session, CloseReason reason)
        {
            Console.WriteLine($"세션 번호 {0} 접속해제: {1}", session.SessionID, reason.ToString());
        }

        private void BattleServer_NewSessionConnected(BattleSession session)
        {
            Console.WriteLine("NewSessionConnected : " + session.SessionID);
        }
    }
}
