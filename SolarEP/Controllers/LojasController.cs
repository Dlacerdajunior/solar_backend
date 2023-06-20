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
using System.Security.Cryptography;
using System.Text;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.ComponentModel;

namespace SolarEP.Controllers
{
    public class LojasController : Controller
    {
        private SolardbEntities db = new SolardbEntities();


        public ActionResult ImportExcel(int? id, string razao_social_loja, string cnpj_loja, string kunnr, string loja_status)
        {
            var sql = "select * from lojas where lojas.status_loja != 4 and lojas.status_loja != 3";
            if (razao_social_loja != null && razao_social_loja.Trim() != "")
            {
                sql = sql + " and lojas.razao_social_loja like '%" + razao_social_loja + "%'";
            }
            if (cnpj_loja != null && cnpj_loja.Trim() != "")
            {
                sql = sql + " and lojas.cnpj_loja like '%" + cnpj_loja + "%'";
            }
            if (kunnr != null && kunnr.Trim() != "")
            {
                sql = sql + " and lojas.kunnr like '%" + kunnr + "%'";
            }
            if (loja_status != null && loja_status.Trim() != "")
            {
                sql = sql + " and lojas.status_loja = '" + loja_status + "'";
            }

            DataTable dt = new DataTable();

            dt.Columns.Add("ID Loja", typeof(string));
            dt.Columns.Add("Razão Social Loja", typeof(string));
            dt.Columns.Add("CNPJ/CPF", typeof(string));
            dt.Columns.Add("KUNNR", typeof(string));
            dt.Columns.Add("Nome Contato", typeof(string));
            dt.Columns.Add("E-mail", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Data Criação", typeof(string));
            dt.Columns.Add("Latitude", typeof(string));
            dt.Columns.Add("Longitude", typeof(string));

            var itemLog = db.lojas.SqlQuery(sql).ToList();

            foreach (var item in itemLog)
            {
                var razao_social = "";
                if (item.razao_social_loja != null)
                {
                    razao_social = item.razao_social_loja;
                }

                var idloja = item.id.ToString();

                var cnpj = "";
                if (item.cnpj_loja != null)
                {
                    cnpj = item.cnpj_loja;
                }
                var kunnr1 = "";
                if (item.kunnr != null)
                {
                    kunnr1 = item.kunnr;
                }
                var nome_contato = "";
                if (item.nome_contato != null)
                {
                    nome_contato = item.nome_contato;
                }
                var email_contato = "";
                if (item.email_contato != null)
                {
                    email_contato = item.email_contato;
                }
                var status_loja = "";
                if (item.status_loja != null)
                {
                    status_loja = item.loja_status.status_nome;
                }
                var created = "";
                if (item.created != null)
                {
                    created = item.created.ToString();
                }
                var lattext = "";
                if (item.lattext != null)
                {
                    lattext = item.lattext;
                }
                var longtext = "";
                if (item.longtext != null)
                {
                    longtext = item.longtext;
                }

                dt.Rows.Add(
                    idloja,
                    razao_social,
                    cnpj,
                    kunnr1,
                    nome_contato,
                    email_contato,
                    status_loja,
                    created,
                    lattext,
                    longtext);
            }

            WriteExcelWithNPOI(dt, "xlsx");

            return View("Lojas/ListaLojas"); //left join ou right join
        }


        public ActionResult ImportExcel2(int? id, string razao_social_loja, string cnpj_loja, string kunnr, string loja_status)
        {
            var sql = "select * from lojas where  lojas.status_loja = 4";
            if (razao_social_loja != null && razao_social_loja.Trim() != "")
            {
                sql = sql + " and lojas.razao_social_loja like '%" + razao_social_loja + "%'";
            }
            if (cnpj_loja != null && cnpj_loja.Trim() != "")
            {
                sql = sql + " and lojas.cnpj_loja like '%" + cnpj_loja + "%'";
            }
            if (kunnr != null && kunnr.Trim() != "")
            {
                sql = sql + " and lojas.kunnr like '%" + kunnr + "%'";
            }
            if (loja_status != null && loja_status.Trim() != "")
            {
                sql = sql + " and lojas.status_loja = '" + loja_status + "'";
            }

            DataTable dt = new DataTable();

            dt.Columns.Add("ID Loja", typeof(string));
            dt.Columns.Add("Razão Social Loja", typeof(string));
            dt.Columns.Add("CNPJ/CPF", typeof(string));
            dt.Columns.Add("KUNNR", typeof(string));
            dt.Columns.Add("Nome Contato", typeof(string));
            dt.Columns.Add("E-mail", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Data Criação", typeof(string));
            dt.Columns.Add("Latitude", typeof(string));
            dt.Columns.Add("Longitude", typeof(string));

            var itemLog = db.lojas.SqlQuery(sql).ToList();

            foreach (var item in itemLog)
            {
                var razao_social = "";
                if (item.razao_social_loja != null)
                {
                    razao_social = item.razao_social_loja;
                }

                var idloja = item.id.ToString();

                var cnpj = "";
                if (item.cnpj_loja != null)
                {
                    cnpj = item.cnpj_loja;
                }
                var kunnr1 = "";
                if (item.kunnr != null)
                {
                    kunnr1 = item.kunnr;
                }
                var nome_contato = "";
                if (item.nome_contato != null)
                {
                    nome_contato = item.nome_contato;
                }
                var email_contato = "";
                if (item.email_contato != null)
                {
                    email_contato = item.email_contato;
                }
                var status_loja = "";
                if (item.status_loja != null)
                {
                    status_loja = item.loja_status.status_nome;
                }
                var created = "";
                if (item.created != null)
                {
                    created = item.created.ToString();
                }
                var lattext = "";
                if (item.lattext != null)
                {
                    lattext = item.lattext;
                }
                var longtext = "";
                if (item.longtext != null)
                {
                    longtext = item.longtext;
                }

                dt.Rows.Add(
                    idloja,
                    razao_social,
                    cnpj,
                    kunnr1,
                    nome_contato,
                    email_contato,
                    status_loja,
                    created,
                    lattext,
                    longtext);
            }

            WriteExcelWithNPOI(dt, "xlsx");

            return View("Lojas/AguardandoAprovacao"); //left join ou right join
        }

        public void WriteExcelWithNPOI(DataTable dt, String extension)
        {
            IWorkbook workbook;

            if (extension == "xlsx")
            {
                workbook = new XSSFWorkbook();
            }
            else if (extension == "xls")
            {
                workbook = new HSSFWorkbook();
            }
            else
            {
                throw new Exception("This format is not supported");
            }

            ISheet sheet1 = workbook.CreateSheet("Sheet 1");

            //make a header row
            IRow row1 = sheet1.CreateRow(0);

            for (int j = 0; j < dt.Columns.Count; j++)
            {
                ICell cell = row1.CreateCell(j);
                String columnName = dt.Columns[j].ToString();
                cell.SetCellValue(columnName);
            }

            //loops through data
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    ICell cell = row.CreateCell(j);
                    String columnName = dt.Columns[j].ToString();
                    cell.SetCellValue(dt.Rows[i][columnName].ToString());
                }
            }

            using (var exportData = new MemoryStream())
            {
                Response.Clear();
                workbook.Write(exportData);
                if (extension == "xlsx") //xlsx file format
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "Relatorio-Lojas.xlsx"));
                    Response.BinaryWrite(exportData.ToArray());
                }
                else if (extension == "xls")  //xls file format
                {
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "Relatorio-Lojas.xls"));
                    Response.BinaryWrite(exportData.GetBuffer());
                }
                Response.End();
            }
        }

        public void WriteTsv<T>(IEnumerable<T> data, TextWriter output)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in props)
            {
                output.Write(prop.DisplayName); // header
                output.Write("\t");
            }
            output.WriteLine();
            foreach (T item in data)
            {
                foreach (PropertyDescriptor prop in props)
                {
                    output.Write(prop.Converter.ConvertToString(
                         prop.GetValue(item)));
                    output.Write("\t");
                }
                output.WriteLine();
            }
        }


        // GET: /Lojas/
        public ActionResult ListaLojas(int? pagina, string razao_social_loja, string cnpj_loja, string kunnr, string loja_status, string sortOrder)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.loja_status = new SelectList(db.loja_status.Where(a => a.id != 4 && a.id != 3), "id", "status_nome");

            int tamanhoPagina = 25;
            int numeroPagina = pagina ?? 1;

            var sql = "select * from lojas where lojas.status_loja != 3 and lojas.status_loja != 4";
            if (razao_social_loja != null && razao_social_loja.Trim() != "")
            {
                sql = sql + " and lojas.razao_social_loja like '%" + razao_social_loja + "%'";
            }
            if (cnpj_loja != null && cnpj_loja.Trim() != "")
            {
                sql = sql + " and lojas.cnpj_loja like '%" + cnpj_loja + "%'";
            }
            if (kunnr != null && kunnr.Trim() != "")
            {
                sql = sql + " and lojas.kunnr like '%" + kunnr + "%'";
            }
            if (loja_status != null && loja_status.Trim() != "")
            {
                sql = sql + " and lojas.status_loja = '" + loja_status + "'";
            }

            var lojas = db.lojas.SqlQuery(sql).ToList();
            ViewBag.qtdlojas = lojas.Count();

            if (sortOrder == "razao_social_loja")
            {
                return View(lojas.OrderBy(p => p.razao_social_loja).ToPagedList(numeroPagina, tamanhoPagina));
            }
            else if (sortOrder == "cnpj_loja")
            {
                return View(lojas.OrderBy(p => p.cnpj_loja).ToPagedList(numeroPagina, tamanhoPagina));
            }
            else if (sortOrder == "kunnr")
            {
                return View(lojas.OrderBy(p => p.kunnr).ToPagedList(numeroPagina, tamanhoPagina));
            }
            else if (sortOrder == "created")
            {
                return View(lojas.OrderByDescending(p => p.created).ToPagedList(numeroPagina, tamanhoPagina));
            }
            else
            {
                return View(lojas.OrderByDescending(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
            }

        }

        // GET: /Lojas/AguardandoAprovacao
        public ActionResult AguardandoAprovacao(int? pagina, string razao_social_loja, string cnpj_loja, string kunnr, string loja_status, string sortOrder)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

           // ViewBag.loja_status = new SelectList(db.loja_status, "id", "status_nome");

            int tamanhoPagina = 25;
            int numeroPagina = pagina ?? 1;

            var sql = "select * from lojas where lojas.status_loja = 4";
            if (razao_social_loja != null && razao_social_loja.Trim() != "")
            {
                sql = sql + " and lojas.razao_social_loja like '%" + razao_social_loja + "%'";
            }
            if (cnpj_loja != null && cnpj_loja.Trim() != "")
            {
                sql = sql + " and lojas.cnpj_loja like '%" + cnpj_loja + "%'";
            }
            if (kunnr != null && kunnr.Trim() != "")
            {
                sql = sql + " and lojas.kunnr like '%" + kunnr + "%'";
            }
            if (loja_status != null && loja_status.Trim() != "")
            {
                sql = sql + " and lojas.status_loja = '" + loja_status + "'";
            }

            var lojas = db.lojas.SqlQuery(sql).ToList();
            ViewBag.qtdlojas = lojas.Count();


            if (sortOrder == "razao_social_loja")
            {
                return View(lojas.OrderBy(p => p.razao_social_loja).ToPagedList(numeroPagina, tamanhoPagina));
            }
            else if (sortOrder == "cnpj_loja")
            {
                return View(lojas.OrderBy(p => p.cnpj_loja).ToPagedList(numeroPagina, tamanhoPagina));
            }
            else if (sortOrder == "kunnr")
            {
                return View(lojas.OrderBy(p => p.kunnr).ToPagedList(numeroPagina, tamanhoPagina));
            }
            else if (sortOrder == "created")
            {
                return View(lojas.OrderByDescending(p => p.created).ToPagedList(numeroPagina, tamanhoPagina));
            }
            else
            {
                return View(lojas.OrderByDescending(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
            }
        }

        // GET: /Lojas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lojas lojas = db.lojas.Find(id);
            if (lojas == null)
            {
                return HttpNotFound();
            }
            return View(lojas);
        }

        // GET: /Lojas/Create
        public ActionResult Create()
        {
            ViewBag.status_loja = new SelectList(db.loja_status, "id", "status_nome");
            ViewBag.ufloja = new SelectList(db.uf, "id", "uf_sigla");
            return View();
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

        // POST: /Lojas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cnpj_loja, razao_social_loja, loja_ativa, nome_contato, email_contato, id, lattext, longtext, senha, status_loja, email_codigo, kunnr")] lojas lojas)
        {
            if (ModelState.IsValid)
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[8];
                var random = new Random();

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                var finalString = new String(stringChars);

                lojas.senha = MD5Hash(lojas.senha);
                lojas.email_codigo = finalString;
                db.lojas.Add(lojas);
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.PathAndQuery);
                // return RedirectToAction("Index");
            }

            ViewBag.status_loja = new SelectList(db.loja_status, "id", "status_nome", lojas.status_loja);
            return View(lojas);
        }

        // GET: /Lojas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lojas lojas = db.lojas.Find(id);
            if (lojas == null)
            {
                return HttpNotFound();
            }

            ViewBag.status_loja = new SelectList(db.loja_status.Where(a => a.id != 4 && a.id != 3), "id", "status_nome", lojas.status_loja);
            return View(lojas);
        }

        // POST: /Lojas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cnpj_loja, razao_social_loja, loja_ativa, nome_contato, email_contato, id, lattext, longtext, senha, status_loja, email_codigo, kunnr")] lojas lojas)
        {
            if (ModelState.IsValid)
            {                
                db.Entry(lojas).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            ViewBag.status_loja = new SelectList(db.loja_status.Where(a => a.id != 4 && a.id != 3), "id", "status_nome", lojas.status_loja);
            return View(lojas);
        }

        // GET: /Lojas/Aprovar/5
        public ActionResult Aprovar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lojas lojas = db.lojas.Find(id);
            if (lojas == null)
            {
                return HttpNotFound();
            }
            return View(lojas);
        }


        //gerador maquina de codigos
        public String GeraCodigo2()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[10];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            var confere = db.lojas.Where(a => a.email_codigo == finalString);
            if (confere.Count() != 0)
            {
                var finalString2 = GeraCodigo2();
                return finalString2;
            }
            else
            {
                return finalString;
            }
        }

        // POST: /Lojas/Aprovar/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Aprovar(lojas lojas)
        {
            if (ModelState.IsValid)
            {
                var finalString = GeraCodigo2();


                var dadosloja = db.lojas.FirstOrDefault(l => l.id == lojas.id);
                dadosloja.email_codigo = finalString;
                dadosloja.status_loja = 5;
                db.SaveChanges();

                var message = new MailMessage();

                var body = "<p>Olá, {0}</p>  <p>Obrigado por se registrar no App Solar. Para ativar a conta basta visitar o endereço fornecido abaixo, confirmando assim o seu endereço de email.</p> <p>Para completar o registro visite o link:</p> <p> http://solarcoke.solarbr.com.br:32014/SolarActivate?code=" +finalString+ "</p> <p> * Caso o link neste email não funcione, por favor abra o seu navegador, copie o link na área de endereço e aperte enter.</p> <p> Atenciosamente,<br> SolarBr </p>";

            //    var body = "<p>Parabéns, Sua Loja foi aprovada! {0} {1}</p><p>Campo util para:</p><p>{2}</p>";

                message.To.Add(new MailAddress(dadosloja.email_contato));  // replace with valid value 
                message.From = new MailAddress("cadastroclientessolar@solarbr.com.br");  // replace with valid value
                message.Subject = "Confirme seu endereço de e-mail";
                message.Body = string.Format(body, dadosloja.razao_social_loja);
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
                return Redirect(Request.UrlReferrer.PathAndQuery);
                //     return RedirectToAction("Index");
            }
            return View(lojas);
        }

        // GET: /Lojas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lojas lojas = db.lojas.Find(id);
            if (lojas == null)
            {
                return HttpNotFound();
            }
            return View(lojas);
        }

        // POST: /Lojas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            lojas lojas = db.lojas.Find(id);
            db.lojas.Remove(lojas);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.PathAndQuery);
            //            return RedirectToAction("Index");
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
