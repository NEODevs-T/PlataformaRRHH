using System.Net.Http.Json;
using NeoRH.DTOs;

public class RRHHApiService : IRRHHApiService
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;

    public RRHHApiService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _baseUrl = config["ApiSettings:BaseUrl"] ?? "";
    }

    public async Task<List<T>> GetAsync<T>(string endpoint)
    {
        try
        {
            var response = await _http.GetFromJsonAsync<List<T>>($"{_baseUrl}{endpoint}");
            return response ?? new List<T>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error GET {endpoint}: {ex.Message}");
            return new List<T>();
        }
    }

    public async Task<string?> Login(UserLoginDto dto)
    {
        var response = await _http.PostAsJsonAsync(
            "api/Auth/Login",
            dto);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<List<MaestroTrabajadorDTO>> GetMaestroTrabajadores()
    {
        return await _http.GetFromJsonAsync<List<MaestroTrabajadorDTO>>("api/maestrotrabajador")
            ?? new List<MaestroTrabajadorDTO>();
    }

    public async Task<List<AusenciaVDTO>> GetAusencias()
    {
        return await GetAsync<AusenciaVDTO>("AusenciaV");
    }

    public async Task<List<PeriodosVDTO>> GetPeriodos()
    {
        return await GetAsync<PeriodosVDTO>("PeriodosV");
    }

    public async Task<List<PermisosNomDiariaVDTO>> GetPermisosNomDiaria()
    {
        return await GetAsync<PermisosNomDiariaVDTO>("PermisosNomDiaria");
    }

    public async Task<List<RepososVDTO>> GetReposos()
    {
        return await GetAsync<RepososVDTO>("Reposos");
    }

    public async Task<bool> PostAsync<T>(string endpoint, T data)
    {
        try
        {
            var response = await _http.PostAsJsonAsync($"{_baseUrl}{endpoint}", data);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error POST {endpoint}: {ex.Message}");
            return false;
        }
    }
}