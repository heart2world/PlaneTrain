using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ACMS.ApplicationBase;
using ACMS.Applications;
using ACMS.Applications.Impl;
using ACMS.EF;
namespace ACMS.Controllers
{
    [RoutePrefix("api/V_PlanesTCtrlItem_Check")]
    [RequestAuthorize]
    public class V_PlanesTCtrlItem_CheckController : BaseController
    {
        IV_PlanesTCtrlItem_CheckService _service = new V_PlanesTCtrlItem_CheckService();

        [HttpGet, Route("getlist")]
        public IHttpActionResult GetList(int pageSize, int pageNo, int isPrinted, int? days, decimal? airTime, decimal? upTemprTime, int? onOffTime)
        {
            return Ok(_service.GetList(pageSize, pageNo, isPrinted,days,airTime,upTemprTime,onOffTime));

        }
    }
}
