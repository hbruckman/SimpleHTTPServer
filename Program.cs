namespace SimpleHTTPServer;

public class Program
{
	public static async Task Main(string[] args)
	{
		SimpleHTTPServer server = new SimpleHTTPServer();

		Task serverListenTask = Task.Run(() => server.StartListening());
		await Task.Delay(5000);

		server.StopListening();
		await serverListenTask;
	}
}
