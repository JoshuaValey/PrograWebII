using System;
using System.Collections.Generic;

namespace SelfMedixModels;

/// <summary>
/// 	
/// </summary>
public partial class Historialmedico
{
    public int Id { get; set; }

    public int IdPaciente { get; set; }

    public string Enfermedad { get; set; } = null!;

    public string? Tratamiento { get; set; }

    public string Fechaingreso { get; set; } = null!;

    public virtual Paciente IdPacienteNavigation { get; set; } = null!;
}
