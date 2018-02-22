using ACMS.ApplicationBase;
using ACMS.EF;
using ACMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

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
            if (!string.IsNullOrEmpty(startDate))
            {
                result = result.Where(a => string.Compare(a.InputDate, startDate) >= 0);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                result = result.Where(a => string.Compare(a.InputDate, endDate) <= 0);
            }

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
            var result = _dbContext.Set<V_DailyRecordReport>().Where(a => a.IsActive);
            if (!string.IsNullOrEmpty(startDate))
            {
                result = result.Where(x => string.Compare(x.CreateTime, startDate) > 0);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                result = result.Where(x => string.Compare(x.CreateTime, endDate) < 0);
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
            //if (list != null && list.ResultData.Count > 0)
            //{
            //    //飞行天数查询
            //    var result2 = _dbContext.Set<V_DailyRecordReport>().Where(a => a.IsActive);
            //    if (!string.IsNullOrEmpty(startDate))
            //    {
            //        result = result.Where(x => string.Compare(x.CreateTime, startDate) > 0);
            //    }
            //    if (!string.IsNullOrEmpty(endDate))
            //    {
            //        result = result.Where(x => string.Compare(x.CreateTime, endDate) < 0);
            //    }
            //    var result3 =from item in result2
            //    group item by { item.PlaneNo,} into stgrp
            //    select stgrp;
            //    result.GroupBy(a => new { a.PlaneNo, a.InputDate })
            //             .Select(g => new { PlaneNo } ()
            //             {

            //                  g.Key.PlaneNo,
            //                 PlanID = g.Key.InputDate
            //             })
            //}

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
        public PageResult<CESSNA172RDailyRecordDto> GetCESSNA172RDailyRecordList(int pageSize, int pageNo, int type, string planID, string startDate, string endDate)
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
            if (type != -1)
            {
                result = result.Where(x => x.Type == type);
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
            var query = _dbContext.Set<CESSNA172RDailyRecord>().Where(x => x.ID == ID).FirstOrDefault();
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

            try
            {
                item.ID = Guid.NewGuid().ToString();
                item.CreateTime = item.UpdateTime = DateTime.Now.ToString();
                item.Creator = item.Updator = userID;
                item.IsActive = true;
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
                    editModel.UpdateTime = DateTime.Now.ToString();

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
                    editModel.UpdateTime = DateTime.Now.ToString();

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
        public PageResult<PA44_180DailyRecordDto> GetPA44_180DailyRecordList(int pageSize, int pageNo, int type, string planID, string startDate, string endDate)
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
            if (type != -1)
            {
                result = result.Where(x => x.Type == type);
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
        /// 新增
        /// </summary>
        /// <param name="item">新增实体</param>
        /// <param name="userID">操作人ID</param>
        /// <returns>操作结果</returns>
        public OperationResult Add(PA44_180DailyRecord item, string userID)
        {

            try
            {
                item.ID = Guid.NewGuid().ToString();
                item.CreateTime = item.UpdateTime = DateTime.Now.ToString();
                item.Creator = item.Updator = userID;
                item.IsActive = true;
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
                    editModel.UpdateTime = DateTime.Now.ToString();

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
                    editModel.UpdateTime = DateTime.Now.ToString();

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

        #endregion
    }
}