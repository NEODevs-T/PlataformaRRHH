using System;
using System.Collections.Generic;

namespace Inspecciones.Model;

public partial class Imaquina
{
    public int IdMaquina { get; set; }

    public string Mnombre { get; set; } = null!;

    public string? Mdescri { get; set; }

    public bool Mestado { get; set; }

    public DateTime Mfecha { get; set; }

    public virtual ICollection<ImaqPre> ImaqPres { get; set; } = new List<ImaqPre>();
}
