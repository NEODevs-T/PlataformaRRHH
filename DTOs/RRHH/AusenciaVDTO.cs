using System;
using System.Collections.Generic;

namespace NeoRH.DTOs;

public partial class AusenciaVDTO
{
    public string Ciadnh { get; set; } = null!;

    public string Tpndnh { get; set; } = null!;

    public decimal? Añodnh { get; set; }

    public decimal? Prddnh { get; set; }

    public string? Ficdnh { get; set; } = null!;

    public decimal Ctodnh { get; set; }

    public decimal Candnh { get; set; }

    public string?  Dptdnh { get; set; } = null!;

    public decimal? Mesdnh { get; set; }

    public decimal? Fecmdh { get; set; }
}
