﻿using System;
using System.Collections.Generic;

namespace SelfMedixModels;

/// <summary>
/// 	
/// </summary>
public partial class Historialmedico
{
    public int Id { get; set; }

    public int IdPaciente { get; set; }

    public string? Enfermedad { get; set; } 

    public string? Tratamiento { get; set; }

    public DateTime? Fechaingreso { get; set; } 

    public virtual Paciente? IdPacienteNavigation { get; set; } 
}
