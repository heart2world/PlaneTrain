using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACMS.EF;
using ACMS.Models;

namespace ACMS.Applications
{
    public interface IReportRecordService
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="reportType"></param>
        /// <param name="keyWord"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        PageResult<ReportRecord> GetList(int pageSize, int pageNo, string reportType,string keyWord,string startDate,string endDate);


        /// <summary>
        /// /// 根据ID获取信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        ReportRecord Get(string ID);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        OperationResult Add(ReportRecord item, string userID);


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">删除数据主键</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        OperationResult Delete(string ID, string userID);
    }
}
