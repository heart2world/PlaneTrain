﻿using ACMS.ApplicationBase;
using ACMS.EF;
using ACMS.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ACMS.Services.Impl
{
    public class UserService : ServiceBase, IUserService
    {
        private DbContext _dbContext = null;

        public UserService()
        {
            _dbContext = base.CreateDbContext();
            base.AddDisposableObject(_dbContext);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns>用户列表信息</returns>
        public PageResult<UserDto> GetList(int pageSize, int pageNo, string keyWord)
        {
            PageResult<UserDto> list = new PageResult<UserDto>();

            //用户列表
            var query = from a in _dbContext.Set<User>()
                        where a.IsActive
                        select new UserDto()
                        {
                            ID = a.ID,
                            UserName = a.UserName,
                            LoginAccount = a.LoginAccount,
                            DeptName = a.DeptName,
                            CreateTime = a.CreateTime,
                            Creator = a.Creator,
                            IsActive = a.IsActive
                        };
            //用户角色信息
            var userRoleList = (from a in _dbContext.Set<UserRole>()
                                join b in _dbContext.Set<Role>() on a.RoleID equals b.ID
                                where a.IsActive && b.IsActive
                                select new
                                {
                                    UserID = a.UserID,
                                    RoleID = a.RoleID,
                                    RoleName = b.RoleName
                                }).ToList();

            if (!string.IsNullOrEmpty(keyWord))//关键词查询
            {
                query = query.Where(a => a.UserName.ToLower().Contains(keyWord.ToLower()));
            }

            list.Total = query.Count();
            query = query.OrderByDescending(a => a.CreateTime).Skip((pageNo - 1) * pageSize).Take(pageSize);
            list.ResultData = query.ToList();
            list.TotalPagesCount = list.Total / pageSize + 1;

            list.ResultData.ForEach(p =>
            {
                p.RoleIDs = userRoleList.Where(x => x.UserID == p.ID).Select(s => s.RoleID).ToList();
                p.RoleNames = userRoleList.Where(x => x.UserID == p.ID).Select(s => s.RoleName).ToList();
            });

            return list;
        }

        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <param name="ID">主键</param>
        /// <returns>用户实体</returns>
        public UserDto Get(string ID)
        {
            var query = _dbContext.Set<User>().Where(x => x.ID == ID).FirstOrDefault();
            if (query != null)
            {
                var dto = new UserDto();
                dto.ID = query.ID;
                dto.UserName = query.UserName;
                dto.LoginAccount = query.LoginAccount;
                dto.DeptName = query.DeptName;
                dto.IsActive = query.IsActive;
                dto.CreateTime = query.CreateTime;
                dto.Creator = query.Creator;
                dto.UpdateTime = query.UpdateTime;
                dto.Updator = query.Updator;

                //查找用户角色
                var userRoleList = _dbContext.Set<UserRole>().Where(x => x.UserID == ID && x.IsActive).Select(s => s.RoleID).ToList();
                dto.RoleIDs = userRoleList;

                return dto;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="item">用户Entity</param>
        /// <returns>返回操作结果</returns>
        public OperationResult Add(UserDto item, string userID)
        {
            if (item != null)
            {
                var existUser = _dbContext.Set<User>().Where(x => x.LoginAccount == item.LoginAccount);

                if (existUser.Any())
                {
                    return new OperationResult()
                    {
                        Result = false,
                        ResultMsg = "该登录账号已存在！"
                    };
                }

                try
                {
                    var addModel = new User();
                    addModel.ID = Guid.NewGuid().ToString();
                    addModel.DeptName = item.DeptName;
                    addModel.LoginAccount = item.LoginAccount;
                    addModel.UserName = item.UserName;
                    addModel.IsActive = true;
                    //todo 需要做加密处理
                    addModel.Password = "123456";
                    addModel.CreateTime = addModel.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    addModel.Creator = addModel.Updator = userID;

                    var roleModelList = new List<UserRole>();
                    if (item.RoleIDs != null && item.RoleIDs.Count > 0)
                    {
                        //增加人员角色
                        foreach (var roleID in item.RoleIDs)
                        {
                            var roleModel = new UserRole();
                            roleModel.ID = Guid.NewGuid().ToString();
                            roleModel.UserID = addModel.ID;
                            roleModel.RoleID = roleID;
                            roleModel.IsActive = true;
                            roleModel.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            roleModel.Creator = userID;

                            roleModelList.Add(roleModel);
                        }
                    }

                    if (roleModelList.Count > 0)
                    {
                        _dbContext.Set<UserRole>().AddRange(roleModelList);
                    }

                    _dbContext.Set<User>().Add(addModel);
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
                    ResultMsg = "正在添加的数据不存在！"
                };
            }
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user">用户Entity</param>
        /// <returns>返回操作结果</returns>
        public OperationResult Edit(UserDto item, string userID)
        {
            var editModel = _dbContext.Set<User>().Where(x => x.ID == item.ID).FirstOrDefault();
            if (editModel != null)
            {
                try
                {
                    //修改人员信息
                    editModel.UserName = item.UserName;
                    editModel.DeptName = item.DeptName;
                    editModel.Updator = userID;
                    editModel.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


                    //修改人员角色信息
                    var roleModelList = new List<UserRole>();
                    if (item.RoleIDs.Count > 0)
                    {
                        //逻辑删除人员角色信息
                        var userRoleList = _dbContext.Set<UserRole>().Where(x => x.UserID == item.ID).ToList();
                        if (userRoleList.Count > 0)
                        {
                            foreach (var userRole in userRoleList)
                            {
                                userRole.IsActive = false;
                            }
                        }

                        //添加需要新增人员角色信息
                        foreach (var roleID in item.RoleIDs)
                        {
                            var roleModel = new UserRole();
                            roleModel.ID = Guid.NewGuid().ToString();
                            roleModel.UserID = item.ID;
                            roleModel.RoleID = roleID;
                            roleModel.IsActive = true;
                            roleModel.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            roleModel.Creator = userID;

                            roleModelList.Add(roleModel);
                        }
                    }

                    if (roleModelList.Count > 0)
                    {
                        _dbContext.Set<UserRole>().AddRange(roleModelList);
                    }

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
        /// 修改用户密码
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns>返回操作结果</returns>
        public OperationResult EditPwd(string pwd, string operationUserID)
        {
            var editModel = _dbContext.Set<User>().Where(x => x.ID == operationUserID).FirstOrDefault();
            if (editModel != null)
            {
                try
                {
                    editModel.Password = pwd;
                    editModel.Updator = operationUserID;
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
                    ResultMsg = "需要修改的数据不存在！"
                };
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">需要删除的数据的主键</param>
        /// <param name="operationUserID">操作人ID</param>
        /// <returns>操作结果</returns>
        public OperationResult Delete(string ID, string operationUserID)
        {
            var deleteModel = _dbContext.Set<User>().Where(x => x.ID == ID).FirstOrDefault();
            if (deleteModel != null)
            {
                try
                {
                    deleteModel.IsActive = false;
                    deleteModel.Updator = operationUserID;
                    deleteModel.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
        /// 用户登录
        /// </summary>
        /// <param name="loginDto">用户输入的登录信息</param>
        /// <returns>登录成功的用户信息</returns>
        public OperationResult<User> Login(UserLoginDto loginDto)
        {
            var result = new OperationResult<User>()
            {
                Result = false,
                ResultData = null,
                ResultMsg = string.Empty
            };

            var userList = _dbContext.Set<User>().ToList();


            var userNameQuery = _dbContext.Set<User>().Where(x => x.LoginAccount.ToLower() == loginDto.LoginAccount.ToLower()).FirstOrDefault();

            if (userNameQuery == null)
            {
                //用户名不存在
                result.ResultMsg = "用户名不存在！";
            }
            else
            {
                var userPWDQuery = userList.Where(x => x.LoginAccount.ToLower() == loginDto.LoginAccount.ToLower() && x.Password == loginDto.Password && x.IsActive).FirstOrDefault();
                if (userPWDQuery == null)
                {
                    //密码错误
                    result.ResultMsg = "登录密码错误！";
                }
                else
                {
                    result.Result = true;
                    result.ResultData = userPWDQuery;
                    result.ResultMsg = "登录成功！";
                }
            }
            return result;
        }


        /// <summary>
        /// 根据用户获取角色菜单
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<MenuForDisplayDto> GetPrivilegeMenuByUserID(string userID)
        {

            var menuList = _dbContext.Set<Menu>().Where(x => x.IsActive).ToList();

            var userRoleMenuQuery = (from a in _dbContext.Set<User>()
                                     join b in _dbContext.Set<UserRole>() on a.ID equals b.UserID
                                     join c in _dbContext.Set<RoleMenu>() on b.RoleID equals c.RoleID
                                     join d in _dbContext.Set<Menu>() on c.MenuID equals d.ID
                                     where a.ID == userID && b.IsActive && c.IsActive && d.IsActive && d.MenuLevel < 3
                                     select d).ToList();

            //find rootMenu
            var rootMenus = userRoleMenuQuery.Where(x => x.ParentMenuID == null).OrderBy(o => o.OrderIndex).ToList();

            if (rootMenus.Where(x => x.MenuName == "退出/注销账号").FirstOrDefault() == null)
            {
                var specialMenu = menuList.Where(x => x.MenuName == "退出/注销账号").FirstOrDefault();
                if (specialMenu != null)
                {
                    rootMenus.Add(specialMenu);
                }
            }

            var rootMenusForDisplay = new List<MenuForDisplayDto>();

            rootMenus.ForEach(p =>
            {
                rootMenusForDisplay.Add(new MenuForDisplayDto()
                    {
                        menu_id = p.ID,
                        menu_name = p.MenuName,
                        menu_url = p.MenuURL,
                        menu_level = p.MenuLevel,
                        parent_menu_id = p.ParentMenuID,
                        order_index = p.OrderIndex,
                        icon = p.MenuIcon
                    });
            });

            if (rootMenusForDisplay != null)
            {
                foreach (var rootMenu in rootMenusForDisplay)
                {
                    GetMenuRecursion(rootMenu, userRoleMenuQuery);
                }
            }

            var result = new List<MenuForDisplayDto>();

            //将没有子菜单权限的父节点移除
            rootMenusForDisplay.ForEach(p =>
            {
                if (p.menu_name == "飞行逐日登记" || p.menu_name == "时控管理" || p.menu_name == "用户管理")
                {
                    if (p.child.Count > 0)
                    {
                        result.Add(p);
                    }
                }
                else
                {
                    result.Add(p);
                }
            });


            return result;
        }

        /// <summary>
        /// 递归查找Menu树
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="menus"></param>
        private void GetMenuRecursion(MenuForDisplayDto menu, List<Menu> menus)
        {
            var subMenus = menus.Where(x => x.ParentMenuID == menu.menu_id).OrderBy(o => o.OrderIndex).ToList();
            menu.child = new List<MenuForDisplayDto>();

            subMenus.ForEach(p =>
            {
                menu.child.Add(new MenuForDisplayDto()
                {
                    menu_id = p.ID,
                    menu_name = p.MenuName,
                    menu_url = p.MenuURL,
                    menu_level = p.MenuLevel,
                    parent_menu_id = p.ParentMenuID,
                    order_index = p.OrderIndex,
                    icon = p.MenuIcon
                });
            });

            foreach (var subMenu in menu.child)
            {
                GetMenuRecursion(subMenu, menus);
            }
        }
    }
}