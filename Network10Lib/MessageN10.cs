using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Network10Lib
{
    public  class MessageN10
    {

        public enum EnumMsgType
        {
            Tcp,
            ClientHandshake,
            ServerHandshake,
        }

        /// <summary>
        /// Player number of the sender of this message
        /// </summary>
        public int Sender { get; set; }
        /// <summary>
        /// Player number of the receiver of this message
        /// </summary>
        public int Receiver { get; set; }
        /// <summary>
        /// only Tcp is valid for ex ternal messages
        /// </summary>
        public EnumMsgType MsgType { get; set; }
        /// <summary>
        /// Json converted object to transfer
        /// </summary>
        [JsonInclude]
        public string dataJson {get;private set; } = "";

        /// <summary>
        /// Data which should be transferred
        /// </summary>
        [JsonIgnore]
        public object Data
        {
            set { dataJson = JsonSerializer.Serialize(value, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }); }
        }

        /// <summary>
        /// Serialize the whole message object
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        /// <summary>
        /// Deserialize JSON string of a message
        /// </summary>
        /// <param name="s">JSON string of a message</param>
        /// <returns></returns>
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

        /// <summary>
        /// Deserialize the tranferred data
        /// </summary>
        /// <typeparam name="T">type of tranferred data</typeparam>
        /// <returns>deserialized data or null on fail</returns>
        public T? DeserializeData<T>()
        {
            if (dataJson is not null)
            {
                return JsonSerializer.Deserialize<T>(dataJson, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// clone message efficiently if you want to send the same message to a different client
        /// </summary>
        /// <param name="otherReceiver">received of the clones message</param>
        /// <returns>cloned message</returns>
        public MessageN10 Clone4otherReceiver(int otherReceiver)
        {
            return new MessageN10 { Sender = Sender, Receiver = otherReceiver, MsgType = MsgType, dataJson = dataJson };
        }

        public override string ToString()
        {
            return $"Message{{ Sender: {Sender}, Receiver: { Receiver}, MsgType: { MsgType } Data: { dataJson }  }}";
        }

    }
}
