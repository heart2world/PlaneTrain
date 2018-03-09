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
    public class PlanesService : ServiceBase, IPlanesService
    {
        private DbContext _dbContext = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PlanesService()
        {
            _dbContext = base.CreateDbContext();
            base.AddDisposableObject(_dbContext);

        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns>数据列表</returns>
        public PageResult<V_Planes> GetList(int pageSize, int pageNo, string keyWord)
        {
            PageResult<V_Planes> list = new PageResult<V_Planes>();

            if (_dbContext == null)
            {
                _dbContext = base.CreateDbContext();
            }
            var result = _dbContext.Set<V_Planes>().Where(a => a.IsActive);
            if (!string.IsNullOrEmpty(keyWord))//关键词查询    
                result = result.Where(a => a.PlaneNo.Contains(keyWord));
            list.Total = result.Count();
            result = result.OrderByDescending(a => a.CreateTime).Skip((pageNo - 1) * pageSize).Take(pageSize);
            list.ResultData = result.ToList();
            //查询每个
            if(list.ResultData!=null && list.ResultData.Count>0)
            {
                foreach(var item in list.ResultData)
                {
                    var tempResult = _dbContext.Set<PlanesTCtrlItem>().Where(a => a.IsActive && a.PlaneNo==item.ID && a.IsCtrl).ToList();
                    if(tempResult!=null && tempResult.Count>0)
                    {//如果飞机监控项目中存在任何一项处于监控的项目  则表示该飞机处于监控下
                        item.IsMonitored = true;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 根据机型ID获取列表
        /// </summary>
        /// <returns>列表信息</returns>
        public PageResult<Planes> GetListByPlaneType(int pageSize, int pageNo, List<string> PlaneTypeID)
        {
            PageResult<Planes> list = new PageResult<Planes>();

            if (_dbContext == null)
            {
                _dbContext = base.CreateDbContext();
            }
            var result = _dbContext.Set<Planes>().Where(a => a.IsActive).ToList();
            List<Planes> tempResult = new List<Planes>();//临时放结果的列表
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
        /// 根据机型名称获取列表(只针对逐日登记的2种类型)
        /// </summary>
        /// <returns>列表信息</returns>
        public List<Planes> GetListByPlaneType(string SpecialPlaneType)
        {
            List<Planes> list = new List<Planes>();

            if (_dbContext == null)
            {
                _dbContext = base.CreateDbContext();
            }
            var result = (from a in _dbContext.Set<Planes>()
                          join b in _dbContext.Set<PlaneType>() on a.PlaneTypeID equals b.ID
                          where a.IsActive && b.TypeName.ToLower() == SpecialPlaneType.ToLower()
                          select a).ToList();

            return result;
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <param name="ID">主键</param>
        /// <returns>数据实体</returns>
        public Planes Get(string ID)
        {
            var query = _dbContext.Set<Planes>().Where(x => x.ID == ID).FirstOrDefault();
            return query;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item">新增实体</param>
        /// <param name="userID">操作人ID</param>
        /// <returns>操作结果</returns>
        public OperationResult Add(Planes item, string userID)
        {
            var query = _dbContext.Set<Planes>().Where(x => x.PlaneNo == item.PlaneNo && x.IsActive).FirstOrDefault();
            if (query != null)
            {
                return new OperationResult()
                {
                    Result = false,
                    ResultMsg = "该机号已存在"
                };
            }
            try
            {
                item.ID = Guid.NewGuid().ToString();
                item.CreateTime = item.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                item.Creator = item.Updator = userID;
                item.IsActive = true;
                _dbContext.Set<Planes>().Add(item);
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
        public OperationResult Update(Planes item, string userID)
        {
            var editModel = _dbContext.Set<Planes>().Where(x => x.ID == item.ID).FirstOrDefault();
            if (editModel != null)
            {
                try
                {
                    //修改信息
                    editModel.PlaneNo = item.PlaneNo;
                    editModel.PlaneFacDate = item.PlaneFacDate;
                    editModel.PlaneSeria = item.PlaneSeria;
                    editModel.PlaneTypeID = item.PlaneTypeID;
                    editModel.IsMonitored = item.IsMonitored;
                    editModel.RankNo = item.RankNo;
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
            var editModel = _dbContext.Set<Planes>().Where(x => x.ID == ID).FirstOrDefault();
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