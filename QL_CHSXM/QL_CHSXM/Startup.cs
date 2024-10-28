using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QL_CHSXM.Startup))]
namespace QL_CHSXM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
