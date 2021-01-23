using BattleProtocol.Entities;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleProtocol
{
    public static class ProtocolFactory
    {
        public static byte[] SerializeProtocol(MessageType messageType, BaseProtocol protocol)
        {
            switch (messageType)
            {
                case MessageType.Test:
                    return MessagePackSerializer.Serialize((ProtoTest)protocol);
                default:
                    throw new Exception("[ProtocolFactory] Invalid MessageType : " + messageType);
            }
        }

        public static BaseProtocol DeserializeProtocol(MessageType messageType, byte[] bytes)
        {
            switch (messageType)
            {
                case MessageType.Test:
                    return MessagePackSerializer.Deserialize<ProtoTest>(bytes);
                default:
                    throw new Exception("[ProtocolFactory] Invalid Message Type : " + messageType);
            }
        }
    }
}
