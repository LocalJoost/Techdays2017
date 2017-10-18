using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DemoServices.Startup))]

namespace DemoServices
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}