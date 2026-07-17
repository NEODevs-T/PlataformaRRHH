using System.Security.Claims;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

namespace NeoRH.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;

        public CustomAuthStateProvider(
            ILocalStorageService localStorage,
            HttpClient http)
        {
            _localStorage = localStorage;
            _http = http;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // TOKEN DE LA PLATAFORMA
            var token = await _localStorage.GetItemAsStringAsync("PlataformaRHToken") ?? "";

            var identity = new ClaimsIdentity();
            _http.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(token))
            {
                token = token
                    .Replace("\"", "")
                    .Replace("\n", "")
                    .Replace("\r", "")
                    .Trim();

                identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }

        public async Task Login(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return;

            token = token
                .Replace("\"", "")
                .Replace("\n", "")
                .Replace("\r", "")
                .Trim();

            await _localStorage.SetItemAsync("PlataformaRHToken", token);

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("PlataformaRHToken");

            _http.DefaultRequestHeaders.Authorization = null;

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];

            var jsonBytes = ParseBase64WithoutPadding(payload);

            var keyValuePairs =
                JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            return (keyValuePairs ?? new Dictionary<string, object>())
                .Select(kvp => new Claim(kvp.Key, kvp.Value?.ToString() ?? ""));
        }

        public static string GetUserName(ClaimsPrincipal user)
        {
            return user?.Identity?.Name ?? "Usuario";
        }

        public static string GetUserRole(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type.Contains("role"))?.Value ?? "User";
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);
        }
    }
}