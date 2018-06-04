using NetHelper.Network;
using RedisImplementation.Core;
using RedisImplementation.Core.Data;
using RedisImplementation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisImplementation.App
{
    public class TestObject : MessageContract
    {
        public TestObject()
        {

        }
        //when creating the derived class must force constructor
        //public TestObject(Type type) : base(type)
        //{
        //    this.ResponseType = type;
        //}
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //TestRedisPush();
            //TestRetrieveAndExpiry();
            //TestConfiguration();
            TestMessageContract();
            //TestDeserialization();
            Console.ReadLine();
        }
        static void TestRedisPush()
        {
            string errMsg = string.Empty;
            Account account = new Account()
            {
                KYCLevel = "1",
                Name = "Osazee Igbinosun",
                Number = "1020165500",
                Status = AccountStatus.Active
            };
            AccountDAO.Push(account, out errMsg);

            var response = AccountDAO.Pull(account.Number, out errMsg);
            Console.WriteLine(response.Name);

            
        }
        static void TestDeserialization()
        {
            //var x = ObjectTransformer<int>.SerializeType()
        }
        static void TestRetrieveAndExpiry()
        {
            string errMsg = string.Empty;
            var response = AccountDAO.Pull("1020165500", out errMsg);
            Console.WriteLine(response.Name);
            AccountDAO.Subscribe();
        }

        static void TestConfiguration()
        {
            string errMsg = string.Empty;
            AccountDAO.SetConfiguration(new KeyValuePair<string, string>("Test", ""), out errMsg);
        }
        static void TestMessageContract()
        {
            TestObject obj = new TestObject();// typeof(string);
            obj.Name = "Osazee Igbinosun";
            obj.Age = 21;
            obj.DateOfBirth = DateTime.Now;
            obj.IsXml = true;
            string message = obj.RequestMessage();
        }
    }
}
