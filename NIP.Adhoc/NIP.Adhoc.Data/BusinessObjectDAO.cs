using NIP.Adhoc.Core.contracts;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIP.Adhoc.Data
{
    public class BusinessObjectDAO<T> where T : IBusinessObject
    {
        private static IDatabaseObject Database;
        public static T RetrieveDataObjectByParameter(string connectionName, string query, out string errMsg, bool isUnique = false, params object[] args)
        {
            T result = default(T);
            errMsg = string.Empty;
            try
            {
                using (Database = DatabaseManager.GetDatabaseByName(connectionName, true))
                {
                    var intResult = Database.Fetch<T>(query, args);
                    if(intResult != null && intResult.Count == 1)
                    {
                        result = intResult.FirstOrDefault();
                    }
                    else if (intResult.Count > 1 && isUnique == false)
                    {
                        throw new Exception("Result not unique");
                    }
                    else if (intResult.Count > 1 && isUnique == true)
                    {
                        result = intResult.FirstOrDefault();
                    }
                    else if (intResult.Count < 0)
                    {
                        throw new Exception("Invalid Result");
                    }
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
            return result;
        }
        public static T Retrieve(string connectionName, string query, out string errMsg, params object[] args)
        {
            T result = default(T);
            errMsg = string.Empty;

            return result;
        }
        public static IList<T> RetrieveAll(string connectionName, string query, out string errMsg)
        {
            IList<T> result = null;
            errMsg = string.Empty;
            try
            {
                using (Database = DatabaseManager.GetDatabaseByName(connectionName, true))
                {
                    result = Database.Fetch<T>(query);
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
            return result;
        }
        public static IList<T> RetrieveAllByParameters(string connectionName, string query, out string errMsg, params object[] args)
        {
            IList<T> result = null;
            errMsg = string.Empty;
            try
            {
                using (Database = DatabaseManager.GetDatabaseByName(connectionName, true))
                {
                    result = Database.Fetch<T>(query, args);
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
            return result;
        }
        public static IList<T> RetrieveAllWithPaging(string connectionName, string query, string sort, Direction direction, int page, int pageSize, out int itemsCount, out string errMsg, params object[] args)
        {
            IList<T> result = null;
            errMsg = string.Empty;
            itemsCount = 0;
            try
            {
                using (Database = DatabaseManager.GetDatabaseByName(connectionName, true))
                {
                    pageSize = pageSize <= 0 ? 10 : pageSize;
                    var subResult = Database.Page<T>(page, pageSize, query, args);
                    itemsCount = (int)subResult.TotalItems;
                    result = subResult.Items;
                }
                if (direction == Direction.Default)
                {
                    if (string.IsNullOrWhiteSpace(sort))
                    {
                        result.OrderBy(x => x.ID);
                    }
                    else
                    {
                        result.OrderBy(x => x.GetType().GetProperties().Where(p => p.Name.ToLower() == sort.ToLower()));
                    }
                }
                else
                {
                    if (direction == Direction.Descending)
                    {
                        if (string.IsNullOrWhiteSpace(sort))
                        {
                            result.OrderByDescending(x => x.ID);
                        }
                        else
                        {
                            result.OrderByDescending(x => x.GetType().GetProperties().Where(p => p.Name.ToLower() == sort.ToLower()));
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(sort))
                        {
                            result.OrderByDescending(x => x.ID);
                        }
                        else
                        {
                            result.OrderByDescending(x => x.GetType().GetProperties().Where(p => p.Name.ToLower() == sort.ToLower()));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
            return result;
        }
    }

    public enum Direction
    {
        Default = 0,
        Ascending = 1,
        Descending = 2
    }
}
