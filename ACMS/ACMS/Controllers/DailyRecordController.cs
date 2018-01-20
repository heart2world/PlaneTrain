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

        #region CESSNA172RDailyRecord

        [HttpGet, Route("cessna172r/getlist")]
        public IHttpActionResult GetCESSNA172RDailyRecordList(int pageSize, int pageNo, int type, string planID, string startDate, string endDate)
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

        #endregion

        #region PA44-180DailyRecord

        [HttpGet, Route("pa44-180/getlist")]
        public IHttpActionResult GetPA44_180DailyRecordList(int pageSize, int pageNo, int type, string planID, string startDate, string endDate)
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

        #endregion
    }
}
