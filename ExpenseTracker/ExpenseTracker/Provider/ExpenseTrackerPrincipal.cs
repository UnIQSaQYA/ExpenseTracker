using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseTracker { 
    public class ExpenseTrackerPrincipal : System.Security.Principal.IPrincipal
    {
        private ExpenseTrackerIdentity _identity;

        public ExpenseTrackerPrincipal(ExpenseTrackerIdentity identity)

        {

            _identity = identity;

        }
        public System.Security.Principal.IIdentity Identity

        {

            get { return _identity; }

        }

        public bool IsInRole(string role)
        {
            if (_identity.RoleName.ToLower() == role)
                return true;
            return false;
        }

        public bool hasPermission()
        {
            return false;
        }
    }
}