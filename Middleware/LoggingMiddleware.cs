using System.Net;
namespace Middleware;

using Helpers;

public class LoggingMiddleware : AbstractMiddleware {
    public LoggingMiddleware(ColorScheme? colorScheme = null, AbstractMiddleware? next = null) : base(colorScheme, next) {

    }
    public override async Task HandleRequest(Context context){
        var req = context.httpContext.Request;
        var res = context.httpContext.Response;
        StartColor();
        Console.WriteLine("--------------------------");
        Console.WriteLine("[Logging middleware begin]");
        Console.WriteLine($"{req.HttpMethod} {req.RawUrl}");
        Console.WriteLine($"{req.Headers}");
        StopColor();
        if(next != null) {
            await next.HandleRequest(context);
        }
        StartColor();
        Console.WriteLine("Sending response");
        Console.WriteLine($"{res.StatusCode} {res.StatusDescription}");
        Console.WriteLine($"{res.Headers}");
        Console.WriteLine("Response created, sending");
        Console.WriteLine("[Logging middleware end]");
        Console.WriteLine("--------------------------");
        StopColor();
    }
}