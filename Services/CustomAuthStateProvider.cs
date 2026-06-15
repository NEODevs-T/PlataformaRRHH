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

        // ✅ ESTADO GLOBAL
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _localStorage.GetItemAsStringAsync("NeoRHToken");

                // ✅ LIMPIEZA SEGURA DEL TOKEN
                token = token?.Replace("\"", "");

                if (string.IsNullOrWhiteSpace(token))
                {
                    // ❌ NO AUTENTICADO
                    _http.DefaultRequestHeaders.Authorization = null;

                    return new AuthenticationState(
                        new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var claims = ParseClaimsFromJwt(token).ToList();

                // ✅ ASEGURAR ROLE
                if (!claims.Any(c => c.Type == ClaimTypes.Role))
                {
                    claims.Add(new Claim(ClaimTypes.Role, "User"));
                }

                var identity = new ClaimsIdentity(claims, "jwt");
                var user = new ClaimsPrincipal(identity);

                // ✅ HEADERS HTTP
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                return new AuthenticationState(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Auth error: {ex.Message}");

                _http.DefaultRequestHeaders.Authorization = null;

                return new AuthenticationState(
                    new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        // ✅ LOGIN
        public async Task Login(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return;

            token = token.Replace("\"", "");

            await _localStorage.SetItemAsync("NeoRHToken", token);

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        // ✅ LOGOUT
        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("NeoRHToken");

            _http.DefaultRequestHeaders.Authorization = null;

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        // ✅ NOMBRE USUARIO
        public static string GetUserName(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value
                ?? "Usuario";
        }

        // ✅ ROL
        public static string GetUserRole(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Role)?.Value
                ?? "User";
        }

        // ✅ PARSE JWT ROBUSTO
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            try
            {
                var parts = jwt.Split('.');

                if (parts.Length != 3)
                    return Enumerable.Empty<Claim>();

                var payload = parts[1];

                var jsonBytes = ParseBase64WithoutPadding(payload);

                var keyValuePairs =
                    JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes)
                    ?? new Dictionary<string, object>();

                return keyValuePairs.Select(kvp =>
                    new Claim(kvp.Key, kvp.Value?.ToString() ?? ""));
            }
            catch
            {
                return Enumerable.Empty<Claim>();
            }
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
