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
    [RoutePrefix("api/planes")]
    [RequestAuthorize]
    public class PlanesController : BaseController
    {
        IPlanesService _service = new PlanesService();

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


        // <summary>
        /// 根据机型名称获取列表(只针对逐日登记的2种类型)
        /// </summary>
        /// <returns>列表信息</returns>
        [HttpGet, Route("getlistbyplanetypename")]
        public IHttpActionResult GetListByPlaneType(string SpecialPlaneType)
        {
            return Ok(_service.GetListByPlaneType(SpecialPlaneType));
        }

        [HttpPost, Route("add")]
        public IHttpActionResult Add(Planes item)
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
        public IHttpActionResult Update(Planes item)
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
