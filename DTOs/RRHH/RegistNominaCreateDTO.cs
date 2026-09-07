using System;
using System.Collections.Generic;

namespace NeoAPI.DTOs.RRHH;

public class RegistNominaCreateDTO
{
    public string Ciahnh { get; set; } = string.Empty;

    public string Tpnhnh { get; set; } = string.Empty;

    public int Añohnh { get; set; }

    public int Prdhnh { get; set; }

    public string Fichnh { get; set; } = string.Empty;

    public string Dpthnh { get; set; } = string.Empty;

    public string Dg01hh { get; set; } = string.Empty;
    public string Dg02hh { get; set; } = string.Empty;
    public string Dg03hh { get; set; } = string.Empty;
    public string Dg04hh { get; set; } = string.Empty;
    public string Dg05hh { get; set; } = string.Empty;
    public string Dg06hh { get; set; } = string.Empty;
    public string Dg07hh { get; set; } = string.Empty;

    public string? Dg08hh { get; set; }
    public string? Dg09hh { get; set; }
    public string? Dg010hh { get; set; }
    public string? Dg011hh { get; set; }
    public string? Dg012hh { get; set; }
    public string? Dg013hh { get; set; }
    public string? Dg014hh { get; set; }
    public string? Dg015hh { get; set; }

    public string Tpnom { get; set; } = string.Empty;

    public bool Stareg { get; set; } = true;
}