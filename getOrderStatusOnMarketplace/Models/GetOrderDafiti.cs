using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Ainda não implementado
namespace GetOrderDafiti.Models
{
    public class OrderRootDafiti
    {
        [System.Xml.Serialization.XmlElement("OrderId")]
        public string orderNumber { get; set; }

        [System.Xml.Serialization.XmlElement("Statuses")]
        public string orderStatus { get; set; }
    }
}
