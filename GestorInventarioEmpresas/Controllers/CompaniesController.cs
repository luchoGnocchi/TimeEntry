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
   
    public class CompaniesController : BaseController
    {
        private GestorInventarioEmpresasContext db = new GestorInventarioEmpresasContext();

        // GET: Companies
        public ActionResult Index()
        {
            return View(db.Companies.ToList());
        }

        // GET: Companies/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Code, Location")] Company company)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Companies.Add(company);
                    db.SaveChanges();
                    Success("Empresa creada con exito", "Exito");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Error("Existen códigos duplicados", "Error");
                return View(company);
            }
            return RedirectToAction("Index");
             
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Code, Location")] Company company)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(company).State = EntityState.Modified;
                    db.SaveChanges();
                    Success("Empresa modificada con exito", "Exito");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Error("Existen códigos duplicados", "Error");
                return View(company);
            }
            return RedirectToAction("Index");
           
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpGet]
        public JsonResult DeleteConfirmed(long id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
            db.SaveChanges();
            return Json(new { success = false, responseText = "The attached file is not supported." }, JsonRequestBehavior.AllowGet);
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
