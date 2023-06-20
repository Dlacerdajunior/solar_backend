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
    public class AjudaController : Controller
    {
        private SolardbEntities db = new SolardbEntities();

        // GET: /Ajuda/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var ajuda = db.ajuda;

            int tamanhoPagina = 20;
            int numeroPagina = pagina ?? 1;

            ViewBag.qtd = ajuda.Count();

            return View(ajuda.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Ajuda/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ajuda ajuda = db.ajuda.Find(id);
            if (ajuda == null)
            {
                return HttpNotFound();
            }
            return View(ajuda);
        }

        // GET: /Ajuda/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Ajuda/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,titulo,descricao,url")] ajuda ajuda)
        {
            if (ModelState.IsValid)
            {
                db.ajuda.Add(ajuda);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ajuda);
        }

        // GET: /Ajuda/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ajuda ajuda = db.ajuda.Find(id);
            if (ajuda == null)
            {
                return HttpNotFound();
            }
            return View(ajuda);
        }

        // POST: /Ajuda/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,titulo,descricao,url")] ajuda ajuda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ajuda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ajuda);
        }

        // GET: /Ajuda/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ajuda ajuda = db.ajuda.Find(id);
            if (ajuda == null)
            {
                return HttpNotFound();
            }
            return View(ajuda);
        }

        // POST: /Ajuda/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ajuda ajuda = db.ajuda.Find(id);
            db.ajuda.Remove(ajuda);
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
