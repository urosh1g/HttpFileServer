using System.Net;
using System.Text;

namespace MyHttpFileServer;

class HttpFileServer {
    short port;
    string address;
    HttpListener listener;
    bool stop = false;

    public HttpFileServer(string address, short port = 5050){
        this.address = address;
        this.port = port;
        listener = new HttpListener();
        listener.Prefixes.Add($"http://{address}:{port}/");
    }

    public void Start() {
        listener.Start();
        Console.WriteLine($"Started listening @ http://{address}:{port}");
        while(!stop) {
            var context = listener.GetContext();
            ThreadPool.QueueUserWorkItem((state) => {
                HandleRequest(context);
            });
        }
    }

    private void HandleRequest(HttpListenerContext ctx) {
        var res = ctx.Response;
        Console.WriteLine("Handling request");
        Console.WriteLine($"{ctx.Request.HttpMethod} {ctx.Request.RawUrl}");
        res.ContentType = "text/plain";
        string responseBody = $"hello from {ctx.Request.RawUrl}";
        res.ContentLength64 = responseBody.Length;
        res.OutputStream.Write(Encoding.ASCII.GetBytes(responseBody));
        res.OutputStream.Close();
        Console.WriteLine("Sending response");
    }
}