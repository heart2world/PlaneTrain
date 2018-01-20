
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
    public class PlaneTypeService : ServiceBase, IPlaneTypeService
    {
        private DbContext _dbContext = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PlaneTypeService()
        {
            _dbContext = base.CreateDbContext();
            base.AddDisposableObject(_dbContext);

        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns>数据列表</returns>
        public PageResult<PlaneType> GetList(int pageSize, int pageNo, string keyWord)
        {
            PageResult<PlaneType> list = new PageResult<PlaneType>();

            if (_dbContext == null)
            {
                _dbContext = base.CreateDbContext();
            }
            var result = _dbContext.Set<PlaneType>().Where(a => a.IsActive);
            if (!string.IsNullOrEmpty(keyWord))//关键词查询    
                result = result.Where(a => a.TypeName.Contains(keyWord));
            list.Total = result.Count();
            result = result.OrderByDescending(a => a.CreateTime).Skip((pageNo - 1) * pageSize).Take(pageSize);
            list.ResultData = result.ToList();
            return list;
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <param name="ID">主键</param>
        /// <returns>数据实体</returns>
        public PlaneType Get(string ID)
        {
            var query = _dbContext.Set<PlaneType>().Where(x => x.ID == ID).FirstOrDefault();
            return query;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item">新增实体</param>
        /// <param name="userID">操作人ID</param>
        /// <returns>操作结果</returns>
        public OperationResult Add(PlaneType item, string userID)
        {
            var query = _dbContext.Set<PlaneType>().Where(x => x.TypeName == item.TypeName && x.IsActive).FirstOrDefault();
            if (query != null)
            {
                return new OperationResult()
                {
                    Result = false,
                    ResultMsg = "该机型名已存在"
                };
            }
            try
            {
                item.ID = Guid.NewGuid().ToString();
                item.CreateTime = item.UpdateTime = DateTime.Now.ToString();
                item.Creator = item.Updator = userID;
                item.IsActive = true;
                _dbContext.Set<PlaneType>().Add(item);
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
        public OperationResult Update(PlaneType item, string userID)
        {
            var editModel = _dbContext.Set<PlaneType>().Where(x => x.ID == item.ID).FirstOrDefault();
            if (editModel != null)
            {
                try
                {
                    //修改信息
                    editModel.TypeName = item.TypeName;
                    editModel.Memo = item.Memo;
                    editModel.Updator = userID;
                    editModel.UpdateTime = DateTime.Now.ToString();

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
            var editModel = _dbContext.Set<PlaneType>().Where(x => x.ID == ID).FirstOrDefault();
            if (editModel != null)
            {
                try
                {
                    //修改信息
                    editModel.IsActive = false;
                    editModel.Updator = userID;
                    editModel.UpdateTime = DateTime.Now.ToString();

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