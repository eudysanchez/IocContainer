using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcIoC.Startup))]
namespace MvcIoC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
