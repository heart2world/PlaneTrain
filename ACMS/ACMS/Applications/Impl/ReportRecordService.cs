using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACMS.EF;
using ACMS.ApplicationBase;
using ACMS.Models;

namespace ACMS.Applications.Impl
{
    public class ReportRecordService : ServiceBase, IReportRecordService
    {
        private DbContext _dbContext = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReportRecordService()
        {
            _dbContext = base.CreateDbContext();
            base.AddDisposableObject(_dbContext);

        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns>数据列表</returns>
        public PageResult<ReportRecord> GetList(int pageSize, int pageNo, string reportType, string keyWord, string startDate, string endDate)
        {
            PageResult<ReportRecord> list = new PageResult<ReportRecord>();
            if (_dbContext == null)
            {
                _dbContext = base.CreateDbContext();
            }
            var result = _dbContext.Set<ReportRecord>().Where(a => a.IsActive);
            if(!string.IsNullOrEmpty(reportType))
            {
                result = result.Where(a => a.ReportType == reportType);
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                result = result.Where(a => a.Reportor.Contains(keyWord));
            }
            if (!string.IsNullOrEmpty(startDate))
            {
                result = result.Where(a => string.Compare(a.ReportTime,startDate)>=0);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                result = result.Where(a => string.Compare(a.ReportTime, endDate) <= 0);
            }
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
        public ReportRecord Get(string ID)
        {
            var query = _dbContext.Set<ReportRecord>().Where(x => x.ID == ID).FirstOrDefault();
            return query;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item">新增实体</param>
        /// <param name="userID">操作人ID</param>
        /// <returns>操作结果</returns>
        public OperationResult Add(ReportRecord item, string userID)
        {
            try
            {
                item.ID = Guid.NewGuid().ToString();
                item.CreateTime = item.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                item.Creator = item.Updator = userID;
                item.IsActive = true;
                _dbContext.Set<ReportRecord>().Add(item);
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
        /// 删除数据
        /// </summary>
        /// <param name="ID">删除数据主键</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        public OperationResult Delete(string ID, string userID)
        {
            var editModel = _dbContext.Set<ReportRecord>().Where(x => x.ID == ID).FirstOrDefault();
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
