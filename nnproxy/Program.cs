using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace nnproxy
{
    class Program
    {
        private const int APIVERSION = 1;
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == "?" || new Regex(@"^[\\|\/|\-](help|h)").IsMatch(args[0]))
                {
                    Console.WriteLine("-port：指定端口号");
                    Console.WriteLine("-svr：指定服务器地址");
                    Console.ReadKey();
                    return;
                }
                foreach (var item in args)
                {
                    if (new Regex(@"^[\\|\/|\-]port\=").IsMatch(item))
                    {
                        int TmpPort;
                        if (int.TryParse(item.Substring(6), out TmpPort) && TmpPort > 0)
                        {
                            Console.WriteLine("port：" + TmpPort);
                            xingshenProxyMgr.defaultPort = (ushort)TmpPort;
                        }
                        else
                        {
                            Console.WriteLine("参数port无效！已使用默认值：" + xingshenProxyMgr.defaultPort);
                        }
                    }
                    else if (new Regex(@"^[\\|\/|\-]svr\=").IsMatch(item))
                    {
                        xingshenProxyMgr.SvrApiUrl = item.Substring(5);
                        Console.WriteLine("SvrUrl：" + xingshenProxyMgr.SvrApiUrl);
                    }
                }
            }

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
                if (kk.Key == ConsoleKey.A)
                {
                    xingshenProxyMgr.showInfo = !xingshenProxyMgr.showInfo;
                    Console.WriteLine("ShowAllInfo:" + xingshenProxyMgr.showInfo);
                }
                else if (kk.Key == ConsoleKey.Q /*&& kk.Modifiers == ConsoleModifiers.Control*/)
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
