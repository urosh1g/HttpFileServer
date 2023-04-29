using System.Net;
namespace Middleware;

using Helpers;

public abstract class AbstractMiddleware {
    public AbstractMiddleware? next;
    protected ColorScheme? colorScheme;
    public abstract void HandleRequest(Context context);

    public AbstractMiddleware(ColorScheme? colorScheme, AbstractMiddleware? next = null) {
        this.next = next;
        this.colorScheme = colorScheme;
    }

    protected void StartColor(){
        if(colorScheme == null){
            return;
        }
        if(colorScheme.Background != null){
            Console.BackgroundColor = (ConsoleColor)colorScheme.Background;
        }
        if(colorScheme.Foreground != null){
            Console.ForegroundColor = (ConsoleColor)colorScheme.Foreground;
        }
    }

    protected void StopColor() {
        Console.ResetColor();
    }
}