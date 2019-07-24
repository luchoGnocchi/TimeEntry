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

namespace GestorInventarioEmpresas.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class TaskTypesController : BaseController
    {
        private GestorInventarioEmpresasContext db = new GestorInventarioEmpresasContext();

        // GET: TaskTypes
        public ActionResult Index()
        {
        
            return View(db.TaskTypes.Where(x=>!TypeTaskReservados.Contains(x.Code)));
        }

        // GET: TaskTypes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskType taskType = db.TaskTypes.Find(id);
            if (taskType == null)
            {
                return HttpNotFound();
            }
            return View(taskType);
        }

        // GET: TaskTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TaskTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(  TaskType taskType)
        {
            try
            {
               if (ModelState.IsValid)
                {
                    db.TaskTypes.Add(taskType);
                    db.SaveChanges();
                    Success("Tipo de tarea creado con exito", "Exito");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Error("Existen códigos duplicados", "Error");
                return View(taskType);
            }
            return RedirectToAction("Index");
        }

        // GET: TaskTypes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskType taskType = db.TaskTypes.Find(id);
            if (taskType == null)
            {
                return HttpNotFound();
            }
            return View(taskType);
        }

        // POST: TaskTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(  TaskType taskType)
        {
            try { 
            if (ModelState.IsValid)
            {
                db.Entry(taskType).State = EntityState.Modified;
                db.SaveChanges();
                Success("Tipo de tarea modificado con exito", "Exito");
                return RedirectToAction("Index");
            }
        }
            catch (Exception ex)
            {
                Error("Existen códigos duplicados", "Error");
                return View(taskType);
    }
            return RedirectToAction("Index");
}

// GET: TaskTypes/Delete/5
public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskType taskType = db.TaskTypes.Find(id);
            if (taskType == null)
            {
                return HttpNotFound();
            }
            return View(taskType);
        }

        // POST: TaskTypes/Delete/5
        [HttpGet]
        public JsonResult DeleteConfirmed(long id)
        {
            TaskType taskType = db.TaskTypes.Find(id);
            db.TaskTypes.Remove(taskType);
            db.SaveChanges();
            return Json(new { success = false, responseText = "Eliminado" }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
