using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QuotationApp1.Startup))]
namespace QuotationApp1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
