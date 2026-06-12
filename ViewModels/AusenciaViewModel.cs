using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NeoRH.ViewModels;

public class AusenciaViewModel
{
public string? Ficha { get; set; }

public string? Nombre { get; set; }

public string? Departamento { get; set; }

public string? Cargo { get; set; }

public string? Tipo { get; set; }

public decimal? Año { get; set; }

public string? Fecha { get; set; }
}
