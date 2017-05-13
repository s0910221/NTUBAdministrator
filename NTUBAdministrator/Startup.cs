using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NTUBAdministrator.Startup))]
namespace NTUBAdministrator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
