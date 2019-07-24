using GestorInventarioEmpresas.BackEnd.Commons.Enums;
using GestorInventarioEmpresas.BackEnd.Services;
using GestorInventarioEmpresas.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GestorInventarioEmpresas.Controllers
{
    [System.Web.Http.Authorize(Roles = "Administrador,Reportes")]
    public class ReportesController : BaseController
    {
        private ICompanyService _companyService;
        private IProyectService _projectService;
        private IWorkDayService _workDayService;
        private ITaskTypeService _taskTypeService;
        private IInstanceDayService _instanceDayService;
        private IUserService _userService;

        public List<string> ListaColores = new List<string>();
        public ReportesController(ICompanyService CompanyService, IProyectService ProyectService, IWorkDayService WorkDayService, ITaskTypeService TaskTypeService, IInstanceDayService InstanceDayService, IUserService userService)
        {
            _companyService = CompanyService;
            _projectService = ProyectService;
            _workDayService = WorkDayService;
            _taskTypeService = TaskTypeService;
            _instanceDayService = InstanceDayService;
            _userService = userService;
            ListaColores.Add("#071826");
            ListaColores.Add("#164059");
            ListaColores.Add("#567D8C");
            ListaColores.Add("#337ab7");
            ListaColores.Add("#071826");
            ListaColores.Add("#F3E3D5");
            ListaColores.Add("#324472");
            ListaColores.Add("#2693A1");
            ListaColores.Add("#EFA517");
            ListaColores.Add("#E86D1F");
            ListaColores.Add("#14A01E"); 
            ListaColores.Add("#FFD21D"); 
            ListaColores.Add("#00B4FF"); 
            ListaColores.Add("#FF3C61");
            ListaColores.Add("#517339");
            ListaColores.Add("#FFF500");
            ListaColores.Add("#7664FF");
            
        }
        public ReportesController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ICompanyService CompanyService, IProyectService ProyectService,
           IUserService userService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _companyService = CompanyService;
            _projectService = ProyectService;
            _userService = userService;
        }
        public ActionResult Index() { return View(); }
        public ActionResult ExportUrudata()
        {
            return View();
        }
        public void ExportXML(string dateString)
        {
            var ope = UserManager.FindById(User.Identity.GetUserId()).UserProfile.Id;
            List<dataViewModel> listaReporteFullDetalleSolos = new List<dataViewModel>();
            var date = DateTime.ParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var list = _workDayService.GetAll().Where(x => x.UserProfileId == UserManager.FindById(User.Identity.GetUserId()).UserProfile.Id & x.Date >= firstDayOfMonth & x.Date <= lastDayOfMonth).ToList();
            var objw = list.GroupBy(x => x.Date);
            var listOfProyectInMonth = new List<string>();
            List<Data> listaPieGrafic = new List<Data>(); ;
            while (firstDayOfMonth <= lastDayOfMonth)
            {
                if (!list.Exists(x => x.Date == firstDayOfMonth))
                {
                    list.Add(new BackEnd.Domain.Entities.WorkDay() { Date = firstDayOfMonth });
                }
                firstDayOfMonth = firstDayOfMonth.AddDays(1);
            }
            var listAux = list.OrderBy(q => q.Date);
            var listColors = new List<string>();
            list = listAux.ToList();
            List<dataForTableViewModel> resultado = new List<dataForTableViewModel>();
            foreach (var x in list)
            {
                dataForTableViewModel item = new dataForTableViewModel { date = x.Date.ToString("yyyy-MM-dd"), data = 0 };
                foreach (var y in x.InstanceDay)
                {
                    if (y.Hours != 0)
                    {
                        item.data += y.Hours;
                        var dataInstance = new dataViewModel();
                        dataInstance.company = string.Join(",", y.Companies.Select(t => t.Name));
                        dataInstance.data = y.Hours;
                        dataInstance.project = y.Proyect.Name;
                        dataInstance.taskType = y.TaskType.Name;
                        dataInstance.date = y.WorkDay.Date.ToString("yyyy/MM/dd");
                        item.list.Add(dataInstance);
                        listaReporteFullDetalleSolos.Add(dataInstance);
                    } 
                }
                resultado.Add(item);
            }
            //Borra los dias que no tienen horas cargadas
            resultado.RemoveAll(x => x.data == 0);


            XmlDocument doc = new XmlDocument();

            // XML declaration
            XmlNode declaration = doc.CreateNode(XmlNodeType.XmlDeclaration, null, null);
            doc.AppendChild(declaration);

            // Root element: Catalog
            XmlElement root = doc.CreateElement("HORAS");
            doc.AppendChild(root);
            foreach (var item in listaReporteFullDetalleSolos)
            {
                // Sub-element: srsapiversion of root
                XmlElement Hora = doc.CreateElement("HORA");
                XmlAttribute Fecha = doc.CreateAttribute("Fecha");
                Fecha.Value = item.date;
                XmlAttribute Empresa = doc.CreateAttribute("Empresa");
                Empresa.Value = "ITX";
                XmlAttribute Proyecto = doc.CreateAttribute("Proyecto");
                Proyecto.Value = "NM002";
                XmlAttribute Horas = doc.CreateAttribute("Horas");
                Horas.Value = item.data.ToString();
                XmlAttribute HE = doc.CreateAttribute("HE");
                HE.Value = "S";
                XmlAttribute OT = doc.CreateAttribute("OT");
                OT.Value = "100";
                XmlAttribute DESC = doc.CreateAttribute("DESC");
                DESC.Value = item.taskType +" - " + item.project;
                XmlAttribute Tipo = doc.CreateAttribute("Tipo");
                Tipo.Value = "DS";
                 
                Hora.Attributes.Append(Fecha);
                Hora.Attributes.Append(Empresa);
                Hora.Attributes.Append(Proyecto);
                Hora.Attributes.Append(Horas);
                Hora.Attributes.Append(HE);
                Hora.Attributes.Append(OT);
                Hora.Attributes.Append(DESC);
                Hora.Attributes.Append(Tipo);
                root.AppendChild(Hora);
            }
          
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, System.Text.Encoding.UTF8);

            doc.WriteTo(writer);
            writer.Flush();
            Response.Clear();
            byte[] byteArray = stream.ToArray();
            Response.AppendHeader("Content-Disposition", "filename=ExportarHoras_"+dateString+"_"+ User.Identity.Name + ".xml");
            Response.AppendHeader("Content-Length", byteArray.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.BinaryWrite(byteArray);
            writer.Close();
            //return File(byteArray, System.Net.Mime.MediaTypeNames.Application.Octet, "Sample.xml");
        }
        public ActionResult ReportePorProyectos() { return View(); }
        public ActionResult ProyectoPorCompanias() { return View(); } 
        public ActionResult getReportForTableByCompany(string dateString, long idCompany)
        {
            var date = DateTime.ParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            List<dataViewModel> listaReporteFullDetalleSolos = new List<dataViewModel>();
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var list = _workDayService.GetAll().Where(x => x.Date >= firstDayOfMonth & x.Date <= lastDayOfMonth).ToList();
            var objw = list.GroupBy(x => x.Date);
            var listOfProjectInMonth = new List<string>();
            List<Data> listaPieGrafic = new List<Data>(); ;
            do
            {
                if (!list.Exists(x => x.Date == firstDayOfMonth))
                {
                    list.Add(new BackEnd.Domain.Entities.WorkDay() { Date = firstDayOfMonth });
                }
                firstDayOfMonth = firstDayOfMonth.AddDays(1);
            } while (firstDayOfMonth <= lastDayOfMonth);

            var listAux = list.OrderBy(q => q.Date);
            var listColors = new List<string>();
            list = listAux.ToList();
            List<dataForTableViewModel> resultado = new List<dataForTableViewModel>();
            var firstDay = new DateTime(date.Year, date.Month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);
            do
            {
                dataForTableViewModel item = new dataForTableViewModel { date = firstDay.ToString("dd-MM-yyyy"), data = 0 };
                var listX = list.FindAll(e => e.Date == firstDay);
                foreach (var x in listX)
                {
                    foreach (var y in x.InstanceDay)
                    {
                        if (y.Hours != 0 && y.Companies.Exists(t=>t.Id == idCompany))
                        {
                            item.data += Math.Round(y.Hours / y.Companies.Count,2);
                            var dataInstance = new dataViewModel();
                            dataInstance.project = y.Proyect.Name; 
                            dataInstance.data = Math.Round(y.Hours/y.Companies.Count,2);
                            dataInstance.user = y.WorkDay.UserProfile.Name;
                            dataInstance.locationEmployer =  y.WorkDay.UserProfile.Location.GetEnumDescription();
                            dataInstance.taskType = y.TaskType.Name;
                            dataInstance.company = string.Join(",", y.Companies.Select(t => t.Name));
                            dataInstance.date = y.WorkDay.Date.ToString("dd-MM-yyyy");
                            item.list.Add(dataInstance);
                            listaReporteFullDetalleSolos.Add(dataInstance);
                        }
                    }
                }
                resultado.Add(item);
                firstDay = firstDay.AddDays(1);
            } while (firstDay <= lastDay);
            //Borra los dias que no tienen horas cargadas
            resultado.RemoveAll(x => x.data == 0);
            return Json(new { data = resultado, dataReporteFullDetalle = listaReporteFullDetalleSolos }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getReportForTableByProject(string dateString, long idProject)
        {
            var date = DateTime.ParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            List<dataViewModel> listaReporteFullDetalleSolos = new List<dataViewModel>();
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var list = _workDayService.GetAll().Where(x => x.Date >= firstDayOfMonth & x.Date <= lastDayOfMonth).ToList();
            var objw = list.GroupBy(x => x.Date);
            var listOfProjectInMonth = new List<string>();
            List<Data> listaPieGrafic = new List<Data>(); ;
            do
            {
                if (!list.Exists(x => x.Date == firstDayOfMonth))
                {
                    list.Add(new BackEnd.Domain.Entities.WorkDay() { Date = firstDayOfMonth });
                }
                firstDayOfMonth = firstDayOfMonth.AddDays(1);
            } while (firstDayOfMonth <= lastDayOfMonth);

            var listAux = list.OrderBy(q => q.Date);
            var listColors = new List<string>();
            list = listAux.ToList();
            List<dataForTableViewModel> resultado = new List<dataForTableViewModel>();
            var firstDay = new DateTime(date.Year, date.Month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);
            do
            {
                dataForTableViewModel item = new dataForTableViewModel { date = firstDay.ToString("yyyy-MM-dd"), data = 0 };
                var listX = list.FindAll(e => e.Date == firstDay);
                foreach (var x in listX)
                {
                    foreach (var y in x.InstanceDay)
                    {
                        if (y.Hours != 0 && y.ProyectId == idProject)
                        {
                            item.data += y.Hours;
                            var dataInstance = new dataViewModel();
                            dataInstance.company = string.Join(",", y.Companies.Select(t => t.Name));
                            dataInstance.data = y.Hours;
                            dataInstance.user = y.WorkDay.UserProfile.Name;
                            dataInstance.locationEmployer = y.WorkDay.UserProfile.Location.GetEnumDescription();
                            dataInstance.taskType = y.TaskType.Name;
                            dataInstance.date = y.WorkDay.Date.ToString("dd-MM-yyyy");
                  
                            listaReporteFullDetalleSolos.Add(dataInstance);
                       
                            item.list.Add(dataInstance);
                        }
                    }
                }
                resultado.Add(item);
                firstDay = firstDay.AddDays(1);
            } while (firstDay <= lastDay);
            //Borra los dias que no tienen horas cargadas
            resultado.RemoveAll(x => x.data == 0);
            return Json(new { data = resultado, dataReporteFullDetalle = listaReporteFullDetalleSolos }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult getReportForTable(string dateString, long idUser)
        {
            List<dataViewModel> listaReporteFullDetalleSolos = new List<dataViewModel>();
            var date = DateTime.ParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var list = _workDayService.GetAll().Where(x => x.UserProfileId == idUser & x.Date >= firstDayOfMonth & x.Date <= lastDayOfMonth).ToList();
            var objw = list.GroupBy(x => x.Date);
            var listOfProjectInMonth = new List<string>();
            List<Data> listaPieGrafic = new List<Data>(); ;
            while (firstDayOfMonth <= lastDayOfMonth)
            {
                if (!list.Exists(x => x.Date == firstDayOfMonth))
                {
                    list.Add(new BackEnd.Domain.Entities.WorkDay() { Date = firstDayOfMonth });
                }
                firstDayOfMonth = firstDayOfMonth.AddDays(1);
            }
            var listAux = list.OrderBy(q => q.Date);
            var listColors = new List<string>();
            list = listAux.ToList();
            List<dataForTableViewModel> resultado = new List<dataForTableViewModel>();
            foreach (var x in list)
            {
                dataForTableViewModel item = new dataForTableViewModel { date = x.Date.ToString("yyyy-MM-dd"), data = 0 };
                foreach (var y in x.InstanceDay)
                {
                    if (y.Hours != 0)
                    {
                        item.data += y.Hours;
                        var dataInstance = new dataViewModel();
                        dataInstance.company = string.Join(",", y.Companies.Select(t => t.Name));
                        dataInstance.data = y.Hours;
                        dataInstance.project = y.Proyect.Name;
                        dataInstance.taskType = y.TaskType.Name;
                        dataInstance.date = y.WorkDay.Date.ToString("dd-MM-yyyy");
                       // dataInstance.locationProject = y.Proyect.Location.GetEnumDescription();
                        item.list.Add(dataInstance);
                        listaReporteFullDetalleSolos.Add(dataInstance);
                    }

                }
                resultado.Add(item);
            }
            //Borra los dias que no tienen horas cargadas
            resultado.RemoveAll(x => x.data == 0); 
            return Json(new { data = resultado, dataReporteFullDetalle=listaReporteFullDetalleSolos }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult getAllUser()
        {
            return this.Json(
                  (from obj in _userService.GetAll() select new { Id = obj.Id, Text = obj.Name })
                  , JsonRequestBehavior.AllowGet);
        }
        public ActionResult getAllProject()
        {
            return this.Json(
                  (from obj in _projectService.GetAll() select new { Id = obj.Id, Text = obj.Name })
                  , JsonRequestBehavior.AllowGet);
        }
        public ActionResult getAllCompanies()
        {
            return this.Json(
                  (from obj in _companyService.GetAll() select new { Id = obj.Id, Text = obj.Name })
                  , JsonRequestBehavior.AllowGet);
        }
        public ActionResult getReportBasicByProject(string dateString, long idProject)
        {
            var date = DateTime.ParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var list = _workDayService.GetAll().Where(x => x.Date >= firstDayOfMonth & x.Date <= lastDayOfMonth).ToList();

            var listOfProjectInMonth = new List<string>();
            List<Data> listaPieGrafic = new List<Data>(); ;


            do
            {
                if (!list.Exists(x => x.Date == firstDayOfMonth))
                {
                    list.Add(new BackEnd.Domain.Entities.WorkDay() { Date = firstDayOfMonth });
                }
                firstDayOfMonth = firstDayOfMonth.AddDays(1);
            } while (firstDayOfMonth != lastDayOfMonth);

            var listAux = list.OrderBy(q => q.Date);
            var listColors = new List<string>();
            list = listAux.ToList();
            List<Dictionary<string, string>> resultado = new List<Dictionary<string, string>>();
            List<string> keyList = new List<string>();
            Dictionary<string, string> datosPieGrafic = new Dictionary<string, string>();
            decimal HorasTotales = 0;
            var firstDay = new DateTime(date.Year, date.Month, 1);
            var lastDay = firstDayOfMonth.AddMonths(1).AddDays(-1);
            do
            {
                Dictionary<string, string> item = new Dictionary<string, string>();
                var listX = list.FindAll(e => e.Date == firstDay);
                item.Add("y", firstDay.Date.ToString("yyyy-MM-dd"));
                foreach (var x in listX)
                {

                    foreach (var y in x.InstanceDay)
                    {
                        if (y.Proyect.Id == idProject)
                        {
                            var userName = y.WorkDay.UserProfile.Name;
                            if (item.ContainsKey(userName))
                            {
                                item[userName] = (Convert.ToDecimal(item[userName]) + Convert.ToDecimal(y.Hours.ToString())).ToString();
                            }
                            else
                            {
                                item.Add(userName, y.Hours.ToString());
                            }
                     
                            HorasTotales += Convert.ToDecimal(y.Hours.ToString());
                            if (!keyList.Contains(userName))
                            {
                                datosPieGrafic.Add(userName, y.Hours.ToString());
                                keyList.Add(userName);
                                listColors.Add(ListaColores[keyList.Count%ListaColores.Count]);
                            }
                            else { datosPieGrafic[userName] = (Convert.ToDecimal(datosPieGrafic[userName]) + y.Hours).ToString(); }

                        }
                    }

                    resultado.Add(item);

                    listaPieGrafic = datosPieGrafic.Select(p => new Data { label = p.Key, data = Convert.ToDecimal(p.Value) }).ToList();

                }

                firstDay = firstDay.AddDays(1);
            } while (firstDay != lastDay);
            return Json(new { data = resultado, Keys = keyList.ToArray(), colores = listColors, datosPieGrafic = listaPieGrafic, HorasTotales = HorasTotales }, JsonRequestBehavior.AllowGet);
            
        }
        public ActionResult getReportBasic(string dateString, long idUser)
        {
            var date = DateTime.ParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var list = _workDayService.GetAll().Where(x => x.UserProfileId == idUser & x.Date >= firstDayOfMonth & x.Date <= lastDayOfMonth).ToList();
            var listOfProjectInMonth = new List<string>();
            List<Data> listaPieGrafic = new List<Data>(); ;
            do
            {
                if (!list.Exists(x => x.Date == firstDayOfMonth))
                {
                    list.Add(new BackEnd.Domain.Entities.WorkDay() { Date = firstDayOfMonth });
                }
                firstDayOfMonth = firstDayOfMonth.AddDays(1);
            } while (firstDayOfMonth <= lastDayOfMonth);
            var listAux = list.OrderBy(q => q.Date);
            var listColors = new List<string>();
            list = listAux.ToList();
            List<Dictionary<string, string>> resultado = new List<Dictionary<string, string>>();
            List<string> keyList = new List<string>();
            Dictionary<string, string> datosPieGrafic = new Dictionary<string, string>();
            decimal HorasTotales = 0;
            foreach (var x in list)
            {
                Dictionary<string, string> item = new Dictionary<string, string>();
                item.Add("y", x.Date.ToString("yyyy-MM-dd"));
                foreach (var y in x.InstanceDay)
                {
                    string NameCompanies = y.Companies.Count == 1 ? string.Join(",", y.Companies.Select(t => t.Name)) : "";
                    if (item.ContainsKey(y.Proyect.Name + "-" + NameCompanies))
                    {
                        item[y.Proyect.Name + "-" + NameCompanies]= (Convert.ToDecimal(item[y.Proyect.Name + "-" + NameCompanies]) + Convert.ToDecimal(y.Hours.ToString())).ToString();
                    }
                    else
                    {
                    item.Add(y.Proyect.Name + "-" + NameCompanies, y.Hours.ToString());
                    }
                    HorasTotales += Convert.ToDecimal(y.Hours.ToString());
                    if (!keyList.Contains(y.Proyect.Name + "-" + NameCompanies))
                    {
                        datosPieGrafic.Add(y.Proyect.Name + "-" + NameCompanies, y.Hours.ToString());
                        keyList.Add(y.Proyect.Name + "-" + NameCompanies);

                        listColors.Add(ListaColores[keyList.Count%ListaColores.Count]);
                    }
                    else { datosPieGrafic[y.Proyect.Name + "-" + NameCompanies] = (Convert.ToDecimal(datosPieGrafic[y.Proyect.Name + "-" + NameCompanies]) + y.Hours).ToString(); }
                }
                resultado.Add(item);

                listaPieGrafic = datosPieGrafic.Select(p => new Data { label = p.Key, data = Convert.ToDecimal(p.Value) }).ToList();
            }

            return Json(new { data = resultado, Keys = keyList.ToArray(), colores = listColors, datosPieGrafic = listaPieGrafic, HorasTotales = HorasTotales }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult getReportBasicByCompany(string dateString, long idCompany)
        {
            var date = DateTime.ParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var list = _workDayService.GetAll().Where(x => x.Date >= firstDayOfMonth & x.Date <= lastDayOfMonth).ToList();

            var listOfProjectInMonth = new List<string>();
            List<Data> listaPieGrafic = new List<Data>(); ;
            do
            {
                if (!list.Exists(x => x.Date == firstDayOfMonth))
                {
                    list.Add(new BackEnd.Domain.Entities.WorkDay() { Date = firstDayOfMonth });
                }
                firstDayOfMonth = firstDayOfMonth.AddDays(1);
            } while (firstDayOfMonth != lastDayOfMonth);

            var listAux = list.OrderBy(q => q.Date);
            var listColors = new List<string>();
            list = listAux.ToList();
            List<Dictionary<string, string>> resultado = new List<Dictionary<string, string>>();
            List<string> keyList = new List<string>();
            Dictionary<string, string> datosPieGrafic = new Dictionary<string, string>();
            decimal HorasTotales = 0;
            var firstDay = new DateTime(date.Year, date.Month, 1);
            var lastDay = firstDayOfMonth.AddMonths(1).AddDays(-1);
            do
            {
                Dictionary<string, string> item = new Dictionary<string, string>();
                var listX = list.FindAll(e => e.Date == firstDay);
                item.Add("y", firstDay.Date.ToString("yyyy-MM-dd"));
                foreach (var x in listX)
                { 
                    foreach (var y in x.InstanceDay)
                    {
                        if (y.Companies.Exists(z=>z.Id==idCompany))
                        {
                            var projectName = y.Proyect.Name;
                            if (item.ContainsKey(projectName)) {
                                item[projectName] = Math.Round((Convert.ToDecimal(item[projectName]) + y.Hours / y.Companies.Count),2).ToString();
                            } else {
                                item.Add(projectName, Math.Round((y.Hours / y.Companies.Count), 2).ToString());
                            }
                           
                            HorasTotales +=Math.Round(y.Hours / y.Companies.Count, 2);
                            if (!keyList.Contains(projectName))
                            {
                                datosPieGrafic.Add(projectName, (y.Hours / y.Companies.Count).ToString());
                                keyList.Add(projectName);
                                listColors.Add(ListaColores[keyList.Count%ListaColores.Count]);
                            }
                            else { datosPieGrafic[projectName] = Math.Round(Convert.ToDecimal(datosPieGrafic[projectName]) + (y.Hours / y.Companies.Count),2).ToString(); }

                        }
                    }

                    resultado.Add(item);

                    listaPieGrafic = datosPieGrafic.Select(p => new Data { label = p.Key, data = Convert.ToDecimal(p.Value) }).ToList();

                }

                firstDay = firstDay.AddDays(1);
            } while (firstDay != lastDay);
            return Json(new { data = resultado, Keys = keyList.ToArray(), colores = listColors, datosPieGrafic = listaPieGrafic, HorasTotales = HorasTotales }, JsonRequestBehavior.AllowGet);
                 }

    }
}
