using ACMS.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACMS.Applications
{
    public interface IExecuteUnitService
    {
        /// <summary>
        /// 获取单位列表
        /// </summary>
        /// <returns>单位列表</returns>
        List<ExecuteUnit> GetList();
    }
}
