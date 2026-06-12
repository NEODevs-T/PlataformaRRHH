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

        public override async Task<AuthenticationState>
            GetAuthenticationStateAsync()
        {
            var token =
                await _localStorage.GetItemAsStringAsync("NeoRHToken");

            var identity = new ClaimsIdentity();

            _http.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrWhiteSpace(token))
            {
                identity = new ClaimsIdentity(
                    ParseClaimsFromJwt(token),
                    "jwt");

                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        token.Replace("\"", ""));
            }

            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        // ✅ LOGIN
        public async Task Login(string token)
        {
            await _localStorage.SetItemAsync("NeoRHToken", token);

            NotifyAuthenticationStateChanged(
                GetAuthenticationStateAsync());
        }

        // ✅ LOGOUT
        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("NeoRHToken");

            NotifyAuthenticationStateChanged(
                GetAuthenticationStateAsync());
        }

        // ✅ OBTENER NOMBRE DEL USUARIO
        public static string GetUserName(ClaimsPrincipal user)
        {
            var nombre = user.Claims.FirstOrDefault(c =>
                c.Type == "name" || c.Type.Contains("name"));

            if (nombre != null && !string.IsNullOrWhiteSpace(nombre.Value))
                return nombre.Value;

            return user.Identity?.Name ?? "Usuario";
        }

        // ✅ OBTENER ROL
        public static string GetUserRole(ClaimsPrincipal user)
        {
            var rol = user.Claims.FirstOrDefault(c =>
                c.Type.Contains("role"));

            return rol?.Value ?? "Sin rol";
        }

        // ✅ PARSE JWT
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];

            var jsonBytes = ParseBase64WithoutPadding(payload);

            var keyValuePairs =
                JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes)
                ?? new Dictionary<string, object>();

            return keyValuePairs.Select(kvp =>
                new Claim(kvp.Key, kvp.Value?.ToString() ?? ""));
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