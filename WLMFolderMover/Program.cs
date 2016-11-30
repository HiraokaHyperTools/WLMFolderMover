using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Isam.Esent.Interop;
using System.IO;

namespace WLMFolderMover {
    static class Program {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(String[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Api.JetSetSystemParameter(JET_INSTANCE.Nil, JET_SESID.Nil, JET_param.DatabasePageSize, 8192, null);

            foreach (String a in args) {
                if (File.Exists(a)) {
                    Run(a);
                    return;
                }
            }

            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Mail.MSMessageStore|Mail.MSMessageStore";
                ofd.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\Windows Live Mail");
                if (ofd.ShowDialog() == DialogResult.OK) Run(ofd.FileName);
            }
        }

        private static void Run(string a) {
            bool isReadOnly = false;
            String fp = a;
            String dir = Path.GetDirectoryName(fp);
            using (Instance inst = new Instance("WLMFolderMover", "WLMFolderMover")) {
                inst.Parameters.LogFileDirectory = dir;
                inst.Parameters.TempDirectory = dir;
                inst.Parameters.SystemDirectory = dir;
                inst.Init();
                using (Session ses = new Session(inst)) {
                    JET_DBID dbid;
                    EUt.Check(Api.JetAttachDatabase(ses.JetSesid, fp, isReadOnly ? AttachDatabaseGrbit.ReadOnly : AttachDatabaseGrbit.None), "JetAttachDatabase");
                    EUt.Check(Api.JetOpenDatabase(ses.JetSesid, fp, null, out dbid, isReadOnly ? OpenDatabaseGrbit.ReadOnly : OpenDatabaseGrbit.None), "JetOpenDatabase");

                    try {
                        using (Table Folders = new Table(ses.JetSesid, dbid, "Folders", isReadOnly ? OpenTableGrbit.ReadOnly : OpenTableGrbit.None))
                        using (Table Messages = new Table(ses.JetSesid, dbid, "Messages", isReadOnly ? OpenTableGrbit.ReadOnly : OpenTableGrbit.None))
                        using (Table Streams = new Table(ses.JetSesid, dbid, "Streams", isReadOnly ? OpenTableGrbit.ReadOnly : OpenTableGrbit.None))
                        using (Table Uidl = new Table(ses.JetSesid, dbid, "Uidl", isReadOnly ? OpenTableGrbit.ReadOnly : OpenTableGrbit.None)) 
                        {
                            WForm form = new WForm();
                            form.inst = inst;
                            form.ses = ses;
                            form.Folders = Folders;
                            form.Messages = Messages;
                            form.Streams = Streams;
                            form.Uidl = Uidl;
                            Application.Run(form);
                        }
                    }
                    finally {
                        Api.JetCloseDatabase(ses.JetSesid, dbid, CloseDatabaseGrbit.None);
                    }
                }
            }
        }

        class EUt {
            internal static void Check(JET_wrn wrn, String message) {
                if (wrn == JET_wrn.Success) return;
                throw new ApplicationException(message + " " + wrn);
            }
        }
    }
}