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
    public class PedidosController : Controller
    {
        private SolardbEntities db = new SolardbEntities();

        // GET: /Pedidos/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var pedido = db.pedido.Include(p => p.status_pedido).Include(p => p.lojas);
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            ViewBag.qtd = pedido.Count();

            return View(pedido.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Pedidos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pedido pedido = db.pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // GET: /Pedidos/Create
        public ActionResult Create()
        {
            ViewBag.id_status = new SelectList(db.status_pedido, "id", "status_nome");
            ViewBag.id_loja = new SelectList(db.lojas, "id", "razao_social_loja");
            return View();
        }

        // POST: /Pedidos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,nome,descricao,id_loja,id_status")] pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.pedido.Add(pedido);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_status = new SelectList(db.status_pedido, "id", "status_nome", pedido.id_status);
            ViewBag.id_loja = new SelectList(db.lojas, "id", "razao_social_loja", pedido.id_loja);
            return View(pedido);
        }

        // GET: /Pedidos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pedido pedido = db.pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_status = new SelectList(db.status_pedido, "id", "status_nome", pedido.id_status);
            ViewBag.id_loja = new SelectList(db.lojas, "id", "razao_social_loja", pedido.id_loja);
            return View(pedido);
        }

        // POST: /Pedidos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,nome,descricao,id_loja,id_status")] pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_status = new SelectList(db.status_pedido, "id", "status_nome", pedido.id_status);
            ViewBag.id_loja = new SelectList(db.lojas, "id", "razao_social_loja", pedido.id_loja);
            return View(pedido);
        }

        // GET: /Pedidos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pedido pedido = db.pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: /Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pedido pedido = db.pedido.Find(id);
            db.pedido.Remove(pedido);
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
