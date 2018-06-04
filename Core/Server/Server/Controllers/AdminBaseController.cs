using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using Server.Authentication;
using Server.Models;
using Server.Objects;
using Permission = Server.Authentication.Permission;

namespace Server.Controllers
{
    /// <summary>
    /// Základni třída pro všechny kontrolery, kterými se ovládá chování nebo data v Backupperu
    /// Obsahuje logiku pro kontrolu přihlášení uživatele
    /// </summary>
    public abstract class AdminBaseController : Controller
    {
        public IEnumerable<Permission> CurrentUserPermissions { get; set; }
        public IEnumerable<Models.Group> CurrentUserGroups { get; set; }
        public Models.User CurrentUser { get; set; }

        public string OperationResultMessage
        {
            get => TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE].ToString();
            set => TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = value;
        }

        public string ErrorMessage
        {
            get => TempData[Objects.MagicStrings.ERROR_MESSAGE].ToString();
            set => TempData[Objects.MagicStrings.ERROR_MESSAGE] = value;
        }

        public Guid? SessionUuid
        {
            get => (Guid?)Session[Objects.MagicStrings.SESSION_UUID];
            set => Session[Objects.MagicStrings.SESSION_UUID] = value;
        }

        /// <summary>
        /// Jakákoliv výjimka, která se sem dostane bude zobrazena na AdminError stránce
        /// </summary>
        /// <param name="fc"></param>
        protected override void OnException(ExceptionContext fc)
        {
            ServerLogger.Debug("EXCEPTION: ", fc.Exception);

            if (fc.ExceptionHandled)
            {
                return;
            }

            fc.Result = new RedirectToRouteResult(
                new RouteValueDictionary(
                    new
                    {
                        controller = "AdminError",
                        action = "Index"
                    }
                )
            );

            ((AdminBaseController)fc.Controller).ErrorMessage = fc.Exception.Message;
            fc.ExceptionHandled = true;
        }
    }

    /// <summary>
    /// Pokud je použit zajistí, že se k metodě v kontroleru dostanou přihlášení uživatelé se specifikovanými permisemi
    /// </summary>
    public class AdminSecAttribute : FilterAttribute, IAuthenticationFilter, IAuthorizationFilter
    {
        private readonly Permission[] _requiredPermissions;

        /// <summary>
        /// Constructor: if any permissions are specified then user is required to have at least one of them
        /// </summary>
        /// <param name="permissions"></param>
        public AdminSecAttribute(params Permission[] permissions)
        {
            _requiredPermissions = permissions;
        }

        /// <summary>
        /// Přihlásí uživatele
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var ctr = (AdminBaseController)filterContext.Controller;
            ctr.ViewBag.IsUserLoggedIn = false;

            ServerLogger.Information("Authentication for: " + filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "/" + filterContext.ActionDescriptor.ActionName);

            if (!Util.IsUserAlreadyLoggedIn(ctr.Session))
            {
                ctr.ViewBag.IsUserLogedIn = false;

                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                            new
                            {
                                controller = "AdminLogin",
                                action = "Login"
                            }
                        )
                );

                return;
            }

            ctr.ViewBag.IsUserLoggedIn = true;
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext) { }

        /// <summary>
        /// Načte permise z databáze a zkontroluje, že uživatel může pokračovat dál
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            ServerLogger.Information("Authotization for: " + filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "/" + filterContext.ActionDescriptor.ActionName);

            var ctr = (AdminBaseController)filterContext.Controller;

            using (var db = new MySQLContext())
            {
                var auth = new Authenticator(db);
                ctr.CurrentUser = auth.GetUserFromUuid(ctr.SessionUuid.Value);
                ctr.CurrentUserPermissions = auth.GetPermissionsFromUserId(ctr.CurrentUser.Id);
                ctr.CurrentUserGroups = auth.GetGroupsFromUserId(ctr.CurrentUser.Id);
            }

            if (_requiredPermissions == null || _requiredPermissions.Length == 0 || ctr.CurrentUserPermissions.Contains(Permission.SKIP))
                return;

            bool foundPermission = false;
            foreach (var item in _requiredPermissions)
            {
                if (ctr.CurrentUserPermissions.Contains(item))
                {
                    foundPermission = true;
                    break;
                }
            }

            if (!foundPermission)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            controller = "Admin",
                            action = "Index"
                        }
                    )
                );

                ctr.OperationResultMessage = "User does not have necessary permission";
#if DEBUG
                ctr.OperationResultMessage += @"| Missing permissions are: ";
                foreach (var i in _requiredPermissions.Where(x => !ctr.CurrentUserPermissions.Contains(x)))
                {
                    ctr.OperationResultMessage += @", " + i.ToString();
                }
#endif
            }
        }
    }
}