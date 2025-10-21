using ByteLink.Web.Services.Interfaces;
using ByteLink.Web.Services.Models;
using System.Net.Http.Json;

namespace ByteLink.Web.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private const string TokenKey = "auth_token";

    public AuthService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage)
    {
        _httpClient = httpClientFactory.CreateClient("ByteLinkAPI");
        _localStorage = localStorage;
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/Auth/login", request);
            
            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (loginResponse != null)
                {
                    await _localStorage.SetItemAsync(TokenKey, loginResponse.Token);
                    return new AuthResult
                    {
                        IsSuccess = true,
                        Token = loginResponse.Token
                    };
                }
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            return new AuthResult
            {
                IsSuccess = false,
                ErrorMessage = errorContent
            };
        }
        catch (Exception ex)
        {
            return new AuthResult
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<AuthResult> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/Auth/register", request);
            
            if (response.IsSuccessStatusCode)
            {
                return new AuthResult
                {
                    IsSuccess = true
                };
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            return new AuthResult
            {
                IsSuccess = false,
                ErrorMessage = errorContent
            };
        }
        catch (Exception ex)
        {
            return new AuthResult
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(TokenKey);
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetTokenAsync();
        return !string.IsNullOrEmpty(token);
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _localStorage.GetItemAsync<string>(TokenKey);
    }
}
