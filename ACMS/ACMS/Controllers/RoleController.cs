using ACMS.ApplicationBase;
using ACMS.Applications;
using ACMS.Applications.Impl;
using ACMS.EF;
using ACMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ACMS.Controllers
{
    [RoutePrefix("api/role")]
    [RequestAuthorize]
    public class RoleController : BaseController
    {
        IRoleService _service = new RoleService();

        [HttpGet, Route("getlist")]
        public IHttpActionResult GetList(int pageSize, int pageNo, string keyWord)
        {
            return Ok(_service.GetList(pageSize, pageNo, keyWord));
        }

        [HttpPost, Route("add")]
        public IHttpActionResult Add(Role item)
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
        public IHttpActionResult Update(Role item)
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


        /// <summary>
        /// 添加角色菜单
        /// </summary>
        /// <param name="item">角色-菜单（1对N）</param>
        /// <param name="operationUserID">操作人</param>
        /// <returns>操作结果</returns>
        [HttpPost, Route("addRoleMenu")]
        public IHttpActionResult AddRoleMenu(RoleMenuDto item)
        {
            return Ok(_service.AddRoleMenu(item, base.CurrentUserId));
        }


        /// <summary>
        /// 根据角色获取该角色的权限菜单
        /// </summary>
        /// <param name="RoleID">角色ID</param>
        /// <returns>权限菜单列表</returns>
        [HttpGet, Route("getMenusByRole")]
        public IHttpActionResult GetMenusByRole(string RoleID)
        {
            return Ok(_service.GetMenusByRole(RoleID));
        }
    }
}
