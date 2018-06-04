using RedisImplementation.Core;
using RedisImplementation.Core.Data;
using ServiceStack.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisImplementation.Data
{
    public class AccountDAO
    {
        public static bool Push(Account account, out string errMsg)
        {
            bool result = false;
            errMsg = string.Empty;
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");

            IDatabase db = redis.GetDatabase();
            var redisAccount = ObjectTransformer<Account>.SerializeType(account, out errMsg);
            if (db.StringSet(account.Number, redisAccount, new TimeSpan(0, 0, 2)))
            {
                var val = db.StringGet(account.Number);
                result = true;
                Console.WriteLine(val);
            }
            return result;
        }
        public static Account Pull(string key, out string errMsg)
        {
            Account result = null;
            errMsg = string.Empty;
            try
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
                IDatabase db = redis.GetDatabase();
                var val = db.StringGet(key);
                Console.WriteLine(val);
                result = ObjectTransformer<Account>.DeserializeType(val.ToString(), out errMsg);
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
            return result;
        }
        public static bool SetConfiguration(KeyValuePair<string, string> keyValue, out string errMsg)
        {
            bool result = false;
            errMsg = string.Empty;
            try
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");

                var subscriber = redis.GetSubscriber();
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
            return result;
        }
        public static void Subscribe()
        {
            //const string ChannelName = "__key*__:*";
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");

            //var subscriber = ConnectionMultiplexer.;
            const string ChannelName = "__keyevent@0__:expired";
            using (var redisConsumer = new RedisClient("localhost"))
            {
                redisConsumer.ConfigSet("notify-keyspace-events", Encoding.UTF8.GetBytes("Exg"));
                using (var subscription = redisConsumer.CreateSubscription())
                {
                    subscription.OnSubscribe = channel =>
                    {
                        Console.WriteLine(String.Format("Subscribed to '{0}'", channel));
                    };
                    subscription.OnUnSubscribe = channel =>
                    {
                        Console.WriteLine(String.Format("UnSubscribed from '{0}'", channel));
                    };
                    subscription.OnMessage = (channel, msg) =>
                    {
                        Console.WriteLine(String.Format("Received '{0}' from channel '{1}'",
                            msg, channel));
                        RefreshObject(msg);
                        Console.WriteLine(String.Format("Account with number '{0}' has expired and is refreshed at {1}",
                            msg, DateTime.Now.ToString("hh:mm:ss:ss dd-MM-yyyy")));
                    };
                    Console.WriteLine(String.Format("Started Listening On '{0}'", ChannelName));
                    subscription.SubscribeToChannelsMatching(ChannelName); //blocking
                }
            }
        }
        public static void RefreshObject(string accountNumber)
        {
            string errMsg = string.Empty;
            Account account = new Account()
            {
                KYCLevel = "1",
                Name = "Osazee Igbinosun",
                Number = accountNumber,
                Status = AccountStatus.Active
            };
            AccountDAO.Push(account, out errMsg);
        }
        public static void NotificationsExample()
        {
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect("localhost");
            var subscriber = connection.GetSubscriber();
            int db = 0; //what Redis DB do you want notifications on?
            string notificationChannel = "__keyspace@" + db + "__:*";

            //you only have to do this once, then your callback will be invoked.
            subscriber.Subscribe(notificationChannel, (channel, notificationType) =>
            {
                // IS YOUR CALLBACK NOT GETTING CALLED???? 
                // -> See comments above about enabling keyspace notifications on your redis instance
                var key = GetKey(channel);
                switch (notificationType) // use "Kxge" keyspace notification options to enable all of the below...
                {
                    case "expire": // requires the "Kg" keyspace notification options to be enabled
                        Console.WriteLine("Expiration Set for Key: " + key);
                        break;
                    case "expired": // requires the "Kx" keyspace notification options to be enabled
                        Console.WriteLine("Key EXPIRED: " + key);
                        break;
                    case "rename_from": // requires the "Kg" keyspace notification option to be enabled
                        Console.WriteLine("Key RENAME(From): " + key);
                        break;
                    case "rename_to": // requires the "Kg" keyspace notification option to be enabled
                        Console.WriteLine("Key RENAME(To): " + key);
                        break;
                    case "del": // requires the "Kg" keyspace notification option to be enabled
                        Console.WriteLine("KEY DELETED: " + key);
                        break;
                    case "evicted": // requires the "Ke" keyspace notification option to be enabled
                        Console.WriteLine("KEY EVICTED: " + key);
                        break;
                    case "set": // requires the "K$" keyspace notification option to be enabled for STRING operations
                        Console.WriteLine("KEY SET: " + key);
                        break;
                    default:
                        Console.WriteLine("Unhandled notificationType: " + notificationType);
                        break;
                }
            });

            Console.WriteLine("Subscribed to notifications...");

            // setup for delete notification example
            connection.GetDatabase(db).StringSet("DeleteExample", "Anything");

            // key rename callbacks example
            connection.GetDatabase(db).StringSet("{RenameExample}From", "Anything");
            connection.GetDatabase(db).KeyRename("{RenameExample}From", "{RenameExample}To");

            var random = new Random();
            //add some keys that will expire to test the above callback configured above.
            for (int i = 0; i < 10; i++)
            {
                var expiry = TimeSpan.FromSeconds(random.Next(2, 10));
                connection.GetDatabase(db).StringSet("foo" + i, "bar", expiry);
            }

            // should result in a delete notification callback.
            connection.GetDatabase(db).KeyDelete("DeleteExample");
        }

        private static string GetKey(string channel)
        {
            var index = channel.IndexOf(':');
            if (index >= 0 && index < channel.Length - 1)
                return channel.Substring(index + 1);

            //we didn't find the delimeter, so just return the whole thing
            return channel;
        }
        public static void OnAccountExpiriation()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            IDatabase db = redis.GetDatabase();
        }
        
    }

   
}
