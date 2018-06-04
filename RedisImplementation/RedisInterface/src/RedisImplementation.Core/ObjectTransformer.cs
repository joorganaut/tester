using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RedisImplementation.Core
{
    public class ObjectTransformer<T> where T : class
    {
        public static string SerializeType(T obj, out string errMsg)
        {
            string result = string.Empty;
            errMsg = string.Empty;
            try
            {
                result = JsonConvert.SerializeObject(obj);
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
            return result;
        }

        public static T DeserializeType(string obj, out string errMsg)
        {
            T result = null;
            errMsg = string.Empty;
            try
            {
                result = JsonConvert.DeserializeObject<T>(obj) as T;
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
            return result;

        }
    }
}
