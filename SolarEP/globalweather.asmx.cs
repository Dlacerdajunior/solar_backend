using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SolarEP
{
    /// <summary>
    /// Summary description for WebServiceTeste
    /// </summary>
    [WebService(Namespace = "http://www.webservicex.net/globalweather.asmx?WSDL")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebServiceTeste : System.Web.Services.WebService
    {

        [WebMethod]
        private string GetCitiesByCountry(string CountryName)
        {
            return GetCitiesByCountry(CountryName); //ing country name input to Service  
        }
    }
}
