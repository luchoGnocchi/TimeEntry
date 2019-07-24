using GestorInventarioEmpresas.BackEnd.Commons.Enums;
using GestorInventarioEmpresas.BackEnd.Commons.GlobalObjects;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestorInventarioEmpresas.Controllers
{
    public class BaseController : Controller
    {
        #region UserManager
        protected ApplicationSignInManager _signInManager;
        protected ApplicationUserManager _userManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            protected set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            protected set
            {
                _userManager = value;
            }
        }
        #endregion
        #region Notificaciones
        public void Success(string message, string title = null)
        {
            AddAlert(AlertTypeEnum.Success, message, title);
        }

        public void Info(string message, string title = null)
        {
            AddAlert(AlertTypeEnum.Info, message, title);
        }

        public void Warning(string message, string title = null)
        {
            AddAlert(AlertTypeEnum.Warning, message, title);
        }

        public void Error(string message, string title = null)
        {
            AddAlert(AlertTypeEnum.Error, message, title);
        }

        private void AddAlert(AlertTypeEnum alertStyle, string message, string title = null)
        {

            var alerts = TempData.ContainsKey("GestorInventarioAlerts") ? (List<AlertObject>)TempData["GestorInventarioAlerts"] : new List<AlertObject>();

            alerts.Add(new AlertObject
            {
                AlertTypeEnum = alertStyle,
                Message = message,
                Title = title
            });

            TempData["GestorInventarioAlerts"] = alerts;
        }

        #endregion
      public  string[] TypeTaskReservados = new string[] { "DES", "LES", "Vac", "Fer", "DEN" };
    }
}