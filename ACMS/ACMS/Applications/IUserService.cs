using ACMS.EF;
using ACMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACMS.Services
{
    public interface IUserService
    {
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns>用户列表信息</returns>
        PageResult<UserDto> GetList(int pageSize, int pageNo, string keyWord);


        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <param name="ID">主键</param>
        /// <returns>用户实体</returns>
        UserDto Get(string ID);


        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user">用户Entity</param>
        /// <returns>返回操作结果</returns>
        OperationResult Add(UserDto item, string operationUserID);


        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user">用户Entity</param>
        /// <returns>返回操作结果</returns>
        OperationResult Edit(UserDto item, string operationUserID);

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns>返回操作结果</returns>
        OperationResult EditPwd(string pwd, string operationUserID);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">需要删除的数据的主键</param>
        /// <param name="operationUserID">操作人ID</param>
        /// <returns>操作结果</returns>
        OperationResult Delete(string ID, string operationUserID);


        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginDto">用户输入的登录信息</param>
        /// <returns>登录成功的用户信息</returns>
        OperationResult<User> Login(UserLoginDto loginDto);
    }
}
