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
    public class CampanhasController : Controller
    {
        private SolardbEntities db = new SolardbEntities();

        // GET: /Campanhas/
        public ActionResult Campanha(int? pagina, string descricao_curta, string titulo, string aderir)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            int tamanhoPagina = 15;
            int numeroPagina = pagina ?? 1;

            var sql = "select * from gestao_campanha where gestao_campanha.id_tipocampanha = 1";
            if (descricao_curta != null && descricao_curta.Trim() != "")
            {
                sql = sql + " and gestao_campanha.descricao_curta like '%" + descricao_curta + "%'";
            }
            if (titulo != null && titulo.Trim() != "")
            {
                sql = sql + " and gestao_campanha.titulo like '%" + titulo + "%'";
            }
            if (aderir != null && aderir.Trim() != "")
            {
                sql = sql + " and gestao_campanha.aderir = " + aderir + "";
            }

            var gestao_campanha = db.gestao_campanha.SqlQuery(sql).ToList();
            ViewBag.qtd = gestao_campanha.Count();

            return View(gestao_campanha.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /MaterialPromocional/
        public ActionResult MaterialPromocional(int? pagina, string descricao_curta, string titulo, string descricao)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            int tamanhoPagina = 15;
            int numeroPagina = pagina ?? 1;

            var sql = "select * from gestao_campanha where gestao_campanha.id_tipocampanha = 2";
            if (descricao_curta != null && descricao_curta.Trim() != "")
            {
                sql = sql + " and gestao_campanha.descricao_curta like '%" + descricao_curta + "%'";
            }
            if (titulo != null && titulo.Trim() != "")
            {
                sql = sql + " and gestao_campanha.titulo like '%" + titulo + "%'";
            }
            if (descricao != null && descricao.Trim() != "")
            {
                sql = sql + " and gestao_campanha.descricao like '%" + descricao + "%'";
            }

            var gestao_campanha = db.gestao_campanha.SqlQuery(sql).ToList();

            ViewBag.qtd = gestao_campanha.Count();

            return View(gestao_campanha.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Lancamentos/
        public ActionResult Lancamentos(int? pagina, string descricao_curta, string titulo, string aderir)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            int tamanhoPagina = 15;
            int numeroPagina = pagina ?? 1;

            var sql = "select * from gestao_campanha where gestao_campanha.id_tipocampanha = 3";
            if (descricao_curta != null && descricao_curta.Trim() != "")
            {
                sql = sql + " and gestao_campanha.descricao_curta like '%" + descricao_curta + "%'";
            }
            if (titulo != null && titulo.Trim() != "")
            {
                sql = sql + " and gestao_campanha.titulo like '%" + titulo + "%'";
            }
            if (aderir != null && aderir.Trim() != "")
            {
                sql = sql + " and gestao_campanha.aderir = " + aderir + "";
            }

            var gestao_campanha = db.gestao_campanha.SqlQuery(sql).ToList();
            ViewBag.qtd = gestao_campanha.Count();

            return View(gestao_campanha.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }



        // GET: /Campanhas/Create
        public ActionResult CreateCampanha()
        {
            ViewBag.ativa = new SelectList(db.campanha_ativa, "id", "ativa");
            ViewBag.id_tipocampanha = new SelectList(db.campanha_tipo, "id", "nome");
            return View();
        }

        // POST: /Campanhas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCampanha([Bind(Include = "id,titulo,url_foto,descricao_curta,descricao,permissao_img,aderir,url_facebok,url_twitter,url_youtube,id_tipocampanha,url_arquivo,url_ldpi,url_mdpi,url_hdpi,url_xhdpi,link,link_nome,permitir_lista,iphone4,iphone5,iphone6,iphone6plus,base64_foto,ativa,icon_image")] gestao_campanha gestao_campanha, HttpPostedFileBase coverimage, HttpPostedFileBase icon_image)
        {
            var fileName = "";
            var fileName2 = "";

            if (ModelState.IsValid)
            {
                var finalString = GeraCodigo();

                if (coverimage != null && coverimage.ContentLength > 0)
                {         
                    fileName = Path.GetFileName(finalString + coverimage.FileName).ToString().Replace(" ", "");
                    fileName = removerAcentos(fileName);
                    fileName = removerSpecial(fileName);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/CoverImages"), fileName);
                    coverimage.SaveAs(path);
                }


                if (fileName == "")
                {
                    gestao_campanha.url_foto = null;
                }
                else
                {
                    gestao_campanha.url_foto = "http://solarcoke.solarbr.com.br:32014/Content/assets/CoverImages/" + fileName;
                }


                if (icon_image != null && icon_image.ContentLength > 0)
                {
                    fileName2 = Path.GetFileName(finalString + icon_image.FileName).ToString().Replace(" ", "");
                    fileName2 = removerAcentos(fileName2);
                    fileName2 = removerSpecial(fileName2);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/CoverImages"), fileName2);
                    icon_image.SaveAs(path);
                }


                if (fileName2 == "")
                {
                    gestao_campanha.icon_image = null;
                }
                else
                {
                    gestao_campanha.icon_image = "http://solarcoke.solarbr.com.br:32014/Content/assets/CoverImages/" + fileName2;
                }

                gestao_campanha.id_tipocampanha = 1;
                db.gestao_campanha.Add(gestao_campanha);
                db.SaveChanges();
                return RedirectToAction("Campanha");
            }

            ViewBag.ativa = new SelectList(db.campanha_ativa, "id", "ativa", gestao_campanha.ativa);
            ViewBag.id_tipocampanha = new SelectList(db.campanha_tipo, "id", "nome", gestao_campanha.id_tipocampanha);
            return View(gestao_campanha);
        }



        // GET: /Campanhas/CreateMaterial
        public ActionResult CreateMaterial()
        {
            ViewBag.ativa = new SelectList(db.campanha_ativa, "id", "ativa");
            ViewBag.id_tipocampanha = new SelectList(db.campanha_tipo, "id", "nome");
            return View();
        }

        // POST: /Campanhas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMaterial([Bind(Include = "id,titulo,url_foto,descricao_curta,descricao,permissao_img,aderir,url_facebok,url_twitter,url_youtube,id_tipocampanha,url_arquivo,url_ldpi,url_mdpi,url_hdpi,url_xhdpi,link,link_nome,permitir_lista,iphone4,iphone5,iphone6,iphone6plus,base64_foto,ativa,icon_image")] gestao_campanha gestao_campanha, HttpPostedFileBase coverimage, HttpPostedFileBase arquivo, HttpPostedFileBase ldpi, HttpPostedFileBase mdpi, HttpPostedFileBase hdpi, HttpPostedFileBase xhdpi, HttpPostedFileBase iphone4, HttpPostedFileBase iphone5, HttpPostedFileBase iphone6, HttpPostedFileBase iphone6plus, HttpPostedFileBase icon_image)
        {
            var fileName = "";
            var fileName2 = "";
            var fileName3 = "";
            var ldpiName = "";
            var mdpiName = "";
            var hdpiName = "";
            var xhdpiName = "";
            var iphone4Name = "";
            var iphone5Name = "";
            var iphone6Name = "";
            var iphone6plusName = "";

            if (ModelState.IsValid)
            {
                var finalString = GeraCodigo();

                if (coverimage != null && coverimage.ContentLength > 0)
                {
                    fileName = Path.GetFileName(finalString + coverimage.FileName).ToString().Replace(" ", "");
                    fileName = removerAcentos(fileName);
                    fileName = removerSpecial(fileName);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/CoverImages"), fileName);
                    coverimage.SaveAs(path);
                }

                if (arquivo != null && arquivo.ContentLength > 0)
                {
                    fileName2 = Path.GetFileName(finalString + arquivo.FileName).ToString().Replace(" ", "");
                    fileName2 = removerAcentos(fileName2);
                    fileName2 = removerSpecial(fileName2);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/ArquivoMaterial"), fileName2);
                    arquivo.SaveAs(path);
                }

                if (ldpi != null && ldpi.ContentLength > 0)
                {
                    
                    ldpiName = Path.GetFileName("ldpi" + finalString + ldpi.FileName).ToString().Replace(" ", "");
                    ldpiName = removerAcentos(ldpiName);
                    ldpiName = removerSpecial(ldpiName);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/ArquivoMaterial"), ldpiName);
                    ldpi.SaveAs(path);               

                }

                if (mdpi != null && mdpi.ContentLength > 0)
                {
                    mdpiName = Path.GetFileName("mdpi" + finalString + mdpi.FileName).ToString().Replace(" ", "");
                    mdpiName = removerAcentos(mdpiName);
                    mdpiName = removerSpecial(mdpiName);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/ArquivoMaterial"), mdpiName);
                    mdpi.SaveAs(path);
                }

                if (hdpi != null && hdpi.ContentLength > 0)
                {
                    hdpiName = Path.GetFileName("hdpi" + finalString + hdpi.FileName).ToString().Replace(" ", "");
                    hdpiName = removerAcentos(hdpiName);
                    hdpiName = removerSpecial(hdpiName);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/ArquivoMaterial"), hdpiName);
                    hdpi.SaveAs(path);
                }

                if (xhdpi != null && xhdpi.ContentLength > 0)
                {
                    xhdpiName = Path.GetFileName("xhdpi" + finalString + xhdpi.FileName).ToString().Replace(" ", "");
                    xhdpiName = removerAcentos(xhdpiName);
                    xhdpiName = removerSpecial(xhdpiName);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/ArquivoMaterial"), xhdpiName);
                    xhdpi.SaveAs(path);
                }

                if (iphone4 != null && iphone4.ContentLength > 0)
                {
                    iphone4Name = Path.GetFileName("iphone4" + finalString + iphone4.FileName).ToString().Replace(" ", "");
                    iphone4Name = removerAcentos(iphone4Name);
                    iphone4Name = removerSpecial(iphone4Name);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/ArquivoMaterial"), iphone4Name);
                    iphone4.SaveAs(path);
                }

                if (iphone5 != null && iphone5.ContentLength > 0)
                {
                    iphone5Name = Path.GetFileName("iphone5" + finalString + iphone5.FileName).ToString().Replace(" ", "");
                    iphone5Name = removerAcentos(iphone5Name);
                    iphone5Name = removerSpecial(iphone5Name);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/ArquivoMaterial"), iphone5Name);
                    iphone5.SaveAs(path);
                }

                if (iphone6 != null && iphone6.ContentLength > 0)
                {
                    iphone6Name = Path.GetFileName("iphone6" + finalString + iphone6.FileName).ToString().Replace(" ", "");
                    iphone6Name = removerAcentos(iphone6Name);
                    iphone6Name = removerSpecial(iphone6Name);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/ArquivoMaterial"), iphone6Name);
                    iphone6.SaveAs(path);
                }

                if (iphone6plus != null && iphone6plus.ContentLength > 0)
                {
                    iphone6plusName = Path.GetFileName("iphone6plus" + finalString + iphone6plus.FileName).ToString().Replace(" ", "");
                    iphone6plusName = removerAcentos(iphone6plusName);
                    iphone6plusName = removerSpecial(iphone6plusName);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/ArquivoMaterial"), iphone6plusName);
                    iphone6plus.SaveAs(path);
                }

                if (icon_image != null && icon_image.ContentLength > 0)
                {
                    fileName3 = Path.GetFileName(finalString + icon_image.FileName).ToString().Replace(" ", "");
                    fileName3 = removerAcentos(fileName3);
                    fileName3 = removerSpecial(fileName3);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/CoverImages"), fileName3);
                    icon_image.SaveAs(path);
                }


                if (fileName3 == "")
                {
                    gestao_campanha.icon_image = null;
                }
                else
                {
                    gestao_campanha.icon_image = "http://solarcoke.solarbr.com.br:32014/Content/assets/CoverImages/" + fileName3;
                }

                if (fileName == "")
                {
                    gestao_campanha.url_foto = null;
                }
                else
                {
                    gestao_campanha.url_foto = "http://solarcoke.solarbr.com.br:32014/Content/assets/CoverImages/" + fileName;
                }

                if (fileName2 == "")
                {
                    gestao_campanha.url_arquivo = null;
                }
                else
                {
                    gestao_campanha.url_arquivo = "http://solarcoke.solarbr.com.br:32014/Content/assets/ArquivoMaterial/" + fileName2;
                }
                if (ldpiName == "")
                {
                    gestao_campanha.url_ldpi = null;
                }
                else
                {
                    gestao_campanha.url_ldpi = "http://solarcoke.solarbr.com.br:32014/Content/assets/ArquivoMaterial/" + ldpiName;
                }

                if (mdpiName == "")
                {
                    gestao_campanha.url_mdpi = null;
                }
                else
                {
                    gestao_campanha.url_mdpi = "http://solarcoke.solarbr.com.br:32014/Content/assets/ArquivoMaterial/" + mdpiName;
                }

                if (hdpiName == "")
                {
                    gestao_campanha.url_hdpi = null;
                }
                else
                {
                    gestao_campanha.url_hdpi = "http://solarcoke.solarbr.com.br:32014/Content/assets/ArquivoMaterial/" + hdpiName;
                }

                if (xhdpiName == "")
                {
                    gestao_campanha.url_xhdpi = null;
                }
                else
                {
                    gestao_campanha.url_xhdpi = "http://solarcoke.solarbr.com.br:32014/Content/assets/ArquivoMaterial/" + xhdpiName;
                }

                if (iphone4Name == "")
                {
                    gestao_campanha.iphone4 = null;
                }
                else
                {
                    gestao_campanha.iphone4 = "http://solarcoke.solarbr.com.br:32014/Content/assets/ArquivoMaterial/" + iphone4Name;
                }

                if (iphone5Name == "")
                {
                    gestao_campanha.iphone5 = null;
                }
                else
                {
                    gestao_campanha.iphone5 = "http://solarcoke.solarbr.com.br:32014/Content/assets/ArquivoMaterial/" + iphone5Name;
                }

                if (iphone6Name == "")
                {
                    gestao_campanha.iphone6 = null;
                }
                else
                {
                    gestao_campanha.iphone6 = "http://solarcoke.solarbr.com.br:32014/Content/assets/ArquivoMaterial/" + iphone6Name;
                }

                if (iphone6plusName == "")
                {
                    gestao_campanha.iphone6plus = null;
                }
                else
                {
                    gestao_campanha.iphone6plus = "http://solarcoke.solarbr.com.br:32014/Content/assets/ArquivoMaterial/" + iphone6plusName;
                }

                gestao_campanha.id_tipocampanha = 2;
                db.gestao_campanha.Add(gestao_campanha);
                db.SaveChanges();
                return RedirectToAction("MaterialPromocional");
            }

            ViewBag.ativa = new SelectList(db.campanha_ativa, "id", "ativa", gestao_campanha.ativa);
            ViewBag.id_tipocampanha = new SelectList(db.campanha_tipo, "id", "nome", gestao_campanha.id_tipocampanha);
            return View(gestao_campanha);
        }


        // GET: /Campanhas/CreateMaterial
        public ActionResult CreateLancamentos()
        {
            ViewBag.ativa = new SelectList(db.campanha_ativa, "id", "ativa");
            ViewBag.id_tipocampanha = new SelectList(db.campanha_tipo, "id", "nome");
            return View();
        }

        // POST: /Campanhas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLancamentos([Bind(Include = "id,titulo,url_foto,descricao_curta,descricao,permissao_img,aderir,url_facebok,url_twitter,url_youtube,id_tipocampanha,url_arquivo,url_ldpi,url_mdpi,url_hdpi,url_xhdpi,link,link_nome,permitir_lista,iphone4,iphone5,iphone6,iphone6plus,base64_foto,ativa,icon_image")] gestao_campanha gestao_campanha, HttpPostedFileBase coverimage, HttpPostedFileBase icon_image)
        {
            var fileName = "";
            var fileName2 = "";

            if (ModelState.IsValid)
            {
                var finalString = GeraCodigo();

                if (coverimage != null && coverimage.ContentLength > 0)
                {
                    fileName = Path.GetFileName(finalString + coverimage.FileName).ToString().Replace(" ", "");
                    fileName = removerAcentos(fileName);
                    fileName = removerSpecial(fileName);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/CoverImages"), fileName);
                    coverimage.SaveAs(path);
                }

                if (fileName == "")
                {
                    gestao_campanha.url_foto = null;
                }
                else
                {
                    gestao_campanha.url_foto = "http://solarcoke.solarbr.com.br:32014/Content/assets/CoverImages/" + fileName;
                }


                if (icon_image != null && icon_image.ContentLength > 0)
                {
                    fileName2 = Path.GetFileName(finalString + icon_image.FileName).ToString().Replace(" ", "");
                    fileName2 = removerAcentos(fileName2);
                    fileName2 = removerSpecial(fileName2);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/CoverImages"), fileName2);
                    icon_image.SaveAs(path);
                }


                if (fileName2 == "")
                {
                    gestao_campanha.icon_image = null;
                }
                else
                {
                    gestao_campanha.icon_image = "http://solarcoke.solarbr.com.br:32014/Content/assets/CoverImages/" + fileName2;
                }
                
                gestao_campanha.id_tipocampanha = 3;
                db.gestao_campanha.Add(gestao_campanha);
                db.SaveChanges();
                return RedirectToAction("Lancamentos");
            }

            ViewBag.ativa = new SelectList(db.campanha_ativa, "id", "ativa", gestao_campanha.ativa);
            ViewBag.id_tipocampanha = new SelectList(db.campanha_tipo, "id", "nome", gestao_campanha.id_tipocampanha);
            return View(gestao_campanha);
        }

        // GET: /Campanhas/Edit/5
        public ActionResult EditCampanha(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gestao_campanha gestao_campanha = db.gestao_campanha.Find(id);
            if (gestao_campanha == null)
            {
                return HttpNotFound();
            }
            ViewBag.ativa = new SelectList(db.campanha_ativa, "id", "ativa", gestao_campanha.ativa);
            ViewBag.id_tipocampanha = new SelectList(db.campanha_tipo, "id", "nome", gestao_campanha.id_tipocampanha);
            return View(gestao_campanha);
        }

        // POST: /Campanhas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCampanha([Bind(Include = "id,titulo,url_foto,descricao_curta,descricao,permissao_img,aderir,url_facebok,url_twitter,url_youtube,id_tipocampanha,url_arquivo,url_ldpi,url_mdpi,url_hdpi,url_xhdpi,link,link_nome,permitir_lista,iphone4,iphone5,iphone6,iphone6plus,base64_foto,ativa,icon_image")] gestao_campanha gestao_campanha, HttpPostedFileBase coverimage, HttpPostedFileBase icon_image)
        {

            var fileName = "";
            var fileName2 = "";

            if (ModelState.IsValid)
            {
                var finalString = GeraCodigo();

                if (coverimage != null && coverimage.ContentLength > 0)
                {
                    fileName = Path.GetFileName(finalString + coverimage.FileName).ToString().Replace(" ", "");
                    fileName = removerAcentos(fileName);
                    fileName = removerSpecial(fileName);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/CoverImages"), fileName);
                    coverimage.SaveAs(path);
                }

                if (fileName == "")
                {
                    gestao_campanha.url_foto = gestao_campanha.url_foto;
                }
                else
                {
                    gestao_campanha.url_foto = "http://solarcoke.solarbr.com.br:32014/Content/assets/CoverImages/" + fileName;
                }


                if (icon_image != null && icon_image.ContentLength > 0)
                {
                    fileName2 = Path.GetFileName(finalString + icon_image.FileName).ToString().Replace(" ", "");
                    fileName2 = removerAcentos(fileName2);
                    fileName2 = removerSpecial(fileName2);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/CoverImages"), fileName2);
                    icon_image.SaveAs(path);
                }


                if (fileName2 == "")
                {
                    gestao_campanha.icon_image = gestao_campanha.icon_image;
                }
                else
                {
                    gestao_campanha.icon_image = "http://solarcoke.solarbr.com.br:32014/Content/assets/CoverImages/" + fileName2;
                }

                db.Entry(gestao_campanha).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Campanha");
            }
            ViewBag.ativa = new SelectList(db.campanha_ativa, "id", "ativa", gestao_campanha.ativa);
            ViewBag.id_tipocampanha = new SelectList(db.campanha_tipo, "id", "nome", gestao_campanha.id_tipocampanha);
            return View(gestao_campanha);
        }


        // GET: /Campanhas/Edit/5
        public ActionResult EditMaterial(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gestao_campanha gestao_campanha = db.gestao_campanha.Find(id);
            if (gestao_campanha == null)
            {
                return HttpNotFound();
            }
            ViewBag.ativa = new SelectList(db.campanha_ativa, "id", "ativa", gestao_campanha.ativa);
            ViewBag.id_tipocampanha = new SelectList(db.campanha_tipo, "id", "nome", gestao_campanha.id_tipocampanha);
            return View(gestao_campanha);
        }

        // POST: /Campanhas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMaterial([Bind(Include = "id,titulo,url_foto,descricao_curta,descricao,permissao_img,aderir,url_facebok,url_twitter,url_youtube,id_tipocampanha,url_arquivo,url_ldpi,url_mdpi,url_hdpi,url_xhdpi,link,link_nome,permitir_lista,iphone4,iphone5,iphone6,iphone6plus,base64_foto,ativa,icon_image")] gestao_campanha gestao_campanha, HttpPostedFileBase coverimage, HttpPostedFileBase icon_image, HttpPostedFileBase arquivo, HttpPostedFileBase ldpi, HttpPostedFileBase mdpi, HttpPostedFileBase hdpi, HttpPostedFileBase xhdpi, HttpPostedFileBase iphone4, HttpPostedFileBase iphone5, HttpPostedFileBase iphone6, HttpPostedFileBase iphone6plus)
        {
            var fileName = "";
            var fileName2 = "";
            var fileName3 = "";
            var ldpiName = "";
            var mdpiName = "";
            var hdpiName = "";
            var xhdpiName = "";
            var iphone4Name = "";
            var iphone5Name = "";
            var iphone6Name = "";
            var iphone6plusName = "";

            if (ModelState.IsValid)
            {
                var finalString = GeraCodigo();

                if (coverimage != null && coverimage.ContentLength > 0)
                {
                    fileName = Path.GetFileName(finalString + coverimage.FileName).ToString().Replace(" ", "");
                    fileName = removerAcentos(fileName);
                    fileName = removerSpecial(fileName);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/CoverImages"), fileName);
                    coverimage.SaveAs(path);
                }

                if (arquivo != null && arquivo.ContentLength > 0)
                {
                    fileName2 = Path.GetFileName(finalString + arquivo.FileName).ToString().Replace(" ", "");
                    fileName2 = removerAcentos(fileName2);
                    fileName2 = removerSpecial(fileName2);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/ArquivoMaterial"), fileName2);
                    arquivo.SaveAs(path);
                }

                if (ldpi != null && ldpi.ContentLength > 0)
                {
                    ldpiName = Path.GetFileName("ldpi" + finalString + ldpi.FileName).ToString().Replace(" ", "");
                    ldpiName = removerAcentos(ldpiName);
                    ldpiName = removerSpecial(ldpiName);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/ArquivoMaterial"), ldpiName);
                    ldpi.SaveAs(path);
                }

                if (mdpi != null && mdpi.ContentLength > 0)
                {
                    mdpiName = Path.GetFileName("mdpi" + finalString + mdpi.FileName).ToString().Replace(" ", "");
                    mdpiName = removerAcentos(mdpiName);
                    mdpiName = removerSpecial(mdpiName);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/ArquivoMaterial"), mdpiName);
                    mdpi.SaveAs(path);
                }

                if (hdpi != null && hdpi.ContentLength > 0)
                {
                    hdpiName = Path.GetFileName("hdpi" + finalString + hdpi.FileName).ToString().Replace(" ", "");
                    hdpiName = removerAcentos(hdpiName);
                    hdpiName = removerSpecial(hdpiName);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/ArquivoMaterial"), hdpiName);
                    hdpi.SaveAs(path);
                }

                if (xhdpi != null && xhdpi.ContentLength > 0)
                {
                    xhdpiName = Path.GetFileName("xhdpi" + finalString + xhdpi.FileName).ToString().Replace(" ", "");
                    xhdpiName = removerAcentos(xhdpiName);
                    xhdpiName = removerSpecial(xhdpiName);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/ArquivoMaterial"), xhdpiName);
                    xhdpi.SaveAs(path);
                }

                if (iphone4 != null && iphone4.ContentLength > 0)
                {
                    iphone4Name = Path.GetFileName("iphone4" + finalString + iphone4.FileName).ToString().Replace(" ", "");
                    iphone4Name = removerAcentos(iphone4Name);
                    iphone4Name = removerSpecial(iphone4Name);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/ArquivoMaterial"), iphone4Name);
                    iphone4.SaveAs(path);
                }

                if (iphone5 != null && iphone5.ContentLength > 0)
                {
                    iphone5Name = Path.GetFileName("iphone5" + finalString + iphone5.FileName).ToString().Replace(" ", "");
                    iphone5Name = removerAcentos(iphone5Name);
                    iphone5Name = removerSpecial(iphone5Name);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/ArquivoMaterial"), iphone5Name);
                    iphone5.SaveAs(path);
                }

                if (iphone6 != null && iphone6.ContentLength > 0)
                {
                    iphone6Name = Path.GetFileName("iphone6" + finalString + iphone6.FileName).ToString().Replace(" ", "");
                    iphone6Name = removerAcentos(iphone6Name);
                    iphone6Name = removerSpecial(iphone6Name);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/ArquivoMaterial"), iphone6Name);
                    iphone6.SaveAs(path);
                }

                if (iphone6plus != null && iphone6plus.ContentLength > 0)
                {
                    iphone6plusName = Path.GetFileName("iphone6plus" + finalString + iphone6plus.FileName).ToString().Replace(" ", "");
                    iphone6plusName = removerAcentos(iphone6plusName);
                    iphone6plusName = removerSpecial(iphone6plusName);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/ArquivoMaterial"), iphone6plusName);
                    iphone6plus.SaveAs(path);
                }

                if (icon_image != null && icon_image.ContentLength > 0)
                {
                    fileName3 = Path.GetFileName(finalString + icon_image.FileName).ToString().Replace(" ", "");
                    fileName3 = removerAcentos(fileName3);
                    fileName3 = removerSpecial(fileName3);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/CoverImages"), fileName3);
                    icon_image.SaveAs(path);
                }


                if (fileName3 == "")
                {
                    gestao_campanha.icon_image = gestao_campanha.icon_image;
                }
                else
                {
                    gestao_campanha.icon_image = "http://solarcoke.solarbr.com.br:32014/Content/assets/CoverImages/" + fileName3;
                }

                if (fileName == "")
                {
                    gestao_campanha.url_foto = gestao_campanha.url_foto;
                }
                else
                {
                    gestao_campanha.url_foto = "http://solarcoke.solarbr.com.br:32014/Content/assets/CoverImages/" + fileName;
                }

                if (fileName2 == "")
                {
                    gestao_campanha.url_arquivo = gestao_campanha.url_arquivo;
                }
                else
                {
                    gestao_campanha.url_arquivo = "http://solarcoke.solarbr.com.br:32014/Content/assets/ArquivoMaterial/" + fileName2;
                }

                if (ldpiName == "")
                {
                    gestao_campanha.url_ldpi = gestao_campanha.url_ldpi;
                }

                else
                {
                    gestao_campanha.url_ldpi = "http://solarcoke.solarbr.com.br:32014/Content/assets/ArquivoMaterial/" + ldpiName;
                }

                if (mdpiName == "")
                {
                    gestao_campanha.url_mdpi = gestao_campanha.url_mdpi;
                }
                else
                {
                    gestao_campanha.url_mdpi = "http://solarcoke.solarbr.com.br:32014/Content/assets/ArquivoMaterial/" + mdpiName;
                }

                if (hdpiName == "")
                {
                    gestao_campanha.url_hdpi = gestao_campanha.url_hdpi;
                }
                else
                {
                    gestao_campanha.url_hdpi = "http://solarcoke.solarbr.com.br:32014/Content/assets/ArquivoMaterial/" + hdpiName;
                }

                if (xhdpiName == "")
                {
                    gestao_campanha.url_xhdpi = gestao_campanha.url_xhdpi;
                }
                else
                {
                    gestao_campanha.url_xhdpi = "http://solarcoke.solarbr.com.br:32014/Content/assets/ArquivoMaterial/" + xhdpiName;
                }


                if (iphone4Name == "")
                {
                    gestao_campanha.iphone4 = gestao_campanha.iphone4;
                }
                else
                {
                    gestao_campanha.iphone4 = "http://solarcoke.solarbr.com.br:32014/Content/assets/ArquivoMaterial/" + iphone4Name;
                }

                if (iphone5Name == "")
                {
                    gestao_campanha.iphone5 = gestao_campanha.iphone5;
                }
                else
                {
                    gestao_campanha.iphone5 = "http://solarcoke.solarbr.com.br:32014/Content/assets/ArquivoMaterial/" + iphone5Name;
                }

                if (iphone6Name == "")
                {
                    gestao_campanha.iphone6 = gestao_campanha.iphone6;
                }
                else
                {
                    gestao_campanha.iphone6 = "http://solarcoke.solarbr.com.br:32014/Content/assets/ArquivoMaterial/" + iphone6Name;
                }

                if (iphone6plusName == "")
                {
                    gestao_campanha.iphone6plus = gestao_campanha.iphone6plus;
                }
                else
                {
                    gestao_campanha.iphone6plus = "http://solarcoke.solarbr.com.br:32014/Content/assets/ArquivoMaterial/" + iphone6plusName;
                }
                db.Entry(gestao_campanha).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MaterialPromocional");
            }
            ViewBag.ativa = new SelectList(db.campanha_ativa, "id", "ativa", gestao_campanha.ativa);
            ViewBag.id_tipocampanha = new SelectList(db.campanha_tipo, "id", "nome", gestao_campanha.id_tipocampanha);
            return View(gestao_campanha);
        }


        // GET: /Campanhas/Edit/5
        public ActionResult EditLancamentos(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gestao_campanha gestao_campanha = db.gestao_campanha.Find(id);
            if (gestao_campanha == null)
            {
                return HttpNotFound();
            }
            ViewBag.ativa = new SelectList(db.campanha_ativa, "id", "ativa", gestao_campanha.ativa);
            ViewBag.id_tipocampanha = new SelectList(db.campanha_tipo, "id", "nome", gestao_campanha.id_tipocampanha);
            return View(gestao_campanha);
        }

        // POST: /Campanhas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditLancamentos([Bind(Include = "id,titulo,url_foto,descricao_curta,descricao,permissao_img,aderir,url_facebok,url_twitter,url_youtube,id_tipocampanha,url_arquivo,url_ldpi,url_mdpi,url_hdpi,url_xhdpi,link,link_nome,permitir_lista,iphone4,iphone5,iphone6,iphone6plus,base64_foto,ativa,icon_image")] gestao_campanha gestao_campanha, HttpPostedFileBase coverimage, HttpPostedFileBase icon_image)
        {
            var fileName = "";
            var fileName2 = "";

            if (ModelState.IsValid)
            {
                var finalString = GeraCodigo();

                if (coverimage != null && coverimage.ContentLength > 0)
                {
                    fileName = Path.GetFileName(finalString + coverimage.FileName).ToString().Replace(" ", "");
                    fileName = removerAcentos(fileName);
                    fileName = removerSpecial(fileName);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/CoverImages"), fileName);
                    coverimage.SaveAs(path);
                }

                if (fileName == "")
                {
                    gestao_campanha.url_foto = gestao_campanha.url_foto;
                }
                else
                {
                    gestao_campanha.url_foto = "http://solarcoke.solarbr.com.br:32014/Content/assets/CoverImages/" + fileName;
                }

                if (icon_image != null && icon_image.ContentLength > 0)
                {
                    fileName2 = Path.GetFileName(finalString + icon_image.FileName).ToString().Replace(" ", "");
                    fileName2 = removerAcentos(fileName2);
                    fileName2 = removerSpecial(fileName2);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/CoverImages"), fileName2);
                    icon_image.SaveAs(path);
                }


                if (fileName2 == "")
                {
                    gestao_campanha.icon_image = gestao_campanha.icon_image;
                }
                else
                {
                    gestao_campanha.icon_image = "http://solarcoke.solarbr.com.br:32014/Content/assets/CoverImages/" + fileName2;
                }
                db.Entry(gestao_campanha).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Lancamentos");
            }
            ViewBag.ativa = new SelectList(db.campanha_ativa, "id", "ativa", gestao_campanha.ativa);
            ViewBag.id_tipocampanha = new SelectList(db.campanha_tipo, "id", "nome", gestao_campanha.id_tipocampanha);
            return View(gestao_campanha);
        }

        // GET: /Campanhas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gestao_campanha gestao_campanha = db.gestao_campanha.Find(id);
            if (gestao_campanha == null)
            {
                return HttpNotFound();
            }
            return View(gestao_campanha);
        }

        // POST: /Campanhas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            gestao_campanha gestao_campanha = db.gestao_campanha.Find(id);
            db.gestao_campanha.Remove(gestao_campanha);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.PathAndQuery);
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
