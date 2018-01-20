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
    [RoutePrefix("api/PlTypeTCtrlList")]
    //[RequestAuthorize]
    public class PlTypeTCtrlListController : BaseController
    {
        IPlTypeTCtrlListService _service = new PlTypeTCtrlListService();

        [HttpGet, Route("getrlist_history")]
        public IHttpActionResult GetListHistory(int pageSize, int pageNo, string tCtrlID)
        {
            return Ok(_service.GetListHostory(pageSize, pageNo, tCtrlID));

        }

        [HttpGet, Route("getlist")]
        public IHttpActionResult GetList(int pageSize, int pageNo, string keyWord)
        {
            return Ok(_service.GetList(pageSize, pageNo, keyWord));

        }

        [HttpGet, Route("getlistbyplanetype")]
        public IHttpActionResult GetListByPlaneType(int pageSize, int pageNo, string planeTypeID)
        {
            return Ok(_service.GetListByPlaneType(pageSize, pageNo, planeTypeID));

        }

        [HttpPost, Route("add")]
        public IHttpActionResult Add(PlTypeTCtrlList item)
        {
            return Ok(_service.Add(item, base.CurrentUserId));
        }


        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="item">修改数据实体</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        [HttpPost, Route("edit")]
        public IHttpActionResult Update(PlTypeTCtrlList item)
        {
            return Ok(_service.Update(item, base.CurrentUserId));
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
