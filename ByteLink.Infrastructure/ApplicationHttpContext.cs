using ByteLink.Domain;
using Microsoft.AspNetCore.Http;

namespace ByteLink.Infrastructure;

public interface IApplicationHttpContext
{
    public string GetAuthorizedEmail();
}

public class ApplicationHttpContext(
    IHttpContextAccessor httpContextAccessor
) : IApplicationHttpContext
{
    public string GetAuthorizedEmail()
    {
        var httpContext = httpContextAccessor.HttpContext;
        var loggedInUser = httpContext?.User
            ?? throw new UnauthorizedAccessException("Failed to get user from http context.");

        var email = loggedInUser?.FindFirst(Constants.EmailClaimKey)?.Value
            ?? throw new UnauthorizedAccessException("Failed to get user email from http context.");

        return email;
    }
}
