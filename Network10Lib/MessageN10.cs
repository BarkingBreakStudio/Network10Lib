using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Network10Lib
{
    public  class MessageN10
    {

        public enum EnumMsgType
        {
            Tcp,
            Health,
            ClientHandshake,
            ServerHandshake,
        }

        public int Sender { get; set; }
        public int Receiver { get; set; }
        public EnumMsgType MsgType { get; set; }
        public object Data { get; set; } = "";

        public string Serialize()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        public static MessageN10? TryDeserialize(string s)
        {
            try
            {
                return JsonSerializer.Deserialize<MessageN10>(s, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public T? DeserializeData<T>()
        {
            if (Data is not null)
            {
                return JsonSerializer.Deserialize<T>((JsonElement)Data, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
            else
            {
                return default(T);
            }
        }

        public override string ToString()
        {
            //T? data = DeserializeData<T>();
            return $"Message{{ Sender: {Sender}, Receiver: { Receiver}, MsgType: { MsgType } Data: { Data.ToString() }  }}";
        }

    }
}
