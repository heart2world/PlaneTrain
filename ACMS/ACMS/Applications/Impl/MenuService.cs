using ACMS.ApplicationBase;
using ACMS.EF;
using ACMS.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ACMS.Applications.Impl
{
    public class MenuService : ServiceBase, IMenuService
    {
        private DbContext _dbContext = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MenuService()
        {
            _dbContext = base.CreateDbContext();
            base.AddDisposableObject(_dbContext);
        }


        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns>菜单树</returns>
        public List<MenuDto> GetList()
        {
            var menus = _dbContext.Set<Menu>().Where(x => x.IsActive).ToList();
            //find rootMenu
            var rootMenus = menus.Where(x => x.ParentMenuID == null).OrderBy(o => o.OrderIndex).Select(s => Mapper.Map<MenuDto>(s)).ToList();

            if (rootMenus != null)
            {
                foreach (var rootMenu in rootMenus)
                {
                    GetMenuRecursion(rootMenu, menus);
                }
            }

            return rootMenus;
        }

        /// <summary>
        /// 递归查找Menu树
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="menus"></param>
        private void GetMenuRecursion(MenuDto menu, List<Menu> menus)
        {
            var subMenus = menus.Where(x => x.ParentMenuID == menu.ID).OrderBy(o => o.OrderIndex).Select(s => Mapper.Map<MenuDto>(s)).ToList();
            menu.ChildMenus = subMenus;
            foreach (var subMenu in subMenus)
            {
                GetMenuRecursion(subMenu, menus);
            }
        }
    }
}