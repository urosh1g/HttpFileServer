using System.Net;
namespace Helpers;

public class Context {
    public HttpListenerContext httpContext;
    public MemoryStream outputStream;

    public Context(HttpListenerContext httpContext){
        this.httpContext = httpContext;
        this.outputStream = new MemoryStream();
    }
}