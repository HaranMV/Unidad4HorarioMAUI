using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorarioMAUI.Models
{
    public enum Colores {Azul, Verde, Amarillo, Rosa, Aqua, Rojo, Naranja }
    public class NotaModel
    {

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string NombreMaestro { get; set; } = null!;
        public string Aula { get; set; } = null!;   
        public string Descripcion { get; set; } = null!;   
        public int HoraInicio { get; set; } 
        public int HoraFin { get; set; } 
        public Colores Color { get; set; }
        public Dias Dia { get; set; }   
    }
}
