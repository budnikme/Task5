using Task5.Domain.DTOs;


namespace Task5.Domain.Interfaces;

public interface IMailService
{
    Task<List<InboxDto>> GetUserMails(int userId);
    Task<string?> GetMailBody(int userId, int mailId);
    Task SendMail(SendMailDto mail);
}