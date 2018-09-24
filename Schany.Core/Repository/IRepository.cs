
using Schany.Infrastructure;
using Schany.Infrastructure.Common.GenericConstraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Schany.Core.Repository
{
    /// <summary>
    /// 实体仓储模型的数据标准操作
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="Guid">主键类型</typeparam>
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        #region 属性
        /// <summary>
        /// 获取 当前实体类型的查询数据集，数据将使用不跟踪变化的方式来查询，
        /// 当数据用于展现时，推荐使用此数据集，如果用于新增，更新，删除时，请使用<see cref="TrackEntities"/>数据集
        /// </summary>
        IQueryable<TEntity> Entities { get; }

        /// <summary>
        /// 获取 当前实体类型的查询数据集，当数据用于新增，更新，删除时，
        /// 使用此数据集，如果数据用于展现，推荐使用<see cref="Entities"/>数据集
        /// </summary>
        IQueryable<TEntity> TrackEntities { get; }
        #endregion

        #region 方法
        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        Notification Insert(TEntity entity);

        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        Notification Insert(IEnumerable<TEntity> entities);

        /// <summary>
        /// 大批量插入数据
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="entities">数据</param>
        void BulkInsert(string tableName, List<TEntity> entities);

        
        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        Notification Recycle(TEntity entity);

        /// <summary>
        /// 逻辑删除指定编号的实体
        /// </summary>
        /// <param name="key">实体编号</param>
        /// <returns>操作影响的行数</returns>
        Notification Recycle(Guid key);

        /// <summary>
        /// 逻辑删除所有符合特定条件的实体
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <returns>操作影响的行数</returns>
        Notification Recycle(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 批量逻辑删除实体
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        Notification Recycle(IEnumerable<TEntity> entities);

        /// <summary>
        /// 逻辑还原实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        Notification Restore(TEntity entity);

        /// <summary>
        /// 逻辑还原指定编号的实体
        /// </summary>
        /// <param name="key">实体编号</param>
        /// <returns>操作影响的行数</returns>
        Notification Restore(Guid key);

        /// <summary>
        /// 逻辑还原所有符合特定条件的实体
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <returns>操作影响的行数</returns>
        Notification Restore(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 批量逻辑还原实体
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        Notification Restore(IEnumerable<TEntity> entities);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        int Delete(TEntity entity);

        /// <summary>
        /// 删除指定编号的实体
        /// </summary>
        /// <param name="key">实体主键</param>
        /// <returns>操作影响的行数</returns>
        int Delete(Guid key);

        /// <summary>
        /// 删除所有符合特定条件的实体
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <returns>操作影响的行数</returns>
        int Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        int Delete(IEnumerable<TEntity> entities);

        /// <summary>
        /// 以标识集合批量删除实体
        /// </summary>
        /// <param name="ids">标识集合</param>
        /// <param name="checkAction">删除前置检查委托</param>
        /// <param name="deleteFunc">删除委托，用于删除关联信息</param>
        /// <returns>业务操作结果</returns>
        Notification Delete(ICollection<Guid> ids, Action<TEntity> checkAction = null, Func<TEntity, TEntity> deleteFunc = null);

        /// <summary>
        /// 直接删除指定编号的实体
        /// </summary>
        /// <param name="key">实体主键</param>
        /// <returns></returns>
        int DeleteDirect(Guid key);

        /// <summary>
        /// 直接删除所有符合特定条件的实体
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <returns>操作影响的行数</returns>
        //int DeleteDirect(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 更新实体对象
        /// </summary>
        /// <param name="entity">更新后的实体对象</param>
        /// <returns>操作影响的行数</returns>
        Notification Update(TEntity entity);

        /// <summary>
        /// 批量更新实体对象
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        //int Update(IEnumerable<TEntity> entitys);



        /// <summary>
        /// 直接更新指定编号的数据
        /// </summary>
        /// <param name="key">实体编号</param>
        /// <param name="updatExpression">更新属性表达式</param>
        /// <returns>操作影响的行数</returns>
        //int UpdateDirect(Guid key, Expression<Func<TEntity, TEntity>> updatExpression);

        /// <summary>
        /// 直接更新指定条件的数据
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="updatExpression">更新属性表达式</param>
        /// <returns>操作影响的行数</returns>
        //int UpdateDirect(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updatExpression);

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="id">编辑的实体标识</param>
        /// <returns>是否存在</returns>
        bool CheckExists(Expression<Func<TEntity, bool>> predicate, Guid id = default(Guid));

        /// <summary>
        /// 查找指定主键的实体
        /// </summary>
        /// <param name="key">实体主键</param>
        /// <returns>符合主键的实体，不存在时返回null</returns>
        TEntity GetByKey(Guid key);

        
        ///// <summary>
        ///// 根据Id查询实体
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //TEntity FindById(Guid id);

        ///// <summary>
        ///// 查询符合条件的一个数据
        ///// </summary>
        ///// <param name="whereLambda">查询表达式</param>
        ///// <returns>实体</returns>
        //TEntity Find(Expression<Func<TEntity, bool>> whereLambda);

        ///// <summary>
        ///// 查询符合条件的多个数据
        ///// </summary>
        ///// <param name="whereLamdba"></param>
        ///// <returns></returns>
        //IQueryable<TEntity> FindList(Expression<Func<TEntity, bool>> whereLamdba);

        

        #endregion
    }
}
