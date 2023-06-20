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
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace SolarEP.Controllers
{
    public class AdministradorController : Controller
    {
        private SolardbEntities db = new SolardbEntities();
        

        // GET: /Administrador/
        public ActionResult Index(int? pagina, string email, string nome)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            
            int tamanhoPagina = 20;
            int numeroPagina = pagina ?? 1;

            var sql = "select * from administrador where 1=1";
            if (email != null && email.Trim() != "")
            {
                sql = sql + " and administrador.email like '%" + email + "%'";
            }
            if (nome != null && nome.Trim() != "")
            {
                sql = sql + " and administrador.nome like '%" + nome + "%'";
            }

            var administrador = db.administrador.SqlQuery(sql).ToList();
            ViewBag.qtd = administrador.Count();
            return View(administrador.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));          
        }

        public bool verificaigual(string email)
        {
            var check = db.administrador.Where(u => u.email == email);

            if (check.Count() != 0)
            {
                return false;
            }
            return true;
        }

        public bool verificaigualedit(string email, int id)
        {
            var check = db.administrador.Where(u => u.email == email).FirstOrDefault();

            if (check != null && check.id != id)
            {
                return false;
            }
            return true;
        }

        // GET: /Administrador/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            administrador administrador = db.administrador.Find(id);
            if (administrador == null)
            {
                return HttpNotFound();
            }
            return View(administrador);
        }

        // GET: /Administrador/Create
        public ActionResult Create()
        {
            ViewBag.adm_tipo = new SelectList(db.adm_tipo, "id", "nome");
            return View();
        }

        // POST: /Administrador/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,codigo,nome,login,email,senha,mandante,email_codigo,tipo_usuario,adm_tipo")] administrador administrador)
        {
            if (ModelState.IsValid)
            {

                var finalString = GeraCodigo();
                var md5 = MD5Hash(administrador.senha);
                administrador.senha = md5;
                administrador.email_codigo = finalString;
                db.administrador.Add(administrador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.adm_tipo = new SelectList(db.adm_tipo, "id", "nome", administrador.adm_tipo);
            return View(administrador);
        }

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        //gerador maquina de codigos
        public  String GeraCodigo()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            var confere = db.administrador.Where(a => a.email_codigo == finalString);
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

        // GET: /Administrador/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            administrador administrador = db.administrador.Find(id);
            if (administrador == null)
            {
                return HttpNotFound();
            }
            ViewBag.adm_tipo = new SelectList(db.adm_tipo, "id", "nome", administrador.adm_tipo);
            return View(administrador);
        }

        // POST: /Administrador/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,codigo,nome,login,email,senha,mandante,email_codigo,tipo_usuario,adm_tipo")] administrador administrador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(administrador).State = EntityState.Modified;

                var finalString = GeraCodigo();
                var md5 = MD5Hash(administrador.senha);
                administrador.senha = md5;

                if (administrador.email_codigo == "" || administrador.email_codigo == null)
                {
                    administrador.email_codigo = finalString;                    
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.adm_tipo = new SelectList(db.adm_tipo, "id", "nome", administrador.adm_tipo);
            return View(administrador);
        }

        // GET: /Administrador/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            administrador administrador = db.administrador.Find(id);
            if (administrador == null)
            {
                return HttpNotFound();
            }
            return View(administrador);
        }

        // POST: /Administrador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            administrador administrador = db.administrador.Find(id);
            db.administrador.Remove(administrador);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //JsonMethod
        public ActionResult JsonListaAdministrador()
        {
            var adm = db.administrador;
            var listaadm = new List<dynamic>();

            foreach (var item in adm)
            {
                listaadm.Add(new
                {
                    id = item.id,
                    codigo = item.codigo,
                    nome = item.nome,
                    login = item.login,
                    email = item.email,
                    senha = item.senha,                    
                });
            }

            return Json(listaadm, JsonRequestBehavior.AllowGet);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

            public static string base64Decode(string sData) //Decode    
            {
                try
                {
                    var encoder = new System.Text.UTF8Encoding();
                    System.Text.Decoder utf8Decode = encoder.GetDecoder();
                    byte[] todecodeByte = Convert.FromBase64String(sData);
                    int charCount = utf8Decode.GetCharCount(todecodeByte, 0, todecodeByte.Length);
                    char[] decodedChar = new char[charCount];
                    utf8Decode.GetChars(todecodeByte, 0, todecodeByte.Length, decodedChar, 0);
                    string result = new String(decodedChar);
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error in base64Decode" + ex.Message);
                }
            }
        
    }
}
