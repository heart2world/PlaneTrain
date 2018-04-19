using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACMS.Models
{
    public class PlanesTCtrlItemDto
    {
        public string ID { get; set; }
        public string PlaneTypeID { get; set; }
        public string PlaneNo { get; set; }
        public bool IsCtrl { get; set; }
        public string JianNO { get; set; }
        public string SerialNO { get; set; }
        public Nullable<decimal> TSN { get; set; }
        public Nullable<decimal> TSO { get; set; }
        public Nullable<bool> IsWatDate { get; set; }
        public Nullable<bool> IsWatAirTime { get; set; }
        public Nullable<bool> IsWatOnOffTime { get; set; }
        public Nullable<bool> IsWatUpTemprTime { get; set; }
        public string FCheckDate { get; set; }
        public Nullable<decimal> FAirTime { get; set; }
        public Nullable<int> FOnOffTime { get; set; }
        public Nullable<decimal> FUpTemprTime { get; set; }
        public string RCheckDate { get; set; }
        public Nullable<decimal> RAirTime { get; set; }
        public Nullable<int> ROnOffTime { get; set; }
        public Nullable<decimal> RUpTemprTime { get; set; }
        public string Memo { get; set; }
        public bool IsActive { get; set; }
        public string CreateTime { get; set; }
        public string Creator { get; set; }
        public string UpdateTime { get; set; }
        public string Updator { get; set; }
        public Nullable<int> ExeCapacity { get; set; }
        public List<string> ReList { get; set; }
    }
}
