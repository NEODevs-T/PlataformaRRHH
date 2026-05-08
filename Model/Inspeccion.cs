using System;
using System.Collections.Generic;

namespace Inspecciones.Model;

public partial class Inspeccion
{
    public int IdInspec { get; set; }

    public DateTime Ifecha { get; set; }

    public string Iturno { get; set; } = null!;

    public string Igrupo { get; set; } = null!;

    public string Iequipo { get; set; } = null!;

    public string Iarea { get; set; } = null!;

    public string Ificha { get; set; } = null!;

    public virtual ICollection<InspecDatum> InspecData { get; set; } = new List<InspecDatum>();
}
