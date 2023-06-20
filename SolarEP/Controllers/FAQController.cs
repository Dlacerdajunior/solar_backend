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
    public class FAQController : Controller
    {
        private SolardbEntities db = new SolardbEntities();

        // GET: /FAQ/
        public ActionResult Index(int? pagina, string perguntas, string respostas, string faq_grupo, string atualizacao)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            ViewBag.faq_grupo = new SelectList(db.grupos_faq, "id", "grupo_faq");

            var sql = "select * from faq where 1=1";
            if (perguntas != null && perguntas.Trim() != "")
            {
                sql = sql + " and faq.faq_pergunta like '%" + perguntas + "%'";
            }
            if (respostas != null && respostas.Trim() != "")
            {
                sql = sql + " and faq.faq_resposta like '%" + respostas + "%'";
            }
            if (faq_grupo != null && faq_grupo.Trim() != "")
            {
                sql = sql + " and faq.faq_grupo = '" + faq_grupo + "'";
            }
            if (atualizacao != null && atualizacao.Trim() != "")
            {
                string[] arrDate = atualizacao.Split('/');

                string day = arrDate[0].ToString();
                string month = arrDate[1].ToString();
                string year = arrDate[2].ToString();

                sql = sql + " and faq.ultima_atualizacao like '%" + year + "-" + month + "-" + day + "%'";
            }

            var faq = db.faq.SqlQuery(sql).ToList();
            ViewBag.qtd = faq.Count();

            return View(faq.OrderByDescending(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /FAQ/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            faq faq = db.faq.Find(id);
            if (faq == null)
            {
                return HttpNotFound();
            }
            return View(faq);
        }

        // GET: /FAQ/Create
        public ActionResult Create()
        {
            ViewBag.faq_grupo = new SelectList(db.grupos_faq, "id", "grupo_faq");
            return View();
        }

        // POST: /FAQ/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,faq_grupo,faq_pergunta,faq_resposta,ultima_atualizacao,color")] faq faq)
        {
            if (ModelState.IsValid)
            {
                faq.ultima_atualizacao = DateTime.Now;
                db.faq.Add(faq);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.faq_grupo = new SelectList(db.grupos_faq, "id", "grupo_faq", faq.faq_grupo);
            return View(faq);
        }

        // GET: /FAQ/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            faq faq = db.faq.Find(id);
            if (faq == null)
            {
                return HttpNotFound();
            }
            ViewBag.faq_grupo = new SelectList(db.grupos_faq, "id", "grupo_faq", faq.faq_grupo);
            return View(faq);
        }

        // POST: /FAQ/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,faq_grupo,faq_pergunta,faq_resposta,ultima_atualizacao,color")] faq faq)
        {
            if (ModelState.IsValid)
            {
                faq.ultima_atualizacao = DateTime.Now;
                db.Entry(faq).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.faq_grupo = new SelectList(db.grupos_faq, "id", "grupo_faq", faq.faq_grupo);
            return View(faq);
        }

        // GET: /FAQ/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            faq faq = db.faq.Find(id);
            if (faq == null)
            {
                return HttpNotFound();
            }
            return View(faq);
        }

        // POST: /FAQ/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            faq faq = db.faq.Find(id);
            db.faq.Remove(faq);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //JsonMethod
        public ActionResult JsonListaFAQ()
        {
            var faq = db.faq;
            var listafaq = new List<dynamic>();

            foreach (var item in faq)
            {
                listafaq.Add(new
                {
                    id = item.id,
                    faq_grupo = item.faq_grupo,
                    faq_pergunta = item.faq_pergunta,
                    faq_resposta = item.faq_resposta,
                    ultima_atualizacao = item.ultima_atualizacao,
                });
            }

            return Json(listafaq, JsonRequestBehavior.AllowGet);

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
