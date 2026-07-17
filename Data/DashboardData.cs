using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using NeoRH.DTOs;
using NeoAPI.DTOs.RRHH;
using NeoAPI.RRHHModels;
using System.Linq;

namespace NeoRH.Data;

public class DashboardData : IDashboardData
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly string _baseUrl;

    private HttpClient cliente = new();

    public DashboardData(
        IHttpClientFactory clientFactory,
        IConfiguration configuration)
    {
        _clientFactory = clientFactory;

        _baseUrl =
            configuration["ApiSettings:BaseUrl"]
            ?? throw new Exception(
                "ApiSettings:BaseUrl no configurado");
    }

        public async Task<List<PermisosNomDiariaHistVDTO>> GetPermisos()
    {
        try
        {
            cliente = _clientFactory.CreateClient();

            var url =
                $"{_baseUrl}PermisosNomDiariaHistV?page=1&pageSize=2000";

            Console.WriteLine(
                $"URL PERMISOS => {url}");

            var response =
                await cliente.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var error =
                    await response.Content.ReadAsStringAsync();

                Console.WriteLine(
                    $"PERMISOS ERROR HTTP = {response.StatusCode}");

                Console.WriteLine(
                    $"PERMISOS VACIOS");

                Console.WriteLine(
                    $"ERROR => {error}");

                return new List<PermisosNomDiariaHistVDTO>();
            }

            var data =
                await response.Content.ReadFromJsonAsync<
                    PagedResponse<PermisosNomDiariaHistVDTO>>();

            if (data == null)
            {
                return new List<PermisosNomDiariaHistVDTO>();
            }

            var permisos =
                data.Data.ToList();

            Console.WriteLine(
                $"PERMISOS CARGADOS = {permisos.Count}");

            return permisos;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"ERROR PERMISOS => {ex}");

            return new List<PermisosNomDiariaHistVDTO>();
        }
    }

    public async Task<List<VRotacionDTO>> GetNominaMensual()
    {
        try
        {
            cliente = _clientFactory.CreateClient();

            var anio = DateTime.Now.Year;

            var url =
                $"{_baseUrl}VRotacion?anio={anio}";

            Console.WriteLine(
                $"URL => {url}");

            var response =
                await cliente.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var error =
                    await response.Content
                        .ReadAsStringAsync();

                Console.WriteLine(
                    $"ERROR => {error}");

                return new();
            }

            var data =
                await response.Content
                    .ReadFromJsonAsync<
                        List<VRotacionDTO>>();

            Console.WriteLine(
                $"MENSUAL CARGADOS = {data?.Count ?? 0}");

            return data ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"ERROR NOMINA MENSUAL => {ex}");

            return new();
        }
    }

    public async Task<List<RepososVDTO>> GetReposos()
    {
        try
        {
            cliente = _clientFactory.CreateClient();

            var url =
                $"{_baseUrl}RepososV/GetRepososFiltrados";

            Console.WriteLine(
                $"URL => {url}");

            var response =
                await cliente.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var error =
                    await response.Content
                        .ReadAsStringAsync();

                Console.WriteLine(
                    $"ERROR => {error}");

                return new();
            }

            var data =
                await response.Content
                    .ReadFromJsonAsync<
                        List<RepososVDTO>>();

            Console.WriteLine(
                $"REPOSOS CARGADOS = {data?.Count ?? 0}");

            return data ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"ERROR REPOSOS => {ex}");

            return new();
        }
    }

            public async Task<IndicadoresResumenDTO> GetResumenDiario(int anio)
    {
        try
        {
            cliente = _clientFactory.CreateClient();

            var url =
                $"{_baseUrl}PermisosNomDiariaHistV/resumen?anio={anio}";

            Console.WriteLine(
                $"URL RESUMEN DIARIO => {url}");

            var response =
                await cliente.GetAsync(url);

            var contenido =
                await response.Content.ReadAsStringAsync();

            Console.WriteLine(
                $"RESPUESTA DIARIO = {contenido}");

            response.EnsureSuccessStatusCode();

            var data =
                System.Text.Json.JsonSerializer.Deserialize
                    <IndicadoresResumenDTO>(
                        contenido,
                        new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

            return data ?? new IndicadoresResumenDTO();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"ERROR RESUMEN DIARIO => {ex}");

            return new IndicadoresResumenDTO();
        }
    }

        public async Task<IndicadoresResumenDTO> GetResumenMensual(int anio)
    {
        try
        {
            cliente = _clientFactory.CreateClient();

            var url =
                $"{_baseUrl}VRotacionHist/resumen?anio={anio}";

            var response =
                await cliente.GetAsync(url);

            var contenido =
                await response.Content.ReadAsStringAsync();

            Console.WriteLine(
                $"RESPUESTA MENSUAL = {contenido}");

            response.EnsureSuccessStatusCode();

            var data =
                System.Text.Json.JsonSerializer.Deserialize
                    <IndicadoresResumenDTO>(
                        contenido,
                        new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

            return data ?? new IndicadoresResumenDTO();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"ERROR RESUMEN MENSUAL => {ex}");

            return new IndicadoresResumenDTO();
        }
    }
}
