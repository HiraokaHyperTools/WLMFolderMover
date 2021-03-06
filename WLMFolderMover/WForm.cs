using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Isam.Esent.Interop;
using System.IO;
using System.Diagnostics;

namespace WLMFolderMover {
    public partial class WForm : Form {
        public WForm() {
            InitializeComponent();
        }

        internal Instance inst;
        internal Session ses;
        internal Table Folders, Messages, Streams, Uidl;

        class AH : IDisposable {
            Cursor prev;

            public AH() { prev = Cursor.Current; Cursor.Current = Cursors.WaitCursor; }

            #region IDisposable メンバ

            public void Dispose() { Cursor.Current = prev; }

            #endregion
        }

        class UtBytea {
            internal static byte[] Cut(byte[] src, int x, int cx) {
                byte[] dst = new byte[cx];
                Buffer.BlockCopy(src, x, dst, 0, cx);
                return dst;
            }
        }

        private void WForm_FormClosing(object sender, FormClosingEventArgs e) {

        }

        class CCol {
            internal JET_COLUMNID FLDCOL_ID, FLDCOL_PARENT, FLDCOL_NAME, FLDCOL_FLAGS;

            internal JET_COLUMNID MSGCOL_FOLDERID, MSGCOL_NORMALSUBJ, MSGCOL_DATE, MSGCOL_EMAILFROM, MSGCOL_POP3UIDL, MSGCOL_STREAM, MSGCOL_MESSAGEID;

            internal JET_COLUMNID Streams_Id;

            internal JET_COLUMNID UIDLCOL_UIDL;
        }

        CCol C = new CCol();

        private void WForm_Load(object sender, EventArgs e) {
            Text += " " + Application.ProductVersion;

            C.FLDCOL_ID = Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_ID");
            C.FLDCOL_PARENT = Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_PARENT");
            C.FLDCOL_NAME = Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_NAME");
            C.FLDCOL_FLAGS = Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_FLAGS");

            C.MSGCOL_FOLDERID = Api.GetTableColumnid(ses.JetSesid, Messages.JetTableid, "MSGCOL_FOLDERID");
            C.MSGCOL_NORMALSUBJ = Api.GetTableColumnid(ses.JetSesid, Messages.JetTableid, "MSGCOL_NORMALSUBJ");
            C.MSGCOL_DATE = Api.GetTableColumnid(ses.JetSesid, Messages.JetTableid, "MSGCOL_DATE");
            C.MSGCOL_DATE = Api.GetTableColumnid(ses.JetSesid, Messages.JetTableid, "MSGCOL_DATE");
            C.MSGCOL_EMAILFROM = Api.GetTableColumnid(ses.JetSesid, Messages.JetTableid, "MSGCOL_EMAILFROM");
            C.MSGCOL_POP3UIDL = Api.GetTableColumnid(ses.JetSesid, Messages.JetTableid, "MSGCOL_POP3UIDL");
            C.MSGCOL_STREAM = Api.GetTableColumnid(ses.JetSesid, Messages.JetTableid, "MSGCOL_STREAM");
            C.MSGCOL_MESSAGEID = Api.GetTableColumnid(ses.JetSesid, Messages.JetTableid, "MSGCOL_MESSAGEID");

            C.Streams_Id = Api.GetTableColumnid(ses.JetSesid, Streams.JetTableid, "Id");

            C.UIDLCOL_UIDL = Api.GetTableColumnid(ses.JetSesid, Uidl.JetTableid, "UIDLCOL_UIDL");

            tvF.Nodes.Clear();
            Walk(-1, tvF.Nodes);
            tvF.ExpandAll();

            lvM.ListViewItemSorter = new Sort(chDate.Index, false);
        }

        private void Walk(Int64 parent, TreeNodeCollection tnc) {
            Trace.Assert(Api.TryMoveFirst(ses.JetSesid, Folders.JetTableid));

            for (int y = 0; ; y++) {
                Int64 FLDCOL_ID = Api.RetrieveColumnAsInt64(ses.JetSesid, Folders.JetTableid, C.FLDCOL_ID).Value;
                Int64 FLDCOL_PARENT = Api.RetrieveColumnAsInt64(ses.JetSesid, Folders.JetTableid, C.FLDCOL_PARENT).Value;
                String FLDCOL_NAME = Api.RetrieveColumnAsString(ses.JetSesid, Folders.JetTableid, C.FLDCOL_NAME, Encoding.Default).TrimEnd('\0');
                Int32 FLDCOL_FLAGS = Api.RetrieveColumnAsInt32(ses.JetSesid, Folders.JetTableid, C.FLDCOL_FLAGS).Value;

                if (FLDCOL_PARENT == parent) {
                    byte[] bookmark = Api.GetBookmark(ses.JetSesid, Folders.JetTableid);

                    TreeNode tn = tnc.Add(FLDCOL_NAME);
                    tn.Tag = bookmark;
                    if (0 != (FLDCOL_FLAGS & 0x800000)) tn.ImageKey = tn.SelectedImageKey = "8";
                    Walk(FLDCOL_ID, tn.Nodes);

                    Api.JetGotoBookmark(ses.JetSesid, Folders.JetTableid, bookmark, bookmark.Length);
                }

                if (Api.TryMoveNext(ses.JetSesid, Folders.JetTableid))
                    continue;
                break;
            }
        }

        private void tvF_ItemDrag(object sender, ItemDragEventArgs e) {
            tvF.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void tvF_DragEnter(object sender, DragEventArgs e) {
            e.Effect = (
                e.Data.GetDataPresent(typeof(TreeNode)) || e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection))
                ) ? DragDropEffects.All : DragDropEffects.None;
        }

        private void tvF_DragDrop(object sender, DragEventArgs e) {
            TreeNode tnTo = tvF.GetNodeAt(tvF.PointToClient(new Point(e.X, e.Y)));
            TreeNode tnsrc = (TreeNode)e.Data.GetData(typeof(TreeNode));

            ListView.SelectedListViewItemCollection lvsel = (ListView.SelectedListViewItemCollection)e.Data.GetData(typeof(ListView.SelectedListViewItemCollection));
            if (lvsel != null) {
                byte[] bin;

                bin = (byte[])tnTo.Tag;
                Api.JetGotoBookmark(ses.JetSesid, Folders.JetTableid, bin, bin.Length);
                Int64 FLDCOL_IDto = Api.RetrieveColumnAsInt64(ses.JetSesid, Folders.JetTableid, C.FLDCOL_ID).Value;

                foreach (ListViewItem lvi in lvsel) {
                    byte[] bookmarkMsg = (byte[])lvi.Tag;
                    Api.JetGotoBookmark(ses.JetSesid, Messages.JetTableid, bookmarkMsg, bookmarkMsg.Length);

                    Api.JetPrepareUpdate(ses.JetSesid, Messages.JetTableid, JET_prep.InsertCopyDeleteOriginal);
                    Api.SetColumn(ses.JetSesid, Messages.JetTableid, C.MSGCOL_FOLDERID, FLDCOL_IDto);
                    Api.JetUpdate(ses.JetSesid, Messages.JetTableid);
                }

                tvF_AfterSelect(sender, new TreeViewEventArgs(tvF.SelectedNode));
                return;
            }
            else if (tnsrc != null) {
                TreeNode tnt = tnTo;
                while (tnt != null) {
                    if (tnt == tnsrc) return;
                    tnt = tnt.Parent;
                }

                {
                    byte[] bin;

                    bin = (byte[])tnTo.Tag;
                    Api.JetGotoBookmark(ses.JetSesid, Folders.JetTableid, bin, bin.Length);
                    Int64 FLDCOL_IDto = Api.RetrieveColumnAsInt64(ses.JetSesid, Folders.JetTableid, C.FLDCOL_ID).Value;

                    bin = (byte[])tnsrc.Tag;
                    Api.JetGotoBookmark(ses.JetSesid, Folders.JetTableid, bin, bin.Length);
                    Api.JetPrepareUpdate(ses.JetSesid, Folders.JetTableid, JET_prep.Replace);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, C.FLDCOL_PARENT, FLDCOL_IDto);
                    Api.JetUpdate(ses.JetSesid, Folders.JetTableid);
                }

                tvF.Nodes.Clear();
                Walk(-1, tvF.Nodes);
                tvF.ExpandAll();
            }
        }

        private void tvF_AfterSelect(object sender, TreeViewEventArgs e) {
            if (e.Node == null) return;

            byte[] bin;

            bin = (byte[])e.Node.Tag;
            Api.JetGotoBookmark(ses.JetSesid, Folders.JetTableid, bin, bin.Length);
            Int64 FLDCOL_ID = Api.RetrieveColumnAsInt64(ses.JetSesid, Folders.JetTableid, C.FLDCOL_ID).Value;

            lvM.Items.Clear();

            Trace.Assert(Api.TryMoveFirst(ses.JetSesid, Messages.JetTableid));

            lvM.Hide();
            try {
                while (true) {
                    Int64? MSGCOL_FOLDERID = Api.RetrieveColumnAsInt64(ses.JetSesid, Messages.JetTableid, C.MSGCOL_FOLDERID);
                    if (MSGCOL_FOLDERID.HasValue && MSGCOL_FOLDERID == FLDCOL_ID) {
                        Int64 MSGCOL_DATE = Api.RetrieveColumnAsInt64(ses.JetSesid, Messages.JetTableid, C.MSGCOL_DATE).Value;
                        DateTime dt = new DateTime(MSGCOL_DATE).AddYears(1600).ToLocalTime();
                        String MSGCOL_NORMALSUBJ = Api.RetrieveColumnAsString(ses.JetSesid, Messages.JetTableid, C.MSGCOL_NORMALSUBJ);
                        String MSGCOL_EMAILFROM = Api.RetrieveColumnAsString(ses.JetSesid, Messages.JetTableid, C.MSGCOL_EMAILFROM, Encoding.Default);
                        String MSGCOL_POP3UIDL = Api.RetrieveColumnAsString(ses.JetSesid, Messages.JetTableid, C.MSGCOL_POP3UIDL, Encoding.Unicode);
                        UInt32 MSGCOL_STREAM = Api.RetrieveColumnAsUInt32(ses.JetSesid, Messages.JetTableid, C.MSGCOL_STREAM) ?? 0U;
                        String MSGCOL_MESSAGEID = Api.RetrieveColumnAsString(ses.JetSesid, Messages.JetTableid, C.MSGCOL_MESSAGEID, Encoding.GetEncoding("latin1"));

                        byte[] bookmark = Api.GetBookmark(ses.JetSesid, Messages.JetTableid);

                        ListViewItem lvi = new ListViewItem(MSGCOL_NORMALSUBJ);
                        lvi.ImageKey = "M";
                        lvi.SubItems.Add(dt.ToString("yyyy/MM/dd HH:mm:ss"));
                        lvi.SubItems.Add(MSGCOL_EMAILFROM);
                        lvi.SubItems.Add(MSGCOL_POP3UIDL);//SubItems[3]
                        lvi.SubItems.Add(MSGCOL_STREAM + "");//SubItems[4]
                        lvi.SubItems.Add(MSGCOL_MESSAGEID);//SubItems[5]
                        lvi.Tag = bookmark;
                        lvM.Items.Add(lvi);
                    }

                    if (Api.TryMoveNext(ses.JetSesid, Messages.JetTableid))
                        continue;
                    break;
                }
            }
            finally {
                lvM.Show();
            }
        }

        class Sort : System.Collections.IComparer {
            public int i;
            public int ord;

            public Sort(int i, bool asc) {
                this.i = i;
                this.ord = asc ? 1 : -1;
            }

            #region IComparer メンバ

            public int Compare(object vx, object vy) {
                ListViewItem x = (ListViewItem)vx;
                ListViewItem y = (ListViewItem)vy;
                return ord * x.SubItems[i].Text.CompareTo(y.SubItems[i].Text);
            }

            #endregion
        }

        private void lvM_ColumnClick(object sender, ColumnClickEventArgs e) {
            lvM.ListViewItemSorter = new Sort(e.Column, 0 == (ModifierKeys & Keys.Shift));
        }

        private void lvM_ItemDrag(object sender, ItemDragEventArgs e) {
            lvM.DoDragDrop(lvM.SelectedItems, DragDropEffects.Move);
        }

        private void mFilterLost_Click(object sender, EventArgs e) {
            Api.JetSetCurrentIndex(ses.JetSesid, Streams.JetTableid, "Id");
            foreach (ListViewItem lvi in lvM.Items) {
                uint Id;
                if (UInt32.TryParse(lvi.SubItems[4].Text, out Id)) {
                    bool lost = true;
                    Api.MakeKey(ses.JetSesid, Streams.JetTableid, Id, MakeKeyGrbit.NewKey);
                    if (Api.TrySeek(ses.JetSesid, Streams.JetTableid, SeekGrbit.SeekEQ)) {
                        UInt32 RealId = Api.RetrieveColumnAsUInt32(ses.JetSesid, Streams.JetTableid, C.Streams_Id) ?? 0U;
                        if (RealId == Id) {
                            lost = false;
                        }
                    }
                    lvi.Selected = lost;
                }
            }
            MessageBox.Show(this, lvM.SelectedItems.Count.ToString("#,##0") + " 件、選択しました。", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void mDeleteUidl_Click(object sender, EventArgs e) {
            if (MessageBox.Show(this, lvM.SelectedItems.Count.ToString("#,##0") + " 件、削除します。", Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
                return;

            Api.JetSetCurrentIndex(ses.JetSesid, Uidl.JetTableid, "0");
            foreach (ListViewItem lvi in lvM.SelectedItems) {
                String FindUidl = lvi.SubItems[3].Text;
                Api.MakeKey(ses.JetSesid, Uidl.JetTableid, FindUidl, Encoding.Unicode, MakeKeyGrbit.NewKey);
                if (Api.TrySeek(ses.JetSesid, Uidl.JetTableid, SeekGrbit.SeekGE)) {
                    String RealUidl = Api.RetrieveColumnAsString(ses.JetSesid, Uidl.JetTableid, C.UIDLCOL_UIDL, Encoding.Unicode);
                    if (FindUidl == RealUidl) {
                        Api.JetDelete(ses.JetSesid, Uidl.JetTableid);
                    }
                }
            }
        }

        private void mSelDup_Click(object sender, EventArgs e) {
            SortedDictionary<string, string> messageIds = new SortedDictionary<string, string>();
            foreach (ListViewItem lvi in lvM.Items) {
                String messageId = lvi.SubItems[5].Text;
                if (messageIds.ContainsKey(messageId)) {
                    lvi.Selected = true;
                }
                else {
                    messageIds[messageId] = null;
                    lvi.Selected = false;
                }
            }
            MessageBox.Show(this, lvM.SelectedItems.Count.ToString("#,##0") + " 件、選択しました。", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}