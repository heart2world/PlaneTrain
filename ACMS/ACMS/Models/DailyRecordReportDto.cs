using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACMS.Models
{
    public class DailyRecordReportDto
    {
        public string PlanID { get; set; }
        public int DayRiseAndFallNum { get; set; }
        public decimal DayMaintenaceTime { get; set; }
        public Nullable<decimal> PlanDayClearingTime { get; set; }
        public Nullable<decimal> PlanDayAirTime { get; set; }
        public Nullable<decimal> PlanDayGroundTime { get; set; }
        public string ExecUnit { get; set; }
        public Nullable<decimal> HeatingMachineDayTime { get; set; }
        public string PlaneNo { get; set; }
        public string PlaneTypeID { get; set; }
        public string TypeName { get; set; }

        public int FlightDays { get; set; }
    }

    /// <summary>
    /// 飞机生产信息月报
    /// </summary>
    public class PlaneWorkReportDto
    {
        /// <summary>
        /// 当月有记录的机型的飞机数量
        /// </summary>
        public int PlanesCount { get; set; }
        /// <summary>
        /// 起落次数
        /// </summary>
        public int DayRiseAndFallNum { get; set; }
        /// <summary>
        /// 当日空地时间
        /// </summary>
        public Nullable<decimal> PlanDayClearingTime { get; set; }
        /// <summary>
        /// 当日空中时间
        /// </summary>
        public Nullable<decimal> PlanDayAirTime { get; set; }
        /// <summary>
        /// 飞机类型ID
        /// </summary>
        public string PlaneTypeID { get; set; }
        /// <summary>
        /// 飞机类型名称
        /// </summary>
        public string TypeName { get; set; }

    }

    /// <summary>
    /// 飞机报表
    /// </summary>
    public class PlaneReportDto
    {
        /// <summary>
        /// 起落次数
        /// </summary>
        public int DayRiseAndFallNum { get; set; }
        /// <summary>
        /// 自新起落次数
        /// </summary>
        public int? PlanNewRiseAndFallNum { get; set; }
        /// <summary>
        /// 当日空地时间
        /// </summary>
        public Nullable<decimal> PlanDayClearingTime { get; set; }
        /// <summary>
        /// 自新空地时间
        /// </summary>
        public Nullable<decimal> PlanNewClearingTime { get; set; }
        /// <summary>
        /// 当日空中时间
        /// </summary>
        public Nullable<decimal> PlanDayAirTime { get; set; }
        /// <summary>
        /// 自新空中时间
        /// </summary>
        public Nullable<decimal> PlanNewAirTime { get; set; }
        /// <summary>
        /// 飞机类型ID
        /// </summary>
        public string PlaneNo { get; set; }
        /// <summary>
        /// 飞机类型名称
        /// </summary>
        public string TypeName { get; set; }

    }

    public class EngineReportDto
    {
        public string ID { get; set; }
        /// <summary>
        /// 飞机ID
        /// </summary>
        public string PlanID { get; set; }
        /// <summary>
        /// 飞机机型名称
        /// </summary>
        public string PlaneTypeName { get; set; }
        /// <summary>
        /// 发动机状态
        /// </summary>
        public string EngineStatus { get; set; }
        /// <summary>
        /// 发动机型号
        /// </summary>
        public string EngineType { get; set; }
        /// <summary>
        /// 发动机序号
        /// </summary>
        public string EngineNo { get; set; }
        /// <summary>
        /// TSO
        /// </summary>
        public Nullable<decimal> EngineCorrectTSO { get; set; }
        public Nullable<decimal> EngineNewTSN { get; set; }
        /// <summary>
        /// 发动机位置
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 飞机机号
        /// </summary>
        public string PlaneNo { get; set; }
        /// <summary>
        /// 空中时间
        /// </summary>
        public Nullable<decimal> PlanDayAirTime { get; set; }
    }
}
