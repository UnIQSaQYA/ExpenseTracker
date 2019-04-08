using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace ExpenseTracker
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        void Application_OnPostAuthenticateRequest(object sender, EventArgs e)
        {
            // Get a reference to the current User

            IPrincipal usr = HttpContext.Current.User;

            // If we are dealing with an authenticated forms authentication request

            if (usr.Identity.IsAuthenticated && usr.Identity.AuthenticationType == "Forms")

            {

                FormsIdentity fIdent = usr.Identity as FormsIdentity;

                // Create a CustomIdentity based on the FormsAuthenticationTicket  

                ExpenseTrackerIdentity ci = new ExpenseTrackerIdentity(fIdent.Ticket);

                // Create the CustomPrincipal

                ExpenseTrackerPrincipal p = new ExpenseTrackerPrincipal(ci);

                // Attach the CustomPrincipal to HttpContext.User and Thread.CurrentPrincipal

                HttpContext.Current.User = p;

                Thread.CurrentPrincipal = p;
            }
        }
    }
}
