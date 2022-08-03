using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Task5.Domain.DTOs;
using Task5.Domain.Exceptions;
using Task5.Domain.Interfaces;
using Task5.Models;

namespace Task5.Controllers;

public class MailController : Controller
{
    private readonly IUserService _userService;
    private readonly IMailService _mailService;

    public MailController(IUserService userService, IMailService mailService)
    {
        _userService = userService;
        _mailService = mailService;
    }
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Index(string username)
    {
        var user = await _userService.SetUser(username);
        ViewBag.User = user;
        ViewBag.Mails = await _mailService.GetUserMails(user.Id);
        return View("Mails");
    }
    [HttpPost]
    public async Task<IActionResult> Mail(int userId, int mailId)
    {
        var body = await _mailService.GetMailBody(userId, mailId);
        return Ok(body);
    }
    [HttpPost]
    public async Task<IActionResult> Send(SendMailDto mail)
    {
        try
        {
            await _mailService.SendMail(mail);
        }
        catch (UserNotFoundException e)
        {
            return BadRequest(e.Message);
        }
        return Ok();
    }
    [HttpGet]
    public async Task<JsonResult> UserSearch(string query)
    {
        Console.WriteLine(query);
        var users = await _userService.SearchUsers(query);
        return Json(users);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}