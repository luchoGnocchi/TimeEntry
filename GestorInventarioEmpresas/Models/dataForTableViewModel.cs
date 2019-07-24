using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorInventarioEmpresas.Models
{
    public class dataForTableViewModel
    {
        public string date { get; set; }
        public decimal data { get; set; }
        public List<dataViewModel> list = new List<dataViewModel>();
    }
    public class dataViewModel
    {
        public string project { get; set; }
        public string user { get; set; }
        public string company { get; set; }
        public string taskType { get; set; }
        public decimal data { get; set; }
        public string date { get; set; }
        public string locationEmployer { get; set; } 
        public string locationProject { get; set; }
    }
}