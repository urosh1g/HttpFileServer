namespace MyHttpFileServer;

using Middleware;

public class MainClass {
    public static void Main(string[] args){
        HttpFileServer fileServer = new HttpFileServer("localhost");
        fileServer
            .Use(new CacheMiddleware())
            .Use(new LoggingMiddleware());
        fileServer.Start();
    }
}