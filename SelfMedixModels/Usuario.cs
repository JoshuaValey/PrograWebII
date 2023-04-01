using System;
using System.Collections.Generic;

namespace SelfMedixModels;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Nombres { get; set; } 

    public string ?Apellidos { get; set; }

    public DateTime FechaNacimiento { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaElimina { get; set; }

    public bool Vigente { get; set; }

    public string? UrlImg { get; set; }

    public string? Correo { get; set; } 

    public string? Contrasenia { get; set; }

    public virtual ICollection<Medico> Medicos { get; } = new List<Medico>();

    public virtual ICollection<Paciente> Pacientes { get; } = new List<Paciente>();
}
