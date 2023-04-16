using System.Net;
using System.Collections.Generic;

namespace Middleware;

using Caching;
using Helpers;

public class CacheMiddleware : AbstractMiddleware {

    private LRUCache cache;
    public CacheMiddleware(AbstractMiddleware? next = null) : base(next) {
        cache = new LRUCache();
    }
    public override async Task HandleRequest(Context context){
        Console.WriteLine("[Caching middleware begin]");
        int key = context.httpContext.Request.RawUrl!.GetHashCode();
        var body = cache.get(key);
        var res = context.httpContext.Response;
        if(body != null) {
            Console.WriteLine($"Cache hit for {context.httpContext.Request.RawUrl}");
            context.outputStream.Write(body);
            return;
        }
        else {
            Console.WriteLine("Cache miss, calling next handler");
            if(next != null) {
                await Task.Run(() => next.HandleRequest(context));
            }
            Console.WriteLine("Saving response to cache");
            cache.insert(key, context.outputStream.GetBuffer());
        }
        Console.WriteLine("[Caching middleware end]");
    }
}