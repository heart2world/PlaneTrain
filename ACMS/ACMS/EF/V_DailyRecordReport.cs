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
    
    public partial class V_DailyRecordReport
    {
        public string ID { get; set; }
        public int Type { get; set; }
        public string PlanID { get; set; }
        public string InputDate { get; set; }
        public int DayRiseAndFallNum { get; set; }
        public decimal DayMaintenaceTime { get; set; }
        public Nullable<decimal> PlanDayClearingTime { get; set; }
        public Nullable<decimal> PlanDayAirTime { get; set; }
        public Nullable<decimal> PlanDayGroundTime { get; set; }
        public string ExecUnit { get; set; }
        public bool IsActive { get; set; }
        public string CreateTime { get; set; }
        public string Creator { get; set; }
        public string UpdateTime { get; set; }
        public string Updator { get; set; }
        public Nullable<int> HeatingMachineDayTime { get; set; }
        public string PlaneNo { get; set; }
        public string TypeName { get; set; }
        public string PlaneTypeID { get; set; }
    }
}
