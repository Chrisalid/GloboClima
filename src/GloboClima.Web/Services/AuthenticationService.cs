using Blazored.LocalStorage;
using GloboClima.Core.DTOs;
using Microsoft.JSInterop;

namespace GloboClima.Web.Services;

public class AuthenticationService
{
    private readonly ApiService _apiService;
    private readonly ILocalStorageService _localStorage;
    private readonly IJSRuntime _jsRuntime;
    private const string TokenKey = "authToken";
    private const string UserKey = "currentUser";

    public AuthenticationService(ApiService apiService, ILocalStorageService localStorage, IJSRuntime jsRuntime)
    {
        _apiService = apiService;
        _localStorage = localStorage;
        _jsRuntime = jsRuntime;
    }

    private bool IsPrerendering => _jsRuntime is IJSInProcessRuntime == false;

    public async Task<bool> LoginAsync(LoginRequest request)
    {
        // Durante prerendering, não pode executar operações de localStorage
        if (IsPrerendering)
            return false;
            
        var result = await _apiService.LoginAsync(request);
        
        if (result != null)
        {
            await _localStorage.SetItemAsync(TokenKey, result.Token);
            await _localStorage.SetItemAsync(UserKey, result);
            _apiService.SetAuthorizationHeader(result.Token);
            return true;
        }

        return false;
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        // Durante prerendering, não pode executar operações de localStorage
        if (IsPrerendering)
            return false;
            
        var result = await _apiService.RegisterAsync(request);
        
        if (result != null)
        {
            await _localStorage.SetItemAsync(TokenKey, result.Token);
            await _localStorage.SetItemAsync(UserKey, result);
            _apiService.SetAuthorizationHeader(result.Token);
            return true;
        }

        return false;
    }

    public async Task LogoutAsync()
    {
        // Durante prerendering, não faz nada
        if (IsPrerendering)
            return;
            
        await _localStorage.RemoveItemAsync(TokenKey);
        await _localStorage.RemoveItemAsync(UserKey);
        _apiService.ClearAuthorizationHeader();
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        // Retorna false durante prerendering para evitar JavaScript Interop
        if (IsPrerendering)
            return false;
            
        try
        {
            var token = await _localStorage.GetItemAsync<string>(TokenKey);
            
            if (string.IsNullOrEmpty(token))
                return false;

            var user = await _localStorage.GetItemAsync<AuthResponse>(UserKey);
            
            if (user == null || user.ExpiresAt <= DateTime.UtcNow)
            {
                await LogoutAsync();
                return false;
            }

            _apiService.SetAuthorizationHeader(token);
            return true;
        }
        catch
        {
            await LogoutAsync();
            return false;
        }
    }

    public async Task<AuthResponse?> GetCurrentUserAsync()
    {
        // Retorna null durante prerendering para evitar JavaScript Interop
        if (IsPrerendering)
            return null;
            
        try
        {
            return await _localStorage.GetItemAsync<AuthResponse>(UserKey);
        }
        catch
        {
            return null;
        }
    }
}
