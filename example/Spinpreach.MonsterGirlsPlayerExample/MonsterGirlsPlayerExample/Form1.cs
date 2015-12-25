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
using Spinpreach.MonsterGirlsPlayer;

namespace Spinpreach.MonsterGirlsPlayerExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.MonsterGirlsBrowser.LoginCompletedEvent += this.MonsterGirlsBrowser_LoginCompleted;
            this.MonsterGirlsBrowser.LoginErrorEvent += this.MonsterGirlsBrowser_LoginError;
            this.MonsterGirlsBrowser.MuteChangedEvent += (isMute) => this.Invoke(new Action<bool>(MonsterGirlsBrowser_MuteChanged), isMute);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            var login = LoginInfo.Load();
            this.MonsterGirlsBrowser.Start(login);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            var login = LoginInfo.Load();
            this.MonsterGirlsBrowser.Start(login);
        }

        private void ScreenShotButton_Click(object sender, EventArgs e)
        {
            this.ScreenShotButton.Enabled = false;

            string directory = string.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), "ScreenShot");
            if (!Directory.Exists(directory)) { Directory.CreateDirectory(directory); }
            string path = string.Format(@"{0}\{1}.{2}", directory, DateTime.Now.ToString("yyyyMMdd-HHmmss.fff"), "png");

            this.MonsterGirlsBrowser.ScreenShot(path);

            this.ScreenShotButton.Enabled = true;
        }

        private void ToggleMuteButton_Click(object sender, EventArgs e)
        {
            this.MonsterGirlsBrowser.ToggleMute();
        }

        private void MonsterGirlsBrowser_LoginCompleted()
        {
            var isMuted = this.MonsterGirlsBrowser.IsMute();
            if (isMuted == null) { return; }
            this.ToggleMuteButton.Text = (bool)isMuted ? "×" : "●";
        }

        private void MonsterGirlsBrowser_LoginError(Exception ex)
        {
            StringBuilder msg = new StringBuilder();
            msg.AppendLine("ログインに失敗しました。");
            msg.AppendLine("");
            msg.AppendLine("※サーバが重い可能性があります。");
            msg.AppendLine("　何度か[再読み込み]をためしてみてください。");
            MessageBox.Show(msg.ToString(), "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void MonsterGirlsBrowser_MuteChanged(bool isMuted)
        {
            this.ToggleMuteButton.Text = isMuted ? "×" : "●";
        }

    }
}