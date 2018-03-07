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
        public string MenuIcon { get; set; }
        public List<MenuDto> ChildMenus { get; set; }
    }

    public class MenuForDisplayDto
    {
        public string menu_id { get; set; }
        public string menu_name { get; set; }
        public string menu_url { get; set; }
        public string icon { get; set; }
        public Nullable<int> order_index { get; set; }
        public string parent_menu_id { get; set; }
        public List<MenuForDisplayDto> child { get; set; }
    }

}