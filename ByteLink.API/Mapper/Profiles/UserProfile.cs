using AutoMapper;
using ByteLink.API.Mapper.Resolvers;
using ByteLink.Application.Mediator.Commands;
using ByteLink.Domain.Entities;

namespace ByteLink.API.Mapper.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserCommand, ApplicationUser>()
            .ForMember(destination => destination.PasswordHash, opt => opt.MapFrom<PasswordHashResolver>())
            .ForMember(destinationMember => destinationMember.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}
