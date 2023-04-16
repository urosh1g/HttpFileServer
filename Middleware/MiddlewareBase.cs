using System.Net;
namespace Middleware;

public abstract class AbstractMiddleware {
    public AbstractMiddleware? next;
    public abstract Task HandleRequest(HttpListenerContext context);

    public AbstractMiddleware(AbstractMiddleware? next = null) {
        this.next = next;
    }
}