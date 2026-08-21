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

            case "Gerente_RRHH":
            case "SuperUser":
                UsrRol = "RRHH";
                break;

            case "Gerente_Salud":
                UsrRol = "Salud";
                break;

            case "Gerente_Finanzas":
                UsrRol = "Finanzas";
                break;

            default:
                UsrRol = role;
                break;
        }

        return Task.CompletedTask;
    }
}