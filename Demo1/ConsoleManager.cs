namespace Demo1;

public class ConsoleManager
{
    public static void Write(IShape shape)
    {
        Console.WriteLine(shape.Area());
    }
}