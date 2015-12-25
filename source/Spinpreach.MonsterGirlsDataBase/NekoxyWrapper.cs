using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nekoxy;


namespace Spinpreach.MonsterGirlsDataBase
{
    public class NekoxyWrapper : IDisposable
    {
        public Action<string, string, string> SessionComplete;
        private event Action<Session> action;

        public NekoxyWrapper(int ProxyPort)
        {
            this.action = (s) => Task.Run(() => this.Nekoxy_SessionComplete(s));
            HttpProxy.AfterSessionComplete += action;
            HttpProxy.Startup(ProxyPort);
        }

        public void Dispose()
        {
            HttpProxy.Shutdown();
            HttpProxy.AfterSessionComplete -= this.action;
        }

        private void Nekoxy_SessionComplete(Session s)
        {

            //if (!s.Response.ContentType.Equals("text/html")) return;
            //if (!s.Response.BodyAsString.StartsWith(@"{")) return;

            //var api = s.Request.PathAndQuery;
            //var request = s.Request.BodyAsString;
            //var response = s.Response.BodyAsString;

            //Console.WriteLine(api);
            //Console.WriteLine(request);
            //Console.WriteLine(response);

            //SessionComplete?.Invoke(api, request, response);

        }
    }
}