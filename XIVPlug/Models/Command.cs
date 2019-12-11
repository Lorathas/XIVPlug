using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XIVPlug.Models
{
    public class Command
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public CommandType Type { get; set; }

        public double Value { get; set; }
        public long DurationMillis { get; set; }

        [JsonIgnore]
        public TimeSpan Duration
        {
            get => TimeSpan.FromMilliseconds(DurationMillis);
            set => DurationMillis = (long) value.TotalMilliseconds;
        }
    }
}