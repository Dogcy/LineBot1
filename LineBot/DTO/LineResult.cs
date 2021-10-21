using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LineBot.Propertys
{
    /// <summary>
    /// 暫時沒用
    /// </summary>
    public class LineResult
    {
        [JsonPropertyName("destination")]
        public string destination { get; set; }
        [JsonPropertyName("events")]
        public IEnumerable<events> events { get; set; }
    }


    public class events
    {
        [JsonPropertyName("replyToken")]
        public string replyToken { get; set; }
        [JsonPropertyName("type")]
        public string type { get; set; }
        [JsonPropertyName("mode")]
        public string mode { get; set; }
        [JsonPropertyName("timestamp")]
        public int timestamp { get; set; }
        [JsonPropertyName("source")]
        public source source { get; set; }
        [JsonPropertyName("message")]
        public message message { get; set; }
    }
    public class source
    {
        [JsonPropertyName("type")]
        public string type { get; set; }
        [JsonPropertyName("userId")]
        public string userId { get; set; }

    }
    public class message
    {
        [JsonPropertyName("id")]
        public string id { get; set; }
        [JsonPropertyName("type")]
        public string type { get; set; }
        [JsonPropertyName("text")]
        public string text { get; set; }
    }
}
