using RestSharp;
using Newtonsoft.Json;
using GetOrder.Models;
using System.Threading;

void returnStatus(string code, string market)
{
    string[] listStrLine = code.Split(',');//Divide os códigos
    List<string> codes = new List<string>(); //conjunto de string[] deve ser repassado para List<> por meio de foreach

    foreach (string str in listStrLine)
    {
        codes.Add(str); //adiciona na lista
    }


    //Determina o marketplace que será passado na requisição
    switch (market)
    {
        case "51":
            market = "Netshoes"; //Os nomes dos marketplaces e seus códigos identificadores devem ser alterados conforme o seu sistema
            break;

        case "41":
            market = "Dafiti";
            break;

        case "77":
            market = "Centauro";
            break;

        case "4":
            market = "Mercadolivre";
            break;

        default:
            market = "iHub";
            break;
    }

    List<string> orders = new List<string>(); //cria nova lista para armezar um index do foreach

    foreach (string order in codes)
    {
        orders.Add(order);

        var client = new RestClient($"/* Insira aqui a sua rota e ajuste os parâmetros na posição correta */{market}/{order}");
        client.Timeout = -1;
        var request = new RestRequest(Method.GET);

        IRestResponse response = client.Execute(request);

        var data = JsonConvert.DeserializeObject<GetOrderRoot>(response.Content); //Deserializa o JSON conforme o objeto mapeado (GetOrderRoot)
        var orderStatus = data.orderStatus; //Pega as informações do JSON formatado
        var orderNumber = data.orderNumber;

        Console.WriteLine("Marketplace;" + orderNumber + ";" + orderStatus + ";");

        if(orders.Count == 20)
        {
            if (market == "Dafiti")
            {
                Console.WriteLine("Entrando em modo de economia de requisições, por favor aguarde 2 minutos..");
                Thread.Sleep(120000); //Limitador de tempo para impedir o excedimento de requisições na API
            }
            else
            {
                Console.WriteLine("Entrando em modo de economia de requisições, por favor aguarde 1 minuto..");
                Thread.Sleep(60000);
            }

            orders.Clear(); //Limpa o array dos pedidos
        }
    }
}

Console.WriteLine("Informe o marketplace");
var market = Console.ReadLine();

Console.WriteLine("Insira a lista de pedidos. Ex.: 80236978,89556632");
string code = Console.ReadLine();
Console.Clear();

returnStatus(code, market);