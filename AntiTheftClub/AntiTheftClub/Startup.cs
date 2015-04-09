using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AntiTheftClub.Startup))]
namespace AntiTheftClub
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
