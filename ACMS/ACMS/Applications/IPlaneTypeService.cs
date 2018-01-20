using ACMS.EF;
using ACMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACMS.Applications
{
    public interface IPlaneTypeService
    {
        /// <summary>
        /// 获取机型列表
        /// </summary>
        /// <returns>用户列表信息</returns>
        PageResult<PlaneType> GetList(int pageSize, int pageNo, string keyWord);


        /// <summary>
        /// /// 根据ID获取机型信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        PlaneType Get(string ID);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        OperationResult Add(PlaneType item, string userID);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="item">修改数据实体</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        OperationResult Update(PlaneType item, string userID);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">删除数据主键</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        OperationResult Delete(string ID, string userID);
    }
}
