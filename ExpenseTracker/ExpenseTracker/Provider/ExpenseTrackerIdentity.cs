using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ExpenseTracker
{
    public class ExpenseTrackerIdentity : System.Security.Principal.IIdentity
    {
        private FormsAuthenticationTicket _ticket;

        public ExpenseTrackerIdentity(FormsAuthenticationTicket ticket)

        {

            _ticket = ticket;

        }


        public string AuthenticationType

        {

            get { return "Custom"; }

        }

        public bool IsAuthenticated

        {

            get { return true; }

        }

        public string Name

        {

            get { return _ticket.Name; }

        }

        public FormsAuthenticationTicket Ticket

        {

            get { return _ticket; }

        }

        public int UserId

        {

            get

            {

                string[] userDataPieces = _ticket.UserData.Split("|".ToCharArray());

                return Convert.ToInt32(userDataPieces[0]);

            }
        }

        public string UserName

        {

            get

            {

                string[] userDataPieces = _ticket.UserData.Split("|".ToCharArray());

                return userDataPieces[1];

            }
        }

        public string Email

        {

            get

            {

                string[] userDataPieces = _ticket.UserData.Split("|".ToCharArray());

                return userDataPieces[2];

            }
        }

        public int RoleId

        {

            get

            {

                string[] userDataPieces = _ticket.UserData.Split("|".ToCharArray());

                return Convert.ToInt32(userDataPieces[3]);

            }
        }
        public string RoleName

        {

            get

            {

                string[] userDataPieces = _ticket.UserData.Split("|".ToCharArray());

                return userDataPieces[4];

            }
        }
    }
}