using ACMS.ApplicationBase;
using ACMS.EF;
using ACMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ACMS.Applications.Impl
{
    public class PlTypeTCtrlListService : ServiceBase, IPlTypeTCtrlListService
    {
        private DbContext _dbContext = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PlTypeTCtrlListService()
        {
            _dbContext = base.CreateDbContext();
            base.AddDisposableObject(_dbContext);

        }

        /// <summary>
        /// 获取列表 历史记录
        /// </summary>
        /// <returns>用户列表信息</returns>
        public PageResult<V_V_PlTypeTCtrlList_Log> GetListHostory(int pageSize, int pageNo, string tCtrlID)
        {
            PageResult<V_V_PlTypeTCtrlList_Log> list = new PageResult<V_V_PlTypeTCtrlList_Log>();

            if (_dbContext == null)
            {
                _dbContext = base.CreateDbContext();
            }
            var result = _dbContext.Set<V_V_PlTypeTCtrlList_Log>().Where(a => a.IsActive);
            if (!string.IsNullOrEmpty(tCtrlID))//关键词查询    
                result = result.Where(a => a.TCtrlID == tCtrlID);
            list.Total = result.Count();
            result = result.OrderByDescending(a => a.ID).Skip((pageNo - 1) * pageSize).Take(pageSize);
            list.ResultData = result.ToList();
            return list;
        }

        /// <summary>
        /// 获取机型时控项目清单所有历史记录
        /// </summary>
        /// <returns>用户列表信息</returns>
        public PageResult<V_V_PlTypeTCtrlList_Log> GetAllHistoryList(int pageSize, int pageNo)
        {
            PageResult<V_V_PlTypeTCtrlList_Log> list = new PageResult<V_V_PlTypeTCtrlList_Log>();

            if (_dbContext == null)
            {
                _dbContext = base.CreateDbContext();
            }
            var result = _dbContext.Set<V_V_PlTypeTCtrlList_Log>().Where(a => a.IsActive);
            list.Total = result.Count();
            result = result.OrderByDescending(o=>o.PlaneTypeID).OrderByDescending(a => a.ID).Skip((pageNo - 1) * pageSize).Take(pageSize);
            list.ResultData = result.ToList();
            return list;
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns>数据列表</returns>
        public PageResult<V_PlTypeTCtrlList> GetList(int pageSize, int pageNo, string keyWord)
        {
            PageResult<V_PlTypeTCtrlList> list = new PageResult<V_PlTypeTCtrlList>();

            if (_dbContext == null)
            {
                _dbContext = base.CreateDbContext();
            }
            var result = _dbContext.Set<V_PlTypeTCtrlList>().Where(a => a.IsActive);
            if (!string.IsNullOrEmpty(keyWord))//关键词查询    
                result = result.Where(a => a.ProSource.Contains(keyWord));
            list.Total = result.Count();
            result = result.OrderByDescending(a => a.CreateTime).Skip((pageNo - 1) * pageSize).Take(pageSize);
            list.ResultData = result.ToList();
            return list;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>根据机型获取时控项目监控清单</returns>
        public PageResult<V_PlTypeTCtrlList> GetListByPlaneType(int pageSize, int pageNo, List<string> PlaneTypeID)
        {
            PageResult<V_PlTypeTCtrlList> list = new PageResult<V_PlTypeTCtrlList>();

            List<V_PlTypeTCtrlList> tempResult = new List<V_PlTypeTCtrlList>();//临时放结果的列表
            if (_dbContext == null)
            {
                _dbContext = base.CreateDbContext();
            }
            var result = _dbContext.Set<V_PlTypeTCtrlList>().Where(a => a.IsActive).ToList();
            if (result.Count > 0)
            {
                foreach (var temp in PlaneTypeID)
                {
                    if (!string.IsNullOrEmpty(temp))
                    {
                        tempResult.AddRange(result.Where(m => m.PlaneTypeID == temp).ToList());
                    }
                }
            }

            list.Total = tempResult.Count();
            tempResult = tempResult.OrderByDescending(a => a.CreateTime).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            list.ResultData = tempResult.ToList();
            return list;
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <param name="ID">主键</param>
        /// <returns>数据实体</returns>
        public PlTypeTCtrlList Get(string ID)
        {
            var query = _dbContext.Set<PlTypeTCtrlList>().Where(x => x.ID == ID).FirstOrDefault();
            return query;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item">新增实体</param>
        /// <param name="userID">操作人ID</param>
        /// <returns>操作结果</returns>
        public OperationResult Add(PlTypeTCtrlList item, string userID)
        {
            var query = _dbContext.Set<PlTypeTCtrlList>().Where(x => x.MainTainName.ToLower() == item.MainTainName.ToLower() && x.PlaneTypeID == item.PlaneTypeID && x.IsActive).FirstOrDefault();
            if (query != null)
            {
                return new OperationResult()
                {
                    Result = false,
                    ResultMsg = "该项目名称已存在"
                };
            }
            try
            {
                item.ID = Guid.NewGuid().ToString();
                item.CreateTime = item.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                item.Creator = item.Updator = userID;
                item.IsActive = true;
                _dbContext.Set<PlTypeTCtrlList>().Add(item);
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
        public OperationResult Update(PlTypeTCtrlList item, string userID)
        {
            var editModel = _dbContext.Set<PlTypeTCtrlList>().Where(x => x.ID == item.ID).FirstOrDefault();
            if (editModel != null)
            {
                try
                {
                    //修改信息
                    editModel.Capacity = item.Capacity;
                    editModel.FAirTime = item.FAirTime;
                    editModel.FCheckDate = item.FCheckDate;
                    editModel.FOnOffTime = item.FOnOffTime;
                    editModel.FUpTemprTime = item.FUpTemprTime;
                    editModel.MainTainName = item.MainTainName;
                    editModel.PlaneTypeID = item.PlaneTypeID;
                    editModel.ProSource = item.ProSource;
                    editModel.RAirTime = item.RAirTime;
                    editModel.RCheckDate = item.RCheckDate;
                    editModel.ROnOffTime = item.ROnOffTime; ;
                    editModel.RUpTemprTime = item.RUpTemprTime;
                    editModel.RUpTemprTime = item.RUpTemprTime;
                    editModel.Memo = item.Memo;
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
            var editModel = _dbContext.Set<PlTypeTCtrlList>().Where(x => x.ID == ID).FirstOrDefault();
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
    }
}