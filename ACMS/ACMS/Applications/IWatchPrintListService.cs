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


        /// <summary>
        /// 增加年份打印数量
        /// </summary>
        /// <param name="year"></param>
        /// <param name="addCount"></param>
        /// <returns></returns>
        OperationResult AddPrintCount(int year, int addCount);


        /// <summary>
        /// 根据年份，以及需要打印的数量获取最大打印数量
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        int GetPrintCountByYear(int year);
    }
}
