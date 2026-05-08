using System;
using System.Collections.Generic;

namespace Inspecciones.Model;

public partial class ImaqPre
{
    public int IdMaqPre { get; set; }

    public int IdMaquina { get; set; }

    public int IdPregunta { get; set; }

    public DateTime Mpfecha { get; set; }

    public virtual Imaquina IdMaquinaNavigation { get; set; } = null!;

    public virtual Ipreguntum IdPreguntaNavigation { get; set; } = null!;

    public virtual ICollection<InspecDatum> InspecData { get; set; } = new List<InspecDatum>();
}
