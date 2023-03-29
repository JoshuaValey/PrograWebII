using System;
using System.Collections.Generic;

namespace SelfMedix.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaElimina { get; set; }

    public bool Vigente { get; set; }

    public string? UrlImg { get; set; }

    public string Correo { get; set; } = null!;

    public string Contrasenia { get; set; } = null!;

    public virtual ICollection<Medico> Medicos { get; } = new List<Medico>();

    public virtual ICollection<Paciente> Pacientes { get; } = new List<Paciente>();
}
