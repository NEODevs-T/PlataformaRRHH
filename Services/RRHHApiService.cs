using System.Net.Http.Json;
using NeoRH.DTOs;
using NeoAPI.DTOs.RRHH;
using NeoAPI.RRHHModels;

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
            var url = $"{_baseUrl}{endpoint}";

            var response = await _http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"ERROR BODY => {errorBody}");

                return new List<T>();
            }

            var data =
                await response.Content.ReadFromJsonAsync<List<T>>();

            return data ?? new List<T>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR GET {endpoint}");
            Console.WriteLine(ex.ToString());

            return new List<T>();
        }
    }

        public async Task<List<VRotacionDTO>> GetNominaMensual()
    {
        return await _http.GetFromJsonAsync<List<VRotacionDTO>>(
            "api/rrhh/nomina-mensual")
            ?? new();
    }

    public async Task<string?> Login(UserLoginDto dto)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(
                "api/Auth/Login",
                dto);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR LOGIN => {ex}");

            return null;
        }
    }

    public async Task<List<MaestroTrabajadorDTO>> GetMaestroTrabajadores()
    {
        Console.WriteLine("Consultando MaestroTrabajador...");

        return await GetAsync<MaestroTrabajadorDTO>(
            "MaestroTrabajador");
    }

    public async Task<List<PeriodosVDTO>> GetPeriodos()
    {
        Console.WriteLine("Consultando PeriodosV...");

        return await GetAsync<PeriodosVDTO>(
            "PeriodosV");
    }

    public async Task<List<PermisosNomDiariaHistVDTO>> GetPermisosNomDiaria()
    {
        Console.WriteLine("Consultando PermisosNomDiariaV...");

        return await GetAsync<PermisosNomDiariaHistVDTO>(
            "PermisosNomDiariaV");
    }

    public async Task<List<RepososVDTO>> GetReposos()
    {
        Console.WriteLine("Consultando RepososV...");

        return await GetAsync<RepososVDTO>(
            "RepososV/GetRepososFiltrados");
    }

    public async Task<bool> PostAsync<T>(string endpoint, T data)
    {
        try
        {
            var url = $"{_baseUrl}{endpoint}";

            Console.WriteLine($"POST => {url}");

            var response = await _http.PostAsJsonAsync(
                url,
                data);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR POST {endpoint}");
            Console.WriteLine(ex.ToString());

            return false;
        }
    }
}