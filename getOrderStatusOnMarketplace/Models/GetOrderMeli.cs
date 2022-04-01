using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetOrderMeli.Models
{
    public class OrderRootMeli
    {
        [JsonProperty("id")]
        public string orderNumber { get; set; }

        [JsonProperty("shipping")]
        public Shipping shipping { get; set; }
    }

    public class Shipping
    {
        [JsonProperty("external_reference")]
        public string carrinhoOrderNumber { get; set; }

        [JsonProperty("status")]
        public string orderStatus { get; set; }
    }
}
