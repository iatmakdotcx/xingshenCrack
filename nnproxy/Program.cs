using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nnproxy
{
    class Program
    {
        private const int APIVERSION = 1;
        static void Main(string[] args)
        {
            int svrapiversion;
            string errmsg = xingshenProxyMgr.getApiVersion(out svrapiversion);
            if (!string.IsNullOrEmpty(errmsg))
            {
                Console.WriteLine(errmsg);
                Console.ReadKey();
                return;
            }
            if (APIVERSION < svrapiversion)
            {
                Console.WriteLine("请重新下载新版本客户端！");
                Console.ReadKey();
                return;
            }

            xingshenProxyMgr.Start();
            while (true)
            {
                var kk = Console.ReadKey();
                if (kk.Key == ConsoleKey.Q /*&& kk.Modifiers == ConsoleModifiers.Control*/)
                {
                    throw new Exception("exit");
                    break;
                }
                else
                {
                    Console.WriteLine("key:" + kk.Key.ToString() + ",Modifiers:" + kk.Modifiers);
                }
            }           
            xingshenProxyMgr.Stop();
            Console.WriteLine("Exit...");
        }
    }
}
