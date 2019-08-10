using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChessWebsite.Startup))]
namespace ChessWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
