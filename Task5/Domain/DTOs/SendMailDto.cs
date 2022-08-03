using Task5.Models;

namespace Task5.Domain.DTOs;

public class SendMailDto:MailModel
{
    public int From { get; set; }
    public string To { get; set; } = string.Empty;

}