using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetOrderNetshoes.Models
{
    public class OrderRootNetshoes
    {
        [JsonProperty("orderNumber")]
        public string orderNumber { get; set; }

        [JsonProperty("orderStatus")]
        public string orderStatus { get; set; }
    }
}
