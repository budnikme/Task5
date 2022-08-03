using Task5.Domain.DTOs;

namespace Task5.Domain.Interfaces;

public interface IUserService
{
    Task<UserDto> SetUser(string username);
    Task<List<string>> SearchUsers(string searchString);
}