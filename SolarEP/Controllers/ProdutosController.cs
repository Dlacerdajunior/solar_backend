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
    public class ProdutosController : Controller
    {
        private SolardbEntities db = new SolardbEntities();

        // GET: /Produtos/
        public ActionResult Index()
        {
            var produtos = db.produtos.Include(p => p.categoria_produto);
            return View(produtos.ToList());
        }


        // GET: /Produtos/
        public ActionResult Index(int? pagina)
        {
            var produtos = db.produtos.Include(t => t.categoria_produto);

            int tamanhoPagina = 5;
            int numeroPagina = pagina ?? 1;

            return View(produtos.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Produtos/Refrigerantes
        public ActionResult Refrigerantes(int? pagina)
        {
            var produtos = db.produtos.Include(t => t.categoria_produto).Where(a => a.categoria_id == 1);

            int tamanhoPagina = 5;
            int numeroPagina = pagina ?? 1;

            return View(produtos.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Produtos/Sucos
        public ActionResult Sucos(int? pagina)
        {
            var produtos = db.produtos.Include(t => t.categoria_produto).Where(a => a.categoria_id == 2);

            int tamanhoPagina = 5;
            int numeroPagina = pagina ?? 1;

            return View(produtos.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Produtos/Chas
        public ActionResult Chas(int? pagina)
        {
            var produtos = db.produtos.Include(t => t.categoria_produto).Where(a => a.categoria_id == 3);

            int tamanhoPagina = 5;
            int numeroPagina = pagina ?? 1;

            return View(produtos.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Produtos/Aguas
        public ActionResult Aguas(int? pagina)
        {
            var produtos = db.produtos.Include(t => t.categoria_produto).Where(a => a.categoria_id == 4);

            int tamanhoPagina = 5;
            int numeroPagina = pagina ?? 1;

            return View(produtos.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Produtos/Infantis
        public ActionResult Infantis(int? pagina)
        {
            var produtos = db.produtos.Include(t => t.categoria_produto).Where(a => a.categoria_id == 5);

            int tamanhoPagina = 5;
            int numeroPagina = pagina ?? 1;

            return View(produtos.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Produtos/Energeticos
        public ActionResult Energeticos(int? pagina)
        {
            var produtos = db.produtos.Include(t => t.categoria_produto).Where(a => a.categoria_id == 6);

            int tamanhoPagina = 5;
            int numeroPagina = pagina ?? 1;

            return View(produtos.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Produtos/Cervejas
        public ActionResult Cervejas(int? pagina)
        {
            var produtos = db.produtos.Include(t => t.categoria_produto).Where(a => a.categoria_id == 7);

            int tamanhoPagina = 5;
            int numeroPagina = pagina ?? 1;

            return View(produtos.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Produtos/Create
        public ActionResult Create()
        {
            ViewBag.categoria_id = new SelectList(db.categoria_produto, "id", "categoria_nome");
            return View();
        }

        // POST: /Produtos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,nome,preco,url_foto,categoria_id,codigo")] produtos produtos)
        {
            if (ModelState.IsValid)
            {
                db.produtos.Add(produtos);
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.PathAndQuery);
            //    return RedirectToAction("Index");
            }

            ViewBag.categoria_id = new SelectList(db.categoria_produto, "id", "categoria_nome", produtos.categoria_id);
            return View(produtos);
        }

        // GET: /Produtos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            produtos produtos = db.produtos.Find(id);
            if (produtos == null)
            {
                return HttpNotFound();
            }
            ViewBag.categoria_id = new SelectList(db.categoria_produto, "id", "categoria_nome", produtos.categoria_id);
            return View(produtos);
        }

        // POST: /Produtos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,nome,preco,url_foto,categoria_id,codigo")] produtos produtos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(produtos).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.PathAndQuery);
              //  return RedirectToAction("Index");
            }
            ViewBag.categoria_id = new SelectList(db.categoria_produto, "id", "categoria_nome", produtos.categoria_id);
            return View(produtos);
        }

        // GET: /Produtos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            produtos produtos = db.produtos.Find(id);
            if (produtos == null)
            {
                return HttpNotFound();
            }
            return View(produtos);
        }

        // POST: /Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            produtos produtos = db.produtos.Find(id);
            db.produtos.Remove(produtos);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.PathAndQuery);
        //    return RedirectToAction("Index");
        }

        //JsonMethod
        public ActionResult JsonListaProdutos()
        {
            var produtos = db.produtos;
            var listaprodutos = new List<dynamic>();

            foreach (var item in produtos)
            {
                listaprodutos.Add(new
                {
                    id = item.id,
                    nome = item.nome,
                    preco = item.preco,
                    url_foto = item.url_foto,
                    categoria_id = item.categoria_id,
                    codigo = item.codigo,

                });
            }

            return Json(listaprodutos, JsonRequestBehavior.AllowGet);

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
