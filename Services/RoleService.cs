using System.Security.Claims;

public class RoleService
{
    public string? UsrRol { get; set; }
    public string? UsrAdmin { get; set; }

    public Task ProcesarRoles(
        List<Claim> claims,
        IRRHHApiService api)
    {
        var role =
            claims.FirstOrDefault(c => c.Type.Contains("role"))?.Value;

        if (string.IsNullOrWhiteSpace(role))
            return Task.CompletedTask;

        switch (role)
        {
            case "Admin":
            case "SuperAdmin":
                UsrAdmin = "Global";
                break;

            case "RRHH_Admin":
            case "RRHH_User":
                UsrRol = "RRHH";
                break;

            case "Salud_Admin":
            case "Salud_User":
                UsrRol = "Salud";
                break;

            case "Finanzas_Admin":
            case "Finanzas_User":
                UsrRol = "Finanzas";
                break;

            default:
                UsrRol = role;
                break;
        }

        return Task.CompletedTask;
    }
}