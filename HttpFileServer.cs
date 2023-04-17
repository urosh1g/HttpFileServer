using System;
using System.IO;
using System.Net;
using System.Text;

using Middleware;
using Helpers;

namespace MyHttpFileServer;

class HttpFileServer : AbstractMiddleware {
    short port;
    string address;
    HttpListener listener;
    AbstractMiddleware middlewares;
    bool stop = false;
    string[] fileNames = null!;

    public HttpFileServer(string address, short port = 5050): base(null){
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

    public HttpFileServer Use(AbstractMiddleware middleware) {
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
                    Context fakeContext = new Context(context);
                    await Task.Run(() => middlewares.HandleRequest(fakeContext));
                    context.Response.OutputStream.Write(fakeContext.outputStream.GetBuffer());
                    context.Response.Close();
                });
            }
        }
        catch(Exception ex){
            Console.WriteLine(ex.Message);
        }
    }

    public override async Task HandleRequest(Context ctx) {
        var req = ctx.httpContext.Request;
        var res = ctx.httpContext.Response;
        string fileName = req.RawUrl!.Split("/")[1];
        string responseBody;

        if(!fileNames.Contains(fileName)) {
            res.ContentType = "text/html";
            responseBody = @"
            <html>
                <head>
                    <title>File list</title>
                </head>
                <body>";
            foreach(var file in fileNames){
                responseBody += $"<a href='http://{address}:{port}/{file}'>{file}</a>\n";
            }
            responseBody += "</body></html>";
            res.ContentLength64 = responseBody.Length;
            ctx.outputStream.Write(Encoding.ASCII.GetBytes(responseBody));
        }
        else {
            await Task.Run(() => WriteFile(ctx, fileName));
        } 
    }

    private void WriteFile(Context context, string fileName){
        try {
            var fileContents = File.ReadAllBytes(fileName);
            context.httpContext.Response.ContentLength64 = fileContents.Length;
            context.httpContext.Response.Headers.Add("Content-Disposition", "attachment");
            context.outputStream.Write(fileContents);
        }
        catch(Exception ex){
            Console.WriteLine(ex.Message);
        }
        return; 
    }
}