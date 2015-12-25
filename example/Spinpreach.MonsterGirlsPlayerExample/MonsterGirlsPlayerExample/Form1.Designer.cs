namespace Spinpreach.MonsterGirlsPlayerExample
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.MonsterGirlsBrowser = new Spinpreach.MonsterGirlsPlayer.MonsterGirlsBrowser();
            this.StartButton = new System.Windows.Forms.Button();
            this.ScreenShotButton = new System.Windows.Forms.Button();
            this.ToggleMuteButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MonsterGirlsBrowser
            // 
            this.MonsterGirlsBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MonsterGirlsBrowser.Location = new System.Drawing.Point(0, 0);
            this.MonsterGirlsBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.MonsterGirlsBrowser.Name = "MonsterGirlsBrowser";
            this.MonsterGirlsBrowser.Size = new System.Drawing.Size(592, 346);
            this.MonsterGirlsBrowser.TabIndex = 0;
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(12, 12);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(101, 43);
            this.StartButton.TabIndex = 1;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // ScreenShotButton
            // 
            this.ScreenShotButton.Location = new System.Drawing.Point(12, 61);
            this.ScreenShotButton.Name = "ScreenShotButton";
            this.ScreenShotButton.Size = new System.Drawing.Size(101, 43);
            this.ScreenShotButton.TabIndex = 2;
            this.ScreenShotButton.Text = "ScreenShot";
            this.ScreenShotButton.UseVisualStyleBackColor = true;
            this.ScreenShotButton.Click += new System.EventHandler(this.ScreenShotButton_Click);
            // 
            // ToggleMuteButton
            // 
            this.ToggleMuteButton.Location = new System.Drawing.Point(12, 110);
            this.ToggleMuteButton.Name = "ToggleMuteButton";
            this.ToggleMuteButton.Size = new System.Drawing.Size(101, 43);
            this.ToggleMuteButton.TabIndex = 3;
            this.ToggleMuteButton.Text = "ToggleMute";
            this.ToggleMuteButton.UseVisualStyleBackColor = true;
            this.ToggleMuteButton.Click += new System.EventHandler(this.ToggleMuteButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 346);
            this.Controls.Add(this.ToggleMuteButton);
            this.Controls.Add(this.ScreenShotButton);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.MonsterGirlsBrowser);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private MonsterGirlsPlayer.MonsterGirlsBrowser MonsterGirlsBrowser;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button ScreenShotButton;
        private System.Windows.Forms.Button ToggleMuteButton;
    }
}

