using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class OdataResponse<T>
    {
        [JsonProperty("@odata.context")]

        public string Metadata { get; set; }

        public T Value { get; set; }
    }
}
