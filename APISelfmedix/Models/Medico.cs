using System;
using System.Collections.Generic;

namespace APISelfmedix.Models;

public partial class Medico
{
    public int Id { get; set; }

    public int IdUsuarioMedico { get; set; }

    public string? TituloCorto { get; set; }

    public virtual ICollection<Cita> Cita { get; } = new List<Cita>();

    public virtual Usuario? IdUsuarioMedicoNavigation { get; set; }

    public virtual ICollection<Entidadmedica> IdEntidads { get; } = new List<Entidadmedica>();
}
