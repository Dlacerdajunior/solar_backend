using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SolarEP.Models;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net.Http;
using System.Xml;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Globalization;
using System.IO;
using System.Drawing;

namespace SolarEP.Controllers
{
    public class MobileController : Controller
    {
        private SolardbEntities db = new SolardbEntities();
        
        
        public ActionResult LogAcesso(string tela, string kunnr, string cp, int? id_loja)
        {
            var log_acesso = new log_acesso();

            int? loja = 0;
            if (kunnr != null && kunnr != "")
            {
                var loja2 = db.lojas.Where(a => a.kunnr == kunnr).FirstOrDefault();
                loja = loja2.id;
            }
            if (cp != null && cp != "")
            {
                var loja2 = db.lojas.Where(a => a.cnpj_loja == cp).FirstOrDefault();             
                loja = loja2.id;
            }
            if (id_loja != 0)
            {
                var loja2 = db.lojas.Where(a => a.id == id_loja).FirstOrDefault();
                loja = loja2.id;
            }
            if (loja == 0)
            {
                loja = null;
            }

            log_acesso.id_loja = loja;
            log_acesso.datalog = DateTime.Now;
            log_acesso.tela = tela;

            db.log_acesso.Add(log_acesso);
            db.SaveChanges();
            
            return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
        }

        // GET: /Mobile/Insert tabela solicitacao
        public ActionResult InsertSolicitacaoMobile([Bind(Include = "id,numero_comodato,titulo,descricao_problema,id_loja")] solicitacao solicitacao)
        {
            var id_loja = solicitacao.id_loja.ToString();
            if (solicitacao.numero_comodato == null || solicitacao.titulo == null || solicitacao.descricao_problema == null || id_loja == "" || solicitacao.numero_comodato == "" || solicitacao.titulo == "" || solicitacao.descricao_problema == "")
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                db.solicitacao.Add(solicitacao);
                db.SaveChanges();

                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListaVisitaEquipamento(string equipamento)
        {

            var rsLista = db.visita.Where(a => a.id_equipamento == equipamento);

            if (rsLista.Count() == 0)
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }

            var listavisita = new List<dynamic>();
            var listafoto = new List<dynamic>();
            foreach (var item in rsLista)
            {
                var data2 = "";
                if (item.data != null)
                {
                    item.data = item.data.Replace(" 12:00:00 AM", "");

                    string[] arrDate = item.data.Split('/');

                    string month = arrDate[0].ToString();
                    string day = arrDate[1].ToString();
                    string year = arrDate[2].ToString();

                    data2 = day + "/" + month + "/" + year;
                }
                
                var atualiza = "";
                if (item.atualiza != null)
                {
                    item.atualiza = item.atualiza.Replace(" 12:00:00 AM", "");

                    string[] arrDate = item.atualiza.Split('/');

                    string month = arrDate[0].ToString();
                    string day = arrDate[1].ToString();
                    string year = arrDate[2].ToString();

                    atualiza = day + "/" + month + "/" + year;
                }
                
                listafoto = new List<dynamic>();

                var rsFoto = db.visita_fotos.Where(a => a.id_visita == item.id);

                if (rsFoto.Count() != 0)
                {
                    foreach (var item2 in rsFoto)
                    {
                        var data3 = "";
                        if (item2.data != null)
                        {
                            item2.data = item.data.Replace(" 12:00:00 AM", "");

                            string[] arrDate = item2.data.Split('/');

                            string month = arrDate[0].ToString();
                            string day = arrDate[1].ToString();
                            string year = arrDate[2].ToString();

                            data3 = day + "/" + month + "/" + year;
                        }

                        listafoto.Add(new
                        {
                            id = item2.id,
                            id_visita = item2.id_visita,
                            url_foto = item2.url_foto,
                            data = data3
                        });
                    }
                }

                listavisita.Add(new
                {
                    id = item.id,
                    id_loja = item.id_loja,
                    id_status = item.id_status,
                    nome_status = item.visita_status.nome,
                    titulo = item.titulo,
                    descricao = item.descricao,
                    contato = item.contato,
                    data = data2,
                    id_equipamento = item.id_equipamento,
                    obs_admin = item.obs_admin,
                    EQFNR = item.EQFNR,
                    TYPBZ = item.TYPBZ,
                    EQUNR = item.EQUNR,
                    HERST = item.HERST,
                    tag_equipamento = item.tag_equipamento,
                    atualiza = atualiza,
                    previsao = item.previsao,
                    listafoto = listafoto

                });

            }


            return Json(listavisita, JsonRequestBehavior.AllowGet);
        }


        //Lista Visita Técnica por ID
        public ActionResult JsonListaVisita(int id)
        {
            var rsVisita = db.visita.Where(a => a.id == id).FirstOrDefault();

            if (rsVisita == null)
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }

                var data2 = "";
                if (rsVisita.data != null)
                {
                    rsVisita.data = rsVisita.data.Replace(" 12:00:00 AM", "");

                    string[] arrDate = rsVisita.data.Split('/');

                    string month = arrDate[0].ToString();
                    string day = arrDate[1].ToString();
                    string year = arrDate[2].ToString();

                    data2 = day + "/" + month + "/" + year;
                }
                
                var atualiza = "";
                if (rsVisita.atualiza != null)
                {
                    rsVisita.atualiza = rsVisita.atualiza.Replace(" 12:00:00 AM", "");

                    string[] arrDate = rsVisita.atualiza.Split('/');

                    string month = arrDate[0].ToString();
                    string day = arrDate[1].ToString();
                    string year = arrDate[2].ToString();

                    atualiza = day + "/" + month + "/" + year;
                }
                
                var listafoto = new List<dynamic>();
                var rsFoto = db.visita_fotos.Where(a => a.id_visita == rsVisita.id);
                if (rsFoto.Count() != 0)
                {
                    foreach (var item2 in rsFoto)
                    {
                        var data3 = "";
                        if (item2.data != null)
                        {
                            item2.data = rsVisita.data.Replace(" 12:00:00 AM", "");

                            string[] arrDate = item2.data.Split('/');

                            string month = arrDate[0].ToString();
                            string day = arrDate[1].ToString();
                            string year = arrDate[2].ToString();

                            data3 = day + "/" + month + "/" + year;
                        }

                        listafoto.Add(new
                        {
                            id = item2.id,
                            id_visita = item2.id_visita,
                            url_foto = item2.url_foto,
                            data = data3
                        });
                    }
                }

            var listavisita = new List<dynamic>();
                listavisita.Add(new
                {
                    id = rsVisita.id,
                    id_loja = rsVisita.id_loja,
                    id_status = rsVisita.id_status,
                    nome_status = rsVisita.visita_status.nome,
                    titulo = rsVisita.titulo,
                    descricao = rsVisita.descricao,
                    contato = rsVisita.contato,
                    data = data2,
                    id_equipamento = rsVisita.id_equipamento,
                    obs_admin = rsVisita.obs_admin,
                    EQFNR = rsVisita.EQFNR,
                    TYPBZ = rsVisita.TYPBZ,
                    EQUNR = rsVisita.EQUNR,
                    HERST = rsVisita.HERST,
                    tag_equipamento = rsVisita.tag_equipamento,
                    atualiza = atualiza,
                    previsao = rsVisita.previsao,
                    listafoto = listafoto
                });

                return Json(listavisita, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InsertVisitaMobile([Bind(Include = "id_loja, id_status, titulo, descricao, contato, data, id_equipamento, EQFNR, TYPBZ, EQUNR, HERST, atualiza, tag_equipamento")] visita visita)
        {
            if (visita.id_loja == null || visita.titulo == null || visita.id_status == null || visita.id_equipamento == null || visita.tag_equipamento == null)
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                visita.data = DateTime.Now.Date.ToString();
                visita.atualiza = DateTime.Now.Date.ToString();
                db.visita.Add(visita);
                db.SaveChanges();

                var adm = db.configuracao.Where(a => a.nome == "VisitaAdministrador").FirstOrDefault();
                var emailadm = adm.valor;

                if (emailadm != null)
                {
                        var email = new MailMessage();
                        var message = new MailMessage();
                        var body = "<p> {0} {1}</p><p> Solar </p><p>{2}</p>";

                        message.To.Add(new MailAddress(emailadm));  // replace with valid value 
                        message.From = new MailAddress("cadastroclientessolar@solarbr.com.br");  // replace with valid value
                        message.Subject = "Solar - Nova Visita.";
                        message.Body = string.Format(body, "", "", "Há uma nova solicitação de visita.");
                        message.IsBodyHtml = true;
                    
                    try 
	                {	        
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

                            return Json(new { status = 1, visita.id, email = 1 }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception)
                    {
                        return Json(new { status = 1, visita.id, email = 0 }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { status = 1, visita.id, email = 0 }, JsonRequestBehavior.AllowGet);                    
                }
            }
        }




        public void SendMail()
        {
            var recipient = "programacao02@mobitap.com.br";
            var subject = "teste";
            var message = "teste - teste";
            var sender = "cadastroclientessolar@solarbr.com.br";


            SmtpClient client = new SmtpClient("smtp.office365.com");

            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential("cadastroclientessolar@solarbr.com.br", "1q2w3e4r!Q@W#E$R");
            client.EnableSsl = true;
            client.Credentials = credentials;

            try
            {
                var mail = new MailMessage(sender.Trim(), recipient.Trim());
                mail.Subject = subject;
                mail.Body = message;
                client.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //throw ex;
            }
        }

        public ActionResult MostraBadges()
        {

            var rsCampanhas = db.gestao_campanha;

            var listacampanhas = new List<dynamic>();

            var item = rsCampanhas.Where(a => a.id_tipocampanha == 1).OrderByDescending(a => a.id).First();
            
                listacampanhas.Add(new
                {
                    id = item.id,
                    url_foto = item.url_foto
                });
            

            var listamateriais = new List<dynamic>();

            var item2 = rsCampanhas.Where(a => a.id_tipocampanha == 2).OrderByDescending(a => a.id).First();
            
                listamateriais.Add(new
                {
                    id = item2.id,
                    url_foto = item2.url_foto
                });
            

            var listalancamentos = new List<dynamic>();

            var item3 = rsCampanhas.Where(a => a.id_tipocampanha == 3).OrderByDescending(a => a.id).First();
            
                listalancamentos.Add(new
                {
                    id = item3.id,
                    url_foto = item3.url_foto
                });
            

            return Json(new { listacampanhas, listamateriais, listalancamentos }, JsonRequestBehavior.AllowGet);
        }


        //Change Status
        public ActionResult TrocaStatusMobile(int? id, int? status)
        {
            var loja = db.lojas.FirstOrDefault(l => l.id == id);

            loja.status_loja = status;
            db.SaveChanges();

            return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
        }

        //Change Visita
        public ActionResult UpdateVisitaMobile(int? id, string titulo, string descricao, string contato, string tag_equipamento)
        {
            var visita = db.visita.FirstOrDefault(l => l.id == id);

            visita.titulo = titulo;
            visita.descricao = descricao;
            visita.contato = contato;
            visita.tag_equipamento = tag_equipamento;
            db.SaveChanges();

            return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DeleteVisitaFoto(int id)
        {
            visita_fotos visita_fotos = db.visita_fotos.Find(id);
            if (visita_fotos == null)
            {
            return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);                
            }
            else
            {
            db.visita_fotos.Remove(visita_fotos);
            db.SaveChanges();

            return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }

        }

        //Change Senha
        public ActionResult TrocaSenhaMobile(int? id, string senha)
        {
            var loja = db.lojas.FirstOrDefault(l => l.id == id);
            
            if (loja.status_loja != 5)
            {

            loja.senha = MD5Hash(senha);
            loja.status_loja = 5;
            db.SaveChanges();


            var email = new MailMessage();
            var message = new MailMessage();
            var body = "<p>Olá, {0}</p>  <p>Obrigado por se registrar no App Solar. Para ativar a conta basta visitar o endereço fornecido abaixo, confirmando assim o seu endereço de email.</p> <p>Para completar o registro visite o link:</p> <p> http://solarcoke.solarbr.com.br:32014/SolarActivate?code=" + loja.email_codigo + "</p> <p> * Caso o link neste email não funcione, por favor abra o seu navegador, copie o link na área de endereço e aperte enter.</p> <p> Atenciosamente,<br> SolarBr </p>";

            message.To.Add(new MailAddress(loja.email_contato));  // replace with valid value 
            // message.To.Add(new MailAddress(lojas.email_contato));  // replace with valid value 
            message.From = new MailAddress("cadastroclientessolar@solarbr.com.br");  // replace with valid value
            message.Subject = "Confirme seu endereço de e-mail";
            message.Body = string.Format(body, loja.razao_social_loja);
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    //                               UserName = "programacao02@mobitap.com.br",  // replace with valid value
                    UserName = "schcadastrocli@solarbr.com.br",  // replace with valid value
                    //    Password = "flavio@123"  // replace with valid value
                    Password = "1q2w3e4r!Q@W#E$R"  // replace with valid value
                };
                smtp.Credentials = credential;
                //                           smtp.Host = "smtp.gmail.com";
                smtp.Host = "smtp.office365.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(message);
            }
                
            }

            return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
        }



        //Esqueci minha senha Mobile
        public ActionResult EsqueciMinhaSenhaMobile(string cp)
        {
            if (cp == "")
            {
                return Json(new { status = 0, msg = "Informe CPF/CNPJ" }, JsonRequestBehavior.AllowGet);
            }

            var novasenha = GeraCodigo();
            var rsLoja = db.lojas.Where(x => x.cnpj_loja == cp).FirstOrDefault();
            var rsLoja2 = db.lojas.Where(x => x.cnpj_loja == cp).Count();
            if (rsLoja2 == 0)
            {
                return Json(new { status = 0, msg = "Loja não encontrada" }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                var email = new MailMessage();
                var message = new MailMessage();
               // var body = "<p>A nova senha é: {0}</p><p>Campo util</p><p>{2}</p>";
                var body = "<p>Olá, {0}</p> <p>Sua nova senha é: {1}  </p>  <p> Atenciosamente,<br> SolarBr </p>";

                message.To.Add(new MailAddress(rsLoja.email_contato));  // replace with valid value 
                message.From = new MailAddress("cadastroclientessolar@solarbr.com.br");  // replace with valid value
                message.Subject = "SolarBr - Sua nova Senha";
                message.Body = string.Format(body, rsLoja.razao_social_loja, novasenha);
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

            var senhamd5 = MD5Hash(novasenha);
            rsLoja.senha = senhamd5;
            db.SaveChanges();
            return Json(new { status = 1, msg = "Sucesso." }, JsonRequestBehavior.AllowGet);

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


        //Insert tabela lojas campos obrigatorios
        public ActionResult InsertLojasMobile([Bind(Include = "cnpj_loja, razao_social_loja, loja_ativa, nome_contato, email_contato, id, lattext, longtext, senha, status_loja, email_codigo, kunnr")]lojas lojas)
        {
           // var finalString = "123";
             var finalString = GeraCodigo();

            if (lojas.email_contato == null || lojas.razao_social_loja == null || lojas.cnpj_loja == null)
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (lojas.senha != null)
                {
                    lojas.senha = MD5Hash(lojas.senha);
                }
                lojas.cnpj_loja = lojas.cnpj_loja;
                lojas.created = DateTime.Now;
                lojas.razao_social_loja = lojas.razao_social_loja;
                lojas.email_codigo = finalString;
                lojas.status_loja = 4;
                db.lojas.Add(lojas);
                db.SaveChanges();

                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
        }

        //Lista tabela solicitacao
        public ActionResult JsonListaSolicitacao()
        {
            var solicitacao = db.solicitacao;
            var listasolicitacao = new List<dynamic>();

            foreach (var item in solicitacao)
            {
                listasolicitacao.Add(new
                {
                    id = item.id,
                    numero_comodato = item.numero_comodato,
                    titulo = item.titulo,
                    descricao_problema = item.descricao_problema,
                    id_loja = item.id_loja,
                });
            }

            return Json(listasolicitacao, JsonRequestBehavior.AllowGet);
        }

        //Lista Array backgrounds
        public ActionResult JsonListaBackgrounds()
        {
            var rsCampanhas = db.gestao_campanha;

            var checkitem = rsCampanhas.Where(a => a.id_tipocampanha == 1).Count();
            var icon1 = "";
            if (checkitem == 0)
            {
                icon1 = "http://solarcoke.solarbr.com.br:32014/Content/assets/images/ico-campanhas.png";
            }
            else
            {
                var item = rsCampanhas.Where(a => a.id_tipocampanha == 1).OrderByDescending(a => a.id).First();
                icon1 = item.icon_image;
            }

            var checkitem2 = rsCampanhas.Where(a => a.id_tipocampanha == 2).Count();
            var icon2 = "";
            if (checkitem2 == 0)
            {
                icon2 = "http://solarcoke.solarbr.com.br:32014/Content/assets/images/ico-matpromo.png";
                
            }
            else
            {
                var item2 = rsCampanhas.Where(a => a.id_tipocampanha == 2).OrderByDescending(a => a.id).First();
                icon2 = item2.icon_image;
            }

            var checkitem3 = rsCampanhas.Where(a => a.id_tipocampanha == 3).Count();
            var icon3 = "";
            if (checkitem3 == 0)
            {
                icon3 = "http://solarcoke.solarbr.com.br:32014/Content/assets/images/ico-lancamentos.png";
            }
            else
            {
                var item3 = rsCampanhas.Where(a => a.id_tipocampanha == 3).OrderByDescending(a => a.id).First();
                icon3 = item3.icon_image; 
            }
            
/*
            var listacampanhas = new List<dynamic>();

            if (checkitem == 0)
            {
                listacampanhas.Add(new
                {
                    icon_image = "http://solar.mobitapdev.net/Content/assets/images/nophotocamanha.png"
                });
            }
            else
            {
                listacampanhas.Add(new
                {
                    icon_image = item.icon_image
                });
            }

            
            var listamateriais = new List<dynamic>();



            if (checkitem2 == 0)
            {
                listamateriais.Add(new
                {
                    icon_image = "http://solar.mobitapdev.net/Content/assets/images/nophotocamanha.png"
                });
            }
            else
            {
                listamateriais.Add(new
                {
                    icon_image = item2.icon_image
                });
            }

            var listalancamentos = new List<dynamic>();



            if (checkitem3 == 0)
            {
                listalancamentos.Add(new
                {
                    icon_image = "http://solar.mobitapdev.net/Content/assets/images/nophotocamanha.png"
                });
            }
            else
            {
                listalancamentos.Add(new
                {
                    icon_image = item3.icon_image
                });
            } */


            var checkback = db.backgrounds.Count();
            var bg = "";
            var back = db.backgrounds.OrderBy(x => Guid.NewGuid()).Take(1);
            var listaback = new List<dynamic>();
            if (checkback == 0)
            {
                bg = "http://solarcoke.solarbr.com.br:32014/Content/assets/Backgrounds/ico-backgroud.jpg";                
            }
            else
            {
                foreach (var item4 in back.ToArray())
                {
                    bg = item4.url_background;
                }
            }

            listaback.Add(new
            {
                backgroundlancamento = icon3,
                backgroundmaterial = icon2,
                backgroundcampanha = icon1,
                background = bg
            });

            /*
            var back = db.backgrounds.OrderBy(x => Guid.NewGuid()).Take(1);
            var listaback = new List<dynamic>();

            foreach (var item4 in back.ToArray())
            {
                listaback.Add(new
                {
                    backgroundlancamento = icon3,
                    backgroundmaterial = icon2,
                    backgroundcampanha = icon1,
                    background = item4.url_background
                });
            } 
            */

            return Json(listaback, JsonRequestBehavior.AllowGet);
        }

        //Lista tabela solicitacao
        public ActionResult JsonListaChecklist(string codigo)
        {
            var checklist = db.checklist_equipamento.Where(a => a.codigo == codigo);
            var checkcheck = db.checklist_equipamento.Where(a => a.codigo == codigo).Count();

            var checklistpadrao = db.checklist_equipamento.Where(a => a.codigo == "PADRAO");

            if (checkcheck == 0)
            {
                var listapadrao = new List<dynamic>();

                foreach (var item in checklistpadrao)
                {
                    listapadrao.Add(new
                    {
                        id = item.id,
                        texto = item.texto
                    });
                }

                return Json(listapadrao, JsonRequestBehavior.AllowGet);
            }


            var listacheck = new List<dynamic>();

            foreach (var item in checklist)
            {
                listacheck.Add(new
                {
                    id = item.id,
                    texto = item.texto
                });
            }

            return Json(listacheck, JsonRequestBehavior.AllowGet);

        }

        //Lista Loja
        public ActionResult JsonInfoLojas(int? id)
        {
            var infolojas = db.lojas.FirstOrDefault(l => l.id == id);
            var listainfo = new List<dynamic>();

            listainfo.Add(new
            {
                cnpj_loja = infolojas.cnpj_loja,
                razao_social_loja = infolojas.razao_social_loja,
                loja_ativa = infolojas.loja_ativa,
                nome_contato = infolojas.nome_contato,
                email_contato = infolojas.email_contato,
                id = infolojas.id,
                lattext = infolojas.lattext,
                longtext = infolojas.longtext,
                senha = infolojas.senha,
                status_loja = infolojas.status_loja,
                email_codigo = infolojas.email_codigo

            });


            return Json(listainfo, JsonRequestBehavior.AllowGet);
        }

        //Login Mobile Json
        public ActionResult JsonLoginMobile(string cp, string senha)
        {
            if (cp == "" || senha == "")
            {
                return Json(new { status_loja = 0, msg = "Cpf/Cnpj e senha precisam ser preenchidos." }, JsonRequestBehavior.AllowGet);
            }

            var senhamd5 = MD5Hash(senha);

            var rsLoja = db.lojas.Where(lojas => lojas.cnpj_loja == cp && lojas.senha == senhamd5).FirstOrDefault();

            if (rsLoja == null)
            {
                return Json(new { status_loja = 0, msg = "Loja não encontrada." }, JsonRequestBehavior.AllowGet);
            }
            if (rsLoja != null && rsLoja.status_loja == 1)
            {
                var insertlog = LogAcesso("Login", "", rsLoja.cnpj_loja.ToString(),0);

                return Json(new
                {
                    id = rsLoja.id,
                    status_loja = rsLoja.status_loja,
                    kunnr = rsLoja.kunnr,
                    razao_social_loja = rsLoja.razao_social_loja

                }, JsonRequestBehavior.AllowGet);
            }
            if (rsLoja != null && rsLoja.status_loja == 2)
            {
                return Json(new
                {
                    id = rsLoja.id,
                    status_loja = rsLoja.status_loja

                }, JsonRequestBehavior.AllowGet);
            }
           /* if (rsLoja != null && rsLoja.status_loja == 3)
            {
                if (rsLoja.email_contato == null)
                {
                    return Json(new { status_loja = 0, msg = "Loja não tem um e-mail cadastrado" }, JsonRequestBehavior.AllowGet);
                }

                var email = new MailMessage();
                var message = new MailMessage();
                var body = "<p>Token: {0} {1}</p><p>Campo util</p><p>{2}</p>";

                message.To.Add(new MailAddress(rsLoja.email_contato));  // replace with valid value 
                message.From = new MailAddress("cadastroclientessolar@solarbr.com.br");  // replace with valid value
                message.Subject = "Solar - Seu Código (token)";
                message.Body = string.Format(body, rsLoja.email_codigo, "", "Solicitar um texto para o email");
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

                    return Json(new
                    {
                        id = rsLoja.id,
                        status_loja = rsLoja.status_loja,
                        email_codigo = rsLoja.email_codigo

                    }, JsonRequestBehavior.AllowGet);
                }
            } */
            if (rsLoja != null && rsLoja.status_loja == 4)
            {
                return Json(new
                {
                    id = rsLoja.id,
                    status_loja = rsLoja.status_loja

                }, JsonRequestBehavior.AllowGet);
            }

            if (rsLoja != null && rsLoja.status_loja == 5)
            {
                return Json(new
                {
                    id = rsLoja.id,
                    status_loja = rsLoja.status_loja

                }, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        //Login Mobile Json OnlyCP
        public ActionResult JsonLoginMobileCP(string cp, [Bind(Include = "cnpj_loja, razao_social_loja, loja_ativa, nome_contato, email_contato, id, lattext, longtext, senha, status_loja, email_codigo, kunnr")]lojas lojas)
        {
            if (cp == "")
            {
                return Json(new { status_loja = 0, msg = "Cpf/Cnpj precisa ser preenchidos." }, JsonRequestBehavior.AllowGet);
            }



            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://wsrelacionamentocliente/");
                // client.BaseAddress = new Uri("http://wsrelacionamentocliente.solarbr.com.br/");
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("strChave", cp),
            });
                var result = client.PostAsync("wsRelacionamentoCliente.asmx/ObterCliente", content).Result;
                var httpContent = new StringContent(result.ToString(), Encoding.UTF8, "application/xml");
                string resultContent = result.Content.ReadAsStringAsync().Result;

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(resultContent);
                var x1 = XDocument.Parse(resultContent);


                /* CODIGO */
                var KUNNRget = x1.XPathSelectElements("//KUNNR")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                /* Razão Social */
                var RAZ_SOCIALget = x1.XPathSelectElements("//RAZ_SOCIAL")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var RAZ_SOCIAL = RAZ_SOCIALget[0][0];


                /* Contato */
                var CONTATOget = x1.XPathSelectElements("//CONTATO")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var CONTATO = CONTATOget[0][0];

                /* Endereço de e-mail */
                var SMTP_ADDRget = x1.XPathSelectElements("//SMTP_ADDR")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var SMTP_ADDR = SMTP_ADDRget[0][0];

                /* CNPJ */
                var STCD1get = x1.XPathSelectElements("//STCD1")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var STCD1 = STCD1get[0][0];

                /* CPF */
                var STCD2get = x1.XPathSelectElements("//STCD2")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var STCD2 = STCD2get[0][0];

                var KUNNR = KUNNRget[0][0];

                if (KUNNR != "")
                {
                    if (SMTP_ADDR == "")
                    {
                        return Json(new
                        {
                            //USUARIO NAO POSSUI EMAIL
                            id = 0,
                            kunnr = KUNNR,
                            status_loja = 666,
                            email_codigo = 0,
                            email_loja = "",
                            raz_social = RAZ_SOCIAL,
                            contato = CONTATO,
                            cpfcpnj = STCD1 + "" + STCD2

                        }, JsonRequestBehavior.AllowGet);


                    }


                    var rsLoja = db.lojas.Where(l => l.cnpj_loja == cp).FirstOrDefault();
                    //var rsLoja = db.lojas.Where(l => l.kunnr == cp).FirstOrDefault();

                    if (rsLoja == null)
                    {
                        var cpfcpnjstring = STCD1 + "" + STCD2;

                        var finalString = GeraCodigo2();
                        var finalString2 = GeraCodigo();
                        lojas.kunnr = KUNNR;
                        lojas.cnpj_loja = cpfcpnjstring;
                        lojas.razao_social_loja = RAZ_SOCIAL;
                        lojas.nome_contato = CONTATO;
                        lojas.email_contato = SMTP_ADDR;
                        lojas.senha = MD5Hash(finalString2);
                        lojas.status_loja = 3;
                        lojas.email_codigo = finalString;
                        lojas.created = DateTime.Now;
                       // lojas.lattext = "1";
                      //  lojas.email_codigo = "123";                      

                        db.lojas.Add(lojas);
                        db.SaveChanges();

                        /*
                        var email = new MailMessage();
                        var message = new MailMessage();
                        var body = "<p>Olá, {0}</p>  <p>Obrigado por se registrar no App Solar. Para ativar a conta basta visitar o endereço fornecido abaixo, confirmando assim o seu endereço de email.</p> <p>Para completar o registro visite o link:</p> <p> http://solarcoke.solarbr.com.br:32014/SolarActivate?code=" +finalString+ "</p> <p> * Caso o link neste email não funcione, por favor abra o seu navegador, copie o link na área de endereço e aperte enter.</p> <p> Atenciosamente,<br> SolarBr </p>";

                        message.To.Add(new MailAddress(lojas.email_contato));  // replace with valid value 
                       // message.To.Add(new MailAddress(lojas.email_contato));  // replace with valid value 
                        message.From = new MailAddress("cadastroclientessolar@solarbr.com.br");  // replace with valid value
                        message.Subject = "Confirme seu endereço de e-mail";
                        message.Body = string.Format(body, lojas.razao_social_loja);
                        message.IsBodyHtml = true;

                        using (var smtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
  //                               UserName = "programacao02@mobitap.com.br",  // replace with valid value
                               UserName = "schcadastrocli@solarbr.com.br",  // replace with valid value
                            //    Password = "flavio@123"  // replace with valid value
                                Password = "1q2w3e4r!Q@W#E$R"  // replace with valid value
                            };
                            smtp.Credentials = credential;
  //                           smtp.Host = "smtp.gmail.com";
                           smtp.Host = "smtp.office365.com";
                            smtp.Port = 587;
                            smtp.EnableSsl = true;
                            smtp.Send(message);
                        } */

                        var rsLoja2 = db.lojas.Where(lojass => lojass.cnpj_loja == cpfcpnjstring).FirstOrDefault();

                        return Json(new
                        {
                            id = rsLoja2.id,
                            kunnr = KUNNR,
                            status_loja = 5,
                            email_codigo = rsLoja2.email_codigo,
                            email_loja = SMTP_ADDR,
                            raz_social = RAZ_SOCIAL,
                            contato = CONTATO,
                            cpfcpnj = rsLoja2.cnpj_loja

                        }, JsonRequestBehavior.AllowGet);
                    }

                    if (rsLoja != null && rsLoja.status_loja == 1)
                    {
                        return Json(new
                        {
                            id = rsLoja.id,
                            status_loja = rsLoja.status_loja

                        }, JsonRequestBehavior.AllowGet);
                    }
                    if (rsLoja != null && rsLoja.status_loja == 2)
                    {
                        return Json(new
                        {
                            id = rsLoja.id,
                            status_loja = rsLoja.status_loja

                        }, JsonRequestBehavior.AllowGet);
                    }
                    if (rsLoja != null && rsLoja.status_loja != 1 && rsLoja.status_loja != 2)
                    {
                        lojas lojasss = db.lojas.Find(rsLoja.id);
                        db.lojas.Remove(lojasss);
                        db.SaveChanges();

                        var cpfcpnjstring = STCD1 + "" + STCD2;
                        var finalString = GeraCodigo2();
                        var finalString2 = GeraCodigo();
                        lojas.kunnr = KUNNR;
                        lojas.cnpj_loja = cpfcpnjstring;
                        lojas.razao_social_loja = RAZ_SOCIAL;
                        lojas.nome_contato = CONTATO;
                        lojas.email_contato = SMTP_ADDR;
                        lojas.senha = MD5Hash(finalString2);
                        lojas.status_loja = 3;
                        lojas.email_codigo = finalString;
                        lojas.created = DateTime.Now;
                        //lojas.lattext = "1";
                        //  lojas.email_codigo = "123";
                        db.lojas.Add(lojas);
                        db.SaveChanges();

                        /*
                        var email = new MailMessage();
                        var message = new MailMessage();
                        var body = "<p>Olá, {0}</p>  <p>Obrigado por se registrar no App Solar. Para ativar a conta basta visitar o endereço fornecido abaixo, confirmando assim o seu endereço de email.</p> <p>Para completar o registro visite o link:</p> <p> http://solarcoke.solarbr.com.br:32014/SolarActivate?code=" + finalString + "</p> <p> * Caso o link neste email não funcione, por favor abra o seu navegador, copie o link na área de endereço e aperte enter.</p> <p> Atenciosamente,<br> SolarBr </p>";

                        message.To.Add(new MailAddress(lojas.email_contato));  // replace with valid value 
                        // message.To.Add(new MailAddress(lojas.email_contato));  // replace with valid value 
                        message.From = new MailAddress("cadastroclientessolar@solarbr.com.br");  // replace with valid value
                        message.Subject = "Confirme seu endereço de e-mail";
                        message.Body = string.Format(body, lojas.razao_social_loja);
                        message.IsBodyHtml = true;

                        using (var smtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                //                               UserName = "programacao02@mobitap.com.br",  // replace with valid value
                                UserName = "schcadastrocli@solarbr.com.br",  // replace with valid value
                                //    Password = "flavio@123"  // replace with valid value
                                Password = "1q2w3e4r!Q@W#E$R"  // replace with valid value
                            };
                            smtp.Credentials = credential;
                            //                           smtp.Host = "smtp.gmail.com";
                            smtp.Host = "smtp.office365.com";
                            smtp.Port = 587;
                            smtp.EnableSsl = true;
                            smtp.Send(message);
                        }
                        */
                        var rsLoja2 = db.lojas.Where(lojass => lojass.cnpj_loja == cpfcpnjstring).FirstOrDefault();

                        return Json(new
                        {
                            id = rsLoja2.id,
                            kunnr = KUNNR,
                            status_loja = 5,
                            email_codigo = rsLoja2.email_codigo,
                            email_loja = SMTP_ADDR,
                            raz_social = RAZ_SOCIAL,
                            contato = CONTATO,
                            cpfcpnj = rsLoja2.cnpj_loja

                        }, JsonRequestBehavior.AllowGet);
                    }

                    /*
                    if (rsLoja != null && rsLoja.status_loja == 3)
                    {
                        return Json(new
                        {
                            id = rsLoja.id,
                            status_loja = rsLoja.status_loja,
                            cnpj_loja = rsLoja.cnpj_loja,
                            razao_social_loja = rsLoja.razao_social_loja,
                            loja_ativa = rsLoja.loja_ativa,
                            nome_contato = rsLoja.nome_contato,
                            email_contato = rsLoja.email_contato,
                            email_codigo = rsLoja.email_codigo,
                            kunnr = rsLoja.kunnr,


                        }, JsonRequestBehavior.AllowGet);


                    }
                    if (rsLoja != null && rsLoja.status_loja == 4)
                    {
                        return Json(new
                        {
                            id = rsLoja.id,
                            status_loja = rsLoja.status_loja

                        }, JsonRequestBehavior.AllowGet);
                    }
                    if (rsLoja != null && rsLoja.status_loja == 5)
                    {
                        return Json(new
                        {
                            id = rsLoja.id,
                            status_loja = rsLoja.status_loja,
                            kunnr = rsLoja.kunnr,
                            email_codigo = rsLoja.email_codigo,
                            email_loja = rsLoja.email_contato,
                            raz_social = rsLoja.razao_social_loja,
                            contato = rsLoja.nome_contato,
                            cpfcpnj = rsLoja.cnpj_loja

                        }, JsonRequestBehavior.AllowGet);
                    }*/


                }
                else
                {


                    //AJUSTAR NOVA RETRA AQUI 

                    var rsLoja2 = db.lojas.Where(l => l.cnpj_loja == cp).FirstOrDefault();

                    if (rsLoja2 != null && rsLoja2.status_loja == 5)
                    {

                        return Json(new
                        {

                            id = rsLoja2.id,
                            status_loja = rsLoja2.status_loja,
                            cpfcpnj = rsLoja2.cnpj_loja,
                            raz_social = rsLoja2.razao_social_loja,
                            loja_ativa = rsLoja2.loja_ativa,
                            contato = rsLoja2.nome_contato,
                            email_loja = rsLoja2.email_contato,
                            email_codigo = rsLoja2.email_codigo,
                            kunnr = rsLoja2.kunnr,


                        }, JsonRequestBehavior.AllowGet);

                    }
                    else if (rsLoja2 != null && rsLoja2.status_loja == 4)
                    {
                        //LOJA NAO ENCONTRADA NO WEBSERVICE
                        return Json(new
                        {
                            id = 0,
                            status_loja = 4

                        }, JsonRequestBehavior.AllowGet);


                    }
                    else
                    {


                        //LOJA NAO ENCONTRADA NO WEBSERVICE
                        return Json(new
                        {
                            id = 0,
                            status_loja = 999

                        }, JsonRequestBehavior.AllowGet);


                    }


                }

            }

            return Json(JsonRequestBehavior.AllowGet);
        }

        //JsonMethod Lista Lojas
        public ActionResult JsonListaLojas()
        {
            var lojas = db.lojas;
            var listalojas = new List<dynamic>();

            foreach (var item in lojas)
            {
                listalojas.Add(new
                {
                    cnpj_loja = item.cnpj_loja,
                    razao_social_loja = item.razao_social_loja,
                    loja_ativa = item.loja_ativa,
                    nome_contato = item.nome_contato,
                    email_contato = item.email_contato,
                    id = item.id,
                    lattext = item.lattext,
                    longtext = item.longtext,
                    senha = item.senha,
                    status_loja = item.status_loja,
                    email_codigo = item.email_codigo,
                    kunnr = item.kunnr
                });
            }
            return Json(listalojas, JsonRequestBehavior.AllowGet);
        }

        //ReenviarToken
        public ActionResult ReenviarTokenMobile(int? id)
        {
            var rsLoja = db.lojas.FirstOrDefault(l => l.id == id);

            if (rsLoja.email_contato == null)
            {
                return Json(new { status = 0, msg = "Loja não tem um e-mail cadastrado" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var email = new MailMessage();
                var message = new MailMessage();
                var body = "<p>Token: {0} {1}</p><p>Campo util para:</p><p>{2}</p>";

                message.To.Add(new MailAddress(rsLoja.email_contato));  // replace with valid value 
                message.From = new MailAddress("cadastroclientessolar@solarbr.com.br");  // replace with valid value
                message.Subject = "Solar - Seu Código (token)";
                message.Body = string.Format(body, rsLoja.email_codigo, "", "um complemento no e-mail");
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

                    return Json(new
                    {
                        id = rsLoja.id,
                        status_loja = rsLoja.status_loja,
                        email_codigo = rsLoja.email_codigo

                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }


        //JsonMethod Lista FACs
        public ActionResult JsonListaFAQGrupos(int? id_loja)
        {

            var insertlog = LogAcesso("Perguntas Frequentes", "", "", id_loja == null ? 0 : id_loja);

            var faq = db.grupos_faq;
            var listafaq = new List<dynamic>();

            foreach (var item in faq)
            {
                listafaq.Add(new
                {
                    id = item.id,
                    grupo_nome = item.grupo_faq

                });
            }
            return Json(listafaq, JsonRequestBehavior.AllowGet);
        }

        //JsonMethod Lista FACs
        public ActionResult JsonListaFAQ(int? faq_grupo, int? id)
        {
            var faq = db.faq;

            if (faq_grupo == null && id == null)
            {
                var listafaq = new List<dynamic>();

                foreach (var item in faq)
                {
                   // string[] data = item.ultima_atualizacao.ToString().Replace(" 00:00:00", "").Split("-".ToCharArray());
                   
                    listafaq.Add(new
                    {
                        id = item.id,
                        faq_grupo = item.faq_grupo,
                        grupo_nome = item.grupos_faq.grupo_faq,
                        faq_pergunta = item.faq_pergunta,
                        faq_resposta = item.faq_resposta,
                       // ultima_atualizacao = data[2] + "/" + data[1] + "/" + data[0],
                        ultima_atualizacao = item.ultima_atualizacao.ToString().Replace(" 00:00:00", ""),
                        color = item.color
                    });
                }
                return Json(listafaq, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (id != null)
                {
                    var faqid = faq.Where(a => a.id == id);
                    if (faqid.Count() == 0)
                    {
                        return Json(new { status = 0, msg = "FAQ com ID especificado não encontrado" }, JsonRequestBehavior.AllowGet);
                    }
                    var listafaqid = new List<dynamic>();

                    foreach (var item in faqid)
                    {
                        //string[] data = item.ultima_atualizacao.ToString().Split("-".ToCharArray());
                        listafaqid.Add(new
                        {
                            id = item.id,
                            faq_grupo = item.faq_grupo,
                            grupo_nome = item.grupos_faq.grupo_faq,
                            faq_pergunta = item.faq_pergunta,
                            faq_resposta = item.faq_resposta,
                            ultima_atualizacao = item.ultima_atualizacao.ToString().Replace(" 00:00:00", ""),
                            color = item.color

                        });
                    }
                    return Json(listafaqid, JsonRequestBehavior.AllowGet);
                }

                if (faq_grupo != null)
                {
                    var faqgrupo = faq.Where(a => a.faq_grupo == faq_grupo);
                    if (faqgrupo.Count() == 0)
                    {
                        return Json(new { status = 0, msg = "FAQ com Grupo especificado não encontrado" }, JsonRequestBehavior.AllowGet);
                    }

                    var listafaqgrupo = new List<dynamic>();

                    foreach (var item in faqgrupo)
                    {
                    //    string[] data = item.ultima_atualizacao.ToString().Replace(" 00:00:00", "").Split("-".ToCharArray());
                        listafaqgrupo.Add(new
                        {
                            id = item.id,
                            faq_grupo = item.faq_grupo,
                            grupo_nome = item.grupos_faq.grupo_faq,
                            faq_pergunta = item.faq_pergunta,
                            faq_resposta = item.faq_resposta,
                         //   ultima_atualizacao = data[2] + "/" + data[1] + "/" + data[0]
                            ultima_atualizacao = item.ultima_atualizacao.ToString().Replace(" 00:00:00", "")
                        });
                    }
                    return Json(listafaqgrupo, JsonRequestBehavior.AllowGet);
                }

                return Json(new { status = 0, msg = "Grupo ou ID especificado não encontrado" }, JsonRequestBehavior.AllowGet);
            }
        }


        //JsonMethod Lista FACs
        public ActionResult JsonListaFAQAndroid(string faq_grupo)
        {
            var faq = db.faq;

            var faqgrupo = faq.Where(a => a.grupos_faq.grupo_faq == faq_grupo);
            if (faqgrupo.Count() == 0)
            {
                return Json(new { status = 0, msg = "FAQ com Grupo especificado não encontrado" }, JsonRequestBehavior.AllowGet);
            }

            var listafaqgrupo = new List<dynamic>();

            foreach (var item in faqgrupo)
            {
                listafaqgrupo.Add(new
                {
                    id = item.id,
                    faq_grupo = item.faq_grupo,
                    grupo_nome = item.grupos_faq.grupo_faq,
                    faq_pergunta = item.faq_pergunta,
                    faq_resposta = item.faq_resposta,
                    ultima_atualizacao = item.ultima_atualizacao
                });
            }
            return Json(listafaqgrupo, JsonRequestBehavior.AllowGet);

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

        public ActionResult ObterClienteSR(string strChave)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://wsrelacionamentocliente/");
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("strChave", strChave),
            });

                var result = client.PostAsync("wsRelacionamentoCliente.asmx/ObterCliente", content).Result;
                var httpContent = new StringContent(result.ToString(), Encoding.UTF8, "application/xml");
                string resultContent = result.Content.ReadAsStringAsync().Result;

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(resultContent);
                var x1 = XDocument.Parse(resultContent);


                /* Razão Social */
                var RAZ_SOCIALget = x1.XPathSelectElements("//RAZ_SOCIAL")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var RAZ_SOCIAL = RAZ_SOCIALget[0][0];

                /* Rua */
                var STREETget = x1.XPathSelectElements("//STREET")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var STREET = STREETget[0][0];

                /* Complemento do nº */
                var HOUSE_NUM2get = x1.XPathSelectElements("//HOUSE_NUM2")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var HOUSE_NUM2 = HOUSE_NUM2get[0][0];

                /* Nº */
                var HOUSE_NUM1get = x1.XPathSelectElements("//HOUSE_NUM1")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var HOUSE_NUM1 = HOUSE_NUM1get[0][0];

                /* Código postal da localidade */
                var POST_CODE1get = x1.XPathSelectElements("//POST_CODE1")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var POST_CODE1 = POST_CODE1get[0][0];

                /* Nome Fantasia */
                var NAME2get = x1.XPathSelectElements("//NAME2")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var NAME2 = NAME2get[0][0];

                /* Bairro */
                var CITY2get = x1.XPathSelectElements("//CITY2")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var CITY2 = CITY2get[0][0];

                /* Região (estado federal, estado federado, província, condado) */

                var REGIONget = x1.XPathSelectElements("//REGION")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var REGION = REGIONget[0][0];

                /* Contato */
                var CONTATOget = x1.XPathSelectElements("//CONTATO")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var CONTATO = CONTATOget[0][0];

                /* Cidade */
                var CITY1get = x1.XPathSelectElements("//CITY1")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var CITY1 = CITY1get[0][0];

                /* 1º Nº telefone */
                var TELF1get = x1.XPathSelectElements("//TELF1")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var TELF1 = TELF1get[0][0];

                /* 2º Nº telefone */
                var TELF2get = x1.XPathSelectElements("//TELF2")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var TELF2 = TELF2get[0][0];

                /* Nº telefax */
                var TELFXget = x1.XPathSelectElements("//TELFX")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var TELFX = TELFXget[0][0];

                /* Endereço de e-mail */
                var SMTP_ADDRget = x1.XPathSelectElements("//SMTP_ADDR")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var SMTP_ADDR = SMTP_ADDRget[0][0];

                /* CNPJ */
                var STCD1get = x1.XPathSelectElements("//STCD1")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var STCD1 = STCD1get[0][0];

                /* CPF */
                var STCD2get = x1.XPathSelectElements("//STCD2")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var STCD2 = STCD2get[0][0];

                /* Latitude da localização geográfica */
                var LATITUDEget = x1.XPathSelectElements("//LATITUDE")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var LATITUDE = LATITUDEget[0][0];

                /* Longitude da localização geográfica */
                var LONGITUDEget = x1.XPathSelectElements("//LONGITUDE")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var LONGITUDE = LONGITUDEget[0][0];

                var lista = new List<dynamic>();

                lista.Add(new
                {
                    RAZ_SOCIAL = RAZ_SOCIAL,
                    STREET = STREET,
                    HOUSE_NUM2 = HOUSE_NUM2,
                    HOUSE_NUM1 = HOUSE_NUM1,
                    POST_CODE1 = POST_CODE1,
                    NAME2 = NAME2,
                    CITY2 = CITY2,
                    REGION = REGION,
                    CONTATO = CONTATO,
                    CITY1 = CITY1,
                    TELF1 = TELF1,
                    TELF2 = TELF2,
                    TELFX = TELFX,
                    SMTP_ADDR = SMTP_ADDR,
                    STCD1 = STCD1,
                    STCD2 = STCD2,
                    LATITUDE = LATITUDE,
                    LONGITUDE = LONGITUDE
                });

                return Json(lista, JsonRequestBehavior.AllowGet);

            }
        }



        public ActionResult AtulizarClienteCp(string strChave, string strContato, string strTel02, string strEmail)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://wsrelacionamentocliente/");
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("strChave", strChave),
                new KeyValuePair<string, string>("strContato", strContato),
                new KeyValuePair<string, string>("strTel02", strTel02),
                new KeyValuePair<string, string>("strEmail", strEmail),
            });

                var result = client.PostAsync("wsRelacionamentoCliente.asmx/AtualizarCliente", content).Result;
                var httpContent = new StringContent(result.ToString(), Encoding.UTF8, "application/xml");
                string resultContent = result.Content.ReadAsStringAsync().Result;

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(resultContent);
                var x1 = XDocument.Parse(resultContent);

                var retorno = x1.Root.Value;

                var lista = new List<dynamic>();
                lista.Add(new
                {
                    retorno = retorno,
                    logdata = "data"
                });

                return Json(lista, JsonRequestBehavior.AllowGet);


            }
        }



        //Login Mobile Json OnlyCPW
        public ActionResult JsonDetalhelojaWS(string cp, [Bind(Include = "solicitante, id_loja_fabricante, unidade_faturamento, cnpj_loja, razao_social_loja, nome_fantasia_loja, ufloja, inscricao_estadual_loja, id_fabricante, loja_ativa, codigo_tabela_preco, nome_contato, email_contato, numero_contato, numero2_contato, tipo_contato, cod_ibge_municipio_loja, cep_loja, nome_logradouro_loja, numero_local_loja, bairro_local_loja, cod_ibge_municipio_entrega, cep_entrega, nome_logradouro_entrega, numero_local_entrega, bairro_local_entrega, id, lattext, longtext, url_foto, senha, status_loja, email_codigo ")]lojas lojas)
        {            
            using (var client = new HttpClient())
            {
               // client.BaseAddress = new Uri("http://wsrelacionamentocliente.solarbr.com.br/");
                client.BaseAddress = new Uri("http://wsrelacionamentocliente/");
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("strChave", cp),
            });

                var result = client.PostAsync("wsRelacionamentoCliente.asmx/ObterCliente", content).Result;
                var httpContent = new StringContent(result.ToString(), Encoding.UTF8, "application/xml");
                string resultContent = result.Content.ReadAsStringAsync().Result;

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(resultContent);
                var x1 = XDocument.Parse(resultContent);


                /* CODIGO */
                var KUNNRget = x1.XPathSelectElements("//KUNNR")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                /* Razão Social */
                var RAZ_SOCIALget = x1.XPathSelectElements("//RAZ_SOCIAL")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var RAZ_SOCIAL = RAZ_SOCIALget[0][0];


                /* Contato */
                var CONTATOget = x1.XPathSelectElements("//CONTATO")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var CONTATO = CONTATOget[0][0];

                /* Endereço de e-mail */
                var SMTP_ADDRget = x1.XPathSelectElements("//SMTP_ADDR")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var SMTP_ADDR = SMTP_ADDRget[0][0];



                /* Endereço de e-mail */
                var TELF2get = x1.XPathSelectElements("//TELF2")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var TELF2 = TELF2get[0][0];


                /* CNPJ */
                var STCD1get = x1.XPathSelectElements("//STCD1")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var STCD1 = STCD1get[0][0];

                /* CPF */
                var STCD2get = x1.XPathSelectElements("//STCD2")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var STCD2 = STCD2get[0][0];


                /* STREET */
                var STREETget = x1.XPathSelectElements("//STREET")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var STREET = STREETget[0][0];

                /* HOUSE_NUM2 */
                var HOUSE_NUM2get = x1.XPathSelectElements("//HOUSE_NUM2")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var HOUSE_NUM2 = HOUSE_NUM2get[0][0];

                /* HOUSE_NUM1 */
                var HOUSE_NUM1get = x1.XPathSelectElements("//HOUSE_NUM1")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var HOUSE_NUM1 = HOUSE_NUM1get[0][0];

                /* POST_CODE1 */
                var POST_CODE1get = x1.XPathSelectElements("//POST_CODE1")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var POST_CODE1 = POST_CODE1get[0][0];

                /* NAME2 */
                var NAME2get = x1.XPathSelectElements("//NAME2")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var NAME2 = NAME2get[0][0];

                /* CITY2 */
                var CITY2get = x1.XPathSelectElements("//CITY2")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var CITY2 = CITY2get[0][0];

                /* REGION */
                var REGIONget = x1.XPathSelectElements("//REGION")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var REGION = REGIONget[0][0];



                /* CITY1 */
                var CITY1get = x1.XPathSelectElements("//CITY1")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var CITY1 = CITY1get[0][0];

                /* TELF1 */
                var TELF1get = x1.XPathSelectElements("//TELF1")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var TELF1 = TELF1get[0][0];


                /* TELFX */
                var TELFXget = x1.XPathSelectElements("//TELFX")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var TELFX = TELFXget[0][0];

                /* LATITUDE */
                var LATITUDEget = x1.XPathSelectElements("//LATITUDE")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var LATITUDE = LATITUDEget[0][0];

                /* LONGITUDE */
                var LONGITUDEget = x1.XPathSelectElements("//LONGITUDE")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();

                var LONGITUDE = LONGITUDEget[0][0];

                var KUNNR = KUNNRget[0][0];
                var insertlog = LogAcesso("Meus Dados", KUNNR, "",0);
                

                return Json(new
                {
                    //USUARIO NAO POSSUI EMAIL
                    id = 0,
                    kunnr = KUNNR,
                    status_loja = 666,
                    email_codigo = 0,
                    email_loja = SMTP_ADDR,
                    raz_social = RAZ_SOCIAL,
                    contato = CONTATO,
                    cpfcpnj = STCD1 + "" + STCD2,
                    telf2 = TELF2,
                    STREET = STREET,
                    HOUSE_NUM2 = HOUSE_NUM2,
                    HOUSE_NUM1 = HOUSE_NUM1,
                    POST_CODE1 = POST_CODE1,
                    NAME2 = NAME2,
                    CITY2 = CITY2,
                    REGION = REGION,
                    CONTATO = CONTATO,
                    CITY1 = CITY1,
                    TELF1 = TELF1,
                    TELF2 = TELF2,
                    TELFX = TELFX,
                    LATITUDE = LATITUDE,
                    LONGITUDE = LONGITUDE,

                }, JsonRequestBehavior.AllowGet);

            }
        }


        public ActionResult JsonDetalheCampanhaId(int? id_loja, int? id)
        {
            var gestao_campanhadetalhe = db.gestao_campanha.Where(a => a.id == id);

            if (gestao_campanhadetalhe.Count() != 0)
            {
                var gestao_campanhadetalhe2 = db.gestao_campanha.Where(a => a.id == id).FirstOrDefault();

                var insertlog = LogAcesso(gestao_campanhadetalhe2.campanha_tipo.nome + " : " + gestao_campanhadetalhe2.titulo, "", "", id_loja == null ? 0 : id_loja); 
            }

            var listacampanhas = new List<dynamic>();

            foreach (var item in gestao_campanhadetalhe)
            {
                var rscampanhafoto = db.campanha_foto.Where(a => a.id_loja == id_loja && a.id_campanha == id).Count();
                var rscampanhaloja = db.campanha_loja.Where(a => a.id_loja == id_loja && a.id_campanha == id).Count();
                var rslistacompra = db.listacompra_loja.Where(a => a.id_loja == id_loja && a.id_campanha == id).Count();
                listacampanhas.Add(new
                {
                    id = item.id,
                    titulo = item.titulo,
                    url_foto = item.url_foto,
                    descricao_curta = item.descricao_curta,
                    descricao = item.descricao,
                    permissao_img = item.permissao_img,
                    aderir = item.aderir,
                    url_facebok = item.url_facebok,
                    url_twitter = item.url_twitter,
                    url_youtube = item.url_youtube,
                    id_tipocampanha = item.id_tipocampanha,
                    url_arquivo = item.url_arquivo,
                    url_ldpi = item.url_ldpi,
                    url_mdpi = item.url_mdpi,
                    url_hdpi = item.url_hdpi,
                    url_xhdpi = item.url_xhdpi,
                    link = item.link,
                    link_nome = item.link_nome,
                    permitir_lista = item.permitir_lista,
                    iphone4 = item.iphone4,
                    iphone5 = item.iphone5,
                    iphone6 = item.iphone6,
                    iphone6plus = item.iphone6plus,
                    base64_foto = item.base64_foto,
                    icon_image = item.icon_image,
                    ativa = item.ativa,
                    participa = rscampanhaloja,
                    campanha_foto = rscampanhafoto,
                    listacompra_loja = rslistacompra

                });
            }

            return Json(listacampanhas, JsonRequestBehavior.AllowGet);
        }

        public ActionResult JsonListaCampanhaTipo(int id_tipocampanha, int? id_loja)
        {

            if (id_tipocampanha == 1)
            {
                var insertlog = LogAcesso("Campanha", "", "", id_loja == null ? 0 : id_loja);                
            }
            else if (id_tipocampanha == 2)
            {
                var insertlog = LogAcesso("Materiais promocionais", "", "", id_loja == null ? 0 : id_loja);                
            }
            else if (id_tipocampanha == 3)
            {
                var insertlog = LogAcesso("Lançamento  ", "", "", id_loja == null ? 0 : id_loja);                
            }
            var gestao_campanha = db.gestao_campanha.Where(a => a.id_tipocampanha == id_tipocampanha && a.ativa == 1);

            var listacampanhas = new List<dynamic>();

            foreach (var item in gestao_campanha)
            {

                listacampanhas.Add(new
                {
                    id = item.id,
                    titulo = item.titulo,
                    url_foto = item.url_foto,
                    descricao_curta = item.descricao_curta,
                    tipo_id = item.id_tipocampanha,
                    ativa = item.ativa
                    //url_arquivo = item.campanha_loja.FirstOrDefault().id;

                });
            }

            return Json(listacampanhas.OrderByDescending(a => a.id), JsonRequestBehavior.AllowGet);
        }
        /*    
 //JsonMethod Lista Campanhas
 public ActionResult JsonListaCampanha(int? id, int? id_loja, int? id_tipocampanha)
 {
     var gestao_campanha = db.gestao_campanha;
     if (id_tipocampanha != null)
     {
         gestao_campanha.Where(a => a.id_tipocampanha == id_tipocampanha);
     }

     //var exclui = db.ativacao.SqlQuery("DELETE FROM ativacao_projeto WHERE id_projeto = " + projeto.id_projeto);                        

     if (id == null)
     {
         var listacampanhas = new List<dynamic>();

         foreach (var item in gestao_campanha)

         {
             var rscampanhafoto = db.campanha_foto.Where(a => a.id_loja == id_loja && a.id_campanha == item.id).Count();
             var rscampanhaloja = db.campanha_loja.Where(a => a.id_loja == id_loja && a.id_campanha == item.id).Count();

             listacampanhas.Add(new
             {
                 id = item.id,
                 titulo = item.titulo,
                 url_foto = item.url_foto,
                 descricao_curta = item.descricao_curta,
                 descricao = item.descricao,
                 permissao_img = item.permissao_img,
                 aderir = item.aderir,
                 url_facebok = item.url_facebok,
                 url_twitter = item.url_twitter,
                 url_youtube = item.url_youtube,
                 id_tipocampanha = item.id_tipocampanha,
                 url_arquivo = item.url_arquivo,
                 url_ldpi = item.url_ldpi,
                 url_mdpi = item.url_mdpi,
                 url_hdpi = item.url_hdpi,
                 url_xhdpi = item.url_xhdpi,
                 link = item.link,
                 link_nome = item.link_nome,
                 permitir_lista = item.permitir_lista,
                 iphone4 = item.iphone4,
                 iphone5 = item.iphone5,
                 iphone6 = item.iphone6,
                 iphone6plus = item.iphone6plus,
                 participa = rscampanhaloja,
                 campanha_foto = rscampanhafoto,
                 base64_foto = item.base64_foto
                 //url_arquivo = item.campanha_loja.FirstOrDefault().id;

             });
         }

         return Json(listacampanhas, JsonRequestBehavior.AllowGet);
     }
     else
     {

         var rscampanhaloja = db.campanha_loja.Where(a => a.id_loja == id_loja && a.id_campanha == id).Count();
                
         var rslistacompra = db.listacompra_loja.Where(a => a.id_loja == id_loja && a.id_campanha == id).Count(); 
                                
         var gestao_campanhadetalhe = gestao_campanha.Where(a => a.id == id);

         var listacampanhas = new List<dynamic>();

         foreach (var item in gestao_campanhadetalhe)
         {

             listacampanhas.Add(new
             {
                 id = item.id,
                 titulo = item.titulo,
                 url_foto = item.url_foto,
                 descricao_curta = item.descricao_curta,
                 descricao = item.descricao,
                 permissao_img = item.permissao_img,
                 aderir = item.aderir,
                 url_facebok = item.url_facebok,
                 url_twitter = item.url_twitter,
                 url_youtube = item.url_youtube,
                 id_tipocampanha = item.id_tipocampanha,
                 url_arquivo = item.url_arquivo,
                 url_ldpi = item.url_ldpi,
                 url_mdpi = item.url_mdpi,
                 url_hdpi = item.url_hdpi,
                 url_xhdpi = item.url_xhdpi,
                 link = item.link,
                 link_nome = item.link_nome,
                 permitir_lista = item.permitir_lista,
                 iphone4 = item.iphone4,
                 iphone5 = item.iphone5,
                 iphone6 = item.iphone6,
                 iphone6plus = item.iphone6plus,
                 base64_foto = item.base64_foto

             });
         }

         return Json(listacampanhas, JsonRequestBehavior.AllowGet);                    
                    
     }
 } */


        /*Metodo criado para vincular loja com a campanha */

        public ActionResult InsertCampanhaLoja([Bind(Include = "id,id_loja,id_campanha")] campanha_loja campanha_loja)
        {

            var rscampanha = campanha_loja.id_campanha.ToString();
            var rsloja = campanha_loja.id_loja.ToString();
            if (rscampanha == "" || rsloja == "")
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                db.campanha_loja.Add(campanha_loja);
                db.SaveChanges();

                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
        }




        /*Metodo criado para vincular loja com a campanha */

        public ActionResult InsertListacompraLoja([Bind(Include = "id,id_loja,id_campanha")] listacompra_loja listacompra_loja)
        {
            var rscampanha = listacompra_loja.id_campanha.ToString();
            var rsloja = listacompra_loja.id_loja.ToString();
            if (rscampanha == "" || rsloja == "")
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                db.listacompra_loja.Add(listacompra_loja);
                db.SaveChanges();

                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
        }




        /*Metodo criado para vincular as fotos que a loja enviar para campanha */

        public ActionResult InsertCampanhaFoto([Bind(Include = "id_campanha,foto, id_loja")] campanha_foto campanha_foto)
        {
            var rscampanha = campanha_foto.id_campanha.ToString();
            var rsloja = campanha_foto.id_loja.ToString();
            if (rscampanha == "" || rsloja == "" || campanha_foto.foto == null)
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                campanha_foto.foto = campanha_foto.foto;
                db.campanha_foto.Add(campanha_foto);
                db.SaveChanges();

                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
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

        public ActionResult InsertFotoVisitaMobile([Bind(Include = "id_visita, url_foto, data")] visita_fotos visita_fotos, HttpPostedFileBase convert_foto)
        {

            if (visita_fotos.id_visita == null || visita_fotos.url_foto == null)
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var fileName = "";

                var finalString = GeraCodigo();

                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(visita_fotos.url_foto)))
                {
                    using (Bitmap bm2 = new Bitmap(ms))
                    {
                        fileName = "visita" + visita_fotos.id_visita + "-" + finalString.ToString() + ".jpg";
                        var path = Path.Combine(Server.MapPath("~/Content/assets/VisitaFotos"), fileName);
                        bm2.Save(path);
                    }
                }

                if (fileName == "")
                {
                    visita_fotos.url_foto = null;
                }
                else
                {
                    visita_fotos.url_foto = "http://solarcoke.solarbr.com.br:32014/Content/assets/VisitaFotos/" + fileName;
                }

                visita_fotos.data = DateTime.Now.Date.ToString();
                db.visita_fotos.Add(visita_fotos);
                db.SaveChanges();

                return Json(new { status = 1, url_foto = visita_fotos.url_foto }, JsonRequestBehavior.AllowGet);
            }
        }


        //Login Mobile Json OnlyCP
        public ActionResult JsonPedidoMobileCP(string cp)
        {

            var data = DateTime.Now;
            var mes = data.Month.ToString();
            var ano = data.Year.ToString();

            string datapedido = ano+mes+"01";

            if (cp == "")
            {
                return Json(new { status_loja = 0, msg = "Cpf/Cnpj precisa ser preenchidos." }, JsonRequestBehavior.AllowGet);
            }

            try
            {

            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://wsrelacionamentocliente.solarbr.com.br/");
                client.BaseAddress = new Uri("http://wsrelacionamentocliente/");
                var content = new FormUrlEncodedContent(new[] 
                {
                    new KeyValuePair<string, string>("strChave", cp),
                    new KeyValuePair<string, string>("strDataPedido", datapedido),
                });

                var result = client.PostAsync("wsRelacionamentoCliente.asmx/ObterInformacoesPedidos", content).Result;
                var httpContent = new StringContent(result.ToString(), Encoding.UTF8, "application/xml");
                string resultContent = result.Content.ReadAsStringAsync().Result;

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(resultContent);
                var x1 = XDocument.Parse(resultContent);


                /* VBELN */
                var VBELNget = x1.XPathSelectElements("//VBELN")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var VBELN = VBELNget;

                /* NFENUM */
                var NFENUMget = x1.XPathSelectElements("//NFENUM")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var NFENUM = NFENUMget;

                /* ERDAT */
                var ERDATget = x1.XPathSelectElements("//ERDAT")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var ERDAT = ERDATget;

            /*    var ERDAT2 = ERDAT.ToString();

                var data2 = "";
                if (ERDAT2 != null)
                {
                    string[] arrDate = ERDAT2.Split('/');

                    string month = arrDate[0].ToString();
                    string day = arrDate[1].ToString();
                    string year = arrDate[2].ToString();

                    data2 = day + "/" + month + "/" + year;
                }
                var date3 = data2();   */


                /* KZWI1 */
                var KZWI1get = x1.XPathSelectElements("//KZWI1")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var KZWI1 = KZWI1get;

                /* NFTOT */
                var NFTOTget = x1.XPathSelectElements("//NFTOT")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var NFTOT = NFTOTget;

                /* KUNNR */
                var KUNNRget = x1.XPathSelectElements("//KUNNR")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var KUNNR = KUNNRget;

                /* MATNR */
                var MATNRget = x1.XPathSelectElements("//MATNR")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var MATNR = MATNRget;

                /* KWMENG */
                var KWMENGget = x1.XPathSelectElements("//KWMENG")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var KWMENG = KWMENGget;

                /* ARKTX */
                var ARKTXget = x1.XPathSelectElements("//ARKTX")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var ARKTX = ARKTXget;

                /* STATUS */
                var STATUSget = x1.XPathSelectElements("//STATUS")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var STATUS = STATUSget;


                /* MSEHL */
                var MSEHLget = x1.XPathSelectElements("//MSEHL")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var MSEHL = MSEHLget;

                /* UMZIZ */
                var UMZIZget = x1.XPathSelectElements("//UMZIZ")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var UMZIZ = UMZIZget;

                int counttotal = VBELN.Count;

                var listacampanhas = new List<dynamic>(); 

                var VBELNANTIGO = "";
                var insertlog = LogAcesso("Status de Pedidos", KUNNR[0][0], "",0);

             //   double totaltotal = 0;
                for (int i = 0; i < counttotal; i++)
                {
                    // var VBELNANTIGODOIS = "";                 
                  //  totaltotal = 0;
                    if (VBELN[i][0] != VBELNANTIGO)
                    {
                        var listaprodutos = new List<dynamic>();

                        for (int ii = 0; ii < counttotal; ii++)
                        {
                            //if (VBELN[ii][0] != VBELNANTIGODOIS)
                            //{
                            if (VBELN[i][0] == VBELN[ii][0])
                            {

                                String valor = KZWI1[ii][0].Replace(".", ",");

                                listaprodutos.Add(new
                                {
                                    ERDAT = ERDAT[ii][0],
                                    MATNR = MATNR[ii][0],
                                    KWMENG = KWMENG[ii][0].Replace(".000", ""),
                                    ARKTX = ARKTX[ii][0],
                                    STATUS = STATUS[ii][0],
                                    MSEHL = MSEHL[ii][0],
                                    UMZIZ = UMZIZ[ii][0],
                                    KZWI1 = "R$" + " " + string.Format("{0:N}", valor),
                                });

                            //    totaltotal = totaltotal + Convert.ToDouble(KZWI1[ii][0]);
                            }
                            // VBELNANTIGODOIS = VBELN[ii][0];
                            //}
                        }

                      //  String rstotal = totaltotal.ToString();

                     //   String rstotaloutput = string.Format("{0:N}", rstotal.Replace(".", ","));
                        
                        // var rstotaloutput = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", rstotal); 


                        String dataretorno = ERDAT[i][0];

                        string[] dataprimerisplit = dataretorno.Split(" ".ToCharArray());

                        string[] datasegundosplit = dataprimerisplit[0].Split("/".ToCharArray());


                        String valor2 = NFTOT[i][0].Replace(".", ",");

                        if (valor2.Length == 8)
                        {
                            valor2 = valor2.Substring(0, 2) + "." + valor2.Substring(2);
                        }

                        if (valor2.Length == 7)
                        {
                            valor2 = valor2.Substring(0, 1) + "." + valor2.Substring(1);
                        }
                        /*  
                        if (rstotaloutput.Contains(","))
                        {
                        }
                        else
                        {
                            rstotaloutput = rstotaloutput + ",00";
                        }

                        string[] rstotaloutput1 = rstotaloutput.Split(',');

                        if (rstotaloutput1.Count() >= 2)
                        {
                            if (rstotaloutput1[1].Length == 1)
                            {
                                rstotaloutput = rstotaloutput + "0";
                            }
                        } */
                        
                        listacampanhas.Add(new
                        {
                            VBELN = VBELN[i][0],
                            NFENUM = NFENUM[i][0],
                            ERDAT = datasegundosplit[1] + "/" + datasegundosplit[0] + "/" + datasegundosplit[2],
                           // ERDAT = datasegundosplit[1] + "/" + datasegundosplit[0] + "/" + datasegundosplit[2] + " " + dataprimerisplit[1] + " " + dataprimerisplit[2],
                            KUNNR = KUNNR[i][0],
                            STATUS = STATUS[i][0],
                          //  totalKWMENG = "R$ " + rstotaloutput,
                            totalKWMENG = "R$" + " " + string.Format("{0:N}", valor2),
                            PRODUTOSITENS = listaprodutos                            
                        });
                        VBELNANTIGO = VBELN[i][0];
                    }
                }

                //return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
                return Json(listacampanhas, JsonRequestBehavior.AllowGet);
            }
            }
            catch (Exception)
            {
                return Json(new { status = 500, mensagem = "Usuário não possui pedidos" }, JsonRequestBehavior.AllowGet);
            }
        }

        //Login Mobile Json OnlyCP
        public ActionResult JsonEquipamentosMobileCP(string cp)
        {
            //cp = "0000360970";           



            if (cp == "")
            {
                return Json(new { status_loja = 0, msg = "Cpf/Cnpj precisa ser preenchidos." }, JsonRequestBehavior.AllowGet);
            }


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://wsrelacionamentocliente/");
                var content = new FormUrlEncodedContent(new[] 
                {
                    new KeyValuePair<string, string>("strChave", cp),                    
                });

                var result = client.PostAsync("wsRelacionamentoCliente.asmx/ObterEquipamentos", content).Result;
                var httpContent = new StringContent(result.ToString(), Encoding.UTF8, "application/xml");
                string resultContent = result.Content.ReadAsStringAsync().Result;

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(resultContent);
                var x1 = XDocument.Parse(resultContent);


                /* MANDT */
                var MANDTget = x1.XPathSelectElements("//MANDT")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var MANDT = MANDTget;


                /* ERDAT */
                var ERDATget = x1.XPathSelectElements("//ERDAT")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var ERDAT = ERDATget;


                /* EQKTX */
                var EQKTXget = x1.XPathSelectElements("//EQKTX")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var EQKTX = EQKTXget;


                /* EQKTU */
                var EQKTUget = x1.XPathSelectElements("//EQKTU")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var EQKTU = EQKTUget;

                /* EQUNR */
                var EQUNRget = x1.XPathSelectElements("//EQUNR")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var EQUNR = EQUNRget;

                /* HERST */
                var HERSTget = x1.XPathSelectElements("//HERST")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var HERST = HERSTget;

                /* TYPBZ */
                var TYPBZget = x1.XPathSelectElements("//TYPBZ")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var TYPBZ = TYPBZget;

                /* EQFNR */
                var EQFNRget = x1.XPathSelectElements("//EQFNR")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var EQFNR = EQFNRget;

                /* EQFNRI */
                var EQFNRIget = x1.XPathSelectElements("//EQFNRI")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var EQFNRI = EQFNRIget;

            /*    
                var KUNNRget = x1.XPathSelectElements("//KUNNR")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var KUNNR = KUNNRget; */

                int counttotal = MANDT.Count;

                var listacampanhas = new List<dynamic>();

           // public ActionResult ListaVisitaEquipamento(string equipamento)            
          //  var rsLista = db.visita.Where(a => a.id_equipamento == equipamento);
           //     var KUNNR2 = KUNNRget[0][0];
                var insertlog = LogAcesso("Visita Técnica", cp, "",0);

                for (int i = 0; i < counttotal; i++)
                {
                    string iddoequipamento = EQUNR[i][0];

                   // var teste = ListaVisitaEquipamento("ok");                 

                    /*
                    var rslistavisita = db.visita.Where(a => a.EQUNR.Contains("" + iddoequipamento + ""));
                    var ORDENACAOSTRING = rslistavisita.Count();

                    var rslistavisitapendnete = db.visita.Where(a => a.EQUNR.Contains("" + iddoequipamento + "") && a.id_status == 1);
                    var ORDENACAOPENDENTE = rslistavisitapendnete.Count();*/

                //    var iddoequipamento = EQUNR[i][0];

                    var rslistavisita = db.visita.Where(a => a.id_equipamento == iddoequipamento);
                    var ORDENACAOSTRING = rslistavisita.Count();

                    var rslistavisitapendnete = db.visita.Where(a => a.id_equipamento == iddoequipamento && a.id_status == 1);
                    var ORDENACAOPENDENTE = rslistavisitapendnete.Count(); 

                    /*
                     REGRA VAI AQUI 
                    */

                    //EQUNR
                    listacampanhas.Add(new
                    {
                        MANDT = MANDT[i][0],
                        ERDAT = ERDAT[i][0],
                        EQKTX = EQKTX[i][0],
                        EQKTU = EQKTU[i][0],
                        EQUNR = EQUNR[i][0],
                        HERST = HERST[i][0],
                        TYPBZ = TYPBZ[i][0],
                        EQFNR = EQFNR[i][0],
                        EQFNRI = EQFNRI[i][0],
                        ORDENACAO = ORDENACAOSTRING,
                        ORDENACAOPENDENTE = ORDENACAOPENDENTE,
                        ORDENACAOPENDENTEteste = 10,
                    });
                }
                List<dynamic> SortedList = listacampanhas.OrderByDescending(o => o.ORDENACAO).ToList();
                //return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
                return Json(SortedList, JsonRequestBehavior.AllowGet);
            }
        }

        //Login Mobile Json OnlyCP
        public ActionResult JsonEquipamentosPaginacaoMobileCP(string cp, int pagina)
        {
            // cp = "0000360970";


            if (cp == "")
            {
                return Json(new { status_loja = 0, msg = "Cpf/Cnpj precisa ser preenchidos." }, JsonRequestBehavior.AllowGet);
            }


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://wsrelacionamentocliente/");
                var content = new FormUrlEncodedContent(new[] 
                {
                    new KeyValuePair<string, string>("strChave", cp),                    
                });

                var result = client.PostAsync("wsRelacionamentoCliente.asmx/ObterEquipamentos", content).Result;
                var httpContent = new StringContent(result.ToString(), Encoding.UTF8, "application/xml");
                string resultContent = result.Content.ReadAsStringAsync().Result;

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(resultContent);
                var x1 = XDocument.Parse(resultContent);


                /* MANDT */
                var MANDTget = x1.XPathSelectElements("//MANDT")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var MANDT = MANDTget;


                /* ERDAT */
                var ERDATget = x1.XPathSelectElements("//ERDAT")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var ERDAT = ERDATget;


                /* EQKTX */
                var EQKTXget = x1.XPathSelectElements("//EQKTX")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var EQKTX = EQKTXget;


                /* EQKTU */
                var EQKTUget = x1.XPathSelectElements("//EQKTU")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var EQKTU = EQKTUget;

                /* EQUNR */
                var EQUNRget = x1.XPathSelectElements("//EQUNR")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var EQUNR = EQUNRget;

                /* HERST */
                var HERSTget = x1.XPathSelectElements("//HERST")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var HERST = HERSTget;

                /* TYPBZ */
                var TYPBZget = x1.XPathSelectElements("//TYPBZ")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var TYPBZ = TYPBZget;

                /* EQFNR */
                var EQFNRget = x1.XPathSelectElements("//EQFNR")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var EQFNR = EQFNRget;

                /* EQFNRI */
                var EQFNRIget = x1.XPathSelectElements("//EQFNRI")
                .GroupBy(x => x.Parent)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
                var EQFNRI = EQFNRIget;

                int counttotal = MANDT.Count;


                int proximo = pagina + 1;
                int anterio = pagina - 1;


                if (proximo >= counttotal)
                {
                    proximo = 0;
                }


                if (pagina == 0)
                {
                    anterio = 0;
                }


                var listacampanhas = new List<dynamic>();

                //for (int i = 0; i < counttotal; i++)
                //{
                listacampanhas.Add(new
                {
                    MANDT = MANDT[pagina][0],
                    ERDAT = ERDAT[pagina][0],
                    EQKTX = EQKTX[pagina][0],
                    EQKTU = EQKTU[pagina][0],
                    EQUNR = EQUNR[pagina][0],
                    HERST = HERST[pagina][0],
                    TYPBZ = TYPBZ[pagina][0],
                    EQFNR = EQFNR[pagina][0],
                    EQFNRI = EQFNRI[pagina][0],
                    counttotal = counttotal,
                    proximo = proximo,
                    anterio = anterio
                });


                //}


                //return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
                return Json(listacampanhas, JsonRequestBehavior.AllowGet);
            }
        }


    }
}