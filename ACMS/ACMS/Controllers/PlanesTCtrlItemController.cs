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
    [RoutePrefix("api/PlanesTCtrlItem")]
    [RequestAuthorize]
    public class PlanesTCtrlItemController : BaseController
    {
        IPlanesTCtrlItemServices _service = new PlanesTCtrlItemServices();

        [HttpPost, Route("getlist")]
        public IHttpActionResult GetList(QueryParam dto)
        {
            return Ok(_service.GetList(dto.pageSize, dto.pageNo, dto.planeTypeID, dto.planeNo, dto.listID, dto.keyWord));

        }

        public class QueryParam
        {
            public int pageSize { get; set; }
            public int pageNo { get; set; }
            public string planeTypeID { get; set; }
            public string planeNo { get; set; }
            public string listID { get; set; }
            public string keyWord { get; set; }

        }

        [HttpGet, Route("getlist_history")]
        public IHttpActionResult GetListHistory(int pageSize, int pageNo, string itemID)
        {
            return Ok(_service.GetListHistory(pageSize, pageNo, itemID));

        }

        [HttpGet, Route("getallhistorylist")]
        public IHttpActionResult GetAllListHistory(int pageSize, int pageNo)
        {
            return Ok(_service.GetAllListHistory(pageSize, pageNo));

        }

        [HttpPost, Route("add")]
        public IHttpActionResult Add(PlanesTCtrlItemDto item)
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
        public IHttpActionResult Update(PlanesTCtrlItemDto item)
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
