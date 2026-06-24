namespace Demo2;

public class NotificationManager
{
    IMessageService _messageService;
    
    public NotificationManager(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public void Notify()
    {
        _messageService.Send("Hello World!");
    }

}