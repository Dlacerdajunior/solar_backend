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
    public class ConfiguracoesController : Controller
    {
        private SolardbEntities db = new SolardbEntities();

        // GET: /Configuracoes/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var configuracao = db.configuracao.Where(a => a.nome == "VisitaAdministrador");

            int tamanhoPagina = 20;
            int numeroPagina = pagina ?? 1;

            ViewBag.qtd = configuracao.Count();

            return View(configuracao.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Configuracoes/
        public ActionResult EmailAdm(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var configuracao = db.configuracao.Where(a => a.nome == "EmailMonitoramento");

            int tamanhoPagina = 20;
            int numeroPagina = pagina ?? 1;

            ViewBag.qtd = configuracao.Count();

            return View(configuracao.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Configuracoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            configuracao configuracao = db.configuracao.Find(id);
            if (configuracao == null)
            {
                return HttpNotFound();
            }
            return View(configuracao);
        }

        // GET: /Configuracoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Configuracoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,nome,valor")] configuracao configuracao)
        {
            if (ModelState.IsValid)
            {
                db.configuracao.Add(configuracao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(configuracao);
        }

        // GET: /Configuracoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            configuracao configuracao = db.configuracao.Find(id);
            if (configuracao == null)
            {
                return HttpNotFound();
            }
            return View(configuracao);
        }

        // POST: /Configuracoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,nome,valor")] configuracao configuracao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(configuracao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(configuracao);
        }



        // GET: /Configuracoes/Create
        public ActionResult CreateMonitoramento()
        {
            return View();
        }

        // POST: /Configuracoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMonitoramento([Bind(Include = "id,nome,valor,status")] configuracao configuracao)
        {
            if (ModelState.IsValid)
            {
                if (db.configuracao.Where(a => a.nome == "EmailMonitoramento").Count() == 0)
                {
                    configuracao.status = 1;
                }
                else
                {
                    configuracao.status = db.configuracao.Where(a => a.nome == "EmailMonitoramento").FirstOrDefault().status;
                }
                db.configuracao.Add(configuracao);
                db.SaveChanges();
                return RedirectToAction("EmailAdm");
            }

            return View(configuracao);
        }

        // GET: /Configuracoes/Edit/5
        public ActionResult EditMonitoramento(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            configuracao configuracao = db.configuracao.Find(id);
            if (configuracao == null)
            {
                return HttpNotFound();
            }
            return View(configuracao);
        }

        // POST: /Configuracoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMonitoramento([Bind(Include = "id,nome,valor,status")] configuracao configuracao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(configuracao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EmailAdm");
            }
            return View(configuracao);
        }

        // GET: /Configuracoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            configuracao configuracao = db.configuracao.Find(id);
            if (configuracao == null)
            {
                return HttpNotFound();
            }
            return View(configuracao);
        }

        // POST: /Configuracoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            configuracao configuracao = db.configuracao.Find(id);
            db.configuracao.Remove(configuracao);
            db.SaveChanges();

            return Redirect(Request.UrlReferrer.PathAndQuery);
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
