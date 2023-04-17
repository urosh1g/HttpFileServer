namespace MyHttpFileServer;

using Middleware;
using Helpers;

public class MainClass {
    public static void Main(string[] args){
        HttpFileServer fileServer = new HttpFileServer("localhost");
        fileServer
            .Use(new CacheMiddleware())
            .Use(new LoggingMiddleware(new ColorScheme(fg: ConsoleColor.Blue)));
        fileServer.Start();
    }
}