using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.Commons.Enums
{
 
        public enum TypeProyectEnum
        {
        [Description("Proyecto Standard  ")]
        Standard,
        [Description("Proyecto Mantenimiento ")]
        Mantenimiento,
        [Description("Proyecto TimeOff")]
        TimeOff,
        [Description("Proyecto Back Office")]
        BackOffice
    }
    public enum TypeEmployerEnum
    {
        [Description("Interno")]
        Interno,
        [Description("Externo")]
        Externo
    }

    public enum TimeOffTypeEnum
    {
        [Description("Vacaciones")]
        Vacaciones,
        [Description("Feriados")]
        Feriados,
        [Description("Dia Estudio")]
        DiaEstudio,
        [Description("Dia por Enfermedad")]
        DiaporEnfermedad,
        [Description("Licencia especial")]
        LicenciaEspecial

    }

}
