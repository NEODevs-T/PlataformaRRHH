using NeoRH.DTOs;

public interface IRRHHApiService
{
    // AUSENCIAS
    Task<List<AusenciaVDTO>> GetAusencias();

    // Maestro Trabajadores
    Task<List<MaestroTrabajadorDTO>> GetMaestroTrabajadores();

    // PERIODOS
    Task<List<PeriodosVDTO>> GetPeriodos();

    // PERMISOS
    Task<List<PermisosNomDiariaVDTO>> GetPermisosNomDiaria();

    // REPOSOS
    Task<List<RepososVDTO>> GetReposos();

    //Login
    Task<string?> Login(UserLoginDto dto);

    // GENERICO (opcional)
    Task<List<T>> GetAsync<T>(string endpoint);
    Task<bool> PostAsync<T>(string endpoint, T data);
}
