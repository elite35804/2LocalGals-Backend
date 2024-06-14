using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Nexus;

namespace TwoLocalGals
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //Database.LogThis("Application_Start", null);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //Database.LogThis("Session_Start", null);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //Database.LogThis("Application_BeginRequest", null);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //Database.LogThis("Application_AuthenticateRequest", null);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Database.LogThis("Application_Error", null);
        }

        protected void Session_End(object sender, EventArgs e)
        {
            //Database.LogThis("Session_End", null);
        }

        protected void Application_End(object sender, EventArgs e)
        {
            //Database.LogThis("Application_End", null);
        }
    }
}