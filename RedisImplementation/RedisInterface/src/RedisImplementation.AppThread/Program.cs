using RedisImplementation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisImplementation.AppThread
{
    class Program
    {
        static void Main(string[] args)
        {
            TestConfiguration();
            Console.ReadLine();
        }
        static void TestConfiguration()
        {
            string errMsg = string.Empty;
            AccountDAO.SetConfiguration(new KeyValuePair<string, string>("Test", ""), out errMsg);
        }
    }
}
