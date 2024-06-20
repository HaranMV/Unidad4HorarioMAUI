using HorarioMAUI.Models;
using HorarioMAUI.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HorarioMAUI.ViewModels
{
    public class HorarioViewModel : INotifyPropertyChanged
    {
        public Clase Clase { get; set; } = null!;
        public Actividad Actividad { get; set; } = null!;
        public ObservableCollection<NotaModel> Notas { get; set; } = new();
        public string Error { get; set; } = "";
        private Dias dias;
        public Dias Dias
        {
            get
            {
                return dias;
            }

            set
            {
                dias = value;
                MostrarLista();
            }
            
        }
        public IEnumerable<Dias> ListaDias
        {
            get
            {
                return Enum.GetValues(typeof(Dias)).Cast<Dias>(); 
            }
        } 

        ActividadRepository actRepos = new();
        ClaseRepository claseRepos = new(); 

        public ICommand VerAgregarClaseCommand {  get; set; }
        public ICommand VerAgregarActividadCommand {  get; set; }
        public ICommand CancelarCommmad {  get; set; }
        public ICommand AggClaseCommand { get; set; }   
        public ICommand AggActividadCommand { get; set; }   
        public ICommand VerEditarCommand { get; set; }   
        public ICommand EditarClaseCommand { get; set; }   
        public ICommand EditarActividadCommand { get; set; }   
        public ICommand VerEliminarCommand { get; set; }   
        public ICommand EliminarClaseCommand { get; set; }   
        public ICommand EliminarActividadCommand { get; set; }   

        public HorarioViewModel()
        {
            VerAgregarActividadCommand = new Command(VerAggActividad);
            VerAgregarClaseCommand = new Command(VerAggClase);
            CancelarCommmad = new Command(Cancelar);
            AggClaseCommand = new Command(AggClase);
            AggActividadCommand = new Command(AggActividad);
            VerEditarCommand = new Command<NotaModel>(VerEditar);
            EditarClaseCommand = new Command(EditarClase);
            EditarActividadCommand = new Command(EditarActividad);
            VerEliminarCommand = new Command<NotaModel>(VerEliminar);
            EliminarClaseCommand = new Command(EliminarClase);
            EliminarActividadCommand = new Command(EliminarActividad);
        }

        private void EliminarActividad()
        {
            if(Actividad != null)
            {
                actRepos.Delete(Actividad);
                MostrarLista();
                Shell.Current.GoToAsync("//Horario");
            }
           
        }

        private void EliminarClase()
        {
            if(Clase != null)
            {
                claseRepos.Delete(Clase);
                MostrarLista();
                Shell.Current.GoToAsync("//Horario");
            }
        }

        public void VerEliminar(NotaModel nota)
        {
            Error = "";
            if (nota.Descripcion == "")
            {
                var clases = claseRepos.GetByDay(nota.Dia);
                Clase clase = clases.Where(x => x.HoraInicio == nota.HoraInicio).First();
                if (clase != null)
                {
                    Clase = clase;
                    Shell.Current.GoToAsync("//EliminarClase");
                    Actualizar(nameof(Clase));
                }
            }
            else
            {
                var actividades = actRepos.GetByDay(nota.Dia);
                Actividad actividad = actividades.Where(x => x.HoraInicio == nota.HoraInicio).First();
                if (actividad != null)
                {
                    Actividad = actividad;
                    Shell.Current.GoToAsync("//EliminarActividad");
                    Actualizar(nameof(Actividad));
                }
            }
            Actualizar(nameof(Error));
        }

        private void EditarActividad()
        {
            Error = "";

            if (Actividad != null)
            {
                if (string.IsNullOrWhiteSpace(Actividad.NombreActividad))
                {
                    Error += "Ingrese el nombre de la actividad";
                }

                if (string.IsNullOrWhiteSpace(Actividad.Descripcion))
                {
                    Error += "Ingrese la descripción de la actividad";
                }


                if (claseRepos.GetByDay(Dias).Any(x => x.HoraInicio == Actividad.HoraInicio ||
                   (Actividad.HoraInicio >= x.HoraInicio && Actividad.HoraInicio < x.HoraFin) ||
                   (Actividad.HoraInicio < x.HoraInicio && Actividad.HoraFin >= x.HoraFin)))
                {
                    Error += "Ya existe una clase en esa hora\n";
                }

                if (actRepos.GetByDay(Dias).Any(x => (x.HoraInicio == Actividad.HoraInicio && x.Id != Actividad.Id) ||
                   (Actividad.HoraInicio >= x.HoraInicio && Actividad.HoraInicio < x.HoraFin && x.Id != Actividad.Id) ||
                   (Actividad.HoraInicio < x.HoraInicio && Actividad.HoraFin >= x.HoraFin && x.Id != Actividad.Id)))
                {
                    Error += "Ya existe una actividad en esa hora\n";
                }

                if (Actividad.HoraInicio == Actividad.HoraFin)
                {
                    Error += "La hora de inicio no puede ser igual a la hora fin\n";
                }

                if (Error == "")
                {
                    actRepos.Update(Actividad);
                    MostrarLista();
                    Shell.Current.GoToAsync("//Horario");
                }
            }
        }

        private void EditarClase()
        {
            Error = "";
            if (Clase != null)
            {
                if (string.IsNullOrWhiteSpace(Clase.NombreMaestro))
                {
                    Error += "Ingrese el nombre del maestro\n";
                }

                if (string.IsNullOrWhiteSpace(Clase.NombreAsignatura))
                {
                    Error += "Ingrese el nombre de la asignatura\n";
                }

                if (string.IsNullOrWhiteSpace(Clase.Aula))
                {
                    Error += "Ingrese el nombre del aula\n";
                }

                if (Clase.HoraInicio <= 0 || Clase.HoraInicio > 24)
                {
                    Error += "Ingrese una hora valida\n";
                }

                if (Clase.HoraFin <= 0 || Clase.HoraFin > 24)
                {
                    Error += "Ingrese una hora valida\n";
                }

                if (claseRepos.GetByDay(Dias).Any(x => (x.HoraInicio == Clase.HoraInicio && x.Id != Clase.Id) || 
                   (Clase.HoraInicio >= x.HoraInicio && Clase.HoraInicio < x.HoraFin && x.Id != Clase.Id) ||
                   (Clase.HoraInicio < x.HoraInicio && Clase.HoraFin >= x.HoraFin && x.Id != Clase.Id)))
                {
                    Error += "Ya existe una clase en esa hora\n"; 
                }

                if (actRepos.GetByDay(Dias).Any(x => x.HoraInicio == Clase.HoraInicio ||
                   (Clase.HoraInicio >= x.HoraInicio && Clase.HoraInicio < x.HoraFin) ||
                   (Clase.HoraInicio < x.HoraInicio && Clase.HoraFin >= x.HoraFin)))
                {
                    Error += "Ya existe una actividad en esa hora\n";
                }

                if (Clase.HoraInicio == Clase.HoraFin)
                {
                    Error += "La hora de inicio no puede ser igual a la hora fin\n";
                }

                Actualizar(nameof(Error));

                if (Error == "")
                {
                    claseRepos.Update(Clase);
                    MostrarLista();
                    Shell.Current.GoToAsync("//Horario");
                }

            }

        }

        private void VerEditar(NotaModel nota)
        {
            Error = "";
            if(nota.Descripcion == "")
            {
               var clases = claseRepos.GetByDay(nota.Dia);
               Clase clase = clases.Where(x => x.HoraInicio == nota.HoraInicio).First(); 
               if(clase != null)
               { 
                    Clase = clase;
                    Shell.Current.GoToAsync("//EditarClase");
                    Actualizar(nameof(Clase));
               }
            }
            else
            {
                var actividades = actRepos.GetByDay(nota.Dia);
                Actividad actividad = actividades.Where(x => x.HoraInicio == nota.HoraInicio).First();
                if (actividad != null)
                {
                    Actividad = actividad;
                    Shell.Current.GoToAsync("//EditarActividad");
                    Actualizar(nameof(Actividad));
                }
            }
            Actualizar(nameof(Error));
        }

        private void AggActividad()
        {
            Error = "";

            if (Actividad != null)
            {
                if (string.IsNullOrWhiteSpace(Actividad.NombreActividad))
                {
                    Error += "Ingrese el nombre de la actividad\n";
                }

                if (string.IsNullOrWhiteSpace(Actividad.Descripcion))
                {
                    Error += "Ingrese la descripción de la actividad\n";
                }


                if (claseRepos.GetByDay(Dias).Any(x => x.HoraInicio == Actividad.HoraInicio ||
                   (Actividad.HoraInicio >= x.HoraInicio && Actividad.HoraInicio < x.HoraFin) ||
                   (Actividad.HoraInicio < x.HoraInicio && Actividad.HoraFin >= x.HoraFin)))
                {
                    Error += "Ya existe una clase en esa hora\n";
                }

                if (actRepos.GetByDay(Dias).Any(x => x.HoraInicio == Actividad.HoraInicio ||
                   (Actividad.HoraInicio >= x.HoraInicio && Actividad.HoraInicio < x.HoraFin) ||
                   (Actividad.HoraInicio < x.HoraInicio && Actividad.HoraFin >= x.HoraFin)))
                {
                    Error += "Ya existe una actividad en esa hora\n";
                }

                if (Actividad.HoraInicio == Actividad.HoraFin)
                {
                    Error += "La hora de inicio no puede ser igual a la hora fin\n";
                }
                Actualizar(nameof(Error));

                if (Error == "")
                {
                    actRepos.Insert(Actividad);
                    MostrarLista();
                    Shell.Current.GoToAsync("//Horario");
                }
            }        
        }

        private void AggClase()
        {
            Error = "";

            if (Clase != null)
            {
                if (string.IsNullOrWhiteSpace(Clase.NombreMaestro))
                {
                    Error += "Ingrese el nombre del maestro\n";
                }

                if (string.IsNullOrWhiteSpace(Clase.NombreAsignatura))
                {
                    Error += "Ingrese el nombre de la asignatura\n";
                }

                if (string.IsNullOrWhiteSpace(Clase.Aula))
                {
                    Error += "Ingrese el nombre del aula\n";
                }

                if (Clase.HoraInicio <= 0 || Clase.HoraInicio > 24)
                {
                    Error += "Ingrese una hora valida\n";
                }

                if (Clase.HoraFin <= 0 || Clase.HoraFin > 24)
                {
                    Error += "Ingrese una hora valida\n";
                }

                if (claseRepos.GetByDay(Dias).Any(x => x.HoraInicio == Clase.HoraInicio ||
                   (Clase.HoraInicio >= x.HoraInicio && Clase.HoraInicio < x.HoraFin) ||
                   (Clase.HoraInicio < x.HoraInicio && Clase.HoraFin >= x.HoraFin)))
                {
                    Error += "Ya existe una clase en esa hora\n";
                }

                if (actRepos.GetByDay(Dias).Any(x => x.HoraInicio == Clase.HoraInicio ||
                   (Clase.HoraInicio >= x.HoraInicio && Clase.HoraInicio < x.HoraFin) || 
                   (Clase.HoraInicio < x.HoraInicio && Clase.HoraFin >= x.HoraFin)))
                {
                    Error += "Ya existe una actividad en esa hora\n";
                }

                if(Clase.HoraInicio == Clase.HoraFin)
                {
                    Error += "La hora de inicio no puede ser igual a la hora fin\n";
                }

                Actualizar(nameof(Error));

                if (Error == "")
                {
                    claseRepos.Insert(Clase);
                    MostrarLista();
                    Shell.Current.GoToAsync("//Horario");
                }
            }
        }

        Random r = new Random();

        private void MostrarLista()
        {
            var clases = claseRepos.GetByDay(Dias);
            var actividades = actRepos.GetByDay(Dias);
            List<NotaModel> listanota = new();
            foreach (var clase in clases)
            {
                NotaModel nota = new()
                {
                    Id = clase.Id,
                    NombreMaestro = clase.NombreMaestro,
                    Nombre = clase.NombreAsignatura,
                    Aula = clase.Aula,
                    Dia = clase.Dia,
                    Descripcion = "",
                    HoraInicio = clase.HoraInicio,    
                    HoraFin = clase.HoraFin,  
                    Color = (Colores)r.Next(0, 6)
                };
                listanota.Add(nota);
            }

            foreach (var actividad in actividades)
            {                
                NotaModel nota = new()
                {
                    Id = actividad.Id,
                    NombreMaestro = "",
                    Nombre = actividad.NombreActividad,
                    Aula = "",
                    Dia = actividad.Dia,
                    Descripcion = actividad.Descripcion,
                    HoraInicio = actividad.HoraInicio,
                    HoraFin = actividad.HoraFin,
                    Color = (Colores)r.Next(0, 6)
                };
                listanota.Add(nota);    
            }
            Notas.Clear();
            var notas = listanota.OrderBy(x => x.HoraInicio).ToList();
            foreach (var nota in notas)
            {
                Notas.Add(nota);
            }
        }

        private void Cancelar()
        {
            Shell.Current.GoToAsync("//Horario");
        }

        private void VerAggClase()
        {
            Error = "";
            Clase = new()
            {
                Dia = Dias
            };
            Shell.Current.GoToAsync("//AgregarClase");
            Actualizar(nameof(Clase));
            Actualizar(nameof(Error));
        }

        private void VerAggActividad()
        {
            Error = "";
            Actividad = new()
            {
                Dia = Dias
            };
            Shell.Current.GoToAsync("//AgregarActividad");
            Actualizar(nameof(Actividad));
            Actualizar(nameof(Error));
        }

        private void Actualizar(string name)
        {
            PropertyChanged?.Invoke(this, new(name));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
