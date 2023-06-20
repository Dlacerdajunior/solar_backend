using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SolarEP.Models;
using System.Net.Mail;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace SolarEP.Controllers
{
    public class LoginController : Controller
    {
        private SolardbEntities db = new SolardbEntities();
        //
        // GET: /Login/
        public ActionResult Login()
        {   
            return View();
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            Session["userid"] = null;
            Session["usertipo"] = null;
            return RedirectToAction("Login", "Login");
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

        [HttpPost]
        public ActionResult Login(administrador administrador)
        {
            var rsmd5 = MD5Hash(administrador.senha);

            var rsAdmin = db.administrador.Where(a => a.email == administrador.email && a.senha == rsmd5).FirstOrDefault();

            if (rsAdmin != null)
            {
                Session["user"] = rsAdmin.nome;
                Session["userid"] = rsAdmin.id;
                Session["usertipo"] = rsAdmin.adm_tipo;
                if (rsAdmin.adm_tipo == 1)
                {
                    return RedirectToAction("Index", "Administrador");                    
                }
                else
                {
                    return RedirectToAction("Index", "VisitaTecnica");
                }
            }
            else
            {
                Response.Write("<script>alert('Login ou Senha incorretos.')</script>");                
            }
            return View();
        }
        public ActionResult EsqueciSenha()
        {
            return View();
        }

        public  ActionResult ComCodigo(string comcodigo)
        {
            var rsAdmin = db.administrador.Where(a => a.email_codigo == comcodigo).FirstOrDefault();
            
            if (rsAdmin != null)
            {
                Session["email_codigo"] = rsAdmin.email_codigo;
                return RedirectToAction("TrocarSenha", "Login");
            }
            else
            {
                TempData["resposta"] = "true";    
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
        }

        public ActionResult TrocarSenha()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TrocarSenha(string senha, string confsenha)
        {
            var codm = Session["email_codigo"].ToString();
            var rsAdmin = db.administrador.Where(a => a.email_codigo == codm).FirstOrDefault();

            if (senha == "" || confsenha == "")
            {
                TempData["campovazio"] = "true";
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            else if (senha != confsenha)
            {
                TempData["naoconfere"] = "true";
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            else if (ModelState.IsValid)
            {
                rsAdmin.senha = MD5Hash(senha);
                db.SaveChanges();
                TempData["sucesso"] = "true";
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        public ActionResult ComEmail(string comemail)
        {
            var rsAdmin = db.administrador.Where(a => a.email == comemail).FirstOrDefault();

            if (rsAdmin != null)
            {
                Session["email_contato"] = rsAdmin.email;

                var email = new MailMessage();
                var message = new MailMessage();
                var body = "<p>Token: {0} {1}</p><p>Seu código abaixo</p><p>{2}</p>";

                message.To.Add(new MailAddress(rsAdmin.email));  // replace with valid value 
                message.From = new MailAddress("cadastroclientessolar@solarbr.com.br");  // replace with valid value
                message.Subject = "Solar - Seu Código (token)";
                message.Body = string.Format(body, rsAdmin.email_codigo, "", "solicitar texto a ser inserido no email");
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "schcadastrocli@solarbr.com.br",  // replace with valid value
                        Password = "1q2w3e4r!Q@W#E$R"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.office365.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Send(message);
                }

                TempData["deucerto"] = "true";
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            else
            {
                TempData["sememail"] = "true";
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
        }
	}
}