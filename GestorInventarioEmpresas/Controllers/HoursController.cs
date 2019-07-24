using GestorInventarioEmpresas.BackEnd.Domain.Entities;
using GestorInventarioEmpresas.BackEnd.Services;
using GestorInventarioEmpresas.ViewModels;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestorInventarioEmpresas.Controllers
{
    [Authorize]
    public class HoursController : BaseController
    {
        private ICompanyService _companyService;
        private IProyectService _proyectService;
        private IWorkDayService _workDayService;
        private ITaskTypeService _taskTypeService;
        private IInstanceDayService _instanceDayService; 
        public HoursController(ICompanyService CompanyService, IProyectService ProyectService, IWorkDayService WorkDayService, ITaskTypeService TaskTypeService, IInstanceDayService InstanceDayService)
        {
            _companyService = CompanyService;
            _proyectService = ProyectService;
            _workDayService = WorkDayService;
            _taskTypeService = TaskTypeService;
            _instanceDayService = InstanceDayService;
        }
        public HoursController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ICompanyService CompanyService, IProyectService ProyectService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _companyService = CompanyService;
            _proyectService = ProyectService;
        }
        public ActionResult LoadHours()
        {

            //return ret;
            return View();
        }
        [HttpPost]
        public JsonResult SaveWeek(LoadWeekViewModel model)
        {
            model.StartLoad = DateTime.ParseExact(model.StartLoadString, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            List<Company> listCompany = new List<Company>();

            var proyect = _proyectService.GetbyId(model.proyect);
            if (proyect.TypeProyect == BackEnd.Commons.Enums.TypeProyectEnum.Standard)
            {
                listCompany.AddRange(proyect.Companies);
            }

            if (proyect.TypeProyect == BackEnd.Commons.Enums.TypeProyectEnum.Mantenimiento)
            {
                listCompany.Add(_companyService.GetbyId(model.company[0]));
            }
            var taskType = _taskTypeService.GetbyId(model.TaskType);
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (CreateOrUpdateDayAndSetInstance(model, listCompany, proyect, taskType, user, model.Lunes, 0))
            {

                CreateOrUpdateDayAndSetInstance(model, listCompany, proyect, taskType,  user, model.Martes, 1);
                CreateOrUpdateDayAndSetInstance(model, listCompany, proyect, taskType,  user, model.Miercoles, 2);
                CreateOrUpdateDayAndSetInstance(model, listCompany, proyect, taskType,  user, model.Jueves, 3);
                CreateOrUpdateDayAndSetInstance(model, listCompany, proyect, taskType,  user, model.Viernes, 4);
                CreateOrUpdateDayAndSetInstance(model, listCompany, proyect, taskType,  user, model.Sabado, 5);
                CreateOrUpdateDayAndSetInstance(model, listCompany, proyect, taskType, user, model.Domingo, 6);
                return this.Json(
                 "true"
                  );
            }
            else
            {
                return this.Json(
                 "false"
                  );
            }

        }

        [HttpPost]
        public JsonResult deleteWeek(LoadWeekViewModel model)
        {
            model.StartLoad = DateTime.ParseExact(model.StartLoadString, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var user = UserManager.FindById(User.Identity.GetUserId());
            deleteWeek(model, user);
                return this.Json(
                 "true"
                  );
             

        }

        private void deleteWeek(LoadWeekViewModel model, ApplicationUser user)
        {
            var split = model.companyOld?.Split(',');
            List<Company> listOldCompany = new List<Company>();
            if (split != null && split.Length > 0)
            {
                foreach (var item in split)
                    listOldCompany.Add(_companyService.GetbyId(Int32.Parse(item)));
            }
            var proyectOldEntity = _proyectService.GetbyId(model.proyectOld);
            var taskTypeOldEntity = _taskTypeService.GetbyId(model.TaskTypeOld);


            for (int i = 0; i < 7; i++)
            {
            var firstDay = getDayByService(model, user,i);

            if (firstDay == null)
            {
                    Error("No se encontro el dia", "Error");
            }
            else
            {
                 FindAndDelete(listOldCompany, proyectOldEntity, taskTypeOldEntity,firstDay);
            }
            }
            
        }

        private void FindAndDelete(List<Company> listOldCompany, Proyect proyectOldEntity, TaskType taskTypeOldEntity, WorkDay firstDay)
        {
            try
            {
                var instanciaABorrar = firstDay.InstanceDay.FirstOrDefault(x => x.Companies.SequenceEqual(listOldCompany) & x.ProyectId == proyectOldEntity.Id & x.TaskTypeId == taskTypeOldEntity.Id);
                if (instanciaABorrar != null)
                {
                    if (firstDay.InstanceDay.Count == 1)

                        _workDayService.DeleteById(firstDay.Id);
                    else
                    {
                        _instanceDayService.DeleteById(instanciaABorrar.Id);

                        //_workDayService.Update(firstDay);
                    }
                }
            }
            catch (Exception)
            {

 
            }
           
        }

        private bool CreateOrUpdateDayAndSetInstance(LoadWeekViewModel model, List<Company> listCompany, Proyect proyect,TaskType taskType,  ApplicationUser user, decimal hours, int moreDay)
        {
             var firstDay = getDayByService(model, user, moreDay);

            if (firstDay == null)
            {
                var day = new WorkDay()
                {
                    Date = model.StartLoad.AddDays(moreDay),
                    UserProfileId = user.UserProfile.Id
                };

                day.InstanceDay.Add(new InstanceDay() { Hours = hours, ProyectId = proyect.Id, Companies = listCompany, TaskTypeId=taskType.Id });
                _workDayService.Add(day);
                return true;
            }
            else
            {
                return UpdateOrCreateNewInstance(model.companyOld, model.proyectOld, model.TaskTypeOld, hours, listCompany, proyect, taskType, firstDay);

            }
        }

        private WorkDay getDayByService(LoadWeekViewModel model, ApplicationUser user, int addDays)
        {
            return _workDayService.GetAll().FirstOrDefault(x => x.Date == model.StartLoad.AddDays(addDays) & x.UserProfileId == user.UserProfile.Id);
        }

        private bool UpdateOrCreateNewInstance(string companiesOld, long proyectOld ,long taskTypeOld, decimal horasDelDia, List<Company> listCompany, Proyect proyect,TaskType taskType,  WorkDay firstDay)
        {
            //  var exist = firstDay.InstanceDay.Where(y => y.Companies.Equals(listCompany) & y.ProyectId==model.proyect);
            var split = companiesOld?.Split(',');
            List<Company> listOldCompany = new List<Company>();
            if (split != null && split.Length > 0)
            {
                foreach (var item in split)
                    listOldCompany.Add(_companyService.GetbyId(Int32.Parse(item)));
            }
            var proyectOldEntity = _proyectService.GetbyId(proyectOld);
            var taskTypeOldEntity = _taskTypeService.GetbyId(taskTypeOld);



            if (proyectOldEntity == null || (proyectOldEntity.Id == proyect.Id && listOldCompany.SequenceEqual(listCompany) && taskTypeOldEntity.Id== taskType.Id))
            {
                var instanciaDelDia = firstDay.InstanceDay.FirstOrDefault(x => x.Companies.SequenceEqual(listCompany) & x.ProyectId == proyect.Id & x.TaskTypeId==taskType.Id);
                if (instanciaDelDia != null)
                {
                    if (proyectOldEntity == null)
                    {
                        return false;
                    }
                    else
                        firstDay.InstanceDay.Find(x => x.Id == instanciaDelDia.Id).Hours = horasDelDia;
                }
                else
                {
                    firstDay.InstanceDay.Add(new InstanceDay() { Hours = horasDelDia, ProyectId = proyect.Id, Companies = listCompany, TaskType=taskType });
                }
            }
            else
            {
                var instanciaDelNuevoDiaExiste = firstDay.InstanceDay.FirstOrDefault(x => x.Companies.SequenceEqual(listCompany) & x.ProyectId == proyect.Id & x.TaskTypeId == taskType.Id);
                if (instanciaDelNuevoDiaExiste == null)
                {
                    var instanciaDelDia = firstDay.InstanceDay.FirstOrDefault(x => x.Companies.SequenceEqual(listOldCompany) & x.ProyectId == proyectOldEntity.Id);
                    firstDay.InstanceDay.Find(x => x.Id == instanciaDelDia.Id).Hours = horasDelDia;
                    firstDay.InstanceDay.Find(x => x.Id == instanciaDelDia.Id).Companies = listCompany;
                    firstDay.InstanceDay.Find(x => x.Id == instanciaDelDia.Id).ProyectId = proyect.Id;
                    firstDay.InstanceDay.Find(x => x.Id == instanciaDelDia.Id).TaskTypeId = taskType.Id;
                }
                else
                {
                    return false;
                }

            }

            _workDayService.Update(firstDay);
            return true;
        }

        [HttpPost]
        public JsonResult GetWeekByUser(string firstDate)
        {
            DateTime dt = DateTime.ParseExact(firstDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var currentUser = User.Identity.GetUserId();
            var list = _workDayService.GetAll().Where(x => x.UserProfileId == UserManager.FindById(currentUser).UserProfile.Id & x.Date >= dt & x.Date <= dt.AddDays(6)).ToList();

            List<LoadWeekViewModel> listToReturn = new List<LoadWeekViewModel>();

            LoadWeekViewModel LoadWeek;
            for (int i = 0; list.Count > 0 && i < list[0].InstanceDay.Count; i++)
            {
                listToReturn.Add(LoadWeek = new LoadWeekViewModel()
                {
                    company = list[0].InstanceDay[i].Companies.Select(x=>x.Id).ToList(),
                    proyect = list[0].InstanceDay[i].ProyectId,
                    Lunes = list[0].InstanceDay[i].Hours,
                    Martes = list[1].InstanceDay[i].Hours,
                    Miercoles = list[2].InstanceDay[i].Hours,
                    Jueves = list[3].InstanceDay[i].Hours,
                    Viernes = list[4].InstanceDay[i].Hours,
                    Sabado = list[5].InstanceDay[i].Hours,
                    Domingo = list[6].InstanceDay[i].Hours,
                    NombreEmpresas = list[0].InstanceDay[i].Proyect.TypeProyect==BackEnd.Commons.Enums.TypeProyectEnum.TimeOff? "Todas":  string.Join(", ", list[0].InstanceDay[i].Companies.Select(x => x.Name)),
                    NombreProyecto = list[0].InstanceDay[i].Proyect.Name,
                    NombreTipoTarea = list[0].InstanceDay[i]?.TaskType?.Name,
                    TaskType = list[0].InstanceDay[i].TaskTypeId


                });


            }

            return this.Json(

                 listToReturn
                   , JsonRequestBehavior.AllowGet
                   );

        }

        // GET: Hours/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Hours/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Hours/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Hours/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Hours/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Hours/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Hours/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
