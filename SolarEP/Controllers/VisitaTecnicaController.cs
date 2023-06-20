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
    public class VisitaTecnicaController : Controller
    {
        private SolardbEntities db = new SolardbEntities();

        // GET: /VisitaTecnica/
        public ActionResult Index(int? pagina, string titulo, string contato, string loja, string data, string status)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            /*
             *
             *
             */
            
            int tamanhoPagina = 35;
            
            int numeroPagina = pagina ?? 1;

            var sql = "select * from visita inner join lojas on visita.id_loja = lojas.id where 1=1";
            if (titulo != null && titulo.Trim() != "")
            {
                sql = sql + " and visita.titulo like '%" + titulo + "%'";
            }
            if (contato != null && contato.Trim() != "")
            {
                sql = sql + " and visita.contato like '%" + contato + "%'";
            }
            if (loja != null && loja.Trim() != "")
            {
                sql = sql + " and lojas.razao_social_loja like '%" + loja + "%'";
            }
            if (status != null && status.Trim() != "")
            {
                sql = sql + " and visita.id_status = " + status + "";
            }
            if (data != null && data.Trim() != "")
            {
                string[] arrDate = data.Split('/');

                string day = arrDate[0].ToString();
                string month = arrDate[1].ToString();
                string year = arrDate[2].ToString();

                sql = sql + " and visita.data like '%" + month + "/" + day + "/" + year + "%'";
            }

            var visita = db.visita.SqlQuery(sql).ToList();
            ViewBag.qtd = visita.Count();
            return View(visita.OrderByDescending(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /VisitaTecnica/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            visita visita = db.visita.Find(id);
            if (visita == null)
            {
                return HttpNotFound();
            }
            return View(visita);
        }

        // GET: /VisitaTecnica/Create
        public ActionResult Create()
        {
            ViewBag.id_loja = new SelectList(db.lojas, "id", "razao_social_loja");
            ViewBag.id_status = new SelectList(db.visita_status, "id", "nome");
            return View();
        }

        // POST: /VisitaTecnica/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_loja,id_status,titulo,descricao,contato,data,id_equipamento")] visita visita)
        {
            if (ModelState.IsValid)
            {
                db.visita.Add(visita);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_loja = new SelectList(db.lojas, "id", "razao_social_loja", visita.id_loja);
            ViewBag.id_status = new SelectList(db.visita_status, "id", "nome", visita.id_status);
            return View(visita);
        }

        // GET: /VisitaTecnica/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            visita visita = db.visita.Find(id);
            if (visita == null)
            {
                return HttpNotFound();
            }

            var rschamado = db.visita_chamado.Where(s => s.id_visita == id);
            var rschamadocount = db.visita_chamado.Where(s => s.id_visita == id).Count();

            if (rschamadocount >= 1)
            {
                ViewBag.rschamadomostrar = rschamado;
                ViewBag.rschamadomostrarcount = 1;
            }
            else
            {
                ViewBag.fotosmanutencaomostrar = 0;
            }


            //db.manutencao_instalacao
            var rsmanutencaofoto = db.visita_fotos.Where(s => s.id_visita == id);
            var rsmanutencaofotocount = db.visita_fotos.Where(s => s.id_visita == id).Count();

            if (rsmanutencaofotocount >= 1)
            {
                ViewBag.fotosmanutencao = rsmanutencaofoto;
                ViewBag.fotosmanutencaomostrar = 1;
            }
            else
            {
                ViewBag.fotosmanutencaomostrar = 0;
            }     

            ViewBag.id_loja = new SelectList(db.lojas, "id", "razao_social_loja", visita.id_loja);
            ViewBag.id_status = new SelectList(db.visita_status, "id", "nome", visita.id_status);
            return View(visita);
        }

        // POST: /VisitaTecnica/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id, id_loja, id_status, titulo, descricao, contato, data, id_equipamento, obs_admin, EQFNR, TYPBZ, EQUNR, HERST, atualiza, previsao")] visita visita, string chamado)
        {
            if (ModelState.IsValid)
            {
                visita.atualiza = DateTime.Now.ToString();
                db.Entry(visita).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            ViewBag.id_loja = new SelectList(db.lojas, "id", "razao_social_loja", visita.id_loja);
            ViewBag.id_status = new SelectList(db.visita_status, "id", "nome", visita.id_status);
            return View(visita);
        }

        // GET: /VisitaTecnica/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            visita visita = db.visita.Find(id);
            if (visita == null)
            {
                return HttpNotFound();
            }
            return View(visita);
        }

        // POST: /VisitaTecnica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            visita visita = db.visita.Find(id);
            db.visita.Remove(visita);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public ActionResult adicionarchamado([Bind(Include = "id_visita,chamado")] visita_chamado visita_chamado)
        {
            visita_chamado.data = DateTime.Now;
            db.visita_chamado.Add(visita_chamado);
            db.SaveChanges();
            
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        // GET: /Deletarchamado/5
        public ActionResult Deletechamado(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            visita_chamado visita_chamado = db.visita_chamado.Find(id);
            if (visita_chamado == null)
            {
                return HttpNotFound();
            }
            return View(visita_chamado);
        }

        // POST: /Deletarchamado/5
        [HttpPost, ActionName("Deletechamado")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletechamadoConfirmed(int id)
        {
            visita_chamado visita_chamado = db.visita_chamado.Find(id);
            db.visita_chamado.Remove(visita_chamado);
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
