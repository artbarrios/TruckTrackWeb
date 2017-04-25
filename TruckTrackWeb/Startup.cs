using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TruckTrackWeb.Startup))]
namespace TruckTrackWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
