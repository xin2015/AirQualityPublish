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
        ReturnStatus Cover(List<MissingDataRecord> list);

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
        protected CodeTimeRepository<TEntity> Repository { get; set; }
        protected MissingDataRecordRepository MDRRepository { get; set; }
        protected string Type { get; set; }
        protected TimeSpan Interval { get; set; }

        public SyncBase(OpenAccessContext context)
        {
            DB = context;
            Repository = new CodeTimeRepository<TEntity>(context);
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
        protected virtual bool IsSynchronized(string code, DateTime time)
        {
            return Repository.Contains(code, time);
        }

        /// <summary>
        /// 判断是否已同步
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        protected virtual bool IsSynchronized(DateTime time)
        {
            return Repository.Contains(time);
        }

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
        /// 同步数据
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        protected virtual List<bool> Sync(DateTime time)
        {
            List<bool> list = new List<bool>();
            if (!IsSynchronized(time))
            {
                string[] codes = GetCodes(time);
                foreach (string code in codes)
                {
                    list.Add(Sync(code, time));
                }
                DB.SaveChanges();
            }
            return list;
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
        /// 获取回补记录
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        protected virtual MissingDataRecord GetRecord(string code, DateTime time)
        {
            return MDRRepository.Get(Type, code, time);
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

        /// <summary>
        /// 回补数据
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        protected virtual bool Cover(string code, DateTime time)
        {
            bool result;
            MissingDataRecord mdr = GetRecord(code, time);
            if (mdr == null)
            {
                result = Sync(code, time);
            }
            else
            {
                result = Cover(mdr);
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
                List<bool> list = Sync(time);
                Logger.Info(string.Format("同步成功！成功{0}个，失败{1}个。", list.Count(o => o), list.Count(o => !o)));
            }
            catch (Exception e)
            {
                Logger.Error("同步失败！", e);
            }
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
                DB.SaveChanges();
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
                List<bool> list = new List<bool>();
                for (DateTime time = beginTime; time <= endTime; time = time.Add(Interval))
                {
                    list.AddRange(Sync(time));
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
        /// 回补数据
        /// </summary>
        public virtual void Cover()
        {
            try
            {
                List<MissingDataRecord> records = GetRecords();
                ReturnStatus rs = Cover(records);
                if (rs.Status)
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
                Logger.Error("回补失败！", e);
            }
        }

        /// <summary>
        /// 回补数据
        /// </summary>
        /// <param name="records">回补记录</param>
        public virtual ReturnStatus Cover(List<MissingDataRecord> records)
        {
            ReturnStatus rs;
            try
            {
                List<bool> list = new List<bool>();
                records.ForEach(o => list.Add(Cover(o)));
                DB.SaveChanges();
                rs = new ReturnStatus(true, string.Format("回补成功！成功{0}个，失败{1}个。", list.Count(o => o), list.Count(o => !o)));
            }
            catch (Exception e)
            {
                rs = new ReturnStatus("回补失败！", e);
            }
            return rs;
        }

        /// <summary>
        /// 回补数据
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public virtual ReturnStatus Cover(string code, DateTime beginTime, DateTime endTime)
        {
            ReturnStatus rs;
            try
            {
                List<bool> list = new List<bool>();
                for (DateTime time = beginTime; time <= endTime; time = time.Add(Interval))
                {
                    if (!IsSynchronized(code, time))
                    {
                        list.Add(Cover(code, time));
                    }
                }
                DB.SaveChanges();
                rs = new ReturnStatus(true, string.Format("回补成功！成功{0}个，失败{1}个。", list.Count(o => o), list.Count(o => !o)));
            }
            catch (Exception e)
            {
                rs = new ReturnStatus("回补失败！", e);
            }
            return rs;
        }

        /// <summary>
        /// 回补数据
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public ReturnStatus Cover(DateTime beginTime, DateTime endTime)
        {
            ReturnStatus rs;
            try
            {
                List<bool> list = new List<bool>();
                for (DateTime time = beginTime; time <= endTime; time = time.Add(Interval))
                {
                    string[] codes = GetCodes(time);
                    foreach (string code in codes)
                    {
                        if (!IsSynchronized(code, time))
                        {
                            list.Add(Cover(code, time));
                        }
                    }
                    DB.SaveChanges();
                }
                rs = new ReturnStatus(true, string.Format("回补成功！成功{0}个，失败{1}个。", list.Count(o => o), list.Count(o => !o)));
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
