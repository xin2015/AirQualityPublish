using AirQualityPublish.BLL.Models;
using AirQualityPublish.BLL.Services;
using AirQualityPublish.BLL.Syncs;
using AirQualityPublish.Model;
using AirQualityPublish.Model.Entities;
using AirQualityPublish.Model.Repositories;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.OpenAccess;

namespace AirQualityPublish.WindowsForms
{
    public partial class AirQualityPublishForm : Form
    {
        DateTime beginTime;
        DateTime endTime;
        ILog logger;

        public AirQualityPublishForm()
        {
            InitializeComponent();
            TypeComboBox.Items.AddRange(MissingDataRecordService.SyncTypeDic.Select(o => o.Key).ToArray());
            logger = LogManager.GetLogger<AirQualityPublishForm>();
        }

        private void GetTime()
        {
            if (!DateTime.TryParse(BeginTimeTextBox.Text, out beginTime))
            {
                beginTime = DateTime.Today;
            }
            if (!DateTime.TryParse(EndTimeTextBox.Text, out endTime))
            {
                endTime = beginTime;
            }
        }

        private void Sync(Func<OpenAccessContext, string, string, DateTime, DateTime, ReturnStatus> func)
        {
            try
            {
                string type = TypeComboBox.SelectedItem as string;
                string code = CodeTextBox.Text;
                GetTime();
                using (FluentModel db = new FluentModel())
                {
                    ReturnStatus rs = func(db, type, code, beginTime, endTime);
                    if (!rs.Status)
                    {
                        logger.Error(rs.Message, rs.Exception);
                    }
                    ResultTextBox.Text = string.Format("{0}：{1}", rs.Message, DateTime.Now);
                }
            }
            catch (Exception e)
            {
                logger.Error("Sync failed.", e);
                ResultTextBox.Text = string.Format("{0}：{1}", e.Message, DateTime.Now);
            }
        }

        private void SyncButton_Click(object sender, EventArgs e)
        {
            Func<OpenAccessContext, string, string, DateTime, DateTime, ReturnStatus> func = (db, type, code, beginTime, endTime) =>
                 {
                     ReturnStatus rs;
                     ISync sync = (ISync)Activator.CreateInstance(MissingDataRecordService.SyncTypeDic[type], db);
                     if (string.IsNullOrEmpty(code))
                     {
                         rs = sync.Sync(beginTime, endTime);
                     }
                     else
                     {
                         rs = sync.Sync(code, beginTime, endTime);
                     }
                     return rs;
                 };
            Sync(func);
        }

        private void CoverButton_Click(object sender, EventArgs e)
        {
            Func<OpenAccessContext, string, string, DateTime, DateTime, ReturnStatus> func = (db, type, code, beginTime, endTime) =>
            {
                ReturnStatus rs;
                ISync sync = (ISync)Activator.CreateInstance(MissingDataRecordService.SyncTypeDic[type], db);
                if (string.IsNullOrEmpty(code))
                {
                    rs = sync.Cover(beginTime, endTime);
                }
                else
                {
                    rs = sync.Cover(code, beginTime, endTime);
                }
                return rs;
            };
            Sync(func);
        }

        private void UpdateSchemaButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (FluentModel db = new FluentModel())
                {
                    db.UpdateSchema();
                }
                ResultTextBox.Text = string.Format("{0}：{1}", "数据库更新成功！", DateTime.Now);
            }
            catch (Exception ex)
            {
                logger.Error("数据库更新失败！", ex);
                ResultTextBox.Text = string.Format("{0}：{1}", ex.Message, DateTime.Now);
            }
        }

        private void InitDatabaseButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (FluentModel db = new FluentModel())
                {
                    StationRepository repository = new StationRepository(db);
                    if (repository.GetAll().Count() == 0)
                    {
                        repository.Add(new Station()
                        {
                            Code = "430700052",
                            Name = "市监测站",
                            EnvPublishCode = "1853A",
                            Latitude = 29.0244,
                            Longitude = 111.7044,
                            Status = true,
                            Order = 1,
                            IsContrast = false
                        });
                        repository.Add(new Station()
                        {
                            Code = "430700054",
                            Name = "市二中",
                            EnvPublishCode = "1854A",
                            Latitude = 28.9703,
                            Longitude = 111.6975,
                            Status = true,
                            Order = 2,
                            IsContrast = false
                        });
                        repository.Add(new Station()
                        {
                            Code = "430700051",
                            Name = "鼎城区环保局",
                            EnvPublishCode = "1855A",
                            Latitude = 29.0239,
                            Longitude = 111.6753,
                            Status = true,
                            Order = 3,
                            IsContrast = false
                        });
                        repository.Add(new Station()
                        {
                            Code = "430700053",
                            Name = "市技术监督局",
                            EnvPublishCode = "1856A",
                            Latitude = 29.0381,
                            Longitude = 111.6794,
                            Status = true,
                            Order = 4,
                            IsContrast = false
                        });
                        repository.Add(new Station()
                        {
                            Code = "430700049",
                            Name = "白鹤山",
                            EnvPublishCode = "1857A",
                            Latitude = 29.1456,
                            Longitude = 111.7158,
                            Status = true,
                            Order = 5,
                            IsContrast = true
                        });
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("数据库初始化失败！", ex);
                ResultTextBox.Text = string.Format("{0}：{1}", ex.Message, DateTime.Now);
            }
        }
    }
}
