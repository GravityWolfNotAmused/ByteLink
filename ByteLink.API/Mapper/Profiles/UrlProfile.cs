using AutoMapper;
using ByteLink.API.Mapper.Resolvers;
using ByteLink.Application.Models.ResultModels;
using ByteLink.Application.Models.ViewModels;
using ByteLink.Domain.Entities;

namespace ByteLink.API.Mapper.Profiles;

public class UrlProfile : Profile
{
    public UrlProfile()
    {
        CreateMap<Url, UrlViewModel>();

        CreateMap<Url, UrlTotalVisitViewModel>()
            .ForMember(destination => destination.ShortUrl, opt => opt.MapFrom<ShortUrlResolver<UrlTotalVisitViewModel>>());

        CreateMap<Url, CreateShortUrlCommandResult>()
            .ForMember(destination => destination.ShortUrl, opt => opt.MapFrom<ShortUrlResolver<CreateShortUrlCommandResult>>());

        CreateMap<UrlVisit, UrlVisitViewModel>()
            .ForMember(dest => dest.ClickedAt, opt => opt.MapFrom(src => src.ClickedAt))
            .ForMember(dest => dest.ShortCode, opt => opt.MapFrom((src, dest, destMember, context) => (string)context.Items["ShortCode"]))
            .ForMember(dest => dest.ShortUrl, opt => opt.MapFrom((src, dest, destMember, context) => (string)context.Items["ShortUrl"]));
    }
}
