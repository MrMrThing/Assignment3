using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Assignment3;


var server = new TcpListener(IPAddress.Parse("127.0.0.1"), 5000);
server.Start();

Console.WriteLine("Server is started...");


var categories = new List<object>
            {
                new {cid = 1, name = "Beverages"},
                new {cid = 2, name = "Condiments"},
                new {cid = 3, name = "Confections"}
            };

while (true)
{
    var client = server.AcceptTcpClient();
    Console.WriteLine("Client accepted...");

    var stream = client.GetStream();

    var buffer = new byte[1024];

    var response = new Response();

    try
    {

        var request = client.ReadRequest();

        Console.WriteLine(request.Method);
        Console.WriteLine(request.Path);
        Console.WriteLine(request.Date);
        Console.WriteLine(request.Body);

        response = request.CheckRunThough();

        if (response.Status != "4 Bad Request") 
        {
            if (request.Method == "echo") 
            {
                response.Status = "1 ok";
                response.Body = request.Body;
            }
            if (request.Method == "read" && request.Path != null) 
            {
                var tempArr = request.Path.Split('/');
                if (tempArr.Length > 1) {
                    response.Status = "1 ok";
                    int tempNum = Convert.ToInt32(tempArr[2]);
                    response.Body = categories[tempNum].ToJson();
                }
                response.Status = "1 ok";
                response.Body = categories.ToJson();
            }
        }


        var JsonResponse = response.ToJson();
        client.SendResponse(JsonResponse);

        Console.WriteLine(JsonResponse);
        Console.WriteLine("\n");
        Console.WriteLine("--------------------------");
        Console.WriteLine("\n");


    }
    catch (Exception e) 
    {
        Console.WriteLine(e);
    }

    //stream.Write(request);


    stream.Close();
}