using AirQualityPublish.BLL.Models;
using AirQualityPublish.BLL.Syncs;
using AirQualityPublish.Model.Entities;
using AirQualityPublish.Model.Repositories;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telerik.OpenAccess;

namespace AirQualityPublish.BLL.Services
{
    public class MissingDataRecordService
    {
        public static Dictionary<string, Type> SyncTypeDic { get; private set; }
        public static Dictionary<string, Type> RepositoryTypeDic { get; private set; }
        static MissingDataRecordService()
        {
            Type[] typesBLL = Assembly.GetExecutingAssembly().GetTypes();
            Type[] typesDAL = Assembly.Load("AirQualityPublish.Model").GetTypes();
            IEnumerable<Type> syncTypes = typesBLL.Where(o => o.Name.EndsWith("Sync") && o.GetInterfaces().Contains(typeof(ISync)));
            SyncTypeDic = new Dictionary<string, Type>();
            RepositoryTypeDic = new Dictionary<string, Type>();
            Type repositoryBaseType = typeof(CodeTimeRepository<>);
            foreach (Type type in syncTypes)
            {
                string code = type.Name.Replace("Sync", string.Empty);
                SyncTypeDic.Add(code, type);
                Type entityType = typesDAL.FirstOrDefault(o => o.Name == code);
                RepositoryTypeDic.Add(code, repositoryBaseType.MakeGenericType(entityType));
            }
        }

        OpenAccessContext db;
        ILog logger;

        MissingDataRecordRepository Repository { get; set; }

        public MissingDataRecordService(OpenAccessContext context)
        {
            db = context;
            Repository = new MissingDataRecordRepository(db);
            logger = LogManager.GetLogger<MissingDataRecordService>();
        }

        private void Cover(List<MissingDataRecord> list)
        {
            foreach (var codeGroup in list.GroupBy(o => o.Code))
            {
                if (SyncTypeDic.ContainsKey(codeGroup.Key))
                {
                    ISync sync = (ISync)Activator.CreateInstance(SyncTypeDic[codeGroup.Key], db);
                    sync.Cover(codeGroup.ToList());
                }
            }
        }

        public ReturnStatus Cover(int[] ids)
        {
            ReturnStatus rs;
            try
            {
                List<MissingDataRecord> list = Repository.GetList(ids).ToList();
                Cover(list);
                rs = new ReturnStatus(true, "回补成功！");
            }
            catch (Exception e)
            {
                rs = new ReturnStatus(false, e.Message);
                logger.Error("Cover failed.", e);
            }
            return rs;
        }
    }
}
