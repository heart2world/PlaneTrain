using ACMS.ApplicationBase;
using ACMS.Applications;
using ACMS.Applications.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ACMS.Controllers
{
    [RoutePrefix("api/menu")]
    [RequestAuthorize]
    public class MenuController : BaseController
    {
        IMenuService _menuService = new MenuService();

        [HttpGet, Route("getlist")]
        public IHttpActionResult GetList()
        {
            return Ok(_menuService.GetList());

        }
    }
}
