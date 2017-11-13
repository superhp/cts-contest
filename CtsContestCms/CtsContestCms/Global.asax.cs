using System;
using System.Web.Http;
using CtsContestCms.AppStart;

namespace CtsContestCms
{
    public class WebApiApplication : Umbraco.Web.UmbracoApplication
    {
        protected override void OnApplicationStarted(object sender, EventArgs e)
        {
            base.OnApplicationStarted(sender, e);
        
            WebApiConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}