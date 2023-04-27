﻿using System;
using System.Collections.Generic;
using static Mysqlx.Crud.Order.Types;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    public string? Correo { get; set; } 

    public string? Contrasenia { get; set; }

    public virtual ICollection<Medico> Medicos { get; } = new List<Medico>();

    public virtual ICollection<Paciente> Pacientes { get; } = new List<Paciente>();


    public override string ToString()
    {
        return string.Format("{0} {1}", Nombres, Apellidos);
    }
}
