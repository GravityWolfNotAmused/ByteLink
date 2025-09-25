using ByteLink.Domain.Entities;
using ByteLink.Domain.Generators;
using ByteLink.Domain.Settings;
using System.Text;

namespace ByteLink.Application.Generators;

public class ShortCodeUrlGenerator(ByteLinkAppSettings appSettings) : IGenerator<Url, string>
{
    public string Generate(Url url)
    {
        var sb = new StringBuilder();

        sb.Append(appSettings.IsHttps ? "https://" : "http://");
        sb.Append(appSettings.Domain);

        if (sb.Length == 0 || sb[^1] != '/')
        {
            sb.Append('/');
        }

        sb.Append(url.UserId);
        sb.Append('/');
        sb.Append(url.ShortCode);

        return sb.ToString();
    }
}
