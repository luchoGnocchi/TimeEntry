using GestorInventarioEmpresas.ViewModels.LoadHours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorInventarioEmpresas.ViewModels
{
    public class LoadWeekViewModel
    {

        public List<long> company{ get; set; }
        public long proyect { get; set; }
        public long proyectOld { get; set; }
        
        public decimal TipoTarea { get; set; }
        public decimal Lunes { get; set; }
        public decimal Martes { get; set; }
        public decimal Miercoles { get; set; }
        public decimal Jueves { get; set; }
        public decimal Viernes { get; set; }
        public decimal Sabado { get; set; }
        public decimal Domingo { get; set; }
        public DateTime StartLoad { get; set; }
        public string NombreProyecto { get; set; }
        public string NombreEmpresas { get; set; }
        public string companyOld { get; set; }
        public string NombreTipoTarea { get; internal set; }
        public long TaskTypeOld { get;  set; }
        public long TaskType { get; set; }
        public string StartLoadString { get; set; }
    }
}