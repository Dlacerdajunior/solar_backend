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
    public class ChecklistController : Controller
    {
        private SolardbEntities db = new SolardbEntities();

        // GET: /Checklist/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var checklist_equipamento = db.checklist_equipamento;

            int tamanhoPagina = 20;
            int numeroPagina = pagina ?? 1;

            ViewBag.qtd = checklist_equipamento.Count();

            return View(checklist_equipamento.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Checklist/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            checklist_equipamento checklist_equipamento = db.checklist_equipamento.Find(id);
            if (checklist_equipamento == null)
            {
                return HttpNotFound();
            }
            return View(checklist_equipamento);
        }

        // GET: /Checklist/Create
        public ActionResult Create()
        {
            ViewBag.codigo = new SelectList(db.checklist_codigo, "codigo", "codigo");
            return View();
        }

        // POST: /Checklist/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,codigo,texto")] checklist_equipamento checklist_equipamento)
        {
            if (ModelState.IsValid)
            {
                db.checklist_equipamento.Add(checklist_equipamento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.codigo = new SelectList(db.checklist_codigo, "codigo", "codigo", checklist_equipamento.codigo);
            return View(checklist_equipamento);
        }

        // GET: /Checklist/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            checklist_equipamento checklist_equipamento = db.checklist_equipamento.Find(id);
            if (checklist_equipamento == null)
            {
                return HttpNotFound();
            }
            ViewBag.codigo = new SelectList(db.checklist_codigo, "codigo", "codigo", checklist_equipamento.codigo);
            return View(checklist_equipamento);
        }

        // POST: /Checklist/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,codigo,texto")] checklist_equipamento checklist_equipamento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checklist_equipamento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.codigo = new SelectList(db.checklist_codigo, "codigo", "codigo", checklist_equipamento.codigo);
            return View(checklist_equipamento);
        }

        // GET: /Checklist/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            checklist_equipamento checklist_equipamento = db.checklist_equipamento.Find(id);
            if (checklist_equipamento == null)
            {
                return HttpNotFound();
            }
            return View(checklist_equipamento);
        }

        // POST: /Checklist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            checklist_equipamento checklist_equipamento = db.checklist_equipamento.Find(id);
            db.checklist_equipamento.Remove(checklist_equipamento);
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
