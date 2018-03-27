using ACMS.ApplicationBase;
using ACMS.Applications;
using ACMS.Applications.Impl;
using ACMS.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace ACMS.Controllers
{
    [RoutePrefix("api/dailyRecord")]
    [RequestAuthorize]
    public class DailyRecordController : BaseController
    {
        IDailyRecordService _dailyRecordService = new DailyRecordService();

        /// <summary>
        /// 获取飞行数据统计
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="planTypeID"></param>
        /// <param name="planID"></param>
        /// <param name="ExecUnit"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet, Route("getreportlist")]
        public IHttpActionResult GetDailyRecordReportList(int? pageSize, int? pageNo, string planTypeID, string planID, string ExecUnit, string startDate, string endDate)
        {
            List<string> planTypeIDList = new List<string>();
            planTypeIDList = !string.IsNullOrEmpty(planTypeID) ? planTypeID.Split(',').ToList() : null;
            List<string> planIDList = new List<string>();
            planIDList = !string.IsNullOrEmpty(planID) ? planID.Split(',').ToList() : null;
            List<string> ExecUnitList = new List<string>();
            ExecUnitList = !string.IsNullOrEmpty(ExecUnit) ? ExecUnit.Split(',').ToList() : null;
            int tempPageSize = 1;
            if (pageSize != null)
            {
                tempPageSize = pageSize.Value;
            }
            int tempPageNo = 1;
            if (pageNo != null)
            {
                tempPageNo = pageNo.Value;
            }
            return Ok(_dailyRecordService.GetDailyRecordReportList(tempPageSize, tempPageNo, planTypeIDList, planIDList, ExecUnitList, startDate, endDate));
        }

        #region CESSNA172RDailyRecord

        [HttpGet, Route("cessna172r/getlist")]
        public IHttpActionResult GetCESSNA172RDailyRecordList(int pageSize, int pageNo, string type, string planID, string startDate, string endDate)
        {
            return Ok(_dailyRecordService.GetCESSNA172RDailyRecordList(pageSize, pageNo, type, planID, startDate, endDate));

        }

        [HttpPost, Route("cessna172r/add")]
        public IHttpActionResult Add(CESSNA172RDailyRecord item)
        {
            return Ok(_dailyRecordService.Add(item, base.CurrentUserId));
        }


        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="item">修改数据实体</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        [HttpPost, Route("cessna172r/edit")]
        public IHttpActionResult Update(CESSNA172RDailyRecord item)
        {
            return Ok(_dailyRecordService.Update(item, base.CurrentUserId));
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">删除数据主键</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        [HttpGet, Route("cessna172r/delete")]
        public IHttpActionResult DeleteCESSNA172RDailyRecord(string ID)
        {
            return Ok(_dailyRecordService.DeleteCESSNA172RDailyRecord(ID, base.CurrentUserId));
        }

        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet, Route("cessna172r/get")]
        public IHttpActionResult GetCESSNA172RDailyRecord(string ID)
        {
            return Ok(_dailyRecordService.GetCESSNA172RDailyRecord(ID));
        }

        [HttpGet, Route("cessna172r/checkHaveRecord")]
        public IHttpActionResult CheckCESSNA172RDailyRecordHaveRecord()
        {
            return this.Ok(_dailyRecordService.CheckCESSNA172RDailyRecordHaveRecord());
        }

        /// <summary>
        /// 根据飞机号判断该机是否是第一次登记
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("cessna172r/checkHaveRecordByPlaneID")]
        public IHttpActionResult CheckCESSNA172RDailyRecordHaveRecord(string planeID)
        {
            return this.Ok(_dailyRecordService.CheckCESSNA172RDailyRecordHaveRecord(planeID));
        }


        /// <summary>
        /// 获取CESSNA172R型号初始设置的最后一次记录
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("cessna172r/getLatestRecord")]
        public IHttpActionResult GetLatestCESSNA172RDailyRecord(string planeID)
        {
            return this.Ok(_dailyRecordService.GetLatestCESSNA172RDailyRecord(planeID));
        }


        /// <summary>
        /// 获取上一条记录（不分数据类型）
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("cessna172r/getLastRecordByPlaneID")]
        public IHttpActionResult GetLastCESSNA172RDailyRecordByPlaneID(string planeID)
        {
            return this.Ok(_dailyRecordService.GetLastCESSNA172RDailyRecordByPlaneID(planeID));
        }


        [HttpGet, Route("cessna172r/download/{type}/{planeID}/{startDate}/{endDate}")]
        [AllowAnonymous]
        public HttpResponseMessage DownloadCESSNA172RDailyRecord(string type, string planeID, string startDate, string endDate)
        {
            return _dailyRecordService.GetCESSNA172RDownloadFileStream(type, planeID, startDate, endDate);
        }

        #endregion

        #region PA44-180DailyRecord

        [HttpGet, Route("pa44-180/getlist")]
        public IHttpActionResult GetPA44_180DailyRecordList(int pageSize, int pageNo, string type, string planID, string startDate, string endDate)
        {
            return Ok(_dailyRecordService.GetPA44_180DailyRecordList(pageSize, pageNo, type, planID, startDate, endDate));

        }

        [HttpPost, Route("pa44-180/add")]
        public IHttpActionResult Add(PA44_180DailyRecord item)
        {
            return Ok(_dailyRecordService.Add(item, base.CurrentUserId));
        }


        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="item">修改数据实体</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        [HttpPost, Route("pa44-180/edit")]
        public IHttpActionResult Update(PA44_180DailyRecord item)
        {
            return Ok(_dailyRecordService.Update(item, base.CurrentUserId));
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">删除数据主键</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        [HttpGet, Route("pa44-180/delete")]
        public IHttpActionResult DeletePA44_180DailyRecord(string ID)
        {
            return Ok(_dailyRecordService.DeletePA44_180DailyRecord(ID, base.CurrentUserId));
        }

        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet, Route("pa44-180/get")]
        public IHttpActionResult GetPA44_180DailyRecord(string ID)
        {
            return Ok(_dailyRecordService.GetPA44_180DailyRecord(ID));
        }


        [HttpGet, Route("pa44-180/checkHaveRecord")]
        public IHttpActionResult CheckPA44_180DailyRecordHaveRecord()
        {
            return this.Ok(_dailyRecordService.CheckPA44_180DailyRecordHaveRecord());
        }


        [HttpGet, Route("pa44-180/checkHaveRecordByPlaneID")]
        public IHttpActionResult CheckPA44_180DailyRecordHaveRecord(string planeID)
        {
            return this.Ok(_dailyRecordService.CheckPA44_180DailyRecordHaveRecord(planeID));
        }


        /// <summary>
        /// 获取PA44_180型号初始设置的最后一次记录
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("pa44-180/getLatestRecord")]
        public IHttpActionResult GetLatestPA44_180DailyRecord(string planeID)
        {
            return this.Ok(_dailyRecordService.GetLatestPA44_180DailyRecord(planeID));
        }

        /// <summary>
        /// 获取上一条记录（不分数据类型）
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("pa44-180/getLastRecordByPlaneID")]
        public IHttpActionResult GetLastPA44_180DailyRecordByPlaneID(string planeID)
        {
            return this.Ok(_dailyRecordService.GetLastPA44_180DailyRecordByPlaneID(planeID));
        }


        [HttpGet, Route("pa44-180/download/{type}/{planeID}/{startDate}/{endDate}")]
        [AllowAnonymous]
        public HttpResponseMessage DownloadPA44_180DailyRecord(string type, string planeID, string startDate, string endDate)
        {
            return _dailyRecordService.GetPA44_180DownloadFileStream(type, planeID, startDate, endDate);
        }

        #endregion
    }
}
