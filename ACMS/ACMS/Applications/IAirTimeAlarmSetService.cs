using ACMS.EF;
using ACMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACMS.Applications
{
    public interface IAirTimeAlarmSetService
    {
        /// <summary>
        /// /// 根据ID获取信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        AirTimeAlarmSet Get(string ID);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="item">修改数据实体</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        OperationResult Update(AirTimeAlarmSet item, string userID);
    }
}
