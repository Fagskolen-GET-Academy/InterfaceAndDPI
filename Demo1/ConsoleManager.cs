namespace Demo1;

public static class ConsoleManager
{
    public static void Write(IShape shape)
    {
        Console.WriteLine(shape.Area());
    }
}