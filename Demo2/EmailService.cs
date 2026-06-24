namespace Demo2;

public class EmailService:IMessageService
{
    public void Send(string message)
    {
        Console.WriteLine($"Sending Email: {message}");
    }
}