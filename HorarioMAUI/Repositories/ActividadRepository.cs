using HorarioMAUI.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorarioMAUI.Repositories
{
    public class ActividadRepository
    {
        SQLiteConnection conexion;

        public ActividadRepository()
        {
            string ruta = FileSystem.AppDataDirectory + "/actividades.sqlite";
            conexion = new(ruta);
            conexion.CreateTable<Actividad>();  
        }

        public IEnumerable<Actividad> GetByDay(Dias dia)
        {
            return conexion.Table<Actividad>().Where(x => x.Dia == dia);
        }


        public void Insert(Actividad actividad)
        {
            conexion.Insert(actividad); 
        }

        public void Update(Actividad actividad)
        {
            conexion.Update(actividad); 
        }

        public void Delete(Actividad actividad)
        {
            conexion.Delete(actividad);
        }


    }
}
