using System;
using System.Collections.Generic;

namespace NeoAPI.DTOs.RRHH;

public partial class VAusenciaDTO
{
    public decimal Candnh { get; set; }

    public decimal Ctoddh { get; set; }

    public string Ciahnh { get; set; } = null!;

    public string Tpnhnh { get; set; } = null!;

    public string Fichnh { get; set; } = null!;

    public string Gpohnh { get; set; } = null!;

    public decimal Cgphnh { get; set; }

    public decimal Ctodnh { get; set; }

    public decimal Prddnh { get; set; }

    public string Dptdnh { get; set; } = null!;
}