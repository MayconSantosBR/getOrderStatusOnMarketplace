using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetOrderIhub.Models
{
    public class OrderRootIhub
    {
        [JsonProperty("status")]
        public string orderStatus { get; set; } 

        [JsonProperty("originOrderId")]
        public string orderNumber { get; set; }

        [JsonProperty("packageAttachment")]
        public PackageAttachment package { get; set; }
    }

    public class PackageAttachment
    {
        [JsonProperty("packages")]
        public List<Packages> info { get; set; }
    }

    public class Packages
    {
        [JsonProperty("invoiceNumber")]
        public string invoiceNumber { get; set; }

        [JsonProperty("issuanceDate")]
        public string issuanceDate { get; set; }

        [JsonProperty("invoiceKey")]
        public string invoiceKey { get; set; }

        [JsonProperty("carrierName")]
        public string carrierName { get; set; }

        [JsonProperty("trackingNumber")]
        public string trackingNumber { get; set; }

        [JsonProperty("trackingUrl")]
        public string trackingUrl { get; set; }
    }
}
