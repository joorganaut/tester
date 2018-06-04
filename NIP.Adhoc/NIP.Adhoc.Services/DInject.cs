using NIP.Adhoc.Core.contracts;
using NIP.Adhoc.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIP.Adhoc.Services
{
    public class DInject
    {
    }

    public class Retrieve<T> : IService<T> where T : IBusinessObject
    {
        public object ExecuteAction(string connectionStringName, string query, object args, out string errMsg)
        {
            T result = default(T);
            errMsg = string.Empty;
            try
            {
                result = BusinessObjectDAO<T>.Retrieve(connectionStringName, query, out errMsg);
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
            return result;
        }

        public object ExecuteAction(string connectionStringName, string query, out string errMsg, bool isUnique, bool withPaging = true, params object[] parameters)
        {
            T result = default(T);
            errMsg = string.Empty;
            try
            {

                result = BusinessObjectDAO<T>.RetrieveDataObjectByParameter(connectionStringName, query, out errMsg, isUnique, parameters);
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
            return result;
        }

    }
    public class RetrieveMany<T> : IService<T> where T : IBusinessObject
    {
        public object ExecuteAction(string connectionStringName, string query, object args, out string errMsg)
        {
            IList<T> result = default(IList<T>);
            errMsg = string.Empty;
            try
            {
                result = BusinessObjectDAO<T>.RetrieveAll(connectionStringName, query, out errMsg);
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
            return result;
        }
        bool IsSearchParameters(string pair)
        {
            string[] parameterKeys = new string[] { "sort", "direction", "page", "pageSize", "ItemsCount" };
            bool result = parameterKeys.Contains(pair);
            return result;
        }
        public object ExecuteAction(string connectionStringName, string query, out string errMsg, bool isUnique, bool withPaging = true, params object[] parameters)
        {

            IList<T> result = default(IList<T>);
            errMsg = string.Empty;
            try
            {
                if (withPaging == false)
                {
                    result = BusinessObjectDAO<T>.RetrieveAllByParameters(connectionStringName, query, out errMsg, parameters);
                }
                else
                {
                    System.Type type = parameters[0].GetType();
                    //int clientid = (int)type.GetProperty("sort").GetValue(parameters, null);
                    //string sort, string direction, int page, int pageSize, out int totalItemsCount
                    string sort = type.GetProperty("sort").GetValue(parameters[0], null).ToString();

                    string direction = type.GetProperty("direction").GetValue(parameters[0], null).ToString(); 
                    int page = int.Parse(type.GetProperty("page").GetValue(parameters[0], null).ToString());
                    int pageSize = int.Parse(type.GetProperty("pageSize").GetValue(parameters[0], null).ToString());
                    int ItemsCount = int.Parse(type.GetProperty("itemsCount").GetValue(parameters[0], null).ToString());
                    //parameters.ToList().Remove(parameters.Where(x=> { return IsSearchParameters(x); }).FirstOrDefault());
                    var _real_parameters = type.GetProperties().Where(x => { return !IsSearchParameters(x.Name); });
                    //_real_parameters.Remove(parameters.Where(x => { return IsSearchParameters(x); }).FirstOrDefault());
                    result = BusinessObjectDAO<T>.RetrieveAllWithPaging(connectionStringName, query, sort, Direction.Default, page, pageSize, out ItemsCount, out errMsg, _real_parameters.ToArray());
                    parameters[parameters.Length - 1] = new KeyValuePair<string, object>("ItemsCount", ItemsCount);
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
            return result;
        }
    }
    public class DIImplementation<T> where T : IBusinessObject
    {
        public T _obj { get; set; }
        public object[] _parameters { get; set; }
        public bool _hasParameters;
        public bool _isPaged = false;
        public bool _isUnique = true;
        public string _errMsg { get; set; }
        public string _connectionStringName { get; set; }
        public string _query { get; set; }
        IServiceProvider<T> _ServiceProvider { get; set; }
        public DIImplementation()
        {
            Dictionary<string, IService<T>> services = new Dictionary<string, IService<T>>();
            IServiceCollection<T> serviceCollection = new ServiceCollection<T>(services);
            RegisterServices(serviceCollection);
            _ServiceProvider = new ServiceProvider<T>(services);
        }
        public DIImplementation(string connectionStringName, string query, T obj, bool isUnique, params object[] parameters)
        {
            _obj = obj;
            _isUnique = isUnique;
            _connectionStringName = connectionStringName;
            _query = query;
            _parameters = parameters;
            Dictionary<string, IService<T>> services = new Dictionary<string, IService<T>>();
            IServiceCollection<T> serviceCollection = new ServiceCollection<T>(services);
            RegisterServices(serviceCollection);
            _ServiceProvider = new ServiceProvider<T>(services);
        }
        public DIImplementation(string connectionStringName, string query, bool isPaged, params object[] parameters)
        {
            _parameters = parameters;
            _connectionStringName = connectionStringName;
            _query = query;
            _isPaged = isPaged;
            Dictionary<string, IService<T>> services = new Dictionary<string, IService<T>>();
            IServiceCollection<T> serviceCollection = new ServiceCollection<T>(services);
            RegisterServices(serviceCollection);
            _ServiceProvider = new ServiceProvider<T>(services);
        }
        public object Execute(string operation)
        {
            _errMsg = string.Empty;
            string errMsg = string.Empty;
            object response = new object();
            var provider = _ServiceProvider.GetByName(operation);
            if (_hasParameters == false)
            {
                response = provider.ExecuteAction(_connectionStringName, _query, _obj, out errMsg);
            }
            else
            {
                response = provider.ExecuteAction(_connectionStringName, _query, out errMsg, _isUnique, _isPaged, _parameters);
            }
            _errMsg = errMsg;
            return response;
        }




        void RegisterServices(IServiceCollection<T> serviceCollection)
        {
            serviceCollection.AddNamedService("Retrieve", new Retrieve<T>());
            serviceCollection.AddNamedService("RetrieveMany", new RetrieveMany<T>());
        }
    }
    public interface IService<T> where T : IBusinessObject
    {
        object ExecuteAction(string connectionStringName, string query, object obj, out string errMsg);
        object ExecuteAction(string connectionStringName, string query, out string errMsg, bool isUnique, bool withPaging = true, params object[] parameters);
    }
    public interface IServiceCollection<T> where T : IBusinessObject
    {
        void AddNamedService(string name, IService<T> service);
    }
    public class ServiceCollection<T> : IServiceCollection<T> where T : IBusinessObject
    {
        private readonly IDictionary<string, IService<T>> services;

        public ServiceCollection(IDictionary<string, IService<T>> services)
        {
            this.services = services;
        }

        public void AddNamedService(string name, IService<T> service)
        {
            if (services.ContainsKey(name))
            {
                this.services[name] = service;
            }
            else
            {
                this.services.Add(name, service);
            }
        }
    }
    public interface IServiceProvider<T> where T : IBusinessObject
    {
        IService<T> GetByName(string name);
    }
    public class ServiceProvider<T> : IServiceProvider<T> where T : IBusinessObject
    {
        private readonly IDictionary<string, IService<T>> services;

        public ServiceProvider(IDictionary<string, IService<T>> services)
        {
            this.services = services;
        }

        public IService<T> GetByName(string name)
        {
            if (this.services.ContainsKey(name))
            {
                return this.services[name];
            }
            return null;
        }
    }
}
