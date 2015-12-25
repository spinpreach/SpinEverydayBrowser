using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spinpreach.MonsterGirlsPlayer;

namespace Spinpreach.MonsterGirlsPlayerExample
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var login = LoginInfo.Load();
            if (login.IsExists())
            {
                Application.Run(new Form1());
            }
        }
    }
}