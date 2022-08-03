using AutoMapper;
using Task5.Domain.DTOs;
using Task5.Models.Entities;

namespace Task5.Domain.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
    }
}