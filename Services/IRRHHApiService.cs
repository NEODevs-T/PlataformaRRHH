using NeoRH.DTOs;
using NeoAPI.DTOs.RRHH;
using NeoAPI.RRHHModels;

public interface IRRHHApiService
{  
    // Maestro Trabajadores
    Task<List<MaestroTrabajadorDTO>> GetMaestroTrabajadores();

    // PERIODOS
    Task<List<PeriodosVDTO>> GetPeriodos();

    // PERMISOS
    Task<List<PermisosNomDiariaHistVDTO>> GetPermisosNomDiaria();

    Task<List<VRotacionDTO>> GetNominaMensual();

    // REPOSOS
    Task<List<RepososVDTO>> GetReposos();

    //LOGIN
    Task<string?> Login(UserLoginDto dto);

    // GENERICO (opcional)
    Task<List<T>> GetAsync<T>(string endpoint);
    Task<bool> PostAsync<T>(string endpoint, T data);
}
