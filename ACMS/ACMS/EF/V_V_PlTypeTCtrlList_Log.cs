//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ACMS.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class V_V_PlTypeTCtrlList_Log
    {
        public int ID { get; set; }
        public string TCtrlID { get; set; }
        public string ProSource { get; set; }
        public string PlaneTypeID { get; set; }
        public string MainTainName { get; set; }
        public Nullable<int> Capacity { get; set; }
        public string FCheckDate { get; set; }
        public Nullable<decimal> FAirTime { get; set; }
        public Nullable<int> FOnOffTime { get; set; }
        public Nullable<decimal> FUpTemprTime { get; set; }
        public string RCheckDate { get; set; }
        public Nullable<decimal> RAirTime { get; set; }
        public Nullable<int> ROnOffTime { get; set; }
        public Nullable<decimal> RUpTemprTime { get; set; }
        public bool IsActive { get; set; }
        public string CreateTime { get; set; }
        public string Creator { get; set; }
        public string UpdateTime { get; set; }
        public string Updator { get; set; }
        public string TypeName { get; set; }
    }
}
