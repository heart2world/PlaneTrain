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
    
    public partial class Menu
    {
        public string ID { get; set; }
        public string MenuName { get; set; }
        public string MenuURL { get; set; }
        public bool IsActive { get; set; }
        public string ParentMenuID { get; set; }
        public int MenuLevel { get; set; }
        public Nullable<int> OrderIndex { get; set; }
        public string MenuIcon { get; set; }
        public string CreateTime { get; set; }
        public string Creator { get; set; }
        public string UpdateTime { get; set; }
        public string Updator { get; set; }
    }
}
