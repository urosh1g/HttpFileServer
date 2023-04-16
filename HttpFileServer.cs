using System;
using System.IO;
using System.Net;
using System.Text;

using Middleware;

namespace MyHttpFileServer;

class HttpFileServer : AbstractMiddleware {
    short port;
    string address;
    HttpListener listener;
    AbstractMiddleware middlewares;
    bool stop = false;
    string[] fileNames = null!;

    public HttpFileServer(string address, short port = 5050){
        this.address = address;
        this.port = port;

        fileNames = Directory.GetFiles(".")
            .Select((fileName) => fileName.Split("/")[1])
            .ToArray();

        listener = new HttpListener();
        listener.Prefixes.Add($"http://{address}:{port}/");
        middlewares = this;
        middlewares.next = null;
    }

    public HttpFileServer Use<T>(T middleware) where T : AbstractMiddleware {
        middleware.next = middlewares;
        middlewares = middleware;
        return this;
    }

    public void Start() {
        try {
            listener.Start();
            Console.WriteLine($"Started listening @ http://{address}:{port}");
            while(!stop) {
                var context = listener.GetContext();
                ThreadPool.QueueUserWorkItem(async (state) => {
                    await Task.Run(() => middlewares.HandleRequest(context));
                });
            }
        }
        catch(Exception ex){
            Console.WriteLine(ex.Message);
        }
    }

    public override async Task HandleRequest(HttpListenerContext ctx) {
        var req = ctx.Request;
        var res = ctx.Response;
        string fileName = req.RawUrl!.Split("/")[1];
        string responseBody;

        if(!fileNames.Contains(fileName)) {
            res.ContentType = "text/plain";
            res.StatusCode = (int)HttpStatusCode.NotFound;
            responseBody = $"cannot find {fileName}";
            res.ContentLength64 = responseBody.Length;
            res.OutputStream.Write(Encoding.ASCII.GetBytes(responseBody));
            res.OutputStream.Close();
        }
        else {
            await Task.Run(() => WriteFile(res, fileName));
        } 
    }

    private void WriteFile(HttpListenerResponse response, string fileName){
        try {
            var fileContents = File.ReadAllBytes(fileName);
            response.ContentLength64 = fileContents.Length;
            response.OutputStream.Write(fileContents);
            response.OutputStream.Close();
        }
        catch(Exception ex){
            Console.WriteLine(ex.Message);
        }
        return; 
    }
}