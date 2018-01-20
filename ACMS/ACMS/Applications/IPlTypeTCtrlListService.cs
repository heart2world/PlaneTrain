using ACMS.EF;
using ACMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACMS.Applications
{
    public interface IPlTypeTCtrlListService
    {

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>用户列表信息</returns>
        PageResult<V_V_PlTypeTCtrlList_Log> GetListHostory(int pageSize, int pageNo, string tCtrlID);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>用户列表信息</returns>
        PageResult<V_PlTypeTCtrlList> GetList(int pageSize, int pageNo, string keyWord);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>根据机型获取时控项目监控清单</returns>
        PageResult<V_PlTypeTCtrlList> GetListByPlaneType(int pageSize, int pageNo, string PlaneTypeID);

        /// <summary>
        /// /// 根据ID获取信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        PlTypeTCtrlList Get(string ID);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        OperationResult Add(PlTypeTCtrlList item, string userID);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="item">修改数据实体</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        OperationResult Update(PlTypeTCtrlList item, string userID);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">删除数据主键</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        OperationResult Delete(string ID, string userID);
    }
}
