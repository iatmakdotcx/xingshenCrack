using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nnproxy
{
    class Program
    {
        static void Main(string[] args)
        {
            Web.Model.ModelBase.Init("xinshen");

            xingshenSvrHelper.xingshenProxyMgr.Start();

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
            xingshenSvrHelper.xingshenProxyMgr.Stop();
            Console.WriteLine("Exit...");
        }
    }
}
