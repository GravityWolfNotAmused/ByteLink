using AutoMapper;
using ByteLink.Domain.Entities;
using ByteLink.Domain.Enums;
using ByteLink.Domain.Generators;

namespace ByteLink.API.Mapper.Resolvers;

public class ShortUrlResolver<TDestination>(
    [FromKeyedServices(GeneratorKeyedServices.ShortCodeUrlGenerator)] IGenerator<Url, string> shortCodeUrlGenerator
) : IValueResolver<Url, TDestination, string>
{
    public string Resolve(Url source, TDestination destination, string destMember, ResolutionContext context)
    {
        return shortCodeUrlGenerator.Generate(source);
    }
}
