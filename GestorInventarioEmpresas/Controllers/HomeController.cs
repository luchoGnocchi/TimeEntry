using GestorInventarioEmpresas.BackEnd.Domain;
using GestorInventarioEmpresas.BackEnd.Services;
using GestorInventarioEmpresas.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestorInventarioEmpresas.Controllers
{
    [Authorize]
    public class HomeController : BaseController {

        private ICompanyService _companyService;
        private IProyectService _proyectService;
        private IWorkDayService _workDayService;
        private ITaskTypeService _taskTypeService;
        private IInstanceDayService _instanceDayService;
        public List<string> ListaColores = new List<string>();
        public HomeController(ICompanyService CompanyService, IProyectService ProyectService, IWorkDayService WorkDayService, ITaskTypeService TaskTypeService, IInstanceDayService InstanceDayService)
        {
            _companyService = CompanyService;
            _proyectService = ProyectService;
            _workDayService = WorkDayService;
            _taskTypeService = TaskTypeService;
            _instanceDayService = InstanceDayService;
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
            ListaColores.Add("#521132");
            ListaColores.Add("#FF3F01");
            ListaColores.Add("#1621FA");
        }
        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ICompanyService CompanyService, IProyectService ProyectService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _companyService = CompanyService;
            _proyectService = ProyectService;
        }

        
        public ActionResult Index()
        {
            return View();
        }
      
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
        
            return View();
        }
        public ActionResult getReportBasic(string dateString)
        {
            var date = DateTime.ParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var dateNow = DateTime.Now;
            var dateFirstDayOfWeek = DateTime.Now;
            var dateLastDayOfWeek = DateTime.Now;
            
            while (dateLastDayOfWeek.DayOfWeek!=DayOfWeek.Sunday) {
                dateLastDayOfWeek= dateLastDayOfWeek.AddDays(1);
            }
            while (dateFirstDayOfWeek.DayOfWeek != DayOfWeek.Monday)
            {
                dateFirstDayOfWeek=dateFirstDayOfWeek.AddDays(-1);
            }
            //La semana pidieron que vaya de Lunes a Domingo
            dateLastDayOfWeek = dateLastDayOfWeek.AddDays(-1);
            dateFirstDayOfWeek = dateFirstDayOfWeek.AddDays(-1);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var list = _workDayService.GetAll().Where(x => x.UserProfileId == UserManager.FindById(User.Identity.GetUserId()).UserProfile.Id & x.Date >= firstDayOfMonth & x.Date <= lastDayOfMonth).ToList();
            var listOfProyectInMonth = new List<string>();
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
            decimal HorasSemanales = 0;
            foreach (var x in list)
            {
                Dictionary<string, string> item = new Dictionary<string, string>();
                item.Add("y", x.Date.ToString("yyyy-MM-dd"));
                foreach (var y in x.InstanceDay)
                {
                    string NameCompanies = y.Companies.Count == 1 ? string.Join(",", y.Companies.Select(t => t.Name)) : "";
                    if (item.ContainsKey(y.Proyect.Name + "-" + NameCompanies))
                    {
                        item[y.Proyect.Name + "-" + NameCompanies] = (Convert.ToDecimal(item[y.Proyect.Name + "-" + NameCompanies]) + y.Hours).ToString();
                    }
                    else
                    {
                        item.Add(y.Proyect.Name + "-" + NameCompanies, y.Hours.ToString());
                    }
                    HorasTotales += y.Hours;
                    if (y.WorkDay.Date >= dateFirstDayOfWeek & y.WorkDay.Date <= dateLastDayOfWeek)
                    {
                        HorasSemanales+=y.Hours;
                    }
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

            return Json(new { data = resultado, Keys = keyList.ToArray(), colores = listColors, datosPieGrafic = listaPieGrafic, HorasTotales = HorasTotales, HorasSemanales= HorasSemanales }, JsonRequestBehavior.AllowGet);

        } 
    }
}