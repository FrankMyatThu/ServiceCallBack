using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RtdbService.Models
{
    public class RtdbValueModel
    {
        [JsonProperty("value")]
        public int Value { get; set; }
    }
}
