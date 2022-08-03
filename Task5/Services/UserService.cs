using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Task5.Domain.DTOs;
using Task5.Domain.Interfaces;
using Task5.Models.Entities;

namespace Task5.Services;

public class UserService : IUserService
{
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;

    public UserService(ApplicationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDto> SetUser(string username)
    {
        var user = await _mapper.ProjectTo<UserDto>(_context.Users.Where(u => u.Username == username))
            .FirstOrDefaultAsync() ?? await CreateUser(username);
        return user;
    }

    private async Task<UserDto> CreateUser(string username)
    {
        var user = new User {Username = username};
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return _mapper.Map<UserDto>(user);
    }

    public async Task<List<string>> SearchUsers(string searchString)
    {
        var users = await _context.Users.Where(u => u.Username.StartsWith(searchString)).Select(u => u.Username).ToListAsync();
        return users;
    }
}