using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACMS.EF;
using ACMS.Models;

namespace ACMS.Applications
{
    public interface IV_PlanesTCtrlItem_CheckService
    {
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
        PageResult<V_PlanesTCtrlItem_Check> GetList(int pageSize, int pageNo, int isPrinted, int? days, decimal? airTime, decimal? upTemprTime, int? onOffTime);
    }
}
