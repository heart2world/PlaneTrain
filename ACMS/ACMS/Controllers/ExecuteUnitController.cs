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
    [RoutePrefix("api/execUnit")]
    [RequestAuthorize]
    public class ExecuteUnitController : BaseController
    {
        IExecuteUnitService _execUnitService = new ExecuteUnitService();

        [HttpGet, Route("getlist")]
        public IHttpActionResult GetList()
        {
            return Ok(_execUnitService.GetList());

        }
    }
}
