using NeoRH.DTOs;
using NeoAPI.DTOs.RRHH;
using NeoAPI.RRHHModels;

public interface IDashboardData
{
    Task<List<PermisosNomDiariaHistVDTO>> GetPermisos();

    Task<List<VRotacionDTO>> GetNominaMensual();

    Task<List<RepososVDTO>> GetReposos();

    Task<IndicadoresResumenDTO> GetResumenDiario(int anio);

    Task<IndicadoresResumenDTO> GetResumenMensual(int anio);
}