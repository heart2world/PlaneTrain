using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACMS.EF;
using ACMS.Models;

namespace ACMS.Applications
{
    public interface IWatchPrintListSerivce
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        OperationResult Add(WatchPrintList item, string userID);

        /// <summary>
        /// 新增批量打印
        /// </summary>
        /// <param name="list"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        OperationResult AddList(List<WatchPrintList> list, string userID);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>列表信息</returns>
        PageResult<WatchPrintList> GetList(int pageSize, int pageNo, string keyWord);
    }
}
