using System.Net;
namespace Middleware;

using Helpers;

public abstract class AbstractMiddleware {
    public AbstractMiddleware? next;
    public abstract Task HandleRequest(Context context);

    public AbstractMiddleware(AbstractMiddleware? next = null) {
        this.next = next;
    }
}