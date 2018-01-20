using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACMS.EF;
using ACMS.Models;

namespace ACMS.Applications
{
    public interface IPlanesTCtrlItemServices
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>用户列表信息</returns>
        PageResult<V_PlanesTCtrlItem> GetList(int pageSize, int pageNo, string planeTypeID, string planeNo, string listID, string keyWord);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>用户列表信息</returns>
        PageResult<V_PlanesTCtrlItem_Log> GetListHistory(int pageSize, int pageNo, string itemID);

        /// <summary>
        /// /// 根据ID获取信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        PlanesTCtrlItemDto Get(string ID);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        OperationResult Add(PlanesTCtrlItemDto item, string userID);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="item">修改数据实体</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        OperationResult Update(PlanesTCtrlItemDto item, string userID);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">删除数据主键</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        OperationResult Delete(string ID, string userID);
    }
}
