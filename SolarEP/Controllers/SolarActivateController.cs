using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SolarEP.Models;

namespace SolarEP.Controllers
{
    public class SolarActivateController : Controller
    {
        private SolardbEntities db = new SolardbEntities();

        //
        // GET: /ValidarEmail/
        public ActionResult Index(string code)
        {

            var rsLoja = db.lojas.Where(c=>c.email_codigo == code).FirstOrDefault();
            //var confere = rsLoja.Where(a => a.email_codigo == code).Take(1);

            if (rsLoja != null)
            {
                //var loja = confere.FirstOrDefault();

                rsLoja.status_loja = 1;
                db.SaveChanges();
                return View();                
            }
            else
            {
                return RedirectToAction("CodigoInvalido", "SolarActivate");
            }
        }

        public ActionResult CodigoInvalido()
        {
                return View();            
        }
	}
}