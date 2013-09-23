using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Myers.WebSockDemo.Service
{
    public class JsonMessageSerializer
    {
        public static T Deserialize<T>(Message msg)
        {
            byte[] body = msg.GetBody<byte[]>();
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));
            var memStream = new MemoryStream(body);
            return (T)jsonSerializer.ReadObject(memStream);
        }

        public static Message Serialize<T>(T msgBody)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));
            var memStream = new MemoryStream();
            jsonSerializer.WriteObject(memStream, msgBody);
            memStream.Seek(0, SeekOrigin.Begin);
            byte[] body = new byte[memStream.Length];
            memStream.Read(body, 0, (int)memStream.Length);
            string s = ASCIIEncoding.ASCII.GetString(body);
            var msg =  ByteStreamMessage.CreateMessage(new ArraySegment<byte>(Encoding.UTF8.GetBytes(s)));
            msg.Properties["WebSocketMessageProperty"] = new WebSocketMessageProperty { MessageType = WebSocketMessageType.Text };
            return msg;
        }

        public static T Deserialize<T>(string msg)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));
            var memStream = new MemoryStream(ASCIIEncoding.ASCII.GetBytes(msg));
            return (T)jsonSerializer.ReadObject(memStream);
        }

        public static string SerializeString<T>(T msgBody)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));
            var memStream = new MemoryStream();
            jsonSerializer.WriteObject(memStream, msgBody);
            memStream.Seek(0, SeekOrigin.Begin);
            byte[] body = new byte[memStream.Length];
            return ASCIIEncoding.ASCII.GetString(body);
        }
    }
}
