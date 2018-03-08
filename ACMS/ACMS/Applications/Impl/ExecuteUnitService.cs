using ACMS.ApplicationBase;
using ACMS.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ACMS.Applications.Impl
{
    public class ExecuteUnitService : ServiceBase, IExecuteUnitService
    {
        private DbContext _dbContext = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ExecuteUnitService()
        {
            _dbContext = base.CreateDbContext();
            base.AddDisposableObject(_dbContext);
        }


        /// <summary>
        /// 获取单位列表
        /// </summary>
        /// <returns>单位列表</returns>
        public List<ExecuteUnit> GetList()
        {
            return _dbContext.Set<ExecuteUnit>().Where(x => x.IsActive).OrderBy(o => o.OrderIndex).ToList();
        }
    }
}