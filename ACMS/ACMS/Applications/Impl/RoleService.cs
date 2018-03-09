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
    public class RoleService : ServiceBase, IRoleService
    {
        private DbContext _dbContext = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RoleService()
        {
            _dbContext = base.CreateDbContext();
            base.AddDisposableObject(_dbContext);

        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns>数据列表</returns>
        public PageResult<RoleDto> GetList(int pageSize, int pageNo, string keyWord)
        {
            PageResult<RoleDto> list = new PageResult<RoleDto>();

            if (_dbContext == null)
            {
                _dbContext = base.CreateDbContext();
            }
            var result = from a in _dbContext.Set<Role>()
                         where a.IsActive
                         select new RoleDto()
                         {
                             ID = a.ID,
                             RoleName = a.RoleName,
                             RoleDesc = a.RoleDesc,
                             CreateTime = a.CreateTime
                         };

            var roleMenuList = (from a in _dbContext.Set<RoleMenu>()
                                join b in _dbContext.Set<Menu>() on a.MenuID equals b.ID
                                where a.IsActive && b.IsActive && b.MenuLevel == 1
                                select new
                                {
                                    RoleID = a.RoleID,
                                    MenuID = a.MenuID,
                                    MenuName = b.MenuName
                                }).ToList();

            if (!string.IsNullOrEmpty(keyWord))//关键词查询    
                result = result.Where(a => a.RoleName.Contains(keyWord));
            list.Total = result.Count();
            result = result.OrderByDescending(a => a.CreateTime).Skip((pageNo - 1) * pageSize).Take(pageSize);
            var roleListResult = result.ToList();
            roleListResult.ForEach(p =>
            {
                p.MenuIDs = roleMenuList.Where(x => x.RoleID == p.ID).Select(s => s.MenuID).ToList();
                p.MenuNames = roleMenuList.Where(x => x.RoleID == p.ID).Select(s => s.MenuName).ToList();
            });

            list.ResultData = roleListResult;
            return list;
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <param name="ID">主键</param>
        /// <returns>数据实体</returns>
        public Role Get(string ID)
        {
            var query = _dbContext.Set<Role>().Where(x => x.ID == ID).FirstOrDefault();
            return query;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item">新增实体</param>
        /// <param name="userID">操作人ID</param>
        /// <returns>操作结果</returns>
        public OperationResult Add(Role item, string userID)
        {
            var query = _dbContext.Set<Role>().Where(x => x.RoleName == item.RoleName && x.IsActive).FirstOrDefault();
            if (query != null)
            {
                return new OperationResult()
                {
                    Result = false,
                    ResultMsg = "该角色名已存在"
                };
            }
            try
            {
                item.ID = Guid.NewGuid().ToString();
                item.CreateTime = item.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                item.Creator = item.Updator = userID;
                item.IsActive = true;
                _dbContext.Set<Role>().Add(item);
                _dbContext.SaveChanges();
                return new OperationResult()
                {
                    Result = true
                };
            }
            catch (Exception ex)
            {
                return new OperationResult()
                {
                    Result = false,
                    ResultMsg = ex.Message
                };
            }

        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="item">修改数据实体</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        public OperationResult Update(Role item, string userID)
        {
            var editModel = _dbContext.Set<Role>().Where(x => x.ID == item.ID).FirstOrDefault();
            if (editModel != null)
            {
                try
                {
                    //修改信息
                    editModel.RoleDesc = item.RoleDesc;
                    editModel.RoleName = item.RoleName;
                    editModel.Updator = userID;
                    editModel.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    _dbContext.SaveChanges();

                    return new OperationResult()
                    {
                        Result = true
                    };
                }
                catch (Exception ex)
                {
                    return new OperationResult()
                    {
                        Result = false,
                        ResultMsg = ex.Message
                    };
                }
            }
            else
            {
                return new OperationResult()
                {
                    Result = false,
                    ResultMsg = "正在修改的数据不存在！"
                };
            }
        }



        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">删除数据主键</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        public OperationResult Delete(string ID, string userID)
        {
            var editModel = _dbContext.Set<Role>().Where(x => x.ID == ID).FirstOrDefault();
            if (editModel != null)
            {
                try
                {
                    //修改信息
                    editModel.IsActive = false;
                    editModel.Updator = userID;
                    editModel.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    _dbContext.SaveChanges();

                    return new OperationResult()
                    {
                        Result = true
                    };
                }
                catch (Exception ex)
                {
                    return new OperationResult()
                    {
                        Result = false,
                        ResultMsg = ex.Message
                    };
                }
            }
            else
            {
                return new OperationResult()
                {
                    Result = false,
                    ResultMsg = "需要删除的数据不存在！"
                };
            }
        }


        /// <summary>
        /// 添加角色菜单
        /// </summary>
        /// <param name="item">角色-菜单（1对N）</param>
        /// <param name="operationUserID">操作人</param>
        /// <returns>操作结果</returns>
        public OperationResult AddRoleMenu(RoleMenuDto item, string userID)
        {
            if (item != null)
            {
                try
                {
                    //先将原来的角色菜单都删除掉
                    var deleteRoleMenuList = _dbContext.Set<RoleMenu>().Where(x => x.RoleID == item.RoleID).ToList();

                    for (var m = 0; m < deleteRoleMenuList.Count; m++)
                    {
                        deleteRoleMenuList[m].IsActive = false;
                    }

                    _dbContext.SaveChanges();

                    var menuList = _dbContext.Set<Menu>().ToList();

                    var allAddMenuIDs = item.MenuIDs.Distinct().ToList();

                    //增加新的角色菜单数据
                    if (allAddMenuIDs.Count > 0)
                    {
                        var addList = new List<RoleMenu>();

                        foreach (var menuID in allAddMenuIDs)
                        {
                            addList.Add(new RoleMenu()
                            {
                                ID = Guid.NewGuid().ToString(),
                                MenuID = menuID,
                                RoleID = item.RoleID,
                                IsActive = true,
                                Creator = userID,
                                CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                            });

                            var childMenu = menuList.Where(x => x.ParentMenuID == menuID).Distinct().ToList();

                            foreach (var childMenuItem in childMenu)
                            {
                                addList.Add(new RoleMenu()
                                {
                                    ID = Guid.NewGuid().ToString(),
                                    MenuID = childMenuItem.ID,
                                    RoleID = item.RoleID,
                                    IsActive = true,
                                    Creator = userID,
                                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                });
                            }
                            
                        }

                        _dbContext.Set<RoleMenu>().AddRange(addList);
                    }

                    _dbContext.SaveChanges();

                    return new OperationResult() { Result = true };
                }
                catch (Exception ex)
                {
                    return new OperationResult()
                    {
                        Result = false,
                        ResultMsg = ex.Message
                    };
                }
            }
            else
            {
                return new OperationResult()
                {
                    Result = false,
                    ResultMsg = "需要新增的角色菜单数据不存在！"
                };
            }
        }


        /// <summary>
        /// 根据角色获取该角色的权限菜单
        /// </summary>
        /// <param name="RoleID">角色ID</param>
        /// <returns>权限菜单列表</returns>
        public List<Menu> GetMenusByRole(string RoleID)
        {
            var menuList = (from a in _dbContext.Set<RoleMenu>()
                            join b in _dbContext.Set<Menu>() on a.MenuID equals b.ID
                            where a.RoleID == RoleID && a.IsActive && b.IsActive
                            select b).ToList();

            return menuList;
        }
    }
}