CREATE SCHEMA `selfmedix` DEFAULT CHARACTER SET utf8mb4 

use selfmedix

drop table if exists usuario
CREATE TABLE `selfmedix`.`usuario` (
  `id` INT NOT NULL,
  `Nombres` VARCHAR(255) CHARACTER SET 'utf8mb4'  NOT NULL,
  `Apellidos` VARCHAR(255) CHARACTER SET 'utf8mb4' NOT NULL,
  `FechaNacimiento` DATETIME NOT NULL,
  `FechaCreacion` DATETIME NOT NULL,
  `FechaElimina` DATETIME NULL,
  `Vigente` BIT NULL,
  `UrlImg` TEXT CHARACTER SET 'utf8mb4'  NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4

CREATE TABLE `selfmedix`.`paciente` (
  `id` INT NOT NULL,
  `id_usuario_paciente` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `id_usuario_paciente_idx` (`id_usuario_paciente` ASC),
  CONSTRAINT `id_usuario_paciente`
    FOREIGN KEY (`id_usuario_paciente`)
    REFERENCES `selfmedix`.`usuario` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;



drop table if exists medico

CREATE TABLE `selfmedix`.`medico` (
  `id` INT NOT NULL,
  `id_usuario_medico` INT NOT NULL,
  `tituloCorto` VARCHAR(10) CHARACTER SET 'utf8mb4' NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `id_usuario_medico_idx` (`id_usuario_medico` ASC),
  CONSTRAINT `id_usuario_medico`
    FOREIGN KEY (`id_usuario_medico`)
    REFERENCES `selfmedix`.`usuario` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;

/*drop table if exists historialmedico
CREATE TABLE `selfmedix`.`historialmedico` (
`id` INT NOT NULL,
)*/


drop table if exists cita
CREATE TABLE `selfmedix`.`cita` (
  `id` INT NOT NULL,
  `FechaCreacion` DATETIME NOT NULL,
  `FechaCita` DATETIME NOT NULL,
  `idMedico` INT NOT NULL,
  `idPaciente` INT NOT NULL,
  `Estado` INT NOT NULL,
  `Descripcion` VARCHAR(255) NULL,
  PRIMARY KEY (`id`),
  INDEX `id_cita_paciente_idx` (`idPaciente` ASC, `idMedico` ASC),
  INDEX `id_cita_medico_idx` (`idMedico` ASC),
  CONSTRAINT `id_cita_medico`
    FOREIGN KEY (`idMedico`)
    REFERENCES `selfmedix`.`medico` (`id`)
    ON DELETE CASCADE
     ON UPDATE CASCADE,
  CONSTRAINT `id_cita_paciente`
    FOREIGN KEY (`idPaciente`)
    REFERENCES `selfmedix`.`paciente` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE);
    
    drop table if exists entidadmedica
    CREATE TABLE `selfmedix`.`entidadmedica` (
  `id` INT NOT NULL,
  `nombre` VARCHAR(50) NOT NULL,
  `direccion` TEXT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;

select * from entidadmedica_medico;
CREATE TABLE `selfmedix`.`entidadmedica_medico` (
  `id_entidad` INT NOT NULL,
  `id_medico` INT NOT NULL,
  PRIMARY KEY (`id_entidad`, `id_medico`),
  INDEX `id_medico_idx` (`id_medico` ASC),
  CONSTRAINT `id_entidad`
    FOREIGN KEY (`id_entidad`)
    REFERENCES `selfmedix`.`entidadmedica` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `id_medico`
    FOREIGN KEY (`id_medico`)
    REFERENCES `selfmedix`.`medico` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;

show tables


CREATE TABLE `selfmedix`.`historialmedico` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `idPaciente` INT NOT NULL,
  `enfermedad` VARCHAR(255) NOT NULL,
  `tratamiento` VARCHAR(255) NULL,
  `fechaingreso` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `id_historial_paciente_idx` (`idPaciente` ASC),
  CONSTRAINT `id_historial_paciente`
    FOREIGN KEY (`idPaciente`)
    REFERENCES `selfmedix`.`paciente` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COMMENT = '	';

