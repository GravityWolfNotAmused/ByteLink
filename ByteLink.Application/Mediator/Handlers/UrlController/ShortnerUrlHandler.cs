using AutoMapper;
using ByteLink.Application.Generators;
using ByteLink.Application.Mediator.Commands;
using ByteLink.Application.Models.ResultModels;
using ByteLink.Domain.Entities;
using ByteLink.Domain.Enums;
using ByteLink.Domain.Exceptions;
using ByteLink.Domain.Generators;
using ByteLink.Infrastructure.Persistence.Repositories;
using Enyim.Caching;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace ByteLink.Application.Mediator.Handlers.UrlController;

public class CreateShortnerUrlHandler(
    IUrlRepository urlRepository,
    IUserRepository userRepository,
    IMemoryCache cache,
    IMapper mapper,
    [FromKeyedServices(GeneratorKeyedServices.ShortCodeGenerator)] IGenerator<string, string> shortCodeGenerator,
    [FromKeyedServices(GeneratorKeyedServices.UserSqidGenerator)] IGenerator<long, string> userSqidIdGenerator
) : IRequestHandler<ShortenUrlCommand, CreateShortUrlCommandResult>
{
    public async Task<CreateShortUrlCommandResult> Handle(ShortenUrlCommand request, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.OriginalUrl, nameof(request.OriginalUrl));

        if (!Uri.TryCreate(request.OriginalUrl, UriKind.Absolute, out _))
        {
            throw new ArgumentException("Invalid URL format.");
        }

        if (!Regex.IsMatch(request.OriginalUrl, @"\.[a-zA-Z]{2,}$"))
        {
            throw new FormatException("The domain provided does not have a valid top-level domain.");
        }

        ApplicationUser? applicationUser = null;

        try
        {
            applicationUser = await userRepository.GetAuthorizedUserAsync();
        }
        catch (UnauthorizedAccessException)
        {
            applicationUser = await userRepository.GetAnonymouseUserAsync();
        }

        var shortCode = shortCodeGenerator.Generate(request.OriginalUrl);
        var userId = userSqidIdGenerator.Generate(applicationUser.Id);

        var url = Url.Create(request.OriginalUrl, shortCode, userId);

        if (await urlRepository.ExistsAsync(url.ShortCode))
            throw new ArgumentException($"Duplicate short code: {shortCode}");

        var insertedRow = await urlRepository.AddAsync(url);

        if (!insertedRow)
            throw new Exception($"Failure to add row to database for source URL: {url.SourceUrl}");

        cache.Set(shortCode, applicationUser, TimeSpan.FromHours(1));

        return mapper.Map<CreateShortUrlCommandResult>(url);
    }
}
