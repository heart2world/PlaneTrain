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
    public class AirTimeAlarmSetService : ServiceBase, IAirTimeAlarmSetService
    {
        private DbContext _dbContext = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AirTimeAlarmSetService()
        {
            _dbContext = base.CreateDbContext();
            base.AddDisposableObject(_dbContext);

        }
        /// <summary>
        /// /// 根据ID获取信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public AirTimeAlarmSet Get(string ID)
        {
            var query = _dbContext.Set<AirTimeAlarmSet>().Where(x => x.ID == ID).FirstOrDefault();
            return query;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="item">修改数据实体</param>
        /// <param name="userID">操作人</param>
        /// <returns>操作结果</returns>
        public OperationResult Update(AirTimeAlarmSet item, string userID)
        {
            var editModel = _dbContext.Set<AirTimeAlarmSet>().Where(x => x.ID == item.ID).FirstOrDefault();
            if (editModel != null)
            {
                try
                {
                    //修改信息
                    editModel.Days = item.Days;
                    editModel.ReNewAirTime = item.ReNewAirTime;
                    editModel.ReNewOnOffTime = item.ReNewOnOffTime;
                    editModel.ReNewUpTempTime = item.ReNewUpTempTime;
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
    }
}