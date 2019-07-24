using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.Commons.Enums
{
    public static class EnumsExtensions
    {
        // <summary>
        /// Devuelve la descripcion de un enumerador
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes?.Any() ?? false)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
