using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Schany.Infrastructure;
using Schany.Infrastructure.Common.GenericConstraints;
using Schany.Data.DataContext;
using Schany.Infrastructure.Common.Extensions;
using System.Data.Common;
using System.ComponentModel;

namespace Schany.Core.Repository
{
    /// <summary>
    /// EntityFramework的仓储实现
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">主键类型</typeparam>
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly DbSet<TEntity> _dbSet;
        private SchanyDbContext dbContext;
        /// <summary>
        /// 初始化一个<see cref="Repository{TEntity}"/>类型的新实例
        /// </summary>
        public Repository(SchanyDbContext schanyDbContext)
        {
            dbContext = schanyDbContext;
            _dbSet = dbContext.Set<TEntity>();
        }


        /// <summary>
        /// 获取 当前实体类型的查询数据集，数据将使用不跟踪变化的方式来查询，当数据用于展现时，
        /// 推荐使用此数据集，如果用于新增，更新，删除时，请使用<see cref="TrackEntities"/>数据集
        /// </summary>
        public IQueryable<TEntity> Entities
        {
            get
            {
                return _dbSet.AsNoTracking();
            }
        }

        /// <summary>
        /// 获取 当前实体类型的查询数据集，当数据用于新增，更新，删除时，使用此数据集，如果数据用于展现，推荐使用<see cref="Entities"/>数据集
        /// </summary>
        public IQueryable<TEntity> TrackEntities
        {
            get { return _dbSet; }
        }

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        public Notification Insert(TEntity entity)
        {
            //entity.CheckNotNull("entity");
            //entity.CheckICreateUser<TEntity>();
            //.CheckIOrganization<TEntity>();
            _dbSet.Add(entity);
            try
            {
                var res = dbContext.SaveChanges();
                if (res > 0)
                {
                    return new Notification(NotifyType.Success, "新增数据成功");
                }
                else
                {
                    return new Notification(NotifyType.Warning, "本次没有新增任何数据");
                }
            }
            catch (Exception ex)
            {
                return new Notification(NotifyType.Error, "新增数据发生异常：" + ex.Message);
            }
        }

        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public Notification Insert(IEnumerable<TEntity> entities)
        {
            entities = entities as TEntity[] ?? entities.ToArray();
            //foreach (TEntity entity in entities)
            //{
            //    entity.CheckICreateUser<TEntity>();//.CheckIOrganization<TEntity>();
            //}
            _dbSet.AddRange(entities);
            try
            {
                var res = dbContext.SaveChanges();
                if (res > 0)
                {
                    return new Notification(NotifyType.Success, "新增数据成功");
                }
                else
                {
                    return new Notification(NotifyType.Warning, "本次没有新增任何数据");
                }
            }
            catch (Exception ex)
            {
                return new Notification(NotifyType.Error, "新增数据发生异常：" + ex.Message);
            }
        }

        #region 大批量插入（未经测试）
        /// <summary>
        /// 大批量插入数据
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="entities">数据</param>
        public void BulkInsert(string tableName, List<TEntity> entities)
        {

            if (dbContext.Database.GetDbConnection().State != ConnectionState.Open)
            {
                dbContext.Database.OpenConnection();
            }

            //调用BulkInsert方法,将entitys集合数据批量插入到数据库的StudyTimes表中
            BulkInsert<TEntity>((SqlConnection)dbContext.Database.GetDbConnection(), tableName, entities);

            if (dbContext.Database.GetDbConnection().State != ConnectionState.Closed)
            {
                dbContext.Database.CloseConnection();
            }
        }
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="T">泛型集合的类型</typeparam>
        /// <param name="conn">连接对象</param>
        /// <param name="tableName">将泛型集合插入到本地数据库表的表名</param>
        /// <param name="list">要插入大泛型集合</param>
        private void BulkInsert<T>(SqlConnection conn, string tableName, IList<T> list)
        {
            using (var bulkCopy = new SqlBulkCopy(conn))
            {
                bulkCopy.BatchSize = list.Count;
                bulkCopy.DestinationTableName = tableName;

                var table = new DataTable();
                var props = TypeDescriptor.GetProperties(typeof(T))
                    .Cast<PropertyDescriptor>()
                    .Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System") || propertyInfo.PropertyType.Namespace.Contains("Public.Data.Enums"))
                    .ToArray();
                //这个地方写死了Public.Data.Enums,不然不会插入枚举

                foreach (var propertyInfo in props)
                {
                    bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                    table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
                }

                var values = new object[props.Length];
                foreach (var item in list)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }

                    table.Rows.Add(values);
                }

                bulkCopy.WriteToServer(table);
            }
        }
        #endregion

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        public Notification Recycle(TEntity entity)
        {
            //entity.CheckNotNull("entity");
            //entity.CheckIRecycle<TEntity>(RecycleOperation.LogicDelete);            
            return Update(entity);
        }

        /// <summary>
        /// 逻辑删除指定编号的实体
        /// </summary>
        /// <param name="key">实体编号</param>
        /// <returns>操作影响的行数</returns>
        public Notification Recycle(Guid key)
        {
            CheckEntityKey(key, "key");
            TEntity entity = _dbSet.Find(key);
            if (entity == null)
            {
                return new Notification(NotifyType.Error, "指定的数据没有找到");
            }
            else
            {
                return Recycle(entity);
            }
        }

        /// <summary>
        /// 逻辑删除所有符合特定条件的实体
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <returns>操作影响的行数</returns>
        public Notification Recycle(Expression<Func<TEntity, bool>> predicate)
        {
            //predicate.CheckNotNull("predicate");
            TEntity[] entities = _dbSet.Where(predicate).ToArray();
            return Recycle(entities);
        }

        /// <summary>
        /// 批量逻辑删除实体
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public Notification Recycle(IEnumerable<TEntity> entities)
        {
            entities = entities as TEntity[] ?? entities.ToArray();
            int result = 0;
            foreach (TEntity entity in entities)
            {
                //entity.CheckIRecycle<TEntity>(RecycleOperation.LogicDelete);
                var res = Update(entity);
                if (res.NotifyType == NotifyType.Success)
                {
                    result += (int)res.AppendData;
                }
            }
            return new Notification(NotifyType.Success, string.Format("本次共成功删除{0}条数据", result));
        }

        /// <summary>
        /// 逻辑还原实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        public Notification Restore(TEntity entity)
        {
            //entity.CheckNotNull("entity");
            //entity.CheckIRecycle<TEntity>(RecycleOperation.Restore);
            return Update(entity);
        }

        /// <summary>
        /// 逻辑还原指定编号的实体
        /// </summary>
        /// <param name="key">实体编号</param>
        /// <returns>操作影响的行数</returns>
        public Notification Restore(Guid key)
        {
            CheckEntityKey(key, "key");
            TEntity entity = _dbSet.Find(key);
            if (entity == null)
            {
                return new Notification(NotifyType.Error, "指定的数据没有找到");
            }
            return Restore(entity);
        }

        /// <summary>
        /// 逻辑还原所有符合特定条件的实体
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <returns>操作影响的行数</returns>
        public Notification Restore(Expression<Func<TEntity, bool>> predicate)
        {
            //predicate.CheckNotNull("predicate");
            TEntity[] entities = _dbSet.Where(predicate).ToArray();
            return Restore(entities);
        }

        /// <summary>
        /// 批量逻辑还原实体
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public Notification Restore(IEnumerable<TEntity> entities)
        {
            entities = entities as TEntity[] ?? entities.ToArray();
            int result = 0;
            foreach (TEntity entity in entities)
            {
                //entity.CheckIRecycle<TEntity>(RecycleOperation.Restore);              
                var res = Update(entity);
                if (res.NotifyType == NotifyType.Success)
                {
                    result += (int)res.AppendData;
                }
            }

            return new Notification(NotifyType.Success, string.Format("本次共成功还原{0}条数据", result));
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(TEntity entity)
        {
            try
            {
                entity.CheckNotNull("entity");
                //entity.CheckIRecycle<TEntity>(RecycleOperation.PhysicalDelete);
                _dbSet.Remove(entity);
                return dbContext.SaveChanges();
            }
            catch
            {
                return 0;
            }

        }

        /// <summary>
        /// 删除指定编号的实体
        /// </summary>
        /// <param name="key">实体编号</param>
        /// <returns>操作影响的行数</returns>
        public virtual int Delete(Guid key)
        {
            CheckEntityKey(key, "key");
            TEntity entity = _dbSet.Find(key);
            return entity == null ? 0 : Delete(entity);
        }

        /// <summary>
        /// 删除所有符合特定条件的实体
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            //predicate.CheckNotNull("predicate");
            TEntity[] entities = _dbSet.Where(predicate).ToArray();
            return entities.Length == 0 ? 0 : Delete(entities);
        }

        /// <summary>
        /// 批量删除删除实体
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(IEnumerable<TEntity> entities)
        {
            entities = entities as TEntity[] ?? entities.ToArray();
            //foreach (TEntity entity in entities)
            //{
            //    entity.CheckIRecycle<TEntity>(RecycleOperation.PhysicalDelete);
            //}
            _dbSet.RemoveRange(entities);
            return dbContext.SaveChanges();
        }

        /// <summary>
        /// 以标识集合批量删除实体
        /// </summary>
        /// <param name="ids">标识集合</param>
        /// <param name="checkAction">删除前置检查委托</param>
        /// <param name="deleteFunc">删除委托，用于删除关联信息</param>
        /// <returns>业务操作结果</returns>
        public Notification Delete(ICollection<Guid> ids, Action<TEntity> checkAction = null, Func<TEntity, TEntity> deleteFunc = null)
        {
            ids.CheckNotNull("ids");
            List<string> names = new List<string>();
            foreach (Guid id in ids)
            {
                TEntity entity = _dbSet.Find(id);
                try
                {
                    if (checkAction != null)
                    {
                        checkAction(entity);
                    }
                    if (deleteFunc != null)
                    {
                        entity = deleteFunc(entity);
                    }
                    //entity.CheckIRecycle<TEntity>(RecycleOperation.PhysicalDelete);
                    _dbSet.Remove(entity);
                }
                catch (Exception e)
                {
                    return new Notification(NotifyType.Error, e.Message);
                }
                string name = GetNameValue(entity);
                if (name != null)
                {
                    names.Add(name);
                }
            }
            int count = dbContext.SaveChanges();
            if (count > 0)
            {
                return new Notification(NotifyType.Success, "信息删除成功");
            }
            else
            {
                return new Notification(NotifyType.Success, "信息删除失败");
            }
        }

        /// <summary>
        /// 直接删除指定编号的实体
        /// </summary>
        /// <param name="key">实体主键</param>
        /// <returns></returns>
        public int DeleteDirect(Guid key)
        {
            CheckEntityKey(key, "key");
            return DeleteDirect(key);
        }

        /// <summary>
        /// 直接删除所有符合特定条件的实体
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <returns>操作影响的行数</returns>
        //public int DeleteDirect(Expression<Func<TEntity, bool>> predicate)
        //{
        //    //predicate.CheckNotNull("predicate");
        //    return _dbSet.Where(predicate).Delete();
        //}

        /// <summary>
        /// 更新实体对象
        /// </summary>
        /// <param name="entity">更新后的实体对象</param>
        /// <returns>操作影响的行数</returns>
        public Notification Update(TEntity entity)
        {
            entity.CheckNotNull("entity");
            ((DbContext)dbContext).Update<TEntity>(entity);

            try
            {
                var res = dbContext.SaveChanges();
                if (res > 0)
                {
                    return new Notification(NotifyType.Success, "数据更新成功", res);
                }
                else
                {
                    return new Notification(NotifyType.Warning, "本次没有更新任何数据");
                }
            }
            catch (Exception ex)
            {
                return new Notification(NotifyType.Error, "更新数据发生异常：" + ex.Message);
            }
        }
        /// <summary>
        /// 批量更新实体对象
        /// </summary>
        /// <param name="entitys">实体序列</param>
        /// <returns></returns>
        //public int Update(IEnumerable<TEntity> entitys)
        //{
        //    //entitys.CheckNotNull("entitys");
        //    ((DbContext)dbContext).Update<TEntity>(entitys.ToArray());
        //    return dbContext.SaveChanges();
        //}



        /// <summary>
        /// 直接更新指定编号的数据
        /// </summary>
        /// <param name="key">实体编号</param>
        /// <param name="updatExpression">更新属性表达式</param>
        /// <returns>操作影响的行数</returns>
        //public int UpdateDirect(Guid key, Expression<Func<TEntity, TEntity>> updatExpression)
        //{
        //    //CheckEntityKey(key, "key");
        //    //updatExpression.CheckNotNull("updatExpression");
        //    return UpdateDirect(m => m.Id.Equals(key), updatExpression);
        //}

        /// <summary>
        /// 直接更新指定条件的数据
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="updatExpression">更新属性表达式</param>
        /// <returns>操作影响的行数</returns>
        //public int UpdateDirect(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updatExpression)
        //{
        //    //predicate.CheckNotNull("predicate");
        //    //updatExpression.CheckNotNull("updatExpression");
        //    return _dbSet.Where(predicate).Update(updatExpression);
        //}

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="id">编辑的实体标识</param>
        /// <returns>是否存在</returns>
        public bool CheckExists(Expression<Func<TEntity, bool>> predicate, Guid id = default(Guid))
        {
            predicate.CheckNotNull("predicate");
            Guid defaultId = default(Guid);
            var entity = _dbSet.Where(predicate).Select(m => new { m.Id }).FirstOrDefault();
            bool exists = (!(typeof(Guid).IsValueType) && id.Equals(null)) || id.Equals(defaultId)
                ? entity != null
                : entity != null && !entity.Id.Equals(id);
            return exists;
        }

        /// <summary>
        /// 查找指定主键的实体
        /// </summary>
        /// <param name="key">实体主键</param>
        /// <returns>符合主键的实体，不存在时返回null</returns>
        public TEntity GetByKey(Guid key)
        {
            CheckEntityKey(key, "key");
            return _dbSet.Find(key);
        }


        #region 私有方法

        private static void CheckEntityKey(object key, string keyName)
        {
            key.CheckNotNull("key");
            keyName.CheckNotNull("keyName");

            Type type = key.GetType();
            if (type == typeof(int))
            {
                ((int)key).CheckGreaterThan(keyName, 0);
            }
            else if (type == typeof(string))
            {
                ((string)key).CheckNotNullOrEmpty(keyName);
            }
            else if (type == typeof(Guid))
            {
                ((Guid)key).CheckNotEmpty(keyName);
            }
        }
        private static string GetNameValue(object value)
        {
            dynamic obj = value;
            try
            {
                return obj.Name;
            }
            catch
            {
                return null;
            }
        }
        #endregion

    }
}
