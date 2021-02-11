using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HouseRentManagementApp.Startup))]
namespace HouseRentManagementApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
