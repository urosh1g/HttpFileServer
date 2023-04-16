using System.Net;
namespace Middleware;

public class LoggingMiddleware : AbstractMiddleware {
    public LoggingMiddleware(AbstractMiddleware? next = null) : base(next) {

    }
    public override async Task HandleRequest(HttpListenerContext context){
        var req = context.Request;
        var res = context.Response;
        Console.WriteLine("[Logging middleware begin]");
        Console.WriteLine($"{req.HttpMethod} {req.Url}");
        Console.WriteLine($"{req.Headers}");
        if(next != null) {
            await Task.Run(() => next.HandleRequest(context));
        }
        Console.WriteLine("Sending response");
        Console.WriteLine($"{res.StatusCode} {res.StatusDescription}");
        Console.WriteLine($"{res.Headers}");
        Console.WriteLine("Response sent succesfully!");
        Console.WriteLine("[Logging middleware end]");
    }
}