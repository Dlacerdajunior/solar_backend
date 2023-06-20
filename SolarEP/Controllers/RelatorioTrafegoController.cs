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
using System.ComponentModel;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;

namespace SolarEP.Controllers
{
    public class RelatorioTrafegoController : Controller
    {
        private SolardbEntities db = new SolardbEntities();

        //GET: /RelatorioTrafego


        public ActionResult RelatorioTrafego(int? id, string datainicial, string datafinal, string telalog, string id_loja)
        {
            var sql = "select * from log_acesso where 1=1";

            if (telalog != null && telalog.Trim() != "")
            {
                sql = sql + " and log_acesso.tela = '" + telalog + "'";
            }
            if (id_loja != null && id_loja.Trim() != "")
            {
                sql = sql + " and log_acesso.id_loja = " + id_loja + "";
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

                sql = sql + " and log_acesso.datalog between '" + year + "-" + month + "-" + day + " 00:01' AND '" + year2 + "-" + month2 + "-" + day2 + " 23:59';";
            }

            //sql = sql + " GROUP BY ativacao.id " ;

            DataTable dt = new DataTable();

            dt.Columns.Add("ID Loja", typeof(string));
            dt.Columns.Add("Razão Social Loja", typeof(string));
            dt.Columns.Add("Tela", typeof(string));
            dt.Columns.Add("Data e Hora", typeof(string));

            var itemLog = db.log_acesso.SqlQuery(sql).ToList();

            foreach (var item in itemLog)
            {
                var razao_social = "";
                if (item.id_loja != null)
                {
                    razao_social = item.lojas.razao_social_loja;
                }
                var idloja = "";
                if (item.id_loja != null)
                {
                    idloja = item.id_loja.ToString();
                }
                var datalog = "";
                if (item.datalog != null)
                {
                    datalog = item.datalog.ToString();
                }
                var tela = "";
                if (item.tela != "")
                {
                    tela = item.tela;
                }

                dt.Rows.Add(
                    idloja,
                    razao_social,
                    tela,
                    datalog);
            }

            WriteExcelWithNPOI(dt, "xlsx");

            return View("RelatorioTrafego/Index"); //left join ou right join
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
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "Relatorio-Trafego.xlsx"));
                    Response.BinaryWrite(exportData.ToArray());
                }
                else if (extension == "xls")  //xls file format
                {
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "Relatorio-Trafego.xls"));
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


        // GET: /RelatorioTrafego/
        public ActionResult Index(int? pagina, string datainicial, string datafinal, string telalog, string id_loja)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }           

            var log_acesso2 = db.log_acesso.Select(i => new { i.tela }).Distinct().ToList();
            ViewBag.telalog = new SelectList(log_acesso2, "tela", "tela");
            ViewBag.id_loja = new SelectList(db.lojas, "id", "razao_social_loja");

            int tamanhoPagina = 20;
            int numeroPagina = pagina ?? 1;

            var sql = "select * from log_acesso where 1=1";
            if (telalog != null && telalog.Trim() != "")
            {
                sql = sql + " and log_acesso.tela = '" + telalog + "'";
            }
            if (id_loja != null && id_loja.Trim() != "")
            {
                sql = sql + " and log_acesso.id_loja = " + id_loja + "";
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

                sql = sql + " and log_acesso.datalog between '" + year + "-" + month + "-" + day + " 00:01' AND '" + year2 + "-" + month2 + "-" + day2 + " 23:59';";
            }
            var log_acesso = db.log_acesso.SqlQuery(sql).ToList();
            ViewBag.qtd = log_acesso.Count();
            return View(log_acesso.OrderByDescending(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));       
        }

        // GET: /RelatorioTrafego/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            log_acesso log_acesso = db.log_acesso.Find(id);
            if (log_acesso == null)
            {
                return HttpNotFound();
            }
            return View(log_acesso);
        }

        // GET: /RelatorioTrafego/Create
        public ActionResult Create()
        {
            ViewBag.id_loja = new SelectList(db.lojas, "id", "cnpj_loja");
            return View();
        }

        // POST: /RelatorioTrafego/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,tela,datalog,id_loja")] log_acesso log_acesso)
        {
            if (ModelState.IsValid)
            {
                db.log_acesso.Add(log_acesso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_loja = new SelectList(db.lojas, "id", "cnpj_loja", log_acesso.id_loja);
            return View(log_acesso);
        }

        // GET: /RelatorioTrafego/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            log_acesso log_acesso = db.log_acesso.Find(id);
            if (log_acesso == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_loja = new SelectList(db.lojas, "id", "cnpj_loja", log_acesso.id_loja);
            return View(log_acesso);
        }

        // POST: /RelatorioTrafego/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,tela,datalog,id_loja")] log_acesso log_acesso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(log_acesso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_loja = new SelectList(db.lojas, "id", "cnpj_loja", log_acesso.id_loja);
            return View(log_acesso);
        }

        // GET: /RelatorioTrafego/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            log_acesso log_acesso = db.log_acesso.Find(id);
            if (log_acesso == null)
            {
                return HttpNotFound();
            }
            return View(log_acesso);
        }

        // POST: /RelatorioTrafego/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            log_acesso log_acesso = db.log_acesso.Find(id);
            db.log_acesso.Remove(log_acesso);
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
