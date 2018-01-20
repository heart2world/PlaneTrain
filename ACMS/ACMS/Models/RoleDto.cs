using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACMS.Models
{
    public class RoleDto
    {
        public string ID { get; set; }
        public string RoleName { get; set; }
        public string RoleDesc { get; set; }
        public bool IsActive { get; set; }
        public List<string> MenuIDs { get; set; }
        public List<string> MenuNames { get; set; }
        public string CreateTime { get; set; }
        public string Creator { get; set; }
        public string UpdateTime { get; set; }
        public string Updator { get; set; }
    }

    public class RoleMenuDto
    {
        public string RoleID { get; set; }
        public List<string> MenuIDs { get; set; }
    }
}