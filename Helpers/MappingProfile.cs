using AutoMapper;
using Hei_Hei_Api.Models;
using Hei_Hei_Api.Requests.Users;
using Hei_Hei_Api.Responses.Users;

namespace Hei_Hei_Api.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateUserRequest, User>();
        CreateMap<User, CreateUserResponse>();
        CreateMap<LoginUserRequest, User>();
        CreateMap<User, LoginUserResponse>();
    }
}
