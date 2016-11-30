namespace WLMFolderMover {
    partial class WForm {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WForm));
            this.vsc = new System.Windows.Forms.SplitContainer();
            this.tvF = new System.Windows.Forms.TreeView();
            this.il16 = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.lvM = new System.Windows.Forms.ListView();
            this.chTTL = new System.Windows.Forms.ColumnHeader();
            this.chDate = new System.Windows.Forms.ColumnHeader();
            this.chFrom = new System.Windows.Forms.ColumnHeader();
            this.chUidl = new System.Windows.Forms.ColumnHeader();
            this.label2 = new System.Windows.Forms.Label();
            this.ofdDb = new System.Windows.Forms.OpenFileDialog();
            this.cmsMails = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mFilterLost = new System.Windows.Forms.ToolStripMenuItem();
            this.chSt = new System.Windows.Forms.ColumnHeader();
            this.mDeleteUidl = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mSelDup = new System.Windows.Forms.ToolStripMenuItem();
            this.chId = new System.Windows.Forms.ColumnHeader();
            this.vsc.Panel1.SuspendLayout();
            this.vsc.Panel2.SuspendLayout();
            this.vsc.SuspendLayout();
            this.cmsMails.SuspendLayout();
            this.SuspendLayout();
            // 
            // vsc
            // 
            this.vsc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vsc.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.vsc.Location = new System.Drawing.Point(0, 0);
            this.vsc.Name = "vsc";
            // 
            // vsc.Panel1
            // 
            this.vsc.Panel1.Controls.Add(this.tvF);
            this.vsc.Panel1.Controls.Add(this.label1);
            // 
            // vsc.Panel2
            // 
            this.vsc.Panel2.Controls.Add(this.lvM);
            this.vsc.Panel2.Controls.Add(this.label2);
            this.vsc.Size = new System.Drawing.Size(993, 332);
            this.vsc.SplitterDistance = 214;
            this.vsc.SplitterWidth = 6;
            this.vsc.TabIndex = 0;
            // 
            // tvF
            // 
            this.tvF.AllowDrop = true;
            this.tvF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvF.ImageIndex = 0;
            this.tvF.ImageList = this.il16;
            this.tvF.Location = new System.Drawing.Point(0, 12);
            this.tvF.Name = "tvF";
            this.tvF.SelectedImageIndex = 0;
            this.tvF.Size = new System.Drawing.Size(214, 320);
            this.tvF.TabIndex = 1;
            this.tvF.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvF_DragDrop);
            this.tvF.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvF_AfterSelect);
            this.tvF.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvF_DragEnter);
            this.tvF.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvF_ItemDrag);
            this.tvF.DragOver += new System.Windows.Forms.DragEventHandler(this.tvF_DragEnter);
            // 
            // il16
            // 
            this.il16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("il16.ImageStream")));
            this.il16.TransparentColor = System.Drawing.Color.Transparent;
            this.il16.Images.SetKeyName(0, "Folder.ico");
            this.il16.Images.SetKeyName(1, "8");
            this.il16.Images.SetKeyName(2, "M");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "フォルダ構造：";
            // 
            // lvM
            // 
            this.lvM.AllowColumnReorder = true;
            this.lvM.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chTTL,
            this.chDate,
            this.chFrom,
            this.chUidl,
            this.chSt,
            this.chId});
            this.lvM.ContextMenuStrip = this.cmsMails;
            this.lvM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvM.FullRowSelect = true;
            this.lvM.GridLines = true;
            this.lvM.Location = new System.Drawing.Point(0, 12);
            this.lvM.Name = "lvM";
            this.lvM.Size = new System.Drawing.Size(773, 320);
            this.lvM.SmallImageList = this.il16;
            this.lvM.TabIndex = 1;
            this.lvM.UseCompatibleStateImageBehavior = false;
            this.lvM.View = System.Windows.Forms.View.Details;
            this.lvM.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvM_ColumnClick);
            this.lvM.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvM_ItemDrag);
            // 
            // chTTL
            // 
            this.chTTL.DisplayIndex = 1;
            this.chTTL.Text = "タイトル";
            this.chTTL.Width = 250;
            // 
            // chDate
            // 
            this.chDate.DisplayIndex = 0;
            this.chDate.Text = "日付";
            this.chDate.Width = 120;
            // 
            // chFrom
            // 
            this.chFrom.Text = "From";
            this.chFrom.Width = 100;
            // 
            // chUidl
            // 
            this.chUidl.Text = "POP3 Uidl";
            this.chUidl.Width = 130;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "メール一覧：";
            // 
            // ofdDb
            // 
            this.ofdDb.FileName = "C:\\Proj\\eseViewer\\eseSamp\\Mail.MSMessageStore";
            this.ofdDb.Filter = "Mail.MSMessageStore|Mail.MSMessageStore";
            // 
            // cmsMails
            // 
            this.cmsMails.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mFilterLost,
            this.mDeleteUidl,
            this.toolStripSeparator1,
            this.mSelDup});
            this.cmsMails.Name = "cmsMails";
            this.cmsMails.Size = new System.Drawing.Size(311, 98);
            // 
            // mFilterLost
            // 
            this.mFilterLost.Name = "mFilterLost";
            this.mFilterLost.Size = new System.Drawing.Size(310, 22);
            this.mFilterLost.Text = "一覧の中でから Stream 消失分を特定し、選択";
            this.mFilterLost.Click += new System.EventHandler(this.mFilterLost_Click);
            // 
            // chSt
            // 
            this.chSt.Text = "Stream";
            this.chSt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // mDeleteUidl
            // 
            this.mDeleteUidl.Name = "mDeleteUidl";
            this.mDeleteUidl.Size = new System.Drawing.Size(310, 22);
            this.mDeleteUidl.Text = "選択したメールの Uidl (受信履歴)を削除";
            this.mDeleteUidl.Click += new System.EventHandler(this.mDeleteUidl_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(307, 6);
            // 
            // mSelDup
            // 
            this.mSelDup.Name = "mSelDup";
            this.mSelDup.Size = new System.Drawing.Size(310, 22);
            this.mSelDup.Text = "重複メールを上から順番にサーチして選択";
            this.mSelDup.Click += new System.EventHandler(this.mSelDup_Click);
            // 
            // chId
            // 
            this.chId.Text = "MessageId";
            // 
            // WForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(993, 332);
            this.Controls.Add(this.vsc);
            this.Name = "WForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WLM Folder Mover";
            this.Load += new System.EventHandler(this.WForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WForm_FormClosing);
            this.vsc.Panel1.ResumeLayout(false);
            this.vsc.Panel1.PerformLayout();
            this.vsc.Panel2.ResumeLayout(false);
            this.vsc.Panel2.PerformLayout();
            this.vsc.ResumeLayout(false);
            this.cmsMails.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofdDb;
        private System.Windows.Forms.SplitContainer vsc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView tvF;
        private System.Windows.Forms.ImageList il16;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView lvM;
        private System.Windows.Forms.ColumnHeader chTTL;
        private System.Windows.Forms.ColumnHeader chDate;
        private System.Windows.Forms.ColumnHeader chFrom;
        private System.Windows.Forms.ColumnHeader chUidl;
        private System.Windows.Forms.ContextMenuStrip cmsMails;
        private System.Windows.Forms.ToolStripMenuItem mFilterLost;
        private System.Windows.Forms.ColumnHeader chSt;
        private System.Windows.Forms.ToolStripMenuItem mDeleteUidl;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mSelDup;
        private System.Windows.Forms.ColumnHeader chId;
    }
}

