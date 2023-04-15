namespace MyHttpFileServer;

public class MainClass {
    public static void Main(string[] args){
        HttpFileServer fileServer = new HttpFileServer("localhost");
        fileServer.Start();
    }
}