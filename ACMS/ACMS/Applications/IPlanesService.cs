using ACMS.EF;
using ACMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACMS.Applications
{
    public interface IPlanesService
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>用户列表信息</returns>
        PageResult<V_Planes> GetList(int pageSize, int pageNo, string keyWord);

        /// <summary>
        /// 根据机型ID获取列表
        /// </summary>
        /// <returns>列表信息</returns>
        PageResult<Planes> GetListByPlaneType(int pageSize, int pageNo, string PlaneTypeID);

        /// <summary>
        /// /// 根据ID获取信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Planes Get(string ID);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        OperationResult Add(Planes item, string userID);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="item">修改数据实体</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        OperationResult Update(Planes item, string userID);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">删除数据主键</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        OperationResult Delete(string ID, string userID);
    }
}
