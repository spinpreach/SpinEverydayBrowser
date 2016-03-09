using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

using Spinpreach.MonsterGirlsDataBase;
using Spinpreach.MonsterGirlsPlayer;
//using Spinpreach.SpinEverydayBrowser.Timers;

namespace Spinpreach.SpinEverydayBrowser
{
    public partial class MainForm : Form
    {

        private MonsterGirlsData database;
        
        private Action LoginCompletedAction;
        private Action<Exception> LoginErrorAction;
        private Action<bool> MuteChangedAction;
        private Action _login_start;
        private Action missionAction;
        private Action resourceAction;

        //private MissionTimer missionTimer;
        //private ResourceTimer resourceTimer;

        public MainForm(MonsterGirlsData database)
        {
            InitializeComponent();
            this.database = database;
            //this.missionTimer = new MissionTimer(this.database);
            //this.resourceTimer = new ResourceTimer(this.database);
            this.LoginCompletedAction = () => this.Invoke(new Action(this.MonsterGirlsBrowser_LoginCompleted));
            this.LoginErrorAction = (e) => this.Invoke(new Action<Exception>(this.MonsterGirlsBrowser_LoginError), e);
            this.MuteChangedAction = (mute) => this.Invoke(new Action<bool>(this.MonsterGirlsBrowser_MuteChanged), mute);
            this._login_start = () => this.Invoke(new Action(this.apiNotify_login_start));
            this.missionAction = () => { try { this.Invoke(new Action(this.missionTimer_Notify)); } catch (Exception) { } };
            this.resourceAction = () => { try { this.Invoke(new Action(this.resourceTimer_Notify)); } catch (Exception) { } };
            Common.ShapeMemory.Load(this);
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = string.Format("回転日常 ver {0}.{1}.{2}", version.Major, version.Minor, version.Build);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.MonsterGirlsBrowser.LoginCompletedEvent += this.LoginCompletedAction;
            this.MonsterGirlsBrowser.LoginErrorEvent += this.LoginErrorAction;

            this.MonsterGirlsBrowser.Start();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.MonsterGirlsBrowser.MuteChangedEvent -= this.MuteChangedAction;
            //this.missionTimer.Notify = null;
            //this.resourceTimer.Notify = null;
            Common.ShapeMemory.Save(this);
        }

        #region MonsterGirlsBrowser

        private void MonsterGirlsBrowser_LoginCompleted()
        {
            this.MonsterGirlsBrowser.LoginCompletedEvent -= this.LoginCompletedAction;
            this.MonsterGirlsBrowser.LoginErrorEvent -= this.LoginErrorAction;
            this.MonsterGirlsBrowser.MuteChangedEvent += this.MuteChangedAction;
            //this.database.apiNotify._login_start += this._login_start;

            var mute = this.MonsterGirlsBrowser.IsMute();
            if (mute != null)
            {
                this.MonsterGirlsBrowser_MuteChanged((bool)mute);
            }
        }

        private void MonsterGirlsBrowser_LoginError(Exception ex)
        {
            this.MonsterGirlsBrowser.LoginCompletedEvent -= this.LoginCompletedAction;
            this.MonsterGirlsBrowser.LoginErrorEvent -= this.LoginErrorAction;

            StringBuilder msg = new StringBuilder();
            msg.AppendLine("ログインに失敗しました。");
            msg.AppendLine("");
            msg.AppendLine("※サーバが重い可能性があります。");
            msg.AppendLine("　何度か[再読み込み]をためしてみてください。");
            MessageBox.Show(msg.ToString(), "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void MonsterGirlsBrowser_MuteChanged(bool mute)
        {
            switch (mute)
            {
                case true: this.MuteButton.Image = Properties.Resources.MuteonImage; break;
                case false: this.MuteButton.Image = Properties.Resources.MuteoffImage; break;
            }
        }

        #endregion

        #region MenuBar

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            string message = "再読み込みしてもいいですか？";
            string title = string.Empty;
            if (MessageBox.Show(this, message, title, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                this.MonsterGirlsBrowser.LoginCompletedEvent += this.MonsterGirlsBrowser_LoginCompleted;
                this.MonsterGirlsBrowser.LoginErrorEvent += this.MonsterGirlsBrowser_LoginError;
                this.MonsterGirlsBrowser.MuteChangedEvent -= this.MuteChangedAction;
                //this.missionTimer.Notify -= this.missionAction;
                //this.resourceTimer.Notify -= this.resourceAction;
                this.MonsterGirlsBrowser.Start();
            }
        }

        private void ScreenShotButton_Click(object sender, EventArgs e)
        {
            ((ToolStripButton)sender).Enabled = false;

            string directory = string.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), "ScreenShot");
            if (!Directory.Exists(directory)) { Directory.CreateDirectory(directory); }
            string path = string.Format(@"{0}\{1}.{2}", directory, DateTime.Now.ToString("yyyyMMdd-HHmmss.fff"), "png");
            this.MonsterGirlsBrowser.ScreenShot(path);

            ((ToolStripButton)sender).Enabled = true;
        }

        private void MuteButton_Click(object sender, EventArgs e)
        {
            ((ToolStripButton)sender).Enabled = false;

            this.MonsterGirlsBrowser.ToggleMute();

            ((ToolStripButton)sender).Enabled = true;
        }

        private void SettingButton_Click(object sender, EventArgs e)
        {
            ((ToolStripButton)sender).Enabled = false;

            var frm = new LoginForm();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoginInfo.Save(frm.LoginData);
                string message = "入力されたアカウントは次回起動時から使用されます。";
                string title = string.Empty;
                MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ((ToolStripButton)sender).Enabled = true;
        }

        #region SubForms

        private static Form OpenForms(Type type)
        {
            Form form = null;
            foreach (Form item in Application.OpenForms)
            {
                if (item.GetType() == type)
                {
                    form = item;
                }
            }
            return form;
        }

        #endregion

        #endregion

        #region Timer

        private void missionTimer_Notify()
        {
            //StringBuilder msg = new StringBuilder();
            //var q1 = this.database.table.transaction.party.Rows.Where(x => x.status == 2);
            //q1.ToList().ForEach(x => { msg.AppendLine(x.name); });

            //this.MissionNotify.BalloonTipIcon = ToolTipIcon.Info;
            //this.MissionNotify.BalloonTipTitle = "【 遠征帰還通知 】";
            //this.MissionNotify.BalloonTipText = msg.ToString();
            //this.MissionNotify.Visible = true;
            //this.MissionNotify.ShowBalloonTip(2000);
        }

        private void resourceTimer_Notify()
        {
            //if (string.Empty.Equals(this.database.table.transaction.castlekeep.name)) return;
            //using (var table = new SQLiteHelper.Resource(this.database.table.transaction.castlekeep.name))
            //{
            //    var row = new SQLiteHelper.Resource.Row();
            //    row.date = DateTime.Now;
            //    row.bill = this.database.table.transaction.resource.bill;
            //    row.charcoal = this.database.table.transaction.resource.charcoal;
            //    row.steel = this.database.table.transaction.resource.steel;
            //    row.coolant = this.database.table.transaction.resource.coolant;
            //    row.file = this.database.table.transaction.resource.file;
            //    row.datetime = DateTime.Now;
            //    table.Merge(row);
            //}
        }

        #endregion

        #region apiNotify

        private void apiNotify_login_start()
        {
            //this.database.apiNotify._login_start -= this._login_start;
            //this.missionTimer.Notify += this.missionAction;
            //this.resourceTimer.Notify += this.resourceAction;
        }

        #endregion

    }
}