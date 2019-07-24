using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.Commons.Enums
{
    public enum AlertTypeEnum
    {
        [Description("Error")]
        Error,
        [Description("Warning")]
        Warning,
        [Description("Info")]
        Info,
        [Description("Success")]
        Success,
        [Description("Alert")]
        Alert,
        [Description("ErrorButton")]
        ErrorButton,
        [Description("Modal")]
        Modal
    }
}
