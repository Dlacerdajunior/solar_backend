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
using System.Net.Mail;
using System.Net.Http;
using System.Data.SqlClient;


namespace SolarEP.Controllers
{
    public class RoboController : Controller
    {
        private SolardbEntities db = new SolardbEntities();

        public void MonitamentoMetodos(ciclo_robo ciclo_robo)
        {
            var metodotexto = "";

            String url = "http://solarcoke.solarbr.com.br:32014/Mobile/JsonDetalhelojaWS?cp=0002372817";
            try
            {
                // Creates an HttpWebRequest for the specified URL. 
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                // Sends the HttpWebRequest and waits for a response.
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
                    Console.WriteLine("\r\nResponse Status Code is OK and StatusDescription is: {0}",
                                         myHttpWebResponse.StatusDescription);
                // Releases the resources of the response.
                myHttpWebResponse.Close();
            }
            catch (WebException e)
            {
                metodotexto = "ObterCliente";
                Console.WriteLine("\r\nWebException Raised. The following error occured : {0}", e.Status);
            }
            catch (Exception e)
            {
                metodotexto = "ObterCliente";
                Console.WriteLine("\nThe following Exception was raised : {0}", e.Message);
            }

       /*      String url2 = "http://solarcoke.solarbr.com.br:32014/Mobile/JsonLoginMobileCp?cp=0002372817";
             try
             {
                 // Creates an HttpWebRequest for the specified URL. 
                 HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url2);
                 // Sends the HttpWebRequest and waits for a response.
                 HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                 if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
                     Console.WriteLine("\r\nResponse Status Code is OK and StatusDescription is: {0}",
                                          myHttpWebResponse.StatusDescription);
                 // Releases the resources of the response.
                 myHttpWebResponse.Close();
             }
             catch (WebException e)
             {
                 JsonLoginMobileCp = "ObterCliente";
                 Console.WriteLine("\r\nWebException Raised. The following error occured : {0}", e.Status);
             }
             catch (Exception e)
             {
                 JsonLoginMobileCp = "ObterCliente";
                 Console.WriteLine("\nThe following Exception was raised : {0}", e.Message);
             } */

             String url3 = "http://solarcoke.solarbr.com.br:32014/Mobile/JsonPedidoMobileCp?cp=0006000684";
             try
             {
                 // Creates an HttpWebRequest for the specified URL. 
                 HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url3);
                 // Sends the HttpWebRequest and waits for a response.
                 HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                 if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
                     Console.WriteLine("\r\nResponse Status Code is OK and StatusDescription is: {0}",
                                          myHttpWebResponse.StatusDescription);
                 // Releases the resources of the response.
                 myHttpWebResponse.Close();
             }
             catch (WebException e)
             {
                 if (metodotexto != "")
                 {
                    metodotexto = metodotexto + ", ObterInformacoesPedidos";                     
                 }
                 else
                 {
                     metodotexto = "ObterInformacoesPedidos";
                 }
                 Console.WriteLine("\r\nWebException Raised. The following error occured : {0}", e.Status);
             }
             catch (Exception e)
             {
                 if (metodotexto != "")
                 {
                     metodotexto = metodotexto + ", ObterInformacoesPedidos";
                 }
                 else
                 {
                     metodotexto = "ObterInformacoesPedidos";
                 }
                 Console.WriteLine("\nThe following Exception was raised : {0}", e.Message);
             }

             String url4 = "http://solarcoke.solarbr.com.br:32014/Mobile/JsonEquipamentosMobileCP?cp=0002069602";
             try
             {
                 // Creates an HttpWebRequest for the specified URL. 
                 HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url4);
                 // Sends the HttpWebRequest and waits for a response.
                 HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                 if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
                     Console.WriteLine("\r\nResponse Status Code is OK and StatusDescription is: {0}",
                                          myHttpWebResponse.StatusDescription);
                 // Releases the resources of the response.
                 myHttpWebResponse.Close();
             }
             catch (WebException e)
             {
                 if (metodotexto != "")
                 {
                     metodotexto = metodotexto + ", ObterEquipamentos";
                 }
                 else
                 {
                     metodotexto = "ObterEquipamentos";
                 }
                 Console.WriteLine("\r\nWebException Raised. The following error occured : {0}", e.Status);
             }
             catch (Exception e)
             {
                 if (metodotexto != "")
                 {
                     metodotexto = metodotexto + ", ObterEquipamentos";
                 }
                 else
                 {
                     metodotexto = "ObterEquipamentos";
                 }
                 Console.WriteLine("\nThe following Exception was raised : {0}", e.Message);
             }

             if (metodotexto != "")
             {
                 ciclo_robo.ciclo = db.ciclo_robo.OrderByDescending(p => p.id).FirstOrDefault().ciclo + 1;
                 ciclo_robo.status = 0;
                 ciclo_robo.data = DateTime.Now;
                 ciclo_robo.metodo = metodotexto;
                 db.ciclo_robo.Add(ciclo_robo);
                 db.SaveChanges();

                var emailadm = db.configuracao.Where(a => a.nome == "EmailMonitoramento" && a.status == 1).ToList();
                if (emailadm.Count() != 0)
                {
                    foreach (var item in emailadm)
                    {
                        var email = new MailMessage();
                        var message = new MailMessage();
                        var body = "<p>Olá Administrador,</p><p>{0}</p><p>{1}</p>";

                        message.To.Add(new MailAddress(item.valor));  // replace with valid value 
                        message.From = new MailAddress("cadastroclientessolar@solarbr.com.br");  // replace with valid value
                        message.Subject = "Solar - Método Web Service sem retorno.";
                        message.Body = string.Format(body, "Os seguintes métodos do Web Service Solar se encontram ausentes:", metodotexto);
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
                    }
                }
            }
             else
             {
                 ciclo_robo.ciclo = db.ciclo_robo.OrderByDescending(p => p.id).FirstOrDefault().ciclo + 1;
                 ciclo_robo.status = 1;
                 ciclo_robo.data = DateTime.Now;
                 ciclo_robo.metodo = "ObterCliente, ObterInformacoesPedidos, ObterEquipamentos";
                 db.ciclo_robo.Add(ciclo_robo);
                 db.SaveChanges();
             }
        }


        public ActionResult Mudarstatus(int? statusadm)
        {
            if (statusadm == 1)
            {
                using (var ctx = new SolardbEntities())
                {
                    //Update command
                    int noOfRowUpdated = ctx.Database.ExecuteSqlCommand("update configuracao set configuracao.status = 0 where configuracao.status = 1");
                }
             /*   using (SqlConnection connection = new SqlConnection("data source=204.93.178.157;initial catalog=mobitap_solar;user id=mobitap_solar;password=solar@123;MultipleActiveResultSets=True;App=EntityFramework&quot;"))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "update configuracao set configuracao.status = 0 where configuracao.status = 1";
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                } */
            }
            else
            {
                using (var ctx = new SolardbEntities())
                {
                    //Update command
                    int noOfRowUpdated = ctx.Database.ExecuteSqlCommand("update configuracao set configuracao.status = 1 where configuracao.status = 0");
                }

              /*  using (SqlConnection connection = new SqlConnection("data source=204.93.178.157;initial catalog=mobitap_solar;user id=mobitap_solar;password=solar@123;MultipleActiveResultSets=True;App=EntityFramework&quot;"))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "update configuracao set configuracao.status = 1 where configuracao.status = 0";
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                } */
            }

            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        // GET: /Robo/
        public ActionResult Index(int? pagina, string datainicial, string datafinal, string status)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            
            var sql = "select * from ciclo_robo where 1=1";
            if (status != null && status.Trim() != "")
            {
                sql = sql + " and ciclo_robo.status = " + status + "";
            }

            if (datainicial != null && datainicial.Trim() != "")
            {
                string[] arrDate = datainicial.Split('/');

                string day = arrDate[0].ToString();
                string month = arrDate[1].ToString();
                string year = arrDate[2].ToString();

                string[] arrDate2 = datafinal.Split('/');

                string day2 = arrDate2[0].ToString();
                string month2 = arrDate2[1].ToString();
                string year2 = arrDate2[2].ToString();

                sql = sql + " and ciclo_robo.data between '" + year + "-" + month + "-" + day + " 00:01' AND '" + year2 + "-" + month2 + "-" + day2 + " 23:59';";
            }

            int tamanhoPagina = 20;
            int numeroPagina = pagina ?? 1;

            var ciclo_robo = db.ciclo_robo.SqlQuery(sql).ToList();
            ViewBag.qtd = ciclo_robo.Count();
            ViewBag.statusadm = db.configuracao.Where(a => a.nome == "EmailMonitoramento").FirstOrDefault().status;
            return View(ciclo_robo.OrderByDescending(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));    
        }

        // GET: /Robo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ciclo_robo ciclo_robo = db.ciclo_robo.Find(id);
            if (ciclo_robo == null)
            {
                return HttpNotFound();
            }
            return View(ciclo_robo);
        }

        // GET: /Robo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Robo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,metodo,data,status,ciclo")] ciclo_robo ciclo_robo)
        {
            if (ModelState.IsValid)
            {
                db.ciclo_robo.Add(ciclo_robo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ciclo_robo);
        }

        // GET: /Robo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ciclo_robo ciclo_robo = db.ciclo_robo.Find(id);
            if (ciclo_robo == null)
            {
                return HttpNotFound();
            }
            return View(ciclo_robo);
        }

        // POST: /Robo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,metodo,data,status,ciclo")] ciclo_robo ciclo_robo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ciclo_robo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ciclo_robo);
        }

        // GET: /Robo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ciclo_robo ciclo_robo = db.ciclo_robo.Find(id);
            if (ciclo_robo == null)
            {
                return HttpNotFound();
            }
            return View(ciclo_robo);
        }

        // POST: /Robo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ciclo_robo ciclo_robo = db.ciclo_robo.Find(id);
            db.ciclo_robo.Remove(ciclo_robo);
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
