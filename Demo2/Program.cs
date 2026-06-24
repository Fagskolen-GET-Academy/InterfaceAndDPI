using Demo2;

var emailNotificationManager = new NotificationManager(new EmailService());
var smsNotificationManager = new NotificationManager(new SmsService());

emailNotificationManager.Notify();
smsNotificationManager.Notify();

