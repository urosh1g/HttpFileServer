using System.Net;
namespace Middleware;

using Helpers;

public class TestMiddleware : AbstractMiddleware {
    public TestMiddleware(AbstractMiddleware? next = null): base(next) {}

    public override async Task HandleRequest(Context context) {
        Console.WriteLine("[TestMiddleware begin]");
        if(next != null) {
            await Task.Run(() => next.HandleRequest(context));
        }
        Console.WriteLine("[TestMiddleware end]");
    }
}