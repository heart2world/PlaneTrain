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
using ACMS.Models;
namespace ACMS.Controllers
{
    [RoutePrefix("api/ReportRecord")]
    [RequestAuthorize]
    public class ReportRecordController : BaseController
    {
        IReportRecordService _service = new ReportRecordService();
        IDailyRecordService _dailyRecordService = new DailyRecordService();

        [HttpGet, Route("getPlaneWorkReport")]
        public IHttpActionResult GetPlaneWorkReport(string startDate, string endDate)
        {
            return Ok(_dailyRecordService.PlaneWorkReportDtoList(startDate,endDate));
        }

        [HttpGet, Route("getPlaneReport")]
        public IHttpActionResult GetPlaneReport(string startDate, string endDate)
        {
            return Ok(_dailyRecordService.PlaneReportDtoList(startDate, endDate));
        }

        
        [HttpGet, Route("getlist")]
        public IHttpActionResult GetList(int pageSize, int pageNo, string reportType, string keyWord, string startDate, string endDate)
        {
            return Ok(_service.GetList(pageSize, pageNo, reportType, keyWord, startDate, endDate));
        }

        [HttpPost, Route("add")]
        public IHttpActionResult Add(ReportRecord item)
        {
            return Ok(_service.Add(item, base.CurrentUserId));
        }



        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">删除数据主键</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        [HttpGet, Route("delete")]
        public IHttpActionResult Delete(string ID)
        {
            return Ok(_service.Delete(ID, base.CurrentUserId));
        }

        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet, Route("get")]
        public IHttpActionResult Get(string ID)
        {
            return Ok(_service.Get(ID));
        }
    }
}
