using System;
using System.Collections.Generic;

namespace APISelfmedix.Models;

/// <summary>
/// 	
/// </summary>
public partial class Historialmedico
{
    public int Id { get; set; }

    public int IdPaciente { get; set; }

    public string? Enfermedad { get; set; }

    public string? Tratamiento { get; set; }

    public string Fechaingreso { get; set; } = null!;

    public virtual Paciente IdPacienteNavigation { get; set; } = null!;
}
