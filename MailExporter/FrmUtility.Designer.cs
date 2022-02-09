namespace MailExporter
{
    partial class FrmUtility
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Telerik.WinControls.UI.GridViewCheckBoxColumn gridViewCheckBoxColumn1 = new Telerik.WinControls.UI.GridViewCheckBoxColumn();
            Telerik.WinControls.UI.GridViewDecimalColumn gridViewDecimalColumn1 = new Telerik.WinControls.UI.GridViewDecimalColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewDateTimeColumn gridViewDateTimeColumn1 = new Telerik.WinControls.UI.GridViewDateTimeColumn();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor1 = new Telerik.WinControls.Data.SortDescriptor();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.viewMain = new Telerik.WinControls.UI.RadGridView();
            this.btnSearch = new Telerik.WinControls.UI.RadButton();
            this.txtKeywords = new Telerik.WinControls.UI.RadTextBoxControl();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.btnReturn = new Telerik.WinControls.UI.RadButton();
            this.btnDelete = new Telerik.WinControls.UI.RadButton();
            this.lblCount = new Telerik.WinControls.UI.RadLabel();
            this.dataMailItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.viewMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewMain.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKeywords)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnReturn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataMailItemBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Controls.Add(this.viewMain);
            this.radGroupBox1.Controls.Add(this.btnSearch);
            this.radGroupBox1.Controls.Add(this.txtKeywords);
            this.radGroupBox1.Controls.Add(this.radLabel1);
            this.radGroupBox1.HeaderText = "";
            this.radGroupBox1.HeaderTextAlignment = System.Drawing.ContentAlignment.TopCenter;
            this.radGroupBox1.Location = new System.Drawing.Point(3, 2);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Size = new System.Drawing.Size(600, 372);
            this.radGroupBox1.TabIndex = 1;
            this.radGroupBox1.ThemeName = "ControlDefault";
            // 
            // viewMain
            // 
            this.viewMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.viewMain.Cursor = System.Windows.Forms.Cursors.Default;
            this.viewMain.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.viewMain.ForeColor = System.Drawing.Color.Black;
            this.viewMain.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.viewMain.Location = new System.Drawing.Point(14, 39);
            // 
            // 
            // 
            this.viewMain.MasterTemplate.AllowAddNewRow = false;
            this.viewMain.MasterTemplate.AllowCellContextMenu = false;
            this.viewMain.MasterTemplate.AllowColumnChooser = false;
            this.viewMain.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.viewMain.MasterTemplate.AllowColumnReorder = false;
            this.viewMain.MasterTemplate.AllowDeleteRow = false;
            this.viewMain.MasterTemplate.AllowDragToGroup = false;
            this.viewMain.MasterTemplate.AllowRowResize = false;
            gridViewCheckBoxColumn1.AllowGroup = false;
            gridViewCheckBoxColumn1.AllowResize = false;
            gridViewCheckBoxColumn1.AllowSort = false;
            gridViewCheckBoxColumn1.EditMode = Telerik.WinControls.UI.EditMode.OnValueChange;
            gridViewCheckBoxColumn1.EnableExpressionEditor = false;
            gridViewCheckBoxColumn1.EnableHeaderCheckBox = true;
            gridViewCheckBoxColumn1.FieldName = "IsSelected";
            gridViewCheckBoxColumn1.HeaderText = "选择";
            gridViewCheckBoxColumn1.MinWidth = 20;
            gridViewCheckBoxColumn1.Name = "IsSelected";
            gridViewCheckBoxColumn1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewCheckBoxColumn1.Width = 60;
            gridViewDecimalColumn1.AllowGroup = false;
            gridViewDecimalColumn1.DataType = typeof(uint);
            gridViewDecimalColumn1.EnableExpressionEditor = false;
            gridViewDecimalColumn1.FieldName = "No";
            gridViewDecimalColumn1.HeaderText = "邮件Id";
            gridViewDecimalColumn1.Name = "No";
            gridViewDecimalColumn1.ReadOnly = true;
            gridViewDecimalColumn1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewDecimalColumn1.Width = 80;
            gridViewTextBoxColumn1.AllowGroup = false;
            gridViewTextBoxColumn1.EnableExpressionEditor = false;
            gridViewTextBoxColumn1.FieldName = "Subject";
            gridViewTextBoxColumn1.HeaderText = "标题";
            gridViewTextBoxColumn1.Name = "Subject";
            gridViewTextBoxColumn1.ReadOnly = true;
            gridViewTextBoxColumn1.SortOrder = Telerik.WinControls.UI.RadSortOrder.Ascending;
            gridViewTextBoxColumn1.Width = 300;
            gridViewDateTimeColumn1.AllowGroup = false;
            gridViewDateTimeColumn1.DataType = typeof(System.Nullable<System.DateTime>);
            gridViewDateTimeColumn1.EnableExpressionEditor = false;
            gridViewDateTimeColumn1.FieldName = "SendTime";
            gridViewDateTimeColumn1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            gridViewDateTimeColumn1.FormatString = "{0:yyyy-MM-dd}";
            gridViewDateTimeColumn1.HeaderText = "发送日期";
            gridViewDateTimeColumn1.IsAutoGenerated = true;
            gridViewDateTimeColumn1.Name = "SendTime";
            gridViewDateTimeColumn1.Width = 80;
            this.viewMain.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewCheckBoxColumn1,
            gridViewDecimalColumn1,
            gridViewTextBoxColumn1,
            gridViewDateTimeColumn1});
            this.viewMain.MasterTemplate.DataSource = this.dataMailItemBindingSource;
            sortDescriptor1.PropertyName = "Subject";
            this.viewMain.MasterTemplate.SortDescriptors.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor1});
            this.viewMain.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.viewMain.Name = "viewMain";
            this.viewMain.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.viewMain.Size = new System.Drawing.Size(570, 328);
            this.viewMain.TabIndex = 8;
            this.viewMain.ValueChanged += new System.EventHandler(this.viewMain_ValueChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(314, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(69, 24);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "搜索";
            this.btnSearch.ThemeName = "ControlDefault";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtKeywords
            // 
            this.txtKeywords.Location = new System.Drawing.Point(74, 10);
            this.txtKeywords.Name = "txtKeywords";
            this.txtKeywords.Size = new System.Drawing.Size(234, 22);
            this.txtKeywords.TabIndex = 1;
            this.txtKeywords.ThemeName = "ControlDefault";
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(14, 14);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(42, 18);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "关键词";
            this.radLabel1.ThemeName = "ControlDefault";
            // 
            // btnReturn
            // 
            this.btnReturn.Location = new System.Drawing.Point(488, 385);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(110, 24);
            this.btnReturn.TabIndex = 3;
            this.btnReturn.Text = "返回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(12, 385);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(110, 24);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "删除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblCount
            // 
            this.lblCount.Location = new System.Drawing.Point(164, 391);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(42, 18);
            this.lblCount.TabIndex = 4;
            this.lblCount.Text = "关键词";
            this.lblCount.ThemeName = "ControlDefault";
            // 
            // dataMailItemBindingSource
            // 
            this.dataMailItemBindingSource.DataSource = typeof(MailExporter.DataMailItem);
            // 
            // FrmUtility
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 421);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.radGroupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmUtility";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "邮箱工具";
            this.ThemeName = "ControlDefault";
            this.Load += new System.EventHandler(this.FrmUtility_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            this.radGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.viewMain.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKeywords)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnReturn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataMailItemBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private Telerik.WinControls.UI.RadTextBoxControl txtKeywords;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadButton btnSearch;
        private Telerik.WinControls.UI.RadButton btnReturn;
        private Telerik.WinControls.UI.RadButton btnDelete;
        private Telerik.WinControls.UI.RadGridView viewMain;
        private System.Windows.Forms.BindingSource dataMailItemBindingSource;
        private Telerik.WinControls.UI.RadLabel lblCount;
    }
}
