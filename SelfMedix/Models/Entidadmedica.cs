using System;
using System.Collections.Generic;

namespace SelfMedix.Models;

public partial class Entidadmedica
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Direccion { get; set; }

    public virtual ICollection<Medico> IdMedicos { get; } = new List<Medico>();
}
