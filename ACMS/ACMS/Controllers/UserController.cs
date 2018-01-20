using ACMS.ApplicationBase;
using ACMS.EF;
using ACMS.Models;
using ACMS.Services;
using ACMS.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ACMS.Controllers
{
    [RoutePrefix("api/user")]
    [RequestAuthorize]
    public class UserController : BaseController
    {
        IUserService _userService = new UserService();

        [HttpGet, Route("getlist")]
        public IHttpActionResult GetList(int pageSize, int pageNo, string keyWord)
        {
            return Ok(_userService.GetList(pageSize, pageNo, keyWord));
        }

        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet, Route("get")]
        public IHttpActionResult Get(string ID)
        {
            return Ok(_userService.Get(ID));
        }


        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="item">用户Entity</param>
        /// <returns>返回操作结果</returns>
        [HttpPost, Route("add")]
        public IHttpActionResult Add(UserDto item)
        {
            return Ok(_userService.Add(item, base.CurrentUserId));
        }


        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user">用户Entity</param>
        /// <returns>返回操作结果</returns>
        [HttpPost, Route("edit")]
        public IHttpActionResult Edit(UserDto item)
        {
            return Ok(_userService.Edit(item, base.CurrentUserId));
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">需要删除的数据的主键</param>
        /// <param name="operationUserID">操作人ID</param>
        /// <returns>操作结果</returns>
        [HttpGet, Route("delete")]
        public IHttpActionResult Delete(string ID)
        {
            return Ok(_userService.Delete(ID, base.CurrentUserId));
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="operationUserID"></param>
        /// <returns></returns>
        [HttpGet, Route("editPwd")]
        public IHttpActionResult editPwd(string pwd, string operationUserID)
        {
            return Ok(_userService.EditPwd(pwd, operationUserID));
        }
        
    }
}
