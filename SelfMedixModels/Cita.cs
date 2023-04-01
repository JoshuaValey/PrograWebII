using System;
using System.Collections.Generic;

namespace SelfMedixModels;

public partial class Cita
{
    public int Id { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaCita { get; set; }

    public int IdMedico { get; set; }

    public int IdPaciente { get; set; }

    public int Estado { get; set; }

    public string? Descripcion { get; set; }

    public virtual Medico? IdMedicoNavigation { get; set; } 

    public virtual Paciente? IdPacienteNavigation { get; set; }
}
