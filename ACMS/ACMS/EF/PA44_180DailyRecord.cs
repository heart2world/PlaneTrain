//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ACMS.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class PA44_180DailyRecord
    {
        public string ID { get; set; }
        public int Type { get; set; }
        public string PlanID { get; set; }
        public string InputDate { get; set; }
        public decimal DayClearingTime { get; set; }
        public int DayRiseAndFallNum { get; set; }
        public decimal DayAirTime { get; set; }
        public decimal DayHeatingMachineTime { get; set; }
        public decimal DayMaintenaceTime { get; set; }
        public decimal CorrectClearingTime { get; set; }
        public decimal CorrectAirTime { get; set; }
        public decimal CorrectHeatingMachineTime { get; set; }
        public Nullable<decimal> PlanNewClearingTime { get; set; }
        public Nullable<decimal> PlanNewAirTime { get; set; }
        public Nullable<decimal> PlanNewHeatingMachineTime { get; set; }
        public Nullable<int> PlanNewRiseAndFallNum { get; set; }
        public Nullable<decimal> PlanDayClearingTime { get; set; }
        public Nullable<decimal> PlanDayAirTime { get; set; }
        public Nullable<decimal> PlanDayGroundTime { get; set; }
        public string LeftEngineType { get; set; }
        public string LeftEngineNo { get; set; }
        public string LeftEngineCorrectTSO { get; set; }
        public Nullable<decimal> LeftEngineNewTSN { get; set; }
        public string RightEngineType { get; set; }
        public string RightEngineNo { get; set; }
        public string RightEngineCorrectTSO { get; set; }
        public Nullable<decimal> RightEngineNewTSN { get; set; }
        public Nullable<decimal> HeatingMachineDayTime { get; set; }
        public string HeatingMachineCorrectTSO { get; set; }
        public Nullable<decimal> HeatingMachineNewTSN { get; set; }
        public string ExecUnit { get; set; }
        public string Memo { get; set; }
        public bool IsActive { get; set; }
        public string CreateTime { get; set; }
        public string Creator { get; set; }
        public string UpdateTime { get; set; }
        public string Updator { get; set; }
    }
}
