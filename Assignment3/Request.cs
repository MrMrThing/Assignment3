using System;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Assignment3;

public class Request
{
	public string Method { get; set; }
	public string Path { get; set; }
	public string Date { get; set; }
	public string Body { get; set; }

    private List<string> Errors = new List<string>(20);

    public void checkMethod()
	{
        if (Method == null)
		{
            Errors.Add("missing method");
			return;
		}
		else if (Method is "create" or "read" or "update" or "delete" or "echo")
		{
			return;
		}
        Errors.Add("illegal method");
        return;

    }

	public void checkPath() {

		if (Path == null)
		{
			Errors.Add("missing resource");
			return;
		}
		if (Path.Contains("testing"))
		{
			return;
		}
		else if (!Path.Contains("/api/categories"))
		{
			Errors.Add("4 Bad Request");
			return;
		}
		else if (checkPathId(Path) == false) {
            Errors.Add("4 Bad Request");
			Console.WriteLine("Here");
            return;
        }
		return;
	
	}

	public bool checkPathId(String path) {

		var tempArr = path.Split('/');
		Console.WriteLine(tempArr.Length);
		if (tempArr.Length > 3)
		{
			var cid = tempArr[2];
			if (int.TryParse(cid, out int tempNum))
			{
                Console.WriteLine("Is int");
                return true;
            }
			else 
			{
                Console.WriteLine("Is not int");
                return false;
            }

		}
		return false;
	}

	public void checkDate() {

		if (Date == null)
		{
            Errors.Add("missing Date");
            return;
		}
		else if (Date.Contains("-") || Date.Contains(":")) {
            Errors.Add("illegal date");
            return;
		}
	
	}

	public void checkBody() {

		if (Body == null) {
            Errors.Add("missing body");
            return;
		}

		try {
			var tempObj = JsonDocument.Parse(Body) != null;
			return;
		} 
		catch {
            Errors.Add("illegal body");
            return;
		}


    }

	public Response CheckRunThough() 
	{
        Response response = new Response();

		checkMethod();
		checkPath();
		checkDate();
		checkBody();

		foreach (var error in Errors)
		{
			if (Errors.Contains("4 Bad Request")) {
				response.Status = "4 Bad Request";
				break;
            }
			response.Status += error;
			response.Status += ", ";
		}

		return response;


	}


}
