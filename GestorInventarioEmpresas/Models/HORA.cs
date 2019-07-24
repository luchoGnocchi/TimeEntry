using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace GestorInventarioEmpresas.Models
{
    public class HORA
    { 
		[XmlAttribute(AttributeName = "Fecha")]
        public string Fecha { get; set; }
        [XmlAttribute(AttributeName = "Empresa")]
        public string Empresa { get; set; }
        [XmlAttribute(AttributeName = "Proyecto")]
        public string Proyecto { get; set; }
        [XmlAttribute(AttributeName = "Horas")]
        public string Horas { get; set; }
        [XmlAttribute(AttributeName = "HE")]
        public string HE { get; set; }
        [XmlAttribute(AttributeName = "Tipo")]
        public string Tipo { get; set; }
        [XmlAttribute(AttributeName = "OT")]
        public string OT { get; set; }
        [XmlAttribute(AttributeName = "DESC")]
        public string DESC { get; set; }
    }

    [XmlRoot(ElementName = "HORAS")]
    public class HORAS
    {
        [XmlElement(ElementName = "HORA")]
        public List<HORA> HORA { get; set; }
    }

}