using ACMS.EF;
using ACMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace ACMS.Applications
{
    public interface IDailyRecordService
    {


        /// <summary>
        /// 飞机生产信息月报
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        PageResult<PlaneWorkReportDto> PlaneWorkReportDtoList(string startDate, string endDate);

        /// <summary>
        /// 飞机月报
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        PageResult<PlaneReportDto> PlaneReportDtoList(string startDate, string endDate);

        /// <summary>
        /// 发动机报表
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        PageResult<EngineReportDto> EngineReportDtoList(string startDate, string endDate);

        /// <summary>
        /// 发动机月度报表
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        PageResult<EngineReportDto> EngineMonthReportDtoList(string startDate, string endDate);
        /// <summary>
        /// 飞行数据统计
        /// </summary>
        /// <param name="planTypeID"></param>
        /// <param name="planID"></param>
        /// <param name="ExecUnit"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        PageResult<DailyRecordReportDto> GetDailyRecordReportList(int pageSize, int pageNo, List<string> planTypeID, List<string> planID, List<string> ExecUnit, string startDate, string endDate);

        #region CESSNA172RDailyRecord
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="pageSize">每页数量</param>
        /// <param name="pageNo">页码</param>
        /// <param name="type">数据性质</param>
        /// <param name="planID">飞机ID</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>数据列表</returns>
        PageResult<CESSNA172RDailyRecordDto> GetCESSNA172RDailyRecordList(int pageSize, int pageNo, string type, string planID, string startDate, string endDate);


        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <param name="ID">主键</param>
        /// <returns>数据实体</returns>
        CESSNA172RDailyRecord GetCESSNA172RDailyRecord(string ID);


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item">新增实体</param>
        /// <param name="userID">操作人ID</param>
        /// <returns>操作结果</returns>
        OperationResult Add(CESSNA172RDailyRecord item, string userID);


        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="item">修改数据实体</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        OperationResult Update(CESSNA172RDailyRecord item, string userID);


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">删除数据主键</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        OperationResult DeleteCESSNA172RDailyRecord(string ID, string userID);

        /// <summary>
        /// 判断该机型是否是第一次登记
        /// </summary>
        /// <returns></returns>
        bool CheckCESSNA172RDailyRecordHaveRecord();


        /// <summary>
        /// 根据飞机号判断该机是否是第一次登记
        /// </summary>
        /// <returns></returns>
        bool CheckCESSNA172RDailyRecordHaveRecord(string planeID);

        /// <summary>
        /// 获取CESSNA172R型号初始设置的最后一次记录
        /// </summary>
        /// <returns></returns>
        CESSNA172RDailyRecord GetLatestCESSNA172RDailyRecord();

        HttpResponseMessage GetCESSNA172RDownloadFileStream(string type, string planeID, string startDate, string endDate);

        #endregion

        #region PA44-180DailyRecord

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="pageSize">每页数量</param>
        /// <param name="pageNo">页码</param>
        /// <param name="type">数据性质</param>
        /// <param name="planID">飞机ID</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>数据列表</returns>
        PageResult<PA44_180DailyRecordDto> GetPA44_180DailyRecordList(int pageSize, int pageNo, string type, string planID, string startDate, string endDate);

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <param name="ID">主键</param>
        /// <returns>数据实体</returns>
        PA44_180DailyRecord GetPA44_180DailyRecord(string ID);


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item">新增实体</param>
        /// <param name="userID">操作人ID</param>
        /// <returns>操作结果</returns>
        OperationResult Add(PA44_180DailyRecord item, string userID);


        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="item">修改数据实体</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        OperationResult Update(PA44_180DailyRecord item, string userID);


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">删除数据主键</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        OperationResult DeletePA44_180DailyRecord(string ID, string userID);


        /// <summary>
        /// 判断该机型是否是第一次登记
        /// </summary>
        /// <returns></returns>
        bool CheckPA44_180DailyRecordHaveRecord();


        /// <summary>
        /// 根据飞机号判断该机是否是第一次登记
        /// </summary>
        /// <returns></returns>
        bool CheckPA44_180DailyRecordHaveRecord(string planeID);

        /// <summary>
        /// 获取PA44_180型号初始设置的最后一次记录
        /// </summary>
        /// <returns></returns>
        PA44_180DailyRecord GetLatestPA44_180DailyRecord();


        HttpResponseMessage GetPA44_180DownloadFileStream(string type, string planeID, string startDate, string endDate);

        #endregion
    }
}
