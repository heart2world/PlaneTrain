using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACMS.ApplicationBase;
using ACMS.EF;
using ACMS.Models;

namespace ACMS.Applications.Impl
{
    public class PlanesTCtrlItemServices : ServiceBase, IPlanesTCtrlItemServices
    {
        private DbContext _dbContext = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PlanesTCtrlItemServices()
        {
            _dbContext = base.CreateDbContext();
            base.AddDisposableObject(_dbContext);

        }

        
        /*
        public PageResult<PlanesTCtrlItemDto> GetList(int pageSize, int pageNo, string planeTypeID, string planeNo, string listID, string keyWord)
        {
            PageResult<PlanesTCtrlItemDto> list = new PageResult<PlanesTCtrlItemDto>();
            list.ResultData = new List<PlanesTCtrlItemDto>();

            if (_dbContext == null)
            {
                _dbContext = base.CreateDbContext();
            }
            //查询飞机时控项目基本信息 
            var result = (from a in _dbContext.Set<PlanesTCtrlItem>()
                          join b in _dbContext.Set<PlanesTCtrlItem_ReList>() on a.ID equals b.PlanesTCtrlItemID
                          where a.IsActive
                          select new
                          {
                              CreateTime = a.CreateTime,
                              Creator = a.Creator,
                              FAirTime = a.FAirTime,
                              FCheckDate = a.FCheckDate,
                              FOnOffTime = a.FOnOffTime,
                              FUpTemprTime = a.FUpTemprTime,
                              ID = a.ID,
                              IsActive = a.IsActive,
                              IsCtrl = a.IsCtrl,
                              IsWatAirTime = a.IsWatAirTime,
                              IsWatDate = a.IsWatDate,
                              IsWatOnOffTime = a.IsWatOnOffTime,
                              IsWatUpTemprTime = a.IsWatUpTemprTime,
                              JianNO = a.JianNO,
                              Memo = a.Memo,
                              PlaneNo = a.PlaneNo,
                              PlaneTypeID = a.PlaneTypeID,
                              RAirTime = a.RAirTime,
                              RCheckDate = a.RCheckDate,
                              ROnOffTime = a.ROnOffTime,
                              RUpTemprTime = a.RUpTemprTime,
                              SerialNO = a.SerialNO,
                              TSN = a.TSN,
                              TSO = a.TSO,
                              UpdateTime = a.UpdateTime,
                              Updator = a.Updator
                          }
                          );
            if (!string.IsNullOrEmpty(keyWord))//关键词查询    
                result = result.Where(a => a.JianNO.Contains(keyWord) || a.SerialNO.Contains(keyWord));
            if (!string.IsNullOrEmpty(planeTypeID))//查询机型ID    
                result = result.Where(a => a.PlaneTypeID.Equals(planeTypeID));
            if (!string.IsNullOrEmpty(planeNo))//查询机型机号ID    
                result = result.Where(a => a.PlaneNo.Equals(planeNo));
            if (!string.IsNullOrEmpty(listID))//查询机型机号ID    
                result = result.Where(a => a.PlaneNo.Equals(listID));
            list.Total = result.Distinct().Count();
            //查询信息
            result = result.Distinct().OrderByDescending(a => a.CreateTime).Skip((pageNo - 1) * pageSize).Take(pageSize);
            if (result != null && list.Total > 0)
            {
                //查询时控醒目项目清单名称
                var result2 = (from a in _dbContext.Set<PlanesTCtrlItem_ReList>()
                               join b in _dbContext.Set<PlTypeTCtrlList>() on a.PlTypeTCtrl equals b.ID
                               select new
                               {
                                   PlanesTCtrlItemID = a.PlanesTCtrlItemID,
                                   MainTainName = b.MainTainName

                               }
                              ).ToList();

                foreach (var aa in result)
                {
                    PlanesTCtrlItemDto tempItem = new PlanesTCtrlItemDto();
                    tempItem.CreateTime = aa.CreateTime;
                    tempItem.Creator = aa.Creator;
                    tempItem.FAirTime = aa.FAirTime;
                    tempItem.FCheckDate = aa.FCheckDate;
                    tempItem.FOnOffTime = aa.FOnOffTime;
                    tempItem.FUpTemprTime = aa.FUpTemprTime;
                    tempItem.ID = aa.ID;
                    tempItem.IsActive = aa.IsActive;
                    tempItem.IsCtrl = aa.IsCtrl;
                    tempItem.IsWatAirTime = aa.IsWatAirTime;
                    tempItem.IsWatDate = aa.IsWatDate;
                    tempItem.IsWatOnOffTime = aa.IsWatOnOffTime;
                    tempItem.IsWatUpTemprTime = aa.IsWatUpTemprTime;
                    tempItem.JianNO = aa.JianNO;
                    tempItem.Memo = aa.Memo;
                    tempItem.PlaneNo = aa.PlaneNo;
                    tempItem.PlaneTypeID = aa.PlaneTypeID;
                    tempItem.RAirTime = aa.RAirTime;
                    tempItem.RCheckDate = aa.RCheckDate;
                    tempItem.ROnOffTime = aa.ROnOffTime;
                    tempItem.RUpTemprTime = aa.RUpTemprTime;
                    tempItem.SerialNO = aa.SerialNO;
                    tempItem.TSN = aa.TSN;
                    tempItem.TSO = aa.TSO;
                    tempItem.UpdateTime = aa.UpdateTime;
                    tempItem.Updator = aa.Updator;
                    result2 = result2.Where(a => a.PlanesTCtrlItemID.Equals(aa.ID)).ToList();
                    List<string> sMaintanceName = new List<string>();//添加清单名称
                    foreach (var bb in result2)
                    {
                        sMaintanceName.Add(bb.MainTainName);
                    }
                    tempItem.ReList = sMaintanceName;
                    list.ResultData.Add(tempItem);
                }


            }
            //list.ResultData = result.ToList();



            return list;
        }
        */
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns>数据列表</returns>
        public PageResult<V_PlanesTCtrlItem> GetList(int pageSize, int pageNo, string planeTypeID, string planeNo, string listID, string keyWord)
        {
            PageResult<V_PlanesTCtrlItem> list = new PageResult<V_PlanesTCtrlItem>();

            if (_dbContext == null)
            {
                _dbContext = base.CreateDbContext();
            }
            var result = _dbContext.Set<V_PlanesTCtrlItem>().Where(a => a.IsActive);
            if (!string.IsNullOrEmpty(planeTypeID))//关键词查询    
                result = result.Where(a => a.PlaneTypeID.Equals(planeTypeID));
            if (!string.IsNullOrEmpty(planeNo))//关键词查询    
                result = result.Where(a => a.PlaneNo.Equals(planeNo));
            if (!string.IsNullOrEmpty(listID))//关键词查询    
                result = result.Where(a => a.PlTypeTCtrl.Equals(listID));
            if (!string.IsNullOrEmpty(keyWord))//关键词查询    
                result = result.Where(a => a.JianNO.Contains(keyWord) || a.SerialNO.Contains(keyWord));
            list.Total = result.Count();
            result = result.OrderByDescending(a => a.CreateTime).Skip((pageNo - 1) * pageSize).Take(pageSize);
            list.ResultData = result.ToList();
            return list;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>用户历史记录列表信息</returns>
        public PageResult<V_PlanesTCtrlItem_Log> GetListHistory(int pageSize, int pageNo, string itemID)
        {
            PageResult<V_PlanesTCtrlItem_Log> list = new PageResult<V_PlanesTCtrlItem_Log>();

            if (_dbContext == null)
            {
                _dbContext = base.CreateDbContext();
            }
            var result = _dbContext.Set<V_PlanesTCtrlItem_Log>().Where(a => a.IsActive);
            if (!string.IsNullOrEmpty(itemID))//关键词查询    
                result = result.Where(a => a.ItemID.Equals(itemID));
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
        public PlanesTCtrlItemDto Get(string ID)
        {
            PlanesTCtrlItemDto item = new PlanesTCtrlItemDto();

            var query = _dbContext.Set<PlanesTCtrlItem>().Where(x => x.ID == ID).FirstOrDefault();
            if (query != null)
            {
                item.CreateTime = query.CreateTime;
                item.Creator = query.Creator;
                item.FAirTime = query.FAirTime;
                item.FCheckDate = query.FCheckDate;
                item.FOnOffTime = query.FOnOffTime;
                item.FUpTemprTime = query.FUpTemprTime;
                item.ID = query.ID;
                item.IsActive = query.IsActive;
                item.IsCtrl = query.IsCtrl;
                item.IsWatAirTime = query.IsWatAirTime;
                item.IsWatDate = query.IsWatDate;
                item.IsWatOnOffTime = query.IsWatOnOffTime;
                item.IsWatUpTemprTime = query.IsWatUpTemprTime;
                item.JianNO = query.JianNO;
                item.Memo = query.Memo;
                item.PlaneNo = query.PlaneNo;
                item.PlaneTypeID = query.PlaneTypeID;
                item.RAirTime = query.RAirTime;
                item.RCheckDate = query.RCheckDate;
                item.ROnOffTime = query.ROnOffTime;
                item.RUpTemprTime = query.RUpTemprTime;
                item.SerialNO = query.SerialNO;
                item.TSN = query.TSN;
                item.TSO = query.TSO;
                item.UpdateTime = query.UpdateTime;
                item.Updator = query.Updator;
                item.ExeCapacity = query.ExeCapacity;

                var query2 = _dbContext.Set<PlanesTCtrlItem_ReList>().Where(x => x.PlanesTCtrlItemID == ID).ToList();
                if (query2 != null && query2.Count > 0)
                {
                    List<string> sReList = new List<string>();
                    foreach (PlanesTCtrlItem_ReList idItem in query2)
                    {
                        sReList.Add(idItem.PlTypeTCtrl);
                    }
                    item.ReList = sReList;
                }
            }
            return item;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item">新增实体</param>
        /// <param name="userID">操作人ID</param>
        /// <returns>操作结果</returns>
        public OperationResult Add(PlanesTCtrlItemDto item, string userID)
        {

            try
            {
                item.ID = Guid.NewGuid().ToString();
                item.CreateTime = item.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                item.Creator = item.Updator = userID;
                item.IsActive = true;
                PlanesTCtrlItem tempItem = new PlanesTCtrlItem();//临时变量

                tempItem.CreateTime = item.CreateTime;
                tempItem.Creator = item.Creator;
                tempItem.FAirTime = item.FAirTime;
                tempItem.FCheckDate = item.FCheckDate;
                tempItem.FOnOffTime = item.FOnOffTime;
                tempItem.FUpTemprTime = item.FUpTemprTime;
                tempItem.ID = item.ID;
                tempItem.IsActive = item.IsActive;
                tempItem.ExeCapacity = item.ExeCapacity;
                tempItem.IsCtrl = item.IsCtrl;
                tempItem.IsWatAirTime = item.IsWatAirTime;
                tempItem.IsWatDate = item.IsWatDate;
                tempItem.IsWatOnOffTime = item.IsWatOnOffTime;
                tempItem.IsWatUpTemprTime = item.IsWatUpTemprTime;
                tempItem.JianNO = item.JianNO;
                tempItem.Memo = item.Memo;
                tempItem.PlaneNo = item.PlaneNo;
                tempItem.PlaneTypeID = item.PlaneTypeID;
                tempItem.RAirTime = item.RAirTime;
                tempItem.RCheckDate = item.RCheckDate;
                tempItem.ROnOffTime = item.ROnOffTime;
                tempItem.RUpTemprTime = item.RUpTemprTime;
                tempItem.SerialNO = item.SerialNO;
                tempItem.TSN = item.TSN;
                tempItem.TSO = item.TSO;
                tempItem.UpdateTime = item.UpdateTime;
                tempItem.Updator = item.Updator;
                _dbContext.Set<PlanesTCtrlItem>().Add(tempItem);
                if (item.ReList != null)
                {
                    foreach (string id in item.ReList)
                    {
                        PlanesTCtrlItem_ReList reItem = new PlanesTCtrlItem_ReList();
                        reItem.PlanesTCtrlItemID = item.ID;
                        reItem.PlTypeTCtrl = id;
                        _dbContext.Set<PlanesTCtrlItem_ReList>().Add(reItem);
                    }
                }
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
        public OperationResult Update(PlanesTCtrlItemDto item, string userID)
        {
            
            var editModel = _dbContext.Set<PlanesTCtrlItem>().Where(x => x.ID == item.ID).FirstOrDefault();
            if (editModel != null)
            {
                try
                {
                    //修改信息
                    editModel.FAirTime = item.FAirTime;
                    editModel.FCheckDate = item.FCheckDate;
                    editModel.FOnOffTime = item.FOnOffTime;
                    editModel.FUpTemprTime = item.FUpTemprTime;
                    editModel.IsCtrl = item.IsCtrl;
                    editModel.IsWatAirTime = item.IsWatAirTime;
                    editModel.IsWatDate = item.IsWatDate;
                    editModel.IsWatOnOffTime = item.IsWatOnOffTime;
                    editModel.IsWatUpTemprTime = item.IsWatUpTemprTime;
                    editModel.JianNO = item.JianNO;
                    editModel.ExeCapacity = item.ExeCapacity;
                    editModel.Memo = item.Memo;
                    editModel.PlaneNo = item.PlaneNo;
                    editModel.PlaneTypeID = item.PlaneTypeID;
                    editModel.RAirTime = item.RAirTime;
                    editModel.RCheckDate = item.RCheckDate;
                    editModel.ROnOffTime = item.ROnOffTime;
                    editModel.RUpTemprTime = item.RUpTemprTime;
                    editModel.SerialNO = item.SerialNO;
                    editModel.TSN = item.TSN;
                    editModel.TSO = item.TSO;
                    editModel.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    editModel.Updator = userID;

                    if (item.ReList != null)
                    {
                        foreach (string id in item.ReList)
                        {
                            PlanesTCtrlItem_ReList reItem = new PlanesTCtrlItem_ReList();
                            reItem.PlanesTCtrlItemID = item.ID;
                            reItem.PlTypeTCtrl = id;
                            _dbContext.Set<PlanesTCtrlItem_ReList>().Add(reItem);
                        }
                    }


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
            var editModel = _dbContext.Set<PlanesTCtrlItem>().Where(x => x.ID == ID).FirstOrDefault();
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
