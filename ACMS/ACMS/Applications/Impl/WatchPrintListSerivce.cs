using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACMS.ApplicationBase;
using ACMS.EF;
using ACMS.Models;
using System.Data.Entity;
namespace ACMS.Applications.Impl
{
    public class WatchPrintListSerivce : ServiceBase, IWatchPrintListSerivce
    {
        private DbContext _dbContext = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WatchPrintListSerivce()
        {
            _dbContext = base.CreateDbContext();
            base.AddDisposableObject(_dbContext);

        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns>数据列表</returns>
        public PageResult<WatchPrintList> GetList(int pageSize, int pageNo, string keyWord)
        {
            PageResult<WatchPrintList> list = new PageResult<WatchPrintList>();

            if (_dbContext == null)
            {
                _dbContext = base.CreateDbContext();
            }
            var result = _dbContext.Set<WatchPrintList>().Where(a => a.IsActive);
            list.Total = result.Count();
            result = result.OrderByDescending(a => a.CreateTime).Skip((pageNo - 1) * pageSize).Take(pageSize);
            list.ResultData = result.ToList();
            return list;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item">新增实体</param>
        /// <param name="userID">操作人ID</param>
        /// <returns>操作结果</returns>
        public OperationResult Add(WatchPrintList item, string userID)
        {
            var query = _dbContext.Set<WatchPrintList>().Where(x => x.PlaneID == item.PlaneID && x.CtrlItem == item.CtrlItem && x.IsActive).FirstOrDefault();
            if (query != null)
            {
                return new OperationResult()
                {
                    Result = false,
                    ResultMsg = "该信息已打印"
                };
            }
            try
            {
                item.ID = Guid.NewGuid().ToString();
                item.CreateTime = item.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                item.Creator = item.Updator = userID;
                item.IsActive = true;
                _dbContext.Set<WatchPrintList>().Add(item);
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
        /// 新增批量打印
        /// </summary>
        /// <param name="list"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public OperationResult AddList(List<WatchPrintList> list, string userID)
        {
            try
            {
                foreach (var item in list)
                {
                    Add(item, userID);
                }
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
    }
}
