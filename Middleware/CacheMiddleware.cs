using System.Net;
using System.Collections.Generic;

namespace Middleware;

using Caching;
using Helpers;

public class CacheMiddleware : AbstractMiddleware {

    private LRUCache cache;
    private object locker;
    public CacheMiddleware(int capacity = 32, ColorScheme? colorScheme = null, AbstractMiddleware? next = null) : base(colorScheme, next) {
        cache = new LRUCache(capacity);
        locker = new object();
    }
    public override async Task HandleRequest(Context context){
        StartColor();
        Console.WriteLine("[Caching middleware begin]");
        int key = context.httpContext.Request.RawUrl!.GetHashCode();
        var body = cache.get(key);
        var res = context.httpContext.Response;
        if(body != null) {
            Console.WriteLine($"Cache hit for {context.httpContext.Request.RawUrl}");
            context.outputStream.Write(body);
        }
        else {
            Console.WriteLine("Cache miss, calling next handler");
            StopColor();
            if(next != null) {
                await next.HandleRequest(context);
            }
            StartColor();
            Console.WriteLine("Saving response to cache");
            cache.insert(key, context.outputStream.GetBuffer());
        }
        Console.WriteLine("[Caching middleware end]");
        StopColor();
    }
}