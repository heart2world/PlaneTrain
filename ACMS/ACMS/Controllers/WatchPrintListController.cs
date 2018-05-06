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
    [RoutePrefix("api/WatchPrintList")]
    [RequestAuthorize]
    public class WatchPrintListController : BaseController
    {
        IWatchPrintListSerivce _service = new WatchPrintListSerivce();

        [HttpGet, Route("getlist")]
        public IHttpActionResult GetList(int pageSize, int pageNo, string keyWord)
        {
            return Ok(_service.GetList(pageSize, pageNo, keyWord));

        }

        [HttpPost, Route("add")]
        public IHttpActionResult Add(WatchPrintList item)
        {
            return Ok(_service.Add(item, base.CurrentUserId));
        }

        [HttpPost, Route("addList")]
        public IHttpActionResult AddList(Params param)//List<WatchPrintList> list)
        {
            return Ok(_service.AddList(param.list, base.CurrentUserId));
        }

        [HttpGet, Route("addPrintCount")]
        public IHttpActionResult AddPrintCount(int year, int addCount)
        {
            return Ok(_service.AddPrintCount(year, addCount));
        }

        [HttpGet, Route("getPrintCountByYear")]
        public IHttpActionResult GetPrintCountByYear(int year)
        {
            return Ok(_service.GetPrintCountByYear(year));
        }
    }

    public class Params
    {
        public List<WatchPrintList> list { get; set; }
    }
}
