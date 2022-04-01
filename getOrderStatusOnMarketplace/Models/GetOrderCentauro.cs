using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetOrderCentauro.Models
{
    public class OrderRootCentauro
    {
        [JsonProperty("orderData")]
        public OrderData orderData { get; set; }
    }

    public class OrderData
    {
        [JsonProperty("id")]
        public string orderNumber { get; set; }

        [JsonProperty("status")]
        public string orderStatus { get; set; }
    }
}
