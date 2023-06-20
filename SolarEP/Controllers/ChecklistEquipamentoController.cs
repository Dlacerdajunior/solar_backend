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
    public class ChecklistEquipamentoController : Controller
    {
        private SolardbEntities db = new SolardbEntities();


        public bool verificaigual(string codigo)
        {
            var check = db.checklist_codigo.Where(u => u.codigo == codigo);

            if (check.Count() != 0)
            {
                return false;
            }
            return true;
        }

        // GET: /ChecklistEquipamento/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var checklist_codigo = db.checklist_codigo;

            int tamanhoPagina = 20;
            int numeroPagina = pagina ?? 1;

            ViewBag.qtd = checklist_codigo.Count();

            return View(checklist_codigo.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /ChecklistEquipamento/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            checklist_codigo checklist_codigo = db.checklist_codigo.Find(id);
            if (checklist_codigo == null)
            {
                return HttpNotFound();
            }
            return View(checklist_codigo);
        }

        // GET: /ChecklistEquipamento/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /ChecklistEquipamento/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,codigo")] checklist_codigo checklist_codigo)
        {
            if (ModelState.IsValid)
            {
                db.checklist_codigo.Add(checklist_codigo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(checklist_codigo);
        }

        // GET: /ChecklistEquipamento/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            checklist_codigo checklist_codigo = db.checklist_codigo.Find(id);
            if (checklist_codigo == null)
            {
                return HttpNotFound();
            }
            return View(checklist_codigo);
        }

        // POST: /ChecklistEquipamento/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,codigo")] checklist_codigo checklist_codigo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checklist_codigo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(checklist_codigo);
        }

        // GET: /ChecklistEquipamento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            checklist_codigo checklist_codigo = db.checklist_codigo.Find(id);
            if (checklist_codigo == null)
            {
                return HttpNotFound();
            }
            return View(checklist_codigo);
        }

        // POST: /ChecklistEquipamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            checklist_codigo checklist_codigo = db.checklist_codigo.Find(id);
            db.checklist_codigo.Remove(checklist_codigo);
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
