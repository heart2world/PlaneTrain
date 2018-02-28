using ACMS.ApplicationBase;
using ACMS.EF;
using ACMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Util;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ACMS.Applications.Impl
{
    public class DailyRecordService : ServiceBase, IDailyRecordService
    {
        private DbContext _dbContext = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DailyRecordService()
        {
            _dbContext = base.CreateDbContext();
            base.AddDisposableObject(_dbContext);

        }


        /// <summary>
        /// 飞机生产信息月报
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public PageResult<PlaneWorkReportDto> PlaneWorkReportDtoList(string startDate, string endDate)
        {
            PageResult<PlaneWorkReportDto> list = new PageResult<PlaneWorkReportDto>();

            if (_dbContext == null)
            {
                _dbContext = base.CreateDbContext();
            }
            var result = _dbContext.Set<V_DailyRecordReport>().Where(a => a.IsActive);
            if (!string.IsNullOrEmpty(startDate))
            {
                result = result.Where(x => string.Compare(x.CreateTime, startDate) > 0);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                result = result.Where(x => string.Compare(x.CreateTime, endDate) < 0);
            }
            list.ResultData = result.GroupBy(a => new { a.TypeName, a.PlaneTypeID })
                         .Select(g => new PlaneWorkReportDto()
                         {
                             DayRiseAndFallNum = g.Sum(i => i.DayRiseAndFallNum),
                             PlanDayAirTime = g.Sum(i => i.PlanDayAirTime),
                             PlanDayClearingTime = g.Sum(i => i.PlanDayClearingTime),
                             PlaneTypeID = g.Key.PlaneTypeID,
                             TypeName = g.Key.TypeName
                         }).ToList();
            if (!string.IsNullOrEmpty(startDate))
            {
                result = result.Where(a => string.Compare(a.InputDate, startDate) >= 0);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                result = result.Where(a => string.Compare(a.InputDate, endDate) <= 0);
            }

            #region 用于统计机型下的飞机数目
            if (list.ResultData != null && list.ResultData.Count > 0)
            {
                //用于统计机型下的飞机数目
                var result2 = (from a in _dbContext.Set<V_DailyRecordReport>().Where(a => a.IsActive)
                               where string.Compare(a.InputDate, startDate) >= 0 && string.Compare(a.InputDate, endDate) <= 0
                               select new { PlaneNo = a.PlaneNo, PlaneTypeID = a.PlaneTypeID }

                              ).Distinct().ToList();
                foreach (var item in list.ResultData)
                {
                    item.PlanesCount = result2.Where(m => m.PlaneTypeID == item.PlaneTypeID).Count();
                }
            }
            #endregion
            list.Total = list.ResultData.Count();
            return list;
        }

        /// <summary>
        /// 飞机月报
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public PageResult<PlaneReportDto> PlaneReportDtoList(string startDate, string endDate)
        {
            PageResult<PlaneReportDto> list = new PageResult<PlaneReportDto>();

            if (_dbContext == null)
            {
                _dbContext = base.CreateDbContext();
            }
            var result = _dbContext.Set<V_RecordMonthReport>().Where(a => a.IsActive);
            if (!string.IsNullOrEmpty(startDate))
            {
                result = result.Where(x => string.Compare(x.CreateTime, startDate) > 0);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                result = result.Where(x => string.Compare(x.CreateTime, endDate) < 0);
            }
            list.ResultData = result.GroupBy(a => new { a.TypeName, a.PlaneNo })
                         .Select(g => new PlaneReportDto()
                         {
                             DayRiseAndFallNum = g.Sum(i => i.DayRiseAndFallNum),
                             PlanDayAirTime = g.Sum(i => i.PlanDayAirTime),
                             PlanDayClearingTime = g.Sum(i => i.PlanDayClearingTime),
                             PlaneNo = g.Key.PlaneNo,
                             TypeName = g.Key.TypeName
                         }).ToList();
            /*if (!string.IsNullOrEmpty(startDate))
            {
                result = result.Where(a => string.Compare(a.InputDate, startDate) >= 0);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                result = result.Where(a => string.Compare(a.InputDate, endDate) <= 0);
            }*/

            #region 用于计算飞机自新数据
            if (list.ResultData != null && list.ResultData.Count > 0)
            {
                //用于统计机型下的飞机数目
                var result2 = (from a in _dbContext.Set<V_RecordMonthReport>().Where(a => a.IsActive)
                               where string.Compare(a.InputDate, startDate) >= 0 && string.Compare(a.InputDate, endDate) <= 0
                               select a

                              ).OrderByDescending(m => m.InputDate);
                foreach (var item in list.ResultData)
                {
                    var tempItem = result2.Where(m => m.PlaneNo == item.PlaneNo).First();
                    item.PlanNewAirTime = tempItem.PlanNewAirTime;
                    item.PlanNewClearingTime = tempItem.PlanNewClearingTime;
                    item.PlanNewRiseAndFallNum = tempItem.PlanNewRiseAndFallNum;
                }
            }
            #endregion
            list.Total = list.ResultData.Count();
            return list;
        }

        /// <summary>
        /// 发动机报表
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public PageResult<EngineReportDto> EngineReportDtoList(string startDate, string endDate)
        {
            PageResult<EngineReportDto> list = new PageResult<EngineReportDto>();

            if (_dbContext == null)
            {
                _dbContext = base.CreateDbContext();
            }
            var result = _dbContext.Set<V_EngineReport>().Where(a => a.IsActive && string.Compare(a.InputDate, startDate) >= 0
            && string.Compare(a.InputDate, endDate) <= 0 && a.Type == 2);
            list.ResultData = result.GroupBy(a => new { a.PlaneNo, a.EngineNo, a.Position })
                         .Select(g => new EngineReportDto()
                         {
                             EngineCorrectTSO = g.Sum(i => i.EngineCorrectTSO),
                             EngineNewTSN = g.Sum(i => i.EngineNewTSN),
                             PlaneNo = g.Key.PlaneNo,
                             Position = g.Key.Position,
                             EngineNo = g.Key.EngineNo
                         }).ToList();
            //剔除 新增发动机首月不统计 
            if (list.ResultData != null && list.ResultData.Count > 0)
            {
                var tempList = _dbContext.Set<V_EngineReport>().Where(a => a.IsActive && a.Type == 1 &&
                string.Compare(a.InputDate, startDate) >= 0 && string.Compare(a.InputDate, endDate) <= 0
                ).ToList();
                List<EngineReportDto> tempList2 = new List<EngineReportDto>();
                foreach (var item in list.ResultData)
                {//复制列表至临时表中
                    tempList2.Add(item);
                }
                foreach (var item in tempList2)
                {//剔除 数据
                    var tempResult = tempList.Where(a => string.Compare(a.EngineNo, item.EngineNo, true) == 0
                    && string.Compare(a.PlaneNo, item.PlaneNo) == 0 && string.Compare(a.Position, item.Position) == 0).ToList();
                    if (tempResult != null && tempResult.Count() > 0)
                    {
                        list.ResultData.Remove(item);
                    }

                }
            }
            //查询飞机本月空中时间
            if (list.ResultData != null && list.ResultData.Count > 0)
            {
                var result2 = _dbContext.Set<V_EngineReport>().Where(a => a.IsActive && a.Type == 2 &&
                string.Compare(a.InputDate, startDate) >= 0 && string.Compare(a.InputDate, endDate) <= 0
                ).ToList();
                foreach (var item in list.ResultData)
                {
                    var tempResult = result2.Where(a => a.EngineNo == item.EngineNo && a.PlaneNo == item.PlaneNo).ToList();
                    item.PlanDayAirTime = tempResult.Sum(a => a.PlanDayAirTime);
                }
            }
            list.Total = list.ResultData.Count();
            return list;
        }

        /// <summary>
        /// 发动机月报表
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public PageResult<EngineReportDto> EngineMonthReportDtoList(string startDate, string endDate)
        {
            PageResult<EngineReportDto> list = new PageResult<EngineReportDto>();

            if (_dbContext == null)
            {
                _dbContext = base.CreateDbContext();
            }
            var result = _dbContext.Set<V_EngineReport>().Where(a => a.IsActive && string.Compare(a.InputDate, startDate) >= 0
            && string.Compare(a.InputDate, endDate) <= 0 && a.Type == 2);
            list.ResultData = result.GroupBy(a => new { a.PlaneNo, a.EngineNo, a.Position, a.TypeName })
                         .Select(g => new EngineReportDto()
                         {
                             EngineCorrectTSO = g.Sum(i => i.EngineCorrectTSO),
                             EngineNewTSN = g.Sum(i => i.EngineNewTSN),
                             PlaneNo = g.Key.PlaneNo,
                             Position = g.Key.Position,
                             EngineNo = g.Key.EngineNo,
                             PlaneTypeName = g.Key.TypeName
                         }).ToList();

            //查询飞机本月空中时间
            if (list.ResultData != null && list.ResultData.Count > 0)
            {
                var result2 = _dbContext.Set<V_EngineReport>().Where(a => a.IsActive && a.Type == 2 &&
                string.Compare(a.InputDate, startDate) >= 0 && string.Compare(a.InputDate, endDate) <= 0
                ).ToList();
                foreach (var item in list.ResultData)
                {
                    var tempResult = result2.Where(a => a.EngineNo == item.EngineNo && a.PlaneNo == item.PlaneNo).ToList();
                    item.PlanDayAirTime = tempResult.Sum(a => a.PlanDayAirTime);
                }
            }

            //查询发动机本月状态
            if (list.ResultData != null && list.ResultData.Count > 0)
            {
                var result3 = _dbContext.Set<V_EngineReport>().Where(a => a.IsActive && a.Type == 1 &&
                string.Compare(a.InputDate, startDate) >= 0 && string.Compare(a.InputDate, endDate) <= 0
                ).ToList();
                foreach (var item in list.ResultData)
                {
                    var temp = result3.Where(a => a.Position == item.Position && a.PlaneNo == item.PlaneNo).ToList();
                    if (temp != null && temp.Count > 0 && temp.Where(m => string.Compare(m.EngineNo, item.EngineNo, true) == 0).Count() == 0)
                    {//如果在初值中有设置过 飞机的发动机  则之前的发动机表示为：拆卸了的
                        item.EngineStatus = "拆卸";
                    }
                    else
                    {
                        item.EngineStatus = "装机";
                    }
                }
            }

            list.Total = list.ResultData.Count();
            return list;
        }

        /// <summary>
        /// 飞机数据统计
        /// </summary>
        /// <param name="planTypeID"></param>
        /// <param name="planID"></param>
        /// <param name="ExecUnit"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public PageResult<DailyRecordReportDto> GetDailyRecordReportList(List<string> planTypeID, List<string> planID, List<string> ExecUnit, string startDate, string endDate)
        {
            PageResult<DailyRecordReportDto> list = new PageResult<DailyRecordReportDto>();

            if (_dbContext == null)
            {
                _dbContext = base.CreateDbContext();
            }
            var result = _dbContext.Set<V_DailyRecordReport>().Where(a => a.IsActive && a.Type == 2);//初值不统计
            if (!string.IsNullOrEmpty(startDate))
            {
                result = result.Where(x => string.Compare(x.InputDate, startDate) >= 0);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                result = result.Where(x => string.Compare(x.InputDate, endDate) <= 0);
            }
            list.ResultData = result.GroupBy(a => new { a.TypeName, a.PlanID, a.PlaneNo, a.PlaneTypeID })
                         .Select(g => new DailyRecordReportDto()
                         {
                             DayMaintenaceTime = g.Sum(i => i.DayMaintenaceTime),
                             ExecUnit = "遂宁飞行学院",
                             DayRiseAndFallNum = g.Sum(i => i.DayRiseAndFallNum),
                             HeatingMachineDayTime = g.Sum(i => i.HeatingMachineDayTime),
                             PlanDayAirTime = g.Sum(i => i.PlanDayAirTime),
                             PlanDayClearingTime = g.Sum(i => i.PlanDayClearingTime),
                             PlanDayGroundTime = g.Sum(i => i.PlanDayGroundTime),
                             PlaneNo = g.Key.PlaneNo,
                             PlanID = g.Key.PlanID,
                             TypeName = g.Key.TypeName,
                             //FlightDays=10,
                             PlaneTypeID = g.Key.PlaneTypeID

                         }).OrderByDescending(m => m.PlanID).ToList();
            //机号查询
            if (planID != null && planID.Count > 0)
            {
                foreach (string item in planID)
                {
                    list.ResultData = list.ResultData.Where(x => x.PlanID == item).ToList();
                }

            }
            //机型查询
            if (planTypeID != null && planTypeID.Count > 0)
            {
                foreach (string item in planTypeID)
                {
                    list.ResultData = list.ResultData.Where(x => x.PlaneTypeID == item).ToList();
                }

            }

            //执行单位查询
            if (ExecUnit != null && ExecUnit.Count > 0)
            {
                foreach (string item in ExecUnit)
                {
                    list.ResultData = list.ResultData.Where(x => x.ExecUnit == item).ToList();
                }

            }
            if (list != null && list.ResultData.Count > 0)
            {
                //飞行天数查询
                var result2 = _dbContext.Set<V_DailyRecordReport>().Where(a => a.IsActive);
                if (!string.IsNullOrEmpty(startDate))
                {
                    result2 = result2.Where(x => string.Compare(x.CreateTime, startDate) > 0);
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    result2 = result2.Where(x => string.Compare(x.CreateTime, endDate) < 0);
                }

                //汇总每日每个飞机的登记数据
                var result3 = (from item in result2
                               group item by new { item.PlaneNo, item.InputDate }
                                   into temp
                                   select new
                                   {
                                       InputDate = temp.Key.InputDate,
                                       PlaneNo = temp.Key.PlaneNo,
                                       Count = temp.Count()
                                   }).ToList();
                foreach (var item in list.ResultData)
                {
                    item.FlightDays = result3.Where(m => m.PlaneNo == item.PlaneNo).Count();
                }
            }

            list.Total = list.ResultData.Count();
            return list;
        }
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
        public PageResult<CESSNA172RDailyRecordDto> GetCESSNA172RDailyRecordList(int pageSize, int pageNo, string type, string planID, string startDate, string endDate)
        {
            PageResult<CESSNA172RDailyRecordDto> list = new PageResult<CESSNA172RDailyRecordDto>();

            if (_dbContext == null)
            {
                _dbContext = base.CreateDbContext();
            }
            var result = from a in _dbContext.Set<CESSNA172RDailyRecord>()//.Where(a => a.IsActive);
                         join b in _dbContext.Set<Planes>() on a.PlanID equals b.ID
                         where a.IsActive && b.IsActive
                         select new CESSNA172RDailyRecordDto()
                         {
                             ID = a.ID,
                             Type = a.Type,
                             PlanID = a.PlanID,
                             PlanTypeID = b.PlaneTypeID,
                             PlaneNo = b.PlaneNo,
                             InputDate = a.InputDate,
                             DayClearingTime = a.DayClearingTime,
                             DayRiseAndFallNum = a.DayRiseAndFallNum,
                             DayAirTime = a.DayAirTime,
                             DayMaintenaceTime = a.DayMaintenaceTime,
                             CorrectClearingTime = a.CorrectClearingTime,
                             CorrectAirTime = a.CorrectAirTime,
                             PlanNewClearingTime = a.PlanNewClearingTime,
                             PlanNewAirTime = a.PlanNewAirTime,
                             PlanNewRiseAndFallNum = a.PlanNewRiseAndFallNum,
                             PlanDayClearingTime = a.PlanDayClearingTime,
                             PlanDayAirTime = a.PlanDayAirTime,
                             PlanDayGroundTime = a.PlanDayGroundTime,
                             EngineType = a.EngineType,
                             EngineNo = a.EngineNo,
                             EngineCorrectTSO = a.EngineCorrectTSO,
                             EngineNewTSN = a.EngineNewTSN,
                             ExecUnit = a.ExecUnit,
                             Memo = a.Memo,
                             IsActive = a.IsActive,
                             CreateTime = a.CreateTime,
                             Creator = a.Creator,
                             UpdateTime = a.UpdateTime,
                             Updator = a.Updator
                         };

            //如果没选择数据性质，则返回所有的数据
            if (!string.IsNullOrEmpty(type) && type != "-1")
            {
                result = result.Where(x => x.Type.ToString() == type);
            }
            if (!string.IsNullOrEmpty(planID) && planID != "-1")
            {
                result = result.Where(x => x.PlanID == planID);
            }
            if (!string.IsNullOrEmpty(startDate))
            {
                result = result.Where(x => string.Compare(x.InputDate, startDate, StringComparison.Ordinal) >= 0);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                result = result.Where(x => string.Compare(x.InputDate, endDate, StringComparison.Ordinal) <= 0);
            }

            list.Total = result.Count();
            result = result.OrderByDescending(a => a.CreateTime).Skip((pageNo - 1) * pageSize).Take(pageSize);
            list.ResultData = result.ToList();
            return list;
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <param name="ID">主键</param>
        /// <returns>数据实体</returns>
        public CESSNA172RDailyRecord GetCESSNA172RDailyRecord(string ID)
        {
            var query = _dbContext.Set<CESSNA172RDailyRecord>().Where(x => x.ID == ID && x.IsActive).FirstOrDefault();
            return query;
        }

        /// <summary>
        /// 获取上一条记录（不分数据类型）
        /// </summary>
        /// <returns></returns>
        private CESSNA172RDailyRecord GetLastCESSNA172RDailyRecord()
        {
            var query = _dbContext.Set<CESSNA172RDailyRecord>().OrderByDescending(o => o.CreateTime).FirstOrDefault();
            return query;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item">新增实体</param>
        /// <param name="userID">操作人ID</param>
        /// <returns>操作结果</returns>
        public OperationResult Add(CESSNA172RDailyRecord item, string userID)
        {
            //获取上一条记录
            var lastRecord = GetLastCESSNA172RDailyRecord();

            try
            {
                item.ID = Guid.NewGuid().ToString();
                item.CreateTime = item.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                item.Creator = item.Updator = userID;
                item.IsActive = true;
                //如果数据是普通逐日，则需要将部分字段根据页面上录入的值自动计算并做保存；如果数据是初值，则不需要自动计算步骤，因为所有字段均为手工录入
                if (item.Type == 2)
                {
                    //自新空中时间=空中时间（表）+空中时间（表）修正；
                    item.PlanNewAirTime = item.DayAirTime + item.CorrectAirTime;
                    //自新空地时间=空地时间（表）+空地时间（表）修正；
                    item.PlanNewClearingTime = item.DayClearingTime + item.CorrectClearingTime;
                    //自新起落次数=本日起落+自新起落（上一条的记录）
                    item.PlanNewRiseAndFallNum = item.DayRiseAndFallNum + (lastRecord == null ? 0 : lastRecord.PlanNewRiseAndFallNum);
                    //当日空地时间=自新空地时间-自新空地时间（上一条的记录）；
                    item.PlanDayClearingTime = item.PlanNewClearingTime - (lastRecord == null ? 0 : lastRecord.PlanNewClearingTime);
                    //当日空中时间=自新空中时间-自新空中时间（上一条的记录）；
                    item.PlanDayAirTime = item.PlanNewAirTime - (lastRecord == null ? 0 : lastRecord.PlanNewAirTime);
                    //当日地面时间=自新空中时间-自新空地时间；
                    item.PlanDayGroundTime = item.PlanNewAirTime - item.PlanNewClearingTime;


                    //修后时间=当日空中时间+上一条修后时间
                    item.EngineCorrectTSO = item.PlanDayAirTime + (lastRecord == null ? 0 : lastRecord.EngineCorrectTSO);
                    //自新时间=当日空中时间+上一条自新时间
                    item.EngineNewTSN = item.PlanDayAirTime + (lastRecord == null ? 0 : lastRecord.EngineNewTSN);
                }

                _dbContext.Set<CESSNA172RDailyRecord>().Add(item);
                _dbContext.SaveChanges();
                return new OperationResult()
                {
                    Result = true
                };
            }
            catch (Exception ex)
            {
                return new OperationResult()
                {
                    Result = false,
                    ResultMsg = ex.Message
                };
            }

        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="item">修改数据实体</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        public OperationResult Update(CESSNA172RDailyRecord item, string userID)
        {
            var editModel = _dbContext.Set<CESSNA172RDailyRecord>().Where(x => x.ID == item.ID).FirstOrDefault();
            if (editModel != null)
            {
                try
                {
                    //如果修改的数据是普通数据，则需要将下一条初值之前的所有普通数据都做一次修改。
                    if (item.Type == 2)
                    {

                    }
                    //修改信息
                    editModel.Type = item.Type;
                    editModel.PlanID = item.PlanID;
                    editModel.InputDate = item.InputDate;
                    editModel.DayClearingTime = item.DayClearingTime;
                    editModel.DayRiseAndFallNum = item.DayRiseAndFallNum;
                    editModel.DayAirTime = item.DayAirTime;
                    editModel.DayMaintenaceTime = item.DayMaintenaceTime;
                    editModel.CorrectClearingTime = item.CorrectClearingTime;
                    editModel.CorrectAirTime = item.CorrectAirTime;
                    editModel.PlanNewClearingTime = item.PlanNewClearingTime;
                    editModel.PlanNewAirTime = item.PlanNewAirTime;
                    editModel.PlanNewRiseAndFallNum = item.PlanNewRiseAndFallNum;
                    editModel.PlanDayClearingTime = item.PlanDayClearingTime;
                    editModel.PlanDayAirTime = item.PlanDayAirTime;
                    editModel.PlanDayGroundTime = item.PlanDayGroundTime;
                    editModel.EngineType = item.EngineType;
                    editModel.EngineNo = item.EngineNo;
                    editModel.EngineCorrectTSO = item.EngineCorrectTSO;
                    editModel.EngineNewTSN = item.EngineNewTSN;
                    editModel.ExecUnit = item.ExecUnit;
                    editModel.Memo = item.Memo;
                    editModel.Updator = userID;
                    editModel.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    _dbContext.SaveChanges();

                    return new OperationResult()
                    {
                        Result = true
                    };
                }
                catch (Exception ex)
                {
                    return new OperationResult()
                    {
                        Result = false,
                        ResultMsg = ex.Message
                    };
                }
            }
            else
            {
                return new OperationResult()
                {
                    Result = false,
                    ResultMsg = "正在修改的数据不存在！"
                };
            }
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">删除数据主键</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        public OperationResult DeleteCESSNA172RDailyRecord(string ID, string userID)
        {
            var editModel = _dbContext.Set<CESSNA172RDailyRecord>().Where(x => x.ID == ID).FirstOrDefault();
            if (editModel != null)
            {
                try
                {
                    //修改信息
                    editModel.IsActive = false;
                    editModel.Updator = userID;
                    editModel.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    _dbContext.SaveChanges();

                    return new OperationResult()
                    {
                        Result = true
                    };
                }
                catch (Exception ex)
                {
                    return new OperationResult()
                    {
                        Result = false,
                        ResultMsg = ex.Message
                    };
                }
            }
            else
            {
                return new OperationResult()
                {
                    Result = false,
                    ResultMsg = "需要删除的数据不存在！"
                };
            }
        }

        /// <summary>
        /// 判断该机型是否是第一次登记
        /// </summary>
        /// <returns></returns>
        public bool CheckCESSNA172RDailyRecordHaveRecord()
        {
            var query = _dbContext.Set<CESSNA172RDailyRecord>();
            if (query.Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 获取CESSNA172R型号初始设置的最后一次记录
        /// </summary>
        /// <returns></returns>
        public CESSNA172RDailyRecord GetLatestCESSNA172RDailyRecord()
        {
            var query = _dbContext.Set<CESSNA172RDailyRecord>().Where(x => x.Type == 1).OrderByDescending(o => o.InputDate).FirstOrDefault();
            return query;
        }

        public HttpResponseMessage GetDownloadFileStream(string type, string planeID, string startDate, string endDate)
        {
            #region 获取数据

            var result = from a in _dbContext.Set<CESSNA172RDailyRecord>()
                         join b in _dbContext.Set<Planes>() on a.PlanID equals b.ID
                         where a.IsActive && b.IsActive
                         orderby a.InputDate
                         select new CESSNA172RDailyRecordDto()
                         {
                             ID = a.ID,
                             Type = a.Type,
                             PlanID = a.PlanID,
                             PlanTypeID = b.PlaneTypeID,
                             PlaneNo = b.PlaneNo,
                             InputDate = a.InputDate,
                             DayClearingTime = a.DayClearingTime,
                             DayRiseAndFallNum = a.DayRiseAndFallNum,
                             DayAirTime = a.DayAirTime,
                             DayMaintenaceTime = a.DayMaintenaceTime,
                             CorrectClearingTime = a.CorrectClearingTime,
                             CorrectAirTime = a.CorrectAirTime,
                             PlanNewClearingTime = a.PlanNewClearingTime,
                             PlanNewAirTime = a.PlanNewAirTime,
                             PlanNewRiseAndFallNum = a.PlanNewRiseAndFallNum,
                             PlanDayClearingTime = a.PlanDayClearingTime,
                             PlanDayAirTime = a.PlanDayAirTime,
                             PlanDayGroundTime = a.PlanDayGroundTime,
                             EngineType = a.EngineType,
                             EngineNo = a.EngineNo,
                             EngineCorrectTSO = a.EngineCorrectTSO,
                             EngineNewTSN = a.EngineNewTSN,
                             ExecUnit = a.ExecUnit,
                             Memo = a.Memo,
                             IsActive = a.IsActive,
                             CreateTime = a.CreateTime,
                             Creator = a.Creator,
                             UpdateTime = a.UpdateTime,
                             Updator = a.Updator
                         };

            //如果没选择数据性质，则返回所有的数据
            if (!string.IsNullOrEmpty(type) && type != "-1")
            {
                result = result.Where(x => x.Type.ToString() == type);
            }
            if (!string.IsNullOrEmpty(planeID) && planeID != "-1")
            {
                result = result.Where(x => x.PlanID == planeID);
            }
            if (!string.IsNullOrEmpty(startDate))
            {
                result = result.Where(x => string.Compare(x.InputDate, startDate, StringComparison.Ordinal) >= 0);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                result = result.Where(x => string.Compare(x.InputDate, endDate, StringComparison.Ordinal) <= 0);
            }

            var xlsDataSource = result.ToList();

            #endregion

            //建立空白工作簿
            IWorkbook workbook = new HSSFWorkbook();
            //在工作簿中：建立空白工作表
            ISheet sheet = workbook.CreateSheet();

            //第一行
            #region 第一行
            //在工作表中：建立行，参数为行号，从0计
            IRow row1 = sheet.CreateRow(0);
            //在行中：建立单元格，参数为列号，从0计
            var cellMain1 = row1.CreateCell(0);
            //设置单元格内容
            cellMain1.SetCellValue("逐日输入数据");

            var cellMain2 = row1.CreateCell(6);
            cellMain2.SetCellValue("表修正");

            var cellMain3 = row1.CreateCell(8);
            cellMain3.SetCellValue("飞行数据");

            var cellMain4 = row1.CreateCell(14);
            cellMain4.SetCellValue("发动机数据");

            var cellMain5 = row1.CreateCell(17);
            cellMain5.SetCellValue("执行单位");

            var cellMain6 = row1.CreateCell(18);
            cellMain6.SetCellValue("数据性质");

            var cellMain7 = row1.CreateCell(19);
            cellMain7.SetCellValue("备注");

            #endregion

            //第二行
            #region 第二行
            //在工作表中：建立行，参数为行号，从0计
            IRow row2 = sheet.CreateRow(1);
            //在行中：建立单元格，参数为列号，从0计
            var row2Cell0 = row2.CreateCell(0);
            //设置单元格内容
            row2Cell0.SetCellValue("日期");

            var row2Cell1 = row2.CreateCell(1);
            row2Cell1.SetCellValue("飞机机号");

            var row2Cell2 = row2.CreateCell(2);
            row2Cell2.SetCellValue("空地时间（表）");

            var row2Cell3 = row2.CreateCell(3);
            row2Cell3.SetCellValue("当日总起落数");

            var row2Cell4 = row2.CreateCell(4);
            row2Cell4.SetCellValue("空中时间（表）");

            var row2Cell5 = row2.CreateCell(5);
            row2Cell5.SetCellValue("维护开车空地时间");

            var row2Cell6 = row2.CreateCell(6);
            row2Cell6.SetCellValue("空地时间（表）修正");

            var row2Cell7 = row2.CreateCell(7);
            row2Cell7.SetCellValue("空中时间（表）修正");

            var row2Cell8 = row2.CreateCell(8);
            row2Cell8.SetCellValue("自新空中时间");

            var row2Cell9 = row2.CreateCell(9);
            row2Cell9.SetCellValue("自新空地时间");

            var row2Cell10 = row2.CreateCell(10);
            row2Cell10.SetCellValue("自新起落数");

            var row2Cell11 = row2.CreateCell(11);
            row2Cell11.SetCellValue("当日空地时间");

            var row2Cell12 = row2.CreateCell(12);
            row2Cell12.SetCellValue("当日空中时间");

            var row2Cell13 = row2.CreateCell(13);
            row2Cell13.SetCellValue("当日地面时间");

            var row2Cell14 = row2.CreateCell(14);
            row2Cell14.SetCellValue("型号：10-360-L2A");

            #endregion

            #region 第三行
            //在工作表中：建立行，参数为行号，从0计
            IRow row3 = sheet.CreateRow(2);
            //在行中：建立单元格，参数为列号，从0计
            var row3Cell14 = row3.CreateCell(14);
            //设置单元格内容
            row3Cell14.SetCellValue("发动机序号");

            var row3Cell15 = row3.CreateCell(15);
            //设置单元格内容
            row3Cell15.SetCellValue("修后时间");

            var row3Cell16 = row3.CreateCell(16);
            //设置单元格内容
            row3Cell16.SetCellValue("自新时间");

            #endregion

            ICellStyle style = workbook.CreateCellStyle();
            //设置单元格的样式：水平对齐居中
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            //新建一个字体样式对象
            IFont font = workbook.CreateFont();
            //设置字体加粗样式
            font.Boldweight = short.MaxValue;
            //使用SetFont方法将字体样式添加到单元格样式中 
            style.SetFont(font);
            //将新的样式赋给单元格
            cellMain1.CellStyle = style;
            cellMain2.CellStyle = style;
            cellMain3.CellStyle = style;
            cellMain4.CellStyle = style;
            cellMain5.CellStyle = style;
            cellMain6.CellStyle = style;
            cellMain7.CellStyle = style;
            row2Cell0.CellStyle = style;
            row2Cell1.CellStyle = style;
            row2Cell2.CellStyle = style;
            row2Cell3.CellStyle = style;
            row2Cell4.CellStyle = style;
            row2Cell5.CellStyle = style;
            row2Cell6.CellStyle = style;
            row2Cell7.CellStyle = style;
            row2Cell8.CellStyle = style;
            row2Cell9.CellStyle = style;
            row2Cell10.CellStyle = style;
            row2Cell11.CellStyle = style;
            row2Cell12.CellStyle = style;
            row2Cell13.CellStyle = style;
            row2Cell14.CellStyle = style;
            row3Cell14.CellStyle = style;
            row3Cell15.CellStyle = style;
            row3Cell16.CellStyle = style;


            //设置单元格的高度
            row1.Height = 30 * 20;

            //设置单元格的宽度
            for (var i = 0; i < 20; i++)
            {
                sheet.AutoSizeColumn(i);
            }
            //sheet.AutoSizeColumn(0);

            //设置一个合并单元格区域，使用上下左右定义CellRangeAddress区域
            //CellRangeAddress四个参数为：起始行，结束行，起始列，结束列

            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 5));
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 6, 7));
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 8, 13));
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 14, 16));

            sheet.AddMergedRegion(new CellRangeAddress(0, 2, 17, 17));
            sheet.AddMergedRegion(new CellRangeAddress(0, 2, 18, 18));
            sheet.AddMergedRegion(new CellRangeAddress(0, 2, 19, 19));

            sheet.AddMergedRegion(new CellRangeAddress(1, 2, 0, 0));
            sheet.AddMergedRegion(new CellRangeAddress(1, 2, 1, 1));
            sheet.AddMergedRegion(new CellRangeAddress(1, 2, 2, 2));
            sheet.AddMergedRegion(new CellRangeAddress(1, 2, 3, 3));
            sheet.AddMergedRegion(new CellRangeAddress(1, 2, 4, 4));
            sheet.AddMergedRegion(new CellRangeAddress(1, 2, 5, 5));
            sheet.AddMergedRegion(new CellRangeAddress(1, 2, 6, 6));
            sheet.AddMergedRegion(new CellRangeAddress(1, 2, 7, 7));
            sheet.AddMergedRegion(new CellRangeAddress(1, 2, 8, 8));
            sheet.AddMergedRegion(new CellRangeAddress(1, 2, 9, 9));
            sheet.AddMergedRegion(new CellRangeAddress(1, 2, 10, 10));
            sheet.AddMergedRegion(new CellRangeAddress(1, 2, 11, 11));
            sheet.AddMergedRegion(new CellRangeAddress(1, 2, 12, 12));
            sheet.AddMergedRegion(new CellRangeAddress(1, 2, 13, 13));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 14, 16));


            #region 填充数据到xls

            int startRowNumber = 3;

            ICellStyle contentStyle = workbook.CreateCellStyle();
            //设置单元格的样式：水平对齐居中
            contentStyle.Alignment = HorizontalAlignment.Center;
            contentStyle.VerticalAlignment = VerticalAlignment.Center;

            for (var m = 0; m < xlsDataSource.Count; m++)
            {
                var row = sheet.CreateRow(startRowNumber + m);

                //设置数据行高度
                row.Height = 30 * 20;

                //在行中：建立单元格，参数为列号，从0计
                var column0 = row.CreateCell(0);
                //设置单元格内容
                column0.SetCellValue(xlsDataSource[m].InputDate);
                column0.CellStyle = contentStyle;

                //飞机机号
                var column1 = row.CreateCell(1);
                column1.SetCellValue(xlsDataSource[m].PlaneNo);
                column1.CellStyle = contentStyle;
                //空地时间（表）
                var column2 = row.CreateCell(2);
                column2.SetCellValue(xlsDataSource[m].DayClearingTime.ToString());
                column2.CellStyle = contentStyle;
                //当日总起落数
                var column3 = row.CreateCell(3);
                column3.SetCellValue(xlsDataSource[m].DayRiseAndFallNum);
                column3.CellStyle = contentStyle;
                //空中时间（表）
                var column4 = row.CreateCell(4);
                column4.SetCellValue(xlsDataSource[m].DayAirTime.ToString());
                column4.CellStyle = contentStyle;
                //维护开车空地时间
                var column5 = row.CreateCell(5);
                column5.SetCellValue(xlsDataSource[m].DayMaintenaceTime.ToString());
                column5.CellStyle = contentStyle;
                //空地时间（表）修正
                var column6 = row.CreateCell(6);
                column6.SetCellValue(xlsDataSource[m].CorrectClearingTime.ToString());
                column6.CellStyle = contentStyle;
                //空中时间（表）修正
                var column7 = row.CreateCell(7);
                column7.SetCellValue(xlsDataSource[m].CorrectAirTime.ToString());
                column7.CellStyle = contentStyle;
                //自新空地时间
                var column8 = row.CreateCell(8);
                column8.SetCellValue(xlsDataSource[m].PlanNewClearingTime.ToString());
                column8.CellStyle = contentStyle;
                //自新空中时间
                var column9 = row.CreateCell(9);
                column9.SetCellValue(xlsDataSource[m].PlanNewAirTime.ToString());
                column9.CellStyle = contentStyle;
                //自新起落数
                var column10 = row.CreateCell(10);
                column10.SetCellValue(xlsDataSource[m].PlanNewRiseAndFallNum.ToString());
                column10.CellStyle = contentStyle;
                //当日空地时间
                var column11 = row.CreateCell(11);
                column11.SetCellValue(xlsDataSource[m].PlanDayClearingTime.ToString());
                column11.CellStyle = contentStyle;
                //当日空中时间
                var column12 = row.CreateCell(12);
                column12.SetCellValue(xlsDataSource[m].PlanDayAirTime.ToString());
                column12.CellStyle = contentStyle;
                //当日地面时间
                var column13 = row.CreateCell(13);
                column13.SetCellValue(xlsDataSource[m].PlanDayGroundTime.ToString());
                column13.CellStyle = contentStyle;
                //发动机序号
                var column14 = row.CreateCell(14);
                column14.SetCellValue(xlsDataSource[m].EngineNo);
                column14.CellStyle = contentStyle;
                //修后时间
                var column15 = row.CreateCell(15);
                column15.SetCellValue(xlsDataSource[m].EngineCorrectTSO.ToString());
                column15.CellStyle = contentStyle;
                //自新时间
                var column16 = row.CreateCell(16);
                column16.SetCellValue(xlsDataSource[m].EngineNewTSN.ToString());
                column16.CellStyle = contentStyle;
                //执机单位
                var column17 = row.CreateCell(17);
                column17.SetCellValue(xlsDataSource[m].ExecUnit);
                column17.CellStyle = contentStyle;
                //数据性质
                var column18 = row.CreateCell(18);
                column18.SetCellValue(xlsDataSource[m].Type == 1 ? "初值" : "普通");
                column18.CellStyle = contentStyle;
                //备注
                var column19 = row.CreateCell(19);
                column19.SetCellValue(xlsDataSource[m].Memo);
                column19.CellStyle = contentStyle;

            }

            #endregion


            var fileName = "CESSNA172R逐日登记" + DateTime.Now.ToString("yyyyMMddhhmmssfff");

            string tempFolder = System.Web.HttpContext.Current.Server.MapPath("~/Temp/");

            if (!System.IO.Directory.Exists(tempFolder))
            {
                System.IO.Directory.CreateDirectory(tempFolder);
            }

            string destDocumentPath = tempFolder + fileName + ".xlsx";

            var file = new FileStream(destDocumentPath, FileMode.OpenOrCreate);
            workbook.Write(file);
            file.Close();
            workbook.Close();
            workbook = null;

            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new StreamContent(File.OpenRead(destDocumentPath));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Headers.Add("Access-Control-Expose-Headers", "x-file-name");
            response.Content.Headers.Add("x-file-name", HttpUtility.UrlEncode(fileName));
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName + ".xlsx"
            };

            response.StatusCode = System.Net.HttpStatusCode.OK;
            return response;
        }
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
        public PageResult<PA44_180DailyRecordDto> GetPA44_180DailyRecordList(int pageSize, int pageNo, string type, string planID, string startDate, string endDate)
        {
            PageResult<PA44_180DailyRecordDto> list = new PageResult<PA44_180DailyRecordDto>();

            if (_dbContext == null)
            {
                _dbContext = base.CreateDbContext();
            }
            var result = from a in _dbContext.Set<PA44_180DailyRecord>()//.Where(a => a.IsActive);
                         join b in _dbContext.Set<Planes>() on a.PlanID equals b.ID
                         where a.IsActive && b.IsActive
                         select new PA44_180DailyRecordDto()
                         {
                             ID = a.ID,
                             Type = a.Type,
                             PlanID = a.PlanID,
                             PlaneNo = b.PlaneNo,
                             InputDate = a.InputDate,
                             DayClearingTime = a.DayClearingTime,
                             DayRiseAndFallNum = a.DayRiseAndFallNum,
                             DayAirTime = a.DayAirTime,
                             DayHeatingMachineTime = a.DayHeatingMachineTime,
                             DayMaintenaceTime = a.DayMaintenaceTime,
                             CorrectClearingTime = a.CorrectClearingTime,
                             CorrectAirTime = a.CorrectAirTime,
                             CorrectHeatingMachineTime = a.CorrectHeatingMachineTime,
                             PlanNewClearingTime = a.PlanNewClearingTime,
                             PlanNewAirTime = a.PlanNewAirTime,
                             PlanNewHeatingMachineTime = a.PlanNewHeatingMachineTime,
                             PlanNewRiseAndFallNum = a.PlanNewRiseAndFallNum,
                             PlanDayClearingTime = a.PlanDayClearingTime,
                             PlanDayAirTime = a.PlanDayAirTime,
                             PlanDayGroundTime = a.PlanDayGroundTime,
                             LeftEngineType = a.LeftEngineType,
                             LeftEngineNo = a.LeftEngineNo,
                             LeftEngineCorrectTSO = a.LeftEngineCorrectTSO,
                             LeftEngineNewTSN = a.LeftEngineNewTSN,
                             RightEngineType = a.RightEngineType,
                             RightEngineNo = a.RightEngineNo,
                             RightEngineCorrectTSO = a.RightEngineCorrectTSO,
                             RightEngineNewTSN = a.RightEngineNewTSN,
                             HeatingMachineDayTime = a.HeatingMachineDayTime,
                             HeatingMachineCorrectTSO = a.HeatingMachineCorrectTSO,
                             HeatingMachineNewTSN = a.HeatingMachineNewTSN,
                             ExecUnit = a.ExecUnit,
                             Memo = a.Memo,
                             IsActive = a.IsActive,
                             CreateTime = a.CreateTime,
                             Creator = a.Creator,
                             UpdateTime = a.UpdateTime,
                             Updator = a.Updator
                         };
            //如果没选择数据性质，则返回所有的数据
            if (!string.IsNullOrEmpty(type) && type != "-1")
            {
                result = result.Where(x => x.Type.ToString() == type);
            }
            if (!string.IsNullOrEmpty(planID) && planID != "-1")
            {
                result = result.Where(x => x.PlanID == planID);
            }
            if (!string.IsNullOrEmpty(startDate))
            {
                result = result.Where(x => string.Compare(x.InputDate, startDate, StringComparison.Ordinal) >= 0);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                result = result.Where(x => string.Compare(x.InputDate, endDate, StringComparison.Ordinal) <= 0);
            }

            list.Total = result.Count();
            result = result.OrderByDescending(a => a.CreateTime).Skip((pageNo - 1) * pageSize).Take(pageSize);
            list.ResultData = result.ToList();
            return list;
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <param name="ID">主键</param>
        /// <returns>数据实体</returns>
        public PA44_180DailyRecord GetPA44_180DailyRecord(string ID)
        {
            var query = _dbContext.Set<PA44_180DailyRecord>().Where(x => x.ID == ID).FirstOrDefault();
            return query;
        }

        /// <summary>
        /// 获取上一条记录（不分数据类型）
        /// </summary>
        /// <returns></returns>
        private PA44_180DailyRecord GetLastPA44_180DailyRecord()
        {
            var query = _dbContext.Set<PA44_180DailyRecord>().OrderByDescending(o => o.CreateTime).FirstOrDefault();
            return query;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item">新增实体</param>
        /// <param name="userID">操作人ID</param>
        /// <returns>操作结果</returns>
        public OperationResult Add(PA44_180DailyRecord item, string userID)
        {

            //获取上一条记录
            var lastRecord = GetLastPA44_180DailyRecord();

            try
            {
                item.ID = Guid.NewGuid().ToString();
                item.CreateTime = item.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                item.Creator = item.Updator = userID;
                item.IsActive = true;

                //如果数据是普通逐日，则需要将部分字段根据页面上录入的值自动计算并做保存；如果数据是初值，则不需要自动计算步骤，因为所有字段均为手工录入
                if (item.Type == 2)
                {
                    //自新空中时间=空中时间（表）+空中时间（表）修正；
                    item.PlanNewAirTime = item.DayAirTime + item.CorrectAirTime;
                    //自新空地时间=空地时间（表）+空地时间（表）修正；
                    item.PlanNewClearingTime = item.DayClearingTime + item.CorrectClearingTime;
                    //自新起落次数=本日起落+自新起落（上一条的记录）
                    item.PlanNewRiseAndFallNum = item.DayRiseAndFallNum + (lastRecord == null ? 0 : lastRecord.PlanNewRiseAndFallNum);
                    //自新加温机时间==加温机时间（表）+加温机时间（表）修正
                    item.PlanNewHeatingMachineTime = item.DayHeatingMachineTime + item.CorrectHeatingMachineTime;
                    //当日空地时间=自新空地时间-自新空地时间（上一条的记录）；
                    item.PlanDayClearingTime = item.PlanNewClearingTime - (lastRecord == null ? 0 : lastRecord.PlanNewClearingTime);
                    //当日空中时间=自新空中时间-自新空中时间（上一条的记录）；
                    item.PlanDayAirTime = item.PlanNewAirTime - (lastRecord == null ? 0 : lastRecord.PlanNewAirTime);
                    //当日地面时间=自新空中时间-自新空地时间；
                    item.PlanDayGroundTime = item.PlanNewAirTime - item.PlanNewClearingTime;


                    //修后时间TSO=当日空中时间+上一条修后时间
                    item.HeatingMachineCorrectTSO = item.PlanDayAirTime + (lastRecord == null ? 0 : lastRecord.HeatingMachineCorrectTSO);
                    //自新时间TSN=当日空中时间+上一条自新时间
                    item.HeatingMachineNewTSN = item.PlanDayAirTime + (lastRecord == null ? 0 : lastRecord.HeatingMachineNewTSN);


                    //当日时间=加温机时间（表）+加温机时间（表）修正-上一条当日时间
                    item.HeatingMachineDayTime = item.DayHeatingMachineTime + item.CorrectHeatingMachineTime - (lastRecord == null ? 0 : lastRecord.HeatingMachineDayTime);
                    //修后时间TSO=上一条修后时间TSO+（加温机数据）当日时间
                    item.HeatingMachineCorrectTSO = (lastRecord == null ? 0 : lastRecord.HeatingMachineCorrectTSO) + item.HeatingMachineDayTime;
                    //自新时间TSN=上一条自新时间TSN+（加温机数据）当日时间
                    item.HeatingMachineNewTSN = (lastRecord == null ? 0 : lastRecord.HeatingMachineNewTSN) + item.HeatingMachineDayTime;
                }

                _dbContext.Set<PA44_180DailyRecord>().Add(item);
                _dbContext.SaveChanges();

                return new OperationResult()
                {
                    Result = true
                };
            }
            catch (Exception ex)
            {
                return new OperationResult()
                {
                    Result = false,
                    ResultMsg = ex.Message
                };
            }

        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="item">修改数据实体</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        public OperationResult Update(PA44_180DailyRecord item, string userID)
        {
            var editModel = _dbContext.Set<PA44_180DailyRecord>().Where(x => x.ID == item.ID).FirstOrDefault();
            if (editModel != null)
            {
                try
                {
                    //修改信息
                    editModel.Type = item.Type;
                    editModel.PlanID = item.PlanID;
                    editModel.InputDate = item.InputDate;
                    editModel.DayClearingTime = item.DayClearingTime;
                    editModel.DayRiseAndFallNum = item.DayRiseAndFallNum;
                    editModel.DayAirTime = item.DayAirTime;
                    editModel.DayHeatingMachineTime = item.DayHeatingMachineTime;
                    editModel.DayMaintenaceTime = item.DayMaintenaceTime;

                    editModel.CorrectClearingTime = item.CorrectClearingTime;
                    editModel.CorrectAirTime = item.CorrectAirTime;
                    editModel.CorrectHeatingMachineTime = item.CorrectHeatingMachineTime;

                    editModel.PlanNewClearingTime = item.PlanNewClearingTime;
                    editModel.PlanNewAirTime = item.PlanNewAirTime;
                    editModel.PlanNewHeatingMachineTime = item.PlanNewHeatingMachineTime;
                    editModel.PlanNewRiseAndFallNum = item.PlanNewRiseAndFallNum;
                    editModel.PlanDayClearingTime = item.PlanDayClearingTime;
                    editModel.PlanDayAirTime = item.PlanDayAirTime;
                    editModel.PlanDayGroundTime = item.PlanDayGroundTime;

                    editModel.LeftEngineType = item.LeftEngineType;
                    editModel.LeftEngineNo = item.LeftEngineNo;
                    editModel.LeftEngineCorrectTSO = item.LeftEngineCorrectTSO;
                    editModel.LeftEngineNewTSN = item.LeftEngineNewTSN;

                    editModel.RightEngineType = item.RightEngineType;
                    editModel.RightEngineNo = item.RightEngineNo;
                    editModel.RightEngineCorrectTSO = item.RightEngineCorrectTSO;
                    editModel.RightEngineNewTSN = item.RightEngineNewTSN;

                    editModel.HeatingMachineDayTime = item.HeatingMachineDayTime;
                    editModel.HeatingMachineCorrectTSO = item.HeatingMachineCorrectTSO;
                    editModel.HeatingMachineNewTSN = item.HeatingMachineNewTSN;

                    editModel.ExecUnit = item.ExecUnit;
                    editModel.Memo = item.Memo;
                    editModel.Updator = userID;
                    editModel.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    _dbContext.SaveChanges();

                    return new OperationResult()
                    {
                        Result = true
                    };
                }
                catch (Exception ex)
                {
                    return new OperationResult()
                    {
                        Result = false,
                        ResultMsg = ex.Message
                    };
                }
            }
            else
            {
                return new OperationResult()
                {
                    Result = false,
                    ResultMsg = "正在修改的数据不存在！"
                };
            }
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">删除数据主键</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        public OperationResult DeletePA44_180DailyRecord(string ID, string userID)
        {
            var editModel = _dbContext.Set<PA44_180DailyRecord>().Where(x => x.ID == ID).FirstOrDefault();
            if (editModel != null)
            {
                try
                {
                    //修改信息
                    editModel.IsActive = false;
                    editModel.Updator = userID;
                    editModel.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    _dbContext.SaveChanges();

                    return new OperationResult()
                    {
                        Result = true
                    };
                }
                catch (Exception ex)
                {
                    return new OperationResult()
                    {
                        Result = false,
                        ResultMsg = ex.Message
                    };
                }
            }
            else
            {
                return new OperationResult()
                {
                    Result = false,
                    ResultMsg = "需要删除的数据不存在！"
                };
            }
        }

        /// <summary>
        /// 判断该机型是否是第一次登记
        /// </summary>
        /// <returns></returns>
        public bool CheckPA44_180DailyRecordHaveRecord()
        {
            var query = _dbContext.Set<PA44_180DailyRecord>();
            if (query.Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 获取PA44_180型号初始设置的最后一次记录
        /// </summary>
        /// <returns></returns>
        public PA44_180DailyRecord GetLatestPA44_180DailyRecord()
        {
            var query = _dbContext.Set<PA44_180DailyRecord>().Where(x => x.Type == 1 && x.IsActive).OrderByDescending(o => o.CreateTime).FirstOrDefault();
            return query;
        }

        #endregion
    }
}