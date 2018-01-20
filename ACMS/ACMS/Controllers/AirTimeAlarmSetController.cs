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
    [RoutePrefix("api/air_time_alarm_set")]
    [RequestAuthorize]
    public class AirTimeAlarmSetController : BaseController
    {
        IAirTimeAlarmSetService _service = new AirTimeAlarmSetService();

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="item">修改数据实体</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        [HttpPost, Route("edit")]
        public IHttpActionResult Update(AirTimeAlarmSet item)
        {
            return Ok(_service.Update(item, base.CurrentUserId));
        }


        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet, Route("get")]
        public IHttpActionResult Get()
        {
            return Ok(_service.Get("531bfdfe-830a-4d67-ad55-e86a5dc74064"));
        }
    }
}
