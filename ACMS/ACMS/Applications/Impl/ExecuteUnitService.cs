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


        /// <summary>
        /// 根据飞机号获取执行单位
        /// </summary>
        /// <param name="planeIDs">飞机号列表</param>
        /// <returns></returns>
        public List<ExecuteUnit> GetExecuteUnitByPlaneIDs(List<string> planeIDs)
        {
            var planeQuery = _dbContext.Set<Planes>().Where(x => planeIDs.Contains(x.ID)).Select(s => s.ExecuteUnitID).Distinct().ToList();

            return _dbContext.Set<ExecuteUnit>().Where(x => planeQuery.Contains(x.ID)).ToList();
        }
    }
}