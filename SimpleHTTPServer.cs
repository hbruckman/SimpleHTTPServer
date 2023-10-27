namespace SimpleHTTPServer;

using System.Collections.Specialized;
using System.Net;
using System.Text;

public class SimpleHTTPServer
{
	private HttpListener server;
	private bool isListening;

	public SimpleHTTPServer()
	{
		server = new HttpListener();
		server.Prefixes.Add("http://127.0.0.1:8080/");
		isListening = false;
	}

	public void StartListening()
	{
		server.Start();

		isListening = true;

		while (isListening)
		{
			try
			{
				HttpListenerContext ctx = server.GetContext();
				Task.Run(() => HandleContext(ctx));
			}
			catch
			{

			}
		}
	}

	public void StopListening()
	{
		isListening = false;
		server.Stop();
	}

	private void HandleContext(HttpListenerContext ctx)
	{
		HttpListenerRequest req = ctx.Request;
		NameValueCollection q = req.QueryString;
		StreamReader sr = new StreamReader(req.InputStream);
		string reqBody = sr.ReadToEnd();
		q.Add(System.Web.HttpUtility.ParseQueryString(reqBody));

		string content = @$"
		<!DOCTYPE html>
		<html>
		  <body>
		    <h1>Hi, {q["name"]}!</h1>
		  </body>
		</html>
		";

		byte[] resBody = Encoding.UTF8.GetBytes(content);
		HttpListenerResponse res = ctx.Response;
		res.ContentEncoding = Encoding.UTF8;
		res.ContentLength64 = resBody.Length;
		res.ContentType = "text/html";
		res.StatusCode = (int) HttpStatusCode.OK;
		res.OutputStream.Write(resBody);
		res.Close();
	}
}
