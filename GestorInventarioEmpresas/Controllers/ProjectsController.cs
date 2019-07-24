using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;

using System.Web.Mvc;
using GestorInventarioEmpresas.BackEnd.DAL;
using GestorInventarioEmpresas.BackEnd.Domain.Entities;
using Newtonsoft.Json;

namespace GestorInventarioEmpresas.Controllers
{

    public class ProjectsController : BaseController
    {
        private GestorInventarioEmpresasContext db = new GestorInventarioEmpresasContext();

        // GET: Proyects
        [Authorize(Roles = "Administrador")]
        public ActionResult Index()
        {

            return View(db.Proyects.ToList());
        }

        // GET: Proyects/Details/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyect proyect = db.Proyects.Find(id);
            if (proyect == null)
            {
                return HttpNotFound();
            }
            return View(proyect);
        }

        // GET: Proyects/Create
        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {

            return View();
        }

        // POST: Proyects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public ActionResult Create(Proyect proyect)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (db.Proyects.ToList().Find(x => x.Code == proyect.Code & x.Id != proyect.Id) != null)
                    {
                        Error("El código ya esta siendo utilizado para otro proyecto", "Cuidado");
                        return Json("Error");
                    };
                    List<Company> aux = new List<Company>();
                    if (proyect.Companies != null)
                    {
                        foreach (var item in proyect.Companies)
                        {
                            aux.Add(db.Companies.ToList().Find(x => x.Id == item.Id));
                        }
                    }

                    proyect.Companies = aux;

                    db.Proyects.Add(proyect);
                    db.SaveChanges();
                    Success("se ha creado correctamente", proyect.Name);
                    return Json("Exito");
                }
            }
            catch (Exception ex)
            {
                Error("Existen códigos duplicados", "Error");
                return Json("Existen códigos duplicados");
            }
            return Json("Exito");
        }

        // GET: Proyects/Edit/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyect proyect = db.Proyects.Find(id);
            if (proyect == null)
            {
                return HttpNotFound();
            }
            return View(proyect);
        }

        // POST: Proyects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(Proyect proyect)
        {
            if (ModelState.IsValid)
            {
                var proyectToInsert = db.Proyects.ToList().Find(x => x.Id == proyect.Id);
                var proyectToInsertCode = db.Proyects.ToList().Find(x => x.Code == proyect.Code & x.Id != proyect.Id);
                if (proyectToInsertCode != null && proyectToInsert != proyectToInsertCode)
                {
                    Error("El código ya esta siendo utilizado para otro proyecto", "Cuidado");
                    return Json("Error");
                }
                proyectToInsert.Name = proyect.Name;
                proyectToInsert.Code = proyect.Code;
                proyectToInsert.TypeProyect = proyect.TypeProyect;
              //  proyectToInsert.Location = proyect.Location;
                proyectToInsert.isActive = proyect.isActive;
                proyectToInsert.billable = proyect.billable;
                proyectToInsert.Companies.Clear();
                if (proyect.Companies != null)
                {
                    foreach (var item in proyect.Companies)
                    {
                        proyectToInsert.Companies.Add(db.Companies.ToList().Find(x => x.Id == item.Id));
                    }
                }

                db.Entry(proyectToInsert).State = EntityState.Modified;
                db.SaveChanges();
                Success("se ha actualizado correctamente", proyectToInsert.Name);
            }
            return RedirectToAction("Index");
        }

        // GET: Proyects/Delete/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyect proyect = db.Proyects.Find(id);
            if (proyect == null)
            {
                return HttpNotFound();
            }
            return View(proyect);
        }

        // POST: Proyects/Delete/5
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public JsonResult DeleteConfirmed(long id)
        {
            Proyect proyect = db.Proyects.Find(id);
            db.Proyects.Remove(proyect);
            db.SaveChanges();
            Success("Fue eliminado con exito", proyect.Name);
            return Json(new { success = true, responseText = "Eliminado." }, JsonRequestBehavior.AllowGet);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public JsonResult getAllCompanies()
        {

            return this.Json(
                   (from obj in db.Companies.OrderBy(x => x.Name) select new { Id = obj.Id, Name = obj.Name, Code = obj.Code })
                   , JsonRequestBehavior.AllowGet
                   );

        }
        public JsonResult getAllProyects() 
        {
            return this.Json(
                  (from obj in db.Proyects.Where(y=>y.isActive).OrderBy(x => x.Name) select new { Id = obj.Id, Name = obj.Name, Code = obj.Code })
                  , JsonRequestBehavior.AllowGet
                  );
        }
        public JsonResult getAllCompaniesForProyect(long? id)
        {
            var data = db.Proyects.Find(id);

            return this.Json(
                (from obj in data.Companies.OrderBy(x => x.Name) select new { Id = obj.Id, Name = obj.Name, Code = obj.Code })
                , JsonRequestBehavior.AllowGet
                );
        }
        public JsonResult getAllCompaniesForCompany(long? id)
        {
            var data = db.Proyects.Find(id);
            if (data.TypeProyect == BackEnd.Commons.Enums.TypeProyectEnum.TimeOff)
            {
                return this.Json(
                    (new { empresas = "" })
                    , JsonRequestBehavior.AllowGet
                    );

            }
            else if (data.TypeProyect == BackEnd.Commons.Enums.TypeProyectEnum.Mantenimiento)
            {
                return this.Json(
                  (from obj in data.Companies select new { Id = obj.Id, Name = obj.Name, Code = obj.Code })
                  , JsonRequestBehavior.AllowGet
                  );


            }
            else
            {
                string joined = String.Join(",", data.Companies.Select(x => x.Name));
                return this.Json(
                  (new { empresas = joined })
                  , JsonRequestBehavior.AllowGet
                  );
            }
        }
        public JsonResult getAllTypeTasks(long id)
        {
            var data = new List<TaskType>();
            var dataProyect = db.Proyects.Find(id);
            var x1 = db.TaskTypes.ToList();
            if (dataProyect.TypeProyect == BackEnd.Commons.Enums.TypeProyectEnum.TimeOff)
            {
                data.AddRange(db.TaskTypes.Where(x => x.TimeOff).ToList());
            }
            if (dataProyect.TypeProyect == BackEnd.Commons.Enums.TypeProyectEnum.Standard)
            {

                data.AddRange(db.TaskTypes.Where(x => x.Standard).ToList());
            }
            if (dataProyect.TypeProyect == BackEnd.Commons.Enums.TypeProyectEnum.Mantenimiento)
            {
                data.AddRange(db.TaskTypes.Where(x => x.Mantenimiento).ToList());
            }
            if (dataProyect.TypeProyect == BackEnd.Commons.Enums.TypeProyectEnum.BackOffice)
            {
                data.AddRange(db.TaskTypes.Where(x => x.BackOffice).ToList());
            }
            var d = data.OrderBy(x => x.Name);
            return this.Json(
                   (from obj in d.Distinct().ToList() select new { Id = obj.Id, Name = obj.Name, Code = obj.Code })
                   , JsonRequestBehavior.AllowGet
                   );
        }

    }
}
