using AutoMapper;
using Hei_Hei_Api.Models;
using Hei_Hei_Api.Requests.Users;
using Hei_Hei_Api.Responses.Animators;
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
        CreateMap<User, GetUserResponse>();
        CreateMap<UpdateUserRequest, User>()
            .ForAllMembers(opt =>
            opt.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<Animator, GetAnimatorResponse>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
            .ForMember(dest => dest.HomeAddress, opt => opt.MapFrom(src => src.User.HomeAddress));
        CreateMap<Animator, AddAnimatorInfoResponse>();
        CreateMap<Animator, UpdateAnimatorResponse>();
    }
}
