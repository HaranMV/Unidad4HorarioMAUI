using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorarioMAUI.Models
{
    public enum Dias { Lunes, Martes, Miercoles, Jueves, Viernes, Sabado, Domingo }
    [Table("clases")]
    public class Clase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull, MaxLength(100)]
        public string NombreAsignatura { get; set; } = null!;

        [NotNull, MaxLength(100)]
        public string NombreMaestro { get; set; } = null!;

        [NotNull, MaxLength(100)]
        public string Aula { get; set; } = null!;

        [NotNull]
        public int HoraInicio { get; set; }

        [NotNull]
        public int HoraFin { get; set; }

        [NotNull]
        public Dias Dia { get; set; }  

    }
}
