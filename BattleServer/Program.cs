﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;

namespace BattleServer
{
    class Program
    {
        static AutoResetEvent terminatingEvent = new AutoResetEvent(false);
        static void Main(string[] args)
        {
            //https://docs.supersocket.net/v1-6/en-US/SuperSocket-Basic-Configuration
            //https://www.slideshare.net/jacking/kgc-2016-super-socket

            var result = Server.BattleServer.Instance.Setup(new ServerConfig
            {
                Ip = "0.0.0.0",
                Port = 50404,
                Mode = SocketMode.Tcp,
                MaxConnectionNumber = 3000,
                ReceiveBufferSize = 4096,
                MaxRequestLength = 4096,
                SendBufferSize = 4096,
                SendingQueueSize = 512,
                ClearIdleSession = true,
                ClearIdleSessionInterval = 120,
                IdleSessionTimeOut = 600,
                SyncSend = false,
                Name = "Battle Server"
            });

            if (false == result)
            {
                Console.WriteLine("Battle Server Setup Fail...");
                return;
            }

            Console.WriteLine("Battle Server Start...");
            Server.BattleServer.Instance.Start();
            terminatingEvent.WaitOne();
            Server.BattleServer.Instance.Dispose();
        }

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            terminatingEvent.Set();
        }

        static void OnConsoleKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            if (e.SpecialKey == ConsoleSpecialKey.ControlC)
            {
                e.Cancel = true;
                terminatingEvent.Set();
            }
        }
    }
}
