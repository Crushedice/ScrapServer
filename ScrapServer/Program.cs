using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScrapServer
{
    internal class Program
    {
        static void Main(string[] args)
        {


            new Thread(
                delegate () { new ScrapAPI(); }).Start();

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.C && keyInfo.Modifiers == ConsoleModifiers.Control) break;
            }
        }
    }
}
