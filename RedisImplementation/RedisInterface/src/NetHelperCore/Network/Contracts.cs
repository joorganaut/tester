using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace NetHelperCore.Network
{
    [Serializable]
    public abstract class MessageContract : INetworkMessage
    {
        public MessageContract()
        { }
        public MessageContract(Type responseType)
        {
            ResponseType = responseType;
        }
        [JsonIgnore]
        [XmlIgnore]
        public bool UseNamespace { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        public bool IsXml { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        public VerbType Verb { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        public string Error { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        public bool KeepAlive { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        public bool IsAsync { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        private string _requestMessage = string.Empty;
        public string RequestMessage()
        {

            try
            {
                if (IsXml)
                {
                    var serializer = new XmlSerializer(this.GetType());
                    if (UseNamespace == false)
                    {
                        using (var stringwriter = new System.IO.StringWriter())
                        {
                            serializer.Serialize(stringwriter, this);
                            _requestMessage = stringwriter.ToString();
                        }
                    }
                    else
                    {
                        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                        ns.Add("", "");
                        using (var stringwriter = new System.IO.StringWriter())
                        {
                            serializer.Serialize(stringwriter, this, ns);
                            _requestMessage = stringwriter.ToString();
                        }
                    }
                }
                else
                {
                    _requestMessage = JsonConvert.SerializeObject(this);
                }
            }
            catch (Exception e)
            {
                Error = e.Message;
            }
            return _requestMessage;
        }
        [JsonIgnore]
        [XmlIgnore]
        public string ResponseMessage { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        public Type ResponseType { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public INetworkMessage Response
        {
            get
            {
                INetworkMessage result = null;
                try
                {
                    var response = System.Activator.CreateInstance(ResponseType);
                    if (IsXml)
                    {
                        var stringReader = new System.IO.StringReader(ResponseMessage);
                        var serializer = new XmlSerializer(ResponseType);
                        response = serializer.Deserialize(stringReader);
                        result = response as INetworkMessage;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject(ResponseMessage);
                        result = response as INetworkMessage;
                    }
                }
                catch (Exception e)
                {
                    Error = e.Message;
                }
                return result;
            }
        }
        [JsonIgnore]
        [XmlIgnore]
        public IDictionary<string, string> Headers { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        public int StatusCode { get; set; }
    }
    public interface INetworkMessage
    {
        bool UseNamespace { get; set; }
        bool IsXml { get; set; }
        VerbType Verb { get; set; }
        string Error { get; set; }
        bool KeepAlive { get; set; }
        bool IsAsync { get; set; }
        string RequestMessage();
        string ResponseMessage { get; set; }
        INetworkMessage Response { get; }
        Type ResponseType { get; set; }
        IDictionary<string, string> Headers { get; set; }
        int StatusCode { get; set; }
    }
    [Serializable]
    public enum VerbType
    {
        GET = 0,
        POST = 1,
        PUT = 2,
        DELETE = 3,
        PATCH = 4
    }
}
