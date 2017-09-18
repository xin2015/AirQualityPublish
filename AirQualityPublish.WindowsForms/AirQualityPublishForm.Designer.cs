namespace AirQualityPublish.WindowsForms
{
    partial class AirQualityPublishForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.TypeLabel = new System.Windows.Forms.Label();
            this.TypeComboBox = new System.Windows.Forms.ComboBox();
            this.BeginTimeLabel = new System.Windows.Forms.Label();
            this.BeginTimeTextBox = new System.Windows.Forms.TextBox();
            this.EndTimeTextBox = new System.Windows.Forms.TextBox();
            this.EndTimeLabel = new System.Windows.Forms.Label();
            this.ResultLabel = new System.Windows.Forms.Label();
            this.ResultTextBox = new System.Windows.Forms.TextBox();
            this.SyncButton = new System.Windows.Forms.Button();
            this.CoverButton = new System.Windows.Forms.Button();
            this.UpdateSchemaButton = new System.Windows.Forms.Button();
            this.CodeTextBox = new System.Windows.Forms.TextBox();
            this.CodeLabel = new System.Windows.Forms.Label();
            this.InitDatabaseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TypeLabel
            // 
            this.TypeLabel.AutoSize = true;
            this.TypeLabel.Location = new System.Drawing.Point(12, 9);
            this.TypeLabel.Name = "TypeLabel";
            this.TypeLabel.Size = new System.Drawing.Size(41, 12);
            this.TypeLabel.TabIndex = 0;
            this.TypeLabel.Text = "Type：";
            // 
            // TypeComboBox
            // 
            this.TypeComboBox.FormattingEnabled = true;
            this.TypeComboBox.Location = new System.Drawing.Point(59, 6);
            this.TypeComboBox.Name = "TypeComboBox";
            this.TypeComboBox.Size = new System.Drawing.Size(213, 20);
            this.TypeComboBox.TabIndex = 1;
            // 
            // BeginTimeLabel
            // 
            this.BeginTimeLabel.AutoSize = true;
            this.BeginTimeLabel.Location = new System.Drawing.Point(12, 62);
            this.BeginTimeLabel.Name = "BeginTimeLabel";
            this.BeginTimeLabel.Size = new System.Drawing.Size(65, 12);
            this.BeginTimeLabel.TabIndex = 2;
            this.BeginTimeLabel.Text = "开始时间：";
            // 
            // BeginTimeTextBox
            // 
            this.BeginTimeTextBox.Location = new System.Drawing.Point(83, 59);
            this.BeginTimeTextBox.Name = "BeginTimeTextBox";
            this.BeginTimeTextBox.Size = new System.Drawing.Size(189, 21);
            this.BeginTimeTextBox.TabIndex = 3;
            // 
            // EndTimeTextBox
            // 
            this.EndTimeTextBox.Location = new System.Drawing.Point(83, 86);
            this.EndTimeTextBox.Name = "EndTimeTextBox";
            this.EndTimeTextBox.Size = new System.Drawing.Size(189, 21);
            this.EndTimeTextBox.TabIndex = 4;
            // 
            // EndTimeLabel
            // 
            this.EndTimeLabel.AutoSize = true;
            this.EndTimeLabel.Location = new System.Drawing.Point(12, 89);
            this.EndTimeLabel.Name = "EndTimeLabel";
            this.EndTimeLabel.Size = new System.Drawing.Size(65, 12);
            this.EndTimeLabel.TabIndex = 5;
            this.EndTimeLabel.Text = "结束时间：";
            // 
            // ResultLabel
            // 
            this.ResultLabel.AutoSize = true;
            this.ResultLabel.Location = new System.Drawing.Point(12, 231);
            this.ResultLabel.Name = "ResultLabel";
            this.ResultLabel.Size = new System.Drawing.Size(41, 12);
            this.ResultLabel.TabIndex = 6;
            this.ResultLabel.Text = "结果：";
            // 
            // ResultTextBox
            // 
            this.ResultTextBox.Location = new System.Drawing.Point(59, 228);
            this.ResultTextBox.Name = "ResultTextBox";
            this.ResultTextBox.Size = new System.Drawing.Size(213, 21);
            this.ResultTextBox.TabIndex = 7;
            // 
            // SyncButton
            // 
            this.SyncButton.Location = new System.Drawing.Point(61, 113);
            this.SyncButton.Name = "SyncButton";
            this.SyncButton.Size = new System.Drawing.Size(75, 23);
            this.SyncButton.TabIndex = 8;
            this.SyncButton.Text = "同步数据";
            this.SyncButton.UseVisualStyleBackColor = true;
            this.SyncButton.Click += new System.EventHandler(this.SyncButton_Click);
            // 
            // CoverButton
            // 
            this.CoverButton.Location = new System.Drawing.Point(154, 113);
            this.CoverButton.Name = "CoverButton";
            this.CoverButton.Size = new System.Drawing.Size(75, 23);
            this.CoverButton.TabIndex = 9;
            this.CoverButton.Text = "回补数据";
            this.CoverButton.UseVisualStyleBackColor = true;
            this.CoverButton.Click += new System.EventHandler(this.CoverButton_Click);
            // 
            // UpdateSchemaButton
            // 
            this.UpdateSchemaButton.Location = new System.Drawing.Point(49, 142);
            this.UpdateSchemaButton.Name = "UpdateSchemaButton";
            this.UpdateSchemaButton.Size = new System.Drawing.Size(87, 23);
            this.UpdateSchemaButton.TabIndex = 10;
            this.UpdateSchemaButton.Text = "更新数据库";
            this.UpdateSchemaButton.UseVisualStyleBackColor = true;
            this.UpdateSchemaButton.Click += new System.EventHandler(this.UpdateSchemaButton_Click);
            // 
            // CodeTextBox
            // 
            this.CodeTextBox.Location = new System.Drawing.Point(83, 32);
            this.CodeTextBox.Name = "CodeTextBox";
            this.CodeTextBox.Size = new System.Drawing.Size(189, 21);
            this.CodeTextBox.TabIndex = 12;
            // 
            // CodeLabel
            // 
            this.CodeLabel.AutoSize = true;
            this.CodeLabel.Location = new System.Drawing.Point(12, 35);
            this.CodeLabel.Name = "CodeLabel";
            this.CodeLabel.Size = new System.Drawing.Size(41, 12);
            this.CodeLabel.TabIndex = 11;
            this.CodeLabel.Text = "Code：";
            // 
            // InitDatabaseButton
            // 
            this.InitDatabaseButton.Location = new System.Drawing.Point(154, 142);
            this.InitDatabaseButton.Name = "InitDatabaseButton";
            this.InitDatabaseButton.Size = new System.Drawing.Size(87, 23);
            this.InitDatabaseButton.TabIndex = 13;
            this.InitDatabaseButton.Text = "初始化数据库";
            this.InitDatabaseButton.UseVisualStyleBackColor = true;
            this.InitDatabaseButton.Click += new System.EventHandler(this.InitDatabaseButton_Click);
            // 
            // AirQualityPublishForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.InitDatabaseButton);
            this.Controls.Add(this.CodeTextBox);
            this.Controls.Add(this.CodeLabel);
            this.Controls.Add(this.UpdateSchemaButton);
            this.Controls.Add(this.CoverButton);
            this.Controls.Add(this.SyncButton);
            this.Controls.Add(this.ResultTextBox);
            this.Controls.Add(this.ResultLabel);
            this.Controls.Add(this.EndTimeLabel);
            this.Controls.Add(this.EndTimeTextBox);
            this.Controls.Add(this.BeginTimeTextBox);
            this.Controls.Add(this.BeginTimeLabel);
            this.Controls.Add(this.TypeComboBox);
            this.Controls.Add(this.TypeLabel);
            this.Name = "AirQualityPublishForm";
            this.Text = "AirQualityPublishForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TypeLabel;
        private System.Windows.Forms.ComboBox TypeComboBox;
        private System.Windows.Forms.Label BeginTimeLabel;
        private System.Windows.Forms.TextBox BeginTimeTextBox;
        private System.Windows.Forms.TextBox EndTimeTextBox;
        private System.Windows.Forms.Label EndTimeLabel;
        private System.Windows.Forms.Label ResultLabel;
        private System.Windows.Forms.TextBox ResultTextBox;
        private System.Windows.Forms.Button SyncButton;
        private System.Windows.Forms.Button CoverButton;
        private System.Windows.Forms.Button UpdateSchemaButton;
        private System.Windows.Forms.TextBox CodeTextBox;
        private System.Windows.Forms.Label CodeLabel;
        private System.Windows.Forms.Button InitDatabaseButton;
    }
}

