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
    public abstract class AdminBaseController : Controller
    {
        public IEnumerable<Authentication.Permission> CurrentUserPermissions { get; set; }
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
    }

    public class AdminExcAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            ServerLogger.Debug("EXCEPTION: ", filterContext.Exception);
            filterContext.ExceptionHandled = true;
        }
    }

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

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var ctr = (AdminBaseController)filterContext.Controller;
            ctr.ViewBag.IsUserLoggedIn = false;

            ServerLogger.Info("Authentication for: " + filterContext.ActionDescriptor.ActionName);

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

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            ServerLogger.Info("Authorization for: " + filterContext.ActionDescriptor.ActionName);

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
                            controller = "AdminLogin",
                            action = "Login"
                        }
                    )
                );

                ctr.ErrorMessage = "User does not have necessary permission";
            }
        }
    }
}