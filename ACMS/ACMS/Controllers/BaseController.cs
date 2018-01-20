using ACMS.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace ACMS.Controllers
{
    public class BaseController : ApiController
    {
        public static string SESSION_CURRENTUSER = "SESSION_CURRENTUSER";

        /// <summary>
        /// 获取当前登录的系统用户,用户登录成功时存入Session中
        /// </summary>
        public User CurrentUser
        {
            get
            {
                return (User)HttpContext.Current.Session[SESSION_CURRENTUSER];
            }
        }

        /// <summary>
        /// 获取当前登录用户的用户Id
        /// </summary>
        public string CurrentUserId
        {
            get
            {
                return CurrentUser != null ? CurrentUser.ID : "";
            }
        }


        /// <summary>
        /// 获取当前登录用户的用户姓名
        /// </summary>
        public string CurrentUserName
        {
            get
            {
                return CurrentUser != null ? CurrentUser.UserName : string.Empty;
            }
        }


        /// <summary>
        /// 获取当前登录用户的登录帐号
        /// </summary>
        public string CurrentLoginAccount
        {
            get
            {
                return CurrentUser != null ? CurrentUser.LoginAccount : string.Empty;
            }
        }
    }
}
