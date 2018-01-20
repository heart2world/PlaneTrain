using ACMS.EF;
using ACMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACMS.Applications
{
    interface IRoleService
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>用户列表信息</returns>
        PageResult<RoleDto> GetList(int pageSize, int pageNo, string keyWord);


        /// <summary>
        /// /// 根据ID获取机型信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Role Get(string ID);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        OperationResult Add(Role item, string userID);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="item">修改数据实体</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        OperationResult Update(Role item, string userID);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">删除数据主键</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        OperationResult Delete(string ID, string userID);

        /// <summary>
        /// 添加角色菜单
        /// </summary>
        /// <param name="item">角色-菜单（1对N）</param>
        /// <param name="operationUserID">操作人</param>
        /// <returns>操作结果</returns>
        OperationResult AddRoleMenu(RoleMenuDto item, string userID);


        /// <summary>
        /// 根据角色获取该角色的权限菜单
        /// </summary>
        /// <param name="RoleID">角色ID</param>
        /// <returns>权限菜单列表</returns>
        List<Menu> GetMenusByRole(string RoleID);
    }
}
