using ACMS.Models;
using ACMS.Services;
using ACMS.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace ACMS.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        IUserService _userService = new UserService();

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login(UserLoginDto dto)
        {
            var loginUser = _userService.Login(dto);
            if (loginUser != null)
            {
                HttpContext.Current.Session["SESSION_CURRENTUSER"] = loginUser;

                var loginResult = new UserLoginDto() { LoginResult = true, LoginAccount = loginUser.LoginAccount, Password = loginUser.Password, Ticket = loginUser.ID };

                return Ok(loginResult);
            }
            else
            {
                return Ok(new UserLoginDto() { LoginResult = false });
            }
        }
    }
}
