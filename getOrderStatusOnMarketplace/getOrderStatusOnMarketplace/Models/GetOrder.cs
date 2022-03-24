using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetOrder.Models
{
    public class GetOrderRoot //Mapeamento do JSON
    {
        [JsonProperty("orderNumber")]
        public string orderNumber { get; set; }

        [JsonProperty("orderStatus")]
        public string orderStatus { get; set; }
    }
}
