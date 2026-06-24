namespace Demo2;

public class SmsService:IMessageService
{
    public void Send(string message)
    {
        Console.WriteLine($"Sending SMS: {message}");
    }
}