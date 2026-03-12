using AutoMapper;
using Hei_Hei_Api.Models;
using Hei_Hei_Api.Requests.Heroes;
using Hei_Hei_Api.Requests.Users;
using Hei_Hei_Api.Responses.Animators;
using Hei_Hei_Api.Responses.Heroes;
using Hei_Hei_Api.Responses.OrderAnimators;
using Hei_Hei_Api.Responses.Orders;
using Hei_Hei_Api.Responses.Packages;
using Hei_Hei_Api.Responses.Reviews;
using Hei_Hei_Api.Responses.Users;

namespace Hei_Hei_Api.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateUserRequest, User>();

        CreateMap<User, CreateUserResponse>();

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

        CreateMap<Hero, CreateHeroResponse>();

        CreateMap<Hero, GetHeroResponse>();

        CreateMap<Package, CreatePackageResponse>()
            .ForMember(dest => dest.HeroIds, opt => opt.MapFrom(src => src.Heroes.Select(h => h.Id).ToList()));

        CreateMap<Package, GetPackageResponse>();

        CreateMap<Package, UpdatePackageResponse>();

        CreateMap<Order, CreateOrderResponse>()
            .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.Package.Name));

        CreateMap<Order, GetOrderResponse>()
            .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.Package.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<OrderAnimator, OrderAnimatorResponse>()
            .ForMember(dest => dest.AnimatorName, opt => opt.MapFrom(src => src.Animator.User.FullName))
            .ForMember(dest => dest.HeroName, opt => opt.MapFrom(src => src.Hero.Name));

        CreateMap<Payment, PaymentResponse>()
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<OrderAnimator, GetOrderAnimatorResponse>()
            .ForMember(dest => dest.AnimatorName, opt => opt.MapFrom(src => src.Animator.User.FullName))
            .ForMember(dest => dest.HeroName, opt => opt.MapFrom(src => src.Hero.Name));

        CreateMap<OrderAnimator, UpdateOrderAnimatorResponse>();

        CreateMap<Review, GetReviewResponse>()
            .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName));

        CreateMap<Review, CreateReviewResponse>();

        CreateMap<Review, UpdateReviewResponse>();
    }
}
