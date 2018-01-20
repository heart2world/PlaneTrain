using ACMS.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace ACMS.ApplicationBase
{
    public class RequestAuthorizeAttribute : AuthorizeAttribute
    {
        //重写基类的验证方式，加入我们自定义的Ticket验证
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (HttpContext.Current.Session["SESSION_CURRENTUSER"] != null)
            {
                base.IsAuthorized(actionContext);
            }
            else
            {
                HandleUnauthorizedRequest(actionContext);
            }
        }
    }
}