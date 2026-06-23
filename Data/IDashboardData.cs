using NeoRH.DTOs;

public interface IDashboardData
{
    Task<List<PermisosNomDiariaVDTO>> GetPermisos();

    Task<List<RepososVDTO>> GetReposos();

    Task<List<AusenciaVDTO>> GetAusencias();
}