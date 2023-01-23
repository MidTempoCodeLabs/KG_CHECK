namespace Application.InputModels.Mail;

public class MailRequest
{
    public string To { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;
    public string? From { get; set; }
}