using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Spinpreach.MonsterGirlsDataBase.Apis;
using Spinpreach.MonsterGirlsDataBase.Tables;

namespace Spinpreach.MonsterGirlsDataBase
{
    public class MonsterGirlsData
    {

        private NekoxyWrapper nw;

        public Api api = new Api();
        public Table table = new Table();
        public ApiNotify apiNotify = new ApiNotify();
        public TableNotify tableNotify = new TableNotify();

        public DateTime ServerTime
        {
            get
            {
                return DateTime.Now.AddTicks(this.table.ts.Ticks);
            }
        }

        public MonsterGirlsData(NekoxyWrapper nw)
        {
            this.nw = nw;
            this.nw.SessionComplete += this.nw_Log;
            this.nw.SessionComplete += this.nw_SessionComplete;
        }

        private void nw_Log(string api, string request, string response)
        {
            try
            {
                var time = DateTime.Now;
                SessionWriter.JsonWriterTransaction(api, response);
                SessionWriter.JsonWriterHistory(api, response, time);
                SessionWriter.XmlWriterTransaction(api, response);
                SessionWriter.XmlWriterHistory(api, response, time);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception : {0}.{1} >> {2}", ex.TargetSite.ReflectedType.FullName, ex.TargetSite.Name, ex.Message));
            }
        }

        private void nw_SessionComplete(string api, string request, string response)
        {

            Console.WriteLine(api);

            switch (api)
            {
                //case "/login/start": (new Querys(this))._login_start(request, response); break;
                //case "/home": (new Querys(this))._home(request, response); break;
                //case "/conquest/complete": (new Querys(this))._conquest_complete(request, response); break;
                //case "/conquest/start": (new Querys(this))._conquest_start(request, response); break;
                //case "/conquest/cancel": (new Querys(this))._conquest_cancel(request, response); break;
                default: break;
            }
        }
    }
}