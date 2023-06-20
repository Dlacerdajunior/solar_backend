using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SolarEP.Startup))]
namespace SolarEP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
