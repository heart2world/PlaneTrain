using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACMS.Models
{
    public class UserDto
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public string LoginAccount { get; set; }
        public string DeptName { get; set; }
        public List<string> RoleIDs { get; set; }
        public List<string> RoleNames { get; set; }
        public bool IsActive { get; set; }
        public string CreateTime { get; set; }
        public string Creator { get; set; }
        public string UpdateTime { get; set; }
        public string Updator { get; set; }
    }

    public class UserLoginDto
    {
        public string LoginAccount { get; set; }
        public string Password { get; set; }
        public bool LoginResult { get; set; }
        public string Ticket { get; set; }
    }


}