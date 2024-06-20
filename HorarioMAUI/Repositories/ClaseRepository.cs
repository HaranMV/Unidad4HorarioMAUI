using HorarioMAUI.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorarioMAUI.Repositories
{
    public class ClaseRepository
    {
        SQLiteConnection conexion;

        public ClaseRepository()
        {
            string ruta = FileSystem.AppDataDirectory + "/Clases.sqlite";
            conexion = new(ruta);
            conexion.CreateTable<Clase>();  
        }

        public IEnumerable<Clase> GetByDay(Dias dia)
        {            
          return conexion.Table<Clase>().Where(x => x.Dia == dia);            
        }

        public void Insert(Clase clase)
        {
            conexion.Insert(clase); 
        }

        public void Update(Clase clase)
        {
               conexion.Update(clase);
        }

        public void Delete(Clase clase)
        {
            conexion.Delete(clase); 
        }

    }
}
 