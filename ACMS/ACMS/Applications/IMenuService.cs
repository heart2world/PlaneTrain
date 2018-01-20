using ACMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACMS.Applications
{
    public interface IMenuService
    {
        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns>菜单树</returns>
        List<MenuDto> GetList();
    }
}
