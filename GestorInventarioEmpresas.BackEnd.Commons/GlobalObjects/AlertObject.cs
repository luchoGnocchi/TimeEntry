using GestorInventarioEmpresas.BackEnd.Commons.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.Commons.GlobalObjects
{
    public class AlertObject
    {
        public AlertObject()
        {

        }
        public AlertObject(AlertTypeEnum alertTypeEnum, string message, string title)
        {
            AlertTypeEnum = alertTypeEnum;
            Message = message;
            Title = title;
        }

        public AlertTypeEnum AlertTypeEnum { get; set; }

        public string Message { get; set; }

        public string Title { get; set; }
    }
}
