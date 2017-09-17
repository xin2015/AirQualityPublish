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
        /// <param name="code">编码</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        ReturnStatus Sync(string code, DateTime beginTime, DateTime endTime);

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
        void Cover(List<MissingDataRecord> list);

        /// <summary>
        /// 回补数据
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        ReturnStatus Cover(string code, DateTime beginTime, DateTime endTime);

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
        protected OpenAccessContext DB { get; set; }
        protected CodeTimeEntityRepository<TEntity> Repository { get; set; }
        protected MissingDataRecordRepository MDRRepository { get; set; }
        protected string Type { get; set; }
        protected TimeSpan Interval { get; set; }

        public SyncBase(OpenAccessContext context)
        {
            DB = context;
            Repository = new CodeTimeEntityRepository<TEntity>(context);
            MDRRepository = new MissingDataRecordRepository(context);
            Type = typeof(TEntity).Name;
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
        protected virtual bool Sync(string code, DateTime time)
        {
            bool result;
            try
            {
                List<TEntity> list = GetSyncData(code, time);
                if (list == null || list.Count == 0)
                {
                    MDRRepository.Add(Type, code, time, "数据获取失败！");
                    result = false;
                }
                else
                {
                    Repository.Add(GetSyncData(code, time));
                    result = true;
                }
            }
            catch (Exception e)
            {
                MDRRepository.Add(Type, code, time, e.Message);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 获取数据（同步）
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        protected abstract List<TEntity> GetSyncData(string code, DateTime time);

        /// <summary>
        /// 获取数据（回补）
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        protected virtual List<TEntity> GetCoverData(string code, DateTime time)
        {
            return GetSyncData(code, time);
        }

        /// <summary>
        /// 获取回补记录
        /// </summary>
        /// <returns></returns>
        protected virtual List<MissingDataRecord> GetRecords()
        {
            return MDRRepository.GetList(Type).ToList();
        }

        /// <summary>
        /// 数据回补
        /// </summary>
        /// <param name="mdr">数据回补记录</param>
        /// <returns></returns>
        protected virtual bool Cover(MissingDataRecord mdr)
        {
            bool result;
            try
            {
                List<TEntity> list = GetCoverData(mdr.Code, mdr.Time);
                if (list == null || list.Count == 0)
                {
                    MDRRepository.Update(mdr, false, "数据获取失败！");
                    result = false;
                }
                else
                {
                    Repository.Add(list);
                    MDRRepository.Update(mdr, true);
                    result = true;
                }
            }
            catch (Exception e)
            {
                MDRRepository.Update(mdr, false, e.Message);
                result = false;
            }
            return result;
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
                List<bool> list = new List<bool>();
                if (!IsSynchronized(time))
                {
                    string[] codes = GetCodes(time);
                    foreach (string code in codes)
                    {
                        list.Add(Sync(code, time));
                    }
                }
                rs = new ReturnStatus(true, string.Format("同步成功！成功{0}个，失败{1}个。", list.Count(o => o), list.Count(o => !o)));
            }
            catch (Exception e)
            {
                rs = new ReturnStatus("同步失败！", e);
            }
            return rs;
        }

        /// <summary>
        /// 同步数据
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public virtual ReturnStatus Sync(string code, DateTime beginTime, DateTime endTime)
        {
            ReturnStatus rs;
            try
            {
                List<bool> list = new List<bool>();
                for (DateTime time = beginTime; time <= endTime; time = time.Add(Interval))
                {
                    if (!IsSynchronized(code, time))
                    {
                        list.Add(Sync(code, time));
                    }
                }
                rs = new ReturnStatus(true, string.Format("同步成功！成功{0}个，失败{1}个。", list.Count(o => o), list.Count(o => !o)));
            }
            catch (Exception e)
            {
                rs = new ReturnStatus("同步失败！", e);
            }
            return rs;
        }

        /// <summary>
        /// 同步数据
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public virtual ReturnStatus Sync(DateTime beginTime, DateTime endTime)
        {
            ReturnStatus rs;
            try
            {
                for (DateTime time = beginTime; time <= endTime; time = time.Add(Interval))
                {
                    if (!IsSynchronized(time))
                    {
                        string[] codes = GetCodes(time);
                        foreach (string code in codes)
                        {
                            Sync(code, time);
                        }
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
            try
            {
                IEnumerable<MissingDataRecord> records = GetRecords();
                ReturnStatus rs = Cover(records);
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

                throw;
            }
        }

        /// <summary>
        /// 回补数据
        /// </summary>
        /// <param name="records">回补记录</param>
        public virtual ReturnStatus Cover(IEnumerable<MissingDataRecord> records)
        {
            ReturnStatus rs;
            try
            {
                foreach (MissingDataRecord record in records)
                {
                    Cover(record);
                }
                rs = new ReturnStatus(true, "回补成功！");
            }
            catch (Exception e)
            {
                rs = new ReturnStatus("回补失败！", e);
            }
            return rs;
        }
        #endregion
    }
}
