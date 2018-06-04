using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace NIP.Adhoc.Data
{
    public interface IDatabaseObject : IDatabase
    {
        string ConnectionStringName { get; set; }
        string ConnectionKey { get; set; }
    }
    public class DatabaseObject : Database, IDatabaseObject
    {
        public DatabaseObject(string ConnectionName) : base(ConnectionName)
        {
        }
        public string ConnectionStringName { get; set; }
        public string ConnectionKey { get; set; }
    }
    public class DatabaseManager 
    {
        private static string _defaultConnectionKey { get; set; }
        private const string _defaultKey = "core";
        public static IDatabaseObject GetDatabaseByKey(string key)
        {
            IDatabaseObject result = null;
            ObjectCache cache = MemoryCache.Default;
            if (cache != null)
            {
                result = cache[key] as IDatabaseObject;
                if (result == null)
                {
                    result = new DatabaseObject(_defaultKey);
                    result.ConnectionKey = _defaultKey;
                    cache[_defaultConnectionKey] = result;
                }
                else
                {
                    result.ConnectionKey = key;
                }
            }
            else
            {
                throw new Exception("Unable to initialize memory cache");
            }
            return result;
        }

        public static IDatabaseObject GetDatabaseByName(string connectionName, bool isSpecific = true)
        {
            IDatabaseObject result = null;
            ObjectCache cache = MemoryCache.Default;
            if (cache != null)
            {
                result = cache[_defaultConnectionKey] as IDatabaseObject;
                if (result == null)
                {
                    result = new DatabaseObject(connectionName);
                    result.ConnectionStringName = connectionName;
                    cache[_defaultConnectionKey] = result;
                }
                else
                {
                    result.ConnectionStringName = _defaultConnectionKey;
                }
            }
            else
            {
                throw new Exception("Unable to initialize memory cache"); 
            }
            return result;
        }
    }
}
