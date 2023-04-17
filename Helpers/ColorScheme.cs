namespace Helpers;

public class ColorScheme {
    public ConsoleColor? Foreground {get; set;}
    public ConsoleColor? Background {get; set;}

    public ColorScheme(ConsoleColor? fg = null, ConsoleColor? bg = null){
        Foreground = fg;
        Background = bg;
    }
}