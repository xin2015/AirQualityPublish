using AirQualityPublish.BLL.Models;
using AirQualityPublish.Model;
using AirQualityPublish.Model.Entities;
using AirQualityPublish.Model.Repositories;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.OpenAccess;

namespace AirQualityPublish.BLL.Syncs
{
    public interface ISync
    {
        /// <summary>
        /// 同步数据
        /// </summary>
        void Sync();

        /// <summary>
        /// 同步数据
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        ReturnStatus Sync(DateTime time);

        /// <summary>
        /// 同步数据
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        ReturnStatus Sync(DateTime beginTime, DateTime endTime);

        /// <summary>
        /// 回补数据
        /// </summary>
        void Cover();

        /// <summary>
        /// 回补数据
        /// </summary>
        /// <param name="list">回补记录集合</param>
        void Cover(IEnumerable<MissingDataRecord> list);

        /// <summary>
        /// 回补数据
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        ReturnStatus Cover(DateTime time);

        /// <summary>
        /// 回补数据
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        ReturnStatus Cover(DateTime beginTime, DateTime endTime);
    }

    public abstract class SyncBase<TEntity> : ISync where TEntity : ICodeTimeEntity
    {
        protected ILog Logger { get; set; }
        protected CodeTimeEntityRepository<TEntity> Repository { get; set; }
        protected MissingDataRecordRepository MDRRepository { get; set; }
        protected string Type { get; set; }
        protected TimeSpan Interval { get; set; }

        public SyncBase(OpenAccessContext context)
        {
            Repository = new CodeTimeEntityRepository<TEntity>(context);
            MDRRepository = new MissingDataRecordRepository(context);
            Type = typeof(TEntity).Name;
            Logger = LogManager.GetLogger("Sync");
        }

        #region 私有方法
        /// <summary>
        /// 获取时间
        /// </summary>
        /// <returns></returns>
        protected abstract DateTime GetTime();

        /// <summary>
        /// 判断是否已同步
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="conditions">条件</param>
        /// <returns></returns>
        protected abstract bool IsSynchronized(string code, DateTime time);

        /// <summary>
        /// 判断是否已同步
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        protected abstract bool IsSynchronized(DateTime time);

        /// <summary>
        /// 获取时间点对应的Codes
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        protected abstract string[] GetCodes(DateTime time);

        /// <summary>
        /// 同步数据
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="time">时间</param>
        protected virtual void Sync(string code, DateTime time)
        {
            try
            {
                Repository.Add(GetSyncData(code, time));
            }
            catch (Exception e)
            {
                MDRRepository.Add(Type, code, time, e);
            }
        }

        /// <summary>
        /// 获取数据（同步）
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        protected abstract IEnumerable<TEntity> GetSyncData(string code, DateTime time);

        /// <summary>
        /// 获取数据（回补）
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        protected virtual IEnumerable<TEntity> GetCoverData(string code, DateTime time)
        {
            return GetSyncData(code, time);
        }

        /// <summary>
        /// 获取回补记录
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<MissingDataRecord> GetRecords()
        {
            return MDRRepository.GetList(Type);
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 同步数据
        /// </summary>
        public virtual void Sync()
        {
            try
            {
                DateTime time = GetTime();
                ReturnStatus rs = Sync(time);
                if (rs.Exception == null)
                {
                    Logger.Info(rs.Message);
                }
                else
                {
                    Logger.Error(rs.Message, rs.Exception);
                }
            }
            catch (Exception e)
            {
                Logger.Error("同步失败！", e);
            }
        }

        /// <summary>
        /// 同步数据
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public virtual ReturnStatus Sync(DateTime time)
        {
            ReturnStatus rs;
            try
            {
                if (!IsSynchronized(time))
                {
                    string[] codes = GetCodes(time);
                    foreach (string code in codes)
                    {
                        Sync(code, time);
                    }
                }
                rs = new ReturnStatus(true, "同步成功！");
            }
            catch (Exception e)
            {
                rs = new ReturnStatus("同步失败！", e);
            }
            return rs;
        }

        /// <summary>
        /// 回补数据
        /// </summary>
        public virtual void Cover()
        {

        }

        /// <summary>
        /// 回补数据
        /// </summary>
        /// <param name="records">回补记录</param>
        public virtual void Cover(IEnumerable<MissingDataRecord> records)
        {
            try
            {
                foreach (MissingDataRecord record in records)
                {
                    Cover(record);
                }
            }
            catch (Exception e)
            {

            }
        }
        #endregion
    }
}
