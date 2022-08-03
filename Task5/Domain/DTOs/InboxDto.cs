using Task5.Models;

namespace Task5.Domain.DTOs;

public class InboxDto:MailModel
{
    public int Id { get; set; }
    public string From { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    
}