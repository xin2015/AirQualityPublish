using AirQualityPublish.BLL.Services;
using AirQualityPublish.BLL.Syncs;
using AirQualityPublish.Model;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private void SyncButton_Click(object sender, EventArgs e)
        {
            string type = TypeComboBox.SelectedItem as string;
            using (FluentModel db = new FluentModel())
            {
                ISync sync = (ISync)Activator.CreateInstance(MissingDataRecordService.SyncTypeDic[type], db);
                sync.Sync();
            }
        }

        private void CoverButton_Click(object sender, EventArgs e)
        {

        }

        private void UpdateSchemaButton_Click(object sender, EventArgs e)
        {

        }

        private void InitDatabaseButton_Click(object sender, EventArgs e)
        {

        }
    }
}
