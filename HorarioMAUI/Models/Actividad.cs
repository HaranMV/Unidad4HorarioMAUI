using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorarioMAUI.Models
{
    [Table("Actividades")]
    public class Actividad
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull, MaxLength(100)]
        public string NombreActividad { get; set; } = null!;

        [NotNull, MaxLength(250)]
        public string Descripcion { get; set; } = null!;

        [NotNull]
        public int HoraInicio { get; set; }

        [NotNull]
        public int HoraFin { get; set; }

        [NotNull]   
        public Dias Dia {  get; set; }  



    }
}
