using RestSharp;
using Newtonsoft.Json;
using GetOrderNetshoes.Models;
using GetOrderIhub.Models;
using System.Threading;
using GetOrderDafiti.Models;
using System.Xml.Serialization;
using System.Xml;
using GetOrderCentauro.Models;
using GetOrderMeli.Models;
using Newtonsoft.Json.Linq;
using System.Linq;

//Cria a função
void returnStatus(string code, string market)
{
    //Divide os códigos
    string[] listStrLine = code.Split(',');
    List<string> codes = new List<string>(); //conjunto de string[] deve ser repassado para List<> por meio de foreach

    foreach (string str in listStrLine)
    {
        codes.Add(str); //adiciona na lista
    }

    List<string> orders = new List<string>(); //cria nova lista para armezar um index do foreach

    Console.WriteLine("Pedido;" + 
        $"Status {market.ToUpper()};" + 
        "Status ERP;Monitoria;invoiceNumber;invoiceKey;invoiceDate;carrierName;trackingNumber;trackingUrl;"); //cabeçalho para planilhas com separador ';'

    foreach (string order in codes)
    {
        orders.Add(order);

        //Declara os objetos modelos para o Json
        OrderRootNetshoes orderNetshoes;
        OrderRootIhub orderIhub;
        OrderRootCentauro orderCentauro;
        OrderRootMeli orderMeli;

        //Declara variáveis para armazenar valor
        var orderStatus = "";
        var orderNumber = "";
        var orderNumberMeli = "";
        var orderStatusErp = "";
        var comparator = "";
        var invoiceNumber = "";
        var invoiceKey = "";
        var invoiceDate = "";
        var carrierName = "";
        var trackingNumber = "";
        var trackingUrl = "";

        //Busca status do marketplace requerido
        try
        {

            var client = new RestClient($"//insira sua rota aqui e ajuste os parâmetros{market}/{order}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);

            IRestResponse response = client.Execute(request);

            if (market == "netshoes")
            {
                //Deserializa o Json no formato do objeto antes declarado
                orderNetshoes = JsonConvert.DeserializeObject<OrderRootNetshoes>(response.Content);
                orderStatus = orderNetshoes.orderStatus.ToLower();
                orderNumber = orderNetshoes.orderNumber;

                if(orderStatus == "shipped")
                {
                    orderStatus = "dispatched";
                }
            }

            if (market == "ihub")
            {
                //Deserializa o Json no formato do objeto antes declarado
                orderIhub = JsonConvert.DeserializeObject<OrderRootIhub>(response.Content);
                orderStatus = orderIhub.orderStatus.ToLower();
                orderNumber = orderIhub.orderNumber;

                //Faz a tratativa dos status
                if(orderStatus == "handling")
                {
                    orderStatus = "approved";
                }
            }

            //Futura implementação em XML
            //if (market == "dafiti")
            //{
            //    OrderRootDafiti orderDafiti = null;
            //    string path = $"{response.Content}";

            //    XmlSerializer serializer = new XmlSerializer(typeof(OrderRootDafiti));

            //    StreamReader reader = new StreamReader(path);

            //    orderDafiti = (OrderRootDafiti)serializer.Deserialize(reader);
            //    reader.Close();


            //    orderStatus = from i in orderDafiti.orderStatus.ElementAt("Statuses")
            //    orderNumber = orderDafiti.orderNumber.ToString();
            //}

            //Detecta o marketplace digitado pelo usuário
            if (market == "centauro")
            {
                //Deserializa o Json no formato do objeto antes declarado
                orderCentauro = JsonConvert.DeserializeObject<OrderRootCentauro>(response.Content);
                orderStatus = orderCentauro.orderData.orderStatus.ToString().ToLower();
                orderNumber = orderCentauro.orderData.orderNumber.ToString();

                //Faz a tratativa dos status
                if (orderStatus == "not_approved")
                {
                    orderStatus = "canceled";
                }
                else if(orderStatus == "sent")
                {
                    orderStatus = "dispatched";
                }

            }

            //Detecta o marketplace digitado pelo usuário
            if (market == "mercadolivre")
            {
                //Deserializa o Json no formato do objeto antes declarado
                orderMeli = JsonConvert.DeserializeObject<OrderRootMeli>(response.Content);
                orderStatus = orderMeli.shipping.orderStatus.ToLower();
                orderNumber = orderMeli.orderNumber;
                orderNumberMeli = orderMeli.shipping.carrinhoOrderNumber;

                //Faz a tratativa dos status
                if (orderStatus == "pending")
                {
                    orderStatus = "approved";
                }

                if(orderStatus == "shipped")
                {
                    orderStatus = "dispatched";
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            continue;
        }

        //Busca status do ERP para comparação
        if (market != "ihub")
        {
            //Busca status no ERP
            try
            {
                var client = new RestClient($"//sua requisição com o ajuste de parâmetros{order}");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);

                IRestResponse response = client.Execute(request);

                //Se não encontrado o pedido
                if (response.Content == "NOTFOUND")
                {
                    var clientTest = new RestClient($"//sua requisição com o ajuste de parâmetros{orderNumberMeli}"); //Refaz a requisição com o código alternativo
                    client.Timeout = -1;
                    var requestTest = new RestRequest(Method.GET);

                    IRestResponse responseTest = client.Execute(requestTest);

                    if(responseTest.Content == "NOTFOUND") //Define como "Não encontrado" após a segunda tentativa
                    {
                        orderStatusErp = "Pedido não encontrado";
                    }
                    else  //Deserializa o Json no formato do objeto antes declarado
                    {
                        orderIhub = JsonConvert.DeserializeObject<OrderRootIhub>(responseTest.Content);
                        orderStatusErp = orderIhub.orderStatus.ToLower();
                        invoiceNumber = orderIhub.package.info.FirstOrDefault().invoiceNumber.ToString();
                        invoiceKey = orderIhub.package.info.FirstOrDefault().invoiceKey.ToString();
                        invoiceDate = orderIhub.package.info.FirstOrDefault().issuanceDate.ToString();
                        carrierName = orderIhub.package.info.FirstOrDefault().carrierName.ToString();
                        trackingNumber = orderIhub.package.info.FirstOrDefault().trackingNumber.ToString();
                        trackingUrl = orderIhub.package.info.FirstOrDefault().trackingUrl.ToString();

                        //Faz trativa dos status
                        if (orderStatus == "handling")
                        {
                            orderStatus = "approved";
                        }
                    }
                }
                else //Deserializa o Json no formato do objeto antes declarado
                {
                    orderIhub = JsonConvert.DeserializeObject<OrderRootIhub>(response.Content);
                    orderStatusErp = orderIhub.orderStatus.ToLower();
                    invoiceNumber = orderIhub.package.info.FirstOrDefault().invoiceNumber.ToString();
                    invoiceKey = orderIhub.package.info.FirstOrDefault().invoiceKey.ToString();
                    invoiceDate = orderIhub.package.info.FirstOrDefault().issuanceDate.ToString();
                    carrierName = orderIhub.package.info.FirstOrDefault().carrierName.ToString();
                    trackingNumber = orderIhub.package.info.FirstOrDefault().trackingNumber.ToString();
                    trackingUrl = orderIhub.package.info.FirstOrDefault().trackingUrl.ToString();

                    //Faz a tratativa do status
                    if (orderStatus == "handling")
                    {
                        orderStatus = "approved";
                    }
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                continue;
            }
        }

        //Comparação entre as duas plataformas
        if (orderStatus.Equals(orderStatusErp))
        {
            comparator = "Correto";
        }
        else
        {
            comparator = "Divergente";
        }

        //Define a forma em que irá imprimir a mensagem
        if (comparator.Equals("Correto")){
            Console.WriteLine($"{orderNumber};" 
                + $"{orderStatus};" 
                + $"{orderStatusErp};" 
                + $"{comparator};");
        }
        else
        {
            Console.WriteLine($"{orderNumber};"
                + $"{orderStatus};"
                + $"{orderStatusErp};"
                + $"{comparator};"
                + $"{invoiceNumber};"
                + $"{invoiceKey};"
                + $"{invoiceDate};"
                + $"{carrierName};"
                + $"{trackingNumber};"
                + $"{trackingUrl};");
        }

        //Ajuste do limite de requisições em um determinado tempo
        if (orders.Count == 20)
        {
            if (market == "Dafiti")
            {
                Console.WriteLine("Entrando em modo de economia de requisições, por favor aguarde 2 minutos..");
                Thread.Sleep(120000); //2 minutos
            }
            else
            {
                Console.WriteLine("Entrando em modo de economia de requisições, por favor aguarde 1 minuto..");
                Thread.Sleep(60000); //1 minuto
            }

            orders.Clear(); //Limpa a lista de pedidos
        }
    }
}

//Pede informações ao usuário
Console.WriteLine("Informe o marketplace");
var market = Console.ReadLine();

Console.WriteLine("Insira a lista de pedidos. Ex.: 80236978,89556632");
string code = Console.ReadLine();
Console.Clear();

//Chama a função
returnStatus(code, market);