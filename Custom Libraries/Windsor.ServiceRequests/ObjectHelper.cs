using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Windsor.ServiceRequests
{
    public class ObjectHelper
    {
        #region Methods

        /// <summary>
        /// Convert an XMLs to object.
        /// </summary>
        /// <param name="objType">Type of the obj.</param>
        /// <param name="xml">The XML.</param>
        /// <returns>The object</returns>
        private static object XMLToObject(Type objType, string xml)
        {
            if (xml == null)
                throw new ArgumentNullException();
            XmlSerializer serializer = new XmlSerializer(objType);
            StringReader stream = new StringReader(xml);                    // read xml data
            XmlTextReader reader = new XmlTextReader(stream);               // create reader
            return serializer.Deserialize(reader);                          // covert reader to object
        }
        /// <summary>
        /// Convert an Object to XML.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>The XML</returns>
        private static string ObjectToXML(object obj)
        {
            string retval = null;
            if (obj != null)
            {
                StringBuilder sb = new StringBuilder();
                using (XmlWriter writer = XmlWriter.Create(sb, new XmlWriterSettings() { OmitXmlDeclaration = true, NewLineOnAttributes = true, Indent = true }))
                {
                    new XmlSerializer(obj.GetType()).Serialize(writer, obj);
                }
                retval = sb.ToString();
            }
            return retval;
        }
        /// <summary>
        /// DeSerialize an object of T type to a JSON string
        /// </summary>
        /// <typeparam name="T">Type of robject</typeparam>
        /// <param name="inputString">Input string to be DeSerialized</param>
        /// <returns>DeSerialized object</returns>
        private static object JSONToObject<T>(string inputString)
        {
            object deSerializedObject = new object();
            deSerializedObject = JsonConvert.DeserializeObject<T>(inputString);
            return deSerializedObject;
        }
        /// <summary>
        /// Serialize an object to a JSON string
        /// </summary>
        /// <param name="obj">Object to be serialized</param>
        /// <returns>Serialized string</returns>
        private static string ObjectToJSON(object obj)
        {
            string serializedString = string.Empty;
            serializedString = JsonConvert.SerializeObject(obj, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
            return serializedString;
        }
        /// <summary>
        /// Prettify the json string
        /// </summary>
        /// <param name="jsonString">The json string</param>
        /// <returns>The prettified json string</returns>
        public static string PrettyJson(string jsonString)
        {
            return Newtonsoft.Json.Linq.JObject.Parse(jsonString).ToString();
        }
        /// <summary>
        /// Return the serialized string representation of an object.
        /// </summary>
        /// <param name="obj">The object that needs to be serialized</param>
        /// <param name="type">The type of serialization to be used (xml or json)</param>
        /// <returns>The serialized string representation of the object</returns>
        public static string GetSerializedString(object obj, string type = "json")
        {
            if (!string.IsNullOrEmpty(type))
            {
                if (type.ToUpper() == "XML")
                {
                    return ObjectToXML(obj);
                }
                else
                {
                    return ObjectToJSON(obj);
                }
            }
            else
            {
                return ObjectToJSON(obj);
            }
        }

        #endregion Public Methods
    }
    /// <summary>
    /// The URL invocation method. Used by the HttpHelper class
    /// </summary>
    public enum HttpMethod
    {
        /// <summary>
        /// HTTP POST method
        /// </summary>
        Post,
        /// <summary>
        /// HTTP GET method
        /// </summary>
        Get,
        /// <summary>
        /// HTTP HEAD method
        /// </summary>
        Head
    }
    /// <summary>
    /// The content type used in the url. Used by the HttpPostApi class.
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// content type = text/xml; charset=UTF-8
        /// </summary>
        textXml,
        /// <summary>
        /// Content type = appication/json
        /// </summary>
        json,
    }
    /// <summary>
    /// The encoding type. Used by the HttpPostApi class.
    /// </summary>
    public enum EncodingType
    {
        /// <summary>
        /// Ascii encoding
        /// </summary>
        ASCII,
        /// <summary>
        /// Unicode encoding
        /// </summary>
        Unicode,
        /// <summary>
        /// UTF7 encoding
        /// </summary>
        UTF7,
        /// <summary>
        /// UTF8 encoding
        /// </summary>
        UTF8
    }
    [Serializable]
    /// <summary>
    /// An generic HTTP Helper class that can be used for calling api methods using the HTTP protocol
    /// Use this class when you want to call any url & get back a response.
    /// </summary>
    public class HttpPostApi
    {
        /// <summary>
        /// Data to send as post data
        /// </summary>
        string _postData = null;
        /// <summary>
        /// boolean field indicating if compression should be used during communication
        /// </summary>
        bool _enableCompression;
        /// <summary>
        /// Custom headers that the user wants to send
        /// </summary>
        public Dictionary<string, string> _headers = new Dictionary<string, string>();
        /// <summary>
        /// Gets or sets the post data.
        /// </summary>
        /// <value>
        /// The post data.
        /// </value>
        public string PostData { set { _postData = value; } get { return _postData; } }
        /// <summary>
        /// Gets or sets a value indicating whether [enable compression].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable compression]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableCompression { set { _enableCompression = value; } get { return _enableCompression; } }
        /// <summary>
        /// Invokes the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="method">The HTTP method (POST or GET).</param>
        /// <param name="contentType">The content type based on the enum.</param>
        /// <returns>The response as a string</returns>
        public string invokeURL(string url, HttpMethod method, ContentType contentType, long rangeFrom = 0, long rangeTo = 0)
        {
            HttpWebRequest Request = null;
            HttpWebResponse Response = null;
            string retval = "";

            Request = (HttpWebRequest)WebRequest.Create(url);
            Request.Method = (method == HttpMethod.Post) ? "POST" : "GET";
            if (_enableCompression)
                Request.Headers.Add("Accept-Encoding", "gzip,deflate");
            switch (contentType)
            {
                case ContentType.textXml:
                    Request.ContentType = "text/xml; charset=UTF-8";
                    break;
                case ContentType.json:
                    Request.ContentType = "application/json";
                    break;
                default:
                    Request.ContentType = "application/x-www-form-urlencoded";
                    break;
            }
            if (_headers.Count != 0)
            {
                foreach (KeyValuePair<string, string> item in _headers)
                {
                    Request.Headers.Add(item.Key, item.Value);
                }
            }
            //Request.KeepAlive = true;
            Request.ContentLength = 0;
            Request.Proxy = null;

            if (_postData != null)
            {
                System.Text.Encoding encoding = new System.Text.UTF8Encoding();
                byte[] data = encoding.GetBytes(_postData);
                Request.ContentLength = data.Length;
                Stream stream = Request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
            }
            if (rangeTo > 0)
            {
                Request.AddRange(rangeFrom, rangeTo);
                //Request.Headers.Add(HttpRequestHeader.Range, "bytes=" + rangeFrom + "-" + rangeTo);
            }

            Response = (HttpWebResponse)Request.GetResponse();
            retval = StreamToString(Response.GetResponseStream());

            return retval;
        }
        /// <summary>
        /// Convert a Stream to string.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The contents of the stream as string</returns>
        private string StreamToString(Stream stream)
        {
            MemoryStream ret = new MemoryStream();
            stream.CopyTo(ret);
            return ByteArrayToString(ret.ToArray());
        }
        /// <summary> 
        /// Converts a byte array to a string using specified encoding. 
        /// </summary> 
        /// <param name="bytes">Array of bytes to be converted.</param> 
        /// <param name="encodingType">EncodingType enum.</param> 
        /// <returns>The byte array converted to string</returns> 
        public static string ByteArrayToString(byte[] bytes, EncodingType encodingType = EncodingType.UTF8)
        {
            System.Text.Encoding encoding = null;
            switch (encodingType)
            {
                case EncodingType.ASCII:
                    encoding = new System.Text.ASCIIEncoding();
                    break;
                case EncodingType.Unicode:
                    encoding = new System.Text.UnicodeEncoding();
                    break;
                case EncodingType.UTF7:
                    encoding = new System.Text.UTF7Encoding();
                    break;
                case EncodingType.UTF8:
                    encoding = new System.Text.UTF8Encoding();
                    break;
            }
            return encoding.GetString(bytes);
        }
    }
}