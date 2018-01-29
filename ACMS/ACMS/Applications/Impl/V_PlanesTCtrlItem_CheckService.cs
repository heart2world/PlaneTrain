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
    public class V_PlanesTCtrlItem_CheckService : ServiceBase, IV_PlanesTCtrlItem_CheckService
    {
        private DbContext _dbContext = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public V_PlanesTCtrlItem_CheckService()
        {
            _dbContext = base.CreateDbContext();
            base.AddDisposableObject(_dbContext);

        }
        /// <summary>
        /// 时控项目检测获取列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="days"></param>
        /// <param name="airTime"></param>
        /// <param name="upTemprTime"></param>
        /// <param name="onOffTime"></param>
        /// <returns></returns>
        public PageResult<V_PlanesTCtrlItem_Check> GetList(int pageSize, int pageNo, int isPrinted, int days, decimal airTime, decimal upTemprTime, int onOffTime)
        {
            PageResult<V_PlanesTCtrlItem_Check> list = new PageResult<V_PlanesTCtrlItem_Check>();

            if (_dbContext == null)
            {
                _dbContext = base.CreateDbContext();
            }
            var result = _dbContext.Set<V_PlanesTCtrlItem_Check>().Where(a => a.IsActive && a.IsPrinted==isPrinted);
            result = result.Where(a => (a.DateDiffValue <= days && a.IsWatDate.Value) || 
            (a.IsWatAirTime.Value && a.RAirTime-a.PlanNewAirTime<=airTime)
            ||(a.IsWatUpTemprTime.Value && a.RUpTemprTime-a.PlanNewHeatingMachineTime<=upTemprTime)
            ||(a.IsWatOnOffTime.Value && a.ROnOffTime-a.PlanNewRiseAndFallNum<=onOffTime));
            list.Total = result.Count();
            result = result.OrderByDescending(a => a.CreateTime).Skip((pageNo - 1) * pageSize).Take(pageSize);
            list.ResultData = result.ToList();
            return list;
        }
    }
}
