using System.Net;
namespace Middleware;

using Helpers;

public class TestMiddleware : AbstractMiddleware {
    public TestMiddleware(ColorScheme? colorScheme = null, AbstractMiddleware? next = null): base(colorScheme, next) {}

    public override void HandleRequest(Context context) {
        Console.WriteLine("[TestMiddleware begin]");
        if(next != null) {
            next.HandleRequest(context);
        }
        Console.WriteLine("[TestMiddleware end]");
    }
}