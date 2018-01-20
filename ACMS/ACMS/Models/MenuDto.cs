using ACMS.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACMS.Models
{
    public class MenuDto
    {
        public string ID { get; set; }
        public string MenuName { get; set; }
        public string MenuURL { get; set; }
        public bool IsActive { get; set; }
        public string ParentMenuID { get; set; }
        public int MenuLevel { get; set; }
        public Nullable<int> OrderIndex { get; set; }
        public List<MenuDto> ChildMenus { get; set; }
    }

}