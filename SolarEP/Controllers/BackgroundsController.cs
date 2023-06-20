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
using System.IO;

namespace SolarEP.Controllers
{
    public class BackgroundsController : Controller
    {
        private SolardbEntities db = new SolardbEntities();

        // GET: /Backgrounds/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var backgrounds = db.backgrounds;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            ViewBag.qtd = backgrounds.Count();

            return View(backgrounds.OrderByDescending(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Backgrounds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            backgrounds backgrounds = db.backgrounds.Find(id);
            if (backgrounds == null)
            {
                return HttpNotFound();
            }
            return View(backgrounds);
        }

        // GET: /Backgrounds/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Backgrounds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,url_background")] backgrounds backgrounds, HttpPostedFileBase urlback)
        {
            var fileName = "";

            if (ModelState.IsValid)
            {

                var finalString = GeraCodigo();

                if (urlback != null && urlback.ContentLength > 0)
                {
                    fileName = Path.GetFileName(finalString + urlback.FileName).ToString().Replace(" ", "");
                    fileName = removerAcentos(fileName);
                    fileName = removerSpecial(fileName);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/Backgrounds"), fileName);
                    urlback.SaveAs(path);
                }

                if (fileName == "")
                {
                    backgrounds.url_background = null;
                }
                else
                {
                    backgrounds.url_background = "http://solarcoke.solarbr.com.br:32014/Content/assets/Backgrounds/" + fileName;
                }

                db.backgrounds.Add(backgrounds);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(backgrounds);
        }


        public static string removerAcentos(string texto)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }
            return texto;
        }

        public static string removerSpecial(string texto)
        {
            string comAcentos = "[!;\\/:*?\"<>|&']";

            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), "");
            }
            return texto;
        }

        //maquina de codigos para lojas
        public String GeraCodigo()
        {
            var chars = "0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            var confere = db.lojas.Where(a => a.email_codigo == finalString);
            if (confere.Count() != 0)
            {
                var finalString2 = GeraCodigo();
                return finalString2;
            }
            else
            {
                return finalString;
            }
        }

        // GET: /Backgrounds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            backgrounds backgrounds = db.backgrounds.Find(id);
            if (backgrounds == null)
            {
                return HttpNotFound();
            }
            return View(backgrounds);
        }

        // POST: /Backgrounds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            backgrounds backgrounds = db.backgrounds.Find(id);
            db.backgrounds.Remove(backgrounds);
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
