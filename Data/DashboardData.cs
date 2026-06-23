using System.Net.Http.Json;
using NeoRH.DTOs;

namespace NeoRH.Data;

public class DashboardData : IDashboardData
{
    private readonly IHttpClientFactory _clientFactory;

    private HttpClient cliente = new();

#if DEBUG
    private const string BaseUrl =
        "http://localhost:5021/api";
#else
    private const string BaseUrl =
        "URL_PUBLICADA_DE_NEOAPI";
#endif

    public DashboardData(
        IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<List<PermisosNomDiariaVDTO>> GetPermisos()
    {
        try
        {
            cliente = _clientFactory.CreateClient();

            var url =
                $"{BaseUrl}/PermisosNomDiariaV";

            Console.WriteLine("=================================");
            Console.WriteLine($"GET => {url}");
            Console.WriteLine("=================================");

            var response =
                await cliente.GetAsync(url);

            Console.WriteLine(
                $"STATUS => {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                var error =
                    await response.Content.ReadAsStringAsync();

                Console.WriteLine(
                    $"ERROR => {error}");

                return new();
            }

            var data =
                await response.Content.ReadFromJsonAsync<
                    List<PermisosNomDiariaVDTO>>();

            Console.WriteLine(
                $"REGISTROS => {data?.Count ?? 0}");

            return data ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"ERROR PERMISOS => {ex}");

            return new();
        }
    }

    public async Task<List<RepososVDTO>> GetReposos()
    {
        try
        {
            cliente = _clientFactory.CreateClient();

            var url =
                $"{BaseUrl}/RepososV/GetRepososFiltrados";

            Console.WriteLine("=================================");
            Console.WriteLine($"GET => {url}");
            Console.WriteLine("=================================");

            var response =
                await cliente.GetAsync(url);

            Console.WriteLine(
                $"STATUS => {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                var error =
                    await response.Content.ReadAsStringAsync();

                Console.WriteLine(
                    $"ERROR => {error}");

                return new();
            }

            var data =
                await response.Content.ReadFromJsonAsync<
                    List<RepososVDTO>>();

            Console.WriteLine(
                $"REGISTROS => {data?.Count ?? 0}");

            return data ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"ERROR REPOSOS => {ex}");

            return new();
        }
    }

    public async Task<List<AusenciaVDTO>> GetAusencias()
    {
        try
        {
            cliente = _clientFactory.CreateClient();

            var url =
                $"{BaseUrl}/AusenciaV";

            Console.WriteLine("=================================");
            Console.WriteLine($"GET => {url}");
            Console.WriteLine("=================================");

            var response =
                await cliente.GetAsync(url);

            Console.WriteLine(
                $"STATUS => {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                var error =
                    await response.Content.ReadAsStringAsync();

                Console.WriteLine(
                    $"ERROR => {error}");

                return new();
            }

            var data =
                await response.Content.ReadFromJsonAsync<
                    List<AusenciaVDTO>>();

            Console.WriteLine(
                $"REGISTROS => {data?.Count ?? 0}");

            return data ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"ERROR AUSENCIAS => {ex}");

            return new();
        }
    }
}