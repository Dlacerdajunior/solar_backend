using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SolarEP.Models;
using PagedList;

namespace SolarEP.Controllers
{
    public class GruposFAQController : Controller
    {
        private SolardbEntities db = new SolardbEntities();

        // GET: /GruposFAQ/
        public ActionResult Index(int? pagina)
        {

            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var grupos_faq = db.grupos_faq;
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            ViewBag.qtd = grupos_faq.Count();

            return View(grupos_faq.OrderByDescending(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /GruposFAQ/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            grupos_faq grupos_faq = db.grupos_faq.Find(id);
            if (grupos_faq == null)
            {
                return HttpNotFound();
            }
            return View(grupos_faq);
        }

        // GET: /GruposFAQ/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /GruposFAQ/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,grupo_faq")] grupos_faq grupos_faq)
        {
            if (ModelState.IsValid)
            {
                db.grupos_faq.Add(grupos_faq);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(grupos_faq);
        }

        // GET: /GruposFAQ/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            grupos_faq grupos_faq = db.grupos_faq.Find(id);
            if (grupos_faq == null)
            {
                return HttpNotFound();
            }
            return View(grupos_faq);
        }

        // POST: /GruposFAQ/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,grupo_faq")] grupos_faq grupos_faq)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grupos_faq).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grupos_faq);
        }

        // GET: /GruposFAQ/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            grupos_faq grupos_faq = db.grupos_faq.Find(id);
            if (grupos_faq == null)
            {
                return HttpNotFound();
            }
            return View(grupos_faq);
        }

        // POST: /GruposFAQ/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var faq = db.faq.Where(a => a.faq_grupo == id);
            db.faq.RemoveRange(faq);

            grupos_faq grupos_faq = db.grupos_faq.Find(id);
            db.grupos_faq.Remove(grupos_faq);
            db.SaveChanges();
            return RedirectToAction("Index");
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
