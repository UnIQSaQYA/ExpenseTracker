using ExpenseTracker.BAL;
using ExpenseTracker.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpenseTracker.Controllers
{
    public class BaseController : Controller
    {
        //Default Pagination page size
        protected int pageSize = 6;
        // GET: Base
        #region Messageing
        public bool ShowStatus(int StatusCode, string Message )
        {
            if (Request.IsAjaxRequest())
            {
                return StatusCode == 200 ? true : false;
            }
            else
            {
                if (StatusCode == 200)
                {
                    Success(Message, true);
                    return true;


                }
                else
                {
                    Danger(Message, true);
                    return false;


                }
            }


        }
        #region Alert



        public void Success(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Success, message, dismissable);
        }

        public void Information(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Information, message, dismissable);
        }

        public void Warning(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Warning, message, dismissable);
        }

        public void Danger(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Danger, message, dismissable);
        }

        private void AddAlert(string alertStyle, string message, bool dismissable)
        {
            var alerts = TempData.ContainsKey(Alert.TempDataKey)
                ? (List<Alert>)TempData[Alert.TempDataKey]
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable
            });

            TempData[Alert.TempDataKey] = alerts;
        }
        #endregion
        #endregion
    }
}