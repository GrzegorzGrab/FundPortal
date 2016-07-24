using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FundPortal.Startup))]
namespace FundPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
