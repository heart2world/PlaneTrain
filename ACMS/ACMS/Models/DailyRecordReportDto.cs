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
        public int HeatingMachineDayTime { get; set; }
        public string PlaneNo { get; set; }
        public string PlaneTypeID { get; set; }
        public string TypeName { get; set; }

        //public int FlightDays { get; set; }
    }
}
