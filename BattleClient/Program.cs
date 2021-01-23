using BattleClient.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            TestControllerX.Instance.Send();

            while(true)
            {
                Console.ReadLine();
            }
        }
    }
}
