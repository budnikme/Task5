using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Task5.Domain.DTOs;
using Task5.Domain.Exceptions;
using Task5.Domain.Interfaces;
using Task5.Hubs;
using Task5.Models.Entities;

namespace Task5.Services;

public class MailService : IMailService
{
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly IHubContext<MailHub> _hubContext;

    public MailService(ApplicationContext context, IHubContext<MailHub> hubContext, IMapper mapper)
    {
        _context = context;
        _hubContext = hubContext;
        _mapper = mapper;
    }
    public async Task<List<InboxDto>> GetUserMails(int userId)
    {
        Console.WriteLine(userId);
        var mails = await (from m in _context.Mails
            where m.To == userId
            join u in _context.Users on m.From equals u.Id
            select new InboxDto
            {
                Id = m.Id,
                From = u.Username,
                Title = m.Title,
                Body = m.Body,
                Date = m.Date.ToString("dd.MM.yyyy HH:mm:ss")
            }).OrderByDescending(m => m.Id).ToListAsync();
        return mails;
    }
    public async Task<string?> GetMailBody(int userId, int mailId)
    {
        var mail = await (from m in _context.Mails
            where m.Id == mailId && m.To == userId
            select m).FirstOrDefaultAsync();
        return mail?.Body;
    }
    private async Task<int> GetRecipientId(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        return user?.Id ?? throw new UserNotFoundException();
    }
    private async Task<Mail> CreateMail(SendMailDto mail)
    {
        return new Mail
        {
            From = mail.From,
            To = await GetRecipientId(mail.To),
            Title = mail.Title,
            Body = mail.Body,
            Date = DateTime.Now
        };
    }
    public async Task SendMail(SendMailDto mail)
    {
        var sendmail = await CreateMail(mail);
        await _context.Mails.AddAsync(sendmail);
        await _context.SaveChangesAsync();
        var inboxMail = await GetMailById(sendmail.Id);
        await SendToUser(sendmail.To,inboxMail);
    }
    private async Task<InboxDto?> GetMailById(int mailId)
    {
        return await (from m in _context.Mails
            where m.Id == mailId
            join u in _context.Users on m.From equals u.Id
            select new InboxDto
            {
                Id = m.Id,
                From = u.Username,
                Title = m.Title,
                Body = m.Body,
                Date = m.Date.ToString("dd.MM.yyyy HH:mm:ss")
            }).FirstOrDefaultAsync();
    }
    private async Task SendToUser(int userId, InboxDto mail)
    {
        var connections = MailHub.ConnectedUsers.Where(x => x.Value == userId).Select(c=>c.Key).ToList();
        foreach (var connection in connections)
        {
            await _hubContext.Clients.Client(connection).SendAsync("NewMessage", mail);
        }
    }
}